using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DNDWebsite
{
    public partial class Order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserEmail"] == null)
            {
                Response.Redirect("~/Login.aspx"); // Not logged in → redirect
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            /*
            string connStr = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        o.OrderID,
                        o.OrderDate,
                        p.ProductName,
                        oi.Quantity,
                        (oi.Quantity * oi.Price) AS TotalPrice
                    FROM [Order] o
                    INNER JOIN OrderItem oi ON o.OrderID = oi.OrderID
                    INNER JOIN Product p ON oi.ProductID = p.ProductID
                    INNER JOIN Client c ON o.ClientID = c.ClientID
                    WHERE c.ClientEmail = @Email
                    ORDER BY o.OrderDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Session["UserEmail"]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();
                }
            }*/
        }
    }
}
