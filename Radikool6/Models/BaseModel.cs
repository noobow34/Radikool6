using System.Runtime.InteropServices.ComTypes;
using Microsoft.Data.Sqlite;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class BaseModel
    {
        protected Db Db { get; private set; }
        protected SqliteConnection SqliteConnection { get; set; }
        public BaseModel(Db db)
        {
            this.Db = db;
        }

        public BaseModel(SqliteConnection con)
        {
            SqliteConnection = con;
        }
    }
}