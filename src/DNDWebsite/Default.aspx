<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DNDWebsite._Default" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <title>DND Trading & General Supplies</title>
    <style>
        * { 
            box-sizing: border-box; 
            margin: 0; 
            padding: 0; 
        }

        html, body { 
            height: 100%; 
            scroll-behavior: smooth; 
        }

        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            background: #f5f7fa;
        }

        header {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            padding: 15px 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: rgba(0,0,0,0.56);
            color: #fff;
            z-index: 999;
            backdrop-filter: blur(6px);
        }

        header h1 { 
            font-size:1.1rem; 
            margin:0; 
        }

        nav a, .nav-contact { 
            color:#fff; 
            text-decoration:none; 
            margin-right:12px; 
            font-weight:600; 
            padding:6px 10px; 
            border-radius:6px; 
            display:inline-block; 
        }

        nav a:hover, .nav-contact:hover { 
            color:#ccc; 
        }

        .btn-contact, .btn-login, .btn-logout, .btn-help {
             background-color: #111;
             color: #fff;
             padding: 8px 12px;
             border: none;
             border-radius: 8px;
             cursor: pointer;
             font-weight:700;
             margin-left:8px;
             transition: all 0.2s ease;
        }

        .btn-contact:hover, .btn-login:hover, .btn-logout:hover, .btn-help:hover {
            background-color: #333;
            transform: translateY(-2px);
            box-shadow: 0 8px 18px rgba(0,0,0,0.18);
        }

        .hero {
            height: calc(100vh - 60px);
            margin-top: 60px;
            background-image: url('<%= ResolveUrl("~/images/background2.jpeg") %>');
            background-size: cover;
            background-position: center;
            position: relative;
            display:flex;
            align-items:center;
            justify-content:center;
            text-align:center;
        }

        .hero::after {
            content: '';
            position:absolute; inset:0;
            background: linear-gradient(180deg, rgba(0,0,0,0.45), rgba(0,0,0,0.35));
            z-index:1;
        }

        .hero-content { 
            position:relative; 
            z-index:2; 
            color:#fff; 
            padding:24px; 
            max-width:900px; 
        }

        .hero-content h2 { 
            font-size:2.6rem; 
            margin-bottom:8px; 
            text-shadow:2px 2px 10px rgba(0,0,0,0.6); 
        }

        .hero-content p { 
            font-size:1.1rem; 
            opacity:0.95; 
        }

        .stats { 
            display:flex; 
            gap:18px; 
            justify-content:center; 
            padding:56px 18px; 
            background:#fff; 
            flex-wrap:wrap; 
        }
        
        .stat-card { 
            min-width:180px; 
            width:220px; 
            padding:20px; 
            border-radius:12px; 
            background:linear-gradient(180deg,#fff,#f3f8ff); 
            text-align:center; 
            box-shadow:0 12px 30px rgba(27,56,98,0.06); 
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: default;
        }

        .stat-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 20px 40px rgba(27,56,98,0.15);
        }

        .stat-number { 
            font-size:2.4rem; 
            font-weight:800; 
            color:#2b5c8a; 
        }
        
        .stat-label { 
            margin-top:10px; 
            color:#576a78; 
            font-weight:600; 
        }

        footer { 
            background:#2b5c8a; 
            color:#fff; 
            padding:18px; 
            text-align:center; 
            margin-top:18px; 
        }
        
        .socials a { 
            margin:0 10px; 
            color:#fff; 
            text-decoration:none; 
            font-weight:600; 
        }

        .help-overlay { 
            display:none; 
            position:fixed; 
            top:0; 
            left:0; 
            width:100%; 
            height:100%; 
            background:rgba(0,0,0,0.6); 
            justify-content:center; 
            align-items:center; 
            z-index:2000; 
        }
       
        .help-box { 
            background:#fff; 
            padding:24px; 
            border-radius:12px; 
            width:92%; 
            max-width:640px; 
            position:relative; 
        }
       
        .close-btn { 
            position:absolute; 
            right:18px; 
            top:12px; 
            cursor:pointer; 
            font-size:22px; 
            color:#2b5c8a; 
            font-weight:700; 
        }

        @media (max-width:720px) {
            nav a { display:none; }
            .hero-content h2 { font-size:1.8rem; }
        }

        .socials {
            display: flex;
            justify-content: center;
            gap: 18px;
        }

        .social-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 44px;
            height: 44px;
            background: #2b5c8a;
            color: #fff;
            border-radius: 50%;
            font-size: 1.2rem;
            transition: all 0.2s ease;
        }

        .social-icon:hover {
            background: #1d3f70;
            transform: scale(1.1);
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div style="display:flex; align-items:center; gap:12px;">
                <h1 style="margin-right:6px;">DND Trading &amp; General Supplies</h1>
                <nav>
                    <asp:Panel ID="pnlAccountLink" runat="server" Visible="false" style="display:inline;">
                        <a href="EditClient.aspx">Account</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlProductsLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Products.aspx">Make Order</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlOrdersLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Order.aspx">Order History</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlCheckoutLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Checkout.aspx">Checkout</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlClientsLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Clients.aspx">Clients</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlClientOrdersLink" runat="server" Visible="false" style="display:inline;">
                        <a href="ClientOrders.aspx">Client Orders</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlSuppliersLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Supplier.aspx">Suppliers</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlSupplierProductsLink" runat="server" Visible="false" style="display:inline;">
                        <a href="SupplierProducts.aspx">Supplier Products</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlReportLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Report.aspx">Reports</a>
                    </asp:Panel>

                    <asp:Panel ID="pnlAboutLink" runat="server" Visible="false" style="display:inline;"><a href="About.aspx">About Us</a></asp:Panel><asp:Panel ID="pnlContactLink" runat="server" Visible="false" style="display:inline;"><a href="Contact.aspx" class="nav-contact">Contact Us</a></asp:Panel>
                </nav>
            </div>

            <div>
                <asp:Button ID="btnLogin" runat="server" CssClass="btn-login" Text="Login / Sign Up" OnClick="btnLogin_Click" />
                <asp:Label ID="lblWelcome" runat="server" Text="" Visible="false" />
                <asp:Button ID="btnLogout" runat="server" CssClass="btn-logout" Text="Logout" OnClick="btnLogout_Click" Visible="false" />
                <asp:Button ID="btnHelp" runat="server" Text="Help" CssClass="btn-help" OnClientClick="openHelpPanel(); return false;" OnClick="btnHelp_Click" />
            </div>
        </header>

        <!-- HERO -->
        <div class="hero" role="banner">
            <div class="hero-content">
                <h2>Your Smart Solution for Trading &amp; General Supplies</h2>
                <p class="company-description">
                    The right products. Streamlined procurement.<br />
                    Expert support you can rely on.
                </p>
                <div style="margin-top:18px;">
                </div>
            </div>
        </div>

        <main>
            <section class="stats" aria-label="Company statistics">
                <div class="stat-card">
                    <asp:Label ID="lblOrdersCount" runat="server" CssClass="stat-number" Text="0" />
                    <div class="stat-label">Completed Orders</div>
                </div>
                <div class="stat-card">
                    <asp:Label ID="lblSalesCount" runat="server" CssClass="stat-number" Text="0" />
                    <div class="stat-label">Products Sold</div>
                </div>
                <div class="stat-card">
                    <asp:Label ID="lblClientsCount" runat="server" CssClass="stat-number" Text="0" />
                    <div class="stat-label">Happy Clients</div>
                </div>
            </section>
        </main>

        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
            <div id="helpOverlay" class="help-overlay">
                <div class="help-box" aria-orientation="vertical">
                    <span class="close-btn" onclick="closeHelpPanel()">&times;</span>
                    <p>
                        Use this website to explore our offerings and manage your orders - once you're signed in.<br /><br />

                        <b>Getting Started:</b><br />
                        • Click the <b>Login</b> button at the top of the page if you already have an account.<br />
                        • If you’re new, select <b>Register</b> or <b>Sign Up</b> to create an account.<br /><br />

                        <b>After Logging In:</b><br />
                        • You’ll be able to browse products and place orders.<br />
                        • Track your past orders and manage account details.<br /><br />

                        <b>Need Help?</b><br />
                        • If you experience any issues signing in, use the <b>Contact Us</b> option at the top to reach support.<br />
                        • For technical assistance or forgotten passwords, our support team is ready to assist you.<br /><br />

                        Thank you for visiting DND Trading & General Supplies - we look forward to serving you!
                    </p>
                </div>
            </div>
        </asp:Panel>

        <asp:Label ID="lblMessage" runat="server" Text="" />

        <!-- FOOTER -->
<footer style="background:#000; color:#fff; padding:18px; text-align:center; margin-top:18px;">
    <p>&copy; <%: DateTime.Now.Year %> DND Trading &amp; General Supplies</p>
    <div class="socials" style="margin-top:12px;">
    <a href="https://ist-wst-group-22.azurewebsites.net/#" target="_blank" aria-label="Instagram" class="social-icon">
        <i class="fab fa-instagram"></i>
    </a>
    <a href="https://ist-wst-group-22.azurewebsites.net/#" target="_blank" aria-label="Twitter" class="social-icon">
        <i class="fab fa-twitter"></i>
    </a>
    <a href="https://ist-wst-group-22.azurewebsites.net/#" target="_blank" aria-label="Facebook" class="social-icon">
        <i class="fab fa-facebook-f"></i>
    </a>
</div>
</footer>

        <asp:PlaceHolder ID="phChatbotScript" runat="server">
            <script>
                (function () { if (!window.chatbase || window.chatbase("getState") !== "initialized") { window.chatbase = (...arguments) => { if (!window.chatbase.q) { window.chatbase.q = [] } window.chatbase.q.push(arguments) }; window.chatbase = new Proxy(window.chatbase, { get(target, prop) { if (prop === "q") { return target.q } return (...args) => target(prop, ...args) } }) } const onLoad = function () { const script = document.createElement("script"); script.src = "https://www.chatbase.co/embed.min.js"; script.id = "hBuXQYlUTLPyZ-GpRixVm"; script.domain = "www.chatbase.co"; document.body.appendChild(script) }; if (document.readyState === "complete") { onLoad() } else { window.addEventListener("load", onLoad) } })();
            </script>
        </asp:PlaceHolder>

    </form>

   <script>
       // animate label counters
       function animateLabelCountByElement(el, duration) {
           if (!el) return;
           var target = parseInt(el.textContent.replace(/,/g, '')) || 0;
           var start = 0;
           var startTime = null;

           function step(timestamp) {
               if (!startTime) startTime = timestamp;
               var progress = Math.min((timestamp - startTime) / duration, 1);
               el.textContent = Math.floor(progress * target).toLocaleString();
               if (progress < 1) requestAnimationFrame(step);
           }
           requestAnimationFrame(step);
       }

       window.addEventListener('DOMContentLoaded', function () {
           // find labels by ASP.NET suffix
           var ordersLabel = document.querySelector("[id$='lblOrdersCount']");
           var salesLabel = document.querySelector("[id$='lblSalesCount']");
           var clientsLabel = document.querySelector("[id$='lblClientsCount']");

           animateLabelCountByElement(ordersLabel, 1400);
           animateLabelCountByElement(salesLabel, 1600);
           animateLabelCountByElement(clientsLabel, 1200);
       });

       function openHelpPanel() {
           document.getElementById('helpOverlay').style.display = 'flex';
       }

       function closeHelpPanel() {
           document.getElementById('helpOverlay').style.display = 'none';
       }

   </script>
</body>
</html>