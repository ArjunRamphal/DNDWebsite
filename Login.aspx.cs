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
                // After signup success message
                if (Request.QueryString["signup"] == "success")
                    lblMessage.Text = "Signup successful! You can now log in.";

                // Handle "set password" mode
                if (Request.QueryString["setpassword"] == "1")
                {
                    string email = Request.QueryString["email"];
                    txtSignupEmail.Text = email;
                    txtSignupEmail.ReadOnly = true;

                    // Hide name and phone fields in password setup
                    txtSignupName.Visible = false;
                    txtSignupPhone.Visible = false;

                    signupTitle.InnerText = "Set Your Password";
                    lblSignupMessage.Text = "We found your account. Please set a password to activate it.";

                    loginSection.Style["display"] = "none";
                    signupSection.Style["display"] = "block";
                    hfSetPasswordMode.Value = "true";

                    form1.DefaultButton = btnSignup.UniqueID;
                }
                else
                {
                    hfSetPasswordMode.Value = "false";
                    form1.DefaultButton = btnLogin.UniqueID;
                }
            }
            else
            {
                // Maintain correct Enter-key behavior
                if (hfSetPasswordMode.Value == "true")
                    form1.DefaultButton = btnSignup.UniqueID;
                else
                    form1.DefaultButton = btnLogin.UniqueID;
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

                // --- CLIENT LOGIN ---
                string clientQuery = "SELECT ClientName, ClientEmail, ClientPassword FROM Client WHERE ClientEmail = @Input";
                SqlCommand clientCmd = new SqlCommand(clientQuery, conn);
                clientCmd.Parameters.AddWithValue("@Input", input);

                SqlDataReader reader = clientCmd.ExecuteReader();
                if (reader.Read())
                {
                    object passObj = reader["ClientPassword"];
                    string existingPassword = passObj == DBNull.Value ? null : passObj.ToString();

                    if (string.IsNullOrEmpty(existingPassword))
                    {
                        // Client exists but has no password → redirect to set password
                        string email = reader["ClientEmail"].ToString();
                        reader.Close();
                        Response.Redirect($"Login.aspx?setpassword=1&email={email}");
                        return;
                    }

                    if (existingPassword == password)
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
                    else
                    {
                        lblMessage.Text = "Invalid password.";
                        reader.Close();
                        return;
                    }
                }
                reader.Close();

                // --- USER LOGIN (EMPLOYEES) ---
                string userQuery = "SELECT UserFirstName, UserLastName, UserType FROM [User] WHERE UserName = @Input AND UserPassword = @Password";
                SqlCommand userCmd = new SqlCommand(userQuery, conn);
                userCmd.Parameters.AddWithValue("@Input", input);
                userCmd.Parameters.AddWithValue("@Password", password);

                reader = userCmd.ExecuteReader();
                if (reader.Read())
                {
                    string fullName = $"{reader["UserFirstName"]} {reader["UserLastName"]}";

                    bool isManager = Convert.ToBoolean(reader["UserType"]);
                    Session["UserType"] = isManager ? "Manager" : "Sales Representative";
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

        // SIGNUP / SET PASSWORD
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = txtSignupName.Text.Trim();
            string email = txtSignupEmail.Text.Trim();
            string phone = txtSignupPhone.Text.Trim();
            string password = txtSignupPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if client exists
                string checkQuery = "SELECT ClientPassword FROM Client WHERE ClientEmail = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                object result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    // Existing client record found
                    if (result == DBNull.Value)
                    {
                        // Update record with new password
                        string updateQuery = "UPDATE Client SET ClientPassword = @Password WHERE ClientEmail = @Email";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Password", password);
                        updateCmd.Parameters.AddWithValue("@Email", email);
                        updateCmd.ExecuteNonQuery();

                        Response.Redirect("Login.aspx?signup=success");
                        return;
                    }
                    else
                    {
                        lblSignupMessage.Text = "An account with this email already exists.";
                        return;
                    }
                }

                // Otherwise create a new client record
                string insertQuery = @"INSERT INTO Client (ClientName, ClientPhoneNumber, ClientEmail, ClientPassword, ClientOptOut) 
                                       VALUES (@Name, @Phone, @Email, @Password, 0)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Name", name);
                insertCmd.Parameters.AddWithValue("@Phone", phone);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.ExecuteNonQuery();

                Response.Redirect("Login.aspx?signup=success");
            }
        }

        protected void btnBackToHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
