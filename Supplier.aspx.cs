using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvSuppliers.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchSupplier.Text = "";
            gvSuppliers.DataBind();
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

            if (!string.IsNullOrEmpty(phone))
            {
                if (!Regex.IsMatch(phone, @"^0\d{9}$"))
                {
                    lblStatus.Text = "Phone number must be 10 digits long, numbers only, and start with 0.";
                    return;
                }
            }

            if (!string.IsNullOrEmpty(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    lblStatus.Text = "Please enter a valid email address.";
                    return;
                }

                if (email.Length > 50)
                {
                    lblStatus.Text = "Email is too long (max 50 characters).";
                    return;
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                  "INSERT INTO Supplier (SupplierName, SupplierPhoneNumber, SupplierEmail, SupplierOptOut) " +
                  "VALUES (@Name, @Phone, @Email, @OptOut)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);
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
                gvSuppliers.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error adding supplier: " + ex.Message;
            }
        }

        protected void sdsSuppliers_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                if (e.AffectedRows > 0)
                {
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Supplier updated successfully.";
                }
                else
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Update failed. Supplier not found or no changes were made.";
                }
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "An error occurred during the update. Please check the data.";
                e.ExceptionHandled = true;
            }
        }

        protected void sdsSuppliers_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            // Clear any old messages
            lblStatus.Text = "";
            lblStatus.ForeColor = System.Drawing.Color.Red;

            // Get the new values from the command parameters
            string phone = e.Command.Parameters["@SupplierPhoneNumber"].Value?.ToString();
            string email = e.Command.Parameters["@SupplierEmail"].Value?.ToString();

            if (!string.IsNullOrEmpty(phone))
            {
                if (!Regex.IsMatch(phone, @"^0\d{9}$"))
                {
                    lblStatus.Text = "Phone number must be 10 digits long, numbers only, and start with 0.";
                    e.Cancel = true; // Stop the update
                    return;
                }
            }

            if (!string.IsNullOrEmpty(email))
            {
                if (email.Length > 50)
                {
                    lblStatus.Text = "Email is too long (max 50 characters).";
                    e.Cancel = true; // Stop the update
                    return;
                }
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    lblStatus.Text = "Please enter a valid email address.";
                    e.Cancel = true; // Stop the update
                    return;
                }
            }
        }

        protected void gvSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSuppliers.PageIndex = e.NewPageIndex;
            gvSuppliers.DataBind();
        }

        protected void btnBackToProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("SupplierProducts.aspx");
        }
    }
}