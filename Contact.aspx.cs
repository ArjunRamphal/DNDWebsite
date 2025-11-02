using System;
using System.Net.Mail;

namespace DNDWebsite
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            // Optionally disable editing (if you want to keep them fixed)
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Client")
            {
                txtName.ReadOnly = true;
                txtEmail.ReadOnly = true;
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

            try
            {
                MailMessage mail = new MailMessage();

                // Your company inbox (recipient)
                mail.To.Add("dndtrading22@gmail.com");

                // Your company sender address (the one authenticated with SMTP)
                mail.From = new MailAddress("dndtrading22@gmail.com", "DND Website Contact");

                // Allow replies to go to the client's email
                mail.ReplyToList.Add(new MailAddress(txtEmail.Text.Trim()));

                mail.Subject = "Website Contact Form: " + txtName.Text.Trim();
                mail.Body = $"You have received a new message from the DND Website contact form.\n\n" +
                            $"Name: {txtName.Text}\n" +
                            $"Email: {txtEmail.Text}\n\n" +
                            $"Message:\n{txtMessage.Text}\n\n" +
                            $"Sent on: {DateTime.Now}";

                // Configure your SMTP client
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential("dndtrading22@gmail.com", "qyax myny exec tzrb")
                };

                smtp.Send(mail);

                lblStatus.Text = "Thank you! Your message has been sent successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                // Clear fields
                txtName.Text = "";
                txtEmail.Text = "";
                txtMessage.Text = "";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Sorry, there was an error sending your message. Please try again later.";
                lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }
    }
}