<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobDetail.aspx.vb"
	Inherits="Flypal.wfnWOJobDetail" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--AJAX- Changed DOCTYPE from 4.0 to 1.0--%><%--AJAX- Register "AjaxControlToolkit & User Control "MSGBOX"--%>
<html>
<head runat="server">
	<title>W.O. JOB Details</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />	
	<link id="MainStyle" rel="stylesheet" type="text/css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>

	<script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
	<script language="JavaScript" type="text/javascript">

        function autoWOJobNRCList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeWOJobNRCList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeWOJobNRCList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeWOJobNRCList').height = (newheight + 2) + "px";
            document.getElementById('IframeWOJobNRCList').width = (newwidth + 10) + "px";
            document.getElementById('tabWOJobNRC').height = (newheight) + "px";
            document.getElementById('tabWOJobNRC').width = (newwidth) + "px";

            document.getElementById('WOJobDetailsTabContainer').height = (newheight) + "px";
            document.getElementById('WOJobDetailsTabContainer').width = (newwidth) + "px";
        }

        function autoWOJobTaskList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeWOJobTaskList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeWOJobTaskList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeWOJobTaskList').height = (newheight + 2) + "px";
            document.getElementById('IframeWOJobTaskList').width = (newwidth + 10) + "px";
            document.getElementById('tabWOJobTask').height = (newheight) + "px";
            document.getElementById('tabWOJobTask').width = (newwidth) + "px";

            document.getElementById('WOJobDetailsTabContainer').height = (newheight) + "px";
            document.getElementById('WOJobDetailsTabContainer').width = (newwidth) + "px";
        }

        function autoWOJobDesignationAllocationList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeWOJobDesignationAllocationList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeWOJobDesignationAllocationList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeWOJobDesignationAllocationList').height = (newheight + 30) + "px";
            document.getElementById('IframeWOJobDesignationAllocationList').width = (newwidth) + "px";
            document.getElementById('tabWOJobDesignationAllocation').height = (newheight) + "px";
            document.getElementById('tabWOJobDesignationAllocation').width = (newwidth) + "px";

            document.getElementById('WOJobDetailsTabContainer').height = (newheight) + "px";
            document.getElementById('WOJobDetailsTabContainer').width = (newwidth) + "px";
            //            $("#refreshTabs").click();
        }

        function autoWOJobCompsList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeWOJobCompsList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeWOJobCompsList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeWOJobCompsList').height = (newheight + 10) + "px";
            document.getElementById('IframeWOJobCompsList').width = (newwidth + 5) + "px";
            document.getElementById('tabWOJobComps').height = (newheight) + "px";
            document.getElementById('tabWOJobComps').width = (newwidth) + "px";

            document.getElementById('WOJobDetailsTabContainer').height = (newheight) + "px";
            document.getElementById('WOJobDetailsTabContainer').width = (newwidth) + "px";
        }

        function autoWOJobSparesList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeWOJobSparesList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeWOJobSparesList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeWOJobSparesList').height = (newheight + 40) + "px";
            document.getElementById('IframeWOJobSparesList').width = (newwidth) + "px";
            document.getElementById('tabWOJobSpares').height = (newheight) + "px";
            document.getElementById('tabWOJobSpares').width = (newwidth) + "px";

            document.getElementById('WOJobDetailsTabContainer').height = (newheight) + "px";
            document.getElementById('WOJobDetailsTabContainer').width = (newwidth) + "px";
        }

    </script>
