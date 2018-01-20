using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Radikool6.Entities;

namespace Radikool6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Init();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        /// <summary>
        /// 初期化
        /// </summary>
        private static void Init()
        {
            Schemas.Upgrade.Execute();
        }
    }
}
