<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForElectronicLogRegister.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForElectronicLogRegister" %>

<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Search Criteria for elctronic Log Register</title>
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Search criteria for Electronic Log Register</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Aircraft"
                                    ControlToValidate="cmbAircraft" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvAssembly" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="Assembly Required" ControlToValidate="cmbAssembly" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
                            </td>
                            <td>
                                <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate" runat="server" ReadOnly="True"
                                                MaxLength="10" ToolTip="Enter From Date"></asp:TextBox>
                                        </td>
                                        <td>
                                            <obout:Calendar ID="calFromDate" runat="server" DatePickerButtonText='<img src=" Icons\calendar.bmp">'
                                                DatePickerMode="True" TextArrowRight='<img src=" Icons\Next.bmp">' TextArrowLeft='<img src=" Icons\Previous.bmp">'
                                                StyleFolder="Styles/Default" ScriptPath="script" TextBoxId="txtFromDate" DoubleCalendarMode="False"
                                                SelectedDate="2006-09-28" DateMin="1900-01-01" DateMax="2050-01-01" AutoPostBack="True">
                                            </obout:Calendar>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
                            </td>
                            <td>
                                <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtToDate" CssClass="clsTextBoxDate" runat="server" ReadOnly="True"
                                                MaxLength="10" ToolTip="Enter To Date"></asp:TextBox>
                                        </td>
                                        <td>
                                            <obout:Calendar ID="calToDate" runat="server" DatePickerButtonText='<img src=" Icons\calendar.bmp">'
                                                DatePickerMode="True" TextArrowRight='<img src=" Icons\Next.bmp">' TextArrowLeft='<img src=" Icons\Previous.bmp">'
                                                StyleFolder="Styles/Default" ScriptPath="script" TextBoxId="txtToDate" DoubleCalendarMode="False"
                                                SelectedDate="2006-09-28" DateMin="1900-01-01" DateMax="2050-01-01" AutoPostBack="True">
                                            </obout:Calendar>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 1px" align="left">
                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                            </td>
                            <td style="height: 1px" align="left" colspan="7">
                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox3" AutoPostBack="True"
                                    DataTextField="RegNo" DataValueField="MachineID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="8">
                                <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Engine</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                            </td>
                            <td align="left" colspan="7">
                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3" AutoPostBack="True"
                                    DataTextField="Description" DataValueField="AssemblyID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                                <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                            <td align="left" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
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
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Back to Previous Page"
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
