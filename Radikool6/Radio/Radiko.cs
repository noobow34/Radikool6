using System;
using Radikool6.Classes;
using Radikool6.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public static async Task<string> GetAuthToken()
        {
            using (var client = new HttpClient())
            {
                // auth token取得
                client.DefaultRequestHeaders.Add("pragma", "no-cache");
                client.DefaultRequestHeaders.Add("x-radiko-app", "pc_html5");
                client.DefaultRequestHeaders.Add("x-radiko-app-version", "0.0.1");
                client.DefaultRequestHeaders.Add("x-radiko-device", "pc");
                client.DefaultRequestHeaders.Add("x-radiko-user", "dummy_user");

                var res = await client.GetAsync(Define.Radiko.Auth1);
                var token = res.Headers.GetValues("X-Radiko-AuthToken").FirstOrDefault();
                int.TryParse(res.Headers.GetValues("X-Radiko-KeyLength").FirstOrDefault(), out var keyLength);
                int.TryParse(res.Headers.GetValues("X-Radiko-KeyOffset").FirstOrDefault(), out var keyOffset);

                // partial keyの元を取得
                client.DefaultRequestHeaders.Clear();
                var js = await client.GetStringAsync(Define.Radiko.CommonJs);
                
                var m = Regex.Match(js, @"new RadikoJSPlayer.*{");
                var key = "";
                if (m.Success)
                {
                    key = m.Value.Split(",")[2].Replace("'", "").Trim();
                }

                var partialKey =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(key.Substring(keyOffset, keyLength)));
                
                // auto tokenを有効可
                client.DefaultRequestHeaders.Add("x-radiko-authtoken", token);
                client.DefaultRequestHeaders.Add("x-radiko-device", "pc");
                client.DefaultRequestHeaders.Add("x-radiko-partialkey", partialKey);
                client.DefaultRequestHeaders.Add("x-radiko-user", "dummy_user");
                res = await client.GetAsync(Define.Radiko.Auth2);

                return token;
            }
        }

    }
}
