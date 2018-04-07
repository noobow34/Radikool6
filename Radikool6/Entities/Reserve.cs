using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Radikool6.Entities
{
    [Table("reserves")]
    public class Reserve
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [JsonIgnore]
        [Column("content")]
        public string Content
        {
            get => JsonConvert.SerializeObject(this);
            set
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<Reserve>(value);
                    foreach (var info in typeof(Reserve).GetProperties())
                    {
                        if (info.GetCustomAttributes(true).Any(attr => attr.GetType() == typeof(NotMappedAttribute)))
                        {
                            info.SetValue(this, info.GetValue(data));
                        }
                    }
                }
                catch (Exception e)
                {
                    var a = e.Message;
                }
            }
        }
        
        [NotMapped]
        public string Name { get; set; }
        
        [NotMapped]
        public string StationId { get; set; }
        
        [NotMapped]
        public string StationName { get; set; }
        
        [NotMapped]
        public DateTime Start { get; set; }
        
        [NotMapped]
        public DateTime End { get; set; }
        
        [NotMapped]
        public string FormatId { get; set; }

        [NotMapped]
        public List<string> Repeat { get; set; } = new List<string>();

        [NotMapped]
        public bool Enabled { get; set; } = true;
        
        [JsonIgnore]
        public ICollection<ReserveTask> ReserveTasks { get; set; }

    }
}