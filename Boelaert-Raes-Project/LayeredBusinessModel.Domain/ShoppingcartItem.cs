using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     * Represents a shoppingcartitem
     */ 
    public class ShoppingcartItem
    {
        public int shoppingcart_item_id { get; set; }   //a unique id  
        public Customer customer { get; set; }
        public DvdInfo dvdInfo { get; set; }
        public DateTime startdate { get; set; }         //start date from order
        public DateTime enddate { get; set; }           //end date from order
        public DvdCopy dvdCopy { get; set; }
    }
}
