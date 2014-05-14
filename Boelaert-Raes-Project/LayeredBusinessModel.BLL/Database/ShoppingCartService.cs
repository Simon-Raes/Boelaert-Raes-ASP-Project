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
            return new ShoppingCartDAO().getByCustomer(customer);                 //Throws NoRecordException || DALException
        }

        /*adds buy copy to cart(no dates)*/
        public Boolean addByCustomerAndDvd(Customer customer, DvdInfo dvdInfo)
        {
            return new ShoppingCartDAO().addByCustomerAndDvd(customer, dvdInfo);           
        }

        //overloaded method for adding rent items with dates
        /*
        public Boolean addByCustomerAndStartdateAndEndate(Customer customer, String dvdInfoID, DateTime startdate, DateTime enddate)
        {
            return new ShoppingCartDAO().addByCustomerAndStartdateAndEndate(customer, dvdInfoID, startdate, enddate);
        }*/

        public Boolean deleteByID(String cartItemID)
        {
            return new ShoppingCartDAO().deleteByID(cartItemID);
        }

        public Boolean deleteByCustomer(Customer customer)
        {
            return new ShoppingCartDAO().deleteByCustomer(customer);
        }

        public Boolean deleteAll()
        {
            return new ShoppingCartDAO().deleteAll();
        }
    }
}
