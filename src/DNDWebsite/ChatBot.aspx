<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChatBot.aspx.cs" Inherits="DNDWebsite.ChatBot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Chat with the DND Bot! Use "Keyword" to return to directory</h2>
    <hr />
    <div><asp:Label ID="lblYou" runat="server"></asp:Label></div>
    <hr
    <div>
        
        <asp:TextBox ID="txtChat" runat="server" Height="245px" ReadOnly="True" Width="1605px" TextMode="MultiLine"></asp:TextBox></div>
    <hr />
    <div><asp:Label ID="Label1" runat="server" Text="Enter a Question"></asp:Label><asp:TextBox ID="txtInput" runat="server" style="margin-top: 8px" Width="497px"></asp:TextBox>
    </div>
     <hr />
    <div><asp:Button ID="btnEnter" runat="server" Text="Enter" OnClick="btnEnter_Click" /></div>
    
</asp:Content>
