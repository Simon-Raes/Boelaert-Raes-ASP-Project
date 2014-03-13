using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.DAO
{
    public class OrderLine
    {
        public int orderline_id { get; set; }
        public int order_id { get; set; }
        public int order_line_type_id { get; set; }
        public int dvd_copy_id { get; set; }
        public DateTime startdate { get; set; } //in case of rent (or reservation?)
        public DateTime enddate { get; set; } //in case of rent (or reservation?)
    }
}
