using System.Collections.Generic;
using System;

namespace Shared.Class
{
    public class CategoriaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Categoria>? Categoria { get; set; }
    }

    public class Categoria
    {
        public string? Descrizione { get; set; }
        public string? TipoAttivita { get; set; }
        public decimal Valore { get; set; }
        public decimal Media { get; set; }
        public decimal TotaleMese { get; set; }
        public decimal PercentualeSulTotale { get; set; }
        public string? Tipologia { get; set; }
        public string? ColoreHTML { get; set; }
        public int Anno { get; set; }
        public int Mese { get; set; }
    }
}
