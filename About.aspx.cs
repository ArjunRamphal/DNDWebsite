using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] != null)
            {
                string userType = Session["UserType"].ToString();
                if (userType == "Sales Representative" || userType == "Manager")
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
            }
        }
    }
}