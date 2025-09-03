using System;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Products : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }
        }

    }
}
