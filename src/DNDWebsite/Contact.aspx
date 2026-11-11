<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="DNDWebsite.Contact" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="contact-page">
        <section class="intro fade-in">
            <h2>Contact Us</h2>
            <p>
                Have questions or need a quote? We’d love to hear from you.  
                Reach out to us using the details below or send us a quick message.
            </p>
        </section>

        <section class="contact-info slide-left">
            <div class="info-card">
                <h3>Our Office</h3>
                <p>19 Marriott Road<br />Morningside, Durban, 4001</p>
            </div>
            <div class="info-card">
                <h3>Phone</h3>
                <p>084 437 2450<br />Fax: 086 667 8618</p>
            </div>
            <div class="info-card">
                <h3>Email</h3>
                <p><a href="mailto:dndtrading22@gmail.com">dndtrading22@gmail.com</a></p>
            </div>
        </section>

    <section class="map-section slide-left">
    <h3>
        <span class="map-icon"></span> Find Us
    </h3>

    <div class="map-wrapper">
        <iframe
            width="100%"
            height="380"
            loading="lazy"
            allowfullscreen
            referrerpolicy="no-referrer-when-downgrade"
            src="https://www.google.com/maps?q=19+Marriott+Road+Morningside+Durban&output=embed">
        </iframe>
    </div>

    <a class="map-btn" target="_blank"
       href="https://www.google.com/maps?q=19+Marriott+Road+Morningside+Durban">
        Open in Google Maps
    </a>
    </section>

        <section class="contact-form slide-right">
            <h3>Send Us a Message</h3>
            <asp:Label ID="lblStatus" runat="server" CssClass="status-msg" />
            <div class="form-grid">
                <asp:TextBox ID="txtName" runat="server" CssClass="input-box" placeholder="Your Name" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" placeholder="Your Email" TextMode="Email" />
                <asp:TextBox ID="txtMessage" runat="server" CssClass="input-box message-box" placeholder="Your Message" TextMode="MultiLine" Rows="6" />
                <asp:Button ID="btnSend" runat="server" Text="Send Message" CssClass="btn-send" OnClick="btnSend_Click" />
            </div>
        </section>
    </main>

    <style>
        .contact-page {
            color: #2F4F4F;
            background: #ffffff;
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
            background: #f5f5f5;
            border: 1px solid #4682B4;
            padding: 20px;
            border-radius: 10px;
            width: 250px;
            box-shadow: 0 5px 15px rgba(70, 130, 180, 0.3);
            color: #2F4F4F;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: default;
        }

        /* Hover Effect */
        .info-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 30px rgba(70, 130, 180, 0.5);
            background: #fff;
        }

        .info-card h3 {
            margin-top: 0;
        }

        .contact-form {
            max-width: 600px;
            margin: 0 auto;
            text-align: center; 
        }

        .contact-form h3 {
            text-align: center;
        }

        .form-grid {
            display: flex;
            flex-direction: column;
            gap: 15px;
            align-items: center; 
        }

        .input-box {
            width: 100% !important; 
            box-sizing: border-box; 
            padding: 12px;
            border-radius: 6px;
            border: 1px solid #4682B4;
            background: #f5f5f5;
            font-size: 16px;
            max-width: 800px;
        }

        .message-box {
            resize: none;
            min-height: 120px;
        }

        .btn-send {
            background: #4682B4;
            color: #fff;
            padding: 12px;
            border: none;
            border-radius: 8px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-send:hover {
            background: #5A9BD4;
        }

        .status-msg {
            display: block;
            margin-bottom: 15px;
            font-weight: bold;
            text-align: center;
            color: #4682B4;
        }

        .input-box[readonly] {
            background-color: #e9ecef;
            cursor: not-allowed;
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

    <style>
        .map-section {
            margin: 40px auto 60px auto;
            max-width: 800px;
            text-align: center;
            color: #2F4F4F;
        }

        .map-section h3 {
            font-size: 1.8rem;
            margin-bottom: 15px;
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 8px;
        }

        .map-icon {
            font-size: 2rem;
        }

        .map-wrapper {
            width: 100%;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: none;
            margin-bottom: 15px;
            border: none;
        }

        .map-wrapper iframe {
            border: 0;
        }

        .map-wrapper:hover {
            box-shadow: 0 8px 20px rgba(70, 130, 180, 0.25);
        }

        .map-btn {
            display: inline-block;
            background: #4682B4;
            color: #fff;
            padding: 10px 20px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: bold;
            transition: background 0.3s ease;
        }

        .map-btn:hover {
            background: #5A9BD4;
        }

        .map-section.slide-left {
            opacity: 0;
            transform: translateX(-60px);
            transition: all 0.8s ease-out;
        }

        .map-section.visible {
            opacity: 1 !important;
            transform: translateX(0) translateY(0) !important;
        }

        .input-box, .message-box {
            font-family: inherit;
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