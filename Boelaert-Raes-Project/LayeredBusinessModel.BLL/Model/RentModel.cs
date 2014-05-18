using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class RentModel
    {
        /*Returns the number of rent items in the user's cart + active orders.*/
        public int getNumberOfActiveRentTotalCopiesForCustomer(Customer customer)
        {
            int numberOfCurrentlyRentedItems = 0;

            numberOfCurrentlyRentedItems += getNumberOfActiveRentCartCopiesForCustomer(customer);
            numberOfCurrentlyRentedItems += getNumberOfActiveRentOrdersCopiesForCustomer(customer);                    

            return numberOfCurrentlyRentedItems;
        }


        /*Returns the number of rent items in the user's cart.*/
        public int getNumberOfActiveRentCartCopiesForCustomer(Customer customer)
        {
            int numberOfCurrentlyRentedItems = 0;

            try
            {                
                List<ShoppingcartItem> cartContent = new ShoppingCartService().getCartContentForCustomer(customer);           //Throws NoRecordException
                
                foreach (ShoppingcartItem item in cartContent)
                {
                    if (item.dvdCopyType.name.Equals("Verhuur"))
                    {
                        numberOfCurrentlyRentedItems++;
                    }
                }
            }
            catch (Exception ex)
            {
                //no rent items in the user's cart
                numberOfCurrentlyRentedItems = 0;
            }

            return numberOfCurrentlyRentedItems;
        }


        /*Returns the number of items currently being rented by the user.*/
        public int getNumberOfActiveRentOrdersCopiesForCustomer(Customer customer)
        {
            int numberOfCurrentlyRentedItems = 0;
            
            try
            {
                List<OrderLine> orderLines = new OrderLineService().getActiveRentOrderLinesByCustomer(customer);          //Throws NoRecordException
                foreach (OrderLine orderLine in orderLines)
                {
                    numberOfCurrentlyRentedItems++;
                }
            }
            catch (NoRecordException ex)
            {
                //user has no active rent orders
            }
            return numberOfCurrentlyRentedItems;
        }
    }
}
