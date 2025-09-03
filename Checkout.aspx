<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="DNDWebsite.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Checkout</h2>

    <p style="text-align:center; color:#2F4F4F;">Below are the products you’ve added to your order:</p>

    <asp:GridView ID="gvCheckout" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C2}" />
            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 900px;
            border-collapse: collapse;
            background-color: #f5f5f5; /* light gray background */
            color: #2F4F4F; /* dark slate text */
        }

        .grid th, .grid td {
            border: 1px solid #4682B4; /* steel gray border */
            padding: 10px;
            text-align: center;
        }

        .grid th {
            background-color: #4682B4; /* steel gray header */
            color: #ffffff; /* white text for headers */
        }

        .grid tr:hover {
            background-color: #d0e4f5; /* subtle hover effect */
        }
    </style>
</asp:Content>