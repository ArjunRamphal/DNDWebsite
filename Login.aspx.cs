using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

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
                    lblMessage.Text = "Signup successful! You can now log in.";

                if (Request.QueryString["setpassword"] == "1")
                {
                    string email = Request.QueryString["email"];
                    txtSignupEmail.Text = email;
                    txtSignupEmail.ReadOnly = true;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT ClientName, ClientPhoneNumber FROM Client WHERE ClientEmail = @Email";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string clientName = reader["ClientName"] as string;
                                string clientPhone = reader["ClientPhoneNumber"] as string;

                                if (!string.IsNullOrEmpty(clientName))
                                {
                                    txtSignupName.Text = clientName;
                                    txtSignupName.ReadOnly = true;
                                }
                                else
                                {
                                    txtSignupName.Visible = true;
                                    txtSignupName.ReadOnly = false;
                                }

                                if (!string.IsNullOrEmpty(clientPhone))
                                {
                                    txtSignupPhone.Text = clientPhone;
                                    txtSignupPhone.ReadOnly = true;
                                }
                                else
                                {
                                    txtSignupPhone.Visible = true;
                                    txtSignupPhone.ReadOnly = false;
                                }
                            }
                        }
                    }

                    signupTitle.InnerText = "Set Your Password";
                    lblSignupMessage.Text = "We found your account. Please set a password and choose a verification question.";

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
                string clientQuery = "SELECT ClientID, ClientName, ClientEmail, ClientPassword FROM Client WHERE ClientEmail = @Input";
                SqlCommand clientCmd = new SqlCommand(clientQuery, conn);
                clientCmd.Parameters.AddWithValue("@Input", input);
                SqlDataReader reader = clientCmd.ExecuteReader();

                if (reader.Read())
                {
                    string existingEmail = reader["ClientEmail"].ToString();
                    string existingPassword = reader["ClientPassword"]?.ToString();

                    if (string.IsNullOrEmpty(existingPassword))
                    {
                        reader.Close();
                        // Redirect to the Set Password flow, pre-filling their email
                        Response.Redirect($"Login.aspx?setpassword=1&email={Server.UrlEncode(existingEmail)}");
                        return;
                    }

                    // CASE-SENSITIVE comparison for email and password
                    if (existingEmail == input && existingPassword == password)
                    {
                        Session["UserType"] = "Client";
                        Session["UserName"] = reader["ClientName"].ToString();
                        Session["UserEmail"] = existingEmail;
                        Session["ClientID"] = reader["ClientID"].ToString();
                        reader.Close();

                        lblMessage.Text = "Client login successful! Redirecting...";
                        ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                            "setTimeout(function(){ window.location='Default.aspx'; }, 2000);", true);
                        return;
                    }
                    else if (existingEmail == input)
                    {
                        lblMessage.Text = "Invalid password.";
                        reader.Close();
                        return;
                    }
                    else
                    {
                        reader.Close();
                    }
                }
                reader.Close();

                // --- USER LOGIN ---
                string userQuery = "SELECT UserName, UserFirstName, UserLastName, UserType, UserPassword, UserOptOut FROM [User] WHERE UserName = @Input";
                SqlCommand userCmd = new SqlCommand(userQuery, conn);
                userCmd.Parameters.AddWithValue("@Input", input);
                reader = userCmd.ExecuteReader();

                if (reader.Read())
                {
                    string existingUserName = reader["UserName"].ToString();
                    string existingPassword = reader["UserPassword"].ToString();
                    bool isOptedOut = reader["UserOptOut"] != DBNull.Value && Convert.ToBoolean(reader["UserOptOut"]);

                    // CASE-SENSITIVE comparison
                    if (existingUserName == input && existingPassword == password)
                    {
                        if (isOptedOut)
                        {
                            lblMessage.Text = "Access Denied: This account is currently inactive. Please contact support if you believe this is a mistake.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            reader.Close();
                            return;
                        }

                        string fullName = $"{reader["UserFirstName"]} {reader["UserLastName"]}";
                        bool isManager = Convert.ToBoolean(reader["UserType"]);
                        Session["UserType"] = isManager ? "Manager" : "Sales Representative";
                        Session["UserName"] = reader["UserName"].ToString();
                        Session["UsernameKey"] = input;
                        reader.Close();

                        lblMessage.Text = "User login successful! Redirecting...";
                        ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                            "setTimeout(function(){ window.location='Default.aspx'; }, 2000);", true);
                        return;
                    }
                    else if (existingUserName == input)
                    {
                        lblMessage.Text = "Invalid password.";
                        reader.Close();
                        return;
                    }
                    else
                    {
                        reader.Close();
                    }
                }

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
            string question = ddlQuestion.SelectedValue;
            string answer = txtSignupAnswer.Text.Trim();

            bool isSetPasswordMode = hfSetPasswordMode.Value == "true";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(question) || string.IsNullOrEmpty(answer))
            {
                lblSignupMessage.Text = "Please fill in email, password, and verification fields.";
                signupSection.Style["display"] = "block";
                loginSection.Style["display"] = "none";
                signupSection.Visible = true;
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblSignupMessage.Text = "Please enter a valid email address.";
                signupSection.Style["display"] = "block";
                loginSection.Style["display"] = "none";
                signupSection.Visible = true;
                return;
            }

            if (!isSetPasswordMode || (isSetPasswordMode && !txtSignupName.ReadOnly))
            {
                if (string.IsNullOrEmpty(name))
                {
                    lblSignupMessage.Text = "Please fill in all fields (Name).";
                    signupSection.Style["display"] = "block";
                    loginSection.Style["display"] = "none";
                    signupSection.Visible = true;
                    return;
                }

                if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
                {
                    lblSignupMessage.Text = "Name must contain only letters and spaces.";
                    signupSection.Style["display"] = "block";
                    loginSection.Style["display"] = "none";
                    signupSection.Visible = true;
                    return;
                }
            }

            if (!isSetPasswordMode || (isSetPasswordMode && !txtSignupPhone.ReadOnly))
            {
                if (string.IsNullOrEmpty(phone))
                {
                    lblSignupMessage.Text = "Please fill in all fields (Phone).";
                    signupSection.Style["display"] = "block";
                    loginSection.Style["display"] = "none";
                    signupSection.Visible = true;
                    return;
                }
                if (!Regex.IsMatch(phone, @"^0\d{9}$"))
                {
                    lblSignupMessage.Text = "Phone number must be 10 digits long, numbers only, and start with 0.";
                    signupSection.Style["display"] = "block";
                    loginSection.Style["display"] = "none";
                    signupSection.Visible = true;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // --- CHECK IF EMAIL EXISTS ---
                string checkQuery = "SELECT ClientPassword FROM Client WHERE ClientEmail = @Email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                object result = checkCmd.ExecuteScalar();

                // === CASE B or C: EMAIL EXISTS ===
                if (result != null)
                {
                    string existingPassword = (result == DBNull.Value) ? null : result.ToString();

                    // === CASE B: Password not set yet (NULL or empty) → UPDATE ===
                    if (string.IsNullOrEmpty(existingPassword))
                    {
                        // Build the query dynamically based on which fields are editable
                        string updateQuery = @"UPDATE Client 
                                               SET ClientPassword=@Password, 
                                                   ClientQuestion=@Question, 
                                                   ClientAnswer=@Answer";

                        if (!txtSignupName.ReadOnly)
                        {
                            updateQuery += ", ClientName=@Name";
                        }
                        if (!txtSignupPhone.ReadOnly)
                        {
                            updateQuery += ", ClientPhoneNumber=@Phone";
                        }
                        updateQuery += " WHERE ClientEmail=@Email";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);

                        // Add parameters
                        updateCmd.Parameters.AddWithValue("@Password", password);
                        updateCmd.Parameters.AddWithValue("@Question", question);
                        updateCmd.Parameters.AddWithValue("@Answer", answer);
                        updateCmd.Parameters.AddWithValue("@Email", email);

                        if (!txtSignupName.ReadOnly)
                        {
                            updateCmd.Parameters.AddWithValue("@Name", name);
                        }
                        if (!txtSignupPhone.ReadOnly)
                        {
                            updateCmd.Parameters.AddWithValue("@Phone", phone);
                        }

                        updateCmd.ExecuteNonQuery();

                        Response.Redirect("Login.aspx?signup=success");
                        return;
                    }
                    else
                    {
                        // === CASE C: Email exists AND password already set ===
                        lblSignupMessage.Text = "An account with this email already exists.";
                        signupSection.Style["display"] = "block";
                        loginSection.Style["display"] = "none";
                        signupSection.Visible = true;
                        return;
                    }
                }

                // === CASE A: Email does not exist → INSERT ===
                string insertQuery = @"INSERT INTO Client 
                               (ClientName, ClientPhoneNumber, ClientEmail, ClientPassword, ClientOptOut, ClientQuestion, ClientAnswer)
                               VALUES (@Name, @Phone, @Email, @Password, 0, @Question, @Answer)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Name", name);
                insertCmd.Parameters.AddWithValue("@Phone", phone);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Password", password);
                insertCmd.Parameters.AddWithValue("@Question", question);
                insertCmd.Parameters.AddWithValue("@Answer", answer);
                insertCmd.ExecuteNonQuery();

                Response.Redirect("Login.aspx?signup=success");
            }
        }

        // PASSWORD RESET - GET VERIFICATION QUESTION
        protected void btnGetQuestion_Click(object sender, EventArgs e)
        {
            string email = txtResetEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                lblResetMessage.Text = "Please enter your email.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ClientQuestion FROM Client WHERE ClientEmail=@Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    txtResetEmail.Visible = false;
                    btnGetQuestion.Visible = false;

                    loginSection.Visible = false;
                    resetSection.Style["display"] = "block";
                    resetSection.Visible = true;

                    pnlVerification.Visible = true;

                    lblVerificationQuestion.Text = "Verification Question: " + result.ToString();
                    lblResetMessage.Text = "Please answer your verification question and enter a new password.";

                    form1.DefaultButton = btnResetPassword.UniqueID;

                    ClientScript.RegisterStartupScript(this.GetType(), "scrollTop",
                        "document.getElementById('" + resetSection.ClientID + "').scrollIntoView({behavior:'smooth'});", true);
                }
                else
                {
                    lblResetMessage.Text = "Email not found.";
                    lblVerificationQuestion.Text = "";
                    pnlVerification.Visible = false;
                }
            }
        }

        // PASSWORD RESET - SUBMIT
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string email = txtResetEmail.Text.Trim();
            string answer = txtResetAnswer.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                lblResetMessage.Text = "Passwords do not match.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ClientAnswer FROM Client WHERE ClientEmail=@Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                object result = cmd.ExecuteScalar();
                if (result != null && result.ToString() == answer)
                {
                    string updateQuery = "UPDATE Client SET ClientPassword=@Password WHERE ClientEmail=@Email";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@Password", newPassword);
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    updateCmd.ExecuteNonQuery();

                    lblResetMessage.Text = "Password reset successful. Redirecting to login...";
                    ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                        "setTimeout(function(){ window.location='Login.aspx'; }, 2000);", true);
                }
                else
                {
                    lblResetMessage.Text = "Incorrect answer.";
                }
            }
        }   

        protected void btnBackToHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
