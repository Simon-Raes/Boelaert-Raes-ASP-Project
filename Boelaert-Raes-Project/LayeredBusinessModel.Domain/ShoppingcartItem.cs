using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class ShoppingcartItem
    {
        public int shoppingcart_item_id { get; set; }
        public int customer_id { get; set; }
        public int dvd_info_id { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int copy_type_id { get; set; }

        //extra values that are displayed in the shoppingcart gridview, info comes from sql JOINs with other tables
        public String name { get; set; } //dvd name from table DvdInfo
        public String typeName { get; set; } //copyType name (verhuur or verkoop) from table DvdCopyType

    }
}
