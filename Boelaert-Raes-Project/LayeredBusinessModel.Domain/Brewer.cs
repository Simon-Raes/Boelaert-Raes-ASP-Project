using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Brewer
    {
        public int brouwernr { get; set; }
        public String naam { get; set; }
        public String adres { get; set; }
        public String postcode { get; set; }
        public String gemeente { get; set; }
        public String omzet { get; set; }
    }
}
