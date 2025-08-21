<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="DNDWebsite.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login / Signup</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #111;
            color: #FFD700;
            text-align: center;
            padding: 40px;
        }
        input, .aspNetDisabled, .aspNetInput {
            padding: 10px;
            margin: 5px;
            border-radius: 6px;
            border: 1px solid #ccc;
            width: 250px;
        }
        .btn {
            background-color: #FFD700;
            color: #000;
            font-weight: bold;
            padding: 10px 18px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 0.95rem;
            margin-top: 10px;
        }
        .btn:hover {
            background-color: #fff;
            color: #000;
        }
        h2 {
            margin-top: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login</h2>
            <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email"></asp:TextBox><br />
            <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" /><br />

            <h2>Sign Up</h2>
            <asp:TextBox ID="txtSignupName" runat="server" placeholder="Full Name"></asp:TextBox><br />
            <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Email"></asp:TextBox><br />
            <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
            <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn" OnClick="btnSignup_Click" /><br />

            <br />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
