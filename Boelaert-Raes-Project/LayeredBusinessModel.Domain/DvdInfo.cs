using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class DvdInfo
    {
        public int dvd_info_id { get; set; }
        public String name { get; set; }
        public String code { get; set; }
        public String author { get; set; }
        public String image { get; set; }
    }
}
