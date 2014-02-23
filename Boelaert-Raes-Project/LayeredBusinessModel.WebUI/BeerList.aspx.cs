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
    public partial class BeerList : System.Web.UI.Page
    {
        
        private BrewerService brewerService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                brewerService = new BrewerService();

                ddBrewers.DataSource = brewerService.GetAll();
                ddBrewers.DataTextField = "naam";
                ddBrewers.DataValueField = "brouwernr";

                //beter zonder hardcoded tabelnaam?:

                // ddlBrewers.DataTextField = brewer_service.getAll().Tables[0].Columns[0];
                // ddlBrewers.DataValueField = brewer_service.getAll().Tables[0].Columns[1];

                ddBrewers.DataBind();
                ddBrewers.Items.Insert(0, new ListItem("Selecteer een brouwer", ""));                
            }
            
        }

        protected void ddBrewers_SelectedIndexChanged(object sender, EventArgs e)
        {
            BierService bierService = new BierService();

            gvBeer.DataSource = bierService.getBeersForBrewer(ddBrewers.SelectedItem.Value);
            gvBeer.DataBind();
        }
    }
}