using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class SupplierProducts : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Show Back button only if navigated from ClientOrders
                pnlBackToOrders.Visible = Request.QueryString["fromClientOrders"] == "1";

                LoadSupplierDropdowns();
                LoadSupplierProducts();
            }
        }

        private void LoadSupplierDropdowns()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SupplierID, SupplierName FROM Supplier WHERE SupplierOptOut = 0";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Supplier filter dropdown
                ddlFilterSupplier.DataSource = dt;
                ddlFilterSupplier.DataTextField = "SupplierName";
                ddlFilterSupplier.DataValueField = "SupplierID";
                ddlFilterSupplier.DataBind();
                ddlFilterSupplier.Items.Insert(0, new ListItem("-- All Suppliers --", "0"));

                // Supplier dropdown for add product
                ddlSupplier.DataSource = dt;
                ddlSupplier.DataTextField = "SupplierName";
                ddlSupplier.DataValueField = "SupplierID";
                ddlSupplier.DataBind();
            }
        }

        private void LoadSupplierProducts(string searchTerm = "", int supplierId = 0)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        p.ProductName,
                        s.SupplierName,
                        sp.SupplierProductPrice + (p.ProductSurcharge / 100.0 * sp.SupplierProductPrice) AS FinalPrice
                    FROM SupplierProduct sp
                    INNER JOIN Product p ON sp.ProductID = p.ProductID
                    INNER JOIN Supplier s ON sp.SupplierID = s.SupplierID
                    WHERE s.SupplierOptOut = 0";

                if (!string.IsNullOrEmpty(searchTerm))
                    query += " AND p.ProductName LIKE @Search";

                if (supplierId > 0)
                    query += " AND s.SupplierID = @SupplierID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                        cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");

                    if (supplierId > 0)
                        cmd.Parameters.AddWithValue("@SupplierID", supplierId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvSupplierProducts.DataSource = dt;
                    gvSupplierProducts.DataBind();
                }
            }
        }

        protected void gvSupplierProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSupplierProducts.PageIndex = e.NewPageIndex;
            LoadSupplierProducts(txtSearch.Text.Trim(), Convert.ToInt32(ddlFilterSupplier.SelectedValue));
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSupplierProducts(txtSearch.Text.Trim(), Convert.ToInt32(ddlFilterSupplier.SelectedValue));
        }

        protected void ddlFilterSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSupplierProducts(txtSearch.Text.Trim(), Convert.ToInt32(ddlFilterSupplier.SelectedValue));
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductName.Text.Trim()) || string.IsNullOrEmpty(txtPrice.Text.Trim()) || ddlSupplier.SelectedIndex == -1)
            {
                lblMessage.Text = "Please enter all fields.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = @"
                        INSERT INTO Product (ProductName, ProductSurcharge) VALUES (@ProductName, @Surcharge);
                        DECLARE @ProductID int = SCOPE_IDENTITY();
                        INSERT INTO SupplierProduct (ProductID, SupplierID, SupplierProductPrice) 
                        VALUES (@ProductID, @SupplierID, @Price);";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surcharge", string.IsNullOrEmpty(txtSurcharge.Text.Trim()) ? 0 : Convert.ToDecimal(txtSurcharge.Text.Trim()));
                    cmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(ddlSupplier.SelectedValue));
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text.Trim()));
                    cmd.ExecuteNonQuery();

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Product added successfully.";

                    // Clear form
                    txtProductName.Text = "";
                    txtPrice.Text = "";
                    txtSurcharge.Text = "";
                    ddlSupplier.SelectedIndex = 0;

                    LoadSupplierProducts(txtSearch.Text.Trim(), Convert.ToInt32(ddlFilterSupplier.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        protected void btnBackToOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClientOrders.aspx");
        }

    }
}
