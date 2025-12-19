<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDeferredListForSelection_Ajax.aspx.vb"
	Inherits="Flypal.wfDeferredListForSelection_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<title>Deferred List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" id="clientEventHandlersJS">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
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
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lblDeviationList" class="clsFormHeader">Deferred List</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlTopActionButton" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnTopBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to back"
																		Text="Back"></asp:Button>
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
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table2">
															<tr>
																<td>
																	<span id="lblDescription" class="clsLabelAuto">Description </span>
																</td>
																<td>
																	<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description."
																		AutoPostBack="True"></asp:TextBox>
																</td>
																<td>
																	<span id="lblRectificationInterval" class="clsLabelAuto">Category</span>
																</td>
																<td>
																	<asp:UpdatePanel ID="upnlRectificationInterval" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:DropDownList ID="cmbDeviationCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																				DataValueField="ID" DataTextField="Name" AutoPostBack="true">
																			</asp:DropDownList>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
															</tr>
															<tr>
																<td>
																	<span id="lblATA" class="clsLabel">ATA</span>
																</td>
																<td>
																	<asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																				DataTextField="ATAChapter" AutoPostBack="True">
																			</asp:DropDownList>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td>
																	<span id="lblSubATA" class="clsLabel">Sub ATA</span>
																</td>
																<td>
																	<asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																				AutoPostBack="true" DataTextField="SubATAChapter">
																			</asp:DropDownList>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td></td>
																<td></td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to find list"
															Text="Find Now" Visible="False"></asp:Button>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>

							<tr>
								<td>
									<asp:UpdatePanel ID="upnlDeviationLists" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1" width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgDeviationLists" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True"
															AllowPaging="true" PageSize="25" DataKeyNames="ID" AutoGenerateColumns="False"
															AllowSorting="True">
															<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
															<RowStyle CssClass="clsdgItem"></RowStyle>
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
															<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
															<PagerStyle HorizontalAlign="Right" CssClass="paging" />
															<Columns>
																<%--1--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>

																<%--2--%>
																<asp:BoundField DataField="ATACodeSubATACode" SortExpression="ATACodeSubATACode" HeaderText="ATA">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false"></ItemStyle>
																</asp:BoundField>

																<%--3--%>
																<asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="Item Sequence No.">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left" Width="50px"></HeaderStyle>
																	<ItemStyle Wrap="false" Width="50px" Font-Bold="true"></ItemStyle>
																</asp:BoundField>

																<%--4--%>
																<asp:BoundField DataField="PageNo" SortExpression="PageNo" HeaderText="Page No.">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false"></ItemStyle>
																</asp:BoundField>

																<%--5--%>
																<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="true"></ItemStyle>
																</asp:BoundField>

																<%--6--%>
																<asp:BoundField DataField="RevisionNo" SortExpression="RevisionNo" HeaderText="Revision No.">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false"></ItemStyle>
																</asp:BoundField>

																<%--7--%>
																<asp:BoundField DataField="RevisionDateFormatted" HeaderText="Revision Date">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false"></ItemStyle>
																</asp:BoundField>

																<%--8--%>
																<asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--9--%>
																<asp:BoundField DataField="DeviationCategoryName" SortExpression="DeviationCategoryName" HeaderText="Category">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false"></ItemStyle>
																</asp:BoundField>

																<%--10--%>
																<asp:BoundField DataField="QtyInstalled" HeaderText="Qty. Installed">
																	<HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>

																<%--11--%>
																<asp:BoundField DataField="HoursLimit" HeaderText="Hours Limit" HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn">
																	<HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>

																<%--12--%>
																<asp:BoundField DataField="CyclesLimit" HeaderText="Cycles Limit" HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn">
																	<HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>

																<%--13--%>
																<asp:BoundField DataField="DaysLimit" HeaderText="Days Limit" HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn">
																	<HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>

																<%--14--%>
																<asp:ButtonField CommandName="SelectRecord" HeaderText="Select" Text="Select">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																	<ItemStyle Wrap="False" />
																</asp:ButtonField>
															</Columns>
														</asp:GridView>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<asp:Button ID="btnBottomAdd" runat="server" CssClass="clsbtnH clsinfoH"
															Text="Add New" Visible="false"></asp:Button>
													</td>
													<td>
														<asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
															Text="Close" Visible="false"></asp:Button>
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

		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
			runat="server">
			<ProgressTemplate>
				<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
				</div>
				<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
					<div class="ext-el-mask-msg x-mask-loading">
						<div class="clsLoad_ajax">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
								Height="48px" Width="48px" />
						</div>
					</div>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>

		<%-- Autocomplete functions to set id--%>
		<asp:HiddenField ID="hdnModelId" runat="server" ClientIDMode="Static" />

		<script type="text/javascript">

			function SetModelID(source, e) {
				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}
				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtModel_Autocomplete") {
					textbox = document.getElementById('hdnModelId');
				}
				textbox.value = value;
			}
			//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
			function SetModelIdonChange(source, extenderid) {
				var popup = $find(extenderid);
				var complist = popup.get_completionList();
				var text = $(source).val().toLowerCase();
				for (var i = 0; i < complist.childNodes.length; i++) {
					var texttocompare = complist.childNodes[i].innerText.toLowerCase();
					if (text == texttocompare) {
						var val = complist.childNodes[i]._value;

						if (extenderid == "txtModel_Autocomplete") {
							textbox = document.getElementById('hdnModelId');
						}
						textbox.value = val;
						return;
					}

				}

				if (extenderid == "txtModel_Autocomplete") {
					document.getElementById('hdnModelId').value = '';
				}
			}
		</script>

		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackCDLFunction();
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
						parent.IFrameMELMasterStateComplete();
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
				var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
				var windowheight = $(window).height();
				if (tempMargtop >= windowheight) {
					$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
				}
				else {
					var margintop = (windowheight / 2) - (tempMargtop / 2);
					$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
				}

			}
		</script>
		<%--End--%>
	</form>
</body>
</html>
