using System;
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
        private readonly List<Recorder> _recorders = new List<Recorder>();


        public Core()
        {
            Init();

            _timer = new Timer(1000);
            _timer.Elapsed += this.TimerElapsed;

        }

        public void Run()
        {
         //   var t = new RadikoRecorder(new ReserveTask(){ Station = new Station(){ Code = "ABC"}});
       //     _timer.Start();
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
                var tasks = rModel.GetTasks();
                if (!tasks.Any()) return;
                var cModel = new ConfigModel(db);
                var config = cModel.Get();
                tasks.ForEach(t =>
                {
                    // 予約実行
                    if (_recorders.All(r => r.Id != t.Id))
                    {
                        _recorders.Add(Recorder.GetRecorder(config, t));
                    }
                });

            }
        }
    }
}