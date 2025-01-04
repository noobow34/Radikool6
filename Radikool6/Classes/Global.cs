using NLog;

namespace Radikool6.Classes
{
    public class Global
    {
        public static string EncKey { get; set; }
        public static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static string BaseDir { get; set; }
    }
}