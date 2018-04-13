using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    [Table("reserve_tasks")]
    public class ReserveTask
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("start")]
        public DateTime Start { get; set; }

        [Column("end")]
        public DateTime End { get; set; }

        [ForeignKey("Reserve")]
        [Column("reserve_id")]
        public string ReserveId { get; set; }

        public Reserve Reserve { get; set; }

        [NotMapped]
        public Station Station { get; set; }
        
        [NotMapped]
        public string Status { get; set; }

    }
}