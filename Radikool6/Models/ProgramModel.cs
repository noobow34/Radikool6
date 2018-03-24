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
        /// <param name="cond"></param>
        /// <returns></returns>
        public List<Entities.Program> Search(ProgramSearchCondition cond)
        {
            var q = Db.Programs.Where(p => p.Id != null);
            if (!string.IsNullOrWhiteSpace(cond.StationId))
            {
                q = q.Where(p => p.StationId == cond.StationId);
            }

            if (cond.From != null)
            {
                q = q.Where(p => p.End > cond.From);
            }

            if (cond.To != null)
            {
                q = q.Where(p => p.Start < cond.To);
            }

            if (!string.IsNullOrWhiteSpace(cond.Keyword))
            {
                q = q.Where(p =>
                    p.Title.Contains(cond.Keyword) || p.Cast.Contains(cond.Keyword) ||
                    p.Description.Contains(cond.Keyword));
            }

            var res = q.OrderBy(p => p.StationId).ThenBy(p => p.Start).ToList();


            return res;
        }

        /// <summary>
        /// データの範囲を取得
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DateTime[] GetRange(string stationId)
        {
            var min = Db.Programs.Where(p => p.StationId == stationId).Min(p => p.Start);
            var max = Db.Programs.Where(p => p.StationId == stationId).Max(p => p.Start);
            return new[] {min, max};
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