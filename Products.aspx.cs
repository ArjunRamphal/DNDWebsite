using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Products : Page
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProductName");
                dt.Columns.Add("Quantity");
                ViewState["Products"] = dt;

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string product = txtSearch.Text.Trim();
            string qtyText = txtQuantity.Text.Trim();
            int quantity;

            if (string.IsNullOrEmpty(product))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a product description.";
                return;
            }

            if (!int.TryParse(qtyText, out quantity) || quantity <= 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid quantity.";
                return;
            }

            DataTable dt = ViewState["Products"] as DataTable;
            DataRow dr = dt.NewRow();
            dr["ProductName"] = product;
            dr["Quantity"] = quantity;
            dt.Rows.Add(dr);

            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            txtSearch.Text = "";
            txtQuantity.Text = "";
            lblMessage.Text = "";
        }

        protected void gvProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DataTable dt = ViewState["Products"] as DataTable;
            dt.Rows.RemoveAt(e.RowIndex);

            gvProducts.DataSource = dt;
            gvProducts.DataBind();
            ViewState["Products"] = dt;
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["Products"] as DataTable;
            if (dt.Rows.Count == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please add at least one product before creating the order request.";
                return;
            }

            string clientEmail = Session["UserEmail"].ToString();
            int clientID;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get ClientID
                string clientQuery = "SELECT ClientID FROM Client WHERE ClientEmail=@Email";
                SqlCommand clientCmd = new SqlCommand(clientQuery, conn);
                clientCmd.Parameters.AddWithValue("@Email", clientEmail);
                object result = clientCmd.ExecuteScalar();
                if (result == null)
                {
                    lblMessage.Text = "Client not found.";
                    return;
                }
                clientID = Convert.ToInt32(result);

                // Insert Order
                string insertOrder = @"INSERT INTO [Order] (ClientID, UserName, OrderDate, OrderAmount, OrderStatus) 
                               VALUES (@ClientID, '', @OrderDate, 0, 0); SELECT SCOPE_IDENTITY();";
                SqlCommand orderCmd = new SqlCommand(insertOrder, conn);
                orderCmd.Parameters.AddWithValue("@ClientID", clientID);
                orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Today);
                int orderID = Convert.ToInt32(orderCmd.ExecuteScalar());

                // Insert products into ClientOrderProduct
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    int quantity = Convert.ToInt32(row["Quantity"]);

                    string insertProduct = @"INSERT INTO ClientOrderProduct 
                                     (OrderID, ClientID, ClientOrderProductName, ClientOrderProductQuantity, ClientOrderProductStatus)
                                     VALUES (@OrderID, @ClientID, @ProductName, @Quantity, 0)";
                    SqlCommand prodCmd = new SqlCommand(insertProduct, conn);
                    prodCmd.Parameters.AddWithValue("@OrderID", orderID);
                    prodCmd.Parameters.AddWithValue("@ClientID", clientID);
                    prodCmd.Parameters.AddWithValue("@ProductName", productName);
                    prodCmd.Parameters.AddWithValue("@Quantity", quantity);
                    prodCmd.ExecuteNonQuery();
                }

                // Insert a Payment record with default values
                string insertPayment = @"INSERT INTO Payment 
                                 (OrderID, PaymentTotal, PaymentDue, PaymentSurplus, PaymentStatus)
                                 VALUES (@OrderID, 0, 0, 0, 0)";
                SqlCommand paymentCmd = new SqlCommand(insertPayment, conn);
                paymentCmd.Parameters.AddWithValue("@OrderID", orderID);
                paymentCmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Order request and payment record created successfully.";

            // Clear GridView and input
            DataTable dtClear = new DataTable();
            dtClear.Columns.Add("ProductName");
            dtClear.Columns.Add("Quantity");
            ViewState["Products"] = dtClear;
            gvProducts.DataSource = dtClear;
            gvProducts.DataBind();
        }

    }
}
