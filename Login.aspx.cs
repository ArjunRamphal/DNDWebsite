using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtLoginEmail.Text;
            string password = txtLoginPassword.Text;

            if (email == "test@example.com" && password == "password123")
            {
                lblMessage.Text = "✅ Login successful! Welcome back.";
            }
            else
            {
                lblMessage.Text = "❌ Invalid login. Try again.";
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = txtSignupName.Text;
            string email = txtSignupEmail.Text;
            string password = txtSignupPassword.Text;

            lblMessage.Text = $"✅ Signup successful! Welcome, {name}.";
        }
    }
}
