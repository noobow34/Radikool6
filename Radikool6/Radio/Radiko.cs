using Radikool6.Classes;
using Radikool6.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Radikool6.Radio
{
    public class Radiko
    {
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
    }
}
