using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Entities;

namespace Radikool6.Controllers
{
    public class TaskController : BaseController
    {
        private readonly Db _db;
        
        public TaskController(Db db)
        {
            _db = db;
        }
        
        /// <summary>
        /// 録音タスク取得
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/task/")]
        public async Task<ApiResponse> Index()
        {
            return await Execute(() =>
            {
                Result.Result = true;
                Result.Data = Program.Core.GetStatus();
            });
        }
    }
}