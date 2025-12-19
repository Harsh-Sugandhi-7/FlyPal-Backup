<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELDetail_Ajax.aspx.vb"
	Inherits="Flypal.wfMELDetail_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Minimum Equipment</title>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
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
					<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
						<table id="tblInner" class="clsTableListIn">
							<tr class="clsFormHeader1Newstyle">
								<td colspan="4">
									<table width="100%">
										<tr>
											<td>
												<span id="lblheader" class="clsFormHeader">
													Minimum Equipment Detail
												</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnSave" runat="server"
															ToolTip="Click to save Minimum Equipment information"
															Text="Save" CssClass="clsbtnH clsinfoH"></asp:Button>
														<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
															runat="server" CausesValidation="False"
															ToolTip="Click to Close Screen." Text="Back">
														</asp:Button>
														<asp:Button ID="hdnimgBtnATAChapter" runat="server"
															CausesValidation="false" ClientIDMode="Static"
															Style="display: none;" Text="Add" />
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
											<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvFrequency" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFrequencyInDay"
												Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvFreqHours" runat="server" CssClass="clsLabelAuto" ErrorMessage="Frequency In Hours Required."
												ControlToValidate="txtFrequencyInHours" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlMELDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<fieldset class="clsFieldSetNewStyle">
															<legend id="lbltitle" runat="server" style="font-weight: bold"><b>Minimum Equipment
                                                            Details</b></legend>
															<table>
																<tr>
																	<td>
																		<span id="lblPartNoStar1" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblModel" class="clsLabelAuto">Model</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbModel" runat="server" 
																			CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelName"
																			SelectedValue="<%# mMEL.ModelID %>" DataValueField="ID">
																		</asp:DropDownList>
																	</td>
																	<td>
																		<span id="Span1" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="Span2" class="clsLabel">Item Sequence No.</span>
																	</td>
																	<td>
																		<asp:TextBox runat="server" ID="txtItemSequenceNo" CssClass="clsTextBoxTagSearchSmall" Width="100px"
																			Text="<%# mMEL.ItemNo %>"></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td align="center">
																		<span id="lblStarATA" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
																	</td>
																	<td>
																		<table cellspacing="0" cellpadding="0">
																			<tr>
																				<td>
																					<asp:UpdatePanel ID="upnlATAMaster" runat="server" UpdateMode="Conditional">
																						<ContentTemplate>
																							<table>
																								<tr>
																									<td>
																										<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																											AutoPostBack="true" SelectedValue="<%# mMEL.ATAID %>" DataTextField="ATAChapter"
																											DataValueField="ID">
																										</asp:DropDownList>
																									</td>
																									<td>
																										<asp:ImageButton ID="imgbtnATAChapter" runat="server" ImageUrl="~/images/plus1.png"
																											Height="22px" Width="24px" ToolTip="Click to add new ATA chapter." 
																											CausesValidation="False"></asp:ImageButton>
																									</td>
																								</tr>
																							</table>
																						</ContentTemplate>
																					</asp:UpdatePanel>
																				</td>
																			</tr>
																		</table>
																	</td>
																	<td>
																		<span id="Span3" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<asp:Label ID="lblSubATAChapter" runat="server" CssClass="clsLabel">Sub-ATA Chapter</asp:Label>
																	</td>
																	<td>
																		<asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mMEL.SubATAID %>"
																					DataValueField="ID" DataTextField="SubATAChapter">
																				</asp:DropDownList>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="Span7" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblDescription" class="clsLabel">Description</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
																			Text="<%# mMEL.MELDescription %>" TextMode="MultiLine" ToolTip=" Description"></asp:TextBox>
																	</td>
																	<td>
																		<span id="Span9" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="Span6" class="clsLabel">Page No.</span>
																	</td>
																	<td>
																		<asp:TextBox runat="server" ID="txtPageNo" CssClass="clsTextBoxTagSearchSmall" Width="100px"
																			Text="<%# mMEL.PageNo %>"></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="Span8" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="Span4" class="clsLabel">Issue No. / Rev. No.</span>
																	</td>
																	<td>
																		<asp:TextBox runat="server" ID="txtRevNo" CssClass="clsTextBoxTagSearchSmall" Text="<%# mMEL.RevisionNo %>"></asp:TextBox>
																	</td>
																	<td>
																		<span id="Span10" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="Span5" class="clsLabel">Rev. Date</span>
																	</td>
																	<td>
																		<asp:UpdatePanel ID="upnlRevisionDate" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<asp:TextBox ID="txtRevisionDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchDate"
																					onchange="ValidateDateText(this,'txtRevisionDate_CalendarExtender');" Width="100px"></asp:TextBox>
																				<cc2:CalendarExtender ID="txtRevisionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																					Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevisionDate"></cc2:CalendarExtender>
																				<cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtRevisionDate"
																					WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblMakeMELQty" class="clsLabelAuto">Number Installed</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtMakeMELQty" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchSmall"
																			MaxLength="50" Text="<%# mMEL.MakeMELQty %>" ToolTip="Enter Manufacturer Qty."></asp:TextBox>
																	</td>
																	<td></td>
																	<td>
																		<span id="lblFlyMELQty" class="clsLabelAuto">No Req. to Dispatch</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtFlyMELQty" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchSmall"
																			MaxLength="50" Text="<%# mMEL.FlyMELQty %>" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Enter Dispatch ADD Qty.", "Enter Dispatch MEL Qty.") %>'></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td align="right">
																		<span id="Label4" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblMELCategory" class="clsLabelAuto">Rectification Interval</span>
																	</td>
																	<td>
																		<asp:UpdatePanel ID="upnlCategory" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<asp:DropDownList ID="cmbMELCategory" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle"
																					SelectedValue="<%# mMEL.MELCategoryID %>" DataTextField="Name" DataValueField="ID">
																				</asp:DropDownList>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td colspan="3" valign="top">
																		<asp:UpdatePanel ID="upnlFreq" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<fieldset class="clsFieldSetNewStyle">
																					<legend id="Legend2" runat="server" style="font-weight: bold">
																						<table>
																							<tr>
																								<td>
																									<span class="clsLabelHeader">Frequency</span>
																								</td>
																								<td>
																									<asp:CheckBox ID="chkIsInHours" runat="server" AutoPostBack="True" Checked="<%# mMEL.IsHours %>"
																										CssClass="clsCheckBox" Enabled="False" Text="(Select if Frequency is in Hours e.g. 11:59)" />
																								</td>
																							</tr>
																						</table>
																					</legend>
																					<table>
																						<tr>
																							<td>
																								<table>
																									<tr>
																										<td>
																											<asp:TextBox ID="txtFrequencyInDay" runat="server" ClientIDMode="Static" 
																												CssClass="clsTextBoxTagSearchSmall" Enabled="False" MaxLength="4" 
																												Text="<%# mMEL.FrequencyInDays %>" ToolTip="Enter Frequency In Days">
																											</asp:TextBox>
																										</td>
																										<td>
																											<span id="lblDays" class="clsLabel">Days</span>
																										</td>
																										<td>
																											<asp:TextBox ID="txtFrequencyInHours" runat="server" ClientIDMode="Static" 
																												CssClass="clsTextBoxTagSearchSmall" Enabled="False" MaxLength="5" 
																												Text="<%# mMEL.FrequencyInHours %>" ToolTip="Enter Frequency In Hours">
																											</asp:TextBox>
																											<span id="lblHours" class="clsLabel">Hours</span>
																										</td>
																										<td>
																											<asp:TextBox ID="txtFrequencyInCycles" runat="server" ClientIDMode="Static" 
																												CssClass="clsTextBoxTagSearchSmall" MaxLength="5" 
																												Text="<%# mMEL.FrequencyInCycles %>" ToolTip="Enter Frequency In Cycles" >
																											</asp:TextBox>
																											<span id="lblCycles" class="clsLabel">Cycles</span>
																										</td>
																									</tr>
																								</table>
																							</td>
																						</tr>
																					</table>
																				</fieldset>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																	<td></td>
																	<td colspan="2" align="top">
																		<fieldset class="clsFieldSetNewStyle">
																			<legend id="Legend1" runat="server" style="font-weight: bold"><b>Applicability</b>
																				<asp:CheckBox ID="chkIsApplicable" CssClass="clsCheckBox" runat="server" Checked="<%# mMEL.IsApplicable %>"
																					AutoPostBack="true" /></legend>
																			<table>
																				<tr>
																					<td>
																						<asp:UpdatePanel ID="upnlApplicability" runat="server" UpdateMode="Conditional">
																							<ContentTemplate>
																								<asp:TextBox ID="txtApplicabilityNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
																									Text="<%# mMEL.NotApplicableNote %>" Enabled="false" MaxLength="500" TextMode="MultiLine"
																									ToolTip="Enter Applicability Note"></asp:TextBox>
																							</ContentTemplate>
																						</asp:UpdatePanel>
																					</td>
																				</tr>
																			</table>
																		</fieldset>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblRemark" class="clsLabelAuto">Remark</span>
																	</td>
																	<td colspan="5">
																		<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" 
																			MaxLength="500" Width="98%" Text="<%# mMEL.Remark %>" 
																			TextMode="MultiLine" ToolTip="Enter Remark">
																		</asp:TextBox>
																	</td>
																</tr>
															</table>
														</fieldset>
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
				<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000; height: 58px; width: 58px;">
					<div class="ext-el-mask-msg x-mask-loading">
						<div class="clsLoad_ajax">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
								Height="48px" Width="48px" />
						</div>
					</div>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
		<!-- ATA Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyATA" Text="Dummy ATA" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupATA" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupATA" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupATA" runat="server" TargetControlID="btnDummyATA"
			PopupControlID="pnlPopupATA" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameATAStateComplete() {
				$("#btnDummyATA").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}
			function OpenATAWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#iPopupATA").attr("src", "wfATA_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyATA").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunction() {
				var atawindow = $find("<%=mdlPopupATA.ClientID %>");
				//close ata popup window
				atawindow.hide();
				$("#iPopupATA").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnATAChapter").click();
			}
		</script>
		<script type="text/javascript">
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'false' };
				$.ajax({
					type: "POST",
					url: "DateValidationHandler.ashx",
					cache: false,
					data: params,
					async: false,
					beforeSend: OnBeforeSend,
					success: onSuccess,
					error: onError
				});

				function onSuccess(result) {
					$(elem).removeClass('ac_loading');
					$(elem).val(result);
					$find(extenderid).set_Text(result);
				}

				function onError(result) {
					$(elem).removeClass('ac_loading');
					$(elem).val('');
					$find(extenderid).set_Text('');
				}
				function OnBeforeSend() {
					$(elem).addClass('ac_loading');
				}
			}

		</script>
		<!-- End-->
		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForMELDetail();
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
					parent.IFrameMELDetailStateComplete();
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
     <% Dim IsOpenFrom As String = Request.QueryString("OpenFrom") %>
     <% If Not IsOpenFrom Is Nothing AndAlso IsOpenFrom = "Snag" %>
			$(document).ready(function () {
				$(':input').not('#btnClose').attr('disabled', true);
			});
     <% End If %>

		</script>
	</form>
</body>
</html>
