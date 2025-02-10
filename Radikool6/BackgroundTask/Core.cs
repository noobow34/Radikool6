﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Microsoft.Data.Sqlite;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;
using Radikool6.Radio;

namespace Radikool6.BackgroundTask
{
    public class Core
    {
        private readonly Timer _timer;
        private bool _recorderLock = false;
        private bool _timetableLock = false;
        
        private readonly List<RadikoRecorder> _recorders = [];
        private DateTime _refreshTimetableDate = DateTime.MinValue;
        
        public Core()
        {
            Init();

            _timer = new Timer(60_000);
            _timer.Elapsed += this.TimerElapsed;
        }

        public void Run()
        {
            _timer.Start();
        }

        /// <summary>
        /// 録音ステータス取得
        /// </summary>
        /// <returns></returns>
        public List<ReserveTask> GetStatus()
        {
            return _recorders.Where(r => r.Status != Recorder.RecorderStatus.End).Select(r => r.GetStatus()).ToList();
        }

        /// <summary>
        /// 停止／再開
        /// </summary>
        /// <param name="taskId"></param>
        public void StopRestart(string taskId)
        {
            _recorders.FirstOrDefault(r => r.Id == taskId)?.StopRestart();
        }

        /// <summary>
        /// 起動時の処理
        /// </summary>
        private static void Init()
        {
            using var con = new SqliteConnection($"Data Source={Define.File.DbFile}");
            con.Open();
            var cModel = new ConfigModel(con);
            var config = cModel.Get();
            var model = new ReserveModel(con);
            model.RefreshTasks(config);
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_recorderLock)
            {
                _recorderLock = true;
                using (var con = new SqliteConnection($"Data Source={Define.File.DbFile}"))
                {
                    con.Open();
                    var cModel = new ConfigModel(con);
                    var config = cModel.Get();

                    var rModel = new ReserveModel(con);
                    var tasks = rModel.GetTasks(true);
                    if (tasks.Count != 0)
                    {

                        tasks.ForEach(t =>
                        {
                            // 予約実行
                            if (_recorders.All(r => r.Id != t.Id))
                            {
                                var recorder = new RadikoRecorder(config, t);
                                _recorders.Add(recorder);
                                recorder.Start().Wait();
                                var logger = NLog.LogManager.GetCurrentClassLogger();
                                logger.Info("録音開始");
                            }
                        });
                    }
                }
                
                // 終了タスクを削除する
                _recorders.RemoveAll(r => r.Status == Recorder.RecorderStatus.End);

                _recorderLock = false;
            }

            if (!_timetableLock && (DateTime.Now.Date != _refreshTimetableDate.Date))
            {
                _timetableLock = true;
                RefreshTimeTable();
            }
        }

        /// <summary>
        /// 全番組表再取得
        /// </summary>
        private void RefreshTimeTable()
        {
            using var con = new SqliteConnection($"Data Source={Define.File.DbFile}");
            var sw = new Stopwatch();
            sw.Start();
            con.Open();
            var sModel = new StationModel(con);
            var pModel = new ProgramModel(con);
            foreach (var station in sModel.Get(Define.Radiko.TypeName))
            {
                try
                {
                    var programs = Radiko.GetPrograms(station).Result;
                    pModel.Refresh(programs);
                }
                catch (Exception e)
                {
                    Global.Logger.Error($"{e.Message}¥r¥n{e.StackTrace}");
                }
            }

            _refreshTimetableDate = DateTime.Now;
            _timetableLock = false;
            sw.Stop();
            Global.Logger.Info($"番組表全更新:{sw.Elapsed}");
        }
    }
}