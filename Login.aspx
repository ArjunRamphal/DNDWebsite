<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="DNDWebsite.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login / Signup</title>
    <style>
        body { 
            font-family: Arial, sans-serif; 
            background:#ffffff; 
            color:#2F4F4F; 
            text-align:center; 
            margin:0; 
            padding:40px; 
        }
        input, .aspNetDisabled, .aspNetInput { 
            padding:10px; 
            margin:5px; 
            border-radius:6px; 
            border:1px solid #ccc; 
            width:260px; 
            background:#f5f5f5; 
            color:#2F4F4F; 
        }
        .btn { 
            background:#4682B4; 
            color:#fff; 
            font-weight:bold; 
            padding:10px 18px; 
            border:none; 
            border-radius:6px; 
            cursor:pointer; 
            margin-top:8px; 
        }
        .btn:hover { 
            background:#5a9bd4; 
        }
        .link-btn { 
            background:none; 
            border:none; 
            color:#2F4F4F; 
            cursor:pointer; 
            text-decoration:underline; 
            font-size:0.95rem; 
            margin:5px 0; 
        }
        .link-btn:hover { 
            color:#2F4F4F; 
        }
        h2 { margin:28px 0 10px; }
        .wrap { 
            max-width:420px; 
            margin:0 auto; 
            background:#f9f9f9; 
            padding:24px; 
            border:1px solid #ddd; 
            border-radius:10px; 
        }
        hr { 
            border:0; 
            border-top:1px solid #ddd; 
            margin:24px 0; 
        }
        .msg { 
            display:block; 
            margin-top:12px; 
            min-height:24px; 
            color:#2F4F4F; 
        }
        #signupSection { 
            display:none; 
            margin-top:20px; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrap">

            <!-- Login Section -->
            <div id="loginSection" runat="server">
                <h2>Login</h2>
                <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email/Username"></asp:TextBox><br />
                <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" /><br />

                <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password?" CssClass="link-btn" OnClientClick="alert('Password recovery coming soon!'); return false;" /><br />
                <asp:Button ID="btnShowSignup" runat="server" Text="Create Account" CssClass="link-btn" OnClientClick="showSignup(); return false;" /><br />

                <!-- Login messages -->
                <asp:Label ID="lblMessage" runat="server" CssClass="msg" /><br />
            </div>

            <!-- Signup Section (hidden by default) -->
            <div id="signupSection" runat="server">
                <h2>Sign Up</h2>
                <asp:TextBox ID="txtSignupName" runat="server" placeholder="Full Name"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupPhone" runat="server" placeholder="Phone Number"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Email"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox><br />
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn" OnClick="btnSignup_Click" /><br />

                <!-- Signup messages -->
                <asp:Label ID="lblSignupMessage" runat="server" CssClass="msg" /><br />

                <asp:Button ID="btnBackToLogin" runat="server" Text="Back to Login" CssClass="link-btn" OnClientClick="showLogin(); return false;" />
            </div>

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