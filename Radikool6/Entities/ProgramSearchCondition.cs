using System;

namespace Radikool6.Entities
{
    public class ProgramSearchCondition
    {
        public string StationId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Keyword { get; set; }
        public bool Refresh { get; set; } = false;

    }
}