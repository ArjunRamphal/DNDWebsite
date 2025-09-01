using System;
using System.Data;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class ClientOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (!IsPostBack)
            {
                LoadClientOrders();
            }
            */

            if (Session["UserType"] == null || Session["UserType"].ToString() != "Sales Representative")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadDummyOrders();
            }
        }

        private void LoadClientOrders()
        {
            /*
            string connStr = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        o.OrderID,
                        c.ClientName,
                        o.OrderDate,
                        o.TotalAmount
                    FROM Orders o
                    INNER JOIN Clients c ON o.ClientID = c.ClientID
                    ORDER BY o.OrderDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvClientOrders.DataSource = dt;
                    gvClientOrders.DataBind();
                }
            }
            */
        }

        private void LoadDummyOrders()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(int));
            dt.Columns.Add("ClientName", typeof(string));
            dt.Columns.Add("OrderDate", typeof(DateTime));
            dt.Columns.Add("TotalAmount", typeof(decimal));

            // Dummy orders
            dt.Rows.Add(1001, "Alice Johnson", DateTime.Now.AddDays(-5), 249.99m);
            dt.Rows.Add(1002, "Bob Smith", DateTime.Now.AddDays(-3), 129.50m);
            dt.Rows.Add(1003, "Charlie Brown", DateTime.Now.AddDays(-1), 349.00m);

            gvClientOrders.DataSource = dt;
            gvClientOrders.DataBind();
        }
    }
}
