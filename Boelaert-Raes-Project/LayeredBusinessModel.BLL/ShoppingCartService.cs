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

        public List<DvdCopy> getCartContentForCustomer(int id){
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.getCartContentForCustomer(id);
        }

        public Boolean addItemToCart(int customerID, int dvdCopyID)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customerID, dvdCopyID);
        }

        //overloaded method for adding rent items with dates
        public Boolean addItemToCart(int customerID, int dvdCopyID, DateTime startdate, DateTime enddate)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.addItemToCart(customerID, dvdCopyID, startdate, enddate);
        }

        public Boolean removeItemFromCart(String dvdCopyID)
        {
            shoppingCartDAO = new ShoppingCartDAO();
            return shoppingCartDAO.removeItemFromCart(dvdCopyID);
        }

        
    }
}
