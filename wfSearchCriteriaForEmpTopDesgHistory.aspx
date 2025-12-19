<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmpTopDesgHistory.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmpTopDesgHistory" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Employee Salary History</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }
    </script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
      <link id="MainStyle" type="text/css" rel="stylesheet">
      <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    
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

        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>
    </p>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Employee Salary History</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Employee</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbCrewList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="EmpNoName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblStepIII" runat="server" CssClass="clsLabelHeader">Step II. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblCrewName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                    Text="Current Criteria" ToolTip="Click to display Current Searching criterias.">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Display"
                                                    ToolTip="Click to Display Report"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                    ToolTip="Click to close Employee Salary History screen" CausesValidation="False">
                                                </asp:Button>
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
