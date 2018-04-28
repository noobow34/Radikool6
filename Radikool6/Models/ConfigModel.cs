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
        /// 一般設定取得
        /// </summary>
        /// <returns></returns>
        public CommonConfig Get()
        {
            var config = SqliteConnection
                .Query<Config>("SELECT * FROM Configs WHERE Id = @id", new {id = Define.Config.Common})
                .FirstOrDefault();
            if (config == null) return null;

            var res = JsonConvert.DeserializeObject<CommonConfig>(config.Content);

            res.RadikoEmail = Utility.Text.Decrypt(res.RadikoEmail, Global.EncKey);
            res.RadikoPassword = Utility.Text.Decrypt(res.RadikoPassword, Global.EncKey);

            return res;
        }

        /// <summary>
        /// 一般設定更新
        /// </summary>
        /// <param name="commonConfig"></param>
        /// <returns></returns>
        public bool Update(CommonConfig commonConfig)
        {
            commonConfig.RadikoEmail = Utility.Text.Encrypt(commonConfig.RadikoEmail, Global.EncKey);
            commonConfig.RadikoPassword = Utility.Text.Encrypt(commonConfig.RadikoPassword, Global.EncKey);

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
                new {Id = Define.Config.Common, Content = JsonConvert.SerializeObject(commonConfig)});
            return true;
        }


        /// <summary>
        /// エンコード設定取得
        /// </summary>
        /// <returns></returns>
        public List<EncodeSetting> GetEncodeSettings()
        {
            var config = SqliteConnection
                .Query<Config>("SELECT * FROM Configs WHERE Id = @id", new {id = Define.Config.EncodeSetting})
                .FirstOrDefault();
            List<EncodeSetting> res = null;
            try
            {
                if (config != null)
                {
                    res = JsonConvert.DeserializeObject<List<EncodeSetting>>(config.Content);
                }
            }
            catch (Exception e)
            {
                res = null;
            }

            if (res != null) return res;
            res = (new Define.EncodeSettings()).Default;
            const string query = @"INSERT INTO 
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
                new {Id = Define.Config.EncodeSetting, Content = JsonConvert.SerializeObject(res)});

            return res;
        }
    }
}