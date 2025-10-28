<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DNDWebsite._Default" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>DND Trading & General Supplies</title>
<style>
    body { 
        font-family: Arial, sans-serif; 
        margin: 0; 
        background-color: #ffffff; 
        color: #2F4F4F;
    }

    /* Header */
    header { 
        background-color: #2b5c8a; 
        color: #FFFFFF; 
        padding: 20px; 
        display: flex; 
        justify-content: space-between; 
        align-items: center; 
        height: 60px;
        box-shadow: 0 2px 6px rgba(0,0,0,0.15);
    }

    nav a { 
        color: #FFFFFF; 
        text-decoration: none; 
        font-weight: bold; 
        margin-right: 15px; 
        display: inline-block; 
        padding: 5px 10px;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

    nav a:hover { 
        background-color: #3c7cc0; 
        color: #fff; 
    }

    .btn-contact, .btn-login, .btn-logout { 
        background-color: #3c7cc0; 
        color: #FFFFFF; 
        font-weight: bold; 
        padding: 10px 18px; 
        border: none; 
        border-radius: 6px; 
        cursor: pointer; 
        font-size: 0.95rem; 
        transition: all 0.3s ease; 
        margin-left: 8px; 
    }

    .btn-contact:hover, .btn-login:hover, .btn-logout:hover { 
        background-color: #4e8fd6; 
        box-shadow: 0 5px 15px rgba(70,130,180,0.4); 
    }

    #lblWelcome { 
        font-weight: bold; 
        font-size: 1rem; 
        color: #FFFFFF;
        margin-right: 10px; 
    }

    /* Fullscreen Hero Section */
    .hero-fullscreen-container {
        background-image: url('<%= ResolveUrl("~/background.jpg") %>');
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
        width: 100vw;
        height: calc(100vh - 60px);
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
        position: relative;
        overflow: hidden;
    }

    .hero-fullscreen-container::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.45);
        z-index: 1;
    }

    .hero-text-content {
        position: relative; 
        z-index: 2; 
        color: #FFFFFF;
        padding: 20px;
        max-width: 800px;
    }

    .hero-text-content h2 {
        font-size: 2.7rem;
        margin-bottom: 15px;
        text-shadow: 2px 2px 5px rgba(0,0,0,0.6);
    }

    .hero-text-content .company-description { 
        color: #F0F8FF; 
        font-size: 1.25rem; 
        line-height: 1.7; 
        margin: 0;
        text-shadow: 1px 1px 4px rgba(0,0,0,0.8);
    }

    /* Remove grey and white content backgrounds */
    main {
        background: none;
        padding: 0;
        margin: 0;
    }

    footer { 
        background-color: #2b5c8a; 
        text-align: center; 
        padding: 20px; 
        color: #FFFFFF; 
        margin: 0; 
        border-top: 3px solid #1f4466;
        font-size: 0.95rem;
    }

    /* Help Panel */
    .help-overlay {
        display: none;
        position: fixed;
        top: 0; left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0,0,0,0.6);
        justify-content: center;
        align-items: center;
        z-index: 1000;
    }

    .help-box {
        background-color: #fff;
        padding: 25px;
        border-radius: 12px;
        width: 90%;
        max-width: 600px;
        color: #2F4F4F;
        line-height: 1.6;
        position: relative;
    }

    .close-btn {
        position: absolute;
        top: 10px;
        right: 20px;
        font-size: 22px;
        cursor: pointer;
        color: #2b5c8a;
        font-weight: bold;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div>
                <h1 style="margin:0;">DND Trading & General Supplies</h1>
                <nav>
                    <asp:Panel ID="pnlAccountLink" runat="server" Visible="false" style="display:inline;">
                        <a href="EditClient.aspx">Account</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlProductsLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Products.aspx">Products</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlOrdersLink" runat="server" Visible="false" style="display:inline;">
                        <a href="Order.aspx">Orders</a>
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
                    &nbsp;<a href="About.aspx">About Us</a>
                </nav>
            </div>
            <div>
                <asp:Button ID="btnLogin" runat="server" CssClass="btn-login" Text="Login / Sign Up" OnClick="btnLogin_Click" />
                <asp:Label ID="lblWelcome" runat="server" Text="" Visible="false" />
                <asp:Button ID="btnLogout" runat="server" CssClass="btn-logout" Text="Logout" OnClick="btnLogout_Click" Visible="false" />
                <asp:Button ID="btnContact" runat="server" CssClass="btn-contact" Text="Contact Us" OnClick="btnContact_Click" />
                <asp:Button ID="btnHelp" runat="server" Text="Help"  CssClass="btn-contact" OnClientClick="openHelpPanel(); return false;" OnClick="btnHelp_Click" />
            </div>
        </header>

        <div class="hero-fullscreen-container">
            <div class="hero-text-content">
                <h2>Your Smart Solution for Trading and General Supplies</h2>
                <p class="company-description">
                    The right products. Streamlined procurement.<br />
                    A wide selection of stationery and office essentials.<br />
                    Expert support you can rely on.<br />
                    Efficient, accurate order management powered by modern digital tools—<br />
                    the perfect fit for your business needs.
                </p>
            </div>
            <asp:Label ID="lblMessage" runat="server" Text="" />
        </div>
        <main></main>

        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center">
   
          <div id="helpOverlay" class="help-overlay">
              <div class="help-box" aria-orientation="vertical">
                  <span class="close-btn" onclick="closeHelpPanel()">&times;</span>
                  <p>
                      Use this website to explore our offerings, view pricing, and manage your orders - once you're signed in.<br /><br />

                      <b>Getting Started:</b><br />
                      • Click the <b>Login</b> button at the top of the page if you already have an account.<br />
                      • If you’re new, select <b>Register</b> or <b>Sign Up</b> to create an account.<br /><br />

                      <b>After Logging In:</b><br />
                      • You’ll be able to browse products and place orders.<br />
                      • Track your past orders and manage account details.<br />
                      • Access exclusive offers and business deals.<br /><br />

                      <b>Need Help?</b><br />
                      • If you experience any issues signing in, use the <b>Contact Us</b> option at the top to reach support.<br />
                      • For technical assistance or forgotten passwords, our support team is ready to assist you.<br /><br />

                      Thank you for visiting DND Trading & General Supplies - we look forward to serving you!
                  </p>
              </div>
            </div>
      </asp:Panel>

        <footer>
            &copy; <%: DateTime.Now.Year %> DND Trading & General Supplies
        </footer>
    </form>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const reviewPanels = document.querySelectorAll('.review-panel');
            function revealReviews() {
                reviewPanels.forEach(panel => {
                    const rect = panel.getBoundingClientRect();
                    if (rect.top < window.innerHeight && rect.bottom > 0) {
                        panel.classList.add('visible');
                    } else {
                        panel.classList.remove('visible');
                    }
                });
            }
            window.addEventListener('scroll', revealReviews);
            window.addEventListener('resize', revealReviews);
            revealReviews();
        });

        function openHelpPanel() {
            document.getElementById("helpOverlay").style.display = "flex";
        }

        function closeHelpPanel() {
            document.getElementById("helpOverlay").style.display = "none";
        }

        window.onload = function () {
            document.getElementById("helpOverlay").style.display = "none";
        };
    </script>
</body>
</html>