using AdminCoreBlazorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCoreBlazorClass
{
    public class Periodo : Entity<int>
    {
        public int Mese { get; set; }
        public int Anno { get; set; }
        public string Descrizione { get; set; } = string.Empty;
        public virtual ICollection<Attivita>? Attivita { get; set; }
    }
}
