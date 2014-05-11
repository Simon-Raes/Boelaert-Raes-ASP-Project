using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class ShoppingCartService
    {
        private ShoppingCartDAO shoppingCartDAO;

        public List<ShoppingcartItem> getCartContentForCustomer(Customer customer){
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.getCartContentForCustomer(customer);
        }

        /*adds buy copy to cart(no dates)*/
        public Boolean addItemToCart(Customer customer, DvdInfo dvdInfo)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customer, dvdInfo);
        }

        //overloaded method for adding rent items with dates
        public Boolean addItemToCart(Customer customer, int dvdInfoID, DateTime startdate, DateTime enddate)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customer, dvdInfoID, startdate, enddate);
        }

        public Boolean removeItemFromCart(String cartItemID)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.removeItemFromCart(cartItemID);
        }

        public void clearCustomerCart(Customer customer)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            shoppingCartDAO.clearCustomerCart(customer);
        }

        public void clearTable()
        {
            shoppingCartDAO = new ShoppingCartDAO();
            shoppingCartDAO.clearTable();
        }
    }
}
