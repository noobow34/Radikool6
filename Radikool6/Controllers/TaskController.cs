using Microsoft.AspNetCore.Mvc;

namespace Radikool6.Controllers
{
    public class TaskController : BaseController
    {       
        public TaskController()
        {
        }
        
        /// <summary>
        /// 録音タスク取得
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/task/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                Result.Result = true;
                Result.Data = Globals.Core.GetStatus();
            });
        }
        
        /// <summary>
        /// 録音タスク停止／再開
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/task/{taskId}")]
        public async Task<ApiResponse> Post(string taskId)
        {
            return await Execute(() =>
            {
                Globals.Core.StopRestart(taskId);
                Result.Result = true;
            });
        }
    }
}