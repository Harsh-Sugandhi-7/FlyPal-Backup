<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRecordUpdate_Ajax.aspx.vb"
    Inherits="Flypal.wfRecordUpdate_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <table class="clstablelistin" id="tblInner">
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="upnlUdateRecord" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="4" class="clsFormHeader1Newstyle">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblTitle" class="clsFormHeader">Update Record</span>
                                                        </td>
                                                        <td align="right">
                                                            <table id="Table4" cellspacing="1" cellpadding="1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnLocationOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Ok" ToolTip="Click to Update"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnLocationClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                            Text="Close" CausesValidation="False" ToolTip="Click to Close"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="1" runat="server"
                                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true"
                                                    ErrorMessage="Enter Remark" CssClass="clsLabelAuto" ControlToValidate="txtRemark"
                                                    Display="None" ValidationGroup="1" runat="server" />
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="1" Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkList"
                                                    ClientIDMode="Static" ErrorMessage="Select Consider As Asset."></asp:CustomValidator>
                                                <script type="text/javascript">
                                                    function ValidateChkList(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("ChkIsConsiderAsAsset");
                                                        if (dd.checked) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                </script>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblConsiderAsAssetStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblConsiderAsAsset" class="clsLabel">Consider As Asset</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="ChkIsConsiderAsAsset" runat="server" CssClass="clsCheckBox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblRemarkStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblRemark" class="clsLabel">Remark</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" MaxLength="500"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnSelectFile" value="Select File" 
                                                                runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" />
                                                        </td>
                                                        <td style="padding-left: 3px;">
                                                            <asp:Button ID="btnDelAttach" runat="server" CausesValidation="false" CssClass="clsbtnH clsinfoH1"
                                                                Enabled="False" Text="Remove Attachment" ToolTip="Click to Remove Attachment"
                                                                 />
                                                        </td>
                                                        <td style="padding-left: 2px;">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                           <%-- <td valign="top" align="right" colspan="4">
                                                <table id="Table4" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnLocationOk" ValidationGroup="1" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Ok"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnLocationClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="height: 0px;">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
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

        function OpenFileUploadWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                //                if (!$.browser.msie) {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = "hidden";
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
    <!-- End -->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForGROOutrightConversion();
            return false;
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
        SetPageLayout();
            if ($.browser.msie) {
                parent.IFrameGROOutrightConversionStateComplete();
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
    </form>
</body>
</html>
