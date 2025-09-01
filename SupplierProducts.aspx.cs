using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class SupplierProducts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Sales Representative")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                LoadSupplierProducts();
            }
        }

        private void LoadSupplierProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
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

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvSupplierProducts.DataSource = dt;
                    gvSupplierProducts.DataBind();
                }
            }
        }
    }
}
