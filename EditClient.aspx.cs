using System;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class EditClient : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }

            if (!IsPostBack)
            {
                // Optional: preload client info here
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: Save changes to database
            lblMessage.Text = "Your information has been updated!";
        }
    }
}
