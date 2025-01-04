using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Radikool6.BackgroundTask;
using Radikool6.Classes;

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