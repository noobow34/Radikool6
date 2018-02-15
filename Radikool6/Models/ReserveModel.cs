using System;
using System.Collections.Generic;
using System.Linq;
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
            return Db.Reserves.ToList();
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
            
            return true;
        }
        
    }
}