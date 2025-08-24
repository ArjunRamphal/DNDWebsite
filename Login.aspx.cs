using System;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtLoginEmail.Text.Trim();
            string password = txtLoginPassword.Text.Trim();

            if (email == "test@example.com" && password == "password123")
            {
                lblMessage.Text = "✅ Login successful! Welcome back.";
                // Response.Redirect("Default.aspx"); // uncomment to redirect after login
            }
            else
            {
                lblMessage.Text = "❌ Invalid email or password.";

                // Keep login section visible and hide signup section
                loginSection.Style["display"] = "block";
                signupSection.Style["display"] = "none";
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = txtSignupName.Text.Trim();
            string email = txtSignupEmail.Text.Trim();
            string password = txtSignupPassword.Text.Trim();

            if (name.Length == 0 || email.Length == 0 || password.Length == 0)
            {
                lblMessage.Text = "❌ Please fill in all fields.";

                // Keep signup section visible
                signupSection.Style["display"] = "block";
                loginSection.Style["display"] = "none";
                return;
            }

            // TODO: save user to database
            lblMessage.Text = $"✅ Sign up successful! Welcome, {name}.";

            // Switch back to login section after successful signup
            signupSection.Style["display"] = "none";
            loginSection.Style["display"] = "block";
        }

        protected void btnBackToHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}
