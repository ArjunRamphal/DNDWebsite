<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="DNDWebsite.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">My Orders</h2>

    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" 
        CssClass="grid" GridLines="None" ShowHeader="True">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 600px;
            border-collapse: collapse;
            background-color: #F5F5F5; /* light gray background */
            color: #2F4F4F; /* dark slate text */
        }
        .grid th, .grid td {
            border: 1px solid #4682B4; /* steel gray border */
            padding: 10px;
            text-align: center;
        }
        .grid th {
            background-color: #4682B4; /* steel gray header */
            color: #FFFFFF; /* white text */
        }
        .grid tr:hover {
            background-color: #D0E4F5; /* subtle hover effect */
        }
    </style>
</asp:Content>
