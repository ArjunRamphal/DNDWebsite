using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class Clients : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (Session["UserType"] == null ||
                (Session["UserType"].ToString() != "Sales Representative" &&
                 Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx");
                return;
            }*/
            
            if (Session["UserType"] == null || (Session["UserType"].ToString() != "Sales Representative" && Session["UserType"].ToString() != "Manager"))
            {
                Response.Redirect("Default.aspx");
                return; 
            }

            if (!IsPostBack)
            {
                LoadClientsFromDB();
            }
        }

        private void LoadClientsFromDB()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT ClientID, ClientName, ClientEmail, ClientPhoneNumber 
                         FROM Client
                         WHERE ClientOptOut = 0";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    gvClients.DataSource = dt;
                    gvClients.DataBind();
                }
            }
        }

        protected void gvClients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClients.PageIndex = e.NewPageIndex;
            LoadClientsFromDB();
        }
    }
}
