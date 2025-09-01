<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="DNDWebsite.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">My Orders</h2>

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
            background-color: #111;
        }
        .grid th, .grid td {
            border: 1px solid #FFD700;
            padding: 10px;
            text-align: center;
            color: #FFD700;
        }
        .grid th {
            background-color: #FFD700;
            color: #000;
        }
        .grid tr:hover {
            background-color: #222;
        }
    </style>
</asp:Content>
