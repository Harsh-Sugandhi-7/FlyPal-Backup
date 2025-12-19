<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForEmployeeDocument.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForEmployeeDocument" %>
    <%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Flight Duty Time Limit Entry </title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>



    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtValidity');

            // Prevent copy operation
            textBox.addEventListener('copy', function (event) {
                event.preventDefault();
            });

            // Prevent paste operation
            textBox.addEventListener('paste', function (event) {
                event.preventDefault();
            });
        });
    </script>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtWarningDays');

            // Prevent copy operation
            textBox.addEventListener('copy', function (event) {
                event.preventDefault();
            });

            // Prevent paste operation
            textBox.addEventListener('paste', function (event) {
                event.preventDefault();
            });
        });
    </script>
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
      <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
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
            <td colspan="1">
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="4" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Employee Document</asp:Label>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Employee</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmployee" runat="server" CssClass="clsLabelAuto">Employee</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                    DataTextField="EmpNoName" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkWorkingEmpOnly" runat="server" CssClass="clsCheckBox" Text="Consider working employees only"
                                    AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Document Details</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDocument" runat="server" CssClass="clsLabelAuto">Document</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbDocumentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                    DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblValidity" runat="server" CssClass="clsLabelAuto">Validity</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValidity" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Validity"
                                    MaxLength="4" Height="25px"></asp:TextBox>
                                <span class="clsLabelAuto">(In Days/Months/Years)</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDocumentNo" runat="server" CssClass="clsLabelAuto">Document No.</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Document No."
                                    MaxLength="20" Height="25px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblWarningDays" runat="server" CssClass="clsLabelAuto">Warning Days</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Warning Days"
                                    MaxLength="4" Height="25px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader" Visible="False">Step III. Selection of Date of Expiry</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDateExpiry" runat="server" CssClass="clsLabelAuto" Visible="False">From Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtFromDateExpiry" runat="server" Visible="False"></uc1:SICalendar>
                            </td>
                            <td>
                                <asp:Label ID="lblToDateExpiry" runat="server" CssClass="clsLabelAuto" Visible="False">To Date</asp:Label>
                            </td>
                            <td>
                                <uc1:SICalendar ID="txtToDateExpiry" runat="server" Visible="False"></uc1:SICalendar>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbPortrait" runat="server" CssClass="clsRadioButton" Text="Portrait"
                                    GroupName="a" Checked="True"></asp:RadioButton>
                                <asp:RadioButton ID="rbLandscape" runat="server" CssClass="clsRadioButton" Text="Landscape"
                                    GroupName="a"></asp:RadioButton>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step III. Selection of Applicability of Document(s)</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">Applicability</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbApplicability" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                    <asp:ListItem Text="(All)" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not Applicable" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 14px" colspan="4">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows : </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblEmployeeCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDocumentCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblValiditycriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDocumentNoCriteria" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblWarning" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblApplicability" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <table class="clstableButton" id="Table3" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                ToolTip="Click to Display Current Searching criterias." Text="Current Criteria"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDisplay" runat="server" ToolTip="Click to Display Report" Text="Display"
                                                CssClass="clsbtnH clsinfoH1"></asp:Button>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnClose" runat="server" ToolTip="Click to close Employee Document screen"
                                                Text="Close" CssClass="clsbtnH clsinfoH1"></asp:Button>
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
