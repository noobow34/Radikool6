using Dapper;
using Microsoft.Data.Sqlite;
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
            var libraries = SqliteConnection.Query<Library>("SELECT id,filename,programjson,round(length(FileBinary)/1024.0/1024.0,1) || 'MB' size,Created FROM Libraries").ToList();
            var statios = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id IN @Ids",
                new {Ids = libraries.Select(l => l.Program.StationId).Distinct().ToList()});
            
            
            libraries.ForEach(l =>
            {
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
                                       ProgramJson,
                                       FileBinary,
                                       Created
                                   )
                                   VALUES
                                   (
                                       @Id,
                                       @FileName,
                                       @ProgramJson,
                                       @FileBinary,
                                       datetime()
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
            SqliteConnection.Execute("DELETE FROM Libraries WHERE Id = @Id", new {Id = id});
            return true;
        }
    }
}