using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DNDWebsite
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If redirected after signup, show message
                if (Request.QueryString["signup"] == "success")
                {
                    lblMessage.Text = "Signup successful! You can now log in.";
                }
            }
        }

        // LOGIN
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtLoginEmail.Text.Trim();
            string password = txtLoginPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Client WHERE ClientEmail = @Email AND ClientPassword = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMessage.Text = "Login successful! Redirecting to Home...";
                    Session["UserEmail"] = email;

                    // Redirect after 2 seconds
                    ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function(){ window.location='Default.aspx'; }, 2000);", true);
                }
                else
                {
                    lblMessage.Text = "Invalid email or password.";
                    loginSection.Style["display"] = "block";
                    signupSection.Style["display"] = "none";
                }
            }
        }

        // SIGNUP
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = txtSignupName.Text.Trim();
            string email = txtSignupEmail.Text.Trim();
            string phone = txtSignupPhone.Text.Trim();
            string password = txtSignupPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if email already exists
                string checkQuery = "SELECT COUNT(*) FROM Client WHERE ClientEmail = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    lblSignupMessage.Text = "An account with this email already exists.";
                    loginSection.Style["display"] = "none";
                    signupSection.Style["display"] = "block";
                }
                else
                {
                    // Insert new client
                    string insertQuery = "INSERT INTO Client (ClientName, ClientPhoneNumber, ClientEmail, ClientPassword, ClientOptOut) " +
                                         "VALUES (@Name, @Phone, @Email, @Password, @OptOut)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@Phone", phone);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@OptOut", 0); // default: not opted out

                    int rows = insertCmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        // Redirect to login page with query string to show message
                        Response.Redirect("Login.aspx?signup=success");
                    }
                    else
                    {
                        lblSignupMessage.Text = "Signup failed. Please try again.";
                        loginSection.Style["display"] = "none";
                        signupSection.Style["display"] = "block";
                    }
                }
            }
        }

        protected void btnBackToHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
