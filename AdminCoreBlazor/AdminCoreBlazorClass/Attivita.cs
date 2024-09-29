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

        public int Tipo_Id { get; set; }
        public virtual Tipo? Tipo { get; set; }

        public Guid Perido_Id { get; set; }
        public virtual Periodo? Periodo { get; set; }
    }
}
