using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     *  Represents a Customer
     */
    public class Customer
    {
        public int customer_id { get; set; }        //unique id      
        public String email { get; set; }           //emailaddress
        public String password { get; set; }        //password
        public String name { get; set; }            //name
        public String street { get; set; }          //street
        public String zip { get; set; }             //zip
        public String municipality { get; set; }    //municipality    
        public int numberOfVisits { get; set; }     //number of visits
        public Boolean isVerrified { get; set; }    //flag indicating if an account is verrified
    }
}
