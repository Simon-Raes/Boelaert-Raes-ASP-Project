using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class PageVisits
    {
        public int customer_id { get; set; }
        public int dvd_info_id { get; set; }
        public int number_of_visits { get; set; }
    }
}
