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
        [Route("library/download/{id}")]
        public ActionResult Download(string id)
        {
            using (SqliteConnection)
            {
                var lModel = new LibraryModel(SqliteConnection);
                lModel.Maintenance();
                var library = lModel.Get(id);

                if (library != null && System.IO.File.Exists(library.Path))
                {
                    return File(System.IO.File.OpenRead(library.Path), "audio/mp4", $"{library.FileName}.m4a");
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
                    var lModel = new LibraryModel(SqliteConnection);
                    lModel.Maintenance();
                    Result.Result = true;
                    Result.Data = lModel.Get();
                }
            });
        }
        
        /// <summary>
        /// ライブラリ削除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/library/{id}")]
        public async Task<ApiResponse> Delete(string id)
        {
            return await Execute(() =>
            {
                using (SqliteConnection)
                {
                    var lModel = new LibraryModel(SqliteConnection);
                    Result.Result = lModel.Delete(id);
                }
            });
        }

    }
}
