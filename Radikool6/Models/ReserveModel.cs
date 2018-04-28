using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            var stations = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id IN @StationIds",
                new {StationIds = res.Select(r => r.StationId).Distinct().ToList()}).ToList();


            res.ForEach(r => { r.StationName = stations.FirstOrDefault(s => s.Id == r.StationId)?.Name; });

            return res;
        }

        /// <summary>
        /// 予約更新
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        public bool Update(Reserve reserve)
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
            RefreshTasks(reserve);
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
        public void RefreshTasks(Reserve reserve = null)
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
                if (r.Repeat.Count == 0)
                {
                    // 単発
                    var task = new ReserveTask()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Start = r.Start,
                        End = r.End,
                        ReserveId = r.Id
                    };
                    tasks.Add(task);
                }
            }


            using (var trn = SqliteConnection.BeginTransaction())
            {
                SqliteConnection.Execute("DELETE FROM ReserveTasks WHERE ReserveId IN @ReserveIds",
                    new {ReserveIds = reserves.Select(r => r.Id).ToList()}, trn);
                const string query = @"INSERT INTO 
                                           ReserveTasks
                                       (
                                           Id,
                                           Start,
                                           End,
                                           ReserveId
                                       )
                                       VALUES
                                       (
                                           @Id,
                                           @Start,
                                           @End,
                                           @ReserveId
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