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
        public String year { get; set; }
        public String barcode { get; set; }
        public String author { get; set; }
        
        public String image { get; set; }
        public String descripion { get; set; }
        public float rent_price { get; set; }
        public float buy_price { get; set; }
        public DateTime date_added { get; set; }
        public int amount_sold { get; set; }


        public List<KeyValuePair<int, string>> media { get; set; }
        public String[] actors { get; set; }
        public String duration { get; set; }
        public List<Genre> genres { get; set; }
    }
}
