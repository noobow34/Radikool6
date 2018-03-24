using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    public class Config
    {
        [Key] 
        [Column("id")] 
        public string Id { get; set; }

        [Column("content")]
        public string Content { get; set; }
    }
}