<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="DNDWebsite.Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="color:#2F4F4F; text-align: center;">Reports</h2>
&nbsp;<div style="width:432px; height:235px; text-align: center; float: left; clear: right;">
        <asp:Label ID="lblSupplierRev" runat="server" Text="Monthly Supplier Revenue"></asp:Label>

        <canvas id="revChart" width="400" height="300"></canvas>
    </div>

        <div style="width:442px; height:228px; margin:auto 4px auto 0px; text-align: center; float: right; clear: none; left: -80px;">
       
        <canvas id="supplierPie"></canvas>
    </div>

    
    <div style="height: 299px; width: 1086px;">
    </div>

    
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<script>//script for the monthly rev

    var RevData = JSON.parse('<%= ChartJson %>');;

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
    <script>
        var supplierData = JSON.parse('<%= SupplierChartJson %>');

        new Chart(document.getElementById("supplierPie"), {
            type: 'pie',
            data: {
                labels: supplierData.labels,
                datasets: [{
                    data: supplierData.values
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
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