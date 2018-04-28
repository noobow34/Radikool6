using System.Collections.Generic;
using System.Threading.Tasks;
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

        
        [Route("api/config/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var cModel = new ConfigModel(SqliteConnection);
                    var res = cModel.Get() ?? new CommonConfig();
                    Result.Data = res;
                    Result.Result = true;
                }
            });
        }
        
        [HttpPost]
        [Route("api/config/")]
        public async Task<ApiResponse> Post([FromBody]CommonConfig config)
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
        
        
        [Route("api/encode_settings/")]
        public async Task<ApiResponse> GetEncodeSettings()
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var cModel = new ConfigModel(SqliteConnection);
                    var res = cModel.GetEncodeSettings();
                    Result.Data = res;
                    Result.Result = true;
                }
            });
        }
    }
}