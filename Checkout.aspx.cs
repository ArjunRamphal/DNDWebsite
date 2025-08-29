using System;
using System.Data;

namespace DNDWebsite
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDummyCart();
            }
        }

        private void LoadDummyCart()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));

            // Dummy products
            dt.Rows.Add("Product A", 2, 100.00m, 200.00m);
            dt.Rows.Add("Product B", 1, 50.00m, 50.00m);

            gvCheckout.DataSource = dt;
            gvCheckout.DataBind();
        }
    }
}
