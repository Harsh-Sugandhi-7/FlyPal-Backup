<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceDoneByEmployee_Ajax.aspx.vb"
	Inherits="Flypal.wfMaintenanceDoneByEmployee_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Maintenance Done By Employee</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link rel="stylesheet" type="text/css" href="popup.css" />
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AlertMessage1.1.js"></script>
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script type="text/javascript" src="AutoComplete\autocomplete-setup.js"></script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table class="clstablelistin" id="tblLedgerList" width="100%">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lblPartList" class="clsFormHeader">Maintenance By</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" runat="server"
																		CssClass="clsbtnH clsinfoH" ToolTip="Add Maintenance By"
																		Text="Add" />
																</td>
																<td>
																	<asp:Button ID="btnClose" runat="server"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Go back to the previous Page."
																		CausesValidation="false" Text="Back" />
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
								<td>
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields" />
											<asp:RequiredFieldValidator ID="rqLicenceNo" runat="server" ControlToValidate="txtLicenceNo"
												CssClass="clsLabelAuto" ErrorMessage="License No. Required" />
											<asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Enter correct License No."
												Display="None" ControlToValidate="txtLicenceNo" OnServerValidate="customvalidate" />
											<asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Enter correct License No" Display="None" ControlToValidate="txtRequiredManHours"
												OnServerValidate="customvalidate" />
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table3" width="100%">
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<span class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="Span1" class="clsLabelAuto">License No.</span>
																</td>
																<td>
																	<asp:TextBox ID="txtLicenceNo" runat="server"
																		CssClass="clsTextBoxTagSearch"
																		ToolTip="Enter License No."
																		MaxLength="200" Width="250px" />
																</td>
															</tr>
															<tr>
																<td></td>
																<td>
																	<asp:Label ID="lblRequiredmanHours" runat="server" CssClass="clsLabelAuto"
																		Visible='<%# (Session("mMaintTypeID") <> "2" And
                                                                                        Session("mMaintTypeID") <> "1" And
                                                                                        Session("mMaintTypeID") <> "3" And
                                                                                        Session("mMaintTypeID") <> "4" And
                                                                                        Session("mMaintTypeID") <> "11" And
                                                                                        Session("mMaintTypeID") <> "12") %>'
																		Text="Actual Man Hours" />
																</td>
																<td>
																	<asp:TextBox ID="txtRequiredManHours" runat="server"
																		CssClass="clsTextBoxTagSearchSmall"
																		Visible='<%# (Session("mMaintTypeID") <> "2" And
                                                                                        Session("mMaintTypeID") <> "1" And
                                                                                        Session("mMaintTypeID") <> "3" And
                                                                                        Session("mMaintTypeID") <> "4" And
                                                                                        Session("mMaintTypeID") <> "11" And
                                                                                        Session("mMaintTypeID") <> "12") %>'
																		ToolTip="Enter Actual Man Hours" MaxLength="8" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"
															Text="List of License Nos. : 0 Record(s) found." />
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:GridView ID="dgMaintDoneByList" runat="server" CssClass="clsGridNewStyle"
															ShowHeaderWhenEmpty="true" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
															DataKeyNames="ID" AutoGenerateColumns="False" AllowSorting="True">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
																ForeColor="black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle HorizontalAlign="Right" CssClass="paging" />
															<Columns>
																<%--0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																<%--1--%>
																<asp:BoundField DataField="LicenceNo" SortExpression="LicenceNo"
																	HeaderText="License No">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--2--%>
																<asp:BoundField DataField="EmployeeName" SortExpression="EmployeeName"
																	HeaderText="Employee Name">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="RequiredManHours" HeaderText="Actual Man Hours">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--4--%>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
																	ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png"
																				runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="EditView" runat="server"
																								CommandArgument='<%# Eval("ID") %>'
																								CommandName="EditRec" CssClass="actionICNS"
																								ToolTip="Edit this record."
																								ImageUrl="~/images/edit.png" CausesValidation="false" />
																						</td>
																						<td>
																							<asp:ImageButton ID="DeleteRec" runat="server"
																								CommandArgument='<%# Eval("ID") %>'
																								ToolTip="Delete this record."
																								CommandName="DeleteRec"
																								CssClass="actionICNS  largerActionICNS"
																								ImageUrl="~/images/delete.png"
																								CausesValidation="false" />
																						</td>

																					</tr>

																				</table>
																			</div>
																		</div>
																	</ItemTemplate>
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																</asp:TemplateField>
															</Columns>
														</asp:GridView>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>

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

		<div id="Scripts">

			<%--call parent function after completing subroutine..(when page open as popup)--%>
			<script type="text/javascript">
				function CallParentCallback() {
					parent.ParentCallBackFunctionForMaintDoneBy();
					return false;
				}
			</script>
			<%--End--%>

			<%--Set page layout when open as popup aspx page--%>
			<script type="text/javascript">

            <% Dim mopen As String = Request.QueryString("Type") %>
            <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
				$(document).ready(function () {
					SetPageLayout();
					if ($.browser.msie) {
						parent.IFrameMaintDoneByStateComplete();
					}
				});
            <% End if %>

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

				function endRequestHandler() {
					SetPageLayout();
				}

				function SetPageLayout() {

					<% Dim mopenas As String = Request.QueryString("Type") %>

					<% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
					ReSetPageLayout();
					onResize();//for Top bottom link
					<% End if %>

				}

				function ReSetPageLayout() {

					$("body,html").css({ 'background-color': 'transparent' });
					var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
					var windowheight = $(window).height();
					if (tempMargtop >= windowheight) {
						$("body #tblmain:eq(0)").css({ 'margin': 'auto' });
					}
					else {
						var margintop = (windowheight / 2) - (tempMargtop / 2);
						$("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
					}

				}
			</script>
			<%--End--%>

			<script type="text/javascript">

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

					$("#<%=txtLicenceNo.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx', {
						width: 250,
						autoFill: false,
						matchContains: true,
						max: 20,
						delay: 0
					});

				});

			</script>

		</div>

	</form>

</body>
</html>
