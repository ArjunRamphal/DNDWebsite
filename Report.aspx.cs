using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Report : Page
    {
        private readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Managers only
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Manager")
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                //LoadReports();
            }
        }
        /*
        private void LoadReports()
        {
            LoadClientSummaryReport();
            LoadMonthlyRevenueReport();
            //LoadSupplierPerformanceReport();
        }

        private void LoadClientSummaryReport()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        c.ClientName, 
                        COUNT(o.OrderID) AS TotalOrders,
                        SUM(o.OrderAmount) AS TotalSpent
                    FROM [Order] o
                    INNER JOIN Client c ON o.ClientID = c.ClientID
                    GROUP BY c.ClientName
                    ORDER BY TotalSpent DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/ClientSummaryReport.rpt"));
            rpt.SetDataSource(dt);
            crvClientSummary.ReportSource = rpt;
            crvClientSummary.DataBind();
        }
        
        private void LoadMonthlyRevenueReport()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        YEAR(OrderDate) AS OrderDate,
                        SUM(OrderAmount) AS TotalRevenue,
                        COUNT(OrderID) AS TotalOrders
                    FROM [Order]
                    GROUP BY YEAR(OrderDate), MONTH(OrderDate)
                    ORDER BY YEAR(OrderDate), MONTH(OrderDate);
                    ";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/MonthlyRevenueReport.rpt"));
            rpt.SetDataSource(dt);
            crvMonthlyRevenue.ReportSource = rpt;
            crvMonthlyRevenue.DataBind();
        }
        
        private void LoadSupplierPerformanceReport()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        s.SupplierName,
                        COUNT(osp.ProductID) AS TotalProductsSold,
                        SUM(osp.OrderSupplierProductQuantity * osp.OrderSupplierProductPrice) AS TotalRevenue
                    FROM OrderSupplierProduct osp
                    INNER JOIN Supplier s ON osp.SupplierID = s.SupplierID
                    GROUP BY s.SupplierName
                    ORDER BY TotalRevenue DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDocument rpt = new ReportDocument();
            rpt.Load(Server.MapPath("~/SupplierPerformanceReport.rpt"));
            rpt.SetDataSource(dt);
            crvSupplierPerformance.ReportSource = rpt;
            crvSupplierPerformance.DataBind();
        }*/
    }
}
