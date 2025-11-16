<%@ Page Title="Suppliers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Supplier.aspx.cs" Inherits="DNDWebsite.Supplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Suppliers</h2>

    <asp:Panel ID="pnlBackToProducts" runat="server" Visible="false" Style="text-align:center; margin-bottom:15px;">
        <asp:Button ID="btnBackToProducts" runat="server" CssClass="btn-search" Text="Back to Supplier Products" OnClick="btnBackToProducts_Click" />
    </asp:Panel>


    <div style="display:flex; gap:20px; justify-content:center; align-items:flex-start; flex-wrap:wrap; margin-top:20px;">
        <!-- Grid area -->
        <div style="flex:1; min-width:480px;">
            <asp:SqlDataSource ID="sdsSuppliers" runat="server"
                ConnectionString="<%$ ConnectionStrings:DNDConnectionString %>"
                SelectCommand="SELECT SupplierID, SupplierName, SupplierPhoneNumber, SupplierEmail, SupplierOptOut FROM Supplier ORDER BY SupplierID"
                UpdateCommand="UPDATE Supplier SET
                                 SupplierName=@SupplierName,
                                 SupplierPhoneNumber=@SupplierPhoneNumber,
                                 SupplierEmail=@SupplierEmail,
                                 SupplierOptOut=@SupplierOptOut
                               WHERE SupplierID=@original_SupplierID"
                ConflictDetection="CompareAllValues"
                OldValuesParameterFormatString="original_{0}"
                OnUpdated="sdsSuppliers_Updated"
                OnUpdating="sdsSuppliers_Updating">

                <UpdateParameters>
                    <asp:Parameter Name="SupplierName" Type="String" />
                    <asp:Parameter Name="SupplierPhoneNumber" Type="String" />
                    <asp:Parameter Name="SupplierEmail" Type="String" />
                    <asp:Parameter Name="SupplierOptOut" Type="Boolean" />
                    <asp:Parameter Name="SupplierID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>

            <asp:GridView ID="gvSuppliers" runat="server"
                AutoGenerateColumns="False"
                CssClass="grid" GridLines="None"
                DataSourceID="sdsSuppliers"
                AllowPaging="true" PageSize="10"
                AutoGenerateEditButton="false"
                DataKeyNames="SupplierID"
                OnPageIndexChanging="gvSuppliers_PageIndexChanging">

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
                    <asp:BoundField DataField="SupplierID" HeaderText="Supplier ID" ReadOnly="True" />

                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" />

                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <%# Eval("SupplierPhoneNumber") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditPhone" runat="server" 
                                Text='<%# Bind("SupplierPhoneNumber") %>' 
                                MaxLength="10" CssClass="input-box" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <%# Eval("SupplierEmail") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditEmail" runat="server" 
                                Text='<%# Bind("SupplierEmail") %>' 
                                MaxLength="50" CssClass="input-box" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Opt Out">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkOptOut" runat="server" Checked='<%# Eval("SupplierOptOut") %>' Enabled="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkOptOutEdit" runat="server" Checked='<%# Bind("SupplierOptOut") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <RowStyle CssClass="data-row" />
                <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
        </div>

        <!-- Add Supplier form -->
        <div style="flex:0 0 340px; min-width:280px; background:#F5F5F5; padding:16px; border-radius:8px; border:1px solid #4682B4;">
            <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-msg" />
            <h3 style="margin-top:0;">Add Supplier</h3>

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

        .btn-search:hover { 
            background-color: #5a9bd3; 
        }

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
