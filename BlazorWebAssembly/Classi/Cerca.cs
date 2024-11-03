using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Class
{

    public class Cerca
    {
        public DateTime DataSpesa { get; set; }
        public string? Dettagli { get; set; }
        public string? TipoAttivita { get; set; }
        public string? ColoreHTML { get; set; }
        public string? Tipologia { get; set; }
        public string? Descrizione { get; set; }
        public double Costo { get; set; }
    }

    public class CercaOUT
    {
        public string? Status { get; set; }
        public string? StatusError { get; set; }
        public List<Cerca>? Cerca { get; set; }
    }

}
