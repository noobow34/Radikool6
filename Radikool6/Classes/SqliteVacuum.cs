using Dapper;
using Microsoft.Data.Sqlite;
using Quartz;
using System.Data;
using System.Diagnostics;

namespace Radikool6.Classes
{
    public class SqliteVacuum : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            using SqliteConnection con = new ($"Data Source={Define.File.DbFile}");
            con.Open();
            Stopwatch sw = new();
            sw.Start();
            await con.ExecuteAsync("VACUUM;", commandType: CommandType.Text);
            sw.Stop();
            Global.Logger.Info($"VACUUM:{sw.Elapsed}");
        }
    }
}
