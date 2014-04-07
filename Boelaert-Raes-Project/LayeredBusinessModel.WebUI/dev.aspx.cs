using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;

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
            //Delete all shoppingcarts
            ShoppingCartService shoppingCartService = new ShoppingCartService();
            shoppingCartService.clearTable();  
          
            //Delete all orderlines
            OrderLineService orderLineService = new OrderLineService();
            orderLineService.clearTable();

            //Delete all orders
            OrderService orderService = new OrderService();
            orderService.clearTable();

            //Reset all dvd copies to be back in stock (in_stock = true)
            DvdCopyService dvdCopyService = new DvdCopyService();
            dvdCopyService.resetAllCopies();
        }
    }
}