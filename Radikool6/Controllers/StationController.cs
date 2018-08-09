using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public StationController()
        {
        }

        /// <summary>
        /// 種別指定で放送局取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/station/{type}")]
        public async Task<ApiResponse> Get(string type)
        {
            
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var model = new StationModel(SqliteConnection);
                    Result.Result = true;
                    Result.Data = model.Get(type);
                }
                
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
                using (SqliteConnection)
                {
                    var stations = new List<Station>();
                    switch (type)
                    {
                        case Define.Radiko.TypeName:
                            var cMolde = new ConfigModel(SqliteConnection);
                            var config = cMolde.Get();
                            var login = !string.IsNullOrWhiteSpace(config?.RadikoEmail) &&
                                        !string.IsNullOrEmpty(config.RadikoPassword) &&
                                        Radiko.Login(config.RadikoEmail, config.RadikoPassword).Result;
                            stations = Radiko.GetStations(login).Result;
                            break;
                        case Define.Nhk.TypeName:
                            stations = Nhk.GetStations().Result;
                            break;
                        case Define.ListenRadio.TypeName:
                            stations = ListenRadio.GetStations().Result;
                            break;
                    }

                    var model = new StationModel(SqliteConnection);
                    model.Refresh(stations);

                    Result.Data = stations;
                    Result.Result = true;
                }
            });
        }
    }
}