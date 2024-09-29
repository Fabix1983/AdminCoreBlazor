using AdminCoreBlazorShared;

namespace AdminCoreBlazorClass
{
    public class Attivita : Entity<int>
    {
        public int Giorno { get; set; }
        public string Dettagli { get; set; } = string.Empty;
        public decimal Costo { get; set; }

        public int RifPeriodo { get; set; }
        public virtual Periodo? Periodo { get; set; }
        

        public Guid RifTipoAttivita { get; set; }
        public virtual Tipo? Tipo { get; set; }

    }
}
