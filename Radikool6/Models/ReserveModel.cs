using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;
using Newtonsoft.Json;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ReserveModel : BaseModel
    {
        public ReserveModel(SqliteConnection con) : base(con)
        {
        }

        /// <summary>
        /// 予約取得
        /// </summary>
        /// <returns></returns>
        public List<Reserve> Get()
        {
            var res = SqliteConnection.Query<Reserve>("SELECT * FROM Reserves").ToList();

            if (!res.Any()) return res;

            var stationIds = res.Select(r => r.StationId).Distinct().ToList();
            var stations = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id IN @StationIds",
                new {StationIds = stationIds}).ToList();

            res.ForEach(r => { r.StationName = stations.FirstOrDefault(s => s.Id == r.StationId)?.Name; });

            return res;
        }

        /// <summary>
        /// 予約更新
        /// </summary>
        /// <param name="reserve"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Update(Reserve reserve, Config config)
        {
            if (string.IsNullOrWhiteSpace(reserve.Id))
            {
                reserve.Id = Guid.NewGuid().ToString();
            }

            const string query = @"REPLACE INTO 
                                       Reserves
                                   (
                                       Id,
                                       Content
                                   )
                                   VALUES
                                   (
                                       @Id,
                                       @Content
                                   )";

            SqliteConnection.Execute(query, new {Id = reserve.Id, Content = JsonConvert.SerializeObject(reserve)});
            RefreshTasks(config, reserve);
            return true;
        }

        /// <summary>
        /// 予約削除
        /// </summary>
        /// <param name="reserveId"></param>
        /// <returns></returns>
        public bool Delete(string reserveId)
        {
            SqliteConnection.Execute("DELETE FROM Reserves WHERE Id = @Id", new {Id = reserveId});
            return true;
        }

        /// <summary>
        /// タスク更新
        /// </summary>
        public void RefreshTasks(Config config, Reserve reserve = null)
        {           
            var reserves = new List<Reserve>();
            if (reserve == null)
            {
                reserves = SqliteConnection.Query<Reserve>("SELECT * FROM Reserves").ToList();
            }
            else
            {
                reserves.Add(reserve);
            }
            var tasks = new List<ReserveTask>();
            foreach (var r in reserves)
            {
                var days = new List<DateTime[]>();
                if (r.Repeat.Count == 0)
                {
                    // 単発
                    days.Add(new[]{ r.Start, r.End});
                }
                else
                {
                    // 曜日指定
                    var date = DateTime.Now;
                    for (var i = 0; i < 7; i++)
                    {
                        if (r.Repeat.Contains((int) date.DayOfWeek))
                        {
                            var start = date.Date.AddHours(r.Start.Hour).AddMinutes(r.Start.Minute)
                                .AddSeconds(r.Start.Second);
                            var end = date.Date.AddHours(r.End.Hour).AddMinutes(r.End.Minute).AddSeconds(r.End.Second);
                            if (end < start)
                            {
                                end = end.AddDays(1);
                            }
                            days.Add(new[]{ start, end});
                        }

                        date = date.AddDays(1);
                    }
                }
                
                days.ForEach(d =>
                {
                    if (r.IsTimeFree)
                    {
                        // タイムフリー
                        var task = new ReserveTask()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Start = d[1].AddMinutes(config.TimeFreeMargin),
                            End = d[1].AddMinutes(config.TimeFreeMargin + 10),
                            ProgramStart = d[0],
                            ProgramEnd = d[1],
                            ReserveId = r.Id
                        };
                        tasks.Add(task);
                    
                    }
                    else
                    {
                        // リアルタイム
                        var task = new ReserveTask()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Start = d[0],
                            End = d[1],
                            ReserveId = r.Id,
                            ProgramStart = d[0],
                            ProgramEnd = d[1]
                        };
                        tasks.Add(task);
                    }
                });
                
            }


            using (var trn = SqliteConnection.BeginTransaction())
            {
                var where = reserve != null ? "WHERE ReserveId IN @ReserveIds" : "";
                SqliteConnection.Execute($"DELETE FROM ReserveTasks {where}",
                    new {ReserveIds = reserves.Select(r => r.Id).ToList()}, trn);
                const string query = @"INSERT INTO 
                                           ReserveTasks
                                       (
                                           Id,
                                           Start,
                                           End,
                                           ReserveId,
                                           ProgramStart,
                                           ProgramEnd
                                       )
                                       VALUES
                                       (
                                           @Id,
                                           @Start,
                                           @End,
                                           @ReserveId,
                                           @ProgramStart,
                                           @ProgramEnd
                                       )";
                tasks.ForEach(t => { SqliteConnection.Execute(query, t, trn); });
                
                trn.Commit();

            }
        }

        /// <summary>
        /// タスク取得
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public List<ReserveTask> GetTasks(bool active = false)
        {
            List<ReserveTask> res;
            if (active)
            {
                res = SqliteConnection
                    .Query<ReserveTask>("SELECT * FROM ReserveTasks WHERE Start <= @Now AND End > @Now ORDER BY Start",
                        new {Now = DateTime.Now}).ToList();
            }
            else
            {
                res = SqliteConnection
                    .Query<ReserveTask>("SELECT * FROM ReserveTasks ORDER BY Start").ToList();

            }

            var reserves = SqliteConnection.Query<Reserve>("SELECT * FROM Reserves WHERE Id IN @Ids",
                new {Ids = res.Select(r => r.ReserveId).Distinct().ToList()}).ToList();

            // 放送局取得
            var stations = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id IN @Ids",
                new {Ids = reserves.Select(r => r.StationId).Distinct().ToList()});
            res.ForEach(t =>
            {
                t.Reserve = reserves.FirstOrDefault(r => r.Id == t.ReserveId);
                t.Station = stations.FirstOrDefault(s => s.Id == t.Reserve.StationId);
            });

            return res;
        }

    }
}