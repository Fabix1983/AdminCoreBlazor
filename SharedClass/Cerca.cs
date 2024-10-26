using System;
using System.Collections.Generic;

namespace API.Models
{
    public class CercaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Cerca>? Cerca { get; set; }
    }

    public class Cerca
    {
        public DateTime DataSpesa { get; set; }
        public string? Dettagli { get; set; }
        public string? TipoAttivita { get; set; }
        public string? ColoreHTML { get; set; }
        public string? Tipologia { get; set; }
        public string? Descrizione { get; set; }
        public decimal Costo { get; set; }
    }

}
