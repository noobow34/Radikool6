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
        
    }
}