using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Shared.Class
{
    public class PrevisioneOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Previsione>? Previsione { get; set; }
    }

    public class Previsione
    {
        public int ID { get; set; }
        public int Giorno { get; set; }
        public string? Descrizione { get; set; }
        public decimal Costo { get; set; }
    }

    public class PrevisioneIN
    {
        [Display(Name = "Giorno")]
        [Required]
        public int Giorno { get; set; }
        [Required]
        public int RifPeriodo { get; set; }
        [Display(Name = "Descrizione")]
        [Required]
        public string? Descrizione { get; set; }
        [Display(Name = "Valore")]
        [Required]
        public decimal Costo { get; set; }
    }

    public class PrevisioneTotaleListaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<PrevisioneTotale>? Previsione { get; set; }
    }

    public class PrevisioneTotale
    {
        public int ID { get; set; }
        public int Giorno { get; set; }
        public string? Descrizione { get; set; }
        public decimal Costo { get; set; }
        public string? DescrizionePeriodo { get; set; }
    }
}