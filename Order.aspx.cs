using System;
using System.Data;

namespace DNDWebsite
{
    public partial class Order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadDummyOrders();
            }
        }

        private void LoadDummyOrders()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(int));
            dt.Columns.Add("OrderDate", typeof(DateTime));
            dt.Columns.Add("TotalPrice", typeof(decimal));

            dt.Rows.Add(101, DateTime.Now.AddDays(-5), 200.00m);
            dt.Rows.Add(102, DateTime.Now.AddDays(-3), 50.00m);
            dt.Rows.Add(103, DateTime.Now.AddDays(-1), 300.00m);

            gvOrders.DataSource = dt;
            gvOrders.DataBind();
        }

        protected void gvOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GoToCheckout")
            {
                // Just go to Checkout page for now (no OrderID passing)
                Response.Redirect("Checkout.aspx");
            }
        }
    }
}
