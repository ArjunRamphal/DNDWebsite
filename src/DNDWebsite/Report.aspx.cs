using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace DNDWebsite
{
    public partial class Report : Page
    {
        private readonly string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["DNDConnectionString"].ConnectionString;

        public string RevChartJson { get; set; }
        public string top5ChartJson { get; set; }
        public string SalesRepJson { get; set; }
        public string GrossRevLineJson { get; set; }
        public string SalesRepMonthlyJson { get; set; }
        public string Top5BarChartJson { get; set; }




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
                LoadRevenueChart();
                LoadTop5Chart();
                LoadSalesRepPerformanceChart();
                LoadGrossRevLineChart();
                LoadSalesRepMonthlyChart();
                LoadTop5BarChart();

                Response.Write("<script>console.log('RevChartJson: " + RevChartJson + "');</script>");


            }
        }

        private void LoadRevenueChart()
        {
            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            string query = @"
                SELECT DATENAME(month, OrderDate) + ' ' + CAST(YEAR(OrderDate) AS VARCHAR) AS [MonthYear],
                       SUM(OrderAmount) AS Revenue
                FROM [Order]
                GROUP BY YEAR(OrderDate), MONTH(OrderDate), DATENAME(month, OrderDate)
                ORDER BY YEAR(OrderDate), MONTH(OrderDate);";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string monthYear = rdr["MonthYear"] != DBNull.Value ? rdr["MonthYear"].ToString() : "";
                            decimal revenue = rdr["Revenue"] != DBNull.Value ? Convert.ToDecimal(rdr["Revenue"]) : 0;

                            labels.Add(monthYear);
                            values.Add(revenue);
                        }
                    }
                }

                var chartData = new
                {
                    labels,
                    values
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                RevChartJson = js.Serialize(chartData);
            }
            catch (SqlException ex)
            {
                RevChartJson = "{}";
                System.Diagnostics.Debug.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                RevChartJson = "{}";
                System.Diagnostics.Debug.WriteLine("General Error: " + ex.Message);
            }
        }
        private void LoadTop5Chart()
        {
            List<string> productNames = new List<string>();
            List<int> quantities = new List<int>();

            string query = @"
                SELECT TOP 5 P.ProductName, SUM(OSP.OrderSupplierProductQuantity) AS TotalQty
                FROM OrderSupplierProduct OSP
                JOIN Product P ON OSP.ProductID = P.ProductID
                JOIN [Order] O ON OSP.OrderID = O.OrderID
                WHERE MONTH(O.OrderDate) = MONTH(GETDATE())
                    AND YEAR(O.OrderDate) = YEAR(GETDATE())
                GROUP BY P.ProductName
                ORDER BY TotalQty DESC;
            ";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                int rank = 1;
                while (rdr.Read())
                {
                    string name = rdr["ProductName"].ToString();
                    int qty = Convert.ToInt32(rdr["TotalQty"]);

                    productNames.Add($"{rank}. {name}");
                    quantities.Add(qty);

                    rank++;
                }
            }

            var chartData = new { labels = productNames, values = quantities };
            top5ChartJson = new JavaScriptSerializer().Serialize(chartData);
        }


        private void LoadSalesRepPerformanceChart()
        {
            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            string query = @"
                SELECT U.UserFirstName + ' ' + U.UserLastName AS RepName,
                    SUM(O.OrderAmount) AS TotalSales
                FROM [Order] O
                JOIN [User] U ON O.UserName = U.UserName
                WHERE MONTH(O.OrderDate) = MONTH(GETDATE())
                    AND YEAR(O.OrderDate) = YEAR(GETDATE())
                    AND U.UserType = 0 -- 0 = Sales Representative, based on your User table
                GROUP BY U.UserFirstName, U.UserLastName
                ORDER BY TotalSales DESC;
            ";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    labels.Add(rdr["RepName"].ToString());
                    values.Add(Convert.ToDecimal(rdr["TotalSales"]));
                }
            }

            var chartData = new { labels, values };
            SalesRepJson = new JavaScriptSerializer().Serialize(chartData);
        }


        private void LoadGrossRevLineChart()
        {
            List<string> labels = new List<string>();
            List<decimal> values = new List<decimal>();

            string query = @"
                SELECT DATENAME(month, OrderDate) + ' ' + CAST(YEAR(OrderDate) AS VARCHAR) AS [MonthYear],
                       SUM(OrderAmount) AS Revenue
                FROM [Order]
                GROUP BY YEAR(OrderDate), MONTH(OrderDate), DATENAME(month, OrderDate)
                ORDER BY YEAR(OrderDate), MONTH(OrderDate);
            ";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    labels.Add(rdr["MonthYear"].ToString());
                    values.Add(Convert.ToDecimal(rdr["Revenue"]));
                }
            }

            var chartData = new { labels, values };
            GrossRevLineJson = new JavaScriptSerializer().Serialize(chartData);
        }
        private void LoadSalesRepMonthlyChart()
        {
            string query = @"
                SELECT 
                    U.UserFirstName + ' ' + U.UserLastName AS RepName,
                    DATENAME(month, O.OrderDate) + ' ' + CAST(YEAR(O.OrderDate) AS VARCHAR) AS MonthYear,
                    SUM(O.OrderAmount) AS TotalSales
                FROM [Order] O
                JOIN [User] U ON O.UserName = U.UserName
                WHERE U.UserType = 0   -- Sales Rep
                GROUP BY 
                    U.UserFirstName, U.UserLastName,
                    YEAR(O.OrderDate), MONTH(O.OrderDate), 
                    DATENAME(month, O.OrderDate)
                ORDER BY
                    YEAR(O.OrderDate), MONTH(O.OrderDate);
            ";

            Dictionary<string, Dictionary<string, decimal>> repData =
                new Dictionary<string, Dictionary<string, decimal>>();

            HashSet<string> monthLabels = new HashSet<string>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string rep = rdr["RepName"].ToString();
                            string month = rdr["MonthYear"].ToString();
                            decimal sales = Convert.ToDecimal(rdr["TotalSales"]);

                            monthLabels.Add(month);

                            if (!repData.ContainsKey(rep))
                                repData[rep] = new Dictionary<string, decimal>();

                            repData[rep][month] = sales;
                        }
                    }
                }

                List<object> datasets = new List<object>();
                List<string> monthList = new List<string>(monthLabels);

                foreach (var rep in repData)
                {
                    List<decimal> values = new List<decimal>();

                    foreach (var m in monthList)
                    {
                        values.Add(rep.Value.ContainsKey(m) ? rep.Value[m] : 0);
                    }

                    datasets.Add(new
                    {
                        label = rep.Key,
                        data = values,
                        fill = false,
                        tension = 0.2
                    });
                }

                var chartData = new
                {
                    labels = monthList,
                    datasets = datasets
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                SalesRepMonthlyJson = js.Serialize(chartData);
            }
            catch (Exception ex)
            {
                SalesRepMonthlyJson = "{}";
            }
        }
        private void LoadTop5BarChart()
        {
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            string query = @"
                SELECT TOP 5 
                    P.ProductName, 
                    SUM(OSP.OrderSupplierProductQuantity) AS TotalQty
                FROM OrderSupplierProduct OSP
                JOIN Product P ON OSP.ProductID = P.ProductID
                JOIN [Order] O ON OSP.OrderID = O.OrderID
                WHERE MONTH(O.OrderDate) = MONTH(GETDATE())
                  AND YEAR(O.OrderDate) = YEAR(GETDATE())
                GROUP BY P.ProductName
                ORDER BY TotalQty DESC;
            ";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        labels.Add(rdr["ProductName"].ToString());
                        values.Add(Convert.ToInt32(rdr["TotalQty"]));
                    }
                }
            }

            var chartData = new
            {
                labels = labels,
                values = values
            };

            JavaScriptSerializer js = new JavaScriptSerializer();
            Top5BarChartJson = js.Serialize(chartData);
        }
    }
}