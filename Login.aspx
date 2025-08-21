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
        .link-btn { background:none; border:none; color:#FFD700; cursor:pointer; text-decoration:underline; font-size:0.95rem; margin:5px 0; }
        .link-btn:hover { color:#fff; }
        h2 { margin:28px 0 10px; }
        .wrap { max-width:420px; margin:0 auto; background:#1a1a1a; padding:24px; border:1px solid #333; border-radius:10px; }
        hr { border:0; border-top:1px solid #333; margin:24px 0; }
        .msg { display:block; margin-top:12px; min-height:24px; }
        #signupSection { display:none; margin-top:20px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrap">

            <!-- Login Section -->
            <div id="loginSection">
                <h2>Login</h2>
                <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email"></asp:TextBox><br />
                <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" /><br />

                <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password?" CssClass="link-btn" OnClientClick="alert('Password recovery coming soon!'); return false;" /><br />
                <asp:Button ID="btnShowSignup" runat="server" Text="Create Account" CssClass="link-btn" OnClientClick="showSignup(); return false;" />
            </div>

            <!-- Signup Section (hidden by default) -->
            <div id="signupSection">
                <h2>Sign Up</h2>
                <asp:TextBox ID="txtSignupName" runat="server" placeholder="Full Name"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Email"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn" OnClick="btnSignup_Click" /><br />
                <asp:Button ID="btnBackToLogin" runat="server" Text="Back to Login" CssClass="link-btn" OnClientClick="showLogin(); return false;" />
            </div>

            <!-- Message Label -->
            <asp:Label ID="lblMessage" runat="server" CssClass="msg" /><br />

            <!-- Back to Home button (always visible) -->
            <asp:Button ID="btnBackToHome" runat="server" Text="Back to Home" CssClass="btn" OnClick="btnBackToHome_Click" />

        </div>
    </form>

    <script type="text/javascript">
        function showSignup() {
            document.getElementById('loginSection').style.display = 'none';
            document.getElementById('signupSection').style.display = 'block';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }

        function showLogin() {
            document.getElementById('signupSection').style.display = 'none';
            document.getElementById('loginSection').style.display = 'block';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }
    </script>
</body>
</html>
