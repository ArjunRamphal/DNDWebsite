<%@ Page Title="Client Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientOrders.aspx.cs" Inherits="DNDWebsite.ClientOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 id="lblHeader" runat="server" style="text-align:center;">All Client Orders</h2>

    <div id="filtersRow" style="text-align:center; margin-bottom:10px;">
        <div id="clientSearchRow" style="text-align:center; margin-bottom:10px;">
            <asp:TextBox ID="txtClientSearch" runat="server" CssClass="input-box" placeholder="Search clients by name..." />
            <asp:Button ID="btnSearchClient" runat="server" Text="Search" CssClass="btn-search" OnClick="btnSearchClient_Click" />
        </div>

        <asp:CheckBox ID="chkPending" runat="server" Text="Show Pending Orders Only" AutoPostBack="true" OnCheckedChanged="chkPending_CheckedChanged" />
        <asp:Button ID="btnResetFilter" runat="server" Text="Reset Filter" OnClick="btnResetFilter_Click" CssClass="btn-search" />
    </div>

    <!-- Master GridView -->
    <asp:GridView ID="gvClientOrders" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None"
        OnSelectedIndexChanged="gvClientOrders_SelectedIndexChanged"
        OnPageIndexChanging="gvClientOrders_PageIndexChanging"
        DataKeyNames="OrderID" AllowPaging="true" PageSize="10">

        <RowStyle CssClass="data-row" />

        <Columns>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnSelect" runat="server" 
                        Text="Select" 
                        CommandName="Select" 
                        CssClass="btn-search" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
            <asp:BoundField DataField="ClientName" HeaderText="Client" />
            <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="OrderAmount" HeaderText="Total" DataFormatString="{0:C2}" />
            <asp:BoundField DataField="OrderStatus" HeaderText="Status" />
        </Columns>

        <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />

    </asp:GridView>

    <!-- Detail Panel -->
    <asp:Panel ID="pnlOrderProducts" runat="server" Visible="false" Style="margin-top:20px;">
        <div style="display:flex; gap:20px; align-items:flex-start; flex-wrap:wrap; padding: 0 20px;">
            <!-- LEFT COLUMN -->
            <div style="flex: 1 1 480px;">
                <div style="display:flex; justify-content:space-between; align-items:center;">
                    <h3 style=" margin:0;">Client Order Items</h3>
                    <div style="text-align:right;">
                        <strong>Running total: </strong>
                        <asp:Label ID="lblRunningTotal" runat="server" Text="$0.00" Style="color:#000; font-size:1.1rem;"></asp:Label>
                    </div>
                </div>

                <asp:GridView ID="gvClientOrderProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None" EmptyDataText="No client-specified items">
                    <Columns>
                        <asp:BoundField DataField="ClientOrderProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="ClientOrderProductQuantity" HeaderText="Quantity" />
                        <asp:BoundField DataField="ClientOrderProductStatusText" HeaderText="Status" />
                    </Columns>
                </asp:GridView>

                <h3 style="margin-top:18px;">Supplier Order Items</h3>

                <asp:GridView ID="gvOrderProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None"
                    OnRowDeleting="gvOrderProducts_RowDeleting" EmptyDataText="No supplier items added"
                    DataKeyNames="ProductID,SupplierID">
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                        <asp:BoundField DataField="OrderSupplierProductQuantity" HeaderText="Quantity" />
                        <asp:BoundField DataField="OrderSupplierProductPrice" HeaderText="Price" DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="OrderSupplierProductStatusText" HeaderText="Status" />
                        <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" />
                    </Columns>
                </asp:GridView>

                <div style="text-align:center; margin-top:12px;">
                    <asp:Button ID="btnCreateOrder" runat="server" Text="Create Order (Finalize)" CssClass="btn-search" OnClientClick="return showFinalizeModal();" OnClick="btnCreateOrder_Click" />
                    &nbsp;
                </div>

                <!-- Payment Information Grid -->
                <h3 style="margin-top:18px;">Payment Information</h3>
                <asp:GridView ID="gvPayment" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None" EmptyDataText="No payment record found">
                    <Columns>
                        <asp:BoundField DataField="PaymentID" HeaderText="Payment ID" />
                        <asp:BoundField DataField="PaymentTotal" HeaderText="Total" DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="PaymentDue" HeaderText="Due" DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="PaymentSurplus" HeaderText="Surplus" DataFormatString="{0:C2}" />
                        <asp:BoundField DataField="PaymentStatusText" HeaderText="Status" />
                    </Columns>
                </asp:GridView>

                <!-- Payment Update Section -->
                <div style="margin-top:12px; text-align:center;">
                    <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="input-box" placeholder="Enter payment amount" />
                    <asp:Button ID="btnUpdatePayment" runat="server" Text="Apply Payment" CssClass="btn-search" OnClick="btnUpdatePayment_Click" />
                    <br />
                    <asp:Label ID="lblPaymentMessage" runat="server" Text="" ForeColor="Green" />
                </div>

                <div style="text-align:center; margin-top:12px;">
                    <asp:Button ID="btnBack" runat="server" Text="Back to Orders" OnClick="btnBack_Click" CssClass="btn-search" />
                    &nbsp;
                </div>
            </div>

            <!-- RIGHT COLUMN -->
            <div style="flex: 1 1 360px;">
                <h3 style="margin-top:0;">Supplier Products</h3>

                <div style="margin-bottom:8px;">
                    <label style="font-weight:bold;">Filter</label><br />
                    <asp:TextBox ID="txtSupplierSearch" runat="server" CssClass="input-box" placeholder="Search products..." />
                    <asp:DropDownList ID="ddlSuppliers" runat="server" OnSelectedIndexChanged="ddlSuppliers_SelectedIndexChanged" AutoPostBack="true" CssClass="input-box">
                        <asp:ListItem Text="All suppliers" Value="" />
                    </asp:DropDownList>
                    <asp:Button ID="btnFilterSupplierProducts" runat="server" Text="Filter" CssClass="btn-search" OnClick="btnFilterSupplierProducts_Click" />
                </div>

                <div style="margin:6px 0 12px;">
                    <label style="font-weight:bold;">Add Product</label><br />
                    <asp:Label ID="lblSelectedSupplierProduct" runat="server" Text="(no product selected)" Style="display:block; margin-bottom:6px;"></asp:Label>

                    <asp:TextBox ID="txtAddQty" runat="server" CssClass="input-box" TextMode="Number" placeholder="Quantity" Style="width:110px;" />
                    <asp:Button ID="btnAddToOrder" runat="server" CssClass="btn-search" Text="Add to Order" OnClick="btnAddToOrder_Click" /><br />

                    <div style="margin-top:6px;">
                        <asp:Button ID="btnGoToSupplierProducts" runat="server" CssClass="btn-search" 
                            Text="Can't find product?" OnClick="btnGoToSupplierProducts_Click" />
                    </div>

                    <asp:Label ID="lblAddMessage" runat="server" Text="" ForeColor="Green" />
                </div>

                <!-- Supplier Products Grid -->
                <asp:GridView ID="gvSupplierProducts" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None"
                    OnSelectedIndexChanged="gvSupplierProducts_SelectedIndexChanged"
                    OnPageIndexChanging="gvSupplierProducts_PageIndexChanging"
                    DataKeyNames="ProductID,SupplierID,FinalPrice"
                    AllowPaging="true" PageSize="6">

                    <RowStyle CssClass="data-row" />

                    <Columns>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnSelectProduct" runat="server" 
                                    Text="Select" 
                                    CommandName="Select" 
                                    CssClass="btn-search" />
                            </ItemTemplate>
                        </asp:TemplateField>
        
                        <asp:BoundField DataField="ProductName" HeaderText="Product" />
                        <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                        <asp:BoundField DataField="FinalPrice" HeaderText="Price" DataFormatString="{0:C2}" />
                    </Columns>

                    <%-- ADD THIS LINE --%>
                    <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />

                </asp:GridView>
            </div>
        </div>
    </asp:Panel>

    <!-- Finalize modal -->
    <div id="finalizeModal" style="display:none; position:fixed; left:0; top:0; right:0; bottom:0; background:rgba(0,0,0,0.5); z-index:2000;">
        <div style="background:#fff; width:420px; max-width:90%; margin:60px auto; padding:18px; border-radius:8px; text-align:center;">
            <h3 style="">Confirm Finalize Order</h3>
            <p style="">Finalizing this order will lock editing. Proceed?</p>
            <div style="margin-top:12px;">
                <asp:Button ID="btnConfirmFinalize" runat="server" Text="Yes - Finalize" CssClass="btn-search" OnClick="btnConfirmFinalize_Click" />
                &nbsp;
                <button type="button" onclick="hideFinalizeModal();" class="btn-search" style="background:#ccc; border:none; padding:8px 16px; border-radius:6px;">Cancel</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfSelectedOrderId" runat="server" />
    <asp:HiddenField ID="hfSelectedProductId" runat="server" />
    <asp:HiddenField ID="hfSelectedSupplierId" runat="server" />
    <asp:HiddenField ID="hfSelectedProductPrice" runat="server" />

    <style>
        .grid {
            margin: 12px auto 18px auto;
            width: 100%;
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

        .grid tr.data-row:hover { 
            background-color: #D0E4F5; 
        }

        .btn-search {
            padding: 8px 14px;
            background-color: #4682B4;
            color: #fff;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
        .btn-search:hover { 
            background-color: #5a9bd3;
        }

        .input-box { 
            padding:8px; border-radius:6px; border:1px solid #4682B4; margin:4px 0; width:220px; 
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

    <script type="text/javascript">
        function showFinalizeModal() { document.getElementById('finalizeModal').style.display = 'block'; return false; }
        function hideFinalizeModal() { document.getElementById('finalizeModal').style.display = 'none'; }
    </script>

</asp:Content>
