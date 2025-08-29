<%@ Page Title="Suppliers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="DNDWebsite.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Suppliers</h2>

    <asp:GridView ID="gvSuppliers" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="SupplierID" HeaderText="Supplier ID" />
            <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" />
            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
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
