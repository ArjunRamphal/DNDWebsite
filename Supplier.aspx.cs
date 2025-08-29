using System;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Supplier : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            // Only allow sales representatives
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Employee")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                // Placeholder for future: LoadSuppliers();
            }*/
        }

        // Future method to load suppliers
        private void LoadSuppliers()
        {
            // Implementation will go here
        }
    }
}
