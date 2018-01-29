using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class StationModel : BaseModel
    {
        public StationModel(Db db) : base(db)
        {
        }

        /// <summary>
        /// 放送局IDで放送局取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station GetById(string id)
        {
            return Db.Stations.Find(id);
        }
        
        /// <summary>
        /// 種別で放送局取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Station> Get(string type = "")
        {
            return string.IsNullOrWhiteSpace(type) ? Db.Stations.OrderBy(s => s.Sequence).ToList() : Db.Stations.Where(s => s.Type == type).OrderBy(s => s.Sequence).ToList();
        }
        
        /// <summary>
        /// 種別ごと完全更新
        /// </summary>
        /// <param name="stations"></param>
        public void Refresh(List<Station> stations)
        {
            var types = stations.Select(s => s.Type).Distinct();
            // 種別が同じものを削除
            Db.Stations.RemoveRange(Db.Stations.Where(s => types.Contains(s.Type)));
            Db.SaveChanges();

            Db.Stations.AddRange(stations);
            Db.SaveChanges();

        }
    }
}