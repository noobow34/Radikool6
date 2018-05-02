using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;
using Radikool6.Radio;

namespace Radikool6.BackgroundTask
{
    public class RadikoRecorder : Recorder, IRecorder
    {
        private string _token;
        private Process _ffmpeg;
        private string _filename;
        private Entities.Program _program = new Entities.Program();

        public RadikoRecorder(Config config, ReserveTask task = null) : base(config, task)
        {
        //    Start();
        }

        public async Task TimeFree(Entities.Program program)
        {
            Status = RecorderStatus.Working;
            await Radiko.Login(Config.RadikoEmail, Config.RadikoPassword);
            
            
            _program = program;
            Directory.CreateDirectory("records");
            _filename = Path.GetFullPath(Path.Combine("records", $"{Guid.NewGuid().ToString()}.m4a"));
            StartTime = DateTime.Now;
            var m3U8 = await Radiko.GetTimeFreeM3U8(program);
            
            var arg = Define.Radiko.TimeFreeFfmpegArgs.Replace("[M3U8]", m3U8).Replace("[FILE]", _filename);
            arg = Replace(arg, Task?.Station ?? _program.Station, _program);
            CreateProcess(arg);

        //    await System.Threading.Tasks.Task.Factory.StartNew(() =>
        //    {
                _ffmpeg.Start();

                _ffmpeg.BeginOutputReadLine();
                _ffmpeg.BeginErrorReadLine();
         //   });

        }

        public ReserveTask GetStatus()
        {
            var statusText = (DateTime.Now - StartTime).ToString(@"hh\:mm\:ss");
            
            // ffmpegの状態確認
            if (_ffmpeg.HasExited)
            {
                if (Status == RecorderStatus.Stopping)
                {
                    Status = RecorderStatus.Stopped;
                }
                else
                {
                    Status = DateTime.Now < Task.End ? RecorderStatus.Error : RecorderStatus.End;
                }
            }

            switch (Status)
            {
                case RecorderStatus.Stopped:
                    statusText = "停止中";
                    break;

                case RecorderStatus.Stopping:
                    statusText = "停止処理中";
                    break;
                
                case RecorderStatus.Error:
                    statusText = "エラー";
                    break;
                
                case RecorderStatus.End:
                    statusText = "完了";
                    break;
            }

            return new ReserveTask(){ Id = Id, Start = StartTime, End = Task.End, Status = statusText};
        }

        /// <summary>
        /// 停止／再開
        /// </summary>
        public void StopRestart()
        {
            if (Task.Reserve.IsTimeFree) return;
            if (Status == RecorderStatus.Stopped)
            {
                // 再開
                Start();
            }
            else if(Status == RecorderStatus.Working)
            {
                // 停止
                Stop();
            }
        }

        /// <summary>
        /// ffmpegのプロセス作成
        /// </summary>
        /// <param name="arg"></param>
        private void CreateProcess(string arg)
        {
            _ffmpeg = new Process
            {
                StartInfo =
                {
                    FileName = "ffmpeg",
                    Arguments = arg,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };

            
            _ffmpeg.OutputDataReceived += process_OutputDataReceived;
            _ffmpeg.ErrorDataReceived += process_OutputDataReceived;
        //    _ffmpeg.Exited += process_Exited;
            
            Global.Logger.Info($"ffmpeg起動:{arg}");

            _ffmpeg.Exited += (sender, args) =>
            {
                Global.Logger.Info($"タイムフリー録音終了");
            };
        }



        /// <summary>
        /// 録音開始
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            try
            {
                Status = RecorderStatus.Working;

                
                
                
                using (var con = new SqliteConnection($"Data Source={Define.File.DbFile}"))
                {
                    var pModel = new ProgramModel(con);
                    _program = pModel.Search(new ProgramSearchCondition() { StationId = Task.Station.Id, From = Task.Reserve.Start, To = Task.Reserve.End}).FirstOrDefault();
                }

                if (Task.Reserve.IsTimeFree)
                {
                    // 番組情報取得
                    using (var con = new SqliteConnection($"Data Source={Define.File.DbFile}"))      
                    {
                        _program.Station = Task.Station;
                        TimeFree(_program);
                    }

                }
                else
                {              
                    await Radiko.Login(Config.RadikoEmail, Config.RadikoPassword);
                    Directory.CreateDirectory("records");
                    _filename = Path.GetFullPath(Path.Combine("records", $"{Guid.NewGuid().ToString()}.m4a"));
                    StartTime = DateTime.Now;
                    var t = Task.End - Task.Start;
                    _token = await Radio.Radiko.GetAuthToken();
                    var arg = Define.Radiko.RealTimeFfmpegArgs.Replace("[TOKEN]", _token)
                        .Replace("[TIME]", (Task.End.AddSeconds(Define.Radiko.EndSec) - DateTime.Now).ToString(@"hh\:mm\:ss"))
                        .Replace("[FILE]", _filename);
                    arg = Replace(arg, Task.Station, _program);
                    CreateProcess(arg);

                    _ffmpeg.Start();
                    _ffmpeg.BeginOutputReadLine();
                    _ffmpeg.BeginErrorReadLine();

                    Global.Logger.Info($"ffmpeg起動:{arg}");

                    _ffmpeg.Exited += (sender, args) =>
                    {
                        Global.Logger.Info($"録音終了");
                        
                    };
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
         }

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            Status = RecorderStatus.Stopping;
            _ffmpeg.Kill();            
        }


        void process_Exited(object sender, System.EventArgs e)
        {
            using (var con = new SqliteConnection($"Data Source={Define.File.DbFile}"))
            {
                con.Open();
                var lModel = new LibraryModel(con);
                lModel.Update(new Library() { Id = Guid.NewGuid().ToString(), FileName = _filename, Path = _filename, Program = _program });

            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                this.process_Exited(sender, e);
            }
            else
            {

                var a = System.Text.Encoding.UTF8.GetBytes(e.Data);
            }

        }


    }
}