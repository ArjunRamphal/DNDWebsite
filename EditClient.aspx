<%@ Page Title="Edit Client Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditClient.aspx.cs" Inherits="DNDWebsite.EditClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#333;">Edit Your Information</h2>

    <div class="edit-form">
        <asp:Label ID="lblMessage" runat="server" CssClass="status-msg" />

        <asp:TextBox ID="txtName" runat="server" CssClass="input-box" placeholder="Full Name" /><br />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" placeholder="Email" TextMode="Email" /><br />
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input-box" placeholder="Phone Number" /><br />

        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn-save" OnClick="btnSave_Click" /><br />
    </div>

    <style>
        body {
            background-color: #fff;
            color: #333;
            font-family: Arial, sans-serif;
        }

        h2 {
            margin-top: 20px;
        }

        .edit-form {
            max-width: 400px;
            margin: 30px auto;
            padding: 20px;
            background-color: #f9f9f9;
            border: 1px solid #ccc;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            text-align: center;
        }

        .input-box {
            width: 100%;
            padding: 10px;
            margin: 8px 0;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 16px;
            box-sizing: border-box;
        }

        .btn-save {
            padding: 10px 20px;
            margin: 8px 5px;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-save {
            background-color: #4682B4; /* blue save button */
            color: #fff;
        }

        .btn-save:hover {
            background-color: #5a9bd3;
        }

        .status-msg {
            display: block;
            margin-bottom: 15px;
            font-weight: bold;
            color: #4682B4;
        }
    </style>
</asp:Content>