<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="DNDWebsite.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Clients</h2>

    <!-- Search Form -->
    <div style="text-align:center; margin-bottom:20px;">
        <asp:TextBox ID="txtSearch" runat="server" Placeholder="Search by name or email" CssClass="input-box" />
        <asp:DropDownList ID="ddlOptOut" runat="server" CssClass="input-box" Style="width:150px;">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Opted Out" Value="1"></asp:ListItem>
            <asp:ListItem Text="Active" Value="0"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-search" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-search" OnClick="btnClear_Click" />
    </div>

    <!-- Clients Grid -->
    <asp:GridView ID="gvClients" runat="server"
        AutoGenerateColumns="False"
        CssClass="grid"
        GridLines="None"
        AllowPaging="true"
        PageSize="10"
        OnPageIndexChanging="gvClients_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="ClientID" HeaderText="Client ID" ReadOnly="True" />
            <asp:BoundField DataField="ClientName" HeaderText="Client Name" />
            <asp:BoundField DataField="ClientEmail" HeaderText="Email" />
            <asp:BoundField DataField="ClientPhoneNumber" HeaderText="Phone" />
            <asp:TemplateField HeaderText="Opt-Out Status">
                <ItemTemplate>
                    <%# Convert.ToBoolean(Eval("ClientOptOut")) ? "Opted Out" : "Active" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
    </asp:GridView>

    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="status-msg" />

    <style>
        .input-box {
            padding: 8px;
            border: 1px solid #4682B4;
            border-radius: 6px;
            margin-right: 6px;
            box-sizing: border-box;
        }

        .btn-search {
            padding: 8px 18px;
            background-color: #4682B4;
            color: #fff;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
            margin-right: 5px;
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

        .status-msg {
            display: block;
            margin-top: 10px;
            font-weight: bold;
            color: #d9534f;
            text-align: center;
        }
    </style>
</asp:Content>
