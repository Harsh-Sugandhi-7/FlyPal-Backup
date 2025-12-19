<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComplyCalibrationItem_Ajax.aspx.vb"
    Inherits="Flypal.wfComplyCalibrationItem_Ajax" %>


<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calibration Item Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">

        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
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
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
            </td>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Calibration Item [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlActionBtn" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" Text="Save &amp; Close" ToolTip="Click to save Calibration Item"
                                                                    CssClass="clsbtnH clsinfoH" CausesValidation="true"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrintSticker" runat="server" Text="Print Sticker" ToolTip="Click to Print Sticker"
                                                                    CssClass="clsbtnH clsinfoH" Visible='<%#IIf(AppSettings("ClientCode") = "IRMI", True, False) %>'
                                                                    CausesValidation="true"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" Text="Back" ToolTip="Click to go back to previous page"
                                                                    CssClass="clsbtnH clsinfoH" CausesValidation="False"></asp:Button>
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
                                <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" OnServerValidate="CustomValidate"
                                            ValidateEmptyText="true" Display="None" ControlToValidate="txtDoneOnDate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ErrorMessage="Calibration No. should not be greater than 50 characters."
                                            ControlToValidate="txtNo"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDoneByAgency" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ErrorMessage="Done By Agency name should not be greater than 150 characters."
                                            ControlToValidate="txtDoneByAgency"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCertRef" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ErrorMessage="Certificate Reference should not be greater than 100 characters."
                                            ControlToValidate="txtCertRef"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ErrorMessage="Remark should not be greater than 1000 characters."
                                            ControlToValidate="txtRemark"></asp:CustomValidator>
                                        <script type="text/javascript">

                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtDoneByAgency':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 150) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;

                                                    case 'txtCertRef':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 100) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtRemark':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 1000) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 50) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                }

                                            }
                                             
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlCalibrationDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset style="top: 8px; left: 3px" class="clsFieldSetNewStyle">
                                            <legend><b>Calibration Item Details</b> </legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Label1" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPartNo" runat="server" Text="<%# mCalibrationItemchild.ItemName %>"
                                                            ToolTip="Part No." BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" Text="<%# mCalibrationItemChild.SerialNo %>"
                                                            ToolTip="Serial No." BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDesc" class="clsLabelAuto">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" Text="<%# mCalibrationItemChild.Description %>"
                                                          ToolTip="Description" BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblLocation" class="clsLabelAuto">Location</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocation" runat="server" Text="<%# mCalibrationItemChild.Location %>"
                                                            ToolTip="Description" BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxTagSearch">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                     <td>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabelAuto">Manufacturing Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="clsTextBoxTagSearchDate" Text="<%# mCalibrationItemChild.ManufacturingDateFormatted %>"
                                                            MaxLength="50" BackColor="#E0E0E0" ReadOnly="True">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblFrequency" class="clsLabelAuto">Frequency</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFrequency" runat="server" Text="<%# mCalibrationItemChild.CalibrationItemChildFrequency %>"
                                                            ReadOnly="true" BackColor="#E0E0E0" ClientIDMode="Static" ToolTip="Frequency"
                                                            AutoPostBack="true" CssClass="clsTextBoxTagSearchSmall">
                                                        </asp:TextBox><asp:Label ID="lblMonths" runat="server" CssClass="clsLabelAuto" Text="<%# mCalibrationItemChild.CalibrationPeriodIn %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblCalNo" class="clsLabelAuto">Calibration No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" Text="<%# mCalibrationItemChild.CalibrationNo %>"
                                                            ToolTip="Enter Calibration No" BackColor="White" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox><asp:CustomValidator
                                                                ID="cvCalibrationNo" runat="server" OnServerValidate="CustomValidate" Display="None"
                                                                ErrorMessage="Max. Length should be 50." ControlToValidate="txtNo"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Label7" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDoneOnDate" class="clsLabelAuto">Done On Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDoneOnDate" CssClass="clsTextBoxTagSearchDate"
                                                            AutoPostBack="true" OnTextChanged="txtDoneOnDate_TextChanged" onchange="ValidateDateText(this,'DoneOnDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDoneOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDoneOnDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDoneOnDate" ID="DoneOnDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblIsApplicable" class="clsLabelAuto">Applicable</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" CssClass="clsCheckBox" Checked="<%# mCalibrationItemChild.IsApplicable %>"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblNextDueDate" class="clsLabelAuto">Next Due Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNextDueDate" runat="server" Text="<%# mCalibrationItemChild.NextDueDate %>"
                                                            BackColor="Gainsboro" ReadOnly="True" CssClass="clsTextBoxTagSearchDate" MaxLength="50">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDonebyAgency" class="clsLabelAuto">Done by Agency</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDoneByAgency" runat="server" Text="<%# mCalibrationItemChild.DonebyAgency %>"
                                                            ToolTip="Enter Agency Name" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblCertRef" class="clsLabelAuto">Certificate Reference</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCertRef" runat="server" Text="<%# mCalibrationItemChild.CertificateReference %>"
                                                            ToolTip="Enter Certificate Reference" CssClass="clsTextBoxSearch_Ajax"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Label5" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" Text="<%# mCalibrationItemChild.Remark %>"
                                                            ToolTip="Enter Remark" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="1000"
                                                            TextMode="MultiLine" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" tabindex="13" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" TabIndex="14"></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px" TabIndex="15"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtn" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save &amp; Close" ToolTip="Click to save Calibration Item"
                                                        CssClass="clsbtnH clsinfoH" CausesValidation="true"></asp:Button>
                                                </td>
                                                 <td>
                                                    <asp:Button ID="btnPrintSticker" runat="server" Text="Print Sticker" ToolTip="Click to Print Sticker"
                                                        CssClass="clsbtnH clsinfoH" Visible='<%#iif(AppSettings("ClientCode") = "IRMI",True,False) %>'
                                                        CausesValidation="true"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" Text="Back" ToolTip="Click to go back to previous page"
                                                        CssClass="clsbtnH clsinfoH" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <tr style="height: 0px;">
                            <td>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

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
    <!-- End -->
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForCalibrationItem();
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
                    parent.IFrameCalibrationItemStateComplete();
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
    <%--End--%>
    </form>
</body>
</html>
