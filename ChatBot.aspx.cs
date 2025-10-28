using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNDWebsite
{
    public partial class ChatBot : System.Web.UI.Page
    {

        private static readonly Dictionary<string, string> botResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
           {"hello", "Hello! Welcome to DnD Trading and General Supplies. How can I assist you today? You can type 'Keywords' if you are unsure what to ask"},
    {"hi", "Hi there! How can I help you with your POS tasks?"},
    {"hey", "Hey! How can I support you with DnD Trading’s system?"},
    {"help", "Sure! I can help with clients, orders, suppliers, inventory, payments, or reports. What would you like to do?"},
    {"bye", "Goodbye! Have a productive day and thank you for using DnD Trading’s POS system."},
    {"thanks", "You’re welcome! Glad I could help."},
    {"thank you", "Anytime! Let me know if there’s anything else you need."},
    {"good morning", "Good morning! Ready to manage some orders or clients?"},
    {"good afternoon", "Good afternoon! How can I assist with your business tasks?"},
    {"good evening", "Good evening! Do you need help wrapping up today’s sales or generating reports?"},

    // ===== CLIENT MANAGEMENT =====
    {"client", "You can register new clients, view client details, or update their information."},
    {"register client", "To register a client, go to the Client Management section and fill in name, contact info, and address."},
    {"new client", "Add a new client under the Client tab by entering their personal and business details."},
    {"update client", "To update a client, search their name in the Client List and click 'Edit'."},
    {"delete client", "Clients can be removed from the system by selecting them and choosing 'Delete'. Make sure all orders are completed first."},
    {"client list", "You can view all registered clients in the Client List section of the dashboard."},
    {"client search", "Search for clients using name, phone number, or client ID in the search bar."},

    // ===== ORDER MANAGEMENT =====
    {"order", "You can create, edit, or track orders in the Order Processing module."},
    {"new order", "To create a new order, open the Order page, select a client, and add items to the cart."},
    {"place order", "Orders are placed through the POS interface. Ensure the client is registered first."},
    {"edit order", "Orders can be edited before payment confirmation under 'Pending Orders'."},
    {"cancel order", "To cancel an order, open the order details and click 'Cancel'. Refunds will process automatically if payment was recorded."},
    {"track order", "You can check order status in the 'Order History' or 'Active Orders' section."},
    {"order history", "The Order History shows all past transactions, including dates, clients, and payment status."},
    {"pending orders", "Pending orders are those awaiting payment or approval. You can view them in the Pending tab."},
    {"order status", "Order status can be 'Pending', 'Processed', 'Shipped', or 'Completed'."},
    {"invoice", "Invoices are generated automatically when an order is completed and paid for."},
    {"quotation", "Quotations can be generated before confirming an order. Clients can approve them before checkout."},

    // ===== SUPPLIER MANAGEMENT =====
    {"supplier", "You can view, add, or update supplier details in the Supplier Management module."},
    {"add supplier", "To add a supplier, go to the Supplier section and fill in the company name, contact, and product range."},
    {"update supplier", "Supplier information can be edited in the Supplier List by clicking 'Edit'."},
    {"supplier list", "The Supplier List displays all current suppliers and their contact details."},
    {"supplier orders", "You can manage purchase orders sent to suppliers under the Procurement tab."},
    {"best supplier", "The POS system helps identify the best supplier based on price, delivery time, and reliability."},
    {"new supplier order", "To create a new supplier order, open the Procurement page and choose products to restock."},

    // ===== INVENTORY & STOCK =====
    {"inventory", "The Inventory module helps you track stock levels, reorder items, and manage product details."},
    {"stock", "You can view available stock, reorder low-stock items, or update quantities manually."},
    {"low stock", "The Low Stock Alert shows items that need restocking soon."},
    {"update stock", "Stock levels can be adjusted manually after stock takes or supplier deliveries."},
    {"add product", "To add a product, enter the name, category, price, and supplier details in the Inventory tab."},
    {"remove product", "Products can be deleted if discontinued, but historical sales data will remain intact."},
    {"reorder", "Use the Reorder function to quickly restock items from preferred suppliers."},
    {"inventory report", "Generate inventory reports to view current stock values and movement trends."},

    // ===== PAYMENT & FINANCE =====
    {"payment", "The system records payments automatically when sales are completed."},
    {"record payment", "To record a manual payment, open the order and select 'Record Payment'."},
    {"payment history", "You can view all previous payments under the Finance tab."},
    {"refund", "Refunds can be issued for returned items in the Payment Management section."},
    {"receipt", "Receipts are generated instantly for every payment and can be reprinted anytime."},
    {"cash", "Cash payments are supported and logged immediately in the POS."},
    {"card", "Card and EFT payments are supported. Ensure the terminal connection is active."},
    {"credit", "Clients can be assigned credit terms. Track outstanding balances in Accounts Receivable."},
    {"debtors", "Debtors reports show clients who owe payments, with due dates and balances."},

    // ===== REPORTS & ANALYTICS =====
    {"report", "Reports provide insights into sales, inventory, and performance trends."},
    {"sales report", "Generate daily, weekly, or monthly sales reports to track business growth."},
    {"revenue", "Revenue reports show total income per product or per client."},
    {"profit", "Profit reports analyze total revenue minus supplier and operating costs."},
    {"performance", "Performance analytics highlight top-performing products and employees."},
    {"manager report", "Managers can access custom reports for financial summaries and strategic planning."},
    {"summary", "The Summary dashboard displays total sales, profit, and top-selling items."},
    {"export report", "You can export reports to Excel or PDF for record-keeping or presentations."},

    // ===== USER & SYSTEM SUPPORT =====
    {"login", "Log in using your employee ID and password. Contact admin if you’re locked out."},
    {"logout", "To log out, click your profile icon and select 'Sign Out'."},
    {"password", "If you forgot your password, request a reset from the admin panel."},
    {"user role", "There are two main roles: Sales Representative and Manager. Each has specific access rights."},
    {"admin", "Admins can manage users, adjust permissions, and configure system settings."},
    {"settings", "You can customize company info, tax rates, and currency in the Settings menu."},
    {"error", "If you encounter an error, restart the POS or contact system support."},
    {"update", "System updates are automatic and include performance and security improvements."},

    // ===== BUSINESS CONTEXT =====
    {"dnd", "DnD Trading and General Supplies specializes in stationery products for individuals and organizations."},
    {"company", "DnD Trading operates as a stationery intermediary, sourcing quality products from multiple suppliers."},
    {"mission", "Our mission is to improve efficiency, accuracy, and customer satisfaction through innovation."},
    {"goal", "DnD Trading aims to streamline operations and make data-driven business decisions."},
    {"vision", "Our vision is to be a leading stationery supplier known for reliability and efficiency."},
    {"strategy", "Managers use system insights to optimize pricing, supplier selection, and stock control."},
    {"clients", "Our diverse clients include schools, offices, and individual customers."},
    {"suppliers", "We partner with trusted suppliers to ensure consistent product availability and quality."},

    // ===== SMALL TALK =====
    {"how are you", "I’m great, thank you! Ready to help with your sales and management tasks."},
    {"who are you", "I’m the DnD POS Assistant — here to help you manage sales, clients, and operations efficiently."},
    {"what is this", "This is the DnD Trading POS chatbot, designed to assist staff with everyday system tasks."},
    {"what can you do", "I can help you manage clients, process orders, handle payments, generate reports, and much more!"},

            {"discount", "Discounts can be applied during checkout. Make sure to verify eligibility before applying."},
{"promotion", "You can set up time-limited promotions in the Pricing module under 'Marketing Tools'."},
{"special offer", "Current promotions are displayed on the dashboard under 'Active Campaigns'."},
{"pricing", "Product prices can be updated in the Inventory module or bulk-edited in the Admin panel."},
{"price update", "To update product prices, navigate to Inventory → Product List → Edit Price."},
{"bulk discount", "Bulk discounts can be configured for clients purchasing above a certain quantity."},
{"markup", "The system calculates markup automatically based on supplier cost and retail price."},
{"tax", "You can adjust VAT and tax rates in Settings → Financial Configuration."},


{"delivery", "Delivery details can be added when processing an order."},
{"shipping", "The POS tracks shipping methods and estimated delivery times."},
{"delivery status", "Delivery status updates automatically once a shipment is dispatched."},
{"courier", "You can select a preferred courier during checkout for client deliveries."},
{"pickup", "Clients can also choose self-pickup at our store. Mark it during order processing."},
{"delivery note", "Delivery notes are automatically generated when goods are dispatched."},
{"delivery schedule", "You can view upcoming delivery schedules in the Logistics section."},
{"track shipment", "Tracking numbers can be entered and updated under the Order Details view."},


{"technical issue", "If you're having a technical issue, restart the POS or contact IT support."},
{"bug", "Please describe the bug. I’ll log it for review by the development team."},
{"system error", "If a system error appears, note the error code and contact the admin."},
{"restart", "Restarting the POS app can resolve most connection or sync issues."},
{"backup", "System data is automatically backed up daily. Manual backups can be run in Settings."},
{"restore", "You can restore a previous backup through the Admin > Database Management page."},
{"sync", "The POS automatically syncs data with the central database. You can also trigger a manual sync."},
{"connection", "If you lose connection, the POS will switch to offline mode and sync once back online."},


{"employee", "Employees can be added, updated, or deactivated in the User Management module."},
{"add employee", "To add a new employee, enter their name, role, and contact in User Management."},
{"staff list", "View all registered staff and their access roles in the Staff tab."},
{"attendance", "Staff attendance can be tracked automatically when logging into the system."},
{"shift", "You can assign or view shifts in the HR section of the POS system."},
{"performance review", "Managers can generate staff performance summaries under HR Reports."},
{"role", "User roles include Sales Rep, Manager, and Admin. Each has specific permissions."},
{"access rights", "Access rights can be edited by admins in the Role Management section."},


{"audit", "Audit logs track all system activity for accountability and transparency."},
{"activity log", "You can view system activity logs under Admin → System Logs."},
{"modification", "Every record modification is logged with user, date, and time details."},
{"security", "The POS system follows best practices for data security and encryption."},
{"compliance", "Our system ensures compliance with financial and privacy regulations."},
{"transaction history", "All financial transactions are securely logged and cannot be deleted."},
{"data retention", "Data is stored securely for audit and compliance purposes."},


{"privacy", "DnD Trading values client privacy. Data is stored securely and not shared externally."},
{"confidential", "Confidential client or business data is accessible only to authorized staff."},
{"delete data", "Client data can be deleted only with manager approval for compliance reasons."},
{"personal data", "Personal data is protected under our company privacy policy."},
{"user account", "Each user has a secure account with individual login credentials."},
{"two factor", "Two-factor authentication is available for enhanced account security."},
{"reset password", "Password resets can be done via the admin dashboard or by contacting support."},


{"forecast", "Sales forecasting uses historical data to predict future trends."},
{"trend", "You can view sales trends in the Reports > Analytics Dashboard."},
{"top products", "Top-selling products are listed on the Dashboard under Performance Insights."},
{"low performance", "Low-performing items can be identified for discounts or supplier review."},
{"demand", "Demand analysis helps determine which items to restock more frequently."},
{"seasonal trend", "You can analyze seasonal sales variations in the Analytics module."},
{"comparison", "Compare monthly or yearly performance under the Sales Comparison report."},
{"kpi", "Key Performance Indicators (KPIs) are displayed for sales, profit, and customer growth."},


{"customer service", "Customer service issues can be logged in the Support module."},
{"complaint", "To record a complaint, go to Customer Service → Log Issue."},
{"feedback", "Customer feedback helps us improve. You can log feedback in the Feedback tab."},
{"follow up", "Set reminders to follow up with clients after sales or complaints."},
{"satisfaction", "Customer satisfaction reports summarize feedback ratings and issue resolutions."},
{"return", "Clients can request returns within 7 days. Use the Return Management feature."},
{"exchange", "Exchanges can be processed like a new order, referencing the original transaction."},
{"thank customer", "Always thank the customer for their purchase and feedback — it builds loyalty!"},


{"reminder", "You can set reminders for pending orders or follow-ups with clients."},
{"task", "Tasks can be assigned to staff with due dates under the Task Manager."},
{"calendar", "View all important business dates in the integrated calendar view."},
{"meeting", "Meetings with suppliers or clients can be scheduled through the Events module."},
{"deadline", "Deadlines for deliveries and reports are visible in your dashboard overview."},


{"email", "Emails can be sent directly from the POS for invoices and client communication."},
{"sms", "SMS notifications can be configured for order confirmations and payment alerts."},
{"integration", "The POS integrates with accounting tools like QuickBooks and Xero."},
{"export data", "Data can be exported to Excel, CSV, or PDF for offline analysis."},
{"import data", "You can import supplier or product data from a spreadsheet."},
{"api", "An API is available for integration with third-party business tools."},

{"category", "Products are grouped by categories such as stationery, office supplies, and accessories."},
{"new product", "To add a new product, go to Inventory → Add Product and fill in details."},
{"product details", "You can view product details, supplier, and price history in the Product Info section."},
{"barcode", "Each product can have a barcode assigned for faster checkout."},
{"catalog", "The product catalog lists all items available for sale or order."},
{"discontinued", "Discontinued items can be archived for record-keeping but not sold."},


{"motivation", "Every sale counts! Keep up the great work at DnD Trading."},
{"fun fact", "Did you know? Automating sales reports can save up to 10 hours per week!"},
{"tip", "Tip: Regularly update your supplier details to avoid delays in restocking."},
{"update check", "Checking for new updates... all systems are currently up to date."},
{"system info", "The POS is running version 3.5 with integrated analytics and reporting tools."},
{"about", "This POS was developed to streamline operations for DnD Trading and General Supplies."},

{"keywords",
"Here’s a list of keywords I understand and respond to:\n\n" +

"🗨️ General:\n" +
"hello, hi, hey, help, bye, thanks, thank you, good morning, good afternoon, good evening, how are you, who are you, what is this, what can you do\n\n" +

"👥 Client Management:\n" +
"client, register client, new client, update client, delete client, client list, client search\n\n" +

"🛒 Order Management:\n" +
"order, new order, place order, edit order, cancel order, track order, order history, pending orders, order status, invoice, quotation\n\n" +

"🚚 Supplier & Procurement:\n" +
"supplier, add supplier, update supplier, supplier list, supplier orders, best supplier, new supplier order\n\n" +

"📦 Inventory & Stock:\n" +
"inventory, stock, low stock, update stock, add product, remove product, reorder, inventory report, category, new product, product details, barcode, catalog, discontinued\n\n" +

"💰 Payment & Finance:\n" +
"payment, record payment, payment history, refund, receipt, cash, card, credit, debtors\n\n" +

"📈 Reports & Analytics:\n" +
"report, sales report, revenue, profit, performance, manager report, summary, export report, forecast, trend, top products, low performance, demand, seasonal trend, comparison, kpi\n\n" +

"⚙️ System & Technical Support:\n" +
"login, logout, password, user role, admin, settings, error, update, technical issue, bug, system error, restart, backup, restore, sync, connection\n\n" +

"👨‍💼 Staff & HR Management:\n" +
"employee, add employee, staff list, attendance, shift, performance review, role, access rights\n\n" +

"📋 Audit & Security:\n" +
"audit, activity log, modification, security, compliance, transaction history, data retention\n\n" +

"🔒 Data Privacy & Accounts:\n" +
"privacy, confidential, delete data, personal data, user account, two factor, reset password\n\n" +

"💬 Customer Service:\n" +
"customer service, complaint, feedback, follow up, satisfaction, return, exchange, thank customer\n\n" +

"🕒 Scheduling & Tasks:\n" +
"reminder, task, calendar, meeting, deadline\n\n" +

"💸 Promotions & Pricing:\n" +
"discount, promotion, special offer, pricing, price update, bulk discount, markup, tax\n\n" +

"🌐 Communication & Integration:\n" +
"email, sms, integration, export data, import data, api\n\n" +

"🚛 Delivery & Logistics:\n" +
"delivery, shipping, delivery status, courier, pickup, delivery note, delivery schedule, track shipment\n\n" +

"🏢 Company Info & Strategy:\n" +
"dnd, company, mission, goal, vision, strategy, clients, suppliers, about\n\n" +

"💡 Miscellaneous:\n" +
"motivation, fun fact, tip, update check, system info\n\n" +

"— You can type any of these keywords or phrases, and I’ll respond with the relevant business information or assistance!"
},



    };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtChat.Text = "";
                lblYou.Text = "";
                txtChat.Text = "BOT: Hello! How can I assist you today?";
                txtInput.Focus();
            }
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            txtChat.Text = "";
            lblYou.Text = "";
            string userInput = txtInput.Text.Trim();
            bool bRespFound = false;

            if (!string.IsNullOrEmpty(userInput))
            {
                lblYou.Text = "YOU: " + userInput;
                string botReply = "BOT: ";

                foreach (var pair in botResponses)
                {
                    if (userInput.ToLower().Contains(pair.Key.ToLower()))
                    {
                        botReply = pair.Value;
                        txtChat.Text += botReply + "\n" + " ";
                        txtInput.Text = "";
                        bRespFound = true;
                    }
                }

                if (!bRespFound)
                {
                    txtChat.Text += "BOT: I'm sorry, I don't understand. Can you please rephrase? \n";
                    lblYou.Text = "";
                    txtInput.Text = "";
                }

            } else
            {
               txtChat.Text += "BOT: Please enter a message. \n";
                lblYou.Text = "";
                txtInput.Text = "";
            }

            txtInput.Focus();
        }


    }
}