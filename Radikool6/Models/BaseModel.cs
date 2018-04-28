using System.Runtime.InteropServices.ComTypes;
using Microsoft.Data.Sqlite;
using Radikool6.Entities;

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