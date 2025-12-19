<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfVendorApproval_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfVendorApproval_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>>Vendor Document Approval Detail</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td colspan="4" class="clsFormHeader1Newstyle">
                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Vendor Document Approval Detail</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="Expiry Date must be greater than Issue Date"
                                    ControlToValidate="txtToDate" Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark too long."
                                    ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" Display="None" CssClass="clsLabelAuto"
                                    ErrorMessage="Name Is Required" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend class="clsFieldSet1"><b>Details</b></legend>
                            <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="lblVendor" class="clsLabelAuto">Vendor</span>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelHeader"></asp:Label>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="lblApprovalNo" class="clsLabelAuto">Approval No.</span>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtApprovalNo" runat="server" CssClass="clsTextBoxTagSearch1" ToolTip="Approval No."
                                                    MaxLength="50"
                                                    Text="<%# mVendorApproval.ApprovalNo %>"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblNameStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch1" ToolTip="Enter Name"
                                                    Enabled="<%# mVendorApproval.IsNew and mVendorApproval.SortNo=1 %>" MaxLength="100"
                                                    Text="<%# mVendorApproval.Name %>"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="chkIsOneTime" runat="server" Checked="<%# mVendorApproval.IsOneTime %>"
                                                    CssClass="clsLabelAuto" Text="One Time" ToolTip="Check to apply Applicability" />
                                                <asp:CheckBox ID="chkIsApplicable" runat="server" Checked="<%# mVendorApproval.IsApplicable %>"
                                                    CssClass="clsLabelAuto" Text="Applicable" ToolTip="Check to apply Applicability" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="lblFromDate" class="clsLabel">From Date</span>
                                            </td>
                                            <td>
                                                <table cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                onchange="ValidateDateText(this,'txtFromDate_CalendarExtender');" Width="100px" AutoComplete="off"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="TBWE1" runat="server" TargetControlID="txtFromDate"
                                                                WatermarkText="<%$AppSettings:DateFormat%>" />
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>
                                                            <span id="lblToDate" class="clsLabel">To Date </span>
                                                        </td>
                                                        <td>&nbsp</td>
                                                        <td>

                                                            <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                onchange="ValidateDateText(this,'txtToDate_CalendarExtender');" Width="100px" AutoComplete="Off"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtToDate"
                                                                WatermarkText="<%$AppSettings:DateFormat%>" />

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mVendorApproval.Remark %>"
                                                    ToolTip="Enter Remark" MaxLength="250" TextMode="MultiLine" Width="275px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAttachFile1" runat="server" CssClass="clsLabelAuto">Attach File</asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <input type="button" runat="server" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                        class="clsbtnH clsinfoH1" />
                                                                </td>
                                                                <td style="padding-left: 3px;">
                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="False"
                                                                        Text="Remove Attachment" ToolTip="Click to Remove Attachment" Width="140px" />
                                                                </td>
                                                                <td style="padding-left: 3px;">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                        Visible="false" ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <!--Dummy panel to open modelpopup for FileUpload-->
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
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="4">
                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save" ToolTip="Click to Save"></asp:Button>
                                        </td>
                                        <td class="style3">
                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back"
                                                CausesValidation="False" ToolTip="Click to go Previous page"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
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
            </script>
        </div>
        <div>
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
        </div>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <div>
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
            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackFunctionForVendorApproval();
                    return false;
                }
            </script>
            <%--End--%>
        </div>
        <div>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">

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
        </div>
        <%--End--%>
    </form>
</body>
</html>
