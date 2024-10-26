using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace API.Models
{
    public class AttivitaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<Attivita>? Attivita { get; set; }
    }

    public class Attivita
    {
        public int ID { get; set; }
        public int Giorno { get; set; }
        public string? Dettagli { get; set; }
        public decimal Costo { get; set; }
        public int Anno { get; set; }
        public int Mese { get; set; }
        public string? Descrizione { get; set; }
        public string? TipoAttivita { get; set; }
        public string? ColoreAzione { get; set; }
        public string? ColoreHTML { get; set; }
        public string? Tipologia { get; set; }
    }

    public class PeriodoOUT
    {
        public int ID { get; set; }
        public string? Descrizione { get; set; }
    }

    public class AggregatoAttivitaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<AggregatoAttivita>? AggregatoAttivita { get; set; }
    }

    public class AggregatoAttivita
    {
        public string? TipoAttivita { get; set; }
        public string? ColoreHTML { get; set; }
        public string? Descrizione { get; set; }
        public decimal TotAtt { get; set; }
    }

    public class DatiSuntoOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<DatiSunto>? DatiSunto { get; set; }
    }

    public class DatiSunto
    {
        public string? Tipo { get; set; }
        public decimal Tot { get; set; }
    }

    public class TipoAttivitaOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
        public List<TipoAttivita>? TipoAttivita { get; set; }
    }

    public class TipoAttivita
    {
        public int ID { get; set; }
        public string? Tipo { get; set; }
    }

    public class GenericOUT
    {
        public String? Status { get; set; }
        public String? StatusError { get; set; }
    }

    public class AttivitaIN
    {
        [Display(Name = "Giorno")]
        [Required]
        public int Giorno { get; set; }
        [Required]
        public int RifPeriodo { get; set; }
        [Display(Name = "Tipo Attivita")]
        [Required]
        public int RifTipoAttivita { get; set; }
        public string? Dettagli { get; set; }
        [Display(Name = "Valore")]
        [Required]
        public decimal Costo { get; set; }
    }
}
