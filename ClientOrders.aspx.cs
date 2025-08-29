using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class ClientOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            // Check if logged in and role = SalesRep
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "SalesRep")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadClientOrders();
            }*/
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
            }*/
        }
    }
}
