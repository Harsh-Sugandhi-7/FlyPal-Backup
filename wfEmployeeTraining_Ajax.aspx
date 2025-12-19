<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeTraining_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeTraining_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Training</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }      
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table class="clstablelistin" id="tblInner" border="0">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" TabIndex="1" CssClass="clsFormHeader" runat="server">Employee Training Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Training Information"
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
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvTrainingName" runat="server" ControlToValidate="cmbTrainingList"
                                            Display="None" ClientValidationFunction="validateTrainingName" ErrorMessage="Please select the Training."
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <%--  <asp:CustomValidator ID="cvTrainingOrgName" runat="server" ErrorMessage="Please select the Training Organization."
                                            ValidateEmptyText="true" ControlToValidate="cmbTrainingOrgList" Display="None"
                                            ClientValidationFunction="validateTrainingOrgName" ValidationGroup="valGroup1"
                                            CssClass="clsLabelAuto"></asp:CustomValidator>
                                       <asp:CustomValidator ID="cvDate" runat="server" ControlToValidate="txtDate" Display="None"
                                            OnServerValidate="CustomValidate" ErrorMessage="" ValidationGroup="valGroup1"
                                            CssClass="clsLabelAuto">
                                        </asp:CustomValidator>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <!-- Client side validation for comboboxes CHK if script executes for posted back to server everytime-->
                                <script type="text/javascript">
                                    //Training Name
                                    function validateTrainingName(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbTrainingList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;

                                        }
                                    }

//                                    //Training Org Name
//                                    function validateTrainingOrgName(source, args) {
//                                        args.IsValid = false;
//                                        var dd = $get("cmbTrainingOrgList");

//                                        if (dd.selectedIndex != 0) {
//                                            args.IsValid = true;
//                                            return;

//                                        }
//                                    }
                                   
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTrainingDetails" runat="server" UpdateMode="Conditional" >
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblTrainingDetails" class="clsLabelHeader">Employee Training Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.Name %>"
                                                        BackColor="#E0E0E0" ReadOnly="True" ToolTip="Enter Employee Name" MaxLength="25">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <fieldset id="fdsMonitoringDetails" class="clsFieldSetNewStyle" style="border-width: 1px;
                                                        position: relative">
                                                        <legend id="Legend1"><b>Training Details</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <!-- CHK if seperate update panel is required -->
                                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblTrainingName" class="clsLabelAuto">Training Name</span>
                                                                </td>
                                                                <td>
                                                                    <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:DropDownList ID="cmbTrainingList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                                    AutoPostBack="true" SelectedValue="<%# mEmployeeTraining.TrainingID %>" DataTextField="Name"
                                                                                    DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="right">
                                                                                <%--<asp:Button ID="imgTraining" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                                    ToolTip="Click to Add New Training" CausesValidation="False"></asp:Button>--%>

                                                                                <asp:ImageButton ID="imgTraining" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                                    ToolTip="Click to Add New Training" CausesValidation="False"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                          
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblRecurringStatus" class="clsLabel">Recurring Status </span>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkRecurringStatus" runat="server" CssClass="clsCheckBox" ToolTip="Check this in case the Training is Recurring"  Checked="<%# mEmployeeTraining.RecurringStatus %>"
                                                                        Text="(Check this in case the Training is Recurring)"  Enabled="false"></asp:CheckBox><%--Enabled="<%# not mEmployeeTraining.HistoryCount  %>"--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblFreqInMonths" class="clsLabelAuto">Freq In Months </span>
                                                                </td>
                                                                <td>
                                                                    <table id="Table19" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFreqInMonths" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mEmployeeTraining.FreqInMonths %>"
                                                                                    ToolTip="Enter Freq In Month" MaxLength="5" Enabled="false"><%--Enabled="<%# not mEmployeeTraining.HistoryCount  %>"--%>
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mEmployeeTraining.WarningDays %>"
                                                                                    ToolTip="Enter Warning Days" MaxLength="5" Enabled="false" ><%--Enabled="<%# not mEmployeeTraining.HistoryCount  %>"--%>
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCertificateNo" class="clsLabelAuto">Certificate No</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCertificateNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployeeTraining.CertificateNo %>"
                                                                        ToolTip="Enter Certificate No" MaxLength="25">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Is Training NOT Applicable?</span>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsNOTApplicable" runat="server" CssClass="clsCheckBox" ToolTip="Check this in case the Training is NOT Applicable"
                                                                        Checked="<%# mEmployeeTraining.IsNOTApplicable %>" Text="(Check this in case the Training is NOT Applicable)">
                                                                    </asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                </td>
                                                                <td>
                                                                    <span id="Label17" class="clsLabelAuto">Date</span>
                                                                </td>
                                                                <td>
                                                                    <!-- CHK if seperate update panel is required -->
                                                                    <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static" runat="server"
                                                                        CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Calender_watermarkextender"
                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDuration" class="clsLabelAuto">Training Duration</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDuration" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Text="<%# mEmployeeTraining.Duration %>" ToolTip="Enter Duration" MaxLength="4">
                                                                    </asp:TextBox>
                                                                    <span id="lblInmonth" class="clsLabelAuto"></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <!-- CHK if seperate update panel is required -->
                                                                   <%-- <span id="spnTrainingOrgName" runat="server" class="clsLabelStar" style="color: Red">
                                                                        *</span>--%>
                                                                </td>
                                                                <td>
                                                                    <span id="lblTrainingOrgName" class="clsLabelAuto">Training Org Name</span>
                                                                </td>
                                                                <td>
                                                                    <table id="Table4" border="0" cellspacing="1" cellpadding="1">
                                                                        <tr>
                                                                            <td align="right">
                                                                                <asp:DropDownList ID="cmbTrainingOrgList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    SelectedValue="<%# mEmployeeTraining.TrainingOrgID %>" DataTextField="NameWithCity"
                                                                                    DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="right">
                                                                                <%--<asp:Button ID="imgTrainingOrgName" runat="server" CssClass="clsButtonGrid_Ajax"
                                                                                    Text="..." ToolTip="Click to Add New Training Organizatin" CausesValidation="False">
                                                                                </asp:Button>--%>

                                                                                <asp:ImageButton ID="imgTrainingOrgName" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                                    ToolTip="Click to Add New Training Organizatin" CausesValidation="False"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblMonthOfTraining" class="clsLabelAuto">Month of Training</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMonthList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mEmployeeTraining.MonthOfTrainingID %>"
                                                                        DataTextField="Name" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblYearOfTraining" class="clsLabelAuto">Year of Training</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtYearOfTraining" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Text="<%# mEmployeeTraining.YearOfTraining %>" ToolTip="Enter Year of Training"
                                                                        MaxLength="4">
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
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mEmployeeTraining.Remark %>"
                                                                        ToolTip="Enter Remark">
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
                                                                    <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
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
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
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
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Training Information"
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
                                        <asp:Button ID="hdnimgbtnTrainingMaster" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgbtnTrainingOrgMaster" ClientIDMode="Static" runat="server"
                                            Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
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
            parent.ParentCallBackFunctionForEmpTraining();
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
                        parent.IFrameEmpTrainingStateComplete();
                    }
       
      
            });
            <% End if %>

				Sys.Application.add_load(function () {
					var prm = Sys.WebForms.PageRequestManager.getInstance();
					prm.add_pageLoaded(endRequestHandler);
				});


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
    <!-- Training Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTrainingMaster" Text="Dummy Training Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlTrainingMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeTrainingMaster" frameborder="0" height="100%" width="100%" allowtransparency="true"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTrainingMaster" runat="server" TargetControlID="btnDummyTrainingMaster"
        PopupControlID="pnlTrainingMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameTrainingMasterStateComplete() {
            $("#btnDummyTrainingMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenTrainingMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeTrainingMaster").attr("src", "wfTraining_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyTrainingMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForTrainingMaster() {
            var TrainingMasterwindow = $find("<%=mdlPopupTrainingMaster.ClientID %>");
            //close Training Master popup window
            TrainingMasterwindow.hide();
            //           release resources
            $("#IframeTrainingMaster").attr("src", "JavaScript:''");
            //call Training Master image button
            $("#hdnimgbtnTrainingMaster").click();
        }
    </script>
    <!-- End-->
    <!-- Training Org Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyTrainingOrgMaster" Text="Dummy TrainingOrg Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlTrainingOrgMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeTrainingOrgMaster" frameborder="0" height="100%" width="100%" allowtransparency="true"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupTrainingOrgMaster" runat="server" TargetControlID="btnDummyTrainingOrgMaster"
        PopupControlID="pnlTrainingOrgMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameTrainingOrgMasterStateComplete() {
            $("#btnDummyTrainingOrgMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenTrainingOrgMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeTrainingOrgMaster").attr("src", "wfTrainingOrg_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyTrainingOrgMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForTrainingOrgMaster() {
            var TrainingOrgMasterwindow = $find("<%=mdlPopupTrainingOrgMaster.ClientID %>");
            //close Training Org Master popup window
            TrainingOrgMasterwindow.hide();
            //           release resources
            $("#IframeTrainingOrgMaster").attr("src", "JavaScript:''");
            //call Training Org Master image button
            $("#hdnimgbtnTrainingOrgMaster").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
