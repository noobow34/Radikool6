using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Radio
{
    public class Nhk
    {
        /// <summary>
        /// 放送局取得
        /// </summary>
        /// <returns></returns>
        public static Task<List<Station>> GetStations()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = new List<Station>();
                var r1Ids = new List<string>();
                var r2Ids = new List<string>();
                var fmIds = new List<string>();



                var doc = XDocument.Load(Define.Nhk.Config);
                var timetableUrl = doc.Descendants("url_program_day").First().Value;
                if (timetableUrl.StartsWith("//"))
                {
                    timetableUrl = $"http:{timetableUrl}";
                }

                var order = 2;
                foreach (var data in doc.Descendants("stream_url").First().Descendants("data"))
                {
                    r1Ids.Add($"nhk_R1_{data.Element("area")?.Value}");
                    res.Add(new Station
                    {
                        Id = $"nhk_R1_{data.Element("area")?.Value}",
                        Name = $"ラジオ第1({data.Element("areajp")?.Value})",
                        Type = Define.Nhk.TypeName,
                        RegionName = data.Element("areajp")?.Value,
                        RegionId = data.Element("areakey")?.Value,
                        MediaUrl = data.Element("r1hls")?.Value,
                        Sequence = order++,
                        TimetableUrl = timetableUrl.Replace("{area}", data.Element("areakey")?.Value)
                            .Replace("{service}", "n1")
                    });

                    if (data.Element("areakey")?.Value == "130")
                    {
                        r2Ids.Add("nhk_R2");
                        res.Add(new Station
                        {
                            Id = "nhk_R2",
                            Name = "ラジオ第2",
                            Type = Define.Nhk.TypeName,
                            RegionName = "全国",
                            RegionId = "000",
                            MediaUrl = data.Element("r2hls")?.Value,
                            Sequence = 1,
                            TimetableUrl = timetableUrl.Replace("{area}", data.Element("areakey")?.Value)
                                .Replace("{service}", "n2")
                        });
                    }

                    fmIds.Add($"nhk_fm_{data.Element("area")?.Value}");
                    res.Add(new Station
                    {
                        Id = $"nhk_fm_{data.Element("area")?.Value}",
                        Name = $"NHK FM({data.Element("areajp")?.Value})",
                        Type = Define.Nhk.TypeName,
                        RegionName = data.Element("areajp")?.Value,
                        RegionId = data.Element("areakey")?.Value,
                        MediaUrl = data.Element("fmhls")?.Value,
                        Sequence = order++,
                        TimetableUrl = timetableUrl.Replace("{area}", data.Element("areakey")?.Value)
                            .Replace("{service}", "n3")
                    });
                }

                return res;

            });
        }

        /// <summary>
        /// 番組表取得
        /// </summary>
        /// <param name="station"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static Task<List<Entities.Program>> GetPrograms(Station station, DateTime from, DateTime to)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = new List<Entities.Program>();
                var date = from;


                var tryCount = new Dictionary<string, int>();

                while (date < to)
                {

                    var url = station.TimetableUrl.Replace("[YYYY-MM-DD]", date.ToString("yyyyMMdd"));
                    if (tryCount.ContainsKey(url))
                    {
                        tryCount[url]++;
                    }
                    else
                    {
                        tryCount.Add(url, 1);
                    }

                    try
                    {
                        if (tryCount[url] < 6)
                        {
                            var data = ParseProgramJson(station.Id, url).Result;
                            res = res.Concat(data).ToList();
                        }
                        else
                        {
                            //LogData.Save(Log.LogType.Error, "番組取得エラー(NHK):" + url + " 5回取得に失敗しました、取得をスキップします");
                        }

                        date = date.AddDays(1);

                    }
                    catch (Exception e)
                    {
                        //  LogData.Save(Log.LogType.Warn, "番組取得エラー(NHK):" + url + " " + e.Message + " 再試行します");
                        System.Threading.Thread.Sleep(5000);
                    }


                }

                return res.GroupBy(p => p.Id).Select(p => p.First()).ToList();

            });
        }

        private static async Task<List<Entities.Program>> ParseProgramJson(string stationId, string url)
        {
            var res = new List<Entities.Program>();

            var json = "";
            using (var hc = new HttpClient())
            {
                json = await hc.GetStringAsync(url);
            }

            var xml = JsonConvert.DeserializeXmlNode(json);
            var re = new Regex("[０-９Ａ-Ｚａ-ｚ：－　]+");

            foreach (XmlNode c in xml.FirstChild.ChildNodes)
            {
                string title = "", startTime = "", endTime = "", content = "", link = "";
                foreach (XmlNode n in c.ChildNodes)
                {

                    if (n.Name == "title")
                    {
                        title = n.FirstChild.Value;
                    }

                    if (n.Name == "start_time")
                    {
                        startTime = n.FirstChild.Value;
                    }

                    if (n.Name == "end_time")
                    {
                        endTime = n.FirstChild.Value;
                    }

                    if (n.Name == "content")
                    {
                        content = n.FirstChild.Value;
                    }

                    if (n.Name == "url" && n.FirstChild != null && n.FirstChild.ChildNodes != null)
                    {
                        link = n.FirstChild.FirstChild.Value;
                    }
                }

                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(startTime) &&
                    !string.IsNullOrWhiteSpace(endTime))
                {
                    res.Add(new Entities.Program
                    {
                        Id = $"{stationId}_{Utility.Text.StringToDate(startTime):yyyyMMddHHmmss}",
                        StationId = stationId,
                        Start = Utility.Text.StringToDate(startTime),
                        End = Utility.Text.StringToDate(endTime),
                        Title = title.Trim(),
                        Description = content.Trim(),
                    });
                }


            }

            return res;
        }

    }
}