</head>
<body>
	<form id="form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<script language="javascript" type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            $(document).ready(function () {
                function endRequestHandler() {

                    try {

                        //if (g_isTabPressed == 1) {
                        $get(g_CurrentTextBox).focus();
                        $get(g_CurrentTextBox).select();

                        g_isTabPressed = 0;
                        //}


                    }
                    catch (Error) { }

                }

            });
        </script>
		<script language="javascript" type="text/javascript">
            $(document).ready(function () {
                function onTextFocus() {
                    g_CurrentTextBox = event.srcElement.id;

                }
                function onkeyPressed(keycode, obj) {

                    if (keycode == 9) {

                        g_isTabPressed = 1;
                    }
                }
            });
        </script>
		<%--AJAX- ScriptManager Added--%>
		<table class="clstablelistout" id="OuterTable" cellspacing="1" cellpadding="1" border="0">
			<tr>
				<td>
					<table class="clstablelistin" id="InnerTable" border="0">
						<tr>
							<td>
								<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<cc2:TabContainer ID="WOJobDetailsTabContainer" runat="server" class="clstablelistin"
											AutoPostBack="true">
											<cc2:TabPanel ID="tabWoJobDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
												<HeaderTemplate>
													JOB
												</HeaderTemplate>
												<ContentTemplate>
													<table id="tblmain" class="clstablelistin">
														<tr>
															<td class="clsFormHeader1Newstyle">
																<table width="100%">
																	<tr>
																		<td>
																			<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> W.O. Job Detail</asp:Label>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td align="right">
																			<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table id="Table11" cellspacing="0">
																						<tr>
																							<td>
																								<asp:Button ID="btnReject" runat="server" CssClass="clsbtnH clsinfoH" Text="Reject"
																									ToolTip="Reject" Visible='<%# IIf(AppSettings("ShowNewWOFlow") = "True", True, False) And
																																		Not mnWO.IsNew And mnWO.StatusID > 1 And
																																		Not (mnWO.WOJobs.CurrentItem.WOJobStatusID = 2) And
																																		Session("MiddleFrame") = "wfnWOExecutionList.aspx"  %>'></asp:Button>
																							</td>
																							<td>
																								<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" Enabled="<%# mnWO.WOStatusID <> 3 %>"
																									ToolTip="Save Job Detail"></asp:Button>
																							</td>
																							<td>

																								<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" ToolTip="Print Job Detail Report"
																									Enabled="<%# Not mnWO.WOJobs.CurrentItem.IsNew %>" CausesValidation="False"></asp:Button>
																							</td>
																							<td>
																								<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Go back to Previous screen"
																									CausesValidation="False"></asp:Button>
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
																		<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"
																			runat="server"></asp:ValidationSummary>
																		<asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clsValidationSummary">
																		</asp:CustomValidator>
																		<asp:RequiredFieldValidator ID="rfvWOJobDescription" runat="server" Display="None"
																			CssClass="clsLabelAuto" ErrorMessage="Job Description required." ControlToValidate="txtWOJobDescription">
																		</asp:RequiredFieldValidator>
																		<asp:CustomValidator ID="cvStartDate" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtStartDate" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvEndDate" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtEndDate" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvWOStatusList" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="cmbWOStatusList" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvWOJobDescription" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtWOJobDescription" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvWOJobAction" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtWOJobAction" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvDateOfOccurrence" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtDateOfOccurrence" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvEstimatedTime" runat="server" Display="None" CssClass="clsLabelAuto"
																			ControlToValidate="txtEstimatedTime" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvTaskSourceRef" runat="server" CssClass="clsLabelAuto"
																			Display="None" ControlToValidate="txtTaskSourceRef" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvDueAsOf" runat="server" CssClass="clsLabelAuto" Display="None"
																			ControlToValidate="txtDueAsOf" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvOtherJobSpecification" runat="server" CssClass="clsLabelAuto"
																			Display="None" ControlToValidate="txtOtherJobSpecification" OnServerValidate="CustomVailidity">
																		</asp:CustomValidator>
																		<asp:CustomValidator ID="cvCurrentValue" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtTaskNo"
																			Display="None"></asp:CustomValidator>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlWOJobDetails" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle" id="fdswodetail1" runat="server">
																			<legend id="ldwodetail1" class="clsFieldSet1" runat="server"><b>Job Details</b></legend>
																			<table valign="top" width="100%">
																				<tr>
																					<td align="right" colspan="5">
																						<span id="lblJob" class="clsLabelHeader">Job # </span>
																						<asp:Label ID="lblJobLebel" runat="server" CssClass="clsLabelHeader" Text="<%# mnWO.WOJobs.CurrentItem.SrNo %>"></asp:Label>
																					</td>
																				</tr>
																				<tr>
																					<asp:PlaceHolder runat="server" ID="phServiceProvider" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", True, False) %>'>
																						<td>
																							<asp:Label ID="Label8" runat="server" CssClass="clsLabelStar" 
																								Visible='<%#IIf(AppSettings("ShowAMOOnlyForNewClients") = "True" And (mnWO.WOJobs.CurrentItem.WOJobTypeID <> 1), True, False) %>'>*</asp:Label>
																						</td>
																						<td>
																							<span id="lblTaskNo" class="clsLabelauto">Task No.</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtTaskNo" runat="server" CssClass="clsTextBoxTagSearch" 
																								Text="<%# mnWO.WOJobs.CurrentItem.TaskCardNo %>" 
																								ReadOnly='<%#IIf(AppSettings("ShowAMOOnlyForNewClients") = "True" Or mnWO.WOJobs.CurrentItem.WOJobTypeID <> 2, False, True) %>'></asp:TextBox>
																						</td>

																					</asp:PlaceHolder>
																					<asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", False, True) %>'>
																						<td></td>
																						<td>
																							<span id="lblInspectionCode" class="clsLabelauto">Inspection Code</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtInspCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.InspCode %>"></asp:TextBox>
																						</td>
																					</asp:PlaceHolder>
																					<td>
																						<asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
																					</td>
																					<td>
																						<asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
																							<ContentTemplate>
																								<asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
																									DataTextField="ATAChapter" SelectedValue="<%# mnWO.WOJobs.CurrentItem.ATAChapterID %>">
																								</asp:DropDownList>
																							</ContentTemplate>
																						</asp:UpdatePanel>
																					</td>
																				</tr>
																				<tr>
																					<td>
																						<asp:Label ID="lblStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
																					</td>
																					<td>
																						<span id="lblDescription" runat="server" class="clsLabelauto">Description</span>
																					</td>
																					<td colspan="3">
																						<asp:TextBox ID="txtWOJobDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle1"
																							Text="<%# mnWO.WOJobs.CurrentItem.WOJobDescription %>" Enabled="<%# mnWO.WOStatusID <> 3 %>"
																							MaxLength="500" ToolTip="Enter Description" TextMode="MultiLine" Width="530px"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>&nbsp;
																					</td>
																					<td>
																						<span id="lblSkill" class="clsLabelauto">Skill</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtSkill" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.Skill %>" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", False, True) %>'></asp:TextBox>
																						<asp:DropDownList ID="cmbSkillcode" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", True, False) %>'
																							SelectedValue="<%# mnWO.WOJobs.CurrentItem.SkillID %>" DataTextField="CodeType"
																							DataValueField="Id" AutoPostBack="false" Width="150px" />
																					</td>
																					<td>
																						<span id="lblEstimatedTime" class="clsLabelauto">Man Hours</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtEstimatedTime" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
																							Enabled="<%# mnWO.WOStatusID <> 3 %>" MaxLength="5" Text="<%# mnWO.WOJobs.CurrentItem.WOJobEstimatedTime %>"
																							ToolTip="Enter Estimated Time"></asp:TextBox>
																						<span id="lblEstimated" class="clsLabelauto">(Estimated)</span>
																					</td>
																				</tr>
																				<tr>
																					<td>&nbsp;
																					</td>
																					<td>
																						<span id="lblPublication" class="clsLabelauto"><%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", "Reference Doc.", "Publication") %></span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPublication" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mnWO.WOJobs.CurrentItem.Publication %>"></asp:TextBox>
																					</td>
																					<td>
																						<span id="lblTaskSourceRef" class="clsLabelauto">Task Source Ref. &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtTaskSourceRef" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
																							Text="<%# mnWO.WOJobs.CurrentItem.TaskSourceRef %>" ToolTip="Enter Task Source Ref."
																							MaxLength="500" Enabled="<%# mnWO.WOStatusID <> 3 %>" TextMode="MultiLine"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td></td>
																					<td>
																						<span id="lblZone" class="clsLabelauto">Zone</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtZone" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.Zone %>"></asp:TextBox>
																					</td>
																					<td>
																						<span id="lblArea" class="clsLabelauto">Area</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtArea" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.AREA %>"></asp:TextBox>
																					</td>
																				</tr>
																				<tr>
																					<td>&nbsp;
																					</td>

																					<td>
																						<span id="lblPanels" class="clsLabelauto"><%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", "Access", "Panels") %> </span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPanels" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.Panels %>"></asp:TextBox>
																					</td>
																					<td>
																						<span id="lblWorkPackRef" class="clsLabelauto">Work Pack Ref.</span>
																					</td>
																					<td>
																						<asp:TextBox ID="txtWorkPackRef" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mnWO.WOJobs.CurrentItem.WorkPACKRef %>"></asp:TextBox>
																					</td>
																				</tr>
																				<asp:PlaceHolder ID="phbilling" runat="server" Visible="false">
																					<tr>

																						<td>&nbsp;
																						</td>
																						<td>
																							<span id="lblIsForBilling" class="clsLabelauto">Is For Billing</span>
																						</td>
																						<td>
																							<asp:CheckBox ID="chkIsForBilling" runat="server" Checked="<%# mnWO.WOJobs.CurrentItem.IsForBilling %>"
																								CssClass="clsLabelauto" Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Check if this is for Billing" />
																						</td>
																					</tr>
																				</asp:PlaceHolder>
																				<asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", False, True) %>'>
																					<tr>
																						<td>&nbsp;
																						</td>
																						<td>
																							<span id="lblRevNo" class="clsLabelauto">Rev No.</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtAMPRevNo" CssClass="clsTextBoxTagSearch" runat="server" MaxLength="50"
																								Text="<%# mnWO.WOJobs.CurrentItem.AMPRevNo %>" ToolTip="Enter Rev No"></asp:TextBox>
																						</td>
																						<td>
																							<span id="RevDate" class="clsLabelauto">Rev Date</span>
																						</td>
																						<td>
																							<asp:TextBox ID="txtRevDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'CalendarExtender1');"></asp:TextBox>
																							<cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
																								Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRevDate"></cc2:CalendarExtender>
																							<cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtRevDate"
																								WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsTextBoxDate_Ajax" />
																						</td>
																					</tr>
																				</asp:PlaceHolder>
																				<tr>
																					<td>&nbsp;
																					</td>
																					<td>
																						<asp:Label ID="lblDueAsOf" runat="server" CssClass="clsLabelauto">Due As Of</asp:Label>
																					</td>
																					<td valign="top">
																						<asp:TextBox ID="txtDueAsOf" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mnWO.WOJobs.CurrentItem.DueAsOf %>"
																							TextMode="MultiLine"></asp:TextBox>
																					</td>
																					<td>
																						<span id="lblRII" class="clsLabelAuto">RII</span>
																					</td>
																					<td>
																						<asp:CheckBox ID="chkIsRII" runat="server" Checked="<%# mnWO.WOJobs.CurrentItem.IsRII %>"
																							CssClass="clsCheckBox" />
																						<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">(Check if Repeat / Independent Inspection Required)</asp:Label>
																					</td>
																				</tr>
																				<asp:PlaceHolder ID="phattach" runat="server" Visible="false">
																					<tr>
																						<td></td>
																						<td>
																							<span id="lblAttach" class="clsLabelauto">Attach File</span>
																						</td>
																						<td colspan="3">
																							<asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<table id="Table6">
																										<tr>
																											<td>
																												<input type="button" runat="server" id="btnSelectFile" value="Select File" style="width: 100px;"
																													class="clsbtnH clsinfoH" />
																											</td>
																											<td style="padding-left: 3px;">
																												<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
																													Height="25px" Width="25px"></asp:ImageButton>
																											</td>
																											<td>
																												<asp:ImageButton ID="imgDelAttach1" runat="server" CausesValidation="False" ImageUrl="images/remove.jpg"
																													Enabled="False" ToolTip="Remove Attachment" Height="20px" Width="20px"></asp:ImageButton>

																											</td>
																										</tr>

																									</table>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</td>

																					</tr>
																				</asp:PlaceHolder>
																				<!--Dummy panel to open modelpopup for FileUpload-->
																				<tr style="height: 0px;">
																					<td style="height: 0px;">
																						<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
																							<ContentTemplate>
																								<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnAddJobTaskDetail" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnAddSelectTasks" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnAddWOJobNRCDetail" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="refreshTabs" ClientIDMode="Static" runat="server" Text="----" CausesValidation="True"
																									Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnimgbtnDesignation" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnAddResourceAllocation" ClientIDMode="Static" runat="server"
																									Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnAddWODetail" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																								<asp:Button ID="hdnBtnCompletionDetail" ClientIDMode="Static" runat="server" Text="----"
																									CausesValidation="False" Style="display: none;"></asp:Button>
																							</ContentTemplate>
																						</asp:UpdatePanel>
																					</td>
																				</tr>
																				<tr>
																					<td colspan="5" valign="top">
																						<fieldset class="clsFieldSetNewStyle">
																							<legend class="clsFieldSet1"><b>File Attachments</b></legend>
																							<asp:UpdatePanel ID="upnlWOAttachment" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<table width="100%">
																										<tr>
																											<td style="height: 15px">
																												<asp:UpdatePanel ID="upnldgWOJobAttachment" runat="server" UpdateMode="Conditional">
																													<ContentTemplate>
																														<asp:GridView ID="dgWOJobAttachment" ToolTip="List of File Attachment(s)" runat="server"
																															CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
																															AllowSorting="True" GridLines="Horizontal" CellPadding="5" EnableViewState="true"
																															AllowPaging="False" AutoGenerateColumns="false" ClientIDMode="Static">
																															<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																															<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																															<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																															<Columns>
																																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																																<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																																<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																																	<HeaderStyle HorizontalAlign="Left" Width="10px"></HeaderStyle>
																																</asp:BoundField>
																																<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																																</asp:BoundField>
																																<asp:TemplateField HeaderText="File Name">
																																	<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																																	<ItemTemplate>
																																		<asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100" placeholder="Enter Filename"
																																			ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
																																			Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
																																	</ItemTemplate>
																																</asp:TemplateField>

																																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="
																																	" ItemStyle-HorizontalAlign="Center">
																																	<ItemTemplate>
																																		<%-- <span id="button">Login</span>--%>
																																		<div class="dropdown">
																																			<div class="dropdownbtn-content">
																																				<table id="T1" class="clsGridNew_Ajax">
																																					<tr>

																																						<td>
																																							<asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
																																								Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
																																						</td>
																																						<td>
																																							<asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
																																								CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
																																								CausesValidation="false" />
																																						</td>
																																					</tr>
																																				</table>
																																			</div>
																																			<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																																				Style="cursor: pointer" />
																																		</div>
																																	</ItemTemplate>
																																	<HeaderStyle HorizontalAlign="Center" />
																																	<ItemStyle HorizontalAlign="Center" />
																																</asp:TemplateField>
																															</Columns>
																														</asp:GridView>
																													</ContentTemplate>
																												</asp:UpdatePanel>
																											</td>
																											<td valign="top" style="width: 10px;">
																												<asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
																													Height="22px" Width="24px" ToolTip="Add New Attachment" CausesValidation="false"></asp:ImageButton>
																											</td>
																										</tr>
																									</table>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</fieldset>
																					</td>
																				</tr>

																				<asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%#IIf(mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 And AppSettings("ClientCode") = "STR", True, False) %>'>
																					<tr>
																						<td>&nbsp;
																						</td>
																						<td>
																							<span id="lblOtherJob" class="clsLabelauto" runat="server" visible='<%#IIf(mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 And AppSettings("ClientCode") = "STR", True, False) %>'>Other Job</span>
																						</td>
																						<td>
																							<asp:UpdatePanel ID="upnlOtherJob" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<asp:CheckBox ID="chkOtherJob" runat="server" Checked="<%# mnWO.WOJobs.CurrentItem.OtherJob %>"
																										AutoPostBack="true" CssClass="clsLabelauto" ToolTip="Check if Other Job" Visible='<%# iif(mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 And AppSettings("ClientCode") = "STR", True, False) %>' />
																									&nbsp;&nbsp;
                                                                                                    <asp:TextBox ID="txtOtherJobSpecification" runat="server" CssClass="clsTextBox" Height="25px"
																										Width="150px" Text="<%# mnWO.WOJobs.CurrentItem.OtherJobSpecification %>" ToolTip="Enter Other Job Specification"
																										MaxLength="50" Visible='<%# iif(mnWO.WOJobs.CurrentItem.WOJobTypeID = 1 And AppSettings("ClientCode") = "STR", True, False) %>'
																										Enabled="<%#  mnWO.WOJobs.CurrentItem.OtherJob %>" TextMode="MultiLine"></asp:TextBox>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</td>
																					</tr>
																				</asp:PlaceHolder>
																			</table>
																		</fieldset>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td valign="top">
																<asp:UpdatePanel ID="upnlJobCompletionDetails" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle" id="Fieldset3" runat="server">
																			<legend id="Legend3" class="clsFieldSet1" runat="server">
                                                                                <b>Completion Details</b>
																			</legend>
																			<table>
																				<tr>
																					<td></td>
																					<td colspan="7">
																						<asp:PlaceHolder ID="phCompletionDetailsGridView" runat="server"
																							Visible='<%# IIf(AppSettings("ShowMultipleWOJobActions").ToString.ToLower = "true",
																											True,
																											False)%>'>
																							<asp:UpdatePanel ID="upnlCompletionDetailsList" runat="server" UpdateMode="Conditional">
																								<ContentTemplate>
																									<table width="100%">
																										<tr>
																											<td>
																												<asp:GridView ID="gvCompletionDetails" runat="server" GridLines="Horizontal" CellPadding="5"
																													CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true" AllowSorting="True"
																													AllowPaging="False" AutoGenerateColumns="false" DataKeyNames="ID">
																													<AlternatingRowStyle CssClass="clsdgAltItem" />
																													<RowStyle CssClass="clsdgItem" />
																													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																													<Columns>
																														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																														<asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
																															<HeaderStyle Width="10px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="10px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:BoundField DataField="EmployeeName" HeaderText="Employee">
																															<HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="100px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:BoundField DataField="StartDateFormatted" HeaderText="Start Date">
																															<HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="100px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:BoundField DataField="CloseDateFormatted" HeaderText="End Date">
																															<HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="100px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:BoundField DataField="Action" HeaderText="Action Taken">
																															<HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="250px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:BoundField DataField="ActualTime" HeaderText="Man Hours (Actual)">
																															<HeaderStyle Width="70px" HorizontalAlign="Left"></HeaderStyle>
																															<ItemStyle Width="70px" HorizontalAlign="Left" />
																														</asp:BoundField>
																														<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																															<HeaderStyle HorizontalAlign="Center" />
																															<ItemStyle HorizontalAlign="Center" />
																															<ItemTemplate>
																																<div id="dropDownImg" class="dropdown">
																																	<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																																	<div id="dropdownICN-content" class="dropdownbtn-content">
																																		<table id="dropdown-content" class="clsGridNew_Ajax">
																																			<tr>
																																				<td>
																																					<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																																						CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																																						ToolTip="Edit record" CausesValidation="false"
																																						CommandName="EditRecord" ImageUrl="~/images/edit.png" />
																																				</td>

																																				<td>
																																					<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																																						CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																																						ToolTip="Delete record"
																																						CommandName="DeleteRecord" ImageUrl="~/images/delete.png" CausesValidation="false" />
																																				</td>
																																			</tr>
																																		</table>
																																	</div>
																																</div>
																															</ItemTemplate>
																														</asp:TemplateField>
																													</Columns>
																												</asp:GridView>
																											</td>
																											<td valign="top">
																												<asp:ImageButton ID="btnAddCompletionDetails" runat="server"
																													ImageUrl="~/images/plus1.png" CssClass="clsAddRecord" Height="22px" Width="24px"
																													ToolTip="Add New." CausesValidation="false" />
																											</td>
																										</tr>
																									</table>
																								</ContentTemplate>
																							</asp:UpdatePanel>
																						</asp:PlaceHolder>
																					</td>
                                                                                </tr>
                                                                                <asp:PlaceHolder ID="phDateAndActionControls" runat="server"
                                                                                    Visible='<%# IIf(AppSettings("ShowMultipleWOJobActions").ToString.ToLower = "true",
																									 False,
																									 True)%>'>
                                                                                    <tr id="dateControls">
                                                                                        <td>
                                                                                            <asp:Label ID="lblStarStartDate" runat="server" CssClass="clsLabelStar"
                                                                                                Visible="<%# mnWO.WOJobs.CurrentItem.WOJobStatusID = 2 %>"
                                                                                                Text="*" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblStartDate" class="clsLabelauto">Start Date</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtStartDate" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchDate" autocomplete="off"
                                                                                                            onchange="ValidateDateText(this,'txtStartDate_CalendarExtender');">
                                                                                                        </asp:TextBox>
                                                                                                        <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate" />
                                                                                                        <cc2:TextBoxWatermarkExtender ID="TBWEStartDate" runat="server" TargetControlID="txtStartDate"
                                                                                                            WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsTextBoxDate_Ajax" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtStartDateTime" runat="server"
                                                                                                            AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                                            MaxLength="10" ToolTip="Enter Time" Width="47px" />
                                                                                                        <cc2:MaskedEditExtender ID="txtStartDateTimeMaskedEditExtender"
                                                                                                            TargetControlID="txtStartDateTime" runat="server"
                                                                                                            AutoComplete="true" Mask="99:99" MaskType="Time"
                                                                                                            CultureName="en-us" MessageValidatorTip="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblStarEndDate" runat="server" CssClass="clsLabelStar"
                                                                                                Visible="<%# mnWO.WOJobs.CurrentItem.WOJobStatusID = 2 %>">*</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblEndDate" class="clsLabelauto">End Date </span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtEndDate" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchDate"
                                                                                                            onchange="ValidateDateText(this,'txtEndDate_CalendarExtender');"
                                                                                                            autocomplete="off" />
                                                                                                        <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server"
                                                                                                            CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                                                            TargetControlID="txtEndDate" />
                                                                                                        <cc2:TextBoxWatermarkExtender ID="TBWEEndDate" runat="server"
                                                                                                            TargetControlID="txtEndDate" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                                            WatermarkCssClass="clsTextBoxDate_Ajax" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtEndDateTime" runat="server" AutoPostBack="True"
                                                                                                            CssClass="clsTextBoxTagSearchSmall"
                                                                                                            MaxLength="10" ToolTip="Enter Time" Width="47px" />
                                                                                                        <cc2:MaskedEditExtender ID="txtEndDateTimeMaskedEditExtender"
                                                                                                            TargetControlID="txtEndDateTime" runat="server"
                                                                                                            AutoComplete="true" Mask="99:99" MaskType="Time"
                                                                                                            CultureName="en-us" MessageValidatorTip="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblActualTime" class="clsLabelauto">Man Hours</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtActualTime" runat="server"
                                                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                ReadOnly='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowAMOOnlyForNewClients") = "True", False, True) %>'
                                                                                                Text="<%# mnWO.WOJobs.CurrentItem.WOJobActualTime %>"
                                                                                                ToolTip="Actual Time"></asp:TextBox>
                                                                                            <span id="lblActual" class="clsLabelauto">(Actual)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="actionControls">
                                                                                        <td>
                                                                                            <asp:Label ID="lblStarAction" runat="server"
                                                                                                CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <span id="lblAction" class="clsLabelauto">Action</span>
                                                                                        </td>
                                                                                        <td colspan="7">
                                                                                            <asp:TextBox ID="txtWOJobAction" runat="server"
                                                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                                Text="<%# mnWO.WOJobs.CurrentItem.WOJobAction %>"
                                                                                                ToolTip="Enter Action" TextMode="MultiLine"
                                                                                                Enabled="<%# mnWO.WOStatusID <> 3 %>" Width="645px"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                </asp:PlaceHolder>
																				<%--Sankalp 08-08-25--%>
																				<tr id="methodOfComplianceControl">
																					<td></td>
																					<asp:PlaceHolder runat="server" ID="phMethodOfCompliance" Visible="false">
																						<%--<fieldset class="clsFieldSetNewStyle" id="Fieldset5" runat="server">--%>

																							<td>
																								<span id="lblMethodOfCompliance" class="clsLabelauto">Method Of Compliance</span>
																							</td>
																							<td colspan="7">
																								<asp:TextBox ID="txtMethodOfCompliance" runat="server"
																									CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="645px"
																									ToolTip="Enter Method Of Compliance." TextMode="MultiLine"
																									Enabled='<%# mnWO.WOStatusID <> 3 %>'
																									Text='<%# mnWO.WOJobs.CurrentItem.MethodOfCompliance %>' />
																							</td>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</tr>
                                                                                <tr id="remarkControls">
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span id="lblRemark" class="clsLabelauto">Remark</span>
                                                                                    </td>
                                                                                    <td colspan="7">
                                                                                        <asp:TextBox ID="txtWOJobRemark" runat="server"
                                                                                            CssClass="clsTextBoxTagSearchMultilineNewstyle1"
                                                                                            Text="<%# mnWO.WOJobs.CurrentItem.WOJobRemark %>"
                                                                                            Enabled="<%# mnWO.WOStatusID <> 3 %>" Width="645px"
                                                                                            ToolTip="Enter Remark" TextMode="MultiLine" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="statusControls">
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span id="lblWOJobStatus" class="clsLabelauto">Job Status</span>
                                                                                    </td>
                                                                                    <td colspan="4">
                                                                                        <asp:DropDownList ID="cmbWOStatusList" runat="server"
                                                                                            AutoPostBack="True" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                                            DataTextField="Name" DataValueField="ID"
                                                                                            Enabled="<%# mnWO.WOStatusID <> 3 %>"
                                                                                            SelectedValue="<%# mnWO.WOJobs.CurrentItem.WOJobStatusID %>">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <tr id="watchListControls">
                                                                                    <td colspan="8">
                                                                                        <asp:PlaceHolder runat="server" ID="phWatchListDetails" Visible="false">
                                                                                            <fieldset class="clsFieldSetNewStyle" id="Fieldset4" runat="server">
                                                                                                <legend id="Legend4" class="clsFieldSet1" runat="server">
                                                                                                    <b>Watchlist Details ( If any )</b>
                                                                                                </legend>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label runat="server" ID="lblAddToWatchList" Text="Add To Watchlist" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox runat="server" ID="chkAddToWatchList"
                                                                                                                Checked='<%# mnWO.WOJobs.CurrentItem.AddToWatchList %>'
                                                                                                                Enabled='<%# Not mnWO.WOJobs.CurrentItem.ConsiderInWatchList %>' />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblWatchListInstructions" runat="server"
                                                                                                                CssClass="clsLabelAuto" Text="Watchlist Instructions" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtWatchListInstructions" runat="server"
                                                                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle" Width="435px"
                                                                                                                ToolTip="Enter Watchlist Instructions." TextMode="MultiLine"
                                                                                                                Enabled='<%# Not mnWO.WOJobs.CurrentItem.ConsiderInWatchList %>'
                                                                                                                Text='<%# mnWO.WOJobs.CurrentItem.WatchListInstructions %>' />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </fieldset>
                                                                                        </asp:PlaceHolder>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
														</tr>
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlReqDetails" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<fieldset class="clsFieldSetNewStyle" id="Fieldset2" runat="server">
																			<legend id="Legend2" class="clsFieldSet1" runat="server"><b>Requisition Details</b></legend>
																			<table width="100%">
																				<tr>
																					<td>
																						<asp:LinkButton ID="lnkCreateRequisition" runat="server" Width="150px" CssClass="clsHyperlink1"
																							Font-Underline="true" ToolTip="Create Requisition of Job Spares Items(s)">Create Spare Requisition</asp:LinkButton>
																					</td>
																					<td align="right">
																						<asp:LinkButton ID="lnkViewIndent" runat="server" Width="160px" CssClass="clsHyperlink1"
																							ToolTip="Go on Requested Item(s) screen">Requisition Items</asp:LinkButton>
																					</td>
																				</tr>
																			</table>
																		</fieldset>

																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
														<tr>
															<td valign="top">
																<asp:Panel ID="pnlMELSnagDetails" Visible="False" runat="server">
																	<asp:UpdatePanel ID="upnlMELSnagDetails" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<table id="Table3">
																				<tr>
																					<td valign="top" colspan="1">
																						<asp:Panel ID="pnlSnag" runat="server">
																							<fieldset class="clsFieldSetNewStyle" id="fdswodetail" runat="server"
																								visible="<%# (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3) %>">
																								<legend id="ldwodetail" class="clsLabelHeader" runat="server">
																									<b>
																										<asp:Label ID="Label7" runat="server"
																											Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Details", "Snag Details") %>'>
																										</asp:Label>
																									</b>
																								</legend>
																								<table id="Table2">
																									<tr>
																										<td></td>
																										<td>
																											<asp:CheckBox ID="chkIsMajor" runat="server" Checked="<%# mnWO.WOJobs.CurrentItem.IsMajor %>"
																												CssClass="clsLabelAuto" Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Check if this is Major" />
																										</td>
																										<td>
																											<asp:Label ID="Label2" runat="server" CssClass="clsLabelauto">Is Major</asp:Label>
																										</td>
																									</tr>
																									<tr>
																										<td></td>
																										<td>
																											<asp:CheckBox ID="chkIsRepetitive" runat="server" Checked="<%# mnWO.WOJobs.CurrentItem.IsRepetitive %>"
																												CssClass="clsLabelAuto" Enabled="<%# mnWO.WOStatusID <> 3 %>" ToolTip="Check if this is Repetitive" />
																										</td>
																										<td>
																											<asp:Label ID="lblIsRepetitive" runat="server" CssClass="clsLabelAuto">Is Repetitive</asp:Label>
																										</td>
																									</tr>
																								</table>
																							</fieldset>
																						</asp:Panel>
																					</td>
																					<td valign="top" colspan="1">
																						<fieldset class="clsFieldSetNewStyle" id="Fieldset1" runat="server"
																							visible="<%# (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3) %>">
																							<legend id="Legend1" class="clsFieldSet1" runat="server"><b>Component Details</b></legend>
																							<table id="Table4">
																								<tr>
																									<td valign="top">
																										<table id="Table5">
																											<tr>
																												<td>
																													<asp:Label ID="lblDateOfOccurrence" runat="server" CssClass="clsLabelMedium">Date Of Occurrence</asp:Label>
																												</td>
																												<td>
																													<table>
																														<tr>
																															<td>
																																<asp:TextBox ID="txtDateOfOccurrence" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																																	Width="100px"></asp:TextBox>
																																<cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
																																	Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDateOfOccurrence"></cc2:CalendarExtender>
																																<cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtDateOfOccurrence"
																																	WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsTextBoxDate_Ajax" />
																															</td>
																															<td valign="middle">
																																<asp:Label ID="lblIsUnderMEL" runat="server" CssClass="clsLabelauto" Visible="<%# (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3)  %>"
																																	Text='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Is Under ADD", "Is Under MEL") %>'></asp:Label>
																															</td>
																															<td>
																																<asp:CheckBox ID="chkIsUnderMEL" runat="server" CssClass="clsLabelAuto" Visible="<%# (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3) %>"
																																	Enabled="false" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Check if this is Under ADD", "Check if this is Under MEL") %>'
																																	Checked="<%# mnWO.WOJobs.CurrentItem.IsUnderMEL %>" AutoPostBack="True"></asp:CheckBox>
																															</td>
																															<td>
																																<asp:Label ID="lblIsUnderMELNote" runat="server" CssClass="clsLabelAuto" Visible="<%# (mnWO.WOJobs.CurrentItem.WOJobTypeID = 3) %>"
																																	Text='<%# iif(AppSettings("MELSnagNomenclature") = "True", "(Check if Job is under ADD)", "(Check if Job is under MEL)") %>'></asp:Label>
																															</td>
																														</tr>
																													</table>

																												</td>

																											</tr>
																											<asp:Panel ID="pnlMEL" runat="server">
																												<tr>
																													<td>
																														<asp:Label ID="lblComponent" runat="server" CssClass="clsLabelAuto">Component</asp:Label>

																													</td>
																													<td>
																														<table>
																															<tr>
																																<td>
																																	<asp:DropDownList ID="cmbComponent" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="CompID"
																																		DataTextField="PartNoSerialNo" AutoPostBack="true" Width="200px">
																																	</asp:DropDownList>
																																</td>
																																<td>
																																	<asp:CheckBox ID="chkShowMEL" runat="server" CssClass="clsCheckBox" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Show ADD Parts", "Show MEL Parts") %>'
																																		Visible="false" Enabled="<%# mnWO.WOJobs.CurrentItem.WOJobTypeID = 3 %>" AutoPostBack="True"></asp:CheckBox>

																																</td>
																															</tr>
																														</table>
																													</td>
																												</tr>
																												<asp:Panel ID="pnlMELCategory" runat="server">
																													<tr>
																														<td>
																															<asp:Label ID="lblMELCategory" runat="server" CssClass="clsLabelauto"
																																Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category", "MEL Category") %>'></asp:Label>
																														</td>
																														<td>
																															<table>
																																<tr>
																																	<td>
																																		<asp:DropDownList ID="cmbMELCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
																																			DataValueField="ID" DataTextField="Name" SelectedValue="<%# mnWO.WOJobs.CurrentItem.MELCategoryID %>"
																																			AutoPostBack="True">
																																		</asp:DropDownList>
																																	</td>
																																	<td>
																																		<asp:Label ID="lblFrequency" runat="server" CssClass="clsLabelauto" Width="85px">Frequency</asp:Label>

																																	</td>
																																	<td>
																																		<asp:TextBox ID="txtFrequencyInDay" runat="server" CssClass="clsTextBoxTagSearchDate"
																																			Text="<%# mnWO.WOJobs.CurrentItem.FrequencyInDays %>" ToolTip="Enter Frequency In Days"
																																			MaxLength="4" AutoPostBack="True"></asp:TextBox>
																																	</td>
																																	<td>
																																		<asp:Label ID="lblDays" runat="server" CssClass="clsLabelAuto1">Days</asp:Label>
																																	</td>
																																	<td>
																																		<asp:TextBox ID="txtFrequencyInHours" runat="server" CssClass="clsTextBoxTagSearchDate"
																																			Text="<%# mnWO.WOJobs.CurrentItem.FrequencyInHours %>" ToolTip="Enter Frequency In Hours"
																																			MaxLength="5" AutoPostBack="True"></asp:TextBox>
																																	</td>
																																	<td>
																																		<asp:Label ID="lblHours" runat="server" CssClass="clsLabelAuto1">Hours</asp:Label>
																																	</td>
																																	<td>
																																		<asp:CheckBox ID="chkIsInHours" runat="server" CssClass="clsCheckBox" Text="(Select if Freq. in Hours)"
																																			Visible="false" Enabled="False" AutoPostBack="True" Checked="<%# mnWO.WOJobs.CurrentItem.IsHours %>"
																																			Width="164px"></asp:CheckBox>
																																	</td>
																																</tr>
																															</table>
																														</td>
																													</tr>
																												</asp:Panel>
																											</asp:Panel>
																										</table>
																									</td>
																								</tr>
																							</table>
																						</fieldset>
																					</td>
																				</tr>
																			</table>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</asp:Panel>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</cc2:TabPanel>
											<%-- 1--%>
											<cc2:TabPanel ID="tabWOJobTask" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
												<HeaderTemplate>
													Task Cards (<asp:Label runat="server" Text="<%# mnWO.WOJobs.CurrentItem.WOJobTasks.Count %>"
														ID="lblHeader"></asp:Label>)
												</HeaderTemplate>
												<ContentTemplate>
													<asp:UpdatePanel ID="upnlWOJobTaskList" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<iframe id="IframeWOJobTaskList" width="100%" height="200px" scrolling="yes" marginheight="0"
																frameborder="0" onload="autoWOJobTaskList()"></iframe>
														</ContentTemplate>
													</asp:UpdatePanel>
												</ContentTemplate>
											</cc2:TabPanel>
											<%--2--%>
											<cc2:TabPanel ID="tabWOJobDesignationAllocation" runat="server" CssClass="clsPanel1"
												ClientIDMode="Static">
												<HeaderTemplate>
													Designation Allocations (<asp:Label runat="server" Text="<%# mnWO.WOJobs.CurrentItem.WOJobDesignationAllocations.Count %>"
														ID="Label3"></asp:Label>)
												</HeaderTemplate>
												<ContentTemplate>
													<asp:UpdatePanel ID="upnlWOJobDesignationAllocationList" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<iframe id="IframeWOJobDesignationAllocationList" width="100%" height="200px" scrolling="yes"
																marginheight="0" frameborder="0" onload="autoWOJobDesignationAllocationList()"></iframe>
														</ContentTemplate>
													</asp:UpdatePanel>
												</ContentTemplate>
											</cc2:TabPanel>
											<%-- 3--%>
											<cc2:TabPanel ID="tabWOJobSpares" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
												<HeaderTemplate>
													Spares (<asp:Label runat="server" Text="<%# mnWO.WOJobs.CurrentItem.WOJobSpares.Count %>"
														ID="Label4"></asp:Label>)
												</HeaderTemplate>
												<ContentTemplate>
													<asp:UpdatePanel ID="upnlWOJobSparesList" runat="server" UpdateMode="Conditional"
														ClientIDMode="Static">
														<ContentTemplate>
															<iframe id="IframeWOJobSparesList" width="100%" height="100%" scrolling="yes" marginheight="0"
																frameborder="0" onload="autoWOJobSparesList()"></iframe>
														</ContentTemplate>
													</asp:UpdatePanel>
												</ContentTemplate>
											</cc2:TabPanel>
											<%-- 4--%>
											<cc2:TabPanel ID="tabWOJobComps" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
												<HeaderTemplate>
													Removal / Installations (<asp:Label runat="server" Text="<%# mnWO.WOJobs.CurrentItem.WOJobComps.Count %>"
														ID="Label5"></asp:Label>)
												</HeaderTemplate>
												<ContentTemplate>
													<asp:UpdatePanel ID="upnlWOJobCompsList" runat="server" UpdateMode="Conditional"
														ClientIDMode="Static">
														<ContentTemplate>
															<iframe id="IframeWOJobCompsList" width="100%" height="200px" scrolling="yes" marginheight="0"
																frameborder="0" onload="autoWOJobCompsList()"></iframe>
														</ContentTemplate>
													</asp:UpdatePanel>
												</ContentTemplate>
											</cc2:TabPanel>
											<%-- 5--%>
											<cc2:TabPanel ID="tabWOJobNRC" runat="server" CssClass="clsPanel1" Enabled="<%# mnWO.WOJobs.CurrentItem.WOJobTypeID <> 5 %>"
												ClientIDMode="Static">
												<HeaderTemplate>
													<asp:Label runat="server" Text="NRC" ID="lblWOJobNRCTab"></asp:Label>
													<asp:Label runat="server" Text="<%# mnWO.WOJobs.CurrentItem.WOJobNRCCountForLinK %>"
														ID="Label6"></asp:Label>
												</HeaderTemplate>
												<ContentTemplate>
													<asp:UpdatePanel ID="upnlWOJobNRCList" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<iframe id="IframeWOJobNRCList" width="100%" height="200px" scrolling="yes" marginheight="0"
																frameborder="0" onload="autoWOJobNRCList()"></iframe>
														</ContentTemplate>
													</asp:UpdatePanel>
												</ContentTemplate>
											</cc2:TabPanel>
										</cc2:TabContainer>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>

		<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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

		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" frameborder="0" height="100%" width="100%" allowtransparency="true"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
			PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileUploadStateComplete() {
				$("#btnDummyFileUpload").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}
			$(document).ready(function () {
				$("#btnSelectFile").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = 'visible';
						$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
						$("#IFileUpload").ready(function () {
							$("#btnDummyFileUpload").click();
							$get("AjaxLoader").style.visibility = 'hidden';
						});

						return false;
					} catch (e) {
						alert(e);
					}


				});
			});

			function OpenFileUploadWindow() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
					//                        $("#IFileUpload").ready(function () {
					//                            $("#btnDummyFileUpload").click();
					//                            $get("AjaxLoader").style.visibility = 'hidden';
					//                        });
					//                if (!$.browser.msie) {
					//                    $("#btnDummyFileUpload").click();
					//                    $get("AjaxLoader").style.visibility = 'hidden';
					//                }

					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFileUpload(fileattached) {
				var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
				//close File Upload popup window
				FileUpwindow.hide();
				//Free resources
				$("#IFileUpload").attr("src", "JavaScript:''");
				if (fileattached) {
					//call hidden button to set file upload content to object
					$("#hdnBtnFileUpload").click();
				}
			}
		</script>
		<!-- End File Upload Modal Dialog-->
		<!-- Requisition View-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyRequisitionView" Text="Dummy Requisition View"
				CausesValidation="false" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlRequisitionView" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IRequisitionView" allowtransparency="true" frameborder="0" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupRequisitionView" runat="server" TargetControlID="btnDummyRequisitionView"
			PopupControlID="pnlRequisitionView" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameRequisitionViewComplete() {
				$("#btnDummyRequisitionView").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}
			function RequisitionView() {
				try {
					$get("AjaxLoader").style.visibility = 'visible';
					$("#IRequisitionView").attr("src", "wfReqItemsViewForWO_Ajax.aspx?Type=pup");
					if (!$.browser.msie) {
						$("#btnDummyRequisitionView").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForRequisitionView() {
				var RequisitionViewwindow = $find("<%=mdlPopupRequisitionView.ClientID %>");
				//close Ass Insp Maint Done By Emp popup window
				RequisitionViewwindow.hide();
				//Free resources
				$("#IRequisitionView").attr("src", "JavaScript:''");
				//            $("#hdnBtnRequisitionView").click();

			}
		</script>
		<div>
			<script type="text/javascript">
				function CallWOJobTask() {
					document.getElementById('IframeWOJobTaskList').src = 'wfnWOJobTaskList.aspx?Type=childpup';
				}
				function CallWOJobDesignationAllocations() {
					document.getElementById('IframeWOJobDesignationAllocationList').src = 'wfnWOJobDesignationAllocation_AJAX.aspx?Type=childpup';
				}
				function CallWOJobSpares() {
					document.getElementById('IframeWOJobSparesList').src = 'wfnWOJobSpare_AJAX.aspx?Type=childpup';
				}
				function CallWOJobComps() {
					document.getElementById('IframeWOJobCompsList').src = 'wfnWOJobComp_AJAX.aspx?Type=childpup';
				}
				function CallWOJobNRC() {
					document.getElementById('IframeWOJobNRCList').src = 'wfnWOJobNRCList.aspx?Type=childpup';
				}

			</script>
			<script language="JavaScript" type="text/javascript">
				function CloseChildPage() {
					$find('<%=WOJobDetailsTabContainer.ClientID%>').set_activeTabIndex(0);
				}
			</script>
		</div>
		<!-- SelectTasks Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummySelectTasks" Text="Dummy SelectTasks" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupSelectTasks" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupSelectTasks" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupSelectTasks" runat="server" TargetControlID="btnDummySelectTasks"
			PopupControlID="pnlPopupSelectTasks" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameSelectTasksStateComplete() {
				$("#btnDummySelectTasks").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}

			function OpenToAddSelectTasks() {
				try {
					$get("AjaxLoader").style.visibility = "visible";
					$("#iPopupSelectTasks").attr("src", "wfSelectTaskCardList_Ajax.aspx?Type=pup");
					if (!$.browser.msie) {
						$("#btnDummySelectTasks").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}

					return false;
				} catch (e) {
					alert(e);
				}


			}

		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForSelectTasks() {
				var SelectTasksWindow = $find("<%=mdlPopupSelectTasks.ClientID %>");
                //close SelectTasks popup window
                SelectTasksWindow.hide();
                $("#iPopupSelectTasks").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddSelectTasks").click();
            }
        </script>
		<!-- End-->
		<!-- JobTaskDetail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyJobTaskDetail" Text="Dummy JobTaskDetail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupJobTaskDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupJobTaskDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupJobTaskDetail" runat="server" TargetControlID="btnDummyJobTaskDetail"
			PopupControlID="pnlPopupJobTaskDetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameJobTaskDetailStateComplete() {
                $("#btnDummyJobTaskDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddJobTaskDetail(Index) {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupJobTaskDetail").attr("src", "wfnWOJobTask_AJAX.aspx?Type=pup&Index=" + Index);
                    if (!$.browser.msie) {
                        $("#btnDummyJobTaskDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }


        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForJobTaskDetail() {
                var JobTaskDetailWindow = $find("<%=mdlPopupJobTaskDetail.ClientID %>");
                //close JobTaskDetail popup window
                JobTaskDetailWindow.hide();
                $("#iPopupJobTaskDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddJobTaskDetail").click();
            }
        </script>
		<!-- End-->
		<!-- ResourceAllocation Popup Window -->
		<%-- 'Added by Saylee on 29-May-2019--%>
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyResourceAllocation" Text="Dummy ResourceAllocation"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupResourceAllocation" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="iPopupResourceAllocation" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupResourceAllocation" runat="server" TargetControlID="btnDummyResourceAllocation"
			PopupControlID="pnlPopupResourceAllocation" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameResourceAllocationStateComplete() {
                $("#btnDummyResourceAllocation").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddResourceAllocation() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupResourceAllocation").attr("src", "wfnWOJobResourceAllocation_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyResourceAllocation").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForResourceAllocation() {
                var ResourceAllocationWindow = $find("<%=mdlPopupResourceAllocation.ClientID %>");
                //close ResourceAllocation popup window
                ResourceAllocationWindow.hide();
                $("#iPopupResourceAllocation").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddResourceAllocation").click();
            }
        </script>
		<!-- End-->
		<!-- WOJobNRCDetail Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWOJobNRCDetail" Text="Dummy WOJobNRCDetail"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupWOJobNRCDetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupWOJobNRCDetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupWOJobNRCDetail" runat="server" TargetControlID="btnDummyWOJobNRCDetail"
			PopupControlID="pnlPopupWOJobNRCDetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameWOJobNRCDetailStateComplete() {
                $("#btnDummyWOJobNRCDetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenToAddWOJobNRCDetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupWOJobNRCDetail").attr("src", "wfnWOJobNRC.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyWOJobNRCDetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForWOJobNRCDetail() {
                var WOJobNRCDetailWindow = $find("<%=mdlPopupWOJobNRCDetail.ClientID %>");
                //close WOJobNRCDetail popup window
                WOJobNRCDetailWindow.hide();
                $("#iPopupWOJobNRCDetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddWOJobNRCDetail").click();
            }
        </script>
		<!-- End-->
		<div>
			<!-- Designation Popup Window -->
			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlDesignation" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeDesignation" frameborder="0" height="100%" allowtransparency="true"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
				PopupControlID="pnlDesignation" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
                function IFrameStateComplete() {
                    $("#btnDummyDesignation").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenDesignationWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeDesignation").attr("src", "wfDesignation_Ajax.aspx?Type=pup");
                        // $("#IframeDesignation").load(function () {
                        //                    var doc = IframeDesignation.window;
                        //                    IframeDesignation.SetPageLayout();

                        if (!$.browser.msie) {
                            $("#btnDummyDesignation").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }


                        //});


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForDesignation() {
                    var Designationwindow = $find("<%=mdlPopupDesignation.ClientID %>");
                    //close Designation popup window
                    Designationwindow.hide();
                    //           release resources
                    $("#IframeDesignation").attr("src", "JavaScript:''");
                    //call Designation image button
                    $("#hdnimgbtnDesignation").click();
                }
            </script>
			<!-- End-->
		</div>
		<script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    cache: false,
                    async: false,
                    data: params,
                    beforeSend: OnBeforeSend,
                    success: onSuccess,
                    error: onError
                });
                return false;
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
            function ParentRefresh() {
                $("#hdnimgbtnDesignation").click();
            }
        </script>
		<!-- WO Detail Popup Window Added By Prashant 16-Aug-2019-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWODetail" Text="Dummy WODetail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupWODetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupWODetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupWODetail" runat="server" TargetControlID="btnDummyWODetail"
			PopupControlID="pnlPopupWODetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameWODetailStateComplete() {
                $("#btnDummyWODetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenToAddWODetail() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupWODetail").attr("src", "wfnWODetail_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyWODetail").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }
            }
        </script>
		<script type="text/javascript">
            function ParentCallBackFunctionForWODetail() {
                var WODetailWindow = $find("<%=mdlPopupWODetail.ClientID %>");
                //close WODetail popup window
                WODetailWindow.hide();
                $("#iPopupWODetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddWODetail").click();
            }
        </script>
		<!-- End-->

		<!-- Completion Details Popup -->

		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyCompletionDetails" Text="Dummy Completion Details" />
		</div>
		<asp:Panel runat="server" ID="pnlCompletionDetails" HorizontalAlign="Center">
			<asp:UpdatePanel ID="upnlCompletionDetails" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<div>
						<table class="clstablelistout" id="tblMain">
							<tr>
								<td class="clsFormHeader1Newstyle" colspan="4">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblCompletionDetails" runat="server"
													CssClass="clsFormHeader" Text="Completion Details"></asp:Label>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnSaveCompletionDetails" runat="server"
															CssClass="clsbtnH clsinfoH" ToolTip="Add Completion Details."
															Text="Ok" CausesValidation="False"></asp:Button>
														<asp:Button ID="btnCloseCompletionDetails" runat="server"
															CssClass="clsbtnH clsinfoH" ToolTip="Close Completion Details screen."
															Text="Cancel" CausesValidation="False"></asp:Button>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="4">
									<asp:UpdatePanel ID="upnlCompletionDetailsErrorList" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="vsCompletionDetails" runat="server" CssClass="clsValidationSummary"
												HeaderText="Mandatory Details Required." ValidationGroup="CompletionDetails"></asp:ValidationSummary>
											<asp:RequiredFieldValidator ID="rfvCompletionDetailStartDate" runat="server" Display="None"
												CssClass="clsLabelAuto" ErrorMessage="Start Date and Time is required."
												ControlToValidate="txtCompletionDetailStartDate" ValidationGroup="CompletionDetails">
											</asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvCompletionDetailEndDate" runat="server" Display="None"
												CssClass="clsLabelAuto" ErrorMessage="End Date and Time is required."
												ControlToValidate="txtCompletionDetailEndDate" ValidationGroup="CompletionDetails">
											</asp:RequiredFieldValidator>
											<asp:RequiredFieldValidator ID="rfvCompletionDetailAction" runat="server" Display="None"
												CssClass="clsLabelAuto" ErrorMessage="Action Taken is required." ValidationGroup="CompletionDetails"
												ControlToValidate="txtCompletionDetailAction">
											</asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvCompletionDetailsStartDate" runat="server" Display="None"
												CssClass="clsLabelAuto" ControlToValidate="txtCompletionDetailStartDate"
												ValidationGroup="CompletionDetails" OnServerValidate="CustomValidation_CompletionDetail">
											</asp:CustomValidator>
											<asp:CustomValidator ID="cvCompletionDetailsEndDate" runat="server" Display="None"
												CssClass="clsLabelAuto" ControlToValidate="txtCompletionDetailEndDate"
												ValidationGroup="CompletionDetails" OnServerValidate="CustomValidation_CompletionDetail">
											</asp:CustomValidator>
											<asp:CustomValidator ID="cvCompletionDetailsStartDateTime" runat="server"
												Display="None" CssClass="clsLabelAuto" ControlToValidate="txtCompletionDetailStartDateTime"
												ValidationGroup="CompletionDetails">
											</asp:CustomValidator>
											<asp:CustomValidator ID="cvCompletionDetailsEndDateTime" runat="server"
												Display="None" CssClass="clsLabelAuto" ControlToValidate="txtCompletionDetailEndDateTime"
												ValidationGroup="CompletionDetails">
											</asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="4">
									<asp:UpdatePanel ID="upnlCompletionDetailDateControls" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td></td>
													<td>
														<asp:Label ID="lblCompletionDetailEmployee" runat="server" class="clsLabelAuto">
															Employee
														</asp:Label>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtCompletionDetailEmployee" 
                                                            Enabled="false" ToolTip="Employee Name."
															CssClass="clsTextBoxTagSearch"  />
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblStarCompletionDetailStartDate" runat="server" CssClass="clsLabelStar">
															*
														</asp:Label>
													</td>
													<td>
														<span id="lblCompletionDetailStartDate" class="clsLabelAuto">Start Date</span>
													</td>
													<td>
														<table>
															<tr>
																<td>
																	<asp:TextBox ID="txtCompletionDetailStartDate" runat="server"
																		CssClass="clsTextBoxTagSearchDate" ToolTip="Enter Start Date."
																		onchange="ValidateDateText(this,'txtCompletionDetailStartDate_CalendarExtender');"
																		autocomplete="off" AutoPostBack="True" />
																	<cc2:CalendarExtender ID="txtCompletionDetailStartDate_CalendarExtender"
																		runat="server" CssClass="cal_Theme1" Enabled="True"
																		Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCompletionDetailStartDate" />
																	<cc2:TextBoxWatermarkExtender ID="txtCompletionDetailStartDate_TextBoxWatermarkExtender"
																		runat="server" TargetControlID="txtCompletionDetailStartDate"
																		WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsTextBoxTagSearchDate" />
																</td>
																<td>
																	<asp:TextBox ID="txtCompletionDetailStartDateTime" runat="server"
																		AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																		MaxLength="10" ToolTip="Enter Time." Width="47px" />
																	
																	<cc2:MaskedEditExtender ID="txtCompletionDetailStartDateTimeMaskedEditExtender"
																		TargetControlID="txtCompletionDetailStartDateTime"
																		AutoComplete="true" Mask="99:99" MaskType="Time"
																		CultureName="en-us" MessageValidatorTip="true" runat="server" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblStarCompletionDetailEndDate" runat="server" CssClass="clsLabelStar">
															*
														</asp:Label>
													</td>
													<td>
														<span id="lblCompletionDetailEndDate" class="clsLabelAuto">End Date </span>
													</td>
													<td>
														<table>
															<tr>
																<td>
																	<asp:TextBox ID="txtCompletionDetailEndDate" runat="server"
																		CssClass="clsTextBoxTagSearchDate" ToolTip="Enter End Date."
																		onchange="ValidateDateText(this,'txtCompletionDetailEndDate_CalendarExtender');"
																		autocomplete="off" AutoPostBack="True" />
																	<cc2:CalendarExtender ID="txtCompletionDetailEndDate_CalendarExtender"
																		runat="server" CssClass="cal_Theme1" Enabled="True"
																		Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCompletionDetailEndDate" />
																	<cc2:TextBoxWatermarkExtender ID="txtCompletionDetailEndDate_TextBoxWatermarkExtender"
																		runat="server" TargetControlID="txtCompletionDetailEndDate"
																		WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsTextBoxTagSearchDate" />
																</td>
																<td>
																	<asp:TextBox ID="txtCompletionDetailEndDateTime" runat="server"
																		AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
																		MaxLength="10" ToolTip="Enter Time." Width="47px" />
																	<cc2:MaskedEditExtender ID="txtCompletionDetailEndDateTimeMaskedEditExtender"
																		TargetControlID="txtCompletionDetailEndDateTime"
																		AutoComplete="true" Mask="99:99" MaskType="Time"
																		CultureName="en-us" MessageValidatorTip="true" runat="server" />
																</td>
															</tr>
														</table>
													</td>
												</tr>

												<tr>
													<td></td>
													<td>
														<span id="lblCompletionDetailActualTime" class="clsLabelAuto">Man Hours</span>
														<span id="lblCompletionDetailActual" class="clsLabelAuto">(Actual)</span>
													</td>
													<td>
														<asp:TextBox ID="txtCompletionDetailActualTime" runat="server"
															CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
															ToolTip="Man Hours (Actual)."></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblStarCompletionDetailAction" runat="server" CssClass="clsLabelStar">
															*
														</asp:Label>
													</td>
													<td>
														<span id="lblCompletionDetailAction" class="clsLabelAuto">Action</span>
													</td>
													<td>
														<asp:TextBox ID="txtCompletionDetailAction"
															runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
															Enabled="<%# mnWO.WOStatusID <> 3 %>"
															ToolTip="Enter Action Taken." TextMode="MultiLine" Width="300px"></asp:TextBox>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupCompletionDetails" runat="server" TargetControlID="btnDummyCompletionDetails"
			PopupControlID="pnlCompletionDetails" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<!-------------------->

	</form>

	<script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ("<%=Not HttpContext.Current.Session("ToDisbleJobControlsAsCompletedRightNotGiven") Is Nothing %>" == "True") {
                if ("<%# HttpContext.Current.Session("ToDisbleJobControlsAsCompletedRightNotGiven")  %>" == "True") {
                    $("input[type='text'],textarea,input[type='checkbox'],select,#btnSelectFiles,#imgDelAttach1,#btnSave,#btnPrint").prop("disabled", true);
                    $("a").removeAttr("href");
                    $("a").css("color", "gray");

                    //tabs enabled code shifted to controlvisibility function
                }
            }
        });

    </script>

</body>
</html>
