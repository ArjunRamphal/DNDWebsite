<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="DNDWebsite.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Checkout</h2>

    <p style="text-align:center;">Below are the products you’ve added to your order:</p>

    <asp:Label ID="lblMessage" runat="server" CssClass="instruction-label" 
        ForeColor="Green" 
        Style="display:block; text-align:center; margin-bottom:10px;"></asp:Label>

    <asp:GridView ID="gvCheckout" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="ProductName" HeaderText="Product" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C2}" />
        </Columns>
    </asp:GridView>

    <h3 style="text-align:center; margin-top:30px;">Payment Details</h3>

    <asp:GridView ID="gvPayment" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="PaymentTotal" HeaderText="Total" DataFormatString="{0:C2}" />
            <asp:BoundField DataField="PaymentDue" HeaderText="Due" DataFormatString="{0:C2}" />
            <asp:BoundField DataField="PaymentSurplus" HeaderText="Surplus" DataFormatString="{0:C2}" />
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("PaymentStatus")) ? "Paid" : "Unpaid" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <!-- Add this label BELOW the payment grid -->
    <div style="text-align:center; margin-top:15px;">
        <asp:Label ID="lblPaymentStatusMessage" runat="server" 
            CssClass="instruction-label"
            ForeColor="Red"
            Style="font-weight:bold; display:block;">
        </asp:Label>
    </div>


    <div style="text-align:center; margin-top:10px;">
        <asp:Button ID="btnBackToOrders" runat="server" Text="Back to My Orders" CssClass="btn" OnClick="btnBackToOrders_Click" />
    </div>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 900px;
            border-collapse: collapse;
            background-color: #f5f5f5;
            color: #2F4F4F;
        }

        .grid th, .grid td {
            border: 1px solid #4682B4;
            padding: 10px;
            text-align: center;
        }

        .grid th {
            background-color: #4682B4;
            color: #ffffff;
        }

        .grid tr:hover {
            background-color: #d0e4f5;
        }

        .btn { 
            background:#4682B4; 
            color:#fff; 
            font-weight:bold; 
            padding:10px 18px; 
            border:none; 
            border-radius:6px; 
            cursor:pointer; 
            margin-top:8px;
        }

        .btn:hover { 
            background:#5a9bd4; 
        }
    </style>
</asp:Content>
