using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     * Represents a token
     */ 
    public class Token
    {
        public TokenStatus status { get; set; }     //status
        public Customer customer { get; set; }      //customer
        public String token { get; set; }           //token
        public DateTime timestamp { get; set; }     //timestamp
    }
}
