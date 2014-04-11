using LayeredBusinessModel.BLL;
using LayeredBusinessModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if(int.TryParse(Request.QueryString["id"], out id)) {                  
                    
                    lblID.Text = id.ToString();
                    DvdInfoService dvdbll = new DvdInfoService();
                    DvdInfo dvdInfo = dvdbll.getDvdInfoWithID(id.ToString());

                    foreach (KeyValuePair<int, String> k in dvdInfo.media)
                    {
                        if (k.Key == 1)
                        {
                            imgDvdCoverFocus.ImageUrl = k.Value;
                        }
                        else if (k.Key == 2)
                        {
                            if (imgDvdCover1.ImageUrl == "")
                            {
                                imgDvdCover1.ImageUrl = k.Value;
                            }
                            else if (imgDvdCover2.ImageUrl == "")
                            {
                                imgDvdCover2.ImageUrl = k.Value;
                            }
                            else if (imgDvdCover3.ImageUrl == "")
                            {
                                imgDvdCover3.ImageUrl = k.Value;
                            }
                        }
                    }

                    int i = 0;


                }
                

            }
        }
    }
}