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

        // Updated LoadAvailableProducts to support optional filter
        // Updated LoadAvailableProducts to support multi-word filtering
        // Updated LoadAvailableProducts to filter out products from opted-out suppliers
        private void LoadAvailableProducts(string filter = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Join Product -> SupplierProduct -> Supplier
                // Filter where SupplierOptOut is 0 (False)
                // Use DISTINCT to avoid duplicate products if multiple active suppliers sell the same item
                string query = @"
                    SELECT DISTINCT p.ProductID, p.ProductName, p.ProductSurcharge 
                    FROM Product p
                    INNER JOIN SupplierProduct sp ON p.ProductID = sp.ProductID
                    INNER JOIN Supplier s ON sp.SupplierID = s.SupplierID
                    WHERE s.SupplierOptOut = 0";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                if (!string.IsNullOrEmpty(filter))
                {
                    string[] filterWords = filter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (filterWords.Length > 0)
                    {
                        // Append conditions to the existing WHERE clause
                        for (int i = 0; i < filterWords.Length; i++)
                        {
                            string paramName = "@Filter" + i;
                            // Use 'p.ProductName' to be explicit
                            query += $" AND p.ProductName LIKE {paramName}";
                            cmd.Parameters.AddWithValue(paramName, "%" + filterWords[i] + "%");
                        }
                    }
                }

                query += " ORDER BY p.ProductName";

                cmd.CommandText = query;

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAvailableProducts.DataSource = dt;
                gvAvailableProducts.DataBind();
            }
        }

        protected void gvAvailableProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAvailableProducts.PageIndex = e.NewPageIndex;
            LoadAvailableProducts(txtFilter.Text.Trim());
        }

        protected void gvAvailableProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectProduct")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAvailableProducts.Rows[index % gvAvailableProducts.PageSize];

                string productName = Server.HtmlDecode(row.Cells[1].Text);
                TextBox txtQty = row.FindControl("txtQuantityRow") as TextBox;
                int quantity = 1;

                if (txtQty != null && int.TryParse(txtQty.Text, out int parsedQty) && parsedQty > 0)
                {
                    quantity = parsedQty;
                }

                DataTable dt = ViewState["Products"] as DataTable;
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("ProductName");
                    dt.Columns.Add("Quantity");
                }

                DataRow dr = dt.NewRow();
                dr["ProductName"] = productName;
                dr["Quantity"] = quantity;
                dt.Rows.Add(dr);

                ViewState["Products"] = dt;

                gvProducts.DataSource = dt;
                gvProducts.DataBind();

                if (upSelectedProducts != null)
                    upSelectedProducts.Update();

                txtFilter.Text = "";

                LoadAvailableProducts();

                string script = $"document.getElementById('{txtFilter.ClientID}').value = '';";
                ScriptManager.RegisterStartupScript(upProducts, upProducts.GetType(), "ClearFilterScript", script, true);
            }
        }

        protected void gvAvailableProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var drv = e.Row.DataItem as System.Data.DataRowView;
                if (drv != null)
                {
                    string productName = drv["ProductName"].ToString();
                    e.Row.Attributes["data-product"] = productName;
                }
            }
        }

        // Server-side filter handler
        protected void txtFilter_TextChanged(object sender, EventArgs e)
        {
            gvAvailableProducts.PageIndex = 0; // reset to first page
            LoadAvailableProducts(txtFilter.Text.Trim());
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string product = txtSearch.Text.Trim();
            string qtyText = txtQuantity.Text.Trim();
            int quantity;

            // 1. Basic Empty Check
            if (string.IsNullOrEmpty(product))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a product description.";
                return;
            }

            // 2. Numeric Description Check (Hard Stop)
            if (decimal.TryParse(product, out _))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Product description cannot be just a number. Please enter a valid name.";
                return;
            }

            // 3. Quantity Check
            if (!int.TryParse(qtyText, out quantity) || quantity <= 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid quantity.";
                return;
            }

            // 4. Database Warning Check (Soft Warning) with Multi-Word Matching
            bool productExists = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Split the user's input into individual words
                string[] searchTerms = product.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (searchTerms.Length > 0)
                {
                    // Build a dynamic query: SELECT TOP 1 1 FROM Product WHERE ProductName LIKE @Word0 AND ProductName LIKE @Word1 ...
                    string checkQuery = "SELECT TOP 1 1 FROM Product WHERE 1=1";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    for (int i = 0; i < searchTerms.Length; i++)
                    {
                        string paramName = $"@Word{i}";
                        checkQuery += $" AND ProductName LIKE {paramName}";
                        cmd.Parameters.AddWithValue(paramName, "%" + searchTerms[i] + "%");
                    }

                    cmd.CommandText = checkQuery;
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        productExists = true;
                    }
                }
            }

            // If not found, show the custom modal
            if (!productExists)
            {
                string message = $"Warning: We could not find a match for \"{product}\" in our database. We will try our best to source it, but availability is not guaranteed.";
                string script = $"showValidationModal('{message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "ProductNotFoundModal", script, true);
            }

            // 5. Add to Grid
            DataTable dt = ViewState["Products"] as DataTable;
            DataRow dr = dt.NewRow();
            dr["ProductName"] = product;
            dr["Quantity"] = quantity;
            dt.Rows.Add(dr);

            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            // Clear inputs
            txtSearch.Text = "";
            txtQuantity.Text = "";
            lblMessage.Text = "";
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
            DataTable dt = ViewState["Products"] as DataTable;
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }

        // 1. Initial Button Click: Checks status
        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            DataTable dt = ViewState["Products"] as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please add at least one product before creating the order request.";
                return;
            }

            string clientEmail = Session["UserEmail"].ToString();
            bool isOptedOut = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ClientOptOut FROM Client WHERE ClientEmail=@Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", clientEmail);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isOptedOut = Convert.ToBoolean(result);
                }
            }

            if (isOptedOut)
            {
                // Show the Opt-In Modal
                ClientScript.RegisterStartupScript(this.GetType(), "ShowOptInModal", "showOptInModal();", true);
            }
            else
            {
                // Not opted out, proceed immediately
                CreateOrder();
            }
        }

        // 2. Handle "Yes, Opt In" click from modal
        protected void btnOptInYes_Click(object sender, EventArgs e)
        {
            string clientEmail = Session["UserEmail"].ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Update client to opt-in (set ClientOptOut = 0)
                string updateQuery = "UPDATE Client SET ClientOptOut = 0 WHERE ClientEmail=@Email";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@Email", clientEmail);
                cmd.ExecuteNonQuery();
            }

            // Now create the order
            CreateOrder();
        }

        // 3. Refactored Order Creation Logic (Your original code moved here)
        private void CreateOrder()
        {
            DataTable dt = ViewState["Products"] as DataTable;
            // (Double check, though validation happened earlier)
            if (dt == null || dt.Rows.Count == 0) return;

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
            lblMessage.Text = "Order request created successfully. You will be redirected to the home page in 5 seconds.";

            btnCreateOrder.Enabled = false;
            InitializeClientProductsGrid();

            string script = "setTimeout(function(){ window.location.href = 'Default.aspx'; }, 5000);";
            ClientScript.RegisterStartupScript(this.GetType(), "RedirectScript", script, true);
        }

        // 4. Helper for the "No, Cancel" button (optional, client-side handles close, but if you need server logic)
        protected void btnProceedOrder_Click(object sender, EventArgs e)
        {
            CreateOrder();
        }
        
        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            // Reset the grid page index to 0
            gvAvailableProducts.PageIndex = 0;
            LoadAvailableProducts(); // Load with empty filter
        }
    }
}
