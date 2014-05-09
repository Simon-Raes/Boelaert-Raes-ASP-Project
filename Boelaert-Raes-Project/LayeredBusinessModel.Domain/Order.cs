using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     *  Represents an order
     */ 
    public class Order
    {
        public int order_id { get; set; }               //a unique order id
        public Customer customer { get; set; }          //the customer
        public OrderStatus orderstatus { get; set; }    //orderstatus
        public DateTime date { get; set; }              //date
    }
}
