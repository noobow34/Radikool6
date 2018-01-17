using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        /// <param name="stationId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/program/")]
        public async Task<ApiResponse> Index(string stationId = "", string from = "", string to = "",
            string keyword = "")
        {
            return await Execute(() =>
            {
                var pModel = new ProgramModel(_db);
                Result.Data = pModel.Search(stationId, from, to, keyword);
                Result.Result = true;
            });
        }

        [HttpPost]
        [Route("api/program/{stationId}")]
        public async Task<ApiResponse> Post(string stationId)
        {
            return await Execute(() =>
            {
                var sModel = new StationModel(_db);
                var station = sModel.GetById(stationId);
                if (station == null) return;
                var programs = new List<Entities.Program>();
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
                Result.Data = programs;
                Result.Result = true;
            });
        }
    }
}