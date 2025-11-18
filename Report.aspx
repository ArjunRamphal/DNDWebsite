<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="DNDWebsite.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align: center;">Reports</h2>
    &nbsp;

    <div style="
        display:flex;
        justify-content:space-between;
        align-items:flex-start;
        width: 100%; 
        box-sizing: border-box;
        padding: 0 20px;
        gap: 20px;
        flex-wrap: wrap;">
        
        <div style="flex: 1; min-width: 300px;">
            
            <h4 style="text-align:center; margin-bottom:5px;">Monthly Gross Revenue</h4>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:260px;">
                <canvas id="revChart"></canvas>
            </div>

            <h5 style="text-align:center; margin-top:20px;">Revenue Breakdown</h5>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:220px;">
                <canvas id="RevChartLine"></canvas>
            </div>

        </div>

        <div style="flex: 1; min-width: 300px;">

            <h4 style="text-align:center; margin-bottom:5px;">Monthly Sales Representatives Sales</h4>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:260px;">
                <canvas id="salesRepChart"></canvas>
            </div>

            <h5 style="text-align:center; margin-top:20px;">Monthly Sales Breakdown</h5>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:220px;">
                <canvas id="SalesRepMonthlyChart"></canvas>
            </div>

        </div>
 
        <div style="flex: 1; min-width: 300px;">

            <h4 style="text-align:center; margin-bottom:5px;">Monthly Top 5 Products</h4>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:260px;">
                <canvas id="top5Pie"></canvas>
            </div>

            <h5 style="text-align:center; margin-top:20px;">Monthly Category Breakdown</h5>
            <div style="border: 2px solid #ccc; padding:10px; border-radius:8px; background:white; height:220px;">
                <canvas id="top5BarChart"></canvas>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels@2"></script>

    <script>//script for the monthly rev
        var RevData = JSON.parse('<%= RevChartJson %>');;

        new Chart(document.getElementById("revChart"), {// Monthly Revenue Chart
            type: 'bar',
            data: {
                labels: RevData.labels,
                datasets: [{
                    label: 'Revenue (R)',
                    data: RevData.values
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,

                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return "R " + context.raw.toLocaleString();//rands in columns
                            }
                        }
                    }
                },

                scales: {
                    y: {
                        ticks: {
                            callback: function (value) {
                                return "R " + value.toLocaleString(); //rands for hovering value 
                            }
                        }
                    }
                }
            }
        });

    </script>
    <script>//script for supplier pie chart
        var top5Data = JSON.parse('<%= top5ChartJson %>');

        new Chart(document.getElementById("top5Pie"), {
            type: 'pie',
            data: {
                labels: top5Data.labels,
                datasets: [{
                    data: top5Data.values
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
            }
        });
    </script>
    <script>var salesRepData = <%= SalesRepJson %>;//salesReps Performance chart
        new Chart(document.getElementById("salesRepChart"), {
            type: 'bar',
            data: {
                labels: salesRepData.labels,
                datasets: [{
                    label: 'Sales (R)',
                    data: salesRepData.values,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,

                indexAxis: 'y',  // horizontal bar chart

                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return 'R ' + context.raw.toFixed(2);
                            }
                        }
                    }
                },

                scales: {
                    x: {
                        ticks: {
                            callback: function (value) {
                                return 'R ' + value;
                            }
                        }
                    }
                }
            }
        });
    </script>                        
    <script>var revLineData = <%= GrossRevLineJson %>;//script for gross revenue line chart
        new Chart(document.getElementById("RevChartLine"), {
            type: 'line',
            data: {
                labels: revLineData.labels,
                datasets: [{
                    label: 'Gross Sales (R)',
                    data: revLineData.values,
                    borderWidth: 2,
                    fill: true,
                    tension: 0.3
                }]
            },

            options: {
                responsive: true,
                maintainAspectRatio: false,

                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                return 'R ' + context.raw.toFixed(2);
                            }
                        }
                    }
                },

                scales: {
                    y: {
                        ticks: {
                            callback: function (value) {
                                return 'R ' + value;
                            }
                        }
                    }
                }
            }
        });
    </script>
    
    <script>////second chart for sales rep monthly represented as line chart
        var salesRepData = <%= SalesRepMonthlyJson %>;

        new Chart(document.getElementById("SalesRepMonthlyChart"), {
            type: 'line',
            data: salesRepData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
            plugins: {
                tooltip: {
                callbacks: {
                label: function (ctx) {
                            return "R " + ctx.raw.toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: {
                beginAtZero: true,
            ticks: {
                callback: function (value) {
                            return "R " + value;
                        }
                    }
                }
            }
        }
    });
    </script>

    <script>
        var top5 = JSON.parse('<%= Top5BarChartJson %>');

        new Chart(document.getElementById("top5BarChart"), {
            type: 'doughnut',
            data: {
                labels: top5.labels,
                datasets: [{
                    data: top5.values,
                    backgroundColor: [
                        "rgba(54,162,235,0.5)",
                        "rgba(75,192,192,0.5)",
                        "rgba(255,159,64,0.5)",
                        "rgba(153,102,255,0.5)",
                        "rgba(255,205,86,0.5)"
                    ]
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false
            }
        });
    </script>

    <style>
        h3 {
            margin-top: 40px;
            text-align: center;
        }
    </style>
</asp:Content>