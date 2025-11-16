using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Order : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["unauthorized"] == "true")
                {
                    lblUnauthorizedMessage.Text = "You are not authorized to view that order.";
                }

                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            try
            {
                int clientId = Convert.ToInt32(Session["ClientID"]);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            o.OrderID, 
                            o.OrderDate, 
                            o.OrderAmount, 
                            o.OrderStatus,
                            p.PaymentStatus
                        FROM [Order] o
                        LEFT JOIN Payment p ON o.OrderID = p.OrderID
                        WHERE o.ClientID = @ClientID
                        ORDER BY o.OrderID DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClientID", clientId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add readable text columns for display
                    dt.Columns.Add("OrderStatusText", typeof(string));
                    dt.Columns.Add("PaymentStatusText", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        bool orderStatus = Convert.ToBoolean(row["OrderStatus"]);
                        bool paymentStatus = row["PaymentStatus"] != DBNull.Value && Convert.ToBoolean(row["PaymentStatus"]);

                        row["OrderStatusText"] = orderStatus ? "Completed" : "Pending";
                        row["PaymentStatusText"] = paymentStatus ? "Paid" : "Unpaid";
                    }

                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();

                    lblMessage.Text = dt.Rows.Count == 0
                        ? "You have no orders at this time."
                        : "";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading orders: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrders();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GoToCheckout")
            {
                // e.CommandArgument is now the OrderID itself. No conversion needed!
                string orderId = e.CommandArgument.ToString();

                // Redirect to checkout page with OrderID
                Response.Redirect("Checkout.aspx?orderId=" + orderId);
            }
        }
    }
}
