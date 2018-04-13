using System;
using System.Diagnostics;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.BackgroundTask
{
    public class Recorder
    {
        public string Id { get; set; }
        protected ReserveTask Task { get; set; }
        protected CommonConfig Config { get; set; }
        protected DateTime StartTime { get; set; }
        
        public static Recorder GetRecorder(CommonConfig config, ReserveTask task)
        {
            Recorder res = null;
            switch (task.Station.Type)
            {
                case Define.Radiko.TypeName:
                    res = new RadikoRecorder(config, task);
                    break;

            }

            return res;
        }

        protected Recorder(CommonConfig config, ReserveTask task = null)
        {
            Config = config;
            Task = task;
            Id = task?.Id;
        }

    }
}