using Microsoft.AspNetCore.Mvc;
using Radikool6.Models;

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
                var library = lModel.Get(id);
                if (library != null && library.FileBinary != null && library.FileBinary.Length != 0)
                {
                    return File(new MemoryStream(library.FileBinary), "audio/mp4", $"{library.FileName}.m4a");
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
