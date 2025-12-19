<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeLeaves_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeLeaves_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Leave</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
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
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" TabIndex="1" runat="server" CssClass="clsFormHeader">Employee Leave [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Leave Information"
                                                                    ValidationGroup="valGroup1"></asp:Button>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
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
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvClassification" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="cmbClassificationList" Display="None" ClientValidationFunction="validateClassification"
                                            ErrorMessage="Please Select the Classification." ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNoteLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Note cannot be greater than 500 characters."
                                            Display="None" ControlToValidate="txtNote" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNoOfDays" runat="server" CssClass="clsLabelAuto" ErrorMessage="No Of Days Required."
                                            Display="None" ControlToValidate="txtNoOfDays" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1" ValidateEmptyText="true"></asp:CustomValidator>
                                        <%--<asp:RequiredFieldValidator ID="rfvNoOfDays" runat="server" CssClass="clsLabelAuto"
                                            ControlToValidate="txtNoOfDays" Display="None" ErrorMessage="No Of Days Required."
                                            ValidationGroup="valGroup1" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="calFromDate"
                                            ErrorMessage="From Date should not be blank." ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="calFromDate" ErrorMessage="From Date should not be blank."
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Rejoining Date should be greater than From Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <!-- Client side validation for comboboxes-->
                                        <script type="text/javascript">
                                            //Classification
                                            function validateClassification(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbClassificationList");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;

                                                }
                                            }

                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtNote':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 500) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtNoOfDays':
                                                        var Value = $get(ControlName).value;
                                                        if (Value == "" || Value == "0") {
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
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlLeaveDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <span id="lblSkillDetails" class="clsLabelHeader">Employee Leave Record Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                </td>
                                                <td colspan="3" align="left">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxSearch_Ajax"
                                                        Text="<%# mEmployee.Name %>" BackColor="#E0E0E0" ReadOnly="True" ToolTip="Employee Name"
                                                        MaxLength="25">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Label4" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblClassification" class="clsLabelAuto">Classification</span>
                                                </td>
                                                <td colspan="3">
                                                    <table id="Table5" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbClassificationList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployeeLeave.ClassificationID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgClassification" runat="server" CssClass="clsButtonGrid_Ajax" ToolTip="Click to Add New Classification"
                                                                    Text="..." CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgClassification" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                      Width="24px" ToolTip="Click to Add New Classification" CausesValidation="False"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Label2" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="calFromDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                        runat="server" onchange="ValidateDateText(this,'calFromDate_watermarkextender');"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calFromDate" ID="calFromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table7" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar" ForeColor="Red" Visible="False">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <table id="Table41" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="calToDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                                runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                Visible="false"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="calToDate" ID="calToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblNoOfDays" class="clsLabelAuto">No Of Days</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtNoOfDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mEmployeeLeave.NoOfDays %>"
                                                        MaxLength="6">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label5" class="clsLabelAuto">Re-Joining Date</span>
                                                </td>
                                                <td colspan="3">
                                                    <table id="Table6" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="calReJoiningDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                    runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="calReJoiningDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calReJoiningDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="calReJoiningDate" ID="calReJoiningDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblNote" class="clsLabelAuto">Note</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                        Text="<%# mEmployeeLeave.Note %>" ToolTip="Enter Note" MaxLength="500" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="clsInnerTable">
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td colspan="3">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                    class="clsbtnH clsinfoH1">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CausesValidation="false" CssClass="clsbtnH clsinfoH1"
                                                                    ToolTip="Click to Remove Attachment" Text="Remove Attachment" Enabled="False"
                                                                    Width="140px"></asp:Button>
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
                            <%--<td colspan="5" align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Leave Information"
                                                        ValidationGroup="valGroup1"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="False"></asp:Button>
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
            parent.ParentCallBackFunctionForEmpLeave();
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
                parent.IFrameEmpLeaveStateComplete();
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
    <!-- Leave Master --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyLeaveMaster" Text="Dummy Leave Master" />
    </div>
    <asp:Panel runat="server" ID="pnlLeaveMaster" Style="display: none">
        <div>
            <table class="clstablelistout" id="TABLE2">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlLeaveMaster" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="TABLE3" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitleLeaveMaster" TabIndex="1" CssClass="clsFormHeader" runat="server">Classification Information [New]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlClassificationPopupSave" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table2" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnNewLeaveMaster" CssClass="clsbtnH clsinfoH" runat="server" Text="New"
                                                                                ToolTip="Click to Add the Classification" CausesValidation="False"></asp:Button>

                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnSaveLeaveMaster" CssClass="clsbtnH clsinfoH" runat="server" Text="Save"
                                                                                ToolTip="Click to Save Classification Information"></asp:Button>

                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseLeaveMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                Text="Close" ToolTip="Click to close Classification Information screen" CausesValidation="False"></asp:Button>

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
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Classification Required"
                                                Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDocument" runat="server" CssClass="clsLabelAuto" ErrorMessage="Classification Name too Long."
                                                Display="None" ControlToValidate="txtName" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            
                                        </td>
                                        <%--<td align="right">
                                            <asp:UpdatePanel ID="upnlClassificationPopupSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnNewLeaveMaster" CssClass="clsButton_Ajax" runat="server" Text="New"
                                                                    ToolTip="Click to Add the Classification" CausesValidation="False"></asp:Button>

                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSaveLeaveMaster" CssClass="clsButton_Ajax" runat="server" Text="Save"
                                                                    ToolTip="Click to Save Classification Information"></asp:Button>

                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseLeaveMaster" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                    Text="Close" ToolTip="Click to close Classification Information screen" CausesValidation="False"></asp:Button>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>



                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblDocumentDetails" class="clsLabelHeader">Classification Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table width="100%">
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <span id="Label6" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td valign="middle">
                                                        <span id="lblName" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mClassification.Name %>"
                                                            ToolTip="Enter Classification Name" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            
                                        </td>
                                        <%--<td align="right">
                                            <asp:Button ID="btnSaveLeaveMaster" CssClass="clsButton_Ajax" runat="server" Text="Save"
                                                ToolTip="Click to Save Classification Information"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Classification List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                           <%-- <div style="width: 490px;">
                                                <table cellpadding="0" cellspacing="0" class="clsGridNewStyle" style="width: 490px; border-collapse: collapse;">
                                                    <tr>
                                                        <td class="clsdgHeader" width="370px">
                                                            <span>Name</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="120px">
                                                            <span>Action</span>
                                                        </td>
                                                        
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 511px;">
                                                <asp:GridView ID="dgClassification" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 490px;" DataKeyNames="ID"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="370px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>
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
                                                        </asp:TemplateField>--%>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <div id="dropDownImg" class="dropdown">
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" Style="cursor: pointer"/>
                                                                    <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                        <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="editICN" Style="height: 15px; width: 15px" runat="server"
                                                                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                        ToolTip="Click to Edit record"
                                                                                        CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                        ToolTip="Click to Delete record"
                                                                                        CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td align="right" colspan="4">
                                            <table id="Table4" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                                <tr>
                                                    <td valign="bottom" align="right">
                                                        <asp:Button ID="btnCloseLeaveMaster" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip="Click to close Classification Information screen" CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpLeaveMaster" runat="server" TargetControlID="btnDummyLeaveMaster"
        PopupControlID="pnlLeaveMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            var status = Sys.Extended.UI.TextBoxWrapper.get_Wrapper($get("calReJoiningDate"))._isWatermarked;
            if (!status) {

                args.IsValid = false;
                var fromdate = $("#calFromDate").val();
                var todate = $("#calReJoiningDate").val();
                if (!todate) {
                    rfvToDate.isvalid = true;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
                    return;
                }
                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }
            }

        }


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
    </form>
</body>
</html>
