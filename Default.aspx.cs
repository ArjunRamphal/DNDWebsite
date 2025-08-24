using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class _Default : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateHeader();
            }
        }

        private void UpdateHeader()
        {
            bool isLoggedIn = Session["UserEmail"] != null;

            if (isLoggedIn)
            {
                string email = Session["UserEmail"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ClientName FROM Client WHERE ClientEmail=@Email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        lblWelcome.Text = "Welcome, " + result.ToString();
                        lblWelcome.Visible = true;
                        btnLogout.Visible = true;
                        btnLogin.Visible = false;
                        pnlProductsLink.Visible = true; // Show Products link
                    }
                }
            }
            else
            {
                lblWelcome.Visible = false;
                btnLogout.Visible = false;
                btnLogin.Visible = true;
                pnlProductsLink.Visible = false; // Hide Products link
            }
        }

        protected void btnContact_Click(object sender, EventArgs e)
        {
            Response.Redirect("Contact.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            UpdateHeader();
        }
    }
}
