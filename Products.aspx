<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DNDWebsite.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="text-align:center;">Our Products</h2>

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
