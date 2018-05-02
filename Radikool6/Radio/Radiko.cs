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
using Newtonsoft.Json;

namespace Radikool6.Radio
{
    public class Radiko
    {
        private static CookieContainer _cookieContainer;
        
        /// <summary>
        /// プレミアムログイン
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static async Task<bool> Login(string email, string pass)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass)) return false;
            
            var result = false;
            using (var handler = new HttpClientHandler() {UseCookies = true})
            {
                using (var client = new HttpClient(handler))
                {

                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"mail", email},
                        {"pass", pass}
                    });

                    var res = await client.PostAsync(Define.Radiko.Login, content);
                    var html = await res.Content.ReadAsStringAsync();
                    _cookieContainer = handler.CookieContainer;

                    res = await client.GetAsync(Define.Radiko.LoginCheck);
                    var json = await res.Content.ReadAsStringAsync();
                    try
                    {
                        var loginResult = JsonConvert.DeserializeObject<RadikoLoginCheckResult>(json);
                        result = !string.IsNullOrWhiteSpace(loginResult.UserKey);
                    }
                    catch (Exception e)
                    {
                        result = false;
                    }

                }
            }

            return result;
        }
        
        
        /// <summary>
        /// 放送局取得
        /// </summary>
        /// <returns></returns>
        public static Task<List<Station>> GetStations(bool login)
        {
            return Task.Factory.StartNew(() =>
            {
                var xmlUrl = Define.Radiko.StationListFull;
                if (!login)
                {
                    // 地域判定をする
                    using (var client = new HttpClient())
                    {
                        var text = client.GetStringAsync(Define.Radiko.AreaCheck).Result;
                        var m = Regex.Match(text, @"JP[0-9]+");
                        if (m.Success)
                        {
                            xmlUrl = Define.Radiko.StationListPref.Replace("[AREA]", m.Value);
                        }
                    }
                }
                
                
                var res = new List<Station>();
                var doc = XDocument.Load(xmlUrl);

                // 放送局一覧
                var sequence = 1;
                foreach (var stations in doc.Descendants("stations"))
                {
                    var regionId = stations.Attribute("region_id")?.Value ?? "";
                    var regionName = stations.Attribute("region_name")?.Value ?? "";
                    foreach (var station in stations.Descendants("station"))
                    {
                        var code = station.Descendants("id").First().Value;
                        var name = station.Descendants("name").First().Value;
                        var logo = station.Descendants("logo").FirstOrDefault()?.Value ?? "";
                        var areaId = station.Descendants("area_id").FirstOrDefault()?.Value ?? "";
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
                        StationId = station.Id,
                        TsNg = prog.Element("ts_in_ng")?.Value.Trim()
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
            using (var handler = new HttpClientHandler() {UseCookies = true})
            {
                using (var client = new HttpClient(handler))
                {
                    if (_cookieContainer != null)
                    {
                        handler.CookieContainer = _cookieContainer;
                    }
                    
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

        /// <summary>
        /// タイムフリーのM3U8取得
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static async Task<string> GetTimeFreeM3U8(Entities.Program program)
        {
            var m3U8 = "";
            var token = await GetAuthToken();
            var url = Define.Radiko.TimeFreeM3U8.Replace("[CH]", program.Station.Code)
                .Replace("[FT]", program.Start.ToString("yyyyMMddHHmmss"))
                .Replace("[TO]", program.End.ToString("yyyyMMddHHmmss"));
            using (var handler = new HttpClientHandler() {UseCookies = true})
            {
                using (var client = new HttpClient(handler))
                {
                    if (_cookieContainer != null)
                    {
                        handler.CookieContainer = _cookieContainer;
                    }
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Headers.Add("pragma", "no-cache");
                    request.Headers.Add("X-Radiko-AuthToken", token);
                    var res = await client.SendAsync(request);
                    var text = await res.Content.ReadAsStringAsync();

                    foreach (var line in text.Split("\n"))
                    {
                        if (!line.Contains("http")) continue;
                        m3U8 = line;
                        break;
                    }
                }
            }

            return m3U8;

        }

    }
}
