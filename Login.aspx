<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="DNDWebsite.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login / Signup</title>
    <style>
        body { font-family: Arial, sans-serif; background:#111; color:#FFD700; text-align:center; margin:0; padding:40px; }
        input, .aspNetDisabled, .aspNetInput { padding:10px; margin:5px; border-radius:6px; border:1px solid #444; width:260px; background:#000; color:#FFD700; }
        .btn { background:#FFD700; color:#000; font-weight:bold; padding:10px 18px; border:none; border-radius:6px; cursor:pointer; margin-top:8px; }
        .btn:hover { background:#fff; color:#000; }
        h2 { margin:28px 0 10px; }
        .wrap { max-width:420px; margin:0 auto; background:#1a1a1a; padding:24px; border:1px solid #333; border-radius:10px; }
        hr { border:0; border-top:1px solid #333; margin:24px 0; }
        .msg { display:block; margin-top:12px; min-height:24px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrap">
            <h2>Login</h2>
            <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email"></asp:TextBox><br />
            <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />

            <hr />

            <h2>Sign Up</h2>
            <asp:TextBox ID="txtSignupName" runat="server" placeholder="Full Name"></asp:TextBox><br />
            <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Email"></asp:TextBox><br />
            <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
            <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn" OnClick="btnSignup_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="msg" />
        </div>
    </form>
</body>
</html>
