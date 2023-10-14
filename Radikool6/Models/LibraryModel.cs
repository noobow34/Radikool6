using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Radikool6.Classes;
using Radikool6.Entities;

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
            var libraries = SqliteConnection.Query<Library>("SELECT * FROM Libraries").ToList();
            var statios = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id IN @Ids",
                new {Ids = libraries.Select(l => l.Program.StationId).Distinct().ToList()});
            
            
            libraries.ForEach(l =>
            {
                var fi = new FileInfo(l.Path);
                l.Size = Utility.Text.ToSizeString(fi.Length);
                l.Created = fi.CreationTime;
                l.Path = Path.GetFileName(l.Path);
                l.Program.Station = statios.FirstOrDefault(s => s.Id == l.Program.StationId);

            });
            return libraries;
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
        /// ライブラリ削除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(string id)
        {
            
            var library = Get(id);
            // ファイル削除
            try
            {
                if (File.Exists(library.Path))
                {
                    File.Delete(library.Path);
                }
            }
            catch (Exception ex)
            {
                Global.Logger.Error($"{ex.Message}\n{ex.StackTrace}");
            }

            SqliteConnection.Execute("DELETE FROM Libraries WHERE Id = @Id", new {Id = library.Id});
            return true;
        }

        /// <summary>
        /// 不要データ削除
        /// </summary>
        /// <returns></returns>
        public bool Maintenance()
        {
            var dir = new DirectoryInfo(Path.Combine("wwwroot", "records"));
            var files = dir.EnumerateFiles("*.m4a").Select(f => f.FullName).ToList();
            var libraries = SqliteConnection.Query<Library>("SELECT * FROM Libraries");
            foreach (var library in libraries)
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