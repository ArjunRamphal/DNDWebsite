<%@ Page Title="Supplier Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierProducts.aspx.cs" Inherits="DNDWebsite.SupplierProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Products</h2>

    <asp:GridView ID="gvSupplierProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
            <asp:BoundField DataField="FinalPrice" HeaderText="Price" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <style>
        body {
            background-color: #fff; 
            color: #333; 
            font-family: Arial, sans-serif; 
        }

        h2 {
            margin-top: 20px;
            color: #2F4F4F;
        }

        .grid {
            margin: 20px auto;
            max-width: 900px;
            border-collapse: collapse;
            background-color: #F5F5F5; /* light grid background */
        }

        .grid th, .grid td {
            border: 1px solid #4682B4; /* consistent blue border */
            padding: 10px;
            text-align: center;
        }

        .grid th {
            background-color: #4682B4; /* consistent blue header */
            color: #fff;               /* white text */
            font-weight: bold;
        }

        .grid tr:hover {
            background-color: #d0e4ff; /* light blue hover */
        }
    </style>
</asp:Content>