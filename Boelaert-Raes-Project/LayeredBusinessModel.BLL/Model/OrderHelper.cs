using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL.Model;

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


            foreach (OrderLine orderLine in orderLines)
            {

                if (orderLine.order_line_type_id == 1) //rent copy
                {
                    DvdInfoService dvdInfoService = new DvdInfoService();
                    DvdInfo thisDVD = dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString());

                    //get all available dates starting from TODAY 
                    //(also needs to check dates between today and start of requested rent period to determine smallest open window)
                    RentService rentService = new RentService();
                    Dictionary<int, List<DateTime>> dicCopyUnavailableDates = rentService.getAllUnavailableDaysPerCopyForDvdInfo(thisDVD, DateTime.Today);
                    Dictionary<int, int> dicDaysFreeForCopy = new Dictionary<int, int>();

                    int selectedCopyId = -1; //id of the copy that will be assigned to this orderLine

                    //loop over the list of occupied dates per copy, check how many free dates exist around the chosen period, store that amount in dicDaysFreeForCopy
                    foreach (KeyValuePair<int, List<DateTime>> entry in dicCopyUnavailableDates)
                    {
                        //determine how many days the copy will be rented
                        int rentPeriodDays = (orderLine.enddate - orderLine.startdate).Days;
                        bool copyIsAvailable = true;

                        for (int i = 0; i < rentPeriodDays; i++)
                        {
                            //loop over the occupied days, check if the copy is available on all of the requested dates
                            if (entry.Value.Contains(orderLine.startdate.AddDays(i)))
                            {
                                copyIsAvailable = false;
                            }
                        }

                        if (copyIsAvailable)
                        {
                            //we now know this copy has the needed time available, now to determine how many days this free period contains
                            dicDaysFreeForCopy.Add(entry.Key, 0);

                            //check the amount of free days between start date and the first unavailable date
                            bool unavailableFound = false;
                            int offset = 0;
                            do
                            {
                                if (entry.Value.Contains(orderLine.startdate.AddDays(-offset)) || orderLine.startdate.AddDays(-offset) == DateTime.Today)
                                {
                                    //the copy is reserved on this date, break out of loop
                                    unavailableFound = true;
                                }
                                else
                                {
                                    dicDaysFreeForCopy[entry.Key]++;
                                    offset++;
                                }
                            } while (!unavailableFound);

                            //check the amount of free days between end date and the first unavailable date
                            unavailableFound = false;
                            offset = 0;
                            do
                            {
                                if (entry.Value.Contains(orderLine.enddate.AddDays(offset)) || orderLine.enddate.AddDays(offset) >= DateTime.Today.AddDays(14))
                                {
                                    //the copy is reserved on this date, break out of loop
                                    unavailableFound = true;
                                }
                                else
                                {
                                    dicDaysFreeForCopy[entry.Key]++;
                                    offset++;
                                }
                            } while (!unavailableFound);
                        }
                    }

                    int daysFree = 1000;

                    //loop over all copies, select the one with the smallest free window
                    foreach (KeyValuePair<int, int> entryDays in dicDaysFreeForCopy)
                    {
                        if (entryDays.Value < daysFree)
                        {
                            daysFree = entryDays.Value;
                            selectedCopyId = entryDays.Key;
                        }
                    }





                    //assign the copy if it has been found
                    if (selectedCopyId > 0)
                    {
                        orderLine.dvd_copy_id = selectedCopyId;

                        //set the found copy as the copy for this orderline
                        orderLine.dvd_copy_id = selectedCopyId;
                        orderLineService.updateOrderLine(orderLine);

                        //update the amount_sold field of the dvdInfo record
                        DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(orderLine.dvd_info_id.ToString());
                        dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                        dvdInfoService.updateDvdInfo(dvdInfo);
                    }
                    else
                    {

                        //no copies with future orderLines are suited for this reservation/rent order
                        //get all fully available copies to assign to this orderline

                        List<DvdCopy> dvdCopies = dvdCopyService.getAllFullyAvailableCopies(thisDVD, orderLine.startdate);

                        if (dvdCopies.Count > 0)
                        {
                            //get the first available copy and assign it to this orderLine
                            selectedCopyId = dvdCopies[0].dvd_copy_id;

                            //set the found copy as the copy for this orderline
                            orderLine.dvd_copy_id = selectedCopyId;
                            orderLineService.updateOrderLine(orderLine);

                            //update the amount_sold field of the dvdInfo record
                            DvdInfo dvdInfo = dvdInfoService.getDvdInfoWithID(dvdCopies[0].dvd_info_id.ToString());
                            dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                            dvdInfoService.updateDvdInfo(dvdInfo);
                        }
                        else
                        {
                            //not available either, assign nothing
                            //todo: notify user that his requested rent period is no longer available
                        }
                    }


                }


                else if (orderLine.order_line_type_id == 2) //sale copy
                {
                    //get all available copies
                    //get this list again for every item so you can add a new, available copy to the order (todo: try do this without going to the database for every item)
                    availableCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(Convert.ToString(orderLine.dvd_info_id));

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
                        //all InStock = false
                        //not in stock, will not be assigned a copy!!
                        //todo: handle this error some way, display error for this item or let the user know this item is currently out of stock
                    }
                }


            }
        }

        private void handleRentCopy(OrderLine orderLine)
        {

        }

        private void handleBuyCopy(OrderLine orderLine)
        {

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
