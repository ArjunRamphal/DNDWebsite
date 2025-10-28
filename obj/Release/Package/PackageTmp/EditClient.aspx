<%@ Page Title="Edit Client Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditClient.aspx.cs" Inherits="DNDWebsite.EditClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#333;">Edit Your Information</h2>

    <div class="edit-form">
        <asp:Label ID="lblMessage" runat="server" CssClass="status-msg" />

        <asp:Label runat="server" Text="Full Name" AssociatedControlID="txtName" />
        <asp:TextBox ID="txtName" runat="server" CssClass="input-box" placeholder="Full Name" /><br />

        <asp:Label runat="server" Text="Email" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" placeholder="Email" TextMode="Email" /><br />

        <asp:Label runat="server" Text="Phone Number" AssociatedControlID="txtPhone" />
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input-box" placeholder="Phone Number" /><br />

        <asp:Label runat="server" Text="Password (leave blank to keep current)" AssociatedControlID="txtPassword" />
        <asp:TextBox ID="txtPassword" runat="server" CssClass="input-box" TextMode="Password" /><br />

        <asp:Label runat="server" Text="Security Question" AssociatedControlID="ddlQuestion" />
        <asp:DropDownList ID="ddlQuestion" runat="server" CssClass="input-box" AutoPostBack="false">
        </asp:DropDownList><br />

        <asp:Label runat="server" Text="Security Answer" AssociatedControlID="txtAnswer" />
        <asp:TextBox ID="txtAnswer" runat="server" CssClass="input-box" placeholder="Answer" /><br />

        <asp:CheckBox ID="chkOptOut" runat="server" Text="I want to opt out / archive my account" /><br />

        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn-save" OnClick="btnSave_Click" /><br />
    </div>

    <asp:HiddenField ID="hfOriginalQuestion" runat="server" />

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
            max-width: 460px;
            margin: 30px auto;
            padding: 20px;
            background-color: #f9f9f9;
            border: 1px solid #ccc;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.05);
            text-align: left;
        }

        .input-box {
            width: 100%;
            padding: 10px;
            margin: 6px 0 12px 0;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 14px;
            box-sizing: border-box;
        }

        .btn-save {
            padding: 10px 20px;
            margin: 8px 5px;
            border: none;
            border-radius: 6px;
            font-weight: bold;
            cursor: pointer;
            background-color: #4682B4;
            color: #fff;
        }

        .btn-save:hover { background-color: #5a9bd3; }

        .status-msg {
            display: block;
            margin-bottom: 12px;
            font-weight: bold;
        }
    </style>
</asp:Content>
