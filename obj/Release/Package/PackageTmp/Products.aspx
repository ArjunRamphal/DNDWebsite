<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DNDWebsite.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="search-box">
        <asp:Label ID="lblInstruction" runat="server" Text="Enter product description:" CssClass="instruction-label"></asp:Label><br />
        <asp:TextBox ID="txtSearch" runat="server" CssClass="input-box" placeholder="e.g., Black pens" />
        <asp:TextBox ID="txtQuantity" runat="server" CssClass="input-box" TextMode="Number" placeholder="Quantity" />
        <asp:Button ID="btnAddProduct" runat="server" Text="Add to list" CssClass="btn-search" OnClick="btnAddProduct_Click" />
    </div>

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="grid-view" OnRowDeleting="gvProducts_RowDeleting">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" />
        </Columns>
    </asp:GridView>

    <div style="text-align:center; margin-top:20px;">
        <asp:Button ID="btnCreateOrder" runat="server" Text="Create Order Request" CssClass="btn-search" OnClick="btnCreateOrder_Click" />
    </div>

    <div style="text-align:center; margin-top:10px;">
        <asp:Label ID="lblMessage" runat="server" CssClass="instruction-label" ForeColor="Green"></asp:Label>
    </div>

    <style>
        .search-box {
            margin: 20px auto;
            max-width: 500px;
            padding: 20px;
            background-color: #F5F5F5;
            border: 1px solid #4682B4;
            border-radius: 10px;
            text-align: center;
        }
        .instruction-label {
            font-size: 16px;
            font-weight: bold;
            color: #2F4F4F;
        }
        .input-box {
            width: 40%;
            padding: 10px;
            margin: 10px 5px;
            border: 1px solid #4682B4;
            border-radius: 6px;
            font-size: 14px;
        }
        .btn-search {
            padding: 8px 20px;
            background-color: #4682B4;
            color: #fff;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            margin-top: 10px;
        }
        .btn-search:hover { background-color: #5a9bd3; }
        .grid-view {
            margin: 20px auto;
            max-width: 500px;
            border: 1px solid #4682B4;
            border-radius: 6px;
        }
        .grid-view th, .grid-view td {
            padding: 8px;
            text-align: left;
        }
    </style>

</asp:Content>
