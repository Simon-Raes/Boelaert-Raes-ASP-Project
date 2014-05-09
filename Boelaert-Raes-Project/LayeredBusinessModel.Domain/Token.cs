using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Token
    {
        public TokenStatus status { get; set; }
        public Customer customer { get; set; }
        public String token { get; set; }
        public DateTime timestamp { get; set; }
    }
}
