using System;
using System.Collections.Generic;

namespace API.Models
{
    public class CompareOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Compare>? Compare { get; set; }
    }

    public class Compare
    {  
        public string? TipoAttivita { get; set; }
        public string? ColoreHTML { get; set; }
        public string? Descrizione { get; set; }
        public decimal TotAtt { get; set; }      
    }
}
