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
        /*Converts the customer's shopping cart to an order and returns it*/
        public Order createOrder(Customer user)
        {
            try
            {
                //get all items currently in cart
                List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(user);           //Throws NoRecordException

                //create new order for this user                
                int orderID = new OrderService().addOrderForCustomer(user);           //Throws NoRecordException
                Order thisOrder = new OrderService().getByID(orderID.ToString());

                OrderLineService orderLineService = new OrderLineService();

                //add cart items to newly created order
                foreach (ShoppingcartItem item in cartContent)
                {
                    OrderLine orderline = new OrderLine
                    {
                        order = new OrderService().getByID(orderID.ToString()),            //Throws NoRecordException
                        dvdInfo = new DvdInfoService().getByID(item.dvdInfo.dvd_info_id.ToString()),           //Throws NoRecordException
                        //dvd_copy_id = copy.dvd_copy_id, //don't add a copy_id yet, only do this when a user has paid (id will be set in admin module)                       
                        startdate = item.startdate,
                        enddate = item.enddate,
                        //order_line_type_id is verhuur/verkoop? dan kunnen we dat eigenlijk via join ophalen via dvd_copy tabel
                        orderLineType = new OrderLineTypeService().getByID(item.dvdCopyType.id.ToString())          //Throws NoRecordException
                    };

                    if (orderLineService.add(orderline))
                    {
                        //succes
                    }
                }

                //clear cart
                if (new ShoppingCartService().deleteByCustomer(user))
                {
                    //success
                }

                return thisOrder;
            }
            catch (NoRecordException)
            {
                return null;
            }
        }


        /*Sets the supplied order as paid and assigns a dvd copy to each orderLine (if possible).*/
        public void payOrder(Order selectedOrder)
        {
            //set the order status to PAID
            selectedOrder.orderstatus = new OrderStatusService().getByID("2");          //Throws NoRecordException
            new OrderService().updateOrder(selectedOrder);            

            //assign copies to the orderlines
            List<OrderLine> orderLines = new OrderLineService().getByOrder(selectedOrder);            //Throws NoRecordException

            foreach (OrderLine orderLine in orderLines)
            {
                if (orderLine.orderLineType.id == 1) //rent copy
                {
                    DvdInfo thisDVD = orderLine.dvdInfo;

                    //get all unavailable dates starting from TODAY          
           
                    Dictionary<int, List<DateTime>> dicCopyUnavailableDates = new AvailabilityModel().getAllUnavailableDaysPerCopyForDvdInfo(thisDVD, DateTime.Today);          //Throws NoRecordException     
                    Dictionary<int, int> dicDaysFreeForCopy = new Dictionary<int, int>(); //contains the number of free days for the dvdCopy (copy-id, freedays)

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
                                //one of the requested days is not free, this copy can not be used
                                copyIsAvailable = false;
                            }
                        }

                        if (copyIsAvailable)
                        {
                            //we now know this copy has the needed time available, now to determine the size of this free period
                            dicDaysFreeForCopy.Add(entry.Key, 0);

                            //check the amount of free days between start date and the first unavailable date (going backwards)
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

                            //check the amount of free days between end date and the first unavailable date (going forward)
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
                        new OrderLineService().updateOrderLine(orderLine);

                        //update the amount_sold field of the dvdInfo record
                        orderLine.dvdInfo.amount_sold = orderLine.dvdInfo.amount_sold + 1;
                        new DvdInfoService().update(orderLine.dvdInfo);                                                             //Throws NoRecordException
                    }
                    else
                    {
                        //no copies with future orderLines are available for this reservation/rent order
                        //get all fully available copies (=copies with no orderLines in the future) to assign to this orderline
                        try
                        {
                            List<DvdCopy> dvdCopies = new DvdCopyService().getAllFullyAvailableCopies(thisDVD, orderLine.startdate);      //Throws NoRecordException || DALException
                           
                            //set the first available copy as the copy for this orderline
                            orderLine.dvdCopy = dvdCopies[0];
                            //update database
                            new OrderLineService().updateOrderLine(orderLine);
                            //update the amount_sold field of the dvdInfo record
                            dvdCopies[0].dvdinfo.amount_sold += 1;
                            new DvdInfoService().update(dvdCopies[0].dvdinfo);                                                                 
                        
                        } catch(Exception ex)
                        {
                            //no copy can be assigned at this time, this means someone else received the last copy between the time where
                            //this user added the item to his cart and when he payed for the order.
                        }
                                                    
                           
                        
                    }
                }
                else if (orderLine.orderLineType.id == 2) //sale copy
                {
                    //get all available copies
                    //get this list again for every item so you can add a new, available copy to the order
                    List<DvdCopy> availableCopies = new DvdCopyService().getAllInStockBuyCopiesForDvdInfo(orderLine.dvdInfo);           //Throws NoRecordException || DALException

                    //get the first available copy
                    //set the found copy as the copy for this orderline
                    orderLine.dvdCopy = availableCopies[0];
                    new OrderLineService().updateOrderLine(orderLine);

                    //update the amount_sold field of the dvdInfo record
                    DvdInfo dvdInfo = availableCopies[0].dvdinfo;
                    dvdInfo.amount_sold = dvdInfo.amount_sold + 1;
                    new DvdInfoService().update(dvdInfo);         //Throws NoRecordException

                    //mark the found copy as NOT in_stock
                    availableCopies[0].in_stock = false;
                    if (new DvdCopyService().updateCopy(availableCopies[0]))            //Throws NoRecordException || DALException  
                    {
                        //Success, record updated
                    }
                }
            }
        }        

        public double getOrderCost(Order order)
        {
            double totalCost = 0;

            try
            {
                List<OrderLine> orderLines = new OrderLineService().getByOrder(order);            //Throws NoRecordException
                //set order total cost
                foreach (OrderLine orderLine in orderLines)
                {
                    if (orderLine.orderLineType.id == 1) //rent
                    {
                        TimeSpan duration = orderLine.enddate - orderLine.startdate;
                        totalCost += orderLine.dvdInfo.rent_price * duration.Days;
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
