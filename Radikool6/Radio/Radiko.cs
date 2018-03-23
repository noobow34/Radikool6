using System;
using Radikool6.Classes;
using Radikool6.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Radikool6.Radio
{
    public class Radiko
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
                var doc = XDocument.Load(Define.Radiko.StationListFull);

                // 放送局一覧
                var sequence = 1;
                foreach (var stations in doc.Descendants("stations"))
                {
                    var regionId = stations.Attribute("region_id")?.Value;
                    var regionName = stations.Attribute("region_name")?.Value;
                    foreach (var station in stations.Descendants("station"))
                    {
                        var code = station.Descendants("id").First().Value;
                        var name = station.Descendants("name").First().Value;
                        var logo = station.Descendants("logo").FirstOrDefault()?.Value;
                        var areaId = station.Descendants("area_id").First().Value;
                        var url = station.Descendants("href").First().Value;
                        res.Add(new Station
                        {
                            Id = $"{Define.Radiko.TypeName}_{code}",
                            RegionId = regionId,
                            RegionName = regionName,
                            Code = code,
                            Name = name,
                            Area = areaId,
                            Type = Define.Radiko.TypeName,
                            Sequence = sequence++
                        });
                    }
                }

                return res;
            });
        }

        /// <summary>
        /// 番組表取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static Task<List<Entities.Program>> GetPrograms(Station station)
        {
            return Task.Factory.StartNew(() =>
            {
                var doc = XDocument.Load(Define.Radiko.WeeklyTimeTable.Replace("[stationCode]", station.Code));

                return doc.Descendants("prog")
                    .Select(prog => new Entities.Program()
                    {
                        Id = station.Code + prog.Attribute("ft")?.Value,
                        Start = Utility.Text.StringToDate(prog.Attribute("ft")?.Value),
                        End = Utility.Text.StringToDate(prog.Attribute("to")?.Value),
                        Title = prog.Element("title")?.Value.Trim(),
                        Cast = prog.Element("pfm")?.Value.Trim(),
                        Description = prog.Element("info")?.Value.Trim(),
                        StationId = station.Id
                    })
                    .ToList();

            });
        }

        /// <summary>
        /// auth token取得
        /// </summary>
        /// <returns></returns>
        public static Task<string> GetAuthToken()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    // auth token取得
                    var req = (HttpWebRequest) WebRequest.Create(Define.Radiko.Auth1);
                    req.Headers.Add("pragma", "no-cache");
                    req.Headers.Add("x-radiko-app", "pc_html5");
                    req.Headers.Add("x-radiko-app-version", "0.0.1");
                    req.Headers.Add("x-radiko-device", "pc");
                    req.Headers.Add("x-radiko-user", "dummy_user");

                    var res = (HttpWebResponse) req.GetResponse();
                    var token = res.Headers["X-Radiko-AuthToken"];
                    int.TryParse(res.Headers["X-Radiko-KeyLength"], out var keyLength);
                    int.TryParse(res.Headers["X-Radiko-KeyOffset"], out var keyOffset);


                    // partial keyの元を取得
                    req = (HttpWebRequest) WebRequest.Create(Define.Radiko.CommonJs);
                    res = (HttpWebResponse) req.GetResponse();
                    var js = "";
                    using (var r = new StreamReader(res.GetResponseStream()))
                    {
                        js = r.ReadToEnd();
                    }

                    var m = Regex.Match(js, @"new RadikoJSPlayer.*{");
                    var key = "";
                    if (m.Success)
                    {
                        key = m.Value.Split(",")[2].Replace("'", "").Trim();
                    }

                    var partialKey =
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(key.Substring(keyOffset, keyLength)));

                    // auto tokenを有効可
                    req = (HttpWebRequest) WebRequest.Create(Define.Radiko.Auth2);
                    req.Headers.Add("x-radiko-authtoken", token);
                    req.Headers.Add("x-radiko-device", "pc");
                    req.Headers.Add("x-radiko-partialkey", partialKey);
                    req.Headers.Add("x-radiko-user", "dummy_user");
                    res = (HttpWebResponse) req.GetResponse();

                    return token;
                }
                catch (Exception e)
                {
                    var a = e.Message;
                }

                return "";
            });
        }

    }
}
