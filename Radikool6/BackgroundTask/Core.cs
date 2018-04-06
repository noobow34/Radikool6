﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Radikool6.Entities;
using Radikool6.Models;

namespace Radikool6.BackgroundTask
{
    public class Core
    {
        private readonly Timer _timer;
       // private readonly List<Recorder> _recorders = new List<Recorder>();
        private readonly List<RadikoRecorder> _recorders = new List<RadikoRecorder>();
        public Core()
        {
            Init();

            _timer = new Timer(1000);
            _timer.Elapsed += this.TimerElapsed;

        }

        public void Run()
        {
            _timer.Start();
        }

        /// <summary>
        /// 起動時の処理
        /// </summary>
        private static void Init()
        {
            using (var db = new Db())
            {
                var model = new ReserveModel(db);
                model.RefreshTasks();
            }
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            using (var db = new Db())
            {
                var rModel = new ReserveModel(db);
                var tasks = rModel.GetTasks(true);
                if (!tasks.Any()) return;
                var cModel = new ConfigModel(db);
                var config = cModel.Get();
                tasks.ForEach(t =>
                {
                    // 予約実行
                    if (_recorders.All(r => r.Id != t.Id))
                    {
                    //    var recorder = Recorder.GetRecorder(config, t);
                        var recorder = new RadikoRecorder(config, t);
                        var task = recorder.Start();
                        task.Wait();
                        _recorders.Add(recorder);
                        
                        var logger = NLog.LogManager.GetCurrentClassLogger(); 
                        logger.Info("録音開始");
                    }
                });

            }
        }
    }
}