using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Radikool6.BackgroundTask;
using Radikool6.Classes;
using Radikool6.Entities;
using Radikool6.Radio;

namespace Radikool6
{
    public class Program
    {
        public static Core Core;
        
        public static void Main(string[] args)
        {
            Init();
            
            Core = new Core();
            Core.Run();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://0.0.0.0:5000")
                .Build();

        /// <summary>
        /// 初期化
        /// </summary>port
        private static void Init()
        {
            if (File.Exists(Define.File.KeyFile))
            {
                Global.EncKey = File.ReadAllText(Define.File.KeyFile);
            }
            else
            {
                Global.EncKey = Guid.NewGuid().ToString("N");
                File.WriteAllText(Define.File.KeyFile, Global.EncKey);
            }
            Schemas.Upgrade.Execute();
        }
    }
}