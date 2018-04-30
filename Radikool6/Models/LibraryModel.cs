using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class LibraryModel : BaseModel

    {
        public LibraryModel(SqliteConnection con) : base(con)
        {
        }

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
    }
}