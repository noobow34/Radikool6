using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Radikool6.Entities
{
    public class Program
    {
        public string Id { get; set; }
        public string StationId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Cast { get; set; }
        public string Description { get; set; }
        public string TsNg { get; set; }

      //  [JsonIgnore]
        public Station Station { get; set; }
    }
}
