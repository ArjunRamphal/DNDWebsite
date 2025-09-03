using System;
using System.Data;

namespace DNDWebsite
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Access control: Managers only
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Manager")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadDummyReport();
            }
        }

        private void LoadDummyReport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Month", typeof(string));
            dt.Columns.Add("TotalOrders", typeof(int));
            dt.Columns.Add("TotalRevenue", typeof(decimal));

            dt.Rows.Add("January", 120, 15000.00m);
            dt.Rows.Add("February", 98, 12300.50m);
            dt.Rows.Add("March", 135, 17500.75m);

            gvReports.DataSource = dt;
            gvReports.DataBind();
        }
    }
}
