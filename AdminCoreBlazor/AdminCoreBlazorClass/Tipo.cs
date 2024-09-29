using AdminCoreBlazorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminCoreBlazorClass
{
    public class Tipo : Entity<int>
    {
        public string TipoAttivita { get; set; } = string.Empty;
        public string ColoreHTML { get; set; } = string.Empty;
        public string Tipologia { get; set; } = string.Empty;
        public string ColoreAzione { get; set; } = string.Empty;
        public virtual ICollection<Attivita>? Attivita { get; set; }
    }
}
