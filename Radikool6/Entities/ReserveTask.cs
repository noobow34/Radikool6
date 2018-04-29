using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radikool6.Entities
{
    public class ReserveTask
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ReserveId { get; set; }
        public Reserve Reserve { get; set; }

        public Station Station { get; set; }
        public string Status { get; set; }
        public bool IsTimeFree { get; set; }

    }
}