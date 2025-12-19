<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUnutilizedStoredBalance_Ajax.aspx.vb"
    Inherits="Flypal.wfrptUnutilizedStoredBalance_Ajax" EnableEventValidation="false" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Aging report for Store Balance Items</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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

                    <td class="clsFormHeader1" colspan="2">
                        <table width="100%">
                            <tr>
                                <td>
                                    <span id="lbltitle" class="clsFormHeader">Aging report for Unutilized Store Balance Items</span>
                                </td>
                                <td align="right" colspan="2">
                                    <%--<table border="0" cellspacing="0">
                                        <tr>
                                            <td>
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
                                                <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                    ToolTip="Click to Close Aging report for Store Balance Items screen" CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDate"
                                    Display="None" ErrorMessage="As On Date required"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <span id="StepI" class="clsLabelHeader">Step I. Selection of Date</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="69px">
                                                <span id="lblDate" class="clsLabelAuto">As On Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                    onchange="ValidateDateText(this,'txtDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlSelectionOfRange" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="Span2" class="clsLabelHeader">Step II. Selection of Date Range</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFrom" class="clsLabelAuto" Text="From" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'txtFromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="txtFromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTo" class="clsLabelAuto" Text="To" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
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
                                                        <span id="lblCustomertitle" class="clsLabelHeader">Step III. Selection of Customer</span>
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
                                                        <asp:TextBox ID="txtCustomerList" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
                                                            AutoPostBack="True" Width="275px" onChange="SetPartIdonChange(this,'txtCustomerList_AutoCompleteExtender')"></asp:TextBox>
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
                                                        <span id="lblStep1" class="clsLabelHeader">Step IV. Selection of Store</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px"></td>
                                                    <td>
                                                        <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="69px">
                                                        <span id="lblStore" class="clsLabel">Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server"   DataValueField="ID"
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
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <span id="lblStep2" class="clsLabelHeader">Step V. Selection of Part Number/Description</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="69px">
                                                <span id="lblSearch" class="clsLabel">Search</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" Width="275px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel runat="server" ID="upnlModelSelection" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <span id="Label2" class="clsLabelHeader">Step VI. Selection of Model</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="96px">
                                                                        <span id="spanModel" class="clsLabel">Model</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server"   DataTextField="ModelName"
                                                                            DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                        <asp:CheckBox ID="chkCommonOrApplicability" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                                            Text="Common/No Applicability" ToolTip="Common/No Applicability" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span id="lblSelectionofCategory" class="clsLabelHeader">Step VII. Selection of Category</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="96px">
                                                <span id="lblCategory" class="clsLabel">Category</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server"   DataValueField="ID"
                                                    DataTextField="Name">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="Label4" class="clsLabelHeader">Step VIII. Selection of IsValued Store</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued Stores Only"
                                                    Checked="True"></asp:CheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="Label1" class="clsLabelHeader">Step IX. Selection of ATA</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="25px">
                                    <input type="checkbox" style="vertical-align: bottom;" id="chkSelectAllATACode" />
                                </td>
                                <td width="100%">
                                    <asp:Panel ID="CpnlATACodeList" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                        <div style="float: left; vertical-align: middle;">
                                            <span id="lblATACodeList" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">ATA Code List</span>
                                        </div>
                                        <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                            <image id="imgbtnClpnl" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlATACodeList" runat="server" ClientIDMode="Static" Visible="true">
                                        <asp:CheckBoxList ID="ChkATACodeList" runat="server" ClientIDMode="Static" CssClass="clsComboBox_Ajax"
                                            DataTextField="ATA" DataValueField="ID" EnableViewState="false" RepeatColumns="4"
                                            RepeatDirection="Horizontal" Width="100%">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <cc2:CollapsiblePanelExtender ID="clpATACodeList" runat="Server" BehaviorID="clpATACodeListBehaviour"
                                        ClientIDMode="Static" CollapseControlID="CpnlATACodeList" Collapsed="True" CollapsedImage="~/images/expand_blue.jpg"
                                        CollapsedText="(Show Details...)" ExpandControlID="CpnlATACodeList" ExpandedImage="~/images/collapse_blue.jpg"
                                        ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl" SkinID="CollapsiblePanelDemo"
                                        SuppressPostBack="false" TargetControlID="pnlATACodeList" />
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
                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step X.  Display Report</asp:Label>
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
                                            <asp:Label ID="lblDaysRanges" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDatesRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
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
                                            <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                          <td align="right" colspan="2">
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsbtnH"
                                                        Text="Current Criteria" ToolTip="Click to display current searching criterias">
                                                    </asp:Button>
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
                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH" Text="Close"
                                                        ToolTip="Click to Close Aging report for Store Balance Items screen" CausesValidation="False">
                                                    </asp:Button>
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
        <asp:HiddenField ID="hdnATACodeList" runat="server" ClientIDMode="Static" />
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
            function SetPartIdonChange(source, extenderid) {
                var popup = $find(extenderid);
                var complist = popup.get_completionList();
                var text = $(source).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;

                        if (extenderid == "txtSearch_Autocomplete") {
                            textbox = document.getElementById('hdnpartId');
                        }
                        else if (extenderid == "txtCustomerList_AutoCompleteExtender") {
                            textbox = document.getElementById('hdnCustomerID');
                        }
                        textbox.value = val;
                        return;
                    }

                }

                if (extenderid == "txtSearch_Autocomplete") {
                    document.getElementById('hdnpartId').value = '';
                }
                else if (extenderid == "txtCustomerList_AutoCompleteExtender") {
                    document.getElementById('hdnCustomerID').value = '';
                }


            }

        </script>
        <script type="text/javascript">
            //check all/ uncheck all checkbox of aircraft list
            $(document).ready(function () {

                $("#chkSelectAllATACode").click(function () {
                    var status = $("#chkSelectAllATACode").attr("checked");
                    $("#ChkATACodeList").find(":checkbox").each(function () {
                        if (status == "checked") {
                            $(this).attr("checked", status);
                        }
                        else {
                            $(this).removeAttr("checked");
                        }

                    });
                });

                $("#btnExport,#btnDisplay,#btnCurrentSearchCriteria,#btnByMail,hdnimgBtnSendMail").live('click', function () {
                    try {
                        SetSelectedATACode();
                    } catch (e) {
                        alert(e.Message);
                    }
                    return true;
                });
            });
            //set aircraft list text(i.e. aircraft name) to hidden field to access from code behind
            function SetSelectedATACode() {
                var ATACodelist = new Array();
                $("#ChkATACodeList :checked").each(function (i) {
                    ATACodelist.push($(this).next().text());
                });

                $("#hdnATACodeList").val('');
                $("#hdnATACodeList").val(ATACodelist);
            }

        </script>
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
        function callEvent() {
            document.getElementById("<%= txtCustomerList.ClientID %>").fireEvent("onchange");

        }
    </script>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
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
</body>
</html>
