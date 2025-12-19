<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAuditRegister.aspx.vb"
    Inherits="Flypal.wfrptAuditRegister" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Audit Register</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 
        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAuditScheduleList" runat="server" CssClass="clstitle1">Audit Register</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvtxtFromDate" runat="server" ControlToValidate="txtFromDate"
                                    Display="None" ErrorMessage="From Date required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvtxtToDate" runat="server" ControlToValidate="txtToDate"
                                    Display="None" ErrorMessage="To Date required" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="66px">From Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtFromDate" runat="server"></uc1:SICalendar>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Width="52px">To Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtToDate" runat="server"></uc1:SICalendar>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Audit No.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 13px">
                                <asp:Label ID="lblAuditNo" runat="server" CssClass="clsLabelAuto">Audit No.</asp:Label>
                            </td>
                            <td style="height: 13px" colspan="3">
                                <asp:DropDownList ID="cmbAuditNo" runat="server" CssClass="clsComboBox2" DataTextField="AuditNo"
                                    DataValueField="AuditNo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step III. Selection of Lead Auditor</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 9px">
                                <asp:Label ID="lblLeadAuditor" runat="server" CssClass="clsLabelAuto">Lead Auditor</asp:Label>
                            </td>
                            <td style="height: 9px" colspan="3">
                                <asp:DropDownList ID="cmbLeadAuditor" runat="server" CssClass="clsComboBox2" DataTextField="Name"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Audit Type</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAuditType" runat="server" CssClass="clsLabelAuto">Audit Type</asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="cmbAuditType" runat="server" CssClass="clsComboBox2" DataTextField="Name"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step V. Selection of Audit Status</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 11px">
                                <asp:Label ID="lblAuditStatus" runat="server" CssClass="clsLabelAuto">Audit Status</asp:Label>
                            </td>
                            <td style="height: 11px" colspan="3">
                                <asp:DropDownList ID="cmbAuditStatus" runat="server" CssClass="clsComboBox2" DataTextField="Name"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
                                </td>
                            </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAuditNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblLeadAuditor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAuditType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblAuditStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <table class="clstableButton" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonlong"
                                                ToolTip="Click to display Current Searching criterias" Text="Current Criteria">
                                            </asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton" ToolTip="Click to display Report "
                                                Text="Display"></asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close Audit Register screen"
                                                Text="Close"></asp:Button>
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
