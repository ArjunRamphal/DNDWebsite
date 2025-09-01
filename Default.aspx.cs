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
            if (Session["UserType"] != null)
            {
                string userType = Session["UserType"].ToString();

                // Show Products link only to Clients
                pnlProductsLink.Visible = (userType == "Client");

                // Show Orders link only to Clients
                pnlOrdersLink.Visible = (userType == "Client");

                // Show Checkout link only to Clients
                pnlCheckoutLink.Visible = (userType == "Client");

                // Show Clients link only to Managers / Sales Representatives
                pnlClientsLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Client Orders only to Managers / Sales Representatives
                pnlClientOrdersLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Suppliers only to Managers / Sales Representatives
                pnlSuppliersLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Supplier Products only to Managers / Sales Representatives
                pnlSupplierProductsLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Welcome label
                lblWelcome.Text = "Welcome, " + Session["UserName"];
                lblWelcome.Visible = true;
                btnLogin.Visible = false;
                btnLogout.Visible = true;
            }
            else
            {
                // Not logged in
                pnlProductsLink.Visible = false;
                pnlOrdersLink.Visible = false;
                pnlCheckoutLink.Visible = false;
                pnlClientsLink.Visible = false;
                pnlClientOrdersLink.Visible = false;
                pnlSuppliersLink.Visible = false;
                pnlSupplierProductsLink.Visible = false;
                pnlClientsLink.Visible = false;
                lblWelcome.Visible = false;
                btnLogin.Visible = true;
                btnLogout.Visible = false;
            }
        }


        private void UpdateHeader()
        {
            lblWelcome.Visible = false;
            btnLogout.Visible = false;
            btnLogin.Visible = true;

            pnlProductsLink.Visible = false;
            pnlOrdersLink.Visible = false;
            pnlCheckoutLink.Visible = false;
            pnlClientsLink.Visible = false;
            pnlClientOrdersLink.Visible = false;
            pnlSuppliersLink.Visible = false;
            pnlSupplierProductsLink.Visible = false;

            if (Session["UserType"] != null)
            {
                string userType = Session["UserType"].ToString();

                if (userType == "Client" && Session["UserEmail"] != null)
                {
                    string clientName = Session["UserName"].ToString();
                    lblWelcome.Text = $"Welcome, {clientName}";
                    lblWelcome.Visible = true;
                    btnLogout.Visible = true;
                    btnLogin.Visible = false;

                    pnlProductsLink.Visible = true;
                    pnlOrdersLink.Visible = true;
                }
                else
                {
                    string usernameKey = Session["UsernameKey"]?.ToString();
                    if (!string.IsNullOrEmpty(usernameKey))
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            string query = "SELECT UserFirstName, UserLastName FROM [User] WHERE UserName=@UserName";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@UserName", usernameKey);

                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                string fullName = $"{reader["UserFirstName"]} {reader["UserLastName"]}";
                                lblWelcome.Text = $"Welcome, {fullName}";
                                lblWelcome.Visible = true;
                                btnLogout.Visible = true;
                                btnLogin.Visible = false;

                                pnlClientOrdersLink.Visible = true;
                                pnlSuppliersLink.Visible = true;
                                pnlSupplierProductsLink.Visible = true;
                            }
                            reader.Close();
                        }
                    }
                }
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
