<%@ Page Title="Suppliers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="DNDWebsite.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Suppliers</h2>

    <asp:Panel ID="pnlBackToProducts" runat="server" Visible="false" Style="text-align:center; margin-bottom:15px;">
        <asp:Button ID="btnBackToProducts" runat="server" CssClass="btn-search" Text="Back to Supplier Products" OnClick="btnBackToProducts_Click" />
    </asp:Panel>


    <div style="display:flex; gap:20px; justify-content:center; align-items:flex-start; flex-wrap:wrap; margin-top:20px;">
        <!-- Grid area -->
        <div style="flex:1; min-width:480px;">
            <asp:SqlDataSource ID="sdsSuppliers" runat="server"
                ConnectionString="<%$ ConnectionStrings:DNDConnectionString %>"
                SelectCommand="SELECT SupplierID, SupplierName, SupplierPhoneNumber, SupplierEmail, SupplierOptOut FROM Supplier ORDER BY SupplierID"
                UpdateCommand="UPDATE Supplier SET SupplierName=@SupplierName, SupplierPhoneNumber=@SupplierPhoneNumber, SupplierEmail=@SupplierEmail, SupplierOptOut=@SupplierOptOut WHERE SupplierID=@SupplierID">
                <UpdateParameters>
                    <asp:Parameter Name="SupplierName" Type="String" />
                    <asp:Parameter Name="SupplierPhoneNumber" Type="String" />
                    <asp:Parameter Name="SupplierEmail" Type="String" />
                    <asp:Parameter Name="SupplierOptOut" Type="Boolean" />
                    <asp:Parameter Name="SupplierID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>

            <asp:GridView ID="gvSuppliers" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None"
                DataSourceID="sdsSuppliers" AllowPaging="true" PageSize="10"
                OnPageIndexChanging="gvSuppliers_PageIndexChanging" AutoGenerateEditButton="true">
                <Columns>
                    <asp:BoundField DataField="SupplierID" HeaderText="Supplier ID" ReadOnly="True" />
                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" />
                    <asp:BoundField DataField="SupplierPhoneNumber" HeaderText="Phone" />
                    <asp:BoundField DataField="SupplierEmail" HeaderText="Email" />
                    <asp:CheckBoxField DataField="SupplierOptOut" HeaderText="Opt Out" />
                </Columns>

                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
            </asp:GridView>
        </div>

        <!-- Add Supplier form -->
        <div style="flex:0 0 340px; min-width:280px; background:#F5F5F5; padding:16px; border-radius:8px; border:1px solid #4682B4;">
            <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-msg" />
            <h3 style="margin-top:0; color:#2F4F4F;">Add Supplier</h3>

            <div style="margin-bottom:8px;">
                <asp:Label runat="server" AssociatedControlID="txtNewName" Text="Supplier Name" CssClass="lbl" /><br />
                <asp:TextBox ID="txtNewName" runat="server" CssClass="input-box" />
            </div>

            <div style="margin-bottom:8px;">
                <asp:Label runat="server" AssociatedControlID="txtNewPhone" Text="Phone" CssClass="lbl" /><br />
                <asp:TextBox ID="txtNewPhone" runat="server" CssClass="input-box" />
            </div>

            <div style="margin-bottom:8px;">
                <asp:Label runat="server" AssociatedControlID="txtNewEmail" Text="Email" CssClass="lbl" /><br />
                <asp:TextBox ID="txtNewEmail" runat="server" CssClass="input-box" />
            </div>

            <div>
                <asp:Button ID="btnAddSupplier" runat="server" Text="Add Supplier" CssClass="btn-search" OnClick="btnAddSupplier_Click" />
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

        .status-msg {
            display:block;
            margin-bottom:10px;
            font-weight:bold;
            color:#d9534f;
        }
    </style>
</asp:Content>
