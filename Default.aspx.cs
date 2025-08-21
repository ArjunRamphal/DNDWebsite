using System;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnContact_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "📞 You can reach us at info@dndtrading.com or call 012-345-6789.";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
