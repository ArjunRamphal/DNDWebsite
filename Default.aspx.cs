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

                pnlAccountLink.Visible = (userType == "Client");

                // Show Products link only to Clients
                pnlProductsLink.Visible = (userType == "Client");

                // Show Orders link only to Clients
                pnlOrdersLink.Visible = (userType == "Client");

                // Show Clients link only to Managers / Sales Representatives
                pnlClientsLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Client Orders only to Managers / Sales Representatives
                pnlClientOrdersLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Suppliers only to Managers / Sales Representatives
                pnlSuppliersLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Supplier Products only to Managers / Sales Representatives
                pnlSupplierProductsLink.Visible = (userType == "Manager" || userType == "Sales Representative");

                // Show Reports only to Managers
                pnlReportLink.Visible = (userType == "Manager");

                pnlAboutLink.Visible = (userType == "Client");
                pnlContactLink.Visible = (userType == "Client");

                // Welcome label
                lblWelcome.Text = "Welcome, " + Session["UserName"];
                lblWelcome.Visible = true;
                btnLogin.Visible = false;
                btnLogout.Visible = true;
            }
            else
            {
                // Not logged in
                pnlAccountLink.Visible = false;
                pnlProductsLink.Visible = false;
                pnlOrdersLink.Visible = false;
                pnlCheckoutLink.Visible = false;
                pnlClientsLink.Visible = false;
                pnlClientOrdersLink.Visible = false;
                pnlSuppliersLink.Visible = false;
                pnlSupplierProductsLink.Visible = false;
                pnlReportLink.Visible = false;
                pnlAboutLink.Visible = true;
                pnlContactLink.Visible = true;

                lblWelcome.Visible = false;
                btnLogin.Visible = true;
                btnLogout.Visible = false;
            }


            if (!IsPostBack)
            {
                LoadStats();
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
            pnlReportLink.Visible = false;
            pnlAccountLink.Visible = false;
            pnlAboutLink.Visible = true;
            pnlContactLink.Visible = true;

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
                    pnlAboutLink.Visible = true;
                    pnlContactLink.Visible = true;
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

                                if (userType == "Manager")
                                {
                                    pnlClientOrdersLink.Visible = true;
                                    pnlSuppliersLink.Visible = true;
                                    pnlSupplierProductsLink.Visible = true;
                                    pnlReportLink.Visible = true;
                                }
                                else if (userType == "Sales Representative")
                                {
                                    pnlClientOrdersLink.Visible = true;
                                    pnlSuppliersLink.Visible = true;
                                    pnlSupplierProductsLink.Visible = true;
                                }
                            }
                            reader.Close();
                        }
                    }
                }
            }
        }


        private void LoadStats()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1️⃣ Completed Orders
                    int ordersCount = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [Order]", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        ordersCount = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    }
                    lblOrdersCount.Text = ordersCount.ToString("N0"); // formats with commas

                    // 2️⃣ Products Sold
                    int productsSold = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(OrderSupplierProductQuantity) FROM [OrderSupplierProduct]", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        productsSold = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    }
                    lblSalesCount.Text = productsSold.ToString("N0");

                    // 3️⃣ Happy Clients
                    int clientsCount = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [Client]", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        clientsCount = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    }
                    lblClientsCount.Text = clientsCount.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading stats: " + ex.Message;
            }
        }


        protected void btnHelp_Click(object sender, EventArgs e)
        {

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
