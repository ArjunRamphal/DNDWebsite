using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Supplier : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only allow Sales Representative and Manager
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                lblStatus.Text = "";

                pnlBackToProducts.Visible = Request.QueryString["fromSupplierProducts"] == "1";
            }
        }

        protected void btnAddSupplier_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.Text = "";

            string name = txtNewName.Text.Trim();
            string phone = txtNewPhone.Text.Trim();
            string email = txtNewEmail.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblStatus.Text = "Supplier name is required.";
                return;
            }

            if (!string.IsNullOrEmpty(phone) && phone.Length > 10)
            {
                lblStatus.Text = "Phone number too long.";
                return;
            }

            if (!string.IsNullOrEmpty(email) && email.Length > 100)
            {
                lblStatus.Text = "Email too long.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Supplier (SupplierName, SupplierPhoneNumber, SupplierEmail, SupplierOptOut) " +
                    "VALUES (@Name, @Phone, @Email, @OptOut)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OptOut", false);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Clear inputs and show success
                txtNewName.Text = "";
                txtNewPhone.Text = "";
                txtNewEmail.Text = "";

                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "Supplier added successfully.";

                // Refresh GridView
                sdsSuppliers.DataBind();
                gvSuppliers.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error adding supplier: " + ex.Message;
            }
        }

        protected void gvSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSuppliers.PageIndex = e.NewPageIndex;
            gvSuppliers.DataBind();
        }

        protected void btnBackToProducts_Click(object sender, EventArgs e)
        {
            // Keep the session intact so SupplierProducts knows the user came from ClientOrders
            Response.Redirect("SupplierProducts.aspx");
        }
    }
}
