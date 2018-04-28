using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.Data.Sqlite;
using Radikool6.Classes;

namespace Radikool6.Controllers
{
    public class BaseController : Controller
    {
        public ApiResponse Result { get; set; } = new ApiResponse();

        public SqliteConnection SqliteConnection
        {
            get
            {
                if (_sqliteConnection != null) return _sqliteConnection;
                _sqliteConnection = new SqliteConnection($"Data Source={Define.File.DbFile}");
                _sqliteConnection.Open();

                return _sqliteConnection;
            }
        }

        private SqliteConnection _sqliteConnection;
        
        public class ApiResponse
        {
            public bool Result { get; set; }
            public object Data { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }


        protected Task<ApiResponse> Execute(Action action)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Result.Errors.Add(ex.Message);
                }

                return Result;
            });
        }
    }
}