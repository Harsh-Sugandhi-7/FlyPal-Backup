<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineCertificateRenew_AJAX.aspx.vb"
    Inherits="Flypal.wfMachineCertificateRenew_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>>Aircraft Renewal Certificate Detail</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
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
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Renewal Certificate</asp:Label>
                            </td>
                            <td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save the Certificate"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                                        CausesValidation="False" ToolTip="Click to Print the list of Certificates" Visible="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Send Mail" Enabled="<%# NOT mRenewMachineCertificate.IsNew %>"
                                                        ToolTip="Click to report by mail" Width="96px" />
                                                </td>
                                                <td class="style3">
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"
                                                        CausesValidation="False" ToolTip="Click to go Previous page"></asp:Button>
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
                <td colspan="4">
                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="Expiry Date must be greater than Issue Date"
                                ControlToValidate="txtExpiryDate" Display="None" CssClass="clsLabelAuto"></asp:CustomValidator><asp:CustomValidator
                                    ID="cvRemark" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark too long."
                                    ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <fieldset>
                        <legend class="clsFieldSet1"><b>Aircraft Certificate Details </b></legend>
                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxSearch_Ajax" ToolTip="Certificate Name"
                                                ReadOnly="True" BackColor="#E0E0E0" MaxLength="50" Text="<%# mRenewMachineCertificate.CertificateName %>"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            &nbsp;
                                        </td>
                                        <td class="style4">
                                            &nbsp;
                                        </td>
                                        <td colspan="2" class="style4">
                                            <asp:CheckBox ID="chkOneTimeCertificate" runat="server" CssClass="clsLabelAuto" DESIGNTIMEDRAGDROP="96"
                                                Text="One Time Certificate (Then no need of Expiry Date else Expiry Date is compulsory)"
                                                Checked="<%# mRenewMachineCertificate.OneTimeCertificate %>" ToolTip="Check if certificate is one time " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                        </td>
                                        <td class="style4">
                                            <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
                                        </td>
                                        <td class="style4" colspan="3">
                                            <asp:TextBox ID="txtNo" runat="server" BackColor="White" CssClass="clsTextBoxTagSearch"
                                                MaxLength="50" Text="<%# mRenewMachineCertificate.CertificateNo %>" ToolTip="Certificate Number"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIssueDate" runat="server" CssClass="clsLabelAuto" Width="70px">Issue Date</asp:Label>
                                        </td>
                                        <td>
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtIssueDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchDate"
                                                            onchange="ValidateDateText(this,'txtIssueDate_CalendarExtender');" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWE1" runat="server" TargetControlID="txtIssueDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblExpiryDate" runat="server" CssClass="clsLabelAuto" Width="70px">Expiry Date </asp:Label>
                                        </td>
                                        <td>
                                            <table cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td align="left">
                                                        <%--  <uc1:sicalendar id="calExpiryDate" runat="server"></uc1:sicalendar>--%>
                                                        <asp:TextBox ID="txtExpiryDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchDate"
                                                            onchange="ValidateDateText(this,'txtExpiryDate_CalendarExtender');" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtExpiryDate"
                                                            WatermarkText="<%$AppSettings:DateFormat%>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <%-- <span id="Label4" class="clsLabelStar" style="color: Red;">*</span>--%>
                                        </td>
                                        <td>
                                            <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                Text="<%# mRenewMachineCertificate.WarningDays %>" ToolTip="Enter Warning Days"
                                                MaxLength="4">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblEffectiveDate" class="clsLabelAuto">Effective Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="clsTextBoxTagSearchDate" onchange="ValidateDateText(this,'EffectiveDate_watermarkextender','true');"
                                                ></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtEffectiveDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                 Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                TargetControlID="txtEffectiveDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender ID="EffectiveDate_watermarkextender" runat="server"
                                                ClientIDMode="Static" TargetControlID="txtEffectiveDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mRenewMachineCertificate.Remark %>"
                                               width="570px" ToolTip="Enter Remark" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
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
                                                                <input type="button" runat="server" id="btnSelectFile" value="Select File" class="clsbtnH clsinfoH1" />
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" class="clsbtnH clsinfoH1" Enabled="False"
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
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblApplicable" runat="server" CssClass="clsLabelAuto">Applicable </asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip="Check to apply Applicability"
                                                Text="(Check if Certificate is Applicable)" Checked="<%# mRenewMachineCertificate.IsApplicable %>">
                                            </asp:CheckBox>
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
                <%--<td align="right" colspan="4">
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save the Certificate">
                                        </asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                            CausesValidation="False" ToolTip="Click to Print the list of Certificates" Visible="False">
                                        </asp:Button>
                                    </td>
                                      <td>
                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Send Mail" Enabled="<%# NOT mRenewMachineCertificate.IsNew %>"
                                            ToolTip="Click to report by mail" Width="96px" />
                                    </td>
                                    <td class="style3">
                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"
                                            CausesValidation="False" ToolTip="Click to go Previous page"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                  <!--Dummy panel to open modelpopup-->
                <td style="height: 0px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                        <ContentTemplate>
                            
                            <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <!--End -->
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
    </div>
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
            width: 100%;">
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
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");
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
                parent.ParentCallBackFunctionForMachineCertificate();
                return false;
            }
        </script>
        <%--End--%>
    </div>
    <div>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
            SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameMachineCertificateStateComplete();
                }
       
      
        });
        <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
                    
            }

            function SetPageLayout()
            {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
                <% End if %>
            }
            function ReSetPageLayout()
            {
            $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
                var windowheight=$(window).height();
                if (tempMargtop>=windowheight)
                {
                $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
                }
                else
                {
                var margintop=(windowheight/2)-(tempMargtop/2);
                $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                }
       
            }
        </script>
    </div>
    <%--End--%>
      <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>
