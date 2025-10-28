<%@ Page Title="Suppliers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="DNDWebsite.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Suppliers</h2>

    <asp:GridView ID="gvSuppliers" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="SupplierID" HeaderText="Supplier ID" />
            <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
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