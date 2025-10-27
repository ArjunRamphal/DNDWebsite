<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="DNDWebsite.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">My Orders</h2>

    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False"
        CssClass="grid" GridLines="None" ShowHeader="True"
        OnRowCommand="gvOrders_RowCommand">
        <Columns>
            <asp:ButtonField Text="View" CommandName="GoToCheckout" ButtonType="Link" />
            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField DataField="OrderAmount" HeaderText="Total Amount" DataFormatString="{0:C2}" />
            <asp:BoundField DataField="OrderStatusText" HeaderText="Order Status" />
            <asp:BoundField DataField="PaymentStatusText" HeaderText="Payment Status" />
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" Text="" 
        CssClass="instruction-label" ForeColor="Green" 
        Style="display:block; text-align:center; margin-top:10px;"></asp:Label>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 700px;
            border-collapse: collapse;
            background-color: #F5F5F5;
            color: #2F4F4F;
        }
        .grid th, .grid td {
            border: 1px solid #4682B4;
            padding: 10px;
            text-align: center;
        }
        .grid th {
            background-color: #4682B4;
            color: #FFFFFF;
        }
        .grid tr:hover {
            background-color: #D0E4F5;
        }
    </style>
</asp:Content>
