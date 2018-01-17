using System;
using System.Collections.Generic;
using System.Linq;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ProgramModel : BaseModel
    {
        public ProgramModel(Db db) : base(db)
        {
        }

        /// <summary>
        /// 番組検索
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Entities.Program> Search(string stationId, string from, string to, string keyword)
        {
            var fromDate = Utility.Text.StringToDate(from);
            var toDate = Utility.Text.StringToDate(to);
            
            List<Entities.Program>res = new List<Entities.Program>();
            var q = Db.Programs.Where(p => p.Id != null);
            if (!string.IsNullOrWhiteSpace(stationId))
            {
                q = q.Where(p => p.StationId == stationId);
            }

            if (fromDate > DateTime.MinValue)
            {
                q = q.Where(p => p.End > fromDate);
            }
            if (toDate > DateTime.MinValue)
            {
                q = q.Where(p => p.Start < toDate);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                q = q.Where(p => p.Title.Contains(keyword) || p.Cast.Contains(keyword) || p.Description.Contains(keyword));
            }

            res = q.OrderBy(p => p.StationId).ThenBy(p => p.Start).ToList();


            return res;
        }
        
        
        /// <summary>
        /// 番組データ更新
        /// </summary>
        /// <param name="programs"></param>
        public void Refresh(List<Entities.Program> programs)
        {
            var stationIds = programs.Select(p => p.StationId).Distinct();
            Db.Programs.RemoveRange(Db.Programs.Where(p => stationIds.Contains(p.StationId)));
            Db.SaveChanges();
            
            Db.Programs.AddRange(programs);
            Db.SaveChanges();
        }

    }
}