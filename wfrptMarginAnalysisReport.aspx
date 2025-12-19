<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptMarginAnalysisReport.aspx.vb" Inherits="Flypal.wfrptMarginAnalysisReport" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Margin Analysis Report</title>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->

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
    <form id="wfgroup" method="post" runat="server">

        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>

        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="9" class="clsFormHeader1Newstyle">
                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Search criteria for Margin Analysis Report</asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto" Display="None" ControlToValidate="txtFromDate1"
                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clslabelauto" Display="None" ControlToValidate="txtToDate1"
                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Dates</asp:Label></td>
                            </tr>
                            <tr>

                                <td>
                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel">From Date</asp:Label></td>

                               
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate1" ClientIDMode="Static"
                                        runat="server" CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender');"
                                        AutoPostBack="true"></asp:TextBox>
                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" ClientIDMode="Static" runat="server"
                                        CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate1"></cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate1" ID="Calender_watermarkextender"
                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                </td>

                                <td>
                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label></td>
                              
                                <td colspan="3">
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtToDate1"
                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate1"></cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate1" ID="ToDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Part Number</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>

                                            <td align="left">
                                                <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label></td>
                                            <td style="width: 7px" align="left">
                                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Search</asp:Label></td>
                                            <td align="left"></td>
                                            <td colspan="3" align="left">
                                                <table id="Table1" class="clsTable1" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbSearch" runat="server" AutoPostBack="True">
                                                                <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                                <asp:ListItem Value="Part No.">Part No.</asp:ListItem>
                                                                <asp:ListItem Value="Description">Description</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                        <td>
                                                            <asp:Label ID="lblFor" runat="server" CssClass="clslabelAuto" Visible="False">For</asp:Label></td>
                                                        <td align="left">
                                                            <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearchFor" runat="server" Visible="False" MaxLength="50" Height="25px"></asp:TextBox></td>
                                                        <td align="right">
                                                            <table id="Table2" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td align="right">
                                                                        <%--<asp:button id="btnFindNow" tabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to find as per searching criteria"
																	Text="Find Now"></asp:button>--%>

                                                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find as  per searching criteria" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:DataGrid ID="dgPartSearch" runat="server" CssClass="clsGridNewStyle" AllowSorting="True" OnPageIndexChanged="NewPage"
                                        AllowPaging="True" AutoGenerateColumns="False" PageSize="25" GridLines="Horizontal" CellPadding="3">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Part Number">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid></td>
                            </tr>
                            <tr>
                                <td style="height: 22px" colspan="6" align="left">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step  III. Display Report</asp:Label></td>
                            </tr>
                            <tr>
                               <%-- <td align="left"></td>--%>
                                <td colspan="6" align="left">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label></td>
                            </tr>
                            <tr>
                               <%-- <td align="left"></td>--%>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label></td>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                               <%-- <td align="left"></td>--%>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto"></asp:Label></td>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="right"></td>
                                <td align="right"></td>
                                <td colspan="4" align="right">
                                    <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                        ToolTip="Click to Display Current Searching criterias" Text="Current Criteria" CausesValidation="False"></asp:Button></td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" ToolTip="Click to Display Report"
                                                        Text="Display" CausesValidation="False"></asp:Button></td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" ToolTip="Click to close search criteria for Margin Analysis Report screen"
                                                        Text="Close" CausesValidation="False"></asp:Button></td>
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
