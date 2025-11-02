<%@ Page Title="Supplier Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierProducts.aspx.cs" Inherits="DNDWebsite.SupplierProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Products</h2>

    <div style="text-align:center; margin-bottom:15px;">
        <asp:Panel ID="pnlBackToOrders" runat="server" Visible="false">
            <asp:Button ID="btnBackToOrders" runat="server" CssClass="btn-search" Text="Back to Client Orders" OnClick="btnBackToOrders_Click" />
        </asp:Panel>
    </div>

    <div style="display:flex; justify-content:center; gap:20px; flex-wrap:wrap; margin-top:20px;">
        <!-- GridView Column -->
        <div style="flex:1; min-width:500px;">
            <asp:GridView ID="gvSupplierProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None"
                AllowPaging="true" PageSize="10" PagerSettings-PageButtonCount="20" OnPageIndexChanging="gvSupplierProducts_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                    <asp:BoundField DataField="FinalPrice" HeaderText="Price" DataFormatString="{0:C2}" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Add Product & Filter Column -->
        <div style="flex:1; min-width:300px; background:#F5F5F5; padding:15px; border-radius:8px; border:1px solid #4682B4;">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="instruction-label" />

            <!-- Filter Section -->
            <h3 style="margin-bottom:10px; color:#2F4F4F;">Filter</h3>
            <div style="margin-bottom:15px;">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="input-box" Placeholder="Search Product" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
            </div>
            <div style="margin-bottom:15px;">
                <asp:DropDownList ID="ddlFilterSupplier" runat="server" CssClass="input-box" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterSupplier_SelectedIndexChanged" />
            </div>

            <hr />

            <!-- Add Product Section -->
            <h3 style="margin-bottom:10px; color:#2F4F4F;">Add Product</h3>
            <div style="margin:10px 0;">
                <asp:TextBox ID="txtProductName" runat="server" CssClass="input-box" Placeholder="Product Name"></asp:TextBox>
            </div>
            <div style="margin:10px 0;">
                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="input-box" />
            </div>
            <div style="margin:10px 0;">
                <asp:TextBox ID="txtPrice" runat="server" CssClass="input-box" Placeholder="Price"></asp:TextBox>
            </div>
            <div style="margin:10px 0;">
                <asp:TextBox ID="txtSurcharge" runat="server" CssClass="input-box" Placeholder="Surcharge"></asp:TextBox>
            </div>
            <div style="margin-top:10px;">
                <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn-search" OnClick="btnAddProduct_Click" />
            </div>
            <div style="margin-top:10px;">
                <a runat="server" id="lnkCantFindSupplier" href="Supplier.aspx?fromSupplierProducts=1" class="btn-search" style="display:inline-block; text-decoration:none;">
                    Can't find supplier?
                </a>
            </div>

        </div>
    </div>

    <style>
        .input-box {
            width: 100%;
            padding: 8px;
            border: 1px solid #4682B4;
            border-radius: 6px;
            box-sizing: border-box;
        }

        .btn-search {
            padding: 8px 20px;
            background-color: #4682B4;
            color: #fff;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
        .btn-search:hover { background-color: #5a9bd3; }

        .grid {
            margin: 0 auto;
            width: 100%;
            border-collapse: collapse;
            background-color: #F5F5F5;
        }
        .grid th, .grid td {
            border: 1px solid #4682B4;
            padding: 10px;
            text-align: center;
        }
        .grid th {
            background-color: #4682B4;
            color: #fff;
            font-weight: bold;
        }
        .grid tr:hover {
            background-color: #d0e4ff;
        }
    </style>
</asp:Content>