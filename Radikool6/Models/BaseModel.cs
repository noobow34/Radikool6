using Microsoft.Data.Sqlite;

namespace Radikool6.Models
{
    public class BaseModel
    {
        protected SqliteConnection SqliteConnection { get; }

        protected BaseModel(SqliteConnection con)
        {
            SqliteConnection = con;
        }
    }
}