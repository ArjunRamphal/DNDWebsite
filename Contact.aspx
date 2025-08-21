<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="DNDWebsite.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="contact-page">
        <!-- Intro -->
        <section class="intro fade-in">
            <h2>📞 Contact Us</h2>
            <p>
                Have questions or need a quote? We’d love to hear from you.  
                Reach out to us using the details below or send us a quick message.
            </p>
        </section>

        <!-- Contact Info -->
        <section class="contact-info slide-left">
            <div class="info-card">
                <h3>🏢 Our Office</h3>
                <p>19 Marriott Road<br />Morningside, Durban, 4001</p>
            </div>
            <div class="info-card">
                <h3>📞 Phone</h3>
                <p>084 437 2450<br />Fax: 086 667 8618</p>
            </div>
            <div class="info-card">
                <h3>📧 Email</h3>
                <p><a href="mailto:dndtrading2@gmail.com">dndtrading2@gmail.com</a></p>
            </div>
        </section>

        <!-- Contact Form -->
        <section class="contact-form slide-right">
            <h3>Send Us a Message</h3>
            <asp:Label ID="lblStatus" runat="server" CssClass="status-msg" />
            <div class="form-grid">
                <asp:TextBox ID="txtName" runat="server" CssClass="input-box" placeholder="Your Name" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" placeholder="Your Email" TextMode="Email" />
                <asp:TextBox ID="txtMessage" runat="server" CssClass="input-box message-box" placeholder="Your Message" TextMode="MultiLine" Rows="5" />
                <asp:Button ID="btnSend" runat="server" Text="Send Message" CssClass="btn-send" OnClick="btnSend_Click" />
            </div>
        </section>
    </main>

    <style>
        .contact-page {
            color: #FFD700;
            background: linear-gradient(to bottom, #000, #111);
            padding: 40px 20px;
            text-align: center;
        }

        .intro {
            max-width: 800px;
            margin: 0 auto 50px auto;
        }

        .contact-info {
            display: flex;
            justify-content: center;
            gap: 30px;
            flex-wrap: wrap;
            margin-bottom: 50px;
        }

        .info-card {
            background: #111;
            border: 1px solid #FFD700;
            padding: 20px;
            border-radius: 10px;
            width: 250px;
            box-shadow: 0 5px 15px rgba(255, 215, 0, 0.3);
        }

        .info-card h3 {
            margin-top: 0;
        }

        .contact-form {
            max-width: 600px;
            margin: 0 auto;
        }

        .form-grid {
            display: flex;
            flex-direction: column;
            gap: 15px;
        }

        .input-box {
            padding: 12px;
            border-radius: 6px;
            border: 1px solid #FFD700;
            background: #000;
            color: #FFD700;
        }

        .message-box {
            resize: none;
        }

        .btn-send {
            background: #FFD700;
            color: #000;
            padding: 12px;
            border: none;
            border-radius: 8px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-send:hover {
            background: #fff;
            color: #000;
            box-shadow: 0 5px 15px rgba(255, 215, 0, 0.4);
        }

        .status-msg {
            display: block;
            margin-bottom: 15px;
            font-weight: bold;
        }

        /* Animations */
        .fade-in, .slide-left, .slide-right {
            opacity: 0;
            transform: translateY(20px);
            transition: all 0.8s ease-out;
        }

        .slide-left {
            transform: translateX(-60px);
        }

        .slide-right {
            transform: translateX(60px);
        }

        .visible {
            opacity: 1 !important;
            transform: translateX(0) translateY(0) !important;
        }
    </style>

    <script>
        // Scroll animations
        document.addEventListener('DOMContentLoaded', function () {
            const elements = document.querySelectorAll('.fade-in, .slide-left, .slide-right');

            function revealOnScroll() {
                elements.forEach(el => {
                    const rect = el.getBoundingClientRect();
                    if (rect.top < window.innerHeight - 100) {
                        el.classList.add('visible');
                    }
                });
            }

            window.addEventListener('scroll', revealOnScroll);
            revealOnScroll();
        });
    </script>
</asp:Content>
