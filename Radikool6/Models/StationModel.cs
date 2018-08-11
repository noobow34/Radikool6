using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class StationModel : BaseModel
    {
        public StationModel(SqliteConnection con) : base(con)
        {
        }

        /// <summary>
        /// 放送局IDで放送局取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Station GetById(string id)
        {
            return SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id = @id", new {id = id})
                .FirstOrDefault();
        }

        /// <summary>
        /// 種別で放送局取得
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public Dictionary<string, List<Station>> Get(params string[] types)
        {
            return SqliteConnection.Query<Station>(
                "SELECT * FROM Stations WHERE Type IN @types ORDER BY Type,Sequence ",
                new {types = types}).GroupBy(s => s.Type).ToDictionary(s => s.Key, s => s.ToList());
        }

        /// <summary>
        /// 種別ごと完全更新
        /// </summary>
        /// <param name="stations"></param>
        public void Refresh(List<Station> stations)
        {
            using (var trn = SqliteConnection.BeginTransaction())
            {
                // 種別が同じものを削除
                SqliteConnection.Execute("DELETE FROM Stations WHERE Type IN @types",
                    new {types = stations.Select(s => s.Type).Distinct().ToList()}, trn);
                const string query = @"INSERT INTO 
                                           Stations
                                       (
                                           Id,
                                           RegionId,
                                           RegionName,
                                           Area,
                                           Type,
                                           Code,
                                           Name,
                                           Sequence,
                                           MediaUrl,
                                           TimetableUrl
                                       )
                                       VALUES
                                       (
                                            @Id,
                                            @RegionId,
                                            @RegionName,
                                            @Area,
                                            @Type,
                                            @Code,
                                            @Name,
                                            @Sequence,
                                            @MediaUrl,
                                            @TimetableUrl
                                       )";
                stations.ForEach(s =>
                {
                    try
                    {

                        SqliteConnection.Execute(query, s, trn);
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                    }
                });

                trn.Commit();
            }

        }
    }
}