using AdminCoreBlazorShared;

namespace AdminCoreBlazorClass
{
    public class Attivita : Entity<int>
    {
        public int Giorno { get; set; }
        public int RifPeriodo { get; set; }
        public int RifTipoAttivita { get; set; }
        public string Dettagli { get; set; } = string.Empty;
        public decimal Costo { get; set; }
}

    public class PeriodoAttivita
    {
        public int ID { get; set; }
        public int Mese { get; set; }
        public int Anno { get; set; }
        public string Descrizione { get; set; } = string.Empty;
    }

    public class TipologiaAttivita
    {
        public int ID { get; set; }
        public string TipoAttivita { get; set; } = string.Empty;
        public string ColoreHTML { get; set; } = string.Empty;
        public string Tipologia { get; set; } = string.Empty;
        public string ColoreAzione { get; set; } = string.Empty;
    }
}
