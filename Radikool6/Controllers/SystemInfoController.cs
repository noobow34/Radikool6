using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;

namespace Radikool6.Controllers
{
    
    public class SystemInfoController : BaseController
    {
        public SystemInfoController()
        {
        }


        /// <summary>
        /// システム情報取得
        /// </summary>
        /// <returns></returns>
        [Route("api/system-info/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                var res = new SystemInfo();
                Result.Data = res;
                Result.Result = true;
            });
        }




    }
}