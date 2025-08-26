using System;
using System.Net.Mail;

namespace DNDWebsite
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            try
            {
                // Configure the email
                MailMessage mail = new MailMessage();
                mail.To.Add("dndtrading2@gmail.com"); // Company inbox
                mail.From = new MailAddress(txtEmail.Text.Trim());
                mail.Subject = "Website Contact Form: " + txtName.Text.Trim();
                mail.Body = txtMessage.Text.Trim();

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.yourmailserver.com", // placeholder for e.g., smtp.gmail.com
                    Port = 587,                      // Or 25 / 465 depending on provider
                    Credentials = new System.Net.NetworkCredential("your-email", "your-password"),
                    EnableSsl = true
                };

                smtp.Send(mail);

                lblStatus.Text = "Thank you! Your message has been sent.";
                lblStatus.ForeColor = System.Drawing.Color.LightGreen;

                // Clear fields
                txtName.Text = "";
                txtEmail.Text = "";
                txtMessage.Text = "";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Sorry, there was an error sending your message. Try again later.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;

                // For debugging
                // lblStatus.Text += "<br/>" + ex.Message;
            }
        }
    }
}