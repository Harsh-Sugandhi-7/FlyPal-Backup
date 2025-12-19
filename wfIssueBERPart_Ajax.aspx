<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueBERPart_Ajax.aspx.vb"
    Inherits="Flypal.wfIssueBERPart_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>BER Parts</title>

    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script type="text/javascript">

		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

	</script>

</head>
<body>

    <script type="text/javascript">
        function calltxtSearchEvent() {
            document.getElementById("<%= txtName.ClientID %>").fireEvent("onchange");
        }
    </script>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
									<table width="100%">
                                        <tr>
                                            <td>
												<span id="lbltitle" class="clsFormHeader">BER Parts</span>
                                            </td>
                                            <td align="right">
												<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="tblActionBtns">
															<tr>
																<td>
																	<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
																		runat="server" ToolTip="Click to close BER Part list screen"
																		Text="Close" CausesValidation="False">
																	</asp:Button>
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
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        CssClass="clsValidationSummary" ValidationGroup="valGrp1">
                                    </asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtDate" ErrorMessage="Date Required"
                                        ValidationGroup="valGrp1">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td width="72px">
                                                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                        ClientIDMode="Static" CausesValidation="true" 
                                                                        onchange="ValidateDateText(this,'Calender_watermarkextender')">
                                                                    </asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" 
                                                                        CssClass="cal_Theme1" Enabled="True" 
                                                                        Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ClientIDMode="static" TargetControlID="txtDate" 
                                                                        ID="Calender_watermarkextender" runat="server" 
                                                                        WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td width="72px">
                                                                    <span id="lblPartNo" class="clsLabelAuto">Part No. / Description</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" 
                                                                        MaxLength="100" ClientIDMode="Static" 
                                                                        ToolTip="Search for Part No. / Description" AutoPostBack="True">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Label1" class="clsLabelAuto">Serial No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch"
																		MaxLength="100" ClientIDMode="Static" ToolTip="Search for Serial No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
														<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
															ToolTip="Click to search as per searching Criteria."
															ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />
													</td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
                                                            List of Parts :
                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:GridView ID="dgBERPartList" runat="server" AutoGenerateColumns="False"
															DataKeyNames="IssueChildID" ShowHeaderWhenEmpty="true"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" 
                                                            AllowPaging="True" PageSize="10">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" 
                                                                ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
                                                                <asp:BoundField Visible="False" DataField="IssueChildID"></asp:BoundField>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderInfo" HeaderText="Order Info." HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssueInfo" HeaderText="Issue Info." HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Supplier">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PartInfo" HeaderText="Part Info." HtmlEncode="false">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="OrderItemReceiptBalanceQty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Discard" HeaderText="Discard" CommandName="Discard">
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:ButtonField>
                                                                 <asp:BoundField DataField="OrderItemId" HeaderText="Order Item Id" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn">
                                                                   <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup for category/nomenclature-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                    background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                    z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
            width: 100%;">
            <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
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


            function FileUpload() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForIssueToBERPart.aspx");
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

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
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();

            }
        </script>
         <script type="text/javascript">
             function ParentCallBackFunctionForClose() {
                 var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
                 //close File Upload popup window
                 FileUpwindow.hide();
                 //Free resources
                 $("#IFileUpload").attr("src", "JavaScript:''");
                 //call hidden button to set file upload content to object
             }
        </script>
        <!-- End -->
    </div>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                //                beforeSend: function (xhr, settings) {
                //                    $("[id$=processing]").dialog();
                //                },
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
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtName.ClientID %>").autocomplete('wfAutoItemList.aspx?IsSerialisedPartsList=False', {
                width: 225,
                autoFill: false,
                matchContains: true,
                delay: 0
            });

            $("#<%=txtSerialNo.ClientID %>").autocomplete('wfAutoInventoryList.aspx?Type=SerialNo&LookInType=<%=LookInType%>&PartID=<%=PartID%>', {
                width: 200,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });       
    </script>
    </form>
</body>
</html>
