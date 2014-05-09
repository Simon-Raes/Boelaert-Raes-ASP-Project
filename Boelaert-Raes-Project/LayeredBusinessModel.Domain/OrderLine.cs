using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     *  Represents an orderline
     */
    public class OrderLine
    {
        public int orderline_id { get; set; }               //unique orderline id       
        public Order order { get; set; }
        public OrderLineType orderLineType { get; set; }
        public DvdInfo dvdInfo { get; set; }
        public DvdCopy dvdCopy { get; set; }
        public DateTime startdate { get; set; }             //in case of rent (or reservation?)
        public DateTime enddate { get; set; }               //in case of rent (or reservation?)
    }
}
