using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Beer
    {
        public int beernr { get; set; }
        public String naam { get; set; }
        public int brouwernr { get; set; }
        public int soortnr { get; set; }
        public double alcohol { get; set; }
    }
}
