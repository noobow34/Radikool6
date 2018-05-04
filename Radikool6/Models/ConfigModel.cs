using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Dapper;
using Microsoft.Data.Edm;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ConfigModel : BaseModel
    {
        public ConfigModel(SqliteConnection con) : base(con)
        {
        }

        /// <summary>
        /// 設定取得
        /// </summary>
        /// <returns></returns>
        public Config Get()
        {
            var json = SqliteConnection
                .Query<string>("SELECT Content FROM Configs WHERE Id = @id", new {id = Define.Config.Common})
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(json)) return new Config();

            var res = JsonConvert.DeserializeObject<Config>(json);

            res.RadikoEmail = Utility.Text.Decrypt(res.RadikoEmail, Global.EncKey);
            res.RadikoPassword = Utility.Text.Decrypt(res.RadikoPassword, Global.EncKey);

            return res;
        }

        /// <summary>
        /// 一般設定更新
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Update(Config config)
        {
            config.RadikoEmail = Utility.Text.Encrypt(config.RadikoEmail, Global.EncKey);
            config.RadikoPassword = Utility.Text.Encrypt(config.RadikoPassword, Global.EncKey);

            const string query = @"REPLACE INTO 
                                       Configs
                                   (
                                       Id,
                                       Content
                                   )
                                   VALUES
                                   (
                                       @Id,
                                       @Content
                                   )";
            SqliteConnection.Execute(query,
                new {Id = Define.Config.Common, Content = JsonConvert.SerializeObject(config)});
            return true;
        }

    }
}