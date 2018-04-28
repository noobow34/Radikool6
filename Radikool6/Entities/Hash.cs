using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    public class Hash
    {
        [Key]
        public string TableName { get; set; }
        public string HashValue { get; set; }
    }
}