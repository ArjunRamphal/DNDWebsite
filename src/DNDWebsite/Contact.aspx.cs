using System;
using System.Net.Mail;

namespace DNDWebsite
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] != null)
            {
                string userType = Session["UserType"].ToString();
                if (userType == "Sales Representative" || userType == "Manager")
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
            }

            if (!IsPostBack)
            {
                // Check if user info is stored in session
                if (Session["UserName"] != null)
                {
                    txtName.Text = Session["UserName"].ToString();
                }

                if (Session["UserEmail"] != null)
                {
                    txtEmail.Text = Session["UserEmail"].ToString();
                }

                if (Session["UserType"] != null && Session["UserType"].ToString() == "Client")
                {
                    txtName.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                lblStatus.Text = "Please fill in all fields.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                lblStatus.Text = "Please enter a valid email address.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            string senderAddress = Environment.GetEnvironmentVariable("DND_SMTP_USER");
            string smtpPassword = Environment.GetEnvironmentVariable("DND_SMTP_PASSWORD");
            string contactAddress = Environment.GetEnvironmentVariable("DND_CONTACT_EMAIL");

            if (string.IsNullOrWhiteSpace(senderAddress) || string.IsNullOrWhiteSpace(smtpPassword))
            {
                lblStatus.Text = "The contact form is temporarily unavailable. Please use the email address shown on this page.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
                return;
            }

            if (string.IsNullOrWhiteSpace(contactAddress))
                contactAddress = senderAddress;

            try
            {
                TimeZoneInfo saZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
                DateTime saTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, saZone);

                using (MailMessage mail = new MailMessage())
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    mail.To.Add(contactAddress);
                    mail.From = new MailAddress(senderAddress, "DND Website Contact");
                    mail.ReplyToList.Add(new MailAddress(txtEmail.Text.Trim()));
                    mail.Subject = "Website Contact Form: " + txtName.Text.Trim();
                    mail.Body = "You have received a new message from the DND Website contact form.\n\n" +
                                $"Name: {txtName.Text}\n" +
                                $"Email: {txtEmail.Text}\n\n" +
                                $"Message:\n{txtMessage.Text}\n\n" +
                                $"Sent on: {saTime:dd MMM yyyy HH:mm} (SAST)";

                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(senderAddress, smtpPassword);
                    smtp.Send(mail);
                }

                lblStatus.Text = "Thank you! Your message has been sent successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                txtName.Text = "";
                txtEmail.Text = "";
                txtMessage.Text = "";
            }
            catch (Exception)
            {
                lblStatus.Text = "Sorry, there was an error sending your message. Please try again later.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }
    }
}
