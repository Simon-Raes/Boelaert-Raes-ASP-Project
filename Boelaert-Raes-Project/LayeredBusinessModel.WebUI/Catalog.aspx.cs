using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LayeredBusinessModel.BLL;

namespace LayeredBusinessModel.WebUI
{
    public partial class Catalog : System.Web.UI.Page
    {
        private DvdInfoService dvdInfoService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dvdInfoService = new DvdInfoService();
                gvDvdInfo.DataSource = dvdInfoService.getAll();
                gvDvdInfo.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dvdInfoService = new DvdInfoService();
            gvDvdInfo.DataSource = dvdInfoService.getAllWithTitleSearch(txtSearch.Text);
            gvDvdInfo.DataBind();
        }

       
    }
}