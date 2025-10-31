using System;
using System.Web.UI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DNDWebsite
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private const string ChatbaseSecret = "50k1cm17h8yo98zc6vkrykjmlk74wfy4";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page is DNDWebsite._Default))
            {
                ContactNav.Visible = false;
                ProductNav.Visible = false;
                AboutNav.Visible = false;
            }

            if (Page is DNDWebsite.About)
            {
                ContactNav.Visible = true;
            }

            /*
            if (Session["UserRole"] != null)
            {
                string role = Session["UserRole"].ToString();

                if (role == "SalesRep")
                {
                    pnlClientOrdersLink.Visible = true; // Show Client Orders
                }
            }*/
        }
    }
}