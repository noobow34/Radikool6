using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Radikool6.Entities
{
    [Table("stations")]
    public class Station
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public List<Program> Programs { get; set; } = new List<Program>();

    }
}
