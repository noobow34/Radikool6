using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Schemas
{
    public class Upgrade
    {
        private class Table
        {
            public Dictionary<string, string>Columns { get; set; }
            public string[] PrimaryKey { get; set; }
        }
        
        
        public static void Execute()
        {
            if (!File.Exists(Define.File.DbFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Define.File.DbFile));
            }
            
            
            using (var db = new Db())
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    var tables =
                        JsonConvert.DeserializeObject<Dictionary<string, Table>>(
                            File.ReadAllText("Schemas/schema.json"));
                    
                    // 最初に管理用テーブルを作成する
                    CreateTable("hashes", tables["hashes"], db, false);
                    
                    // その他のテーブルを作成する
                    foreach (var(tableName, table) in tables.Where(t => t.Key != "hashes"))
                    {
                        CreateTable(tableName, table, db);
                    }
                    trn.Commit();
                }


            }
        }
        
        
        private static void CreateTable(string tableName, Table table, Db db, bool check = true)
        {
           
                var createQuery =
                    $"CREATE TABLE IF NOT EXISTS {tableName}({string.Join(",", table.Columns.Select(c => c.Key + " " + c.Value))}{(table.PrimaryKey?.Length > 0 ? " ,PRIMARY KEY(" + string.Join(",", table.PrimaryKey) + ")": "")})";

            var hash = Utility.Text.Sha256(createQuery);

            Hash hashTable = null;
            if (check)
            {             
                hashTable = db.Hashes.Find(tableName);
                if (hashTable == null)
                {
                    // 新規作成
                    db.Database.ExecuteSqlCommand(createQuery);
                }
                else if(hashTable.HashValue != hash)
                {
                    // 更新

                }
            }
            else
            {
                db.Database.ExecuteSqlCommand(createQuery);
                hashTable = db.Hashes.Find(tableName);
            }

            if (hashTable != null)
            {
                hashTable.HashValue = hash;
            }
            else
            {
                hashTable = new Hash(){ TableName =  tableName, HashValue =  hash};
                db.Hashes.Add(hashTable);
            }

            db.SaveChanges();





/*
            TableAttribute table = Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute)) as TableAttribute;
            List<ColumnAttribute> columns = new List<ColumnAttribute>();

            DataTable org_data = new DataTable();
            if (table != null)
            {
                foreach (var p in typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    var attr = p.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
                    if (attr != null)
                    {
                        columns.Add(attr as ColumnAttribute);
                    }
                }

                string create_query = "CREATE TABLE IF NOT EXISTS " + table.Name + "(" + string.Join(",", columns.Where(c => c.Create).Select(c => c.Name + " " + c.Type).ToArray()) + (!string.IsNullOrWhiteSpace(table.PrimaryKey) ? " ,PRIMARY KEY(" + table.PrimaryKey + ")" : "") + ")";
                if (check)
                {
                    // ハッシュチェック
                    string hash = Utility.Text.Md5(create_query);
                    int count = 0;
                    using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                    {
                        cmd.CommandText = "SELECT COUNT(TableName) FROM TableHashes WHERE TableName = @table_name AND Hash = @hash";
                        cmd.Parameters.Add(new SQLiteParameter("table_name", table.Name));
                        cmd.Parameters.Add(new SQLiteParameter("hash", hash));

                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (count != 1)
                    {
                        // 既存データ退避
                        try
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                            {
                                cmd.CommandText = "SELECT * FROM " + table.Name;
                                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                                {
                                    da.Fill(org_data);
                                }
                            }

                            // 既存テーブル削除
                            using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                            {
                                cmd.CommandText = "DROP TABLE " + table.Name;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        // ハッシュ登録
                        using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = "REPLACE INTO "
                                            + "    TableHashes "
                                            + "( "
                                            + "    TableName, "
                                            + "    Hash "
                                            + ") "
                                            + "VALUES "
                                            + "( "
                                            + "    @table_name, "
                                            + "    @hash "
                                            + " )";
                            cmd.Parameters.Add(new SQLiteParameter("table_name", table.Name));
                            cmd.Parameters.Add(new SQLiteParameter("hash", hash));
                            cmd.ExecuteNonQuery();
                        }

                    }

                }

                using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                {
                    cmd.CommandText = create_query;
                    cmd.ExecuteNonQuery();
                }

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
                }

                // 既存データ移送
                if (org_data.Rows.Count > 0)
                {
                    DataTable data = new DataTable();
                    using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                    {
                        cmd.CommandText = "SELECT * FROM " + table.Name;
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                        {
                            da.FillSchema(data, SchemaType.Mapped);
                        }

                        foreach (DataRow row in org_data.Rows)
                        {
                            DataRow add = data.NewRow();

                            foreach (DataColumn col in org_data.Columns)
                            {
                                if (data.Columns.Contains(col.ColumnName))
                                {
                                    add[col.ColumnName] = row[col.ColumnName];
                                }
                            }
                            data.Rows.Add(add);
                        }
                    }

                    List<string> cols = new List<string>();
                    foreach (DataColumn col in data.Columns)
                    {
                        cols.Add(col.ColumnName);
                    }
                    foreach (DataRow row in data.Rows)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand("", trn.Connection, trn))
                        {
                            cmd.CommandText = "INSERT INTO "
                                            + "    " + table.Name
                                            + "( "
                                            + string.Join(",", cols.OrderBy(c => c).ToArray())
                                            + ") "
                                            + "VALUES "
                                            + "( "
                                            + string.Join(",", cols.OrderBy(c => c).Select(c => "@" + c).ToArray())
                                            + ") ";
                            cols.ForEach(c =>
                            {
                                cmd.Parameters.Add(new SQLiteParameter(c, row[c]));
                            });

                            cmd.ExecuteNonQuery();
                        }
                    }


                }

            }*/
        }
    }
}