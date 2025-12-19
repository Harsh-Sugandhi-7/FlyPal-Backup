<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeApproachDuesAsPerStandardsForFTL.aspx.vb"
    Inherits="Flypal.wfEmployeeApproachDuesAsPerStandardsForFTL" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Employee Approach Dues As Per Standards</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td colspan="1">
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Employee Approach Dues As Per Standards For FTL</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                            </td>
                            <td>
                            </td>
                            <td colspan="3">
                                <uc1:SICalendar ID="txtToDate" runat="server"></uc1:SICalendar>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Pilot</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmployee" runat="server" CssClass="clsLabelAuto">Pilot</asp:Label>
                            </td>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsComboBox" AutoPostBack="True"
                                    DataTextField="EmpNoName" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step III. Selection of Standard</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="4">
                                <asp:RadioButton ID="rbCompanyStandard" runat="server" CssClass="clsRadioButton"
                                    Checked="True" GroupName="a" Text="Company Standard"></asp:RadioButton>
                                <asp:RadioButton ID="rbGovtStandard" runat="server" CssClass="clsRadioButton" GroupName="a"
                                    Text="Govt. Standard"></asp:RadioButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblServiceCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right" colspan="4">
                                <table class="clstableButton" id="Table3" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonlong"
                                                Text="Current Criteria" CausesValidation="False" ToolTip="Click to display current searching criterias">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDisplay" runat="server" Text="Display" ToolTip="Click to display report"
                                                CssClass="clsButton"></asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Click to Close Employee Approach Dues As Per Standards For FTL screen"
                                                CssClass="clsButton"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
