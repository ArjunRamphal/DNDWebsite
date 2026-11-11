<%@ Page Title="Edit Client Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditClient.aspx.cs" Inherits="DNDWebsite.EditClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#333;">Edit Your Information</h2>

    <div class="edit-form">
        <asp:Label ID="lblMessage" runat="server" CssClass="status-msg" />

        <asp:Label runat="server" Text="Full Name" AssociatedControlID="txtName" />
        <asp:TextBox ID="txtName" runat="server" CssClass="input-box" placeholder="Full Name" onchange="setDirty();" /><br />

        <asp:Label runat="server" Text="Email" AssociatedControlID="txtEmail" />
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-box" placeholder="Email" TextMode="Email" onchange="setDirty();" /><br />

        <asp:Label runat="server" Text="Phone Number" AssociatedControlID="txtPhone" />
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input-box" placeholder="Phone Number" onchange="setDirty();" /><br />

        <asp:Label runat="server" Text="Password (leave blank to keep current)" AssociatedControlID="txtPassword" />
        <asp:TextBox ID="txtPassword" runat="server" CssClass="input-box" TextMode="Password" onchange="setDirty();" /><br />

        <asp:Label runat="server" Text="Security Question" AssociatedControlID="ddlQuestion" />
        <asp:DropDownList ID="ddlQuestion" runat="server" CssClass="input-box" AutoPostBack="false" onchange="setDirty();">
        </asp:DropDownList><br />

        <asp:Label runat="server" Text="Security Answer" AssociatedControlID="txtAnswer" />
        <asp:TextBox ID="txtAnswer" runat="server" CssClass="input-box" placeholder="Answer" onchange="setDirty();" /><br />

        <asp:CheckBox ID="chkOptOut" runat="server" Text="I want to opt out / archive my account" onchange="setDirty();" /><br />

        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn-save" OnClick="btnSave_Click" />
        
        <asp:Button ID="btnModalSave" runat="server" style="display:none;" OnClick="btnSave_Click" />
        <br />
    </div>

    <asp:HiddenField ID="hfOriginalQuestion" runat="server" />
    <asp:HiddenField ID="hfExitMode" runat="server" Value="false" />

    <div id="confirmModal" style="display:none; position:fixed; left:0; top:0; right:0; bottom:0; background:rgba(0,0,0,0.5); z-index:2000;">
        <div style="background:#fff; width:420px; max-width:90%; margin:60px auto; padding:18px; border-radius:8px; text-align:center;">
            <h3>Unsaved Changes</h3>
            <p>You have unsaved changes. Do you want to save them before leaving?</p>
            <div style="margin-top:12px;">
                <button type="button" class="btn-search" onclick="confirmYes();">Yes</button>
                &nbsp;
                <button type="button" class="btn-search" style="background:#ccc; color:#333;" onclick="confirmNo();">No</button>
            </div>
        </div>
    </div>

    <div id="statusModal" style="display:none; position:fixed; left:0; top:0; right:0; bottom:0; background:rgba(0,0,0,0.5); z-index:2001;">
        <div style="background:#fff; width:420px; max-width:90%; margin:60px auto; padding:18px; border-radius:8px; text-align:center;">
            <h3 id="statusTitle">Notification</h3>
            <p id="statusText">Message goes here</p>
            <div style="margin-top:12px;">
                <button type="button" class="btn-search" onclick="redirectToHome();">OK</button>
            </div>
        </div>
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
        .btn-save:hover { 
            background-color: #5a9bd3; 
        }

        .btn-search {
            padding: 8px 14px;
            background-color: #4682B4;
            color: #fff;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
        }
        .btn-search:hover { 
            background-color: #5a9bd3; 
        }

        .status-msg {
            display: block;
            margin-bottom: 12px;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        var isDirty = false;

        function setDirty() {
            isDirty = true;
        }

        function showConfirmModal() {
            document.getElementById('confirmModal').style.display = 'block';
        }

        function hideConfirmModal() {
            document.getElementById('confirmModal').style.display = 'none';
        }

        function showStatusModal(title, message) {
            document.getElementById('statusTitle').innerText = title;
            document.getElementById('statusText').innerText = message;
            document.getElementById('statusModal').style.display = 'block';
        }

        function confirmYes() {
            hideConfirmModal();
            document.getElementById('<%= hfExitMode.ClientID %>').value = "true";
            document.getElementById('<%= btnModalSave.ClientID %>').click();
        }

        function confirmNo() {
            hideConfirmModal();
            showStatusModal("Notification", "Changes not saved.");
        }

        function redirectToHome() {
            window.location.href = 'Default.aspx';
        }

        document.addEventListener("DOMContentLoaded", function () {
            var links = document.querySelectorAll("nav a");
            links.forEach(function (link) {
                if (link.innerText === "Home" || link.href.indexOf("Default.aspx") > -1) {
                    link.addEventListener("click", function (e) {
                        if (isDirty) {
                            e.preventDefault();
                            showConfirmModal();
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>