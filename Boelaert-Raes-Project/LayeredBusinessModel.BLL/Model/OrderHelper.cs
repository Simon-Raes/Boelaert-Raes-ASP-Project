using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class OrderHelper
    {
        public void payOrder(String orderID)
        {
            OrderService orderService = new OrderService();
            Order selectedOrder = orderService.getOrder(orderID);

            //set the order status to PAID
            selectedOrder.orderstatus_id = 2;
            orderService.updateOrder(selectedOrder);

            //assign copies to the orderlines
            OrderLineService orderLineService = new OrderLineService();
            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));

            DvdCopyService dvdCopyService = new DvdCopyService();
            List<DvdCopy> availableCopies = null;
            DvdCopy copy = null;

            //Boolean allInStock = true;

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

                    //update the amount_sold field of the dvdInfo record
                    DvdInfoService dvdInfoService = new DvdInfoService();
                    DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(copy.dvd_info_id.ToString());
                    dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                    dvdInfoService.updateDvdInfo(dvdInfo);

                    //mark the found copy as NOT in_stock
                    copy.in_stock = false;
                    dvdCopyService.updateCopy(copy);
                }
                else
                {
                    //allInStock = false;
                    //not in stock, will not be assigned a copy!!
                    //todo: handle this error some way, display error for this item or let the user know this item is currently out of stock
                }
            }
        }

        public String getOrderCost(String orderID)
        {
            double totalCost = 0;

            OrderLineService orderLineService = new OrderLineService();
            DvdInfoService dvdInfoService = new DvdInfoService();

            List<OrderLine> orderLines = orderLineService.getOrderLinesForOrder(Convert.ToInt32(orderID));



            //set order total cost
            foreach (OrderLine orderLine in orderLines)
            {
                if (orderLine.order_line_type_id == 1) //rent
                {
                    TimeSpan duration = orderLine.enddate - orderLine.startdate;
                    int durationDays = duration.Days;

                    totalCost += dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString()).rent_price * durationDays;
                }
                else if (orderLine.order_line_type_id == 2) //buy
                {
                    totalCost += dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString()).buy_price;
                }
            }

            return totalCost.ToString();
        }
    }
}
