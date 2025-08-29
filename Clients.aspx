<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="DNDWebsite.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center;">Clients</h2>

    <!-- <p style="text-align:center;">Below is a list of all clients:</p> -->

    <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="False" CssClass="grid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="ClientID" HeaderText="Client ID" />
            <asp:BoundField DataField="ClientName" HeaderText="Client Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" />
        </Columns>
    </asp:GridView>

    <style>
        .grid {
            margin: 20px auto;
            max-width: 900px;
            border-collapse: collapse;
            background-color: #111;
        }
        .grid th, .grid td {
            border: 1px solid #FFD700;
            padding: 10px;
            text-align: center;
            color: #FFD700;
        }
        .grid th {
            background-color: #FFD700;
            color: #000;
        }
        .grid tr:hover {
            background-color: #222;
        }
    </style>
</asp:Content>
 