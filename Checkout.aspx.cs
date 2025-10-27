using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Checkout : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["orderId"] != null)
                {
                    int orderId = Convert.ToInt32(Request.QueryString["orderId"]);
                    LoadOrderProducts(orderId);
                }
            }
        }

        private void LoadOrderProducts(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        p.ProductName,
                        osp.OrderSupplierProductQuantity AS Quantity,
                        osp.OrderSupplierProductPrice AS Price
                    FROM OrderSupplierProduct osp
                    INNER JOIN Product p ON osp.ProductID = p.ProductID
                    WHERE osp.OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCheckout.DataSource = dt;
                gvCheckout.DataBind();

                // Display total order amount in a label
                decimal totalAmount = 0;
                foreach (DataRow row in dt.Rows)
                    totalAmount += Convert.ToDecimal(row["Price"]);

                lblMessage.Text = $"Total Order Amount: {totalAmount:C2}";
            }
        }

        protected void btnBackToOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("Order.aspx");
        }
    }
}
