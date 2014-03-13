using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System.Data;

namespace LayeredBusinessModel.WebUI
{
    public partial class Cart : System.Web.UI.Page
    {
        private ShoppingCartService shoppingCartService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                shoppingCartService = new ShoppingCartService();
                if (Session["user"] != null)
                {
                    gvCart.DataSource = shoppingCartService.getCartContentForCustomer(((Customer)Session["user"]).customer_id);
                    gvCart.DataBind();
                }
            }
        }



        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            
            //remove copy from all shoppingcarts (could only be on one)
            ShoppingCartService shoppingCartService = new ShoppingCartService();
            shoppingCartService.removeItemFromCart(gvCart.Rows[index].Cells[0].Text);
            
            //set copy as in_stock
            DvdCopyService dvdCopyService = new DvdCopyService();
            dvdCopyService.updateDvdCopyInStockStatus(gvCart.Rows[index].Cells[0].Text, true);
            
            //todo: remove row from gridview on button click

        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            //todo: create new order (orderID, customerID, status(=new?))
            //add records in orderline (orderlineID, orderID, type_id, dvd_coyp_id, startdate, enddate) date for rent
        }

        
    }
}