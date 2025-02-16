using Microsoft.AspNetCore.Mvc;
using Radikool6.BackgroundTask;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;
using Radikool6.Radio;

namespace Radikool6.Controllers
{
    public class ProgramController : BaseController
    {
        private static int _timefreeProgress;
        
        public ProgramController()
        {
        }

        /// <summary>
        /// 番組表検索
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/program/")]
        public async Task<ApiResponse> Index([FromBody] ProgramSearchCondition cond)
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var pModel = new ProgramModel(SqliteConnection);
                    var res = pModel.Search(cond);
                    if (res.Count == 0 && string.IsNullOrWhiteSpace(cond.Keyword) && cond.Refresh)
                    {
                        RefreshPrograms(cond.StationId);
                        res = pModel.Search(cond);
                    }
                    Result.Data = new { programs = res, range = pModel.GetRange(cond.StationId)};
                    Result.Result = true;
                }
                
            });
        }

        /// <summary>
        /// サーバから番組表取得
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/program/{stationId}")]
        public async Task<ApiResponse> Post(string stationId)
        {
            return await Execute(() =>
            {
                Result.Data = RefreshPrograms(stationId);
                Result.Result = true;
            });
        }
        
        /// <summary>
        /// タイムフリーダウンロード
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/program/tf/{programId}")]
        public async Task<ApiResponse> Timefree(string programId)
        {
            return await Execute(() =>
            {
                _timefreeProgress = 0;
                using (SqliteConnection)
                {
                    var pModel = new ProgramModel(SqliteConnection);
                    var program = pModel.Get(programId);

                    var cModel = new ConfigModel(SqliteConnection);
                    var config = cModel.Get();
                
                    if (program != null)
                    {
                        var rr = new RadikoRecorder(config)
                        {
                            ChangeProgress = (progress) =>
                            {
                                _timefreeProgress = progress;
                            }
                        };
                        _ = rr.TimeFree(program);
                    }
                    Result.Result = true;
                }
                
            });
        }

        /// <summary>
        /// タイムフリーの進捗確認
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/program/tf/")]
        public async Task<ApiResponse> GetTimefreeProgress()
        {
            return await Execute(() =>
            {
                Result.Data = _timefreeProgress;
                Result.Result = true;
            });
        }

        /// <summary>
        /// 番組表取得処理
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private List<Entities.Program> RefreshPrograms(string stationId)
        {
            using (SqliteConnection)
            {
                var sModel = new StationModel(SqliteConnection);
                var station = sModel.GetById(stationId);
                var programs = new List<Entities.Program>();
                if (station == null) return programs;

                switch (station.Type)
                {
                    case Define.Radiko.TypeName:
                    
                        programs = Radiko.GetPrograms(station).Result;
                        break;
                    case Define.Nhk.TypeName:
                        programs = Nhk.GetPrograms(station, DateTime.Now, DateTime.Now.AddDays(1)).Result;
                        break;
                }

                var pModel = new ProgramModel(SqliteConnection);
                pModel.Refresh(programs);
                
                return programs;
            }
        }
    }
}