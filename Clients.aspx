<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="DNDWebsite.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Clients</h2>

    <asp:GridView ID="gvClients" runat="server"
    AutoGenerateColumns="False"
    CssClass="grid"
    GridLines="None"
    AllowPaging="true"
    PageSize="10"
    OnPageIndexChanging="gvClients_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="ClientID" HeaderText="Client ID" />
            <asp:BoundField DataField="ClientName" HeaderText="Client Name" />
            <asp:BoundField DataField="ClientEmail" HeaderText="Email" />
            <asp:BoundField DataField="ClientPhoneNumber" HeaderText="Phone" />
        </Columns>
    </asp:GridView>

    <style>
        body {
            background-color: #fff; /* white page background */
            color: #333;            /* dark text color */
            font-family: Arial, sans-serif;
        }

        h2 {
            margin-top: 20px;
            color: #2F4F4F;
        }

        .grid {
            margin: 20px auto;
            max-width: 900px;
            border-collapse: collapse;
            background-color: #F5F5F5; /* light grid background */
        }

        .grid th, .grid td {
            border: 1px solid #4682B4; /* consistent blue border */
            padding: 10px;
            text-align: center;
        }

        .grid th {
            background-color: #4682B4; /* consistent blue header */
            color: #fff;               /* white text */
            font-weight: bold;
        }

        .grid tr:hover {
            background-color: #d0e4ff; /* light blue hover */
        }
    </style>
</asp:Content>