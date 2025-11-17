<%@ Page Title="Supplier Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierProducts.aspx.cs" Inherits="DNDWebsite.SupplierProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Products</h2>

    <div style="text-align:center; margin-bottom:15px;">
        <asp:Panel ID="pnlBackToOrders" runat="server" Visible="false">
            <asp:Button ID="btnBackToOrders" runat="server" CssClass="btn-search" Text="Back to Client Orders" OnClick="btnBackToOrders_Click" />
        </asp:Panel>
    </div>

<div style="display:flex; justify-content:center; align-items:flex-start; gap:40px; flex-wrap:wrap; margin-top:30px; padding: 0 30px;">

    <!-- LEFT: GridView -->
    <div style="flex:2; min-width:500px;">
    <asp:GridView ID="gvSupplierProducts" runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            GridLines="None"
            AllowPaging="true"
            PageSize="10"
            DataKeyNames="ProductID,SupplierID"
            AutoGenerateEditButton="false"
            OnRowEditing="gvSupplierProducts_RowEditing"
            OnRowCancelingEdit="gvSupplierProducts_RowCancelingEdit"
            OnRowUpdating="gvSupplierProducts_RowUpdating"
            OnPageIndexChanging="gvSupplierProducts_PageIndexChanging">
            <RowStyle CssClass="data-row" />

            <Columns>
                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server"
                            CommandName="Edit"
                            Text="Edit"
                            CssClass="btn-search" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnUpdate" runat="server"
                            CommandName="Update"
                            Text="Update"
                            CssClass="btn-search" />
                        <asp:Button ID="btnCancel" runat="server"
                            CommandName="Cancel"
                            Text="Cancel"
                            CssClass="btn-search"
                            Style="background-color:#808080;" />
                        </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <%# Eval("ProductName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditProductName" runat="server"
                            Text='<%# Bind("ProductName") %>' CssClass="input-box" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="SupplierName" HeaderText="Supplier" ReadOnly="true" />

                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <%# String.Format("{0:C2}", Eval("SupplierProductPrice")) %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditPrice" runat="server"
                            Text='<%# Bind("SupplierProductPrice") %>' CssClass="input-box" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Surcharge (%)">
                    <ItemTemplate>
                        <%# Eval("ProductSurcharge") %>%
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditSurcharge" runat="server"
                            Text='<%# Bind("ProductSurcharge") %>' CssClass="input-box" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>

            <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />
        </asp:GridView>
    </div>

    <!-- RIGHT: Filter + Add Product Panel -->
    <div style="flex:1; min-width:300px; background:#F5F5F5; padding:15px; border-radius:8px; border:1px solid #4682B4;">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="instruction-label" />

        <h3 style="margin-bottom:10px;">Filter</h3>
        <div style="margin-bottom:15px;">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="input-box"
                Placeholder="Search Product" AutoPostBack="true"
                OnTextChanged="txtSearch_TextChanged" />
        </div>
        <div style="margin-bottom:15px;">
            <asp:DropDownList ID="ddlFilterSupplier" runat="server"
                CssClass="input-box" AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilterSupplier_SelectedIndexChanged" />
        </div>

        <hr />

        <h3 style="margin-bottom:10px;">Add Product</h3>

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
            <asp:Button ID="btnAddProduct" runat="server"
                Text="Add Product" CssClass="btn-search"
                OnClick="btnAddProduct_Click" />
        </div>

        <div style="margin-top:10px;">
            <a runat="server" id="lnkCantFindSupplier"
                href="Supplier.aspx?fromSupplierProducts=1"
                class="btn-search"
                style="display:inline-block; text-decoration:none;">
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

        .grid tr.data-row:hover {
            background-color: #D0E4F5;
        }

        .pager {
            background-color: #4682B4;
            color: #FFFFFF;
            text-align: center;
        }
        .pager a, .pager span {
            display: inline-block;
            padding: 5px 10px;
            margin: 2px;
            border-radius: 4px;
            color: #FFFFFF;
            text-decoration: none;
        }
        .pager a:hover {
            background-color: #5a9bd3;
        }
        .pager span {
            background-color: #315f7d;
        }
    </style>
</asp:Content>