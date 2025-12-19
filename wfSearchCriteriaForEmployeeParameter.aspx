<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeParameter.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeParameter" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Employee Parameter Report</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
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
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Employee Skill</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Employee</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCrew" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbCrewList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                        DataTextField="EmpNoName" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Skill</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblParameter" runat="server" CssClass="clsLabelAuto">Skill</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="cmbParameterList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                        DataTextField="Name" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Value</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValue" runat="server" CssClass="clsLabelAuto">Value</asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Value" Height="25px"
                                        MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStepIV" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblCrewSelection" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblParameterSelection" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblValueSelect" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                        ToolTip="Click to display Current Searching criterias." Text="Current Criteria"></asp:Button>
                                    <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Display Report"
                                        Text="Display"></asp:Button>
                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close Employee Skill screen"
                                        Text="Close" CausesValidation="False"></asp:Button>
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
