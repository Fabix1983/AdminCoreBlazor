﻿using System;
using System.Collections.Generic;

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
        public List<DatiSunto>? DatiSunto { get; set; }
    }

    public class DatiSunto
    {
        public string? Tipo { get; set; }
        public decimal Tot { get; set; }
    }

    public class TipoAttivitaOUT
    {
        public List<TipoAttivita>? TipoAttivita { get; set; }
    }

    public class TipoAttivita
    {
        public int ID { get; set; }
        public string? Tipo { get; set; }
    }
}