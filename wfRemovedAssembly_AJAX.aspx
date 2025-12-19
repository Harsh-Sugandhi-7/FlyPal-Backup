<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemovedAssembly_AJAX.aspx.vb"
    Inherits="Flypal.wfRemovedAssembly_AJAX" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript" language="javascript">

        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .clsCursorStyle
        {
            cursor: pointer;
        }
    </style>
    <%--<script type="text/javascript">
        $(document).ready(function () {

            $("<%=txtLicenceNo.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx', {
                width: 250,
                autoFill: false,
                matchContains: true,
                max: 20,
                delay: 0
            });
        });
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Remove Assembly</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="a"
                                            CssClass="clsValidationSummary"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvAssemblyValue" runat="server" Display="None" ValidationGroup="a"
                                            OnServerValidate="CustomValidate1" CssClass="clsLabel"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNote" runat="server" Display="None" OnServerValidate="customvalidate"
                                            ValidationGroup="a" ErrorMessage="Remark Can't be greater than 200 chars" ControlToValidate="txtNote"
                                            CssClass="clsLabel"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" CssClass="clsLabelAuto"
                                            ValidationGroup="a" Display="None" ErrorMessage="Reason Required" ControlToValidate="cmbReason"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvReason" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ValidationGroup="a" OnServerValidate="customvalidate" ErrorMessage="Select Reason from the list."
                                            ControlToValidate="cmbReason"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblEngineInfo" style="font-weight: bold" runat="server"><b>Model & Serial
                                        No. of the []</b></legend>
                                    <table>
                                        <%-- <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlEngineHeaderInfo" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblEngineInfo" runat="server" CssClass="clsLabelHeader">Model & Serial No. of the []</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlEngineInfo" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="L1" runat="server" CssClass="clsLabelAuto" Width="10px"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabel">ATA Chapter</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mAssemblyStatus.ATAChapter %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblManufacturer" runat="server" CssClass="clsLabel">Manufacturer</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtManufacturer" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mAssemblyStatus.ManufacturerName %>"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabel">Model</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mAssemblyStatus.ModelName %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mAssemblyStatus.Assembly.SerialNo %>"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPosition" runat="server" CssClass="clsLabel">Position</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mAssemblyStatus.Position %>"></asp:TextBox>
                                                                </td>
                                                                 <td>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton"
                                                                        Font-Italic="true" Font-Size="8pt"  >View Log Book Entry</asp:LinkButton>
                                                               
                                                                    <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
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
                        <tr>
                            <td>
                                <fieldset id="fdsRemovalInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblRemovalInfo" style="font-weight: bold" runat="server"><b>Removal Information
                                        of the []</b></legend>
                                    <table>
                                        <%-- <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlRemovalHeaderInfo" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblRemovalInfo" runat="server" CssClass="clsLabelHeader">Removal Information of the []</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlRemovalInfo" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRemovedOnStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRemovedOn" runat="server" CssClass="clsLabel">Removed On</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <table id="Table1" border="0" cellpadding="0" cellspacing="0">
                                                                    </table>
                                                                    <%-- <uc1:sicalendar id="calStartDate" runat="server"></uc1:sicalendar>--%>
                                                                    <asp:TextBox ID="calStartDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                        TabIndex="1" Width="90px" AutoPostBack="true"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calStartDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                        TargetControlID="calStartDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td valign="bottom">
                                                                    <asp:Label ID="lblValuesAtRemoval" runat="server" CssClass="clsLabelHeader">Values at Removal</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="25"
                                                                        Text="<%# mAssemblyStatus.RemovalWONO %>" ToolTip="Enter Work Order Number" TabIndex="2"></asp:TextBox>
                                                                </td>
                                                                <td rowspan="3" valign="top">
                                                                    <asp:GridView ID="dgRemovalValue" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                        CssClass="clsGrid" DataKeyNames="AssemblyStatusID" ShowHeaderWhenEmpty="True"
                                                                        TabIndex="7">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PeriodName" HeaderText="Period " HtmlEncode="False" />
                                                                            <asp:BoundField DataField="AssemblyRemovalValueFormatted" HeaderText="Assembly" HtmlEncode="False" />
                                                                            <asp:BoundField DataField="MachineRemovalValueFormatted" HeaderText="Airframe" HtmlEncode="False" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReasonStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReason" runat="server" CssClass="clsLabel">Reason</asp:Label>
                                                                </td>
                                                                <td style="padding-left: 3px;">
                                                                    <table id="Table2" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                                    Width="185px" DataValueField="ID" TabIndex="3">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgReason" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                    Width="24px" CausesValidation="False" ToolTip="Click to Add Reason" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table id="Table22">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkIsRemUnscheduled" runat="server" Checked="<%# mAssemblyStatus.IsRemUnschedule %>"
                                                                                    CssClass="clsCheckBox" Text="Un-Schedule" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLicenceNo" runat="server" CssClass="clsLabelAuto" Width="72px">License No.</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
                                                                                            AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNo_TextChanged"
                                                                                            AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                                        <asp:CustomValidator ID="cvLicenceNo" runat="server" ControlToValidate="txtLicenceNo"
                                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Enter correct License No"
                                                                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
                                                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                            CompletionInterval="1" ServicePath="wfRemovedAssembly_AJAX.aspx" ServiceMethod="GetLicenceList"
                                                                                            TargetControlID="txtLicenceNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                        </cc2:AutoCompleteExtender>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                            Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mAssemblyStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                            ToolTip="<%# mAssemblyStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto" Width="72px">Place</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                        Text="<%# mAssemblyStatus.RemPlace %>" ToolTip="Enter Place" TabIndex="5"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabel">Note</asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="200"
                                                                        Text="<%# mAssemblyStatus.RemovalRemark %>" TextMode="MultiLine" ToolTip="Enter Note"
                                                                        TabIndex="6"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table id="Table12" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                                        class="clsButton_Ajax" tabindex="8" />
                                                                                                </td>
                                                                                                <td style="padding-left: 3px;">
                                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" Enabled="False"
                                                                                                        Text="Remove Attachment" ToolTip="Click to Remove Attachment" Width="120px" />
                                                                                                </td>
                                                                                                <td style="padding-left: 2px;">
                                                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                                                        ImageUrl="icons/CLIP01.ICO" Width="20px" />
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
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnRemovalReason" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                        Style="display: none;" Text="----" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnTechDirection" runat="server" CssClass="clsButtonLong1" Enabled='<%# Not (mAssemblyStatus.RemovalReasonName Is Nothing or mAssemblyStatus.RemovalReasonName="(SELECT)") %>'
                                                        Text="Technical Direction" ToolTip="Click to print Technical Direction" Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "SAA",True,False) %>'
                                                        Width="120px" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" TabIndex="9" Text="Save"
                                                        ToolTip="Click to save information of Removal Assembly" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="10" Text="Print" ToolTip="Click to Print Removed Assembly Details"
                                                        ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Back" ToolTip="Click to go Back to Previous page" TabIndex="11" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="1200" ClientIDMode="Static" DynamicLayout="false"
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
    <%--Date Validations--%>
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
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    <!-- Removal Reason Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemovalReason" Text="Removal Reason" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemovalReason" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemovalReason" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemovalReason" runat="server" TargetControlID="btnDummyRemovalReason"
        PopupControlID="pnlRemovalReason" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemovalReasonStateComplete() {
            $("#btnDummyRemovalReason").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemovalReasonWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemovalReason").attr("src", "wfRemovalReason_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemovalReason").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemovalReason() {
            var RemovalReasonwindow = $find("<%=mdlPopupRemovalReason.ClientID %>");
            //close Removal Reason popup window
            RemovalReasonwindow.hide();
            //           release resources
            $("#IframeRemovalReason").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemovalReason").click();
        }
    </script>
    <!-- End-->
    <!-- Assembly Insp Maintenance Done By Employee Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
    </div>
    <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
        PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMaintDoneByStateComplete() {
            $("#btnDummyMaintDoneBy").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }


        function AddEmployeeLicNo() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=2");

                if (!$.browser.msie) {
                    $("#btnDummyMaintDoneBy").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }


        }
       
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForMaintDoneBy() {
            var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
            //close Ass Insp Maint Done By Emp popup window
            MaintDoneBywindow.hide();
            //Free resources
            $("#IMaintDoneBy").attr("src", "JavaScript:''");
            $("#hdnBtnMaintDoneBy").click();

        }
    </script>
    <!-- End -->
    </form>
</body>
</html>
