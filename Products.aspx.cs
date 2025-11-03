using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Products : Page
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                InitializeClientProductsGrid();
                LoadAvailableProducts();
            }
        }

        private void InitializeClientProductsGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Quantity");
            ViewState["Products"] = dt;

            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }

        private void LoadAvailableProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, ProductName, ProductSurcharge FROM Product ORDER BY ProductName";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAvailableProducts.DataSource = dt;
                gvAvailableProducts.DataBind();
            }
        }

        protected void gvAvailableProducts_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvAvailableProducts.PageIndex = e.NewPageIndex;
            LoadAvailableProducts();
        }

        protected void gvAvailableProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectProduct")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAvailableProducts.Rows[index];

                string productName = row.Cells[1].Text;
                TextBox txtQty = row.FindControl("txtQuantityRow") as TextBox;
                int quantity = 1;

                if (txtQty != null && int.TryParse(txtQty.Text, out int parsedQty) && parsedQty > 0)
                {
                    quantity = parsedQty;
                }

                DataTable dt = ViewState["Products"] as DataTable;
                DataRow dr = dt.NewRow();
                dr["ProductName"] = productName;
                dr["Quantity"] = quantity;
                dt.Rows.Add(dr);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
                ViewState["Products"] = dt;
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

        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;

            // Rebind the GridView using ViewState
            DataTable dt = ViewState["Products"] as DataTable;
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
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

                string insertOrder = @"INSERT INTO [Order] (ClientID, UserName, OrderDate, OrderAmount, OrderStatus) 
                                       VALUES (@ClientID, '', @OrderDate, 0, 0); SELECT SCOPE_IDENTITY();";
                SqlCommand orderCmd = new SqlCommand(insertOrder, conn);
                orderCmd.Parameters.AddWithValue("@ClientID", clientID);
                orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Today);
                int orderID = Convert.ToInt32(orderCmd.ExecuteScalar());

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

                string insertPayment = @"INSERT INTO Payment 
                                         (OrderID, PaymentTotal, PaymentDue, PaymentSurplus, PaymentStatus)
                                         VALUES (@OrderID, 0, 0, 0, 0)";
                SqlCommand paymentCmd = new SqlCommand(insertPayment, conn);
                paymentCmd.Parameters.AddWithValue("@OrderID", orderID);
                paymentCmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Order request and payment record created successfully.";

            InitializeClientProductsGrid();
        }
    }
}
