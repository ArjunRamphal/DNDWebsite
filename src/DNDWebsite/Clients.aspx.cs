using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Clients : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null ||
                (Session["UserType"].ToString() != "Sales Representative" &&
                 Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadClientsFromDB();
            }
        }

        private void LoadClientsFromDB(string searchTerm = "", string optOutFilter = "All")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT ClientID, ClientName, ClientEmail, ClientPhoneNumber, ClientOptOut
                    FROM Client
                    WHERE (ClientName LIKE @Search OR ClientEmail LIKE @Search)";

                if (optOutFilter != "All")
                    query += " AND ClientOptOut = @OptOut";

                query += " ORDER BY ClientID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");

                    if (optOutFilter != "All")
                        cmd.Parameters.AddWithValue("@OptOut", Convert.ToBoolean(Convert.ToInt32(optOutFilter)));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvClients.DataSource = dt;
                    gvClients.DataBind();

                    lblMessage.Text = dt.Rows.Count == 0 ? "No clients found." : "";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            string optOut = ddlOptOut.SelectedValue;
            LoadClientsFromDB(search, optOut);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlOptOut.SelectedIndex = 0;
            LoadClientsFromDB();
        }

        protected void gvClients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClients.PageIndex = e.NewPageIndex;
            string search = txtSearch.Text.Trim();
            string optOut = ddlOptOut.SelectedValue;
            LoadClientsFromDB(search, optOut);
        }
    }
}
