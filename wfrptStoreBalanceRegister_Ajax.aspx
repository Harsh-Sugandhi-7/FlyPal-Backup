<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptStoreBalanceRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptStoreBalanceRegister_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Store Balance Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 08-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />


    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <style type="text/css">
        .style2 {
            width: 69px;
        }
    </style>
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
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>

                                <td class="clsFormHeader1">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">

                                                <tr>
                                                    <td>
                                                        <span id="lbltitle" class="clsFormHeader" style="width: 100%">Store Balance</span>

                                                    </td>
                                                    <td align="right" colspan="2">
                                                        <table border="0" cellspacing="0">
                                                            <tr>
                                                                <%--<td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Current Criteria" ToolTip="Click to display current searching criterias"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Export to Excel"
                                                                ToolTip="Click to Export report" Width="140px" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Display"
                                                                ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                                ToolTip="Click to report by mail" Width="140px" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                ToolTip="Click to Close Store Balance screen" CausesValidation="False"></asp:Button>
                                                        </td>--%>
                                                                <td>
                                                                    <%--Ajay 08-Nov-2022--%>
                                                                    <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                        Style="display: none;"></asp:Button>
                                                                    <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="width: 1%" align="center">
                                    <asp:UpdatePanel ID="upnlFav" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                class="fa fa-star fa-spin fa-5x circle-icon"
                                                title="Mark As Favourites"></i>
                                                <%--  Ajay 07-Nov-2022--%>
                                            </span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="StepI" class="clsLabelHeader">Selection of Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style2">
                                                        <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagDateSearch" runat="server" ID="txtDate" Width="100px"
                                                            onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlCustomerSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblCustomertitle" class="clsLabelHeader">Selection of Customer</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="chkCustomerStock" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Text="Check Customer Stock"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="69px">
                                                        <span id="lblCustomer" class="clsLabel">Customer</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCustomerList" runat="server" Enabled="False"
                                                            AutoPostBack="True" Width="275px"></asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtCustomerList_AutoCompleteExtender"
                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="" ServiceMethod="GetCustomerList"
                                                            TargetControlID="txtCustomerList" UseContextKey="True" ContextKey="Type=Customer"
                                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetID">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep1" class="clsLabelHeader">Selection of Store</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px"></td>
                                                    <td>
                                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                                            Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="69px">
                                                        <span id="lblStore" class="clsLabel">Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataValueField="ID"
                                                            DataTextField="LocationStore">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>

                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span1" class="clsLabelHeader">Enter Bin Location</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblBinLocation" class="clsLabel">Bin Location</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtLocation" runat="server" Width="275px"
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Label7" class="clsLabelHeader">Selection of Supplier</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSupplierList" runat="server" Width="275px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep4" class="clsLabelHeader">Selection of Category</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server" DataValueField="ID"
                                                            DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep2" class="clsLabelHeader">Selection of Part Number/Description</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabel">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server" Width="275px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblItemTag" class="clsLabelHeader">Selection of Item Tag</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblTag" class="clsLabel">Item Tag</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbItemTag" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            ClientIDMode="Static" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlModelSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Label2" class="clsLabelHeader">Selection of Model</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblModel" class="clsLabelAuto">Model </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtModelList" runat="server" Width="275px"
                                                            AutoPostBack="True" onfocus="GetAssemblyTypeID()"></asp:TextBox>
                                                        <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                            CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                        </cc2:AutoCompleteExtender>
                                                        <script type="text/javascript">
                                                            function GetAssemblyTypeID() {
                                                                var autoComplete = $find('txtModelList_AutoCompleteExtender');

                                                                var str = $("#cmbAssemblyType :selected").val();
                                                                autoComplete.set_contextKey(str);
                                                            }
                                                        </script>
                                                        <%--TargetControlID - The TextBox control where the user types content to be automatically completed.

EnableCaching- Caching is turned on, so typing the same prefix multiple times results in only one call to the web service.

MinimumPrefixLength- Minimum number of characters that must be entered before getting suggestions from the web service.

CompletionInterval - Time in milliseconds when the timer will kick in to get suggestions using the web service.

