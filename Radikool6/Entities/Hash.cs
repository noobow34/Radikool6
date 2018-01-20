using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    [Table("hashes")]
    public class Hash
    {
        [Key]
        [Column("table_name")]
        public string TableName { get; set; }
        
        [Column("hash")]
        public string HashValue { get; set; }
    }
}