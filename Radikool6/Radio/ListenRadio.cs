using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Radio
{
    public class ListenRadio
    {
        private class ListenRadioJson
        {
            public int Result { get; set; }
            public DateTime ServerTime { get; set; }
            public int Count { get; set; }
            public List<ListenRadioStation> Channel { get; set; }
            public List<ListenRadioCategory> Category { get; set; }
            public List<ListenRadioTimetable> ProgramSchedule { get; set; }
        }

        private class ListenRadioCategory
        {
            public string CategoryId { get; set; }
            public string CategoryName { get; set; }
        }

        private class ListenRadioStation
        {
            public string ChannelId { get; set; }
            public string ChannelName { get; set; }
            public string ChannelDetail { get; set; }
            public string ChannelImage { get; set; }
            public string ChannelLogo { get; set; }
            public string ChannelHls { get; set; }

        }

        private class ListenRadioTimetable
        {
            public string ProgramName { get; set; }
            public string ProgramSummary { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        /// <summary>
        /// 放送局取得
        /// </summary>
        /// <returns></returns>
        static public Task<List<Station>> GetStations()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = new List<Station>();
                // カテゴリ取得
                List<ListenRadioCategory> category;
                using (var client = new HttpClient())
                {
                    var data = JsonConvert.DeserializeObject<ListenRadioJson>(client
                        .GetStringAsync(Define.ListenRadio.CategoryList).Result);
                    category = data?.Category.Where(x => x.CategoryId != "99999").ToList();
                }

                if (category == null) return null;

                var sequence = 1;
                category.ForEach(x =>
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var data = JsonConvert.DeserializeObject<ListenRadioJson>(
                                client.GetStringAsync(Define.ListenRadio.StationList + x.CategoryId).Result);

                            var total = data.Channel.Count;

                            data.Channel.ForEach(c =>
                            {
                                if (!c.ChannelHls.StartsWith("http")) return;

                                res.Add(new Station
                                {
                                    Type = Define.ListenRadio.TypeName,
                                    Name = c.ChannelName,
                                    Id = $"{Define.ListenRadio.TypeName}_{c.ChannelId}",
                                    Code = c.ChannelId,
                                    Sequence = sequence++,
                                    RegionId = "lr_" + x.CategoryId,
                                    RegionName = x.CategoryName,
                                    MediaUrl = c.ChannelHls
                                });


                            });
                        }

                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                    }
                });

                return res;

            });

        }

        /// <summary>
        /// 番組表取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        static public Task<List<Entities.Program>> GetPrograms(Station station)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = new List<Entities.Program>();


                try
                {
                    using (var client = new HttpClient())
                    {
                        var data = JsonConvert.DeserializeObject<ListenRadioJson>(client
                            .GetStringAsync(Define.ListenRadio.Timetable + station.Code).Result);

                        data.ProgramSchedule.ForEach(s =>
                        {
                            res.Add(new Entities.Program
                            {
                                Id = station.Id + "_" + Utility.Text.StringToDate(s.StartDate + "00")
                                         .ToString("yyyyMMddHHmmss"),
                                StationId = station.Id,
                                Title = s.ProgramName,
                                Description = s.ProgramSummary,
                                Start = Utility.Text.StringToDate(s.StartDate + "00"),
                                End = Utility.Text.StringToDate(s.EndDate + "00"),
                            });
                        });
                    }

                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                }

                return res;

            });
        }
    }
}