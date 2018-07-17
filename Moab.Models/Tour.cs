using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moab.Models
{
    public class Tour
    {
        public string Name { get; set; }
        public ICollection<Tour_Trail> IncludedTrails { get; set; } = new HashSet<Tour_Trail>();
    }
}
