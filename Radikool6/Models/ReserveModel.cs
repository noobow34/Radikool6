using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ReserveModel : BaseModel
    {
        public ReserveModel(Db db) : base(db)
        {
        }

        /// <summary>
        /// 予約取得
        /// </summary>
        /// <returns></returns>
        public List<Reserve> Get()
        {
            var res = Db.Reserves.ToList();
            var stations = Db.Stations.Where(s => res.Select(r => r.StationId).Distinct().Contains(s.Id)).ToList();            
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
            Reserve orgData = null;
            if (!string.IsNullOrWhiteSpace(reserve.Id))
            {
                orgData = Db.Reserves.Find(reserve.Id);
            }
           
            if (orgData != null)
            {
                orgData.Content = JsonConvert.SerializeObject(reserve);
            }
            else
            {
                reserve.Id = Guid.NewGuid().ToString();
                Db.Reserves.Add(reserve);
            }

            Db.SaveChanges();
            this.RefreshTasks(reserve);
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
                reserves = Db.Reserves.ToList();
            }
            else
            {
                reserves.Add(reserve);
            }
            var tasks = new List<ReserveTask>();
            foreach (var r in Db.Reserves.ToList())
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

            var reserveIds = reserves.Select(r => r.Id).ToList();
            Db.ReserveTasks.RemoveRange(Db.ReserveTasks.Where(t => reserveIds.Contains(t.ReserveId)));
            Db.ReserveTasks.AddRange(tasks);
            Db.SaveChanges();
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
                res = Db.ReserveTasks.Include(t => t.Reserve).Where(t => t.Start <= DateTime.Now && t.End > DateTime.Now).OrderBy(t => t.Start)
                    .ToList();
            }
            else
            {
                res = Db.ReserveTasks.Include(t => t.Reserve).OrderBy(t => t.Start).ToList();

            }
            
            // 放送局取得
            var stationIds = res.Select(t => t.Reserve.StationId).ToList();
            var stations = Db.Stations.Where(s => stationIds.Contains(s.Id)).ToList();
            res.ForEach(t => { t.Station = stations.FirstOrDefault(s => s.Id == t.Reserve.StationId); });

            return res;
        }

    }
}