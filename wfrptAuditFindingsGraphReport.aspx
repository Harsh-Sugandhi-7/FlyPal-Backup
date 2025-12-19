<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAuditFindingsGraphReport.aspx.vb"
    Inherits="Flypal.wfrptAuditFindingsGraphReport" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Audit Findings Graph Report</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
    
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <p>
    </p>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" Width="536px" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Audit Findings Graph Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="6">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvAuditNo" runat="server" CssClass="clsLabelAuto" OnServerValidate="CustomValidate"
                                    ControlToValidate="cmbAuditInfoList" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Year</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table id="Table1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto">Year</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox2">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Audit No.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 13px">
                                            <asp:Label ID="lblAuditNo" runat="server" CssClass="clsLabelAuto">Audit No.</asp:Label>
                                        </td>
                                        <td style="height: 13px">
                                            <asp:DropDownList ID="cmbAuditInfoList" runat="server" CssClass="clsComboBox2" DataValueField="AuditNo"
                                                DataTextField="AuditNo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Department</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDepartment" runat="server" CssClass="clsLabelAuto">Department</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbDepartmentList" runat="server" CssClass="clsComboBox2" DataValueField="ID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Finding Status</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFindingStatus" runat="server" CssClass="clsLabel">Finding Status</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbFindingStatus" runat="server" CssClass="clsComboBox2" DataValueField="ID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblAuditNo1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblDepartment1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblFindingStatus1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="6">
                                <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonlong"
                                                    ToolTip="Click to display Current Searching criterias." Text="Current Criteria">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Display Report"
                                                    Text="Display"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to close Audit Findings Graph Report screen"
                                                    Text="Close" CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
