<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHangarPlanning.aspx.vb"
    Inherits="Flypal.wfHangarPlanning" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Hangar</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="script/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="Script/calendar-en.min.js" type="text/javascript"></script>
    <link href="script/styles/calendar-blue.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script type="text/javascript" id="clientEventHandlersJS">
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
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            height: 25px;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server" name="Hangar">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" class="clstablelistin">
                                            <tr>
                                                <td>
                                                    <%-- <asp:Label ID="lbltitle" runat="server" Width="100%" CssClass="clstitle1">Hangar Planning</asp:Label>--%>
                                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Hangar Planning</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                        ValidationGroup="a" HeaderText="Fill Up The Following Fields" Width="100%"></asp:ValidationSummary>
                                                    <asp:RequiredFieldValidator ID="rTextNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="No. Required"
                                                        Display="None" ControlToValidate="txtNo" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ValidationGroup="a" ID="cvHangarTypeID" runat="server" ErrorMessage="Select Hangar from the list."
                                                        ControlToValidate="DropDownList1" Display="None" ClientValidationFunction="ValidateHangarList"
                                                        CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                                    <asp:CustomValidator ValidationGroup="a" ID="cvAircraftTypeID" runat="server" ErrorMessage="Select Aircraft from the list."
                                                        ControlToValidate="DropDownList2" Display="None" ClientValidationFunction="ValidateAircraftList"
                                                        CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="rdatetimefrom" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="From Date And Time Required" Display="None" ControlToValidate="Txtdatetimefrom"
                                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rdatetimeto" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="To Date And Time Required" Display="None" ControlToValidate="Txtdatetimeto"
                                                        ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ValidationGroup="a" ID="cvFromDate" runat="server" ErrorMessage="Enter From Date Time."
                                                        ControlToValidate="Txtdatetimefrom" Display="None" ClientValidationFunction="ValidateFromTime"
                                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                                    <asp:CustomValidator ValidationGroup="a" ID="cvToDate" runat="server" ErrorMessage="Enter To date Time."
                                                        ControlToValidate="Txtdatetimeto" Display="None" ClientValidationFunction="ValidateToTime"
                                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                                        ControlToValidate="txtText" ValidationGroup="a" ValidateEmptyText="true" Display="None"
                                                        ErrorMessage="" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark must not be greater than 50 characters."
                                                        Display="None" ControlToValidate="Txtattach" ClientValidationFunction="validateName"
                                                        ValidationGroup="a"></asp:CustomValidator>
                                                    <script type="text/javascript">
                                                        function validateName(source, args) {
                                                            var ControlName = source.controltovalidate;
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 50) {
                                                                args.IsValid = false;
                                                                return;
                                                            }
                                                        }
                                                        function ValidateAircraftList(source, args) {
                                                            args.IsValid = false;
                                                            var dd = $get("DropDownList2");
                                                            if (dd.selectedIndex != 0) {
                                                                args.IsValid = true;
                                                                return;
                                                            }
                                                        }
                                                        function ValidateHangarList(source, args) {
                                                            args.IsValid = false;
                                                            var dd = $get("DropDownList1");
                                                            if (dd.selectedIndex != 0) {
                                                                args.IsValid = true;
                                                                return;
                                                            }
                                                        }
                                                        function ValidateFromTime(source, args) {
                                                            var ControlName = source.controltovalidate;
                                                            var Value = $get(ControlName).value;
                                                            var ValueLen = $get(ControlName).value.length;
                                                            var substring = ":";
                                                            if (Value.indexOf(substring) == -1 || ValueLen == 0) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                        }
                                                        function ValidateToTime(source, args) {
                                                            var ControlName = source.controltovalidate;
                                                            var ValueLen = $get(ControlName).value.length;
                                                            var Value = $get(ControlName).value;
                                                            var substring = ":";
                                                            if (Value.indexOf(substring) == -1 || ValueLen == 0) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                        }
                                                        //                                                        function ValidateToTimeFormat(source, args) {
                                                        //                                                            var ControlName = source.controltovalidate;
                                                        //                                                            var ValueLen = $get(ControlName).value.length;
                                                        //                                                            var Value = $get(ControlName).value;
                                                        //                                                            var substring = ":";
                                                        //                                                            if (Value.indexOf(substring) != -1 || ValueLen != 0) {
                                                        //                                                                var regex = /^([0-1][0-9])\:[0-5][0-9]\s*[ap]m$/i;
                                                        //                                                                var match = Value.match(regex);
                                                        //                                                                if (match) {
                                                        //                                                                    var hour = parseInt(match[1]);
                                                        //                                                                    if (!isNaN(hour) && hour <= 11) {
                                                        //                                                                        args.IsValid = true;
                                                        //                                                                        return

                                                        //                                                                    }
                                                        //                                                                }
                                                        //                                                        
                                                        //                                                            }
                                                        //                                                        }
                                                        //                                                        
                                                       
                                                    </script>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlCityDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="Span5" class="clsLabelStar"></span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label6" Class="clsLabel" Width="98px">Planning Number</asp:Label>
                                                </td>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mhangar.Text %>"
                                                                    ToolTip="Enter Text" Height="16px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxSmall_Ajax" Text="<%# mhangar.No %>"
                                                                    ToolTip="Enter No.">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label2" Class="clsLabel" Width="98px">Hangar</asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="HHangerWithCity"
                                                                    DataValueField="HID" SelectedValue="<%# mhangar.HangarID %>" CssClass="clsComboBox_Ajax">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="AddHanger" runat="server" Text="..." CssClass="btn " Height="20px"
                                                                    ToolTip="Add Hangar" Width="30px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <span id="Span6" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label7" Class="clsLabel" Width="98px">Aircraft</asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="DropDownList2" runat="server" DataTextField="HAicraftWithModelSerialNo"
                                                                    DataValueField="HID" SelectedValue="<%# mhangar.AirCraftID %>" CssClass="clsComboBox_Ajax">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="AddAirCraft" runat="server" Text="..." CssClass="btn " Height="20px"
                                                                    ToolTip="Add Aircraft" Width="30px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span2" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label3" Class="clsLabel" Width="98px">FromDateTime</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtdatetimefrom" runat="server" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                        Text="<%# mhangar.HedatetimeromFormatted %>"></asp:TextBox>
                                                    <img src="icons/calender.png" alt="" id="c1" />
                                                </td>
                                                <td>
                                                    <span id="Span7" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label1" Class="clsLabel" Width="98px">ToDateTime</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtdatetimeto" runat="server" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                        Text="<%# mhangar.ToDateFormatted %>"></asp:TextBox>
                                                    <img src="icons/calender.png" alt="" id="c2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span4" class="clsLabelStar"></span>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="Label5" Class="clsLabel" Width="98px">Remark</asp:Label>
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="Txtattach" runat="server" CssClass="clsTextBox_Ajax" MaxLength="100"
                                                        ClientIDMode="Static" TextMode="MultiLine" Width="100%" Text="<%# mhangar.Hremark %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td colspan="4">
                                                    <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <input type="button" id="btnSelectFile" runat="server" value="Select File" style="width: 120px;"
                                                                            tooltip="Click to Upload File" class="clsButton_Ajax" clientidmode="Static" />
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                            Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="~/icons/CLIP01.ICO"
                                                                            Height="20px" Width="20px"></asp:ImageButton>
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
                                                            <asp:Button ID="hdnimgBtnATAChapter" ClientIDMode="Static" runat="server" Text="..."
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnimgbtnKit" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                                                Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnAircraftMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnHangerMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <!--End -->
                                            <tr>
                                                <td colspan="6" align="right">
                                                    <asp:Button ID="btnSave" ValidationGroup="a" runat="server" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to save Hangar Information" Text="Save"></asp:Button>
                                                    <asp:Button ID="Button3" ValidationGroup="a" runat="server" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to close" Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
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
            //            $get("AjaxLoader").style.visibility = 'hidden';
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    //                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                    //                        $("#IFileUpload").ready(function () {
                    //                            $("#btnDummyFileUpload").click();
                    //                            $get("AjaxLoader").style.visibility = 'hidden';
                    //                        });
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        //                        $get("AjaxLoader").style.visibility = 'hidden';
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
    <!-- AircraftMaster Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAircraftMaster" Text="Dummy AircraftMaster"
            ClientIDMode="Static" CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlAircraftMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeAircraftMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupAircraftMaster" runat="server" TargetControlID="btnDummyAircraftMaster"
        PopupControlID="pnlAircraftMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameAircraftMasterStateComplete() {
            $("#btnDummyAircraftMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenAircraftMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeAircraftMaster").attr("src", "wfHangarAircraftMaster.aspx?Type=pup");
                $('#IframeAircraftMaster').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyAircraftMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForAircraftMaster() {
            varAircraftMasterwindow = $find("<%=mdlPopupAircraftMaster.ClientID %>");
            //close AircraftMaster popup window
            varAircraftMasterwindow.hide();
            //           release resources
            $("#IframeAircraftMaster").attr("src", "JavaScript:''");
            //call AircraftMaster image button
            $("#hdnBtnAircraftMaster").click();
        }
    </script>
    <!-- End-->
    <!-- HangerMaster Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHangerMaster" Text="Dummy HangerMaster" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHangerMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHangerMaster" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHangerMaster" runat="server" TargetControlID="btnDummyHangerMaster"
        PopupControlID="pnlHangerMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHangerMasterStateComplete() {
            $("#btnDummyHangerMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenHangerMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHangerMaster").attr("src", "wfHangarPlanningHangarMaster.aspx?Type=pup");
                $('#IframeHangerMaster').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHangerMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForHangerMaster() {
            varHangerMasterwindow = $find("<%=mdlPopupHangerMaster.ClientID %>");
            //close HangerMaster popup window
            varHangerMasterwindow.hide();
            //           release resources
            $("#IframeHangerMaster").attr("src", "JavaScript:''");
            //call HangerMaster image button
            $("#hdnBtnHangerMaster").click();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForHanger();
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
             parent.IFrameHangerStateComplete();
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
          //onResize();//for Top bottom link
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#Txtdatetimefrom,#Txtdatetimeto").dynDateTime({
                showsTime: true,

                //                                            ifFormat: "%Y/%m/%d %H:%M",
                ifFormat: "%d-%b-%Y %H:%M",
                daFormat: "%l;%M %p, %e %m, %Y",
                align: "BR",
                timeformat: "24",
                // onupdate: null,
                showsTime: true,
                //  datetext:"",
                electric: false,
                // singleClick: false,
                displayArea: ".siblings('.dtcDisplayArea')",
                position: false,
                button: ".next()"

            });
        });

      
    </script>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#Txtdatetimefrom").val();
            var todate = $("#Txtdatetimeto").val();
            if (!todate) {
                rfvToDate.isvalid = false;
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
        //        function btnSelectFile_onclick() {

        //        }

    </script>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenTimeValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#Txtdatetimefrom").val();
            var todate = $("#Txtdatetimeto").val();
            var Cntrl = source.controltovalidate;
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate, 'Control': Cntrl };
            $.ajax({
                type: "POST",
                url: "betweenTimeValidation.ashx",
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
        //        function btnSelectFile_onclick() {

        //        }

    </script>
    </form>
</body>
</html>
