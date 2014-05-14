using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using CustomException;

namespace LayeredBusinessModel.WebUI
{
    public partial class dev : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /**Reset database to start state*/
        protected void btnResetCopies_Click(object sender, EventArgs e)
        {
            try
            {
                //Delete all shoppingcarts
                ShoppingCartService shoppingCartService = new ShoppingCartService();
                shoppingCartService.clearTable();

                //Delete all orderlines
                OrderLineService orderLineService = new OrderLineService();
                orderLineService.clearTable();

                //Delete all orders
                new OrderService().DeleteAll();           //Throws NoRecordException 

                //Reset all dvd copies to be back in stock (in_stock = true)
                new DvdCopyService().resetAllCopies();           //Throws NoRecordException 
            }
            catch (NoRecordException)
            {

            }
        }
    }
}