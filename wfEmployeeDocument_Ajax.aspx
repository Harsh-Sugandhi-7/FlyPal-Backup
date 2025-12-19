<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDocument_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDocument_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server"> 
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Document</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
             
        }     
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" TabIndex="1" runat="server" CssClass="clsFormHeader">Employee Document Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlSave" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Document Information"
                                                                    Text="Save" ValidationGroup="valGroup1"></asp:Button>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
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
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvDocumentName" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbDocumentList"
                                            Display="None" ClientValidationFunction="ValidateDocumentList" ErrorMessage="Please select the Document."
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <%--<asp:CustomValidator ID="cvDateOfIssue" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtIssueDate"
                                            Display="None" OnServerValidate="CustomValidate" ValidationGroup="valGroup1"></asp:CustomValidator>--%>
                                        <%--<asp:CustomValidator ID="cvDateOfExpiry" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtExpiryDate"
                                            Display="None" OnServerValidate="CustomValidate" ValidationGroup="valGroup1"></asp:CustomValidator>--%>
                                        <asp:RequiredFieldValidator ID="rfvDocumentNo" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtDocumentNo" Display="None" ErrorMessage="Document No Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDateOfIssue" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtIssueDate" Display="None" ErrorMessage="Date of Issue Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <%--<asp:RequiredFieldValidator ID="rfvDateOfExpiry" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtExpiryDate" Display="None" ErrorMessage="Date of Expiry Required"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>--%>
                                        <asp:CustomValidator ID="cvValidity" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtValidity"
                                            Display="None" ErrorMessage="Enter Validity" OnServerValidate="CustomValidate"
                                            ValidateEmptyText="true" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvWarningDays" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtWarningDays"
                                            Display="None" ErrorMessage="Enter Warning Days" OnServerValidate="CustomValidate"
                                            ValidateEmptyText="true" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtApplicabilityRemark" Display="None" ErrorMessage="" OnServerValidate="CustomValidate"
                                            ValidateEmptyText="true" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <!-- Client side validation for comboboxes-->
                                        <script type="text/javascript">
                                            //Nomenclature
                                            function ValidateDocumentList(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbDocumentList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;

                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDocumentDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblDocumentDetails" class="clsLabelHeader">Employee Document Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxSearch_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" ToolTip="Employee Name" MaxLength="25" Text="<%# mEmployee.Name %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDocument" class="clsLabelAuto">Document</span>
                                                </td>
                                                <td>
                                                    <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:DropDownList ID="cmbDocumentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mEmployeeDocument.DocumentID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="right">
                                                                <%--<asp:Button ID="imgDocument" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Document"
                                                                    Text="..." CausesValidation="False"></asp:Button>--%>
                                                                <asp:ImageButton ID="imgDocument" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                    Width="24px" ToolTip="Click to Add New Document" CausesValidation="True"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDocumentNo" class="clsLabelAuto">Document No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Document No"
                                                        MaxLength="25" Text="<%# mEmployeeDocument.DocNo %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label2" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDateOfIssue" class="clsLabelAuto">Date Of Issue</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIssueDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                        runat="server" AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDateOfIssue_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtIssueDate" ID="Calender_watermarkextender"
                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span1" class="clsLabelAuto">Applicable</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkApplicable" runat="server" AutoPostBack="true" ToolTip="Check this checkbox if Document is Applicable"
                                                        CssClass="clsCheckBox" Checked="<%# mEmployeeDocument.IsApplicable %>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblApplicabilityStar" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                </td>
                                                <td>
                                                    <span id="Span2" class="clsLabelAuto">Not Applicable Remark</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtApplicabilityRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                        TextMode="MultiLine" Text="<%# mEmployeeDocument.ApplicabilityRemark %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span3" class="clsLabelAuto">One Time</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkOneTimeDocument" runat="server" ToolTip="Check this checkbox if Document is One Time"
                                                        AutoPostBack="true" CssClass="clsCheckBox" Checked="<%# mEmployeeDocument.OneTimeDocument %>"
                                                        Text="One Time Document (Then no need of Date Of Expiry else Date Of Expiry is compulsory)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPlaceOfIssue" class="clsLabelAuto">Place Of Issue</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPlaceOfIssue" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Place of Issue"
                                                        MaxLength="25" Text="<%# mEmployeeDocument.PlaceOfIssue %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblValidityStar" runat="server" class="clsLabelStar" style="color: Red;"
                                                        visible='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'>*</span>
                                                </td>
                                                <td>
                                                    <span id="lblValidity" class="clsLabelAuto">Validity</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValidity" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"  Enabled='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'
                                                        ToolTip="Enter Validity" MaxLength="4" Text="<%# mEmployeeDocument.Validity %>"
                                                        AutoPostBack="True">
                                                    </asp:TextBox>
                                                    <asp:DropDownList ID="cmbDocumentValidityIn" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                        AutoPostBack="true" DataValueField="ID" ClientIDMode="Static" DataTextField="Name" Enabled='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'
                                                        SelectedValue="<%# mEmployeeDocument.DocumentValidityInID %>" Width="80px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblDateOfExpiryStar" runat="server" class="clsLabelStar" style="color: Red;"
                                                        visible='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'>*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDateOfExpiry" class="clsLabelAuto">Date Of Expiry</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExpiryDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static" Enabled='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'
                                                        runat="server" AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtExpiryDate" ID="TextBoxWatermarkExtender1"
                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblIssueingAuthority" class="clsLabelAuto">Issuing Authority</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIssuingAuthority" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Issuing Authority"
                                                        MaxLength="25" Text="<%# mEmployeeDocument.IssuingAuthority %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblWarningDaysStar" runat="server" class="clsLabelStar" style="color: Red;"
                                                        visible='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'>*</span>
                                                </td>
                                                <td>
                                                    <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Enabled='<%# iif(mEmployeeDocument.OneTimeDocument,False,True) %>'
                                                        ToolTip="Enter Warning Days" MaxLength="4" Text="<%# mEmployeeDocument.WarningDays %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxSearch_Ajax" ToolTip="Enter Remark"
                                                        Text="<%# mEmployeeDocument.Remark %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="clsInnerTable">
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                    class="clsbtnH clsinfoH1">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                            </td>
                                                            <td style="padding-left: 2px;">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlSave" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Document Information"
                                                        Text="Save" ValidationGroup="valGroup1"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpDocument();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
        var query  = window.location.search.substring(1);
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
            SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameEmpDocumentStateComplete();
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
                    $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                    //                        $("#IFileUpload").ready(function () {
                    //                            $("#btnDummyFileUpload").click();
                    //                            $get("AjaxLoader").style.visibility = 'hidden';
                    //                        });
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
    <!-- Document Master --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyDocumentMaster" Text="Dummy Document Master" />
    </div>
    <asp:Panel runat="server" ID="pnlDocumentMaster" Style="display: none">
        <div>
            <table class="clstablelistout" id="TABLE3">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlDocumentMaster" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="TABLE4">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblTitleDocumentMaster" TabIndex="1" CssClass="clsFormHeader" runat="server">Document Information [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGroup2"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDocumentName"
                                                Display="None" ErrorMessage="Document Required" ValidationGroup="valGroup2">Document Required</asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDocument" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDocumentName"
                                                Display="None" ErrorMessage="Document Name too Long." OnServerValidate="Customvalidate1"
                                                ValidationGroup="valGroup2"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>--%>
                                        <%--<td align="right">
                                            <asp:Button ID="btnNewDocumentMaster" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                ToolTip="Click to Add the Document" Text="New"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="Label7" class="clsLabelHeader">Document Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center">
                                            <span id="Label8" class="clsLabelStar" style="color: Red;">*</span>
                                        </td>
                                        <td>
                                            <span id="lblName" class="clsLabelAuto">Name</span>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtDocumentName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Document Name"
                                                Text="<%# mDocument.Name %>" MaxLength="25">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                       <%-- <td colspan="3">
                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>--%>
                                        <%--<td align="right">
                                            <asp:Button ID="btnSaveDocumentMaster" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Document Information"
                                                Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Document List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 310px;">
                                                <table cellpadding="0" cellspacing="0" class="clsGrid" style="width: 310px; border-collapse: collapse;">
                                                    <tr>
                                                        <td class="clsGridNewStyle" width="190px">
                                                            <span>Name</span>
                                                        </td>
                                                        <td class="clsGridNewStyle" width="70px">
                                                            <span>Edit/View</span>
                                                        </td>
                                                        <td class="clsGridNewStyle" width="50px">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div >
                                                <asp:GridView ID="dgDocument" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 310px;" DataKeyNames="ID"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    AllowPaging="true" PageSize="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" 
                                                        Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns> 
                                                        <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="190px" Wrap="true" />
                                                        </asp:BoundField>
                                                       <%-- <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:ButtonField>--%>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table width="100%">
                                                <tr>                                                    
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnNewDocumentMaster" CssClass="clsbtnH clsinfoH1" runat="server" CausesValidation="False"
                                                                        ToolTip="Click to Add the Document" Text="New"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSaveDocumentMaster" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Document Information"
                                                                        Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseDocumentMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                        CausesValidation="False" ToolTip="Click to close Document Information screen"
                                                                        Text="Close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpDocumentMaster" runat="server" TargetControlID="btnDummyDocumentMaster"
        PopupControlID="pnlDocumentMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End -->
    </form>
</body>
</html>
