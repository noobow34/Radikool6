using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly Db _db;
        public ProgramController(Db db)
        {
            _db = db;
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
                var pModel = new ProgramModel(_db);
                var res = pModel.Search(cond);

                if (res.Count == 0 && string.IsNullOrWhiteSpace(cond.Keyword) && cond.Refresh)
                {
                    RefreshPrograms(cond.StationId);
                    res = pModel.Search(cond);
                }
                Result.Data = new { programs = res, range = pModel.GetRange(cond.StationId)};
                Result.Result = true;
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
                Result.Data = this.RefreshPrograms(stationId);
                Result.Result = true;
            });
        }
        
        [HttpGet]
        [Route("api/program/tf/{programId}")]
        public async Task<ApiResponse> Download(string programId)
        {
            return await Execute(() =>
            {
                var pModel = new ProgramModel(_db);
                var program = pModel.Get(programId);

                var cModel = new ConfigModel(_db);
                var config = cModel.Get();
                
                if (program != null)
                {
                    var rr = new RadikoRecorder(config);
                    rr.TimeFree(program);
                }
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
            var sModel = new StationModel(_db);
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


            var pModel = new ProgramModel(_db);
            pModel.Refresh(programs);

            return programs;
        }

        
        
    }
}