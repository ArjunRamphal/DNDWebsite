using System;
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

        public string ChartJson { get; set; }
        public string SupplierChartJson { get; set; }


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
                LoadSupplierChart();
                Response.Write("<script>console.log('ChartJson: " + ChartJson + "');</script>");


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
                ChartJson = js.Serialize(chartData);
            }
            catch (SqlException ex)
            {
                ChartJson = "{}";
                System.Diagnostics.Debug.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                ChartJson = "{}";
                System.Diagnostics.Debug.WriteLine("General Error: " + ex.Message);
            }
        }
        private void LoadSupplierChart()
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
ORDER BY TotalQty DESC;";

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
            SupplierChartJson = new JavaScriptSerializer().Serialize(chartData);
        }

    }
}