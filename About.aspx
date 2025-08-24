
<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DNDWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="about-page">
        <!-- Intro Section -->
        <section class="intro fade-in">
            <h2>About DND Trading & General Supplies</h2>
            <p>
                Based in the heart of Durban, DND Trading & General Supplies is more than just a stationery provider.
                We act as your dedicated procurement partner - connecting businesses with trusted suppliers, securing
                the best prices, and ensuring smooth, reliable delivery of essential goods.
            </p>
        </section>

        <!-- Our Story -->
        <section class="our-story slide-left">
            <img src="images/worker.jpg" alt="Stationery supplies" />
            <div>
                <h3>📖 Our Story</h3>
                <p>
                    What started as a small trading operation has grown into a trusted name in procurement and supply.
                    We specialise in handling high-volume transactions, from comparing supplier catalogues to managing
                    payments and ensuring accurate, on-time delivery.
                </p>
                <p>
                    Our clients know us as the “middle link” they can trust - we do the legwork of sourcing,
                    negotiating, and processing so that you can focus on what matters most: running your business.
                </p>
            </div>
        </section>

        <!-- Mission and Vision -->
        <section class="mission-vision slide-right">
            <div>
                <h3>🎯 Our Mission</h3>
                <p>
                    To streamline procurement through technology and expertise, providing our clients with
                    reliable, cost-effective solutions for all their stationery and office needs.
                </p>
            </div>
            <div>
                <h3>🌍 Our Vision</h3>
                <p>
                    To become South Africa’s leading procurement partner - recognised for efficiency,
                    transparency, and unwavering commitment to customer satisfaction.
                </p>
            </div>
        </section>

        <!-- How We Work -->
        <section class="how-we-work slide-left">
            <h3>⚙️ How We Work</h3>
            <p>
                Our process is simple, but powerful:
            </p>
            <ul>
                <li>Collect client requirements</li>
                <li>Compare catalogues from multiple suppliers</li>
                <li>Select the best combination of price and availability</li>
                <li>Purchase and process on behalf of the client</li>
                <li>Record payments and provide accurate reports</li>
            </ul>
            <img src="images/procurement.jpg" alt="Procurement process" />
        </section>

        <!-- Values -->
        <section class="values slide-right">
            <h3>💡 What Sets Us Apart</h3>
            <div class="value-grid">
                <div class="value-card">✔ Reliability - We deliver what we promise, on time.</div>
                <div class="value-card">✔ Efficiency - Minimising delays and reducing errors.</div>
                <div class="value-card">✔ Transparency - Honest processes, competitive pricing.</div>
                <div class="value-card">✔ Customer-first - Your needs drive every decision we make.</div>
            </div>
        </section>

        <!-- Call to Action -->
        <section class="cta fade-in">
            <h3>Ready to simplify your procurement?</h3>
            <p>
                Partner with DND Trading & General Supplies today and let us handle the complexity -
                so your business can focus on growth.
            </p>
        </section>
    </main>

    <style>
        .about-page {
            color: #FFD700;
            background: linear-gradient(to bottom, #000, #111);
            padding: 40px 20px;
        }

        .about-page section {
            margin: 60px auto;
            max-width: 1000px;
            text-align: center;
        }

        .about-page h2, .about-page h3 {
            margin-bottom: 20px;
        }

        .our-story, .mission-vision, .how-we-work, .values {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 30px;
            flex-wrap: wrap;
            text-align: left;
        }

        .our-story img, .how-we-work img {
            width: 400px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(255, 215, 0, 0.3);
        }

        .mission-vision div {
            flex: 1;
            background: #111;
            padding: 20px;
            border: 1px solid #FFD700;
            border-radius: 10px;
        }

        .values .value-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 20px;
        }

        .values .value-card {
            background: #111;
            border: 1px solid #FFD700;
            padding: 20px;
            border-radius: 10px;
        }

        /* Animation styles */
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
        // Reveal on scroll animations
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