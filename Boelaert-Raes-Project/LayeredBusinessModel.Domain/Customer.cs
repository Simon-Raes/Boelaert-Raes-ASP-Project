using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Customer
    {
        public int customer_id { get; set; }
        public String name { get; set; }
        public String email { get; set; }
        public String password { get; set; }

        public int numberOfVisits { get; set; }
    }
}
