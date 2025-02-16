using Dapper;
using Microsoft.Data.Sqlite;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ProgramModel : BaseModel
    {
        public ProgramModel(SqliteConnection con) : base(con)
        {
        }

        /// <summary>
        /// 番組情報取得
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Entities.Program Get(string programId)
        {
            var res = SqliteConnection
                .Query<Entities.Program>("SELECT * FROM Programs WHERE Id = @id", new {id = programId})
                .FirstOrDefault();
            if (res != null)
            {
                res.Station = SqliteConnection.Query<Station>("SELECT * FROM Stations WHERE Id = @id", new
                {
                    id = res.StationId
                }).FirstOrDefault();
            }

            return res;
        }

        /// <summary>
        /// 番組検索
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public List<Entities.Program> Search(ProgramSearchCondition cond)
        {
            var wheres = new List<string>();

            if (!string.IsNullOrWhiteSpace(cond.StationId))
            {
                wheres.Add("StationId = @StationId");
            }

            if (cond.From != null)
            {
                wheres.Add("End > @From");
            }

            if (cond.To != null)
            {
                wheres.Add("Start < @To");
            }

            if (!string.IsNullOrWhiteSpace(cond.Keyword))
            {
                wheres.Add("( Title LIKE @Keyword OR Cast LIKE @Keyword OR Description LIKE @Keyword)");
            }

            var where = wheres.Count != 0 ? $"WHERE {string.Join(" AND ", wheres)}" : "";

            var query = $@"SELECT 
                              * 
                          FROM
                              Programs 
                          {where}
                          ORDER BY StationId, Start";
            var res = SqliteConnection.Query<Entities.Program>(query, cond);

            return res.ToList();
        }

        /// <summary>
        /// データの範囲を取得
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DateTime[] GetRange(string stationId)
        {
            var max = SqliteConnection.Query<DateTime>("SELECT MAX(Start) FROM Programs WHERE StationId = @StationId",
                new
                {
                    StationId = stationId
                }).FirstOrDefault();

            var min = SqliteConnection.Query<DateTime>("SELECT MIN(Start) FROM Programs WHERE StationId = @StationId",
                new
                {
                    StationId = stationId
                }).FirstOrDefault();

            return new[] {min, max};
        }

        /// <summary>
        /// 番組データ更新
        /// </summary>
        /// <param name="programs"></param>
        public void Refresh(List<Entities.Program> programs)
        {
            // 重複削除
            programs = programs.GroupBy(p => p.Id).Select(g => g.First()).ToList();

            using var trn = SqliteConnection.BeginTransaction();
            var stationIds = programs.Select(p => p.StationId).Distinct();
            var delCount = SqliteConnection.Execute("DELETE FROM Programs WHERE StationId IN @StationIds",
                new { StationIds = programs.Select(p => p.StationId).Distinct().ToList() }, trn);

            const string query = @"INSERT INTO 
                                           Programs 
                                       (
                                           Id,
                                           StationId,
                                           Start,
                                           End,
                                           Title,
                                           Cast,
                                           Description,
                                           TsNg
                                       )
                                       VALUES
                                       (
                                           @Id,
                                           @StationId,
                                           @Start,
                                           @End,
                                           @Title,
                                           @Cast,
                                           @Description,
                                           @TsNg
                                       )";

            programs.ForEach(p => {
                SqliteConnection.Execute(query, p, trn); 
            });
            trn.Commit();
        }
    }
}