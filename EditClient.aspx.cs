using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class EditClient : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        // Fixed list of security questions (exact text)
        private readonly string[] SecurityQuestions = new string[]
        {
            "What was your childhood nickname?",
            "What was the name of your first pet?",
            "What is your mother's maiden name?",
            "What is your favourite colour?",
            "What is your favourite fruit?"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only clients allowed
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Client")
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PopulateQuestions();
                LoadClientData();
            }
        }

        private void PopulateQuestions()
        {
            ddlQuestion.Items.Clear();
            foreach (var q in SecurityQuestions)
            {
                ddlQuestion.Items.Add(q);
            }
            ddlQuestion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select a question --", ""));
        }

        private void LoadClientData()
        {
            if (Session["ClientID"] == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "No client session found.";
                return;
            }

            int clientId;
            if (!int.TryParse(Session["ClientID"].ToString(), out clientId))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid client session.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT ClientName, ClientPhoneNumber, ClientEmail, ClientQuestion, ClientAnswer, ClientOptOut FROM Client WHERE ClientID = @ClientID", conn))
            {
                cmd.Parameters.AddWithValue("@ClientID", clientId);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        txtName.Text = rdr["ClientName"] as string ?? "";
                        txtEmail.Text = rdr["ClientEmail"] as string ?? "";
                        txtPhone.Text = rdr["ClientPhoneNumber"] as string ?? "";

                        string dbQuestion = rdr["ClientQuestion"] as string ?? "";
                        string dbAnswer = rdr["ClientAnswer"] as string ?? "";
                        bool optOut = rdr["ClientOptOut"] != DBNull.Value && Convert.ToBoolean(rdr["ClientOptOut"]);

                        // preselect question if it matches one of our list (exact text)
                        if (!string.IsNullOrEmpty(dbQuestion))
                        {
                            var item = ddlQuestion.Items.FindByText(dbQuestion);
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                        }

                        // store original question so we can detect change on save
                        hfOriginalQuestion.Value = dbQuestion;

                        // prefill answer (only shown if user wants to change)
                        txtAnswer.Text = dbAnswer;

                        chkOptOut.Checked = optOut;

                        // password must remain empty on load (per requirements)
                        txtPassword.Text = "";
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Client not found.";
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblMessage.ForeColor = System.Drawing.Color.Green;

            if (Session["ClientID"] == null)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Session expired. Please log in again.";
                return;
            }

            if (!int.TryParse(Session["ClientID"].ToString(), out int clientId))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid session data.";
                return;
            }

            // Validate basic required fields
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Name is required.";
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Email is required.";
                return;
            }

            // detect question change -> then answer is required
            string originalQuestion = hfOriginalQuestion.Value ?? "";
            string selectedQuestion = ddlQuestion.SelectedItem != null ? ddlQuestion.SelectedItem.Text : "";
            string answer = txtAnswer.Text.Trim();

            bool questionChanged = !string.Equals(originalQuestion ?? "", selectedQuestion ?? "", StringComparison.Ordinal);

            if (questionChanged)
            {
                if (string.IsNullOrEmpty(selectedQuestion))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Please select a security question.";
                    return;
                }

                if (string.IsNullOrEmpty(answer))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "You changed your security question. Please provide an answer.";
                    return;
                }
            }

            // Everything validated -> build update query
            // We'll update only fields that must change. Password only if entered.
            string newPassword = txtPassword.Text; // plain as requested (Q2 plain)
            bool updatePassword = !string.IsNullOrEmpty(newPassword);

            bool optOut = chkOptOut.Checked;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();

                // Build the update command depending on whether password should update
                if (updatePassword)
                {
                    cmd.CommandText = @"
                        UPDATE Client
                        SET ClientName = @Name,
                            ClientEmail = @Email,
                            ClientPhoneNumber = @Phone,
                            ClientPassword = @Password,
                            ClientQuestion = @Question,
                            ClientAnswer = @Answer,
                            ClientOptOut = @OptOut
                        WHERE ClientID = @ClientID";
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                }
                else
                {
                    cmd.CommandText = @"
                        UPDATE Client
                        SET ClientName = @Name,
                            ClientEmail = @Email,
                            ClientPhoneNumber = @Phone,
                            ClientQuestion = @Question,
                            ClientAnswer = @Answer,
                            ClientOptOut = @OptOut
                        WHERE ClientID = @ClientID";
                }

                // common params
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Question", string.IsNullOrEmpty(selectedQuestion) ? (object)DBNull.Value : selectedQuestion);
                cmd.Parameters.AddWithValue("@Answer", string.IsNullOrEmpty(answer) ? (object)DBNull.Value : answer);
                cmd.Parameters.AddWithValue("@OptOut", optOut ? 1 : 0);
                cmd.Parameters.AddWithValue("@ClientID", clientId);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Your information has been updated.";

                    // update hidden original question stored so future saves compare correctly
                    hfOriginalQuestion.Value = selectedQuestion ?? "";

                    // Clear password field after save
                    txtPassword.Text = "";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "No changes were saved.";
                }
            }
        }
    }
}
