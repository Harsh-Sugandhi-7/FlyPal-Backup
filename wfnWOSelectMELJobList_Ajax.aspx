<%@ page language="vb" autoeventwireup="false" codebehind="wfnWOSelectMELJobList_Ajax.aspx.vb"
	inherits="Flypal.SelectMELJobListPage" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<%@ register tagprefix="uc2" tagname="MSGBox" src="MSGBox.ascx" %>
<%@ import namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="headTag" runat="server">
	<title></title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<script src="json2.js" type="text/javascript"></script>
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">

		<!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
		<script type="text/javascript">
			$(document).ready(function () {

				$('.cbSelectRow').change(function () {

					// detect if the checkbox is checked
					var checked = $(this).prop('checked');
					// gets the table row indiect parent
					var trParent = $(this).closest('tr');

					// add or remove the css class according to the check state
					if (checked == true)
						trParent.addClass('clslightColor');
					else
						trParent.removeClass('clslightColor');
				})

					// the each is used when postback is triggered with checked rows
					.each(function (index, element) {
						var checked = $(element).prop('checked');

						if (checked == true)
							$(element).closest('tr').addClass('clslightColor');
						else
							$(element).closest('tr').removeClass('clslightColor');
					});

				// select all MEL click
				$("#chkSelectAll").change(function () {
					var checked = $(this).prop('checked');
					$('.cbSelectRow').prop('checked', checked).trigger('change');
				});

				// select all Discrepancy click
				$("#chkSelectAllDiscrepancies").change(function () {
					var checked = $(this).prop('checked');
					$('.cbSelectRow').prop('checked', checked).trigger('change');
				});

			});

		</script>
		<!-- End-->

		<asp:ScriptManager AsyncPostBackTimeout="600" ID="WOJobListPage" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblMain" class="clstablelistout">
			<tr>
				<td align="right">
					<asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
						<table class="clsTablelistin" id="tblinner">
							<tr>

								<td colspan="4" class="clsFormHeader1">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
												</asp:Label>
											</td>
											<td style="height: 44px" align="right">
												<asp:UpdatePanel ID="UpnlDone" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table3" cellspacing="1" cellpadding="1" border="0">
															<tr>
																<td align="right">
																	<asp:Button ID="btnDoneTop" runat="server" CssClass="clsbtnH clsinfoH"
																		Text="Done" ToolTip="Click to add checked records"></asp:Button>
																</td>
																<td align="right">
																	<asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH"
																		Text="Back" CausesValidation="False"></asp:Button>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="4" align="left">
									<asp:ValidationSummary ID="Validationsummary2" runat="server"
										CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="left">
									<asp:UpdatePanel ID="UpnlAsOnDat" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table2" cellspacing="1" cellpadding="1" border="0">
												<tr>
													<td>
														<asp:Label ID="lblAsOnDat" runat="server" CssClass="clsLabel">As On Date</asp:Label>
													</td>
													<td align="right">
														<table id="Table10" cellspacing="0" cellpadding="0" border="0">
															<tr>
																<td></td>
																<td>
																	<asp:TextBox ID="txtAsOnDate" runat="server" AutoPostBack="True"
																		CssClass="clsTextBoxTagSearchDate" Width="100px" Height="25px">
																	</asp:TextBox>
																	<cc2:calendarextender id="txtAsOnDate_CalendarExtender" runat="server"
																		cssclass="cal_Theme1" enabled="True"
																		format="<%$AppSettings:DateFormat%>" targetcontrolid="txtAsOnDate">
																	</cc2:calendarextender>
																	<cc2:textboxwatermarkextender id="TBWE2" runat="server"
																		targetcontrolid="txtAsOnDate" watermarkcssclass="watermarked"
																		watermarktext="<%$AppSettings:DateFormat%>" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td align="right">
									<asp:UpdatePanel ID="UpnlFindNow" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1" cellpadding="1">
												<tr>
													<td>
														<asp:Button ID="btnFindNow" runat="server"
															CssClass="clsButton" Visible="False" Text="Find Now"
															ToolTip="Click to find as per searching criteria"></asp:Button>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td style="height: 44px" colspan="3" align="left">
									<asp:UpdatePanel ID="UpnlResult" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table6" cellspacing="1" cellpadding="1" border="0">
												<tr>
													<td align="right">
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
															List of MEL / Snag Jobs as per criteria :  Record(s) found.
														</asp:Label>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top" align="right" colspan="4">

									<asp:UpdatePanel ID="UpnlGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:PlaceHolder ID="phMELJobGrid" runat="server" Visible="false">
												<table>
													<asp:GridView ID="dgMELJob" runat="server" CssClass="clsGridNewStyle" CellPadding="10"
														ToolTip='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Job", "MEL Job") %>'
														AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" GridLines="Horizontal">
														<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
														<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
														<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
														<Columns>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
															<asp:TemplateField HeaderText="Select">
																<HeaderTemplate>
																	<input type="checkbox" id="chkSelectAll" />
																</HeaderTemplate>
																<ItemTemplate>
																	<input type="checkbox" name="chkSelect" class="cbSelectRow"
																		value="<%# Eval("ID") %>"
																		<%# CheckBoxSelection(Eval("ID").ToString()) %>>
																	</input>
																</ItemTemplate>
															</asp:TemplateField>
															<asp:BoundField Visible="False" DataField="SerialNo"
																SortExpression="SerialNo" HeaderText="Sr. No."></asp:BoundField>
															<asp:BoundField DataField="DefectNo" HeaderText="Defect No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="DateOfOccurenceFormatted" HeaderText="Date Of Occurrence">
																<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
															</asp:BoundField>
															<asp:BoundField DataField="LogTextNo" SortExpression="LogTextNo" HeaderText="Log No.">
																<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
															</asp:BoundField>
															<asp:BoundField DataField="PartNoSerialNo" SortExpression="PartNoSerialNo"
																HeaderText="Component">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Defect" SortExpression="Defect" HeaderText="Defect">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Sector" SortExpression="Sector" HeaderText="Sector">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="MajorMinorTag" SortExpression="MajorMinorTag"
																HeaderText="Major/Minor">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Description" SortExpression="Description"
																HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
																<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField Visible="False" DataField="LogDateFormatted" HeaderText="Log Date">
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
																<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="FrequencyInDays" SortExpression="FrequencyInDays"
																HeaderText="Freq. In Days">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="FrequencyInHours" SortExpression="FrequencyInHours"
																HeaderText="Freq. In Hours">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="DateTimeOfDue" SortExpression="DateTimeOfDue"
																HeaderText="Due Date">
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
																<HeaderStyle HorizontalAlign="left" Wrap="False"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Action" SortExpression="Action" HeaderText="Action">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField Visible="False" DataField="InvestigationStatus"
																SortExpression="InvestigationStatus" HeaderText="Investigation Status">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="RectifiedDateFormatted" HeaderText="Rectified Date">
																<ItemStyle Wrap="False"></ItemStyle>
																<FooterStyle Wrap="False"></FooterStyle>
																<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="IsMELPart" SortExpression="IsMELPart" HeaderText="Is MEL">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
														</Columns>
													</asp:GridView>
												</table>
											</asp:PlaceHolder>
										</ContentTemplate>
									</asp:UpdatePanel>

								</td>
							</tr>
							<%--Added by Harsh on 14th May 2024 for FLYPAL 1632 -- W.O Job List page changes--%>
							<tr>
								<td valign="top" align="right" colspan="4">

									<asp:UpdatePanel ID="upnlDiscrepancy" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:PlaceHolder ID="phDiscrepancyList" runat="server" Visible="false">
												<asp:GridView ID="dgDiscrepancyJobs" runat="server" DataKeyNames="ID" ToolTip="Discrepancy Jobs"
													ShowHeaderWhenEmpty="True" EnableViewState="False" AllowSorting="True"
													AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal"
													CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
														ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<Columns>
														<%--0--%>
														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<%--1--%>
														<asp:TemplateField HeaderText="Select">
															<HeaderTemplate>
																<input type="checkbox" id="chkSelectAllDiscrepancies" />
															</HeaderTemplate>
															<ItemTemplate>
																<input type="checkbox" name="chkSelectDiscrepancies"
																	class="cbSelectRow" value="<%# Eval("ID") %>"
																	<%# CheckBoxSelection(Eval("ID").ToString()) %>>
																</input>
															</ItemTemplate>
														</asp:TemplateField>
														<%--2--%>
														<asp:BoundField DataField="DefectNo" HeaderText="Discrepancy No">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
														</asp:BoundField>
														<%--3--%>
														<asp:BoundField DataField="DateOfOccurrenceFormatted" HeaderText="Occurrence Date">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--4--%>
														<asp:BoundField DataField="LogNo" HeaderText="Log No">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--5--%>
														<asp:BoundField DataField="Defect" HeaderText="Discrepancy Detail">
															<HeaderStyle Width="500px" HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Width="500px" Wrap="True"></ItemStyle>
														</asp:BoundField>
														<%--6--%>
														<asp:BoundField DataField="MELorCDL" HeaderText="Type">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--7--%>
														<asp:BoundField DataField="MELCategoryName" HeaderText="Category">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--8--%>
														<asp:BoundField DataField="SubATACodeDisplay" HeaderText="ATA">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--9--%>
														<asp:BoundField DataField="Frequency" HeaderText="Frequency" HtmlEncode="false">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<%--10--%>
														<asp:BoundField DataField="MELOrDeviationDescription" HeaderText="Description">
															<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Width="200px" Wrap="True"></ItemStyle>
														</asp:BoundField>
														<%--11--%>
														<asp:BoundField DataField="DueAsOf" HeaderText="Due As Of" HtmlEncode="false">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="false"></ItemStyle>
														</asp:BoundField>
													</Columns>
												</asp:GridView>
											</asp:PlaceHolder>
										</ContentTemplate>
									</asp:UpdatePanel>

								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>

		<!-- Ajax Loader -->
		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>

	</form>
</body>
</html>
