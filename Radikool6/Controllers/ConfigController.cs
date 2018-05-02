using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Entities;
using Radikool6.Models;

namespace Radikool6.Controllers
{
    
    public class ConfigController : BaseController
    {
        public ConfigController()
        {
        }


        /// <summary>
        /// 設定取得
        /// </summary>
        /// <returns></returns>
        [Route("api/config/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var cModel = new ConfigModel(SqliteConnection);
                    var res = cModel.Get() ?? new Config();
                    Result.Data = res;
                    Result.Result = true;
                }
            });
        }
        
        /// <summary>
        /// 設定保存
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/config/")]
        public async Task<ApiResponse> Post([FromBody]Config config)
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var cModel = new ConfigModel(SqliteConnection);
                    Result.Result = cModel.Update(config);
                }
            });
        }
        

    }
}