CompletionSetCount - Number of suggestions to be retrieved from the web service.--%>
                                                        <asp:CheckBox ID="chkNoApplicability" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                            Visible="False" Text="No Applicability" ToolTip="No Applicability" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <span id="lblPartStatus" class="clsLabelHeader">Selection of Part Status</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Label9" class="clsLabelAuto">Part Status</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbPartStatusList" runat="server" DataValueField="PartStatusID"
                                                    DataTextField="PartStatusName">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span id="Label4" class="clsLabelHeader">Selection of IsValued Store</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued Stores Only"
                                                    Checked="True"></asp:CheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlFormatSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span4" class="clsLabelHeader">Selection of Option/Format</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblOption" class="clsLabelAuto">Option</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAboutBalance" runat="server">
                                                            <asp:ListItem Value="0">Stock Qty. > 0</asp:ListItem>
                                                            <asp:ListItem Value="1">Stock Qty. >=0</asp:ListItem>
                                                            <asp:ListItem Value="2">Stock Qty. = 0</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="chkShowInSTock" runat="server" CssClass="clsCheckBox" Text="Consider Show in Stock Only"
                                                            Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkIsOTP" runat="server" CssClass="clsCheckBox" Text="One Time Purchase Only" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButton ID="rdoPortrait" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Portrait" Checked='<%#IIf(AppSettings("ClientCode") = "BA", False, True) %>' GroupName="x"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoLandScape" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Landscape" GroupName="x"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoLandScapeDetail" runat="server" CssClass="clsRadioButton"
                                                            AutoPostBack="True" Text="Landscape Detail" GroupName="x"
                                                            Checked='<%#IIf(AppSettings("ClientCode") = "BA", True, False) %>'></asp:RadioButton>
                                                        <asp:CheckBox ID="chkCategorywise" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Text="Category Wise"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkReceiptNo" runat="server" CssClass="clsLabel" Text="Receipt No."
                                                            Visible="False" Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkRelNoteNo" runat="server" CssClass="clsLabel" Text="Rel. Note No."
                                                            Visible="False" Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkBatchNo" runat="server" CssClass="clsLabel" Text="Batch No."
                                                            Visible="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkSupplier" runat="server" CssClass="clsLabel" Text="Supplier Name"
                                                            Visible="False"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkSupplierInvNo" runat="server" CssClass="clsLabel" Text="Supplier Inv. No."
                                                            Visible="False"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkSupplierInvDate" runat="server" CssClass="clsLabel" Text="Supplier Inv. Date"
                                                            Visible="False"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkOrderInfo" runat="server" CssClass="clsLabel" Text="Order Info"
                                                            Visible="False"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblSelectCriteria" class="clsLabelHeader">Selection of Base,Landing,Commercial
                                                        Value</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" Enabled="False"
                                                            onclick="Enable();" ClientIDMode="Static" Text="Base" GroupName="Gr1"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" Enabled="False"
                                                            onclick="Enable();" ClientIDMode="Static" Text="Landing" Checked="True" GroupName="Gr1"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" Enabled="False"
                                                            onclick="Enable();" ClientIDMode="Static" Text="Commercial" GroupName="Gr1"></asp:RadioButton>
                                                        <asp:CheckBox ID="chkConsiderGROExpenseValues" runat="server" AutoPostBack="true"
                                                            CssClass="clsCheckBox" Text="Consider Only GRO Expense Values" Enabled="false" />
                                                        <asp:CheckBox ID="chkWithGST" runat="server" Checked="true" CssClass="clsCheckBox"
                                                            ClientIDMode="Static" Text="With GST" Visible='<%# AppSettings("IsGSTApplicable")="True" %>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span2" class="clsLabelHeader">Selection to show only valued parts
                                                        value greater than entered value </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkHighValue" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                            Text="Show only valued parts with selected rate value greater than " Enabled="False" />
                                                        <asp:TextBox ID="txtCEffectiveRate" runat="server" CssClass="clsTextBoxDate_Ajax"
                                                            Enabled="false" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblSortBy" class="clsLabelHeader">Sort By</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSortBy1" class="clsLabelAuto">Sort By</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSortBy" runat="server">
                                                            <asp:ListItem Value="0">Part No.</asp:ListItem>
                                                            <asp:ListItem Value="1">Description</asp:ListItem>
                                                            <asp:ListItem Value="2">Folio No.</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span3" class="clsLabelHeader">Enter text to be Display at bottom
                                                        line of report</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span5" class="clsLabelAuto">Text</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtBottomLine" runat="server" AutoPostBack="False"
                                                            Text='<%# " Submitted By : " + User.Identity.Name %>' Width="275px" MaxLength="100"></asp:TextBox>
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
                    <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Display Report</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCritPartStatus" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnDisplayForRotables" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Rotables Asset"
                                                ToolTip="Click to display report"
                                                Visible='<%#IIf(AppSettings("ClientCode") = "BA" And rdoLandScapeDetail.Checked, True, False) %>'></asp:Button>
                                        </td>
                                        <td align="right" colspan="2">
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                            Text="Current Criteria" ToolTip="Click to display current searching criterias"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Export to Excel"
                                                            ToolTip="Click to Export report" Width="140px" Visible="<%$AppSettings:ShowExportToExcelButton%>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Display"
                                                            ToolTip="Click to display report"></asp:Button>
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" Text="Report By Mail"
                                                            ToolTip="Click to report by mail" Width="140px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                            ToolTip="Click to Close Store Balance screen" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <%--<td>
                                                        Ajay 08-Nov-2022 
                                                        <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <!--Dummy panel to open modelpopup-->
                                    <tr style="height: 0px;">
                                        <td style="height: 0px;" colspan="2" align="right">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                <ContentTemplate>
                                                    <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <!--End -->
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtSearch_Autocomplete") {
                    textbox = document.getElementById('hdnpartId');
                }
                else if (source._id == "txtCustomerList_AutoCompleteExtender") {
                    textbox = document.getElementById('hdnCustomerID');
                }

                textbox.value = value;
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
                        textbox.value = val;
                        return;
                    }

                }

                document.getElementById('hdnpartId').value = '';
            }

        </script>
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
        <!--Ajay S 07-Nov-2022 -->
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplierList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        function callEvent() {
            document.getElementById("<%= txtCustomerList.ClientID %>").fireEvent("onchange");

        }
    </script>
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
    <script type="text/javascript">
        var Enable = function () {
            var LandingChecked = $get("rdoLanding").checked;
            if (LandingChecked) {
                $("#chkWithGST").css('visibility', 'visible');
                $("#chkWithGST").next().css('visibility', 'visible');
                $("#chkWithGST").attr('checked', true);
            }
            else {

                $("#chkWithGST").css('visibility', 'hidden');
                $("#chkWithGST").next().css('visibility', 'hidden');
            }
        }
    </script>
</body>
</html>
