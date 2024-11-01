using System.Collections.Generic;
using System;

namespace Shared.Class
{
    public class TrendOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Trend>? Trend { get; set; }
    }

    public class Trend
    {
        public int ID { get; set; }
        public string? Descrizione { get; set; }
        public decimal Bilancio { get; set; }
    }
}
