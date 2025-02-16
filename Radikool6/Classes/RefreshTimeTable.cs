using Microsoft.Data.Sqlite;
using Quartz;
using Radikool6.Models;
using System.Diagnostics;
using Radikool6.Radio;

namespace Radikool6.Classes
{
    public class RefreshTimeTable : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            using SqliteConnection con = new ($"Data Source={Define.File.DbFile}");
            var sw = new Stopwatch();
            sw.Start();
            con.Open();
            var sModel = new StationModel(con);
            var pModel = new ProgramModel(con);
            foreach (var station in sModel.Get(Define.Radiko.TypeName))
            {
                Global.Logger.Info($"{station.Name}更新");
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
            sw.Stop();
            Global.Logger.Info($"番組表全更新:{sw.Elapsed}");
            
            return Task.CompletedTask;
        }
    }
}
