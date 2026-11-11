<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DNDWebsite.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="about-page">
        <section class="intro fade-in">
            <h2>About DND Trading & General Supplies</h2>
            <p>
                DND Trading & General Supplies is a Durban-based procurement support service built to simplify how 
                businesses manage stationery and office-supply sourcing. Instead of browsing through multiple 
                supplier lists, our system centralises the entire process - making comparisons, tracking requests, 
                and generating organised reports easier than ever.
            </p>
        </section>

        <section class="our-story slide-left">
            <img src="images/worker.jpg" alt="Stationery supplies" />
            <div>
                <h3>Our Story</h3>
                <p>
                    We began as a small trading operation with one goal: reduce the manual workload that businesses 
                    experience when sourcing essential office items. As our clients grew, so did our platform - evolving 
                    from basic procurement assistance into a structured system where admin, comparisons, and 
                    processing could all be managed in one place.
                </p>
                <p>
                    Today, we act as the trusted “middle link” between businesses and suppliers.  
                    We handle the comparison, item selection, request processing, and financial recording - allowing 
                    our clients to focus on running their operations while we manage the admin behind the scenes.
                </p>
            </div>
        </section>

        <section class="mission-vision slide-right">
            <div>
                <h3>Our Mission</h3>
                <p>
                    To streamline procurement through modern tools and clear processes, providing clients with 
                    organised, accurate, and cost-effective solutions for their office and stationery needs.
                </p>
            </div>
            <div>
                <h3>Our Vision</h3>
                <p>
                    To become one of South Africa’s most reliable procurement support platforms - valued for efficiency, 
                    transparency, and our commitment to helping businesses operate smarter, not harder.
                </p>
            </div>
        </section>

        <section class="how-we-work slide-left full-width-section">
            <h3>How We Work</h3>
            <p>We follow a clean, structured process that ensures accuracy and transparency:</p>
            <div class="work-steps">
                <div class="step-card">Collect and confirm client requirements</div>
                <div class="step-card">Compare items from multiple supplier catalogues</div>
                <div class="step-card">Identify the best option based on pricing and availability</div>
                <div class="step-card">Process the request on behalf of the client</div>
                <div class="step-card">Record payments and maintain organised transaction history</div>
                <div class="step-card">Provide clear reports through our system</div>
            </div>
            <img src="images/procurement.jpg" alt="Procurement process" />
        </section>

        <section class="values slide-right">
            <h3>What Sets Us Apart</h3>
           <div class="value-grid">
                <div class="value-card">Reliability - Consistent, accurate processing every time.</div>
                <div class="value-card">Efficiency - Reduced admin and faster procurement workflows.</div>
                <div class="value-card">Transparency - Clear records and simple reporting.</div>
                <div class="value-card">Customer-first - We adapt our process around your requirements.</div>
                <div class="value-card">Modern Tools - A platform built to keep everything organised.</div>
                <div class="value-card">Professional Support - Assistance backed by knowledge and experience.</div>
            </div>
        </section>

        <section class="cta fade-in">
            <h3>Ready to streamline your procurement?</h3>
            <p>
                Partner with DND Trading & General Supplies and let our system handle the admin - so your business 
                can focus on what truly matters.
            </p>
        </section>
    </main>

    <style>
        /* PAGE COLORS */
        .about-page {
            background: #ffffff;
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

        .mission-vision {
            align-items: stretch;
        }

        .our-story img, .how-we-work img {
            width: 400px;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
        }

        /* --- MISSION & VISION BOXES --- */
        .mission-vision div {
            flex: 1;
            background: #f5f5f5;
            padding: 20px;
            border: 1px solid #000;
            border-radius: 10px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: default;
        }

        /* Hover Effect for Mission/Vision */
        .mission-vision div:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 30px rgba(0,0,0,0.2);
            background: #fff;
        }

        .values .value-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
            gap: 20px;
        }

        /* --- VALUE CARDS --- */
        .values .value-card {
            background: #f5f5f5;
            border: 1px solid #000;
            padding: 20px;
            border-radius: 10px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: default;
        }

        /* Hover Effect for Values */
        .values .value-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 15px 25px rgba(0,0,0,0.15);
            background: #fff;
        }

        .full-width-section {
            max-width: 100%;
            text-align: center;
        }

        .work-steps {
            display: flex;
            flex-direction: column;
            gap: 15px;
            margin: 20px 0;
        }

        /* --- STEP CARDS (How We Work) --- */
        .step-card {
            background: #f5f5f5;
            border: 1px solid #000;
            padding: 18px;
            border-radius: 12px;
            font-size: 1.05rem;
            font-weight: 500;
            box-shadow: 0 5px 12px rgba(0,0,0,0.1);
            text-align: center;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            cursor: default;
        }

        /* Hover Effect for Steps */
        .step-card:hover {
            transform: scale(1.02);
            box-shadow: 0 10px 20px rgba(0,0,0,0.15);
            background: #fff;
        }

        /* HEADER */
        header {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            padding: 14px 28px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: rgba(0,0,0,0.56);
            color: #fff;
            z-index: 999;
            backdrop-filter: blur(6px);
        }

        header h1 { 
            font-size:1.1rem; margin:0;
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

        /* BUTTONS */
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

        /* FOOTER */
        footer {
            background: #000;
            color: #fff;
            padding: 18px;
            text-align: center;
            margin-top: 18px;
        }

        footer a {
            color: #fff;
            text-decoration: none;
            margin: 0 10px;
        }

        footer a:hover {
            color: #ccc;
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