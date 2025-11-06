<%@ Register Assembly="CrystalDecisions.Web" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="DNDWebsite.Report" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="text-align:center; color:#2F4F4F;">Reports</h2>

    <h3 style="text-align:center; color:#4682B4;">Client Order Summary</h3>
    <h3 style="text-align:center; color:#4682B4;">Monthly Revenue Report</h3>
    <h3 style="text-align:center; color:#4682B4;">Supplier Product Performance</h3>
    <style>
        h3 {
            margin-top: 40px;
            text-align: center;
        }
        .crystalReportViewer {
            margin: 20px auto;
            max-width: 90%;
            border: 1px solid #ccc;
            background-color: #fdfdfd;
            padding: 10px;
        }
    </style>
</asp:Content>
