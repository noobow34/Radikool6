using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

            using (var con = new SQLiteConnection($"Data Source={Define.File.DbFile}"))
            {
                con.Open();
                using (var trn = con.BeginTransaction())
                {
                    // 最初に管理用テーブルを作成する
                    CreateTable(trn, "hashes", tables["hashes"], false);

                    // その他のテーブルを作成する
                    foreach (var(tableName, table) in tables.Where(t => t.Key != "hashes"))
                    {
                        CreateTable(trn, tableName, table);
                    }

                    trn.Commit();
                }

            }

          

        }


        private static void CreateTable(SQLiteTransaction trn, string tableName, Table table, bool check = true)
        {
            var createQuery =
                $"CREATE TABLE IF NOT EXISTS {tableName}({string.Join(",", table.Columns.Select(c => c.Key + " " + c.Value))}{(table.PrimaryKey?.Length > 0 ? " ,PRIMARY KEY(" + string.Join(",", table.PrimaryKey) + ")" : "")})";

            var hash = Utility.Text.Sha256(createQuery);
            var orgData = new DataTable();
            if (check)
            {
                // ハッシュチェック
                var count = 0;
                using (var cmd = new SQLiteCommand("", trn.Connection, trn))
                {
                    cmd.CommandText =
                        "SELECT COUNT(table_name) FROM hashes WHERE table_name = @table_name AND hash = @hash";
                    cmd.Parameters.Add(new SQLiteParameter("table_name", tableName));
                    cmd.Parameters.Add(new SQLiteParameter("hash", hash));

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (count != 1)
                {
                    // 既存データ退避
                    try
                    {
                        using (var cmd = new SQLiteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = $"SELECT * FROM {tableName}";
                            using (var da = new SQLiteDataAdapter(cmd))
                            {
                                da.Fill(orgData);
                            }
                        }

                        // 既存テーブル削除
                        using (var cmd = new SQLiteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = $"DROP TABLE {tableName}";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    // ハッシュ登録
                    using (var cmd = new SQLiteCommand("", trn.Connection, trn))
                    {
                        cmd.CommandText = @"REPLACE INTO 
                                                hashes 
                                            (
                                                table_name, 
                                                hash 
                                            ) 
                                            VALUES 
                                            ( 
                                                @table_name,
                                                @hash 
                                             )";
                        cmd.Parameters.Add(new SQLiteParameter("table_name", tableName));
                        cmd.Parameters.Add(new SQLiteParameter("hash", hash));
                        cmd.ExecuteNonQuery();
                    }

                }

            }

            using (var cmd = new SQLiteCommand("", trn.Connection, trn))
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
            if (orgData.Rows.Count <= 0) return;

            var data = new DataTable();
            using (var cmd = new SQLiteCommand("", trn.Connection, trn))
            {
                cmd.CommandText = $"SELECT * FROM {tableName}";
                using (var da = new SQLiteDataAdapter(cmd))
                {
                    da.FillSchema(data, SchemaType.Mapped);
                }

                foreach (DataRow row in orgData.Rows)
                {
                    var add = data.NewRow();

                    foreach (DataColumn col in orgData.Columns)
                    {
                        if (data.Columns.Contains(col.ColumnName))
                        {
                            add[col.ColumnName] = row[col.ColumnName];
                        }
                    }

                    data.Rows.Add(add);
                }
            }

            var cols = new List<string>();
            foreach (DataColumn col in data.Columns)
            {
                cols.Add(col.ColumnName);
            }

            foreach (DataRow row in data.Rows)
            {
                using (var cmd = new SQLiteCommand("", trn.Connection, trn))
                {
                    cmd.CommandText = $@"INSERT INTO
                                             {tableName}
                                         ( 
                                             {string.Join(",", cols.OrderBy(c => c))}
                                         )
                                         VALUES 
                                         (
                                             {string.Join(",", cols.OrderBy(c => c).Select(c => "@" + c))}
                                         ) ";
                    cols.ForEach(c => { cmd.Parameters.Add(new SQLiteParameter(c, row[c])); });

                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}