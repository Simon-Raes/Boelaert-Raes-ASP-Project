using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.WebUI
{   
    
    //pagina die we misschien wel of niet gaan gebruiken


    public partial class Admin : System.Web.UI.Page
    {
        private OrderLineService orderLineService;
        private DvdCopyService dvdCopyService;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /**Takes an order (that has been paid) and adds a dvdCopy to each orderLine*/
        public void dispatchOrder(int orderID)
        {
            orderLineService = new OrderLineService();
            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(orderID);

            List<DvdCopy> availableCopies = null;
            DvdCopy copy = null;

            foreach (OrderLine orderLine in orderLines)
            {
                //get all available copies
                //get this list again for every item so you can add a new, available copy to the order (todo: do this without going to the database for every item)
                if (orderLine.order_line_type_id == 1) //rent copy
                {
                    availableCopies = dvdCopyService.getAllInStockRentCopiesForDvdInfo(Convert.ToString(orderLine.dvd_info_id));
                }
                else if (orderLine.order_line_type_id == 2) //sale copy
                {
                    availableCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(Convert.ToString(orderLine.dvd_info_id));
                }

                //check if there is a copy available
                if (availableCopies.Count > 0)
                {
                    //get the first available copy
                    copy = availableCopies[0];

                    //set the found copy as the copy for this orderline
                    orderLine.dvd_copy_id = copy.dvd_copy_id;
                    orderLineService.updateOrderLine(orderLine);

                    //mark the found copy as NOT in_stock
                    copy.in_stock = false;
                    dvdCopyService.updateCopy(copy);
                }
                else
                {
                    //not in stock, display error for this item, let the user know this item is currently out of stock
                }
            }
        }
    }
}