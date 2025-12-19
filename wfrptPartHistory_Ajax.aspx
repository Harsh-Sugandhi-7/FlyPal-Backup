<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPartHistory_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptPartHistory_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part History</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 08-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmrptPartHitory" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>

                                            <td class="clsFormHeader1">
                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader" Style="width: 100%">Part History-Order</asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td align="right">
                                                            <%--<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                    CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPreview1" runat="server" CssClass="clsbtnH clsinfoH" Text="Preview EX"
                                                                                    Visible="false" ToolTip="Click to preview Expendable report" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPreview" runat="server" CssClass="clsbtnH clsinfoH" Text="Preview"
                                                                                    Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", True, False) %>'
                                                                                    ToolTip="Click to preview report" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH clsinfoH" Width="140px"
                                                                                    Visible="<%$AppSettings:ShowExportToExcelButton%>" TabIndex="0" Text="Export to Excel"
                                                                                    ToolTip="Click to Export report" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                                                    Text="Display" ToolTip="Click to display report" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                    TabIndex="0" Text="Close" />
                                                                            </td>
                                                                            <td>
                                                                                <%--Ajay 08-Nov-2022 
                                                                                <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                                    Style="display: none;"></asp:Button>
                                                                                <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 1%" align="center">
                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                    class="fa fa-star fa-spin fa-5x circle-icon"
                                                    title="Mark As Favourites"></i>
                                                    <%--  Ajay 07-Nov-2022--%>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSearch"
                                                Display="None" ErrorMessage="Enter whole part no. and description" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvSelectPart" runat="server" CssClass="clsLabelAuto"
                                                ControlToValidate="txtSearch" Display="None" ErrorMessage="Enter part no."></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader" Visible="<%# mType = 4 %>">Step 
                                                    I. Selection of Customer</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabel" Visible="<%# mType = 4 %>"
                                                            Enabled="False">Customer</asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCustomerList" AutoComplete="off" runat="server"  
                                                            Enabled="False" AutoPostBack="True"></asp:TextBox>
                                                        <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtCustomerList_AutoCompleteExtender"
                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfrptPartHistory_Ajax.aspx"
                                                            ServiceMethod="GetCustomerList" TargetControlID="txtCustomerList" UseContextKey="True"
                                                            ContextKey="Type=Customer" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID"
                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkCustomerStock" runat="server" CssClass="clsLabelAuto" Visible="<%# mType = 4 %>"
                                                            AutoPostBack="True" Text="Check Customer Stock"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="lblStoreSelection" runat="server" CssClass="clsLabelHeader" Visible="<%# mType = 4 %>">Step 
                                                    II. Selection of Store</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"></td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                                            Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblStore" runat="server" CssClass="clsLabel" Visible="<%# mType = 4 %>">Store</asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" EnableViewState="false" ID="cmbStore" runat="server"  
                                                            DataTextField="LocationStore" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="lblIsValued" runat="server" CssClass="clsLabelHeader">Step III.Selection of IsValued Store</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="left"></td>
                                                    <td colspan="2" align="left">
                                                        <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued Stores Only"
                                                            Checked="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Part Number/Description</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblPilotStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td align="left">
                                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                              onChange="SetPartIdonChange()"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                            Text="With Alternate Part"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="5">
                                                        <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader" Visible="<%# mType = 4 %>">Step V. Selection of Serial No.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel" Visible="<%# mType = 4 %>">Serial No.</asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                             onfocus="SetContextKeyForSerialNo()" Visible="true">

                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader" Visible="<%# mType = 4 %>">Step VI. Selection of Release Note No.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblReleaseNoteNo" runat="server" CssClass="clsLabel" Visible="<%# mType = 4 %>">Rel. Note No.</asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td colspan="2" align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRelNoteNo" onfocus="SetContextKeyForRelNoteNo()" AutoComplete="off"
                                                            ClientIDMode="Static" runat="server"   Visible="<%# mType = 4 %>"
                                                            Enabled="False">

                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" align="left">
                                                        <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step VII. Selection of Format</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelAuto">Format</asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" ClientIDMode="Static"  
                                                            onchange="ControlVisibilityForRadioButtons(this);">
                                                            <asp:ListItem Value="0">Format 1 (Without Rate)</asp:ListItem>
                                                            <asp:ListItem Value="1">Format 2 (With Rate)</asp:ListItem>
                                                            <asp:ListItem Value="2">Format 3</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left"></td>
                                                    <td align="left"></td>
                                                    <td colspan="3" align="left">
                                                        <div id="divRadioButtons" style="width: 100%; display: none;">
                                                            <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" Text="Base"
                                                                GroupName="Gr1"></asp:RadioButton>
                                                            <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" Text="Landing"
                                                                Checked="True" GroupName="Gr1"></asp:RadioButton>
                                                            <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" Text="Commercial"
                                                                GroupName="Gr1"></asp:RadioButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                CompletionInterval="1" ServicePath="wfrptPartHistory_Ajax.aspx" ServiceMethod="GetPartNoDescriptionList"
                                                TargetControlID="txtSearch" OnClientItemSelected="SetID" UseContextKey="False"
                                                ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                OnClientShowing="ClientShowing">
                                            </asp:AutoCompleteExtender>
                                            <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtRelNoteNo_AutoComplete" runat="server"
                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                CompletionInterval="1" ServicePath="wfrptPartHistory_Ajax.aspx" ServiceMethod="GetReleNoteNoList"
                                                EnableCaching="false" TargetControlID="txtRelNoteNo" UseContextKey="True" ContextKey=""
                                                CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                OnClientShowing="ClientShowing">
                                            </asp:AutoCompleteExtender>
                                            <asp:AutoCompleteExtender ID="txtSerialNo_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                                CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" ContextKey=""
                                                DelimiterCharacters="" EnableCaching="false" Enabled="True" MinimumPrefixLength="0"
                                                OnClientHiding="ClientHiding" OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
                                                OnClientShowing="ClientShowing" OnClientShown="ClientHiding" ServiceMethod="GetSerialNo"
                                                ServicePath="wfrptPartHistory_Ajax.aspx" TargetControlID="txtSerialNo" UseContextKey="True">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step VIII. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSerachCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="<%# mType = 4 %>">

                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="<%# mType = 4 %>">

                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="<%# mType = 4 %>">

                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblRelNoteNo" runat="server" CssClass="clsLabelAuto" Visible="<%# mType = 4 %>">

                                                        </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                               <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                            CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPreview1" runat="server" CssClass="clsbtnH" Text="Preview EX"
                                                            Visible="false" ToolTip="Click to preview Expendable report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPreview" runat="server" CssClass="clsbtnH" Text="Preview"
                                                            Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", True, False) %>'
                                                            ToolTip="Click to preview report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" runat="server" CssClass="clsbtnH" Width="140px"
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" TabIndex="0" Text="Export to Excel"
                                                            ToolTip="Click to Export report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" />
                                                    </td>
                                                    <td>
                                                         <%--Ajay 08-Nov-2022--%> 
                                                        <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <asp:HiddenField ID="hdnpartId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCustomerID" runat="server" ClientIDMode="Static" />
        <script type="text/javascript">
            function ControlVisibilityForRadioButtons(elem) {
                if (elem.selectedIndex != 1) {
                    $("#divRadioButtons").css('display', 'none');
                }
                else {
                    $("#divRadioButtons").css('display', 'block');
                }

            }

        </script>
        <%--
    Autocomplete functions to set id--%>
        <script type="text/javascript">
            function SetID(source, e) {
                //get id from autocomplete list
                var node;
                var value = e.get_value();

                if (value) node = e.get_item();
                else {
                    value = e.get_item().parentNode._value;
                    node = e.get_item().parentNode;
                }

                var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml; //Boolean Expression ? First Statement : Second Statement Ternary operator ?:
                source.get_element().value = text;

                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtSearch_Autocomplete") {
                    textbox = document.getElementById('hdnpartId');
                }
                else if (source._id == "txtCustomerList_AutoCompleteExtender") {
                    textbox = document.getElementById('hdnCustomerID');
                }

                textbox.value = value.toString();
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetPartIdonChange() {
                var popup = $find("txtSearch_Autocomplete");
                var complist = popup.get_completionList();
                var text = $("#txtSearch").val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;
                        var textbox = document.getElementById('hdnpartId');
                        textbox.value = val.toString();
                        return;
                    }

                }
                //alert(document.getElementById('hdnpartId').value);
                //document.getElementById('hdnpartId').value = '';
            }
            //control visiblility for format radion buttons
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {
                var dd = document.getElementById("cmbFormat");
                ControlVisibilityForRadioButtons(dd);
            }
        </script>
        <%--ReleaseNote No autocomplete--%>
        <script type="text/javascript">
            function GetPartID() {
                var partid = document.getElementById('hdnpartId').value.toString();
                if (partid) {
                    return partid;
                }
                else {
                    return '{00000000-0000-0000-0000-000000000000}';
                }

            }
            function SetContextKeyForRelNoteNo() {
                var autoComplete = $find('txtRelNoteNo_AutoComplete');
                var str = 'PartID=' + GetPartID();
                autoComplete.set_contextKey(str);
            }
            function SetContextKeyForSerialNo() {
                var autoComplete = $find('txtSerialNo_AutoCompleteExtender');
                var str = 'PartID=' + GetPartID();
                autoComplete.set_contextKey(str);
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
        <!--Ajay S 08-Nov-2022 -->
        <script type="text/javascript">
            function FunctionFav(x) {
                if (x.classList.contains("fa-star")) {
                    x.classList.remove("fa-star");
                    x.classList.add("fa-star-o");
                    x.style.color = 'black';
                    x.style.border = 'black';
                    $("#hdnBtnRemoveFav").click();
                }
                else {
                    x.classList.remove("fa-star-o");
                    x.classList.add("fa-star");
                    x.style.color = '#fff';
                    x.style.border = 'black';
                    $("#hdnBtnMarkFav").click();
                }
            }
            function MarkFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
        </script>
        <!--Ajay E -->
    </form>
</body>
</html>
