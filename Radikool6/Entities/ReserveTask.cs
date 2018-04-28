using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    public class ReserveTask
    {
        [Key]
        public string Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [ForeignKey("Reserve")]
        public string ReserveId { get; set; }

        public Reserve Reserve { get; set; }

        [NotMapped]
        public Station Station { get; set; }
        
        [NotMapped]
        public string Status { get; set; }

    }
}