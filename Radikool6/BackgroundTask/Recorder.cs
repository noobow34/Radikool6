using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.BackgroundTask
{
    public class Recorder
    {
        public string Id { get; set; }
        protected ReserveTask Task { get; set; }
        protected Config Config { get; set; }
        protected DateTime StartTime { get; set; }

        public RecorderStatus Status { get; set; } = RecorderStatus.None;
        
        public enum RecorderStatus
        {
            None,
            Working,
            Stopping,
            Stopped,
            Error,
            End
        }
        
        
        public static Recorder GetRecorder(Config config, ReserveTask task)
        {
            Recorder res = null;
            switch (task.Station.Type)
            {
                case Define.Radiko.TypeName:
                    res = new RadikoRecorder(config, task);
                    break;

            }

            return res;
        }

        protected Recorder(Config config, ReserveTask task = null)
        {
            Config = config;
            Task = task;
            Id = task?.Id;
        }

        /// <summary>
        /// 文字列置換
        /// </summary>
        /// <param name="src"></param>
        /// <param name="program"></param>
        /// <returns></returns>
        protected string Replace(string src, Station station, Entities.Program program)
        {
            var res = src;


            res = res.Replace("[TAG_TITLE]", Config.TagTitle);
            res = res.Replace("[TAG_ARTIST]", Config.TagArtist);
            res = res.Replace("[TAG_COMMENT]", Config.TagComment);
            res = res.Replace("[TAG_GENRE]", Config.TagGenre);
            res = res.Replace("[TAG_ALBUM]", Config.TagAlbum);
            
            res = res.Replace("[BITRATE]", Config.BitRate);
            res = res.Replace("[SAMPLINGRATE]", Config.SamplingRate);
            res = res.Replace("[VOLUME]", Config.Volume);
            
            res = res.Replace("[CH_NAME]", station.Name);
            res = res.Replace("[CH]", station.Code);

            if (!string.IsNullOrWhiteSpace(program.Id))
            {
                res = res.Replace("[TITLE]", program.Title);
                res = res.Replace("[CAST]", program.Cast);
                res = res.Replace("[INFO]", program.Description);
                res = res.Replace("[SMONTH]", program.Start.Month.ToString("d2"));
                res = res.Replace("[SDAY]", program.Start.Day.ToString("d2"));
                res = res.Replace("[SHOUR]", program.Start.Hour.ToString("d2"));
                res = res.Replace("[SMIN]", program.Start.Minute.ToString("d2"));
                res = res.Replace("[SYOBI]", GetYobi(program.Start.DayOfWeek));

                // 28時表記
                var date = program.Start.Hour < 5 ? program.Start.AddDays(-1) : program.Start;
                res = res.Replace("[SYEAR28]", Convert.ToString(date.Year));
                res = res.Replace("[SMONTH28]", date.Month.ToString("d2"));
                res = res.Replace("[SDAY28]", date.Day.ToString("d2"));
                res = res.Replace("[SHOUR28]",
                    date.Hour < 5 ? (date.Hour + 24).ToString("d2") : date.Hour.ToString("d2"));
                res = res.Replace("[SMIN28]", date.Minute.ToString("d2"));
                res = res.Replace("[SYOBI28]", GetYobi(date.DayOfWeek));


                res = res.Replace("[EYEAR]", Convert.ToString(program.End.Year));
                res = res.Replace("[EMONTH]", program.End.Month.ToString("d2"));
                res = res.Replace("[EDAY]", program.End.Day.ToString("d2"));
                res = res.Replace("[EHOUR]", program.End.Hour.ToString("d2"));
                res = res.Replace("[EMIN]", program.End.Minute.ToString("d2"));
                res = res.Replace("[EYOBI]", GetYobi(program.End.DayOfWeek));

                // 28時表記
                date = program.End.Hour < 5 ? program.End.AddDays(-1) : program.End;
                res = res.Replace("[EYEAR28]", Convert.ToString(date.Year));
                res = res.Replace("[EMONTH28]", date.Month.ToString("d2"));
                res = res.Replace("[EDAY28]", date.Day.ToString("d2"));
                res = res.Replace("[EHOUR28]",
                    date.Hour < 5 ? (date.Hour + 24).ToString("d2") : date.Hour.ToString("d2"));
                res = res.Replace("[EMIN28]", date.Minute.ToString("d2"));
                res = res.Replace("[EYOBI28]", GetYobi(date.DayOfWeek));

            }


            return res;
        }
        
        /// <summary>
        /// 日本語表記の曜日取得
        /// </summary>
        /// <param name="yobi"></param>
        /// <returns></returns>
        private static string GetYobi(DayOfWeek yobi)
        {
            string res = "";
            switch (yobi)
            {
                case DayOfWeek.Sunday:
                    res = "日";
                    break;
                case DayOfWeek.Monday:
                    res = "月";
                    break;
                case DayOfWeek.Tuesday:
                    res = "火";
                    break;
                case DayOfWeek.Wednesday:
                    res = "水";
                    break;
                case DayOfWeek.Thursday:
                    res = "木";
                    break;
                case DayOfWeek.Friday:
                    res = "金";
                    break;
                case DayOfWeek.Saturday:
                    res = "土";
                    break;
            }


            return res;
        }

    }
}