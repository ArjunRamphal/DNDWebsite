<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DNDWebsite.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="text-align:center; color:#2F4F4F;">Our Products</h2>

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
            <asp:BoundField DataField="FinalPrice" HeaderText="Price" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 900px;
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
