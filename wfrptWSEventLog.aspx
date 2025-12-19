<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfrptWSEventLog.aspx.vb" Inherits="Flypal.wfrptWSEventLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.ModelMonitorModTypeList" %>
<%@ Import Namespace="Flypal.PartMonitorServiceTypeList" %>
<%@ Import Namespace="Flypal.ModelMonitorInspTypeList" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
	<HEAD runat ="server" >
		<title>CRS/SMS Event Log</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
		<script id="clientEventHandlersJS" language="javascript">
			function openTranDetail()
			{
				str = "wfReports.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openTranDetail1()
			{
				str = "webform1.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
			function openDetail()
			{
				str = "wfDetail.aspx"
				window.open(str,"",'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
			}
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="0" rightMargin="5" topMargin="5" MS_POSITIONING="GridLayout">
		<form id="wfgroup" method="post" runat="server">

			 <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>

			<table id="tblmain" class="clstablelistout" border="0">
				<tr>
					<td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
							<TABLE id="tblInner" class="clstablelistin" border="0">
								<TR>
									<TD colSpan="6" class="clsFormHeader1Newstyle">
										<asp:Label id="lbltitle" Runat="server" CssClass="clsFormHeader">Event Log</asp:Label></TD>
								</TR>
								<TR>
									<TD colSpan="6"></TD>
								</TR>
								<TR>
									<TD colSpan="6">
										<asp:label id="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:label></TD>
								</TR>
								<TR>
									<TD>
										<asp:label id="lblDateRange" runat="server" CssClass="clsLabel">Date Range</asp:label></TD>
									<TD>
										<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbDateRange" runat="server" AutoPostBack="True">
											<asp:ListItem Value="0">(All)</asp:ListItem>
											<asp:ListItem Value="1">Last Week</asp:ListItem>
											<asp:ListItem Value="2">Last Month</asp:ListItem>
											<asp:ListItem Value="3">Last Quarter</asp:ListItem>
											<asp:ListItem Value="4">Last Year</asp:ListItem>
											<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
											<asp:ListItem Value="6">Between Dates</asp:ListItem>
										</asp:DropDownList></TD>
									<TD>
										<asp:Label id="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label></TD>
									<TD>
										<TABLE id="Table2" border="0" cellSpacing="0">
											<TR>
												<TD>
													<asp:requiredfieldvalidator id="rfvFromDate" Runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFromDate"
														Display="None" ErrorMessage="From Date Required"></asp:requiredfieldvalidator></TD>
												<%--<TD>
													<uc1:sicalendar id="txtFromDate" runat="server" Visible="False"></uc1:sicalendar></TD>--%>

												<td>
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                            </td>

											</TR>
										</TABLE>
									</TD>
									<TD>
										<asp:Label id="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label></TD>
									<TD>
										<TABLE id="Table3" border="0" cellSpacing="0">
											<TR>
												<TD>
													<asp:requiredfieldvalidator id="rvfToDate" Runat="server" CssClass="clsLabelAuto" ControlToValidate="txtToDate"
														Display="None" ErrorMessage="To Date Required"></asp:requiredfieldvalidator></TD>
												

												 <td align="left">
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                                        ClientIDMode="Static" runat="server"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>

											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Select Module Name</asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblModuleName" runat="server" CssClass="clsLabel">Module Name</asp:label></TD>
									<TD align="left">
										<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbModuleName" runat="server" Visible="True" DataTextField="ModuleName"
											DataValueField="ModuleName"></asp:DropDownList></TD>
									<TD></TD>
									<TD colSpan="3"></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblActionName" runat="server" CssClass="clsLabelHeader">Step III. Select  Action </asp:label></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:label id="lblAction" runat="server" CssClass="clsLabel">Action</asp:label></TD>
									<TD align="left">
										<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbAction" runat="server">
											<asp:ListItem Value="0">(All)</asp:ListItem>
											<asp:ListItem Value="1">1</asp:ListItem>
											<asp:ListItem Value="2">2</asp:ListItem>
											<asp:ListItem Value="3">3</asp:ListItem>
										</asp:DropDownList></TD>
									<TD></TD>
									<TD colSpan="3"></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblStep8" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblTitle1" runat="server" CssClass="clsLabelHeader"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:datagrid id="dgEventLogList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
											AllowPaging="True" PageSize="25" AllowSorting="True" GridLines="Horizontal" CellPadding="5">
											<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
											<ItemStyle CssClass="clsdgItem"></ItemStyle>
											<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" ></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="SrNo" HeaderText="Sr No.">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="DateTime" ItemStyle-Width="100%" HeaderText="DateTime">
												</asp:BoundColumn>
												<asp:BoundColumn DataField="ModuleName" SortExpression="Module Name" HeaderText="Module Name">
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Action" SortExpression="Action" HeaderText="Action">
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" HeaderText="Description">
													
												</asp:BoundColumn>
											</Columns>
											<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows :</asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="6" align="left">
										<asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2" align="left">
										<asp:label id="lblModuleName1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
									<TD colSpan="4" align="left">
										<asp:label id="lblAction1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
								</TR>
								<TR>
									<TD colSpan="2" align="left"></TD>
									<TD colSpan="4" align="left"></TD>
								</TR>
								<TR>
								<TR>
									<TD colSpan="6" align="right">
										<asp:Panel id="pnlButton" Runat="server" CssClass="clspanel1">
											<TABLE cellSpacing="0">
												<TR>
													<TD>
														<asp:button CssClass="clsbtnH clsinfoH" id="btnCurrentSearchCriteria" tabIndex="0" runat="server" 
															Visible="False" Text="Current Criteria" ToolTip="Click to display Current Searching criterias."></asp:button></TD>
													<TD>
														<asp:button CssClass="clsbtnH clsinfoH" id="btnDisplay" tabIndex="0" runat="server" Text="Display"
															ToolTip="Display Report"></asp:button></TD>
													<TD>
														<asp:button CssClass="clsbtnH clsinfoH" id="btnClose" tabIndex="0" runat="server"  Text="Close" ToolTip="Click to Close"
															CausesValidation="False"></asp:button></TD>
												</TR>
											</TABLE>
										</asp:Panel></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
