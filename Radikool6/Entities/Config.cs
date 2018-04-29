using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    public class Config
    {
        public string Id { get; set; }
        public string Content { get; set; }
    }
}