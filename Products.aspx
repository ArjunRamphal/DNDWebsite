<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="DNDWebsite.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Create a Product Order</h2>

    <div class="page-container">
        <!-- LEFT COLUMN -->
        <div class="left-column">
            <div class="search-box">
                <asp:Label ID="lblInstruction" runat="server" Text="Enter product description" CssClass="instruction-label"></asp:Label><br />

                <!-- Search and Quantity Inline -->
                <div class="inline-inputs">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="input-box" placeholder="e.g., Black pens" />
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="input-box" TextMode="Number" placeholder="Quantity" />
                    <asp:Button ID="btnAddProduct" runat="server" Text="Add to list" CssClass="btn-search" OnClick="btnAddProduct_Click" />
                </div>
            </div>

            <!-- SERVER-SIDE FILTER BOX -->
            <div class="live-filter-box">
                <asp:Label ID="lblFilter" runat="server" Text="Filter by product name:" CssClass="instruction-label"></asp:Label><br />
                <asp:TextBox ID="txtFilter" runat="server" CssClass="input-box"
                    placeholder="Start typing to filter..."
                    AutoPostBack="true"
                    OnTextChanged="txtFilter_TextChanged" />
            </div>

            <!-- Available Products Grid wrapped in UpdatePanel -->
            <asp:UpdatePanel ID="upProducts" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvAvailableProducts" runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="grid"
                        GridLines="None"
                        AllowPaging="True" 
                        PageSize="5"
                        OnPageIndexChanging="gvAvailableProducts_PageIndexChanging"
                        OnRowCommand="gvAvailableProducts_RowCommand"
                        OnRowDataBound="gvAvailableProducts_RowDataBound">

                        <RowStyle CssClass="data-row" />
                        <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />

                        <Columns>
                            <asp:BoundField DataField="ProductID" HeaderText="ID" Visible="False" />
                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />

                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantityRow" runat="server" Text="1" TextMode="Number" Min="1" CssClass="qty-input"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnAdd" runat="server" 
                                        Text="Add" 
                                        CommandName="SelectProduct" 
                                        CommandArgument='<%# Container.DataItemIndex %>' 
                                        CssClass="btn-search" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <!-- RIGHT COLUMN -->
        <div class="right-column">
            <h3 style="text-align:center; color:#2F4F4F;">Your Selected Products</h3>

            <!-- Wrap selected products grid in UpdatePanel -->
            <asp:UpdatePanel ID="upSelectedProducts" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvProducts" runat="server" 
                        AutoGenerateColumns="False" 
                        CssClass="grid" 
                        GridLines="None"
                        AllowPaging="True"
                        PageSize="10"
                        OnRowDeleting="gvProducts_RowDeleting"
                        OnPageIndexChanging="gvProducts_PageIndexChanging">

                        <RowStyle CssClass="data-row" />
                        <PagerStyle CssClass="pager" BackColor="#4682B4" ForeColor="White" HorizontalAlign="Center" />

                        <Columns>
                            <asp:BoundField DataField="ProductName" HeaderText="Product" />
                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="Delete" CssClass="btn-search" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>


            <div style="text-align:center; margin-top:20px;">
                <asp:Button ID="btnCreateOrder" runat="server" Text="Create Order Request" CssClass="btn-search" OnClick="btnCreateOrder_Click" />
            </div>

            <div style="text-align:center; margin-top:10px;">
                <asp:Label ID="lblMessage" runat="server" CssClass="instruction-label" ForeColor="Green"></asp:Label>
            </div>
        </div>

        <!-- JS debounce for server-side filtering -->
        <script>
            let filterTimeout;
            const filterBox = document.getElementById('<%= txtFilter.ClientID %>');
            if (filterBox) {
                filterBox.addEventListener('keyup', function () {
                    clearTimeout(filterTimeout);
                    filterTimeout = setTimeout(() => {
                        __doPostBack('<%= txtFilter.UniqueID %>', '');
                    }, 300);
                });
            }
        </script>

    </div>
<style> /* Layout */ .page-container { display: flex; justify-content: center; align-items: flex-start; gap: 40px; flex-wrap: wrap; margin-top: 30px; } .left-column, .right-column { flex: 1; min-width: 400px; max-width: 700px; } /* Search Box */ .search-box, .live-filter-box { background-color: #F5F5F5; border: 1px solid #4682B4; border-radius: 10px; padding: 20px; text-align: center; margin-bottom: 20px; } .inline-inputs { display: inline-flex; align-items: center; gap: 8px; flex-wrap: wrap; justify-content: center; } .instruction-label { font-size: 16px; font-weight: bold; color: #2F4F4F; } .input-box { width: 40%; padding: 10px; border: 1px solid #4682B4; border-radius: 6px; font-size: 14px; } /* Buttons */ .btn-search { padding: 8px 20px; background-color: #4682B4; color: #fff; border: none; border-radius: 6px; cursor: pointer; } .btn-search:hover { background-color: #5a9bd3; } /* Grids */ .grid { margin: 20px auto; width: 100%; max-width: 700px; border-collapse: collapse; background-color: #F5F5F5; color: #2F4F4F; } .grid th, .grid td { border: 1px solid #4682B4; padding: 10px; text-align: center; } .grid th { background-color: #4682B4; color: #FFFFFF; } .grid tr.data-row:hover { background-color: #D0E4F5; } /* Pager styling */ .pager { background-color: #4682B4; color: #FFFFFF; text-align: center; } .pager a, .pager span { display: inline-block; padding: 5px 10px; margin: 2px; border-radius: 4px; color: #FFFFFF; text-decoration: none; } .pager a:hover { background-color: #5a9bd3; } .pager span { background-color: #315f7d; } .qty-input { width: 60px; padding: 5px; text-align: center; border: 1px solid #4682B4; border-radius: 5px; } </style>
</asp:Content>