using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Models;
using Radikool6.Radio;

namespace Radikool6.Controllers
{
    public class LibraryController : BaseController
    {
        public LibraryController()
        {
        }

        [HttpGet]
        [Route("library/play/{id}")]
        public ActionResult Play(string id)
        {
            using (SqliteConnection)
            {
                var lModel = new LibraryModel(SqliteConnection);
                var library = lModel.Get(id);

                if (library != null && System.IO.File.Exists(library.FileName))
                {
                    return File(System.IO.File.OpenRead(library.FileName), "audio/aac");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// ライブラリ取得
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/library/")]
        public async Task<ApiResponse> Get()
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var model = new LibraryModel(SqliteConnection);
                    Result.Result = true;
                    Result.Data = model.Get();
                }
            });
        }

    }
}