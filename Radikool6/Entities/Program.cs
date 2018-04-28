using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Radikool6.Entities
{
    public class Program
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("Station")]
        public string StationId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Title { get; set; }

        public string Cast { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public Station Station { get; set; }
    }
}
