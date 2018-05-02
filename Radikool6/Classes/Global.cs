using System.Collections.Generic;
using NLog;
using Radikool6.Entities;

namespace Radikool6.Classes
{
    public class Global
    {
        public static string EncKey { get; set; }
        public static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}