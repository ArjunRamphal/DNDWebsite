<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="DNDWebsite.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Reports</h2>

    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="False" 
        CssClass="grid" GridLines="None" ShowHeader="True">
        <Columns>
            <asp:BoundField DataField="Month" HeaderText="Month" />
            <asp:BoundField DataField="TotalOrders" HeaderText="Total Orders" />
            <asp:BoundField DataField="TotalRevenue" HeaderText="Total Revenue" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 700px;
            border-collapse: collapse;
            background-color: #F5F5F5; /* light gray background */
            color: #2F4F4F; /* dark slate text */
        }
        .grid th, .grid td {
            border: 1px solid #4682B4; /* blue border */
            padding: 10px;
            text-align: center;
        }
        .grid th {
            background-color: #4682B4; /* blue header */
            color: #FFFFFF; /* white text */
        }
        .grid tr:hover {
            background-color: #D0E4F5; /* subtle blue hover */
        }
    </style>
</asp:Content>
