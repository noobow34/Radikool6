using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Radikool6.Entities;
using SQLitePCL;

namespace Radikool6.Models
{
    public class LibraryModel : BaseModel

    {
        public LibraryModel(SqliteConnection con) : base(con)
        {
        }


        /// <summary>
        /// ライブラリ取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Library> Get()
        {
            return SqliteConnection.Query<Library>("SELECT * FROM Libraries");
        }
        
        /// <summary>
        /// ライブラリ取得
        /// </summary>
        /// <returns></returns>
        public Library Get(string id)
        {
            return SqliteConnection.Query<Library>("SELECT * FROM Libraries WHERE Id = @Id", new { Id = id }).FirstOrDefault();
        }
        
        
        /// <summary>
        /// ライブラリ更新
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public bool Update(Library library)
        {
            const string query = @"REPLACE INTO 
                                       Libraries
                                   (
                                       Id,
                                       FileName,
                                       Path,
                                       ProgramJson

                                   )
                                   VALUES
                                   (
                                       @Id,
                                       @FileName,
                                       @Path,
                                       @ProgramJson
                                   )";

            SqliteConnection.Execute(query, library);
            return true;
        }

        /// <summary>
        /// 不要データ削除
        /// </summary>
        /// <returns></returns>
        public bool Maintenance()
        {
            var dir = new DirectoryInfo("records");
            var files = dir.EnumerateFiles("*.m4a").Select(f => f.FullName).ToList();
            foreach (var library in Get())
            {
                if (!files.Contains(library.Path))
                {
                    SqliteConnection.Execute("DELETE FROM Libraries WHERE Id = @Id", new {Id = library.Id});
                }
            }

            return true;

        }
    }
}