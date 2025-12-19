<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfGroupTrainingConfiguration_Ajax.aspx.vb"
    Inherits="Flypal.wfGroupTrainingConfiguration_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Group Employee Training Allocation</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <style type="text/css">
        .GbiHighlight
        {    
            background-color: Teal;
        }
    </style>
    <!-- End-->
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Group Employee Training Allocation</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <%--AJAX Update Panel --%>
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                                    Text="Allocate" ToolTip="Click to Allocate Training for selected Employee(s)"
                                                                    ValidationGroup="valGroup1" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go to the Previous Page" />
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
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select at least One Employee from list."
                                            ControlToValidate="txtTrainingType" Display="None" ClientValidationFunction="validateEmp"
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                      <%--  <asp:CustomValidator ID="cvTrainingOrgName" runat="server" ErrorMessage="Please select the Training Organization."
                                            ControlToValidate="cmbTrainingOrgList" Display="None" 
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Remark should not be greater than 255 characters."
                                            ControlToValidate="txtRemark" Display="None" ClientValidationFunction="validateRemark"
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!-- Client side validation for comboboxes CHK if script executes for posted back to server everytime-->
                                <script type="text/javascript">
                                    //Training Org Name
//                                    function validateTrainingOrgName(source, args) {
//                                        args.IsValid = false;
//                                        var dd = $get("cmbTrainingOrgList");
//                                        if (dd.selectedIndex != 0) {
//                                            args.IsValid = true;
//                                            return;

//                                        }
//                                    }

                                    function validateEmp(source, args) {
                                        args.IsValid = false;
                                        var NoOfEmp = $("#chkEmployeeList input:checked").length;
                                        if (NoOfEmp > 0) {
                                            args.IsValid = true;
                                            return;

                                        }
                                    }

                                    function validateRemark(source, args) {
                                        var Value = $get("txtRemark").value.length;
                                        if (Value > 255) {
                                            args.IsValid = false;
                                            return
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTrainingDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsTrainingInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="lblTrainingDetails" runat="server" style="font-weight: bold"><b>Training
                                                Details</b></legend>
                                            <table id="Table1" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblTrainingName" class="clsLabelAuto">Training Name </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Training Name"
                                                            Text="<%# mTraining.Name %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTrainingType" runat="server" CssClass="clsLabelAuto">Training Type  </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTrainingType" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Training Type"
                                                            Text="<%# mTraining.TrainingTypeName %>" BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblRecurringStatus" class="clsLabel">Recurring Status </span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkRecurringStatus" runat="server" CssClass="clsCheckBox" ToolTip="Check this in case the Training is Recurring"
                                                            Text="(in case the Training is Recurring)" Checked="<%# mTraining.RecurringStatus %>"
                                                            Enabled="false"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblFreqInMonths" class="clsLabelAuto">Freq In Months </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFreqInMonths" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Enter Freq In Month" Text="<%# mTraining.FreqInMonths %>" MaxLength="5"
                                                            BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                            ToolTip="Enter Warning Days" Text="<%# mTraining.WarningDays %>" MaxLength="5"
                                                            BackColor="#E0E0E0" ReadOnly="true"></asp:TextBox>
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
                                <asp:UpdatePanel ID="upnlRenewalInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="fdsRenewalInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <legend id="ldgRenewalInfo" runat="server" style="font-weight: bold"><b>Training Applicability
                                                Details</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="Span1" class="clsLabelStar" style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span2" class="clsLabelAuto">Employees</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="clsLabelAuto">Designation</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearchDesignation" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                    ClientIDMode="Static" OnTextChanged="txtSearchDesignation_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearchDesignation_AutoCompleteExtender"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                    MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmpDesgList"
                                                                                    TargetControlID="txtSearchDesignation" UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                                </cc2:AutoCompleteExtender>
                                                                            </td>
                                                                            <td>
                                                                                <span class="clsLabelAuto">Name</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearchEmpName" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                                                    ClientIDMode="Static" OnTextChanged="txtSearchEmpName_TextChanged"></asp:TextBox>
                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtSearchEmpName_AutoCompleteExtender"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                    MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetEmpList"
                                                                                    TargetControlID="txtSearchEmpName" UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                                </cc2:AutoCompleteExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="25px">
                                                                                <input type="checkbox" style="vertical-align: bottom;" id="chkSelectAllEmp" />
                                                                            </td>
                                                                            <td width="100%">
                                                                                <asp:Panel ID="CpnlEmpList" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                                    <div style="float: left; vertical-align: middle;">
                                                                                        <span id="lblEmpList" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">
                                                                                            Employee List</span>
                                                                                    </div>
                                                                                    <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                        <image id="imgbtnClpnl" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                                    </div>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:Panel ID="pnlEmpList" runat="server" ClientIDMode="Static" Visible="true">
                                                                                    <asp:CheckBoxList ID="chkEmployeeList" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                                        ClientIDMode="Static" DataValueField="ID" DataTextField="Name" RepeatColumns="5"
                                                                                        RepeatDirection="Horizontal">
                                                                                    </asp:CheckBoxList>
                                                                                </asp:Panel>
                                                                                <cc2:CollapsiblePanelExtender ID="clpEmpList" runat="Server" BehaviorID="clpEmpListBehaviour"
                                                                                    ClientIDMode="Static" CollapseControlID="CpnlEmpList" Collapsed="True" CollapsedImage="~/images/expand_blue.jpg"
                                                                                    CollapsedText="(Show Details...)" ExpandControlID="CpnlEmpList" ExpandedImage="~/images/collapse_blue.jpg"
                                                                                    ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl" SkinID="CollapsiblePanelDemo"
                                                                                    SuppressPostBack="false" TargetControlID="pnlEmpList" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            <%--    <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblCertificateNo" class="clsLabelAuto">Certificate No</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCertificateNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="20"
                                                            ToolTip="Enter Certificate No">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td align="center">
                                                      <%--  <span id="Label2" class="clsLabelStar" style="color: Red">*</span>--%>
                                                    </td>
                                                    <td>
                                                        <%--<span id="Label17" class="clsLabelAuto">Date</span>--%>
                                                    </td>
                                                    <td>
                                                        <!-- CHK if seperate update panel is required -->
                                                        <%--<asp:TextBox ID="txtDate" runat="server" AutoPostBack="true" CausesValidation="true"
                                                            ClientIDMode="Static" CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this,'txtDate_CalendarExtender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="Calender_watermarkextender" runat="server" TargetControlID="txtDate"
                                                            WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>--%>
                                                    </td>
                                                   <%-- <td>
                                                        <span id="lblDuration" class="clsLabelAuto">Duration</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDuration" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                            MaxLength="2" ToolTip="Enter Duration">
                                                        </asp:TextBox>
                                                        <span id="lblInmonth" class="clsLabelAuto">(In Days)</span>
                                                    </td>--%>
                                                </tr>
                                              <%--  <tr>
                                                    <td align="center">
                                                        <!-- CHK if seperate update panel is required -->
                                                     
                                                    </td>
                                                    <td>
                                                        <span id="lblTrainingOrgName" class="clsLabelAuto">Training Org Name</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="cmbTrainingOrgList" runat="server" CssClass="clsComboBox_Ajax"
                                                            DataTextField="NameWithCity" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblMonthOfTraining" class="clsLabelAuto">Month of Training</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbMonthList" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblYearOfTraining" class="clsLabelAuto">Year of Training</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtYearOfTraining" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                            ToolTip="Enter Year of Training" MaxLength="4">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine3_Ajax" ClientIDMode="Static"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td class="clsInnerTable">
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" runat="server" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                class="clsButton_Ajax" causesvalidation="False" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                Enabled="false" Text="Remove Attachment" Width="140px"></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Visible="false" Height="20px" Width="20px"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>--%>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <%-- AJAX Update Panel 
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="clsbtnH clsinfoH"
                                                        Text="Allocate" ToolTip="Click to Allocate Training for selected Employee(s)"
                                                        ValidationGroup="valGroup1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go to the Previous Page" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <!--Dummy panel to open modelpopup-->
        <tr style="height: 0px;">
            <td style="height: 0px;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                    <ContentTemplate>
                        <asp:Button ID="hdnBtnEmpTrainingHistory" ClientIDMode="Static" runat="server" Text="Add"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpTraining();
            return false;
        }
    </script>
    <%--End--%>
    <!--Set page layout when open as popup aspx page-->
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameEmployeeTrainingStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <!-- End-->
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
    <script type="text/javascript">
        //check all/ uncheck all checkbox of aircraft list
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAllEmp").click(function () {
                var status = $("#chkSelectAllEmp").attr("checked");
                $("#chkEmployeeList").find(":checkbox").each(function () {
                    var enableStatus = $(this).attr("disabled");
                    if (!enableStatus) {
                        if (status == "checked") {
                            $(this).attr("checked", status);
                        }
                        else {
                            $(this).removeAttr("checked");
                        }
                    }
                });
            });
        });
    </script>
    </form>
</body>
</html>
