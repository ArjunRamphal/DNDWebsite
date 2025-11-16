using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;

namespace DNDWebsite
{
    public partial class ClientOrders : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadClientOrders();
                LoadSuppliersFilter();

                if (!string.IsNullOrEmpty(Request.QueryString["selectedOrderID"]))
                {
                    int orderId = Convert.ToInt32(Request.QueryString["selectedOrderID"]);

                    // Select the order in GridView
                    GridViewRow row = null;
                    foreach (GridViewRow r in gvClientOrders.Rows)
                    {
                        if (Convert.ToInt32(gvClientOrders.DataKeys[r.RowIndex].Value) == orderId)
                        {
                            row = r;
                            gvClientOrders.SelectedIndex = r.RowIndex;
                            break;
                        }
                    }

                    if (row != null)
                    {
                        // Load order details like when user clicks Select
                        gvClientOrders_SelectedIndexChanged(gvClientOrders, EventArgs.Empty);
                    }
                }
            }

        }

        private void LoadClientOrders(bool pendingOnly = false, string searchTerm = "")
        {
            string query = @"SELECT o.OrderID, c.ClientName, o.OrderDate, o.OrderAmount, o.OrderStatus 
                     FROM [Order] o 
                     INNER JOIN Client c ON o.ClientID = c.ClientID
                     WHERE 1=1";

            if (pendingOnly)
                query += " AND o.OrderStatus = 0";

            if (!string.IsNullOrEmpty(searchTerm))
                query += " AND c.ClientName LIKE @SearchTerm";

            query += " ORDER BY o.OrderID DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                if (!string.IsNullOrEmpty(searchTerm))
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (!dt.Columns.Contains("OrderStatusText"))
                    dt.Columns.Add("OrderStatusText", typeof(string));

                foreach (DataRow r in dt.Rows)
                    r["OrderStatusText"] = Convert.ToBoolean(r["OrderStatus"]) ? "Completed" : "Pending";

                gvClientOrders.DataSource = dt;
                gvClientOrders.DataBind();
            }
        }

        protected void gvClientOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(gvClientOrders.SelectedDataKey.Value);
            string clientName = Server.HtmlDecode(gvClientOrders.SelectedRow.Cells[2].Text);

            lblHeader.InnerText = $"Order {orderId} for {clientName}";
            hfSelectedOrderId.Value = orderId.ToString();

            // hide filters
            chkPending.Visible = false;
            btnResetFilter.Visible = false;
            txtClientSearch.Visible = false;
            btnSearchClient.Visible = false;

            // load all grids
            LoadClientOrderProducts(orderId);
            LoadOrderProducts(orderId);
            LoadSupplierProducts();
            LoadPaymentInfo(orderId);

            // show panel, hide main grid
            pnlOrderProducts.Visible = true;
            gvClientOrders.Visible = false;

            bool orderFinalized = IsOrderFinalized(orderId);
            SetEditingEnabled(!orderFinalized);

            // Hide the Remove column in gvOrderProducts if order is finalized
            if (gvOrderProducts.Columns.Count > 0)
                gvOrderProducts.Columns[gvOrderProducts.Columns.Count - 1].Visible = !orderFinalized;

            CalculateAndDisplayRunningTotal(orderId);
        }

        protected void btnSearchClient_Click(object sender, EventArgs e)
        {
            string searchTerm = txtClientSearch.Text.Trim();
            LoadClientOrders(chkPending.Checked, searchTerm);
        }

        private bool IsOrderFinalized(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT OrderStatus FROM [Order] WHERE OrderID = @OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                object res = cmd.ExecuteScalar();
                return res != null && res != DBNull.Value && Convert.ToBoolean(res);
            }
        }

        private void SetEditingEnabled(bool enabled)
        {
            txtAddQty.Enabled = enabled;
            btnAddToOrder.Enabled = enabled;
            btnCreateOrder.Enabled = enabled;
            btnCreateOrder.CssClass = enabled ? "btn-search" : "btn-search disabled";
            if (!enabled)
                btnCreateOrder.Attributes["style"] = "opacity:0.5; cursor:not-allowed;";
            else
                btnCreateOrder.Attributes.Remove("style");
        }

        private void LoadClientOrderProducts(int orderId)
        {
            string query = @"
                SELECT ClientOrderProductName, ClientOrderProductQuantity,
                       CASE WHEN ClientOrderProductStatus = 1 THEN 'Completed' ELSE 'Pending' END AS ClientOrderProductStatusText
                FROM ClientOrderProduct
                WHERE OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvClientOrderProducts.DataSource = dt;
                    gvClientOrderProducts.DataBind();
                }
            }
        }

        private void LoadOrderProducts(int orderId)
        {
            string query = @"
                SELECT osp.ProductID, p.ProductName, s.SupplierID, s.SupplierName,
                       osp.OrderSupplierProductQuantity, osp.OrderSupplierProductPrice,
                       CASE WHEN osp.OrderSupplierProductStatus = 1 THEN 'Completed' ELSE 'Pending' END AS OrderSupplierProductStatusText
                FROM OrderSupplierProduct osp
                INNER JOIN Product p ON osp.ProductID = p.ProductID
                INNER JOIN Supplier s ON osp.SupplierID = s.SupplierID
                WHERE osp.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvOrderProducts.DataSource = dt;
                    gvOrderProducts.DataBind();
                }
            }
        }

        private void LoadSupplierProducts()
        {
            string supplierFilter = ddlSuppliers.SelectedValue;
            string search = txtSupplierSearch.Text.Trim();

            string query = @"
            SELECT sp.ProductID, sp.SupplierID, p.ProductName, s.SupplierName,
                   (sp.SupplierProductPrice + ((p.ProductSurcharge / 100.0) * sp.SupplierProductPrice)) AS FinalPrice
            FROM SupplierProduct sp
            INNER JOIN Product p ON sp.ProductID = p.ProductID
            INNER JOIN Supplier s ON sp.SupplierID = s.SupplierID
            WHERE s.SupplierOptOut = 0";

            if (!string.IsNullOrEmpty(supplierFilter))
                query += " AND sp.SupplierID = @SupplierID ";
            if (!string.IsNullOrEmpty(search))
                query += " AND (p.ProductName LIKE @Search OR s.SupplierName LIKE @Search) ";

            query += " ORDER BY FinalPrice ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(supplierFilter))
                    cmd.Parameters.AddWithValue("@SupplierID", supplierFilter);
                if (!string.IsNullOrEmpty(search))
                    cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvSupplierProducts.DataSource = dt;
                    gvSupplierProducts.DataBind();
                }
            }
        }

        private void LoadSuppliersFilter()
        {
            string query = "SELECT SupplierID, SupplierName FROM Supplier WHERE SupplierOptOut = 0 ORDER BY SupplierName";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSuppliers.Items.Clear();
                ddlSuppliers.Items.Add(new ListItem("All suppliers", ""));
                foreach (DataRow r in dt.Rows)
                    ddlSuppliers.Items.Add(new ListItem(r["SupplierName"].ToString(), r["SupplierID"].ToString()));
            }
        }

        protected void btnFilterSupplierProducts_Click(object sender, EventArgs e) => LoadSupplierProducts();
        protected void ddlSuppliers_SelectedIndexChanged(object sender, EventArgs e) => LoadSupplierProducts();

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlOrderProducts.Visible = false;
            gvClientOrders.Visible = true;
            LoadClientOrders(chkPending.Checked);
            chkPending.Visible = true;
            btnResetFilter.Visible = true;
            txtClientSearch.Visible = true;
            btnSearchClient.Visible = true;
            lblHeader.InnerText = "All Client Orders";
        }

        protected void chkPending_CheckedChanged(object sender, EventArgs e) => LoadClientOrders(chkPending.Checked);
        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            chkPending.Checked = false;
            txtClientSearch.Text = "";
            LoadClientOrders(false, "");
        }

        protected void gvSupplierProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfSelectedProductId.Value = gvSupplierProducts.SelectedDataKey.Values["ProductID"].ToString();
            hfSelectedSupplierId.Value = gvSupplierProducts.SelectedDataKey.Values["SupplierID"].ToString();
            hfSelectedProductPrice.Value = gvSupplierProducts.SelectedDataKey.Values["FinalPrice"].ToString();

            lblSelectedSupplierProduct.Text = Server.HtmlDecode(gvSupplierProducts.SelectedRow.Cells[1].Text)
                                          + " — " + Server.HtmlDecode(gvSupplierProducts.SelectedRow.Cells[2].Text);
            lblAddMessage.Text = "";
        }

        protected void btnAddToOrder_Click(object sender, EventArgs e)
        {
            lblAddMessage.Text = "";
            lblAddMessage.ForeColor = System.Drawing.Color.Green;

            if (string.IsNullOrEmpty(hfSelectedProductId.Value) || string.IsNullOrEmpty(hfSelectedSupplierId.Value))
            {
                lblAddMessage.Text = "Please select a supplier product first.";
                lblAddMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int orderId = Convert.ToInt32(hfSelectedOrderId.Value);
            if (IsOrderFinalized(orderId))
            {
                lblAddMessage.Text = "Order is finalized; cannot add products.";
                lblAddMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int productId = Convert.ToInt32(hfSelectedProductId.Value);
            int supplierId = Convert.ToInt32(hfSelectedSupplierId.Value);
            if (!int.TryParse(txtAddQty.Text.Trim(), out int qty) || qty <= 0)
            {
                lblAddMessage.Text = "Please enter a valid quantity.";
                lblAddMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            decimal price = 0m;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT sp.SupplierProductPrice, p.ProductSurcharge 
                  FROM SupplierProduct sp
                  INNER JOIN Product p ON sp.ProductID = p.ProductID
                  WHERE sp.ProductID=@ProductID AND sp.SupplierID=@SupplierID", conn))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal supplierPrice = Convert.ToDecimal(reader["SupplierProductPrice"]);
                        decimal surcharge = Convert.ToDecimal(reader["ProductSurcharge"]);
                        price = (supplierPrice + (supplierPrice * surcharge / 100m)) * qty;
                    }
                    else
                    {
                        lblAddMessage.Text = "Could not find supplier product pricing.";
                        lblAddMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }

            string insertQuery = @"
                INSERT INTO OrderSupplierProduct 
                (OrderID, ProductID, SupplierID, OrderSupplierProductQuantity, OrderSupplierProductPrice, OrderSupplierProductStatus)
                VALUES (@OrderID, @ProductID, @SupplierID, @Quantity, @Price, 0)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                cmd.Parameters.AddWithValue("@Price", price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadOrderProducts(orderId);
            CalculateAndDisplayRunningTotal(orderId);

            lblAddMessage.ForeColor = System.Drawing.Color.Green;
            lblAddMessage.Text = "Product added to order.";
            txtAddQty.Text = "";
            hfSelectedProductId.Value = "";
            hfSelectedSupplierId.Value = "";
            hfSelectedProductPrice.Value = "";
            lblSelectedSupplierProduct.Text = "(no product selected)";
        }

        protected void gvOrderProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int orderId = Convert.ToInt32(hfSelectedOrderId.Value);
            if (IsOrderFinalized(orderId)) { e.Cancel = true; return; }

            GridViewRow row = gvOrderProducts.Rows[e.RowIndex];
            int productId = Convert.ToInt32(gvOrderProducts.DataKeys[e.RowIndex].Values["ProductID"]);
            int supplierId = Convert.ToInt32(gvOrderProducts.DataKeys[e.RowIndex].Values["SupplierID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM OrderSupplierProduct WHERE OrderID=@OrderID AND ProductID=@ProductID AND SupplierID=@SupplierID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@SupplierID", supplierId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadOrderProducts(orderId);
            CalculateAndDisplayRunningTotal(orderId);
        }

        private void CalculateAndDisplayRunningTotal(int orderId)
        {
            decimal total = 0m;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(OrderSupplierProductPrice),0) FROM OrderSupplierProduct WHERE OrderID=@OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                total = Convert.ToDecimal(cmd.ExecuteScalar());
            }
            lblRunningTotal.Text = total.ToString("C2");
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e) { /* show modal */ }

        protected void btnConfirmFinalize_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(hfSelectedOrderId.Value);
            if (IsOrderFinalized(orderId)) return;

            decimal total = 0m;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT ISNULL(SUM(OrderSupplierProductPrice),0) FROM OrderSupplierProduct WHERE OrderID=@OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                total = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Update Order
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE [Order] SET OrderAmount=@Amount, OrderStatus=1, UserName=@UserName WHERE OrderID=@OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", total);
                    cmd.Parameters.AddWithValue("@UserName", Session["UserName"] ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();
                }

                // Payment
                using (SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Payment WHERE OrderID=@OrderID", conn))
                {
                    check.Parameters.AddWithValue("@OrderID", orderId);
                    int cnt = Convert.ToInt32(check.ExecuteScalar());
                    if (cnt > 0)
                    {
                        using (SqlCommand upd = new SqlCommand(
                            "UPDATE Payment SET PaymentTotal=@Total, PaymentDue=@Due, PaymentSurplus=0, PaymentStatus=0 WHERE OrderID=@OrderID", conn))
                        {
                            upd.Parameters.AddWithValue("@Total", total);
                            upd.Parameters.AddWithValue("@Due", total);
                            upd.Parameters.AddWithValue("@OrderID", orderId);
                            upd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand ins = new SqlCommand(
                            "INSERT INTO Payment (OrderID, PaymentTotal, PaymentDue, PaymentSurplus, PaymentStatus) VALUES (@OrderID,@Total,@Due,0,0)", conn))
                        {
                            ins.Parameters.AddWithValue("@OrderID", orderId);
                            ins.Parameters.AddWithValue("@Total", total);
                            ins.Parameters.AddWithValue("@Due", total);
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Mark client items completed
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE ClientOrderProduct SET ClientOrderProductStatus=1 WHERE OrderID=@OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();
                }

                // Mark supplier items completed
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE OrderSupplierProduct SET OrderSupplierProductStatus=1 WHERE OrderID=@OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();
                }
            }

            // --------------------- CLIENT EMAIL ---------------------
            string clientEmail = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT c.ClientEmail FROM [Order] o INNER JOIN Client c ON o.ClientID=c.ClientID WHERE o.OrderID=@OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                object res = cmd.ExecuteScalar();
                if (res != null) clientEmail = res.ToString();
            }

            if (!string.IsNullOrEmpty(clientEmail))
            {
                // Get product details directly from OrderSupplierProduct
                DataTable clientProducts = new DataTable();
                using (SqlCommand cmd = new SqlCommand(@"
                SELECT p.ProductName, osp.OrderSupplierProductQuantity AS Quantity,
                       (osp.OrderSupplierProductPrice / osp.OrderSupplierProductQuantity) AS UnitPrice,
                       osp.OrderSupplierProductPrice AS LineTotal
                FROM OrderSupplierProduct osp
                INNER JOIN Product p ON osp.ProductID = p.ProductID
                WHERE osp.OrderID = @OrderID", new SqlConnection(connectionString)))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(clientProducts);
                }

                // Build a clean HTML email
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html><body style='font-family:Arial,sans-serif;'>");
                sb.AppendLine($"<h2>Order Confirmation - Order #{orderId}</h2>");
                sb.AppendLine("<p>Dear Customer,<br>Thank you for your order! Here are your order details:</p>");
                sb.AppendLine("<table border='1' cellspacing='0' cellpadding='6' style='border-collapse:collapse; width:80%;'>");
                sb.AppendLine("<tr style='background-color:#f2f2f2; text-align:left;'><th>Product</th><th>Quantity</th><th>Unit Price</th><th>Line Total</th></tr>");

                decimal clienttotal = 0m;
                foreach (DataRow row in clientProducts.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                    decimal lineTotal = Convert.ToDecimal(row["LineTotal"]);
                    clienttotal += lineTotal;

                    sb.AppendLine($"<tr><td>{productName}</td><td>{qty}</td><td>R{unitPrice:F2}</td><td>R{lineTotal:F2}</td></tr>");
                }

                sb.AppendLine($"<tr><td colspan='3' style='text-align:right;'><b>Total Price:</b></td><td><b>R{clienttotal:F2}</b></td></tr>");
                sb.AppendLine("</table>");
                sb.AppendLine("<p>Please send proof of payment to <b>DNDTrading22@gmail.com</b>.</p>");
                sb.AppendLine("<p>Kind regards,<br><b>DND Trading Team</b></p>");
                sb.AppendLine("</body></html>");

                // Send as HTML email
                SendEmail(clientEmail, $"Your Order #{orderId} - DND Trading", sb.ToString(), true);
            }


            // 2. Emails to suppliers
            DataTable suppliers = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT DISTINCT s.SupplierID, s.SupplierEmail, s.SupplierName 
                  FROM OrderSupplierProduct osp
                  INNER JOIN Supplier s ON osp.SupplierID=s.SupplierID
                  WHERE osp.OrderID=@OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(suppliers);
            }

            foreach (DataRow sRow in suppliers.Rows)
            {
                string supplierEmail = sRow["SupplierEmail"].ToString();
                string supplierName = sRow["SupplierName"].ToString();

                DataTable supplierProducts = new DataTable();
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT p.ProductName, osp.OrderSupplierProductQuantity, osp.OrderSupplierProductPrice
                  FROM OrderSupplierProduct osp
                  INNER JOIN Product p ON osp.ProductID=p.ProductID
                  WHERE osp.OrderID=@OrderID AND osp.SupplierID=@SupplierID", new SqlConnection(connectionString)))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.Parameters.AddWithValue("@SupplierID", sRow["SupplierID"]);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(supplierProducts);
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html><body style='font-family:Arial,sans-serif;'>");
                sb.AppendLine($"<h2 style='color:#2c3e50;'>New Order Request - Order #{orderId}</h2>");
                sb.AppendLine($"<p>Dear {supplierName},</p>");
                sb.AppendLine("<p>Please prepare the following items for delivery to <b>DND Trading</b>:</p>");
                sb.AppendLine("<table border='1' cellspacing='0' cellpadding='6' style='border-collapse:collapse; width:80%;'>");
                sb.AppendLine("<tr style='background-color:#f2f2f2; text-align:left;'><th>Product</th><th>Quantity</th><th>Price</th></tr>");

                decimal supplierTotal = 0m;
                foreach (DataRow row in supplierProducts.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    int qty = Convert.ToInt32(row["OrderSupplierProductQuantity"]);
                    decimal price = Convert.ToDecimal(row["OrderSupplierProductPrice"]);
                    supplierTotal += price;

                    sb.AppendLine($"<tr><td>{productName}</td><td>{qty}</td><td>R{price:F2}</td></tr>");
                }

                sb.AppendLine($"<tr><td colspan='2' style='text-align:right;'><b>Total Price:</b></td><td><b>R{supplierTotal:F2}</b></td></tr>");
                sb.AppendLine("</table>");
                sb.AppendLine("<p>Please confirm receipt of this order and estimated delivery time.</p>");
                sb.AppendLine("<p>Kind regards,<br><b>DND Trading Team</b><br><small>DNDTrading22@gmail.com</small></p>");
                sb.AppendLine("</body></html>");

                SendEmail(supplierEmail, $"New Order Request #{orderId} - DND Trading", sb.ToString(), true);
            }

            LoadOrderProducts(orderId);
            LoadClientOrderProducts(orderId);
            SetEditingEnabled(false);

            // Hide remove column after finalization
            if (gvOrderProducts.Columns.Count > 0)
                gvOrderProducts.Columns[gvOrderProducts.Columns.Count - 1].Visible = false;

            CalculateAndDisplayRunningTotal(orderId);
        }

        private void SendEmail(string toEmail, string subject, string body, bool isHtml = false)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("DNDTrading22@gmail.com");
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isHtml;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("DNDTrading22@gmail.com", "qyax myny exec tzrb");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvClientOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientOrders.PageIndex = e.NewPageIndex;
            LoadClientOrders(chkPending.Checked);
        }

        protected void gvSupplierProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSupplierProducts.PageIndex = e.NewPageIndex;
            LoadSupplierProducts();
        }

        protected void btnGoToSupplierProducts_Click(object sender, EventArgs e)
        {
            // Store the originating order in Session
            Session["FromClientOrders"] = true;
            Session["CurrentOrderID"] = hfSelectedOrderId.Value;

            Response.Redirect("SupplierProducts.aspx");
        }

        private void LoadPaymentInfo(int orderId)
        {
            string query = @"
        SELECT PaymentID, PaymentTotal, PaymentDue, PaymentSurplus,
               CASE WHEN PaymentStatus = 1 THEN 'Paid' ELSE 'Pending' END AS PaymentStatusText
        FROM Payment
        WHERE OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvPayment.DataSource = dt;
                    gvPayment.DataBind();
                }
            }
        }

        protected void btnUpdatePayment_Click(object sender, EventArgs e)
        {
            lblPaymentMessage.Text = "";
            lblPaymentMessage.ForeColor = System.Drawing.Color.Green;

            if (!decimal.TryParse(txtPaymentAmount.Text.Trim(), out decimal paymentAmount) || paymentAmount <= 0)
            {
                lblPaymentMessage.Text = "Please enter a valid payment amount.";
                lblPaymentMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int orderId = Convert.ToInt32(hfSelectedOrderId.Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Get current payment info
                decimal paymentDue = 0m;
                decimal paymentSurplus = 0m;
                bool paymentStatus = false;

                using (SqlCommand cmd = new SqlCommand("SELECT PaymentDue, PaymentSurplus, PaymentStatus FROM Payment WHERE OrderID=@OrderID", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paymentDue = Convert.ToDecimal(reader["PaymentDue"]);
                            paymentSurplus = Convert.ToDecimal(reader["PaymentSurplus"]);
                            paymentStatus = Convert.ToBoolean(reader["PaymentStatus"]);
                        }
                        else
                        {
                            lblPaymentMessage.Text = "Payment record not found.";
                            lblPaymentMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }

                decimal newDue = paymentDue - paymentAmount;
                decimal newSurplus = paymentSurplus;

                if (newDue < 0)
                {
                    newSurplus += Math.Abs(newDue);
                    newDue = 0;
                }

                bool newStatus = newDue == 0;

                using (SqlCommand updateCmd = new SqlCommand(
                    @"UPDATE Payment
              SET PaymentDue=@PaymentDue, PaymentSurplus=@PaymentSurplus, PaymentStatus=@PaymentStatus
              WHERE OrderID=@OrderID", conn))
                {
                    updateCmd.Parameters.AddWithValue("@PaymentDue", newDue);
                    updateCmd.Parameters.AddWithValue("@PaymentSurplus", newSurplus);
                    updateCmd.Parameters.AddWithValue("@PaymentStatus", newStatus);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderId);

                    updateCmd.ExecuteNonQuery();
                }

                lblPaymentMessage.Text = $"Payment applied successfully. Remaining due: {newDue:C2}, Surplus: {newSurplus:C2}";
                txtPaymentAmount.Text = "";

                // Refresh Payment Grid
                LoadPaymentInfo(orderId);
            }
        }
    }
}
