using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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