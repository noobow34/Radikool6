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
    public class ReserveController : BaseController
    {
        public ReserveController()
        {
        }

        /// <summary>
        /// 予約取得
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/reserve/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var model = new ReserveModel(SqliteConnection);
                    Result.Result = true;
                    Result.Data = model.Get();
                }
            });

        }

        /// <summary>
        /// 予約更新
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/reserve")]
        public async Task<ApiResponse> Post([FromBody]Reserve reserve)
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var model = new ReserveModel(SqliteConnection);
                    Result.Result = model.Update(reserve);
                }
            });
        }
        
        /// <summary>
        /// 予約削除
        /// </summary>
        /// <param name="reserveId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/reserve/{reserveId}")]
        public async Task<ApiResponse> Delete(string reserveId)
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var model = new ReserveModel(SqliteConnection);
                    Result.Result = model.Delete(reserveId);
                }
            });
        }
    }
}