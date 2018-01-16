using Radikool6.Classes;
using Radikool6.Entities;
using System.Collections.Generic;
using System.Linq;
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
                var res = new List<Entities.Program>();
                var doc = XDocument.Load(Define.Radiko.WeeklyTimeTable.Replace("[stationCode]", station.Code));

                foreach (var prog in doc.Descendants("prog"))
                {
                    res.Add(new Entities.Program()
                    {
                        Id = station.Code + prog.Attribute("ft")?.Value,
                        Start = Utility.Text.StringToDate(prog.Attribute("ft")?.Value),
                        End = Utility.Text.StringToDate(prog.Attribute("to")?.Value),
                        Title = prog.Element("title")?.Value.Trim(),
                        Cast = prog.Element("pfm")?.Value.Trim(),
                        Description = prog.Element("info")?.Value.Trim(),
                        StationId = station.Id
                    });
                }

                return res;

            });
        }
    }
}
