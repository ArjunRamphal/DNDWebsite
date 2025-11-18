<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DNDWebsite.Login" ClientIDMode="Static" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login / Signup</title>
    <style>
        body { font-family: Arial, sans-serif; background:#ffffff; text-align:center; margin:0; padding:40px; }
        
        input, select, .aspNetDisabled, .aspNetInput { 
            padding:10px; 
            margin:5px; 
            border-radius:6px; 
            border:1px solid #ccc; 
            width:260px; 
            background:#f5f5f5; 
            box-sizing: border-box;
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
            color:#0000EE; 
            cursor:pointer; 
            text-decoration:underline; 
            font-size:0.95rem; 
            margin:5px 0; 
        }
        
        .link-btn:hover { 
            color:#0000EE;
        }
        
        h2 { 
            margin:28px 0 10px;
        }
        
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
        }
        
        #signupSection, #resetSection { 
            display:none; 
            margin-top:20px; 
        }

        .password-wrapper {
            display: inline-block;
            position: relative;
            width: auto;
        }

        .password-wrapper input {
            width: 260px;
            padding-right: 30px;
            box-sizing: border-box;
        }

        .password-wrapper .toggle-password {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
            font-size: 18px;
            color: #888;
            user-select: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfSetPasswordMode" runat="server" Value="false" />

        <div class="wrap">

            <!-- LOGIN SECTION -->
            <div id="loginSection" runat="server">
                <h2>Login</h2>
                <asp:TextBox ID="txtLoginEmail" runat="server" placeholder="Email"></asp:TextBox><br />

                <div class="password-wrapper">
                    <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <span class="toggle-password">&#128065;</span>
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" /><br />

                <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password?" CssClass="link-btn" OnClientClick="showReset(); return false;" /><br />
                <asp:Button ID="btnShowSignup" runat="server" Text="Create Account" CssClass="link-btn" OnClientClick="showSignup(); return false;" /><br />

                <asp:Label ID="lblMessage" runat="server" CssClass="msg" /><br />
            </div>

            <!-- SIGNUP / SET PASSWORD SECTION -->
            <div id="signupSection" runat="server">
                <h2 id="signupTitle" runat="server">Sign Up</h2>
                <asp:TextBox ID="txtSignupName" runat="server" placeholder="Full Name"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupPhone" runat="server" placeholder="Phone Number"></asp:TextBox><br />
                <asp:TextBox ID="txtSignupEmail" runat="server" placeholder="Email"></asp:TextBox><br />
                <div class="password-wrapper">
                    <asp:TextBox ID="txtSignupPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <span class="toggle-password">&#128065;</span>
                </div>

                <!-- Verification Question & Answer -->
                <asp:DropDownList ID="ddlQuestion" runat="server">
                    <asp:ListItem Text="Select a verification question" Value="" />
                    <asp:ListItem Text="What was your childhood nickname?" Value="What was your childhood nickname?" />
                    <asp:ListItem Text="What was the name of your first pet?" Value="What was the name of your first pet?" />
                    <asp:ListItem Text="What is your mother's maiden name?" Value="What is your mother's maiden name?" />
                    <asp:ListItem Text="What is your favourite colour?" Value="What is your favourite colour?" />
                    <asp:ListItem Text="What is your favourite fruit?" Value="What is your favourite fruit?" />
                </asp:DropDownList><br />
                <asp:TextBox ID="txtSignupAnswer" runat="server" placeholder="Answer to verification question"></asp:TextBox><br />

                <asp:Button ID="btnSignup" runat="server" Text="Submit" CssClass="btn" OnClick="btnSignup_Click" /><br />
                <asp:Label ID="lblSignupMessage" runat="server" CssClass="msg" /><br />
                <asp:Button ID="btnBackToLogin" runat="server" Text="Back to Login" CssClass="link-btn" OnClientClick="showLogin(); return false;" />
            </div>

            <!-- PASSWORD RESET SECTION -->
            <div id="resetSection" runat="server">
                <h2>Reset Password</h2>
                <asp:Label ID="lblResetMessage" runat="server" CssClass="msg" /><br />

                <!-- Step 1: Email -->
                <asp:TextBox ID="txtResetEmail" runat="server" placeholder="Enter your email"></asp:TextBox><br />
                <asp:Button ID="btnGetQuestion" runat="server" Text="Next" CssClass="btn" OnClick="btnGetQuestion_Click" /><br />

                <!-- Step 2: Verification + New Password -->
                <asp:Panel ID="pnlVerification" runat="server" Visible="false">
                    <asp:Label ID="lblVerificationQuestion" runat="server" Text="" /><br />
                    <asp:TextBox ID="txtResetAnswer" runat="server" placeholder="Answer"></asp:TextBox><br />

                    <div class="password-wrapper">
                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" placeholder="New Password"></asp:TextBox>
                        <span class="toggle-password">&#128065;</span>
                    </div>

                    <div class="password-wrapper">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm New Password"></asp:TextBox>
                        <span class="toggle-password">&#128065;</span>
                    </div>

                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn" OnClick="btnResetPassword_Click" /><br />
                </asp:Panel>

                <asp:Button ID="btnBackToLoginFromReset" runat="server" Text="Back to Login" CssClass="link-btn" OnClientClick="showLoginResetBack(); return false;" />
            </div>

            <asp:Button ID="btnBackToHome" runat="server" Text="Back to Home" CssClass="btn" OnClick="btnBackToHome_Click" />

        </div>
    </form>

    <script type="text/javascript">
        function showSignup() {
            document.getElementById('loginSection').style.display = 'none';
            document.getElementById('resetSection').style.display = 'none';
            document.getElementById('signupSection').style.display = 'block';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }

        function showLogin() {
            if (window.location.search.indexOf('setpassword=1') > -1) {
                window.location.href = 'Login.aspx';
                return;
            }

            document.getElementById('signupSection').style.display = 'none';
            document.getElementById('resetSection').style.display = 'none';
            document.getElementById('loginSection').style.display = 'block';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }

        function showReset() {
            document.getElementById('loginSection').style.display = 'none';
            document.getElementById('signupSection').style.display = 'none';
            document.getElementById('resetSection').style.display = 'block';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }

        function showLoginResetBack() {
            var loginSection = document.getElementById('loginSection');
            var resetSection = document.getElementById('resetSection');

            if (loginSection) {
                if (resetSection) resetSection.style.display = 'none';
                loginSection.style.display = 'block';

                var emailInput = document.getElementById('<%= txtResetEmail.ClientID %>');
                if (emailInput) emailInput.style.display = 'block';

                var nextBtn = document.getElementById('<%= btnGetQuestion.ClientID %>');
                if (nextBtn) nextBtn.style.display = 'inline-block';

                var verifyPanel = document.getElementById('<%= pnlVerification.ClientID %>');
                if (verifyPanel) verifyPanel.style.display = 'none';

                window.scrollTo({ top: 0, behavior: 'smooth' });
            }
            else {
                window.location.href = 'Login.aspx';
            }
        }

        document.querySelectorAll('.password-wrapper').forEach(wrapper => {
            const input = wrapper.querySelector('input[type="password"], input[type="text"]');
            const toggle = wrapper.querySelector('.toggle-password');

            toggle.addEventListener('mousedown', () => {
                input.type = 'text';
            });

            toggle.addEventListener('mouseup', () => {
                input.type = 'password';
            });

            toggle.addEventListener('mouseleave', () => {
                input.type = 'password';
            });
        });
    </script>
</body>
</html>
