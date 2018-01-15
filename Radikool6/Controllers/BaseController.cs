using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;

namespace Radikool6.Controllers
{
    public class BaseController : Controller
    {
        public ApiResponse Result { get; set; } = new ApiResponse();
        
        
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