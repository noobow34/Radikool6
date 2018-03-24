using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Entities;
using Radikool6.Models;

namespace Radikool6.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly Db _db;
        public ConfigController(Db db)
        {
            _db = db;
        }

        [Route("api/encode_settings/")]
        public async Task<ApiResponse> GetEncodeSettings()
        {
            return await Execute(() =>
            {
                var cModel = new ConfigModel(_db);
                var res = cModel.GetEncodeSettings();
                Result.Data = res;
                Result.Result = true;
            });
        }
    }
}