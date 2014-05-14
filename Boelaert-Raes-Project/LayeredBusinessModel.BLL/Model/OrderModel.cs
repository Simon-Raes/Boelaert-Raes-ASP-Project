using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.Domain;
using LayeredBusinessModel.BLL.Model;
using LayeredBusinessModel.BLL.Database;
using CustomException;

namespace LayeredBusinessModel.BLL
{
    public class OrderModel
    {
        public void payOrder(Order selectedOrder)
        {
            OrderService orderService = new OrderService();

            //set the order status to PAID
            selectedOrder.orderstatus = new OrderStatusService().getByID("2");          //Throws NoRecordException
            orderService.updateOrder(selectedOrder);            

            //assign copies to the orderlines
            List<OrderLine> orderLines = new OrderLineService().getByOrder(selectedOrder);            //Throws NoRecordException

            DvdCopyService dvdCopyService = new DvdCopyService();
            List<DvdCopy> availableCopies = null;
            DvdCopy copy = null;


            foreach (OrderLine orderLine in orderLines)
            {

                if (orderLine.orderLineType.id == 1) //rent copy
                {
                    DvdInfoService dvdInfoService = new DvdInfoService();
                    DvdInfo thisDVD = orderLine.dvdInfo;

                    //get all available dates starting from TODAY 
                    //(also needs to check dates between today and start of requested rent period to determine smallest open window)
                    RentModel rentService = new RentModel();
                    Dictionary<int, List<DateTime>> dicCopyUnavailableDates = rentService.getAllUnavailableDaysPerCopyForDvdInfo(thisDVD, DateTime.Today);          //Throws NoRecordException     
                    Dictionary<int, int> dicDaysFreeForCopy = new Dictionary<int, int>();

                    int selectedCopyId = -1; //id of the copy that will be assigned to this orderLine

                    //loop over the list of occupied dates per copy, check how many free dates exist around the chosen period, store that amount in dicDaysFreeForCopy
                    foreach (KeyValuePair<int, List<DateTime>> entry in dicCopyUnavailableDates)
                    {
                        //determine how many days the copy will be rented
                        int rentPeriodDays = (orderLine.enddate - orderLine.startdate).Days;
                        bool copyIsAvailable = true;

                        for (int i = 0; i <= rentPeriodDays; i++)
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
                        try
                        {
                            orderLine.dvdCopy = new DvdCopyService().getByID(selectedCopyId.ToString());            //Throws NoRecordException || DALException
                        }
                        catch (NoRecordException)
                        {
                            //No dvd's were found
                        }

                        orderLineService.updateOrderLine(orderLine);

                        //update the amount_sold field of the dvdInfo record
                        DvdInfo dvdInfo = orderLine.dvdInfo;
                        dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                        dvdInfoService.update(dvdInfo);                                                             //Throws NoRecordException
                    }
                    else
                    {
                        //no copies with future orderLines are suited for this reservation/rent order
                        //get all fully available copies to assign to this orderline

                        List<DvdCopy> dvdCopies = dvdCopyService.getAllFullyAvailableCopies(thisDVD, orderLine.startdate);      //Throws NoRecordException || DALException
                        //get the first available copy and assign it to this orderLine
                        selectedCopyId = dvdCopies[0].dvd_copy_id;
                        //set the found copy as the copy for this orderline
                        orderLine.dvdCopy = new DvdCopyService().getByID(selectedCopyId.ToString());         //Throws NoRecordException || DALException
                        //update database
                        orderLineService.updateOrderLine(orderLine);
                        //update the amount_sold field of the dvdInfo record
                        DvdInfo dvdInfo = dvdCopies[0].dvdinfo;
                        dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                        dvdInfoService.update(dvdInfo);                                                                 //Throws NoRecordException

                    }
                }


                else if (orderLine.orderLineType.id == 2) //sale copy
                {
                    //get all available copies
                    //get this list again for every item so you can add a new, available copy to the order (todo: try do this without going to the database for every item)

                    availableCopies = dvdCopyService.getAllInStockBuyCopiesForDvdInfo(orderLine.dvdInfo);           //Throws NoRecordException || DALException

                    //get the first available copy
                    copy = availableCopies[0];

                    //set the found copy as the copy for this orderline
                    orderLine.dvdCopy = copy;
                    orderLineService.updateOrderLine(orderLine);

                    //update the amount_sold field of the dvdInfo record
                    DvdInfoService dvdInfoService = new DvdInfoService();
                    DvdInfo dvdInfo = copy.dvdinfo;
                    dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                    dvdInfoService.update(dvdInfo);         //Throws NoRecordException

                    //mark the found copy as NOT in_stock
                    copy.in_stock = false;
                    if (dvdCopyService.updateCopy(copy))            //Throws NoRecordException || DALException  
                    {
                        //Record updated
                    }
                }
            }
        }        

        public double getOrderCost(Order order)
        {
            double totalCost = 0;

            //OrderService orderService = new OrderService();
            //Order order = orderService.getOrder(orderID);
            try
            {
                List<OrderLine> orderLines = new OrderLineService().getByOrder(order);            //Throws NoRecordException

                //set order total cost
                foreach (OrderLine orderLine in orderLines)
                {
                    if (orderLine.orderLineType.id == 1) //rent
                    {
                        TimeSpan duration = orderLine.enddate - orderLine.startdate;
                        int durationDays = duration.Days;

                        totalCost += orderLine.dvdInfo.rent_price * durationDays;
                    }
                    else if (orderLine.orderLineType.id == 2) //buy
                    {
                        totalCost += orderLine.dvdInfo.buy_price;
                    }
                }
            }
            catch (NoRecordException)
            {

            }
            return Math.Round(totalCost, 2);
        }
    }
}
