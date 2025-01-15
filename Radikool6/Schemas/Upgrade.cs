using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Radikool6.Classes;

namespace Radikool6.Schemas
{
    public class Upgrade
    {
        private class Table
        {
            public Dictionary<string, string> Columns { get; set; }
            public string[] PrimaryKey { get; set; }
        }


        public static void Execute()
        {
            if (!File.Exists(Define.File.DbFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Define.File.DbFile));
            }

            var tables =
                JsonConvert.DeserializeObject<Dictionary<string, Table>>(
                    File.ReadAllText("Schemas/schema.json"));

            using var con = new SqliteConnection($"Data Source={Define.File.DbFile}");
            con.Open();
            using var trn = con.BeginTransaction();
            // 最初に管理用テーブルを作成する
            CreateTable(trn, "Hashes", tables["Hashes"], false);

            // その他のテーブルを作成する
            foreach (var (tableName, table) in tables.Where(t => t.Key != "Hashes"))
            {
                CreateTable(trn, tableName, table);
            }

            trn.Commit();

        }


        private static void CreateTable(SqliteTransaction trn, string tableName, Table table, bool check = true)
        {
            var createQuery =
                $"CREATE TABLE IF NOT EXISTS {tableName}({string.Join(",", table.Columns.Select(c => c.Key + " " + c.Value))}{(table.PrimaryKey?.Length > 0 ? " ,PRIMARY KEY(" + string.Join(",", table.PrimaryKey) + ")" : "")})";

            var hash = Utility.Text.Sha256(createQuery);
            var orgData = new List<Dictionary<string, object>>();
            if (check)
            {
                // ハッシュチェック
                var count = 0;
                using (var cmd = new SqliteCommand("", trn.Connection, trn))
                {
                    cmd.CommandText =
                        "SELECT COUNT(TableName) FROM Hashes WHERE TableName = @table_name AND Hash = @hash";
                    cmd.Parameters.Add(new SqliteParameter("table_name", tableName));
                    cmd.Parameters.Add(new SqliteParameter("hash", hash));

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count != 1)
                {
                    // 既存データ退避
                    try
                    {
                        using (var cmd = new SqliteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = $"SELECT * FROM {tableName}";
                            using var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();

                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                orgData.Add(row);
                            }
                        }

                        // 既存テーブル削除
                        using (var cmd = new SqliteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = $"DROP TABLE {tableName}";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {

                    }

                    // ハッシュ登録
                    using (var cmd = new SqliteCommand("", trn.Connection, trn))
                    {
                        cmd.CommandText = @"REPLACE INTO 
                                                Hashes 
                                            (
                                                TableName, 
                                                Hash 
                                            ) 
                                            VALUES 
                                            ( 
                                                @table_name,
                                                @hash 
                                             )";
                        cmd.Parameters.Add(new SqliteParameter("table_name", tableName));
                        cmd.Parameters.Add(new SqliteParameter("hash", hash));
                        cmd.ExecuteNonQuery();
                    }

                }

            }

            using (var cmd = new SqliteCommand("", trn.Connection, trn))
            {
                cmd.CommandText = createQuery;
                cmd.ExecuteNonQuery();
            }

            /*
                if (table.Index != null)
                {
                    for (int i = 0; i < table.Index.Length; i++)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                        {
                            //create index nameindex on user(name);
                            cmd.CommandText = "CREATE INDEX IF NOT EXISTS " + table.Name + "_index_" + i + " ON " + table.Name + " (" + table.Index[i] + ")";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }*/

            // 既存データ移送
            if (orgData.Count <= 0) return;

            
            var data = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            using (var cmd = new SqliteCommand("", trn.Connection, trn))
            {
                cmd.CommandText = $"SELECT * FROM {tableName} LIMIT 1";
                using var reader = cmd.ExecuteReader();
                reader.Read();


                foreach (var orgRow in orgData)
                {
                    var row = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var colName = reader.GetName(i);
                        if (!orgRow.ContainsKey(colName)) continue;
                        row[colName] = orgRow[colName];
                        if (!cols.Contains(colName))
                        {
                            cols.Add(colName);
                        }
                    }
                    data.Add(row);
                }

            }

            foreach (var row in data)
            {
                using var cmd = new SqliteCommand("", trn.Connection, trn);
                cmd.CommandText = $@"INSERT INTO
                                             {tableName}
                                         ( 
                                             {string.Join(",", cols.OrderBy(c => c))}
                                         )
                                         VALUES 
                                         (
                                             {string.Join(",", cols.OrderBy(c => c).Select(c => "@" + c))}
                                         ) ";
                cols.ForEach(c => { cmd.Parameters.Add(new SqliteParameter(c, row[c])); });

                cmd.ExecuteNonQuery();
            }
            

        }
    }
}