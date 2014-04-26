﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LayeredBusinessModel.WebUI
{
    public partial class dvdInfoUserControl : System.Web.UI.UserControl
    {
        public int id { get; set; }
        public String imageUrl { get; set; }
        public String title { get; set; }
        public float buy_price { get; set; }
        public float rent_price { get; set; }        


        protected void Page_Load(object sender, EventArgs e)
        {
            dvdInfoLink.NavigateUrl = "~/detail.aspx?id=" + id;
            dvdInfoLink2.NavigateUrl = dvdInfoLink.NavigateUrl;
            imgDvdCover.ImageUrl = imageUrl;
            if(title.Length<=40)
            {
                lblTitle.Text = title;
            }
            else
            {
                lblTitle.Text = title.Substring(0, 28) + "...";
            }


            if (Request.QueryString["currency"] == null)
            {
                if (CookieUtil.CookieExists("currency"))
                {
                    switch (CookieUtil.GetCookieValue("currency"))
                    {
                        case "usd":                            
                            buy_price = buy_price * (-1);
                            rent_price = rent_price * (-1);
                            break;
                    }
                }
            }
            else
            {
                switch (Request.QueryString["currency"])
                {
                    case "usd":
                        buy_price = buy_price * (-1);
                        rent_price = rent_price * (-1);
                        break;
                }
            }
            btnBuy.Text = "Buy € " + buy_price;
            btnRent.Text = "Rent € " + rent_price;
        }
    }
}