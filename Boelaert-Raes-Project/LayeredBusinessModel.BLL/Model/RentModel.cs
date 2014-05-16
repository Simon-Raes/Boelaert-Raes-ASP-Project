using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL.Model
{
    public class RentModel
    {
        public int getNumberOfActiveRentCopiesForCustomer(Customer customer)
        {
            int numberOfCurrentlyRentedItems = 0;
            try
            {
                //check the number of rent items in the user's cart
                List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(customer);           //Throws NoRecordException


                foreach (ShoppingcartItem item in cartContent)
                {
                    if (item.dvdCopyType.name.Equals("Verhuur"))
                    {
                        numberOfCurrentlyRentedItems++;
                    }
                }
            }
            catch(Exception ex)
            {
                //no rent items in the user's cart
            }            

            //check the number of items currently being rented by the user
            try
            {
                List<OrderLine> orderLines = new OrderLineService().getActiveRentOrderLinesByCustomer(customer);          //Throws NoRecordException
                foreach (OrderLine orderLine in orderLines)
                {
                    numberOfCurrentlyRentedItems++;
                }
            }
            catch(NoRecordException ex)
            {
                //user has no active rent orders
            }            

            return numberOfCurrentlyRentedItems;
        }


    }
}
