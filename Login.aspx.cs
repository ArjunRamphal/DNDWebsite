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
                if (Request.QueryString["signup"] == "success")
                {
                    lblMessage.Text = "Signup successful! You can now log in.";
                }
            }
        }

        // LOGIN
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string input = txtLoginEmail.Text.Trim();
            string password = txtLoginPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check Client table
                string clientQuery = "SELECT ClientName, ClientEmail FROM Client WHERE ClientEmail = @Input AND ClientPassword = @Password";
                SqlCommand clientCmd = new SqlCommand(clientQuery, conn);
                clientCmd.Parameters.AddWithValue("@Input", input);
                clientCmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = clientCmd.ExecuteReader();
                if (reader.Read())
                {
                    Session["UserType"] = "Client";
                    Session["UserName"] = reader["ClientName"].ToString();
                    Session["UserEmail"] = reader["ClientEmail"].ToString();
                    reader.Close();

                    lblMessage.Text = "Client login successful! Redirecting...";
                    ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                        "setTimeout(function(){ window.location='Default.aspx'; }, 2000);", true);
                    return;
                }
                reader.Close();

                // Check User table (employees)
                string userQuery = "SELECT UserFirstName, UserLastName, UserType FROM [User] WHERE UserName = @Input AND UserPassword = @Password";
                SqlCommand userCmd = new SqlCommand(userQuery, conn);
                userCmd.Parameters.AddWithValue("@Input", input);
                userCmd.Parameters.AddWithValue("@Password", password);

                reader = userCmd.ExecuteReader();
                if (reader.Read())
                {
                    string fullName = $"{reader["UserFirstName"]} {reader["UserLastName"]}";
                    Session["UserType"] = reader["UserType"].ToString();
                    Session["UserName"] = fullName;
                    Session["UsernameKey"] = input;
                    reader.Close();

                    lblMessage.Text = "User login successful! Redirecting...";
                    ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                        "setTimeout(function(){ window.location='Default.aspx'; }, 2000);", true);
                    return;
                }

                reader.Close();
                lblMessage.Text = "Invalid email/username or password.";
                loginSection.Style["display"] = "block";
                signupSection.Style["display"] = "none";
            }
        }

        // SIGNUP (clients only)
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = txtSignupName.Text.Trim();
            string email = txtSignupEmail.Text.Trim();
            string phone = txtSignupPhone.Text.Trim();
            string password = txtSignupPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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
                    string insertQuery = @"INSERT INTO Client 
                                           (ClientName, ClientPhoneNumber, ClientEmail, ClientPassword, ClientOptOut) 
                                           VALUES (@Name, @Phone, @Email, @Password, @OptOut)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Name", name);
                    insertCmd.Parameters.AddWithValue("@Phone", phone);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@OptOut", 0);

                    int rows = insertCmd.ExecuteNonQuery();
                    if (rows > 0)
                        Response.Redirect("Login.aspx?signup=success");
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
