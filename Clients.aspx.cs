using System;
using System.Data;

namespace DNDWebsite
{
    public partial class Clients : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx"); // Not authorized
                return;
            }


            if (!IsPostBack)
            {
                LoadDummyClients();
            }
        }

        private void LoadDummyClients()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ClientID", typeof(int));
            dt.Columns.Add("ClientName", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));

            // Dummy clients
            dt.Rows.Add(1, "Alice Johnson", "alice.johnson@example.com", "0824328973");
            dt.Rows.Add(2, "Bob Smith", "bob.smith@example.com", "0812138329");
            dt.Rows.Add(3, "Charlie Brown", "charlie.brown@example.com", "0718129017");

            gvClients.DataSource = dt;
            gvClients.DataBind();
        }
    }
}
