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

        public List<ShoppingcartItem> getCartContentForCustomer(int id){
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.getCartContentForCustomer(id);
        }

        /*adds buy copy to cart(no dates)*/
        public Boolean addItemToCart(int customerID, int dvd_info_id)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customerID, dvd_info_id);
        }

        //overloaded method for adding rent items with dates
        public Boolean addItemToCart(int customerID, int dvdInfoID, DateTime startdate, DateTime enddate)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customerID, dvdInfoID, startdate, enddate);
        }

        public Boolean removeItemFromCart(String cartItemID)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.removeItemFromCart(cartItemID);
        }

        public void clearCustomerCart(int customerID)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            shoppingCartDAO.clearCustomerCart(customerID);
        }

        public void clearTable()
        {
            shoppingCartDAO = new ShoppingCartDAO();
            shoppingCartDAO.clearTable();
        }
    }
}
