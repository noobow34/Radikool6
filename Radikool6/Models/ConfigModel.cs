using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Microsoft.Data.Edm;
using Newtonsoft.Json;
using Radikool6.Classes;
using Radikool6.Entities;

namespace Radikool6.Models
{
    public class ConfigModel : BaseModel
    {
        public ConfigModel(Db db) : base(db)
        {
        }

        /// <summary>
        /// 設定取得
        /// </summary>
        /// <returns></returns>
        public CommonConfig Get()
        {
            var config = Db.Configs.Find(Define.Config.Common);
            if (config == null) return null;

            var res = JsonConvert.DeserializeObject<CommonConfig>(config.Content);
            
            res.RadikoEmail = Utility.Text.Decrypt(res.RadikoEmail, Global.EncKey);
            res.RadikoPassword = Utility.Text.Decrypt(res.RadikoPassword, Global.EncKey);

            return res;
        }

        public bool Update(CommonConfig commonConfig)
        {
            commonConfig.RadikoEmail = Utility.Text.Encrypt(commonConfig.RadikoEmail, Global.EncKey);
            commonConfig.RadikoPassword = Utility.Text.Encrypt(commonConfig.RadikoPassword, Global.EncKey);
            
            
            var orgConfig = Db.Configs.Find(Define.Config.Common);
            if (orgConfig != null)
            {
                orgConfig.Content = JsonConvert.SerializeObject(commonConfig);
            }
            else
            {
                Db.Configs.Add(new Config()
                {
                    Id = Define.Config.Common,
                    Content = JsonConvert.SerializeObject(commonConfig)
                });
            }

            Db.SaveChanges();
            return true;
        }
        
        
        public List<EncodeSetting> GetEncodeSettings()
        {
            var config = Db.Configs.Find(Define.Config.EncodeSetting);
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
            var json = JsonConvert.SerializeObject(res);
            Db.Configs.Add(new Config
            {
                Id = Define.Config.EncodeSetting,
                Content = json
            });
            Db.SaveChanges();
            
            return res;
        }
    }
}