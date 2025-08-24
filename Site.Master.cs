using System;

namespace DNDWebsite
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
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
        }
    }
}