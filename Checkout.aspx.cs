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
            // Only allow clients
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

                    // Check if the order belongs to the logged-in client
                    if (!DoesOrderBelongToClient(orderId))
                    {
                        Response.Redirect("Order.aspx?unauthorized=true");
                        return;
                    }

                    // If the order belongs to the client, load data
                    LoadOrderProducts(orderId);
                    LoadPaymentDetails(orderId);
                }
                else
                {
                    // No order ID in URL, go back to orders
                    Response.Redirect("Order.aspx");
                }
            }
        }

        private bool DoesOrderBelongToClient(int orderId)
        {
            if (Session["ClientID"] == null)
                return false;

            int clientId = Convert.ToInt32(Session["ClientID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM [Order] WHERE OrderID = @OrderID AND ClientID = @ClientID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@ClientID", clientId);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();

                // Return true if this order belongs to the logged-in client
                return count > 0;
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

                decimal totalAmount = 0;
                foreach (DataRow row in dt.Rows)
                    totalAmount += Convert.ToDecimal(row["Price"]);

                lblMessage.Text = $"Total Order Amount: {totalAmount:C2}";
            }
        }

        private void LoadPaymentDetails(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT PaymentTotal, PaymentDue, PaymentSurplus, PaymentStatus 
                                 FROM Payment WHERE OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPayment.DataSource = dt;
                gvPayment.DataBind();

                if (dt.Rows.Count > 0)
                {
                    decimal total = Convert.ToDecimal(dt.Rows[0]["PaymentTotal"]);
                    bool paymentStatus = Convert.ToBoolean(dt.Rows[0]["PaymentStatus"]);

                    if (total == 0)
                    {
                        lblPaymentStatusMessage.ForeColor = System.Drawing.Color.DarkOrange;
                        lblPaymentStatusMessage.Text =
                            "A sales representative will soon fulfill your order.<br />" +
                            "You will receive an email shortly containing the order details.";
                        return;
                    }

                    if (!paymentStatus)
                    {
                        lblPaymentStatusMessage.ForeColor = System.Drawing.Color.Red;
                        lblPaymentStatusMessage.Text =
                            "Your payment is currently pending.<br />" +
                            "Please send an email to <b>dndtrading22@gmail.com</b> with your proof of payment attached.";
                    }
                    else
                    {
                        lblPaymentStatusMessage.ForeColor = System.Drawing.Color.Green;
                        lblPaymentStatusMessage.Text =
                            "Payment received successfully. Thank you!";
                    }
                }
                else
                {
                    lblPaymentStatusMessage.ForeColor = System.Drawing.Color.Red;
                    lblPaymentStatusMessage.Text = "No payment details found for this order.";
                }
            }
        }

        protected void btnBackToOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("Order.aspx");
        }
    }
}
