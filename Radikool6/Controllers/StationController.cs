using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;
using Radikool6.Radio;

namespace Radikool6.Controllers
{
    public class StationController : BaseController
    {
        private readonly Db _db;
        public StationController(Db db)
        {
            _db = db;
        }

        /// <summary>
        /// 種別指定で放送局取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/station/")]
        public async Task<ApiResponse> Get()
        {
            
            return await Execute(() =>
            {
                var model = new StationModel(_db);
                Result.Result = true;
                Result.Data = model.Get();
            });

        }

        /// <summary>
        /// 放送局再取得
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/station/{type}")]
        public async Task<ApiResponse> Post(string type)
        {
            return await Execute(() =>
            {
                var stations = new List<Station>();
                switch (type)
                {
                    case Define.Radiko.TypeName:
                        stations = Radiko.GetStations().Result;
                        break;
                    case Define.Nhk.TypeName:
                        stations = Nhk.GetStations().Result;
                        break;
                }

                var model = new StationModel(_db);
                using (var trn = _db.Database.BeginTransaction())
                {
                    model.Refresh(stations);
                    trn.Commit();
                }

                Result.Result = true;
            });
        }
    }
}