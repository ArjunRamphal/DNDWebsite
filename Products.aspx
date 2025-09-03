<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DNDWebsite.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="search-box">
        <asp:Label ID="lblInstruction" runat="server" Text="Enter general product description:" CssClass="instruction-label"></asp:Label><br />
        <asp:TextBox ID="txtSearch" runat="server" CssClass="input-box" placeholder="e.g., 4 black pens, 5 erasers, etc." />
        <asp:Button ID="btnSearch" runat="server" Text="Add to order" CssClass="btn-search" />
    </div>

    <style>
        .search-box {
            margin: 40px auto;
            max-width: 500px;
            padding: 20px;
            background-color: #F5F5F5; /* light gray */
            border: 1px solid #4682B4; /* steel gray border */
            border-radius: 10px;
            text-align: center;
        }
        .instruction-label {
            font-size: 16px;
            font-weight: bold;
            color: #2F4F4F; /* dark slate */
        }
        .input-box {
            width: 80%;
            padding: 10px;
            margin: 10px 0;
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
        }
        .btn-search:hover {
            background-color: #5a9bd3;
        }
    </style>

</asp:Content>
