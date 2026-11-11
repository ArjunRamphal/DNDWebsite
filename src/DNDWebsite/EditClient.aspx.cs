using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace DNDWebsite
{
    public partial class EditClient : Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

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

                        if (!string.IsNullOrEmpty(dbQuestion))
                        {
                            var item = ddlQuestion.Items.FindByText(dbQuestion);
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                        }

                        hfOriginalQuestion.Value = dbQuestion;
                        txtAnswer.Text = dbAnswer;
                        chkOptOut.Checked = optOut;
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

            // --- VALIDATION START ---
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Name is required.";
                return;
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Name must contain only letters and spaces.";
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Email is required.";
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter a valid email address.";
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Phone number is required.";
                return;
            }

            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Phone number must be exactly 10 digits and start with 0.";
                return;
            }
            // --- VALIDATION END ---

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

            string newPassword = txtPassword.Text;
            bool updatePassword = !string.IsNullOrEmpty(newPassword);
            bool optOut = chkOptOut.Checked;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();

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
                    hfOriginalQuestion.Value = selectedQuestion ?? "";
                    txtPassword.Text = "";

                    if (hfExitMode.Value == "true")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ExitSave",
                            "isDirty = false; showStatusModal('Notification', 'Changes saved.');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ResetDirty", "isDirty = false;", true);
                    }
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