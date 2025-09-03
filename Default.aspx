<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="DNDWebsite._Default" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>DND Trading & General Supplies</title>
    <style>
        /* Body */
        body { 
            font-family: Arial, sans-serif; 
            margin: 0; 
            background-color: #FFFFFF; /* white background */ 
            color: #2F4F4F; /* dark slate gray text */ 
        }

        /* Header */
        header { 
            background-color: #4682B4; /* steel gray */ 
            color: #FFFFFF; 
            padding: 20px; 
            display: flex; 
            justify-content: space-between; 
            align-items: center; 
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
            background-color: #5A9BD4; 
            color: #FFFFFF; 
        }

        /* Buttons */
        .btn-contact, .btn-login, .btn-logout { 
            background-color: #4682B4; 
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
            background-color: #5A9BD4; 
            color: #FFFFFF; 
            box-shadow: 0 5px 15px rgba(70,130,180,0.4); 
        }

        #lblWelcome { 
            font-weight: bold; 
            font-size: 1rem; 
            color: #FFFFFF;
            margin-right: 10px; 
        }

        /* Main content */
        main { 
            padding: 40px 20px; 
            text-align: center; 
            background-color: #F5F5F5; 
            color: #2F4F4F;
        }

        .company-description { 
            color: #2F4F4F; 
            font-size: 1.1rem; 
            line-height: 1.8; 
            margin: 20px 0 40px 0; 
            text-align: center; 
        }

        .best-sellers { 
            background-color: #FFFFFF; 
            padding: 30px 20px; 
            border-radius: 12px; 
            margin: 30px auto; 
            max-width: 1200px; 
            border: 1px solid #4682B4; 
        }

        .product-grid { 
            display: flex; 
            flex-wrap: wrap; 
            justify-content: center; 
            gap: 20px; 
            margin-top: 20px; 
        }

        .product-item { 
            background-color: #FFFFFF; 
            border: 1px solid #4682B4; 
            border-radius: 8px; 
            padding: 15px; 
            width: 200px; 
            color: #2F4F4F; 
            text-align: center; 
            transition: transform 0.3s ease, box-shadow 0.3s ease; 
        }

        .product-item img { 
            width: 100%; 
            height: auto; 
            border-radius: 5px; 
        }

        .product-item h4 { 
            margin: 10px 0 5px; 
        }

        .product-item p { 
            font-size: 0.9rem; 
        }

        .product-item:hover { 
            transform: scale(1.05); 
            box-shadow: 0 10px 20px rgba(70,130,180,0.3); 
        }

        .reviews { 
            display: flex; 
            gap: 20px; 
            flex-wrap: wrap; 
            justify-content: center; 
            max-width: 1000px; 
            margin: 0 auto 60px auto; 
        }

        .review-panel { 
            background-color: #FFFFFF; 
            border: 1px solid #4682B4; 
            padding: 15px; 
            border-radius: 8px; 
            width: 250px; 
            color: #2F4F4F; 
            opacity: 0; 
            transform: translateY(20px); 
            transition: all 0.6s ease-out; 
        }

        .review-panel.visible { 
            opacity: 1; 
            transform: translateY(0); 
        }

        .review-panel h4 { 
            margin-top: 0; 
        }

        footer { 
            background-color: #D3D3D3; 
            text-align: center; 
            padding: 15px; 
            color: #2F4F4F; 
            margin-top: 40px; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div>
                <h1 style="margin:0;">DND Trading & General Supplies</h1>
                <nav>
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

                    <a href="#reviews-section">Reviews</a>
                    <a href="About.aspx">About</a>
                </nav>
            </div>
            <div>
                <asp:Button ID="btnLogin" runat="server" CssClass="btn-login" Text="Login / Sign Up" OnClick="btnLogin_Click" />
                <asp:Label ID="lblWelcome" runat="server" Text="" Visible="false" />
                <asp:Button ID="btnLogout" runat="server" CssClass="btn-logout" Text="Logout" OnClick="btnLogout_Click" Visible="false" />
                <asp:Button ID="btnContact" runat="server" CssClass="btn-contact" Text="Contact Us" OnClick="btnContact_Click" />
            </div>
        </header>

        <main>
            <h2>Your Smart Solution for Trading and General Supplies</h2>
            <p class="company-description">
                The right products. Streamlined procurement.<br />
                A wide selection of stationery and office essentials.<br />
                Expert support you can rely on.<br />
                Efficient, accurate order management powered by modern digital tools-<br />
                the perfect fit for your business needs.
            </p>

            <asp:Label ID="lblMessage" runat="server" Text="" />

            <!-- Best Selling Products Section -->
            <section class="best-sellers">
                <h3>Best Selling Products</h3>
                <div class="product-grid">
                    <div class="product-item">
                        <img src="images/Rotatrim-Reams.jpg" alt="Rotatrim Reams" />
                        <h4>Rotatrim Reams</h4>
                        <p>High-quality paper for professional use.</p>
                    </div>
                    <div class="product-item">
                        <img src="images/pens.jpg" alt="Pens" />
                        <h4>Pens</h4>
                        <p>Reliable pens with smooth ink flow.</p>
                    </div>
                    <div class="product-item">
                        <img src="images/192 pages.jpg" alt="192 Pages Notebook" />
                        <h4>192 Pages Notebook</h4>
                        <p>Durable notebooks perfect for notes and records.</p>
                    </div>
                </div>
            </section>

            <!-- Reviews Section -->
            <h3 style="text-align:center; margin-top:40px;" id="reviews-section">Our Reviews</h3>
            <div class="reviews">
                <div class="review-panel">
                    <h4>Sarah M.</h4>
                    <p>Fast shipping and excellent product quality. Highly recommend!</p>
                </div>
                <div class="review-panel">
                    <h4>John D.</h4>
                    <p>Great customer service and amazing prices. My go-to supplier!</p>
                </div>
                <div class="review-panel">
                    <h4>Lina P.</h4>
                    <p>Super reliable and trustworthy. Never had a single issue.</p>
                </div>
            </div>
        </main>

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
    </script>
</body>
</html>
