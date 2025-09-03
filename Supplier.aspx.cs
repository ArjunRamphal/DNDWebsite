using System;
using System.Data;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Supplier : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only allow sales representatives
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadSuppliers();
            }
        }

        private void LoadSuppliers()
        {
            // Create a dummy data table for suppliers
            DataTable dt = new DataTable();
            dt.Columns.Add("SupplierID", typeof(int));
            dt.Columns.Add("SupplierName", typeof(string));
            dt.Columns.Add("PhoneNumber", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            // Add some dummy rows
            dt.Rows.Add(1, "Golden Goods Ltd.", "0847389563", "supplies@goldengoods.com");
            dt.Rows.Add(2, "Mystic Merchants", "0923483742", "supplies@mysticmerchants.com");
            dt.Rows.Add(3, "Dragon Supplies", "0812372189", "supplies@dragonsupplies.com");

            gvSuppliers.DataSource = dt;
            gvSuppliers.DataBind();
        }
    }
}
