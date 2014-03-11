using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class DvdCopy
    {
        public int dvd_copy_id { get; set; }
        public int dvd_info_id { get; set; }
        public int copy_type_id { get; set; }
        public String serialnumber { get; set; }
        public String note { get; set; }
    }
}
