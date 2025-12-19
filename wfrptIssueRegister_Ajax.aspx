<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptIssueRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfrptIssueRegister_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Receipt Register</title>
    <meta name="vs_showGrid" content="False">
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <style type="text/css">
        .style1
        {
            width: 400px;
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
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Issue Register </asp:Label>
                                            </td>

                                            <%--<td align="right">
                                                <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criteria" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" runat="server" TabIndex="0"
                                                                        Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" TabIndex="0"
                                                                        Text="Display" ToolTip="Click to display report" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" CausesValidation="False"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close the Issue Register screen" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>


                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" Height="72px" Width="440px"
                                        HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="" ClientValidationFunction="ValidateChkList" ErrorMessage="Select atleast one Aircraft"></asp:CustomValidator>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkAircraftListCount"
                                        ErrorMessage="Report does not allow more than 10 Aircrafts, please break Reports into multiple report prints."></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function ValidateChkList(source, args) {
                                            var IssueToIndex = $get("cmbIssue").value;
                                            if (IssueToIndex == 14) {
                                                args.IsValid = false;
                                                $("#<%=ChkAircraftList.ClientID %>").find(":checkbox").each(function () {
                                                    if ($(this).attr("checked")) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                });
                                            }

                                        }
                                        function ValidateChkAircraftListCount(source, args) {
                                            var IssueToIndex = $get("cmbIssue").selectedValue;
                                            if (IssueToIndex == 2) {
                                                var count = 0;
                                                args.IsValid = false;
                                                $("#<%=ChkAircraftList.ClientID %>").find(":checkbox").each(function () {
                                                    if ($(this).attr("checked")) {
                                                        count += 1;
                                                    }
                                                });
                                                if (count <= 10) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }


                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Issue</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlcmbIssue" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblIssue" runat="server" CssClass="clsLabel" Width="80px">Issue Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbIssue" runat="server" AutoPostBack="True"
                                                            ClientIDMode="Static" >
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDateSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabel" Width="80px">Date Range</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True"
                                                            >
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Last Week</asp:ListItem>
                                                            <asp:ListItem Value="2">Last Month</asp:ListItem>
                                                            <asp:ListItem Value="3">Last Quarter</asp:ListItem>
                                                            <asp:ListItem Value="4">Last Year</asp:ListItem>
                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="45px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                            Width="45px">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                            ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'txtToDateTextBoxWatermarkExtender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="txtToDateTextBoxWatermarkExtender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Document & its Number</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCustomerSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDocType" runat="server" CssClass="clsLabelAuto" Width="80px">Doc. Type</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall1" ID="cmbDocType" runat="server"  AutoPostBack="True"
                                                            >
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Receipt</asp:ListItem>
                                                            <asp:ListItem Value="2">Issue</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <input type="hidden" id="SelectedDocTypeID" runat="server" />
                                                    </td>
                                                    <td width="75">
                                                        <asp:Label ID="lblDocTypeNo" runat="server" CssClass="clsLabelAuto" Visible="False">Receipt No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReceiptTextList" runat="server" Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIssueTextList" runat="server" Visible="False"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtNo" runat="server" Visible="False"
                                                            MaxLength="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpnlToTypeSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblType" runat="server" CssClass="clsLabel" Width="80px">To Type</asp:Label>
                                                    </td>
                                                    <td style="height: 6px" align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbType" runat="server" >
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Vendor</asp:ListItem>
                                                            <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="8">Store</asp:ListItem>
                                                            <asp:ListItem Value="7">Discard</asp:ListItem>
                                                            <asp:ListItem Value="16">WorkShop</asp:ListItem>
                                                            <asp:ListItem Value="17">WorkOrder</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False" Width="75px">Vendor</asp:Label>
                                                    </td>
                                                    <td style="height: 6px" align="left">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCustomer" runat="server" 
                                                            Visible="False" ></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSupplier" runat="server" 
                                                            Visible="False" ></asp:TextBox>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataTextField="LocationStore"
                                                            DataValueField="ID" >
                                                        </asp:DropDownList>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkShop" runat="server" 
                                                            Visible="False" ></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAircraft" runat="server" 
                                                            Visible="False" ></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkOrder" runat="server" ></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWONo" runat="server" Visible="False"
                                                            MaxLength="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraftType" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                            Width="75px">Aircraft</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="chkSelectAll" CssClass="clsRadioButton" runat="server" Text="Select All Aircraft(s)" />
                                                        <asp:CheckBoxList ID="ChkAircraftList" runat="server" CssClass="clsComboBox" DataTextField="RegNo"
                                                            DataValueField="ID" RepeatColumns="7" RepeatDirection="Horizontal" Width="600px">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. 
                                Selection of&nbsp; Release Note No.&nbsp; Or Serial No.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Width="80px">Rel. Note No.</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReleaseNoteNo" runat="server" 
                                                    MaxLength="200" ></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelAuto" Width="75px">Serial No.</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" MaxLength="50"
                                                    ></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Store</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto" Width="80px">From Store</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList  CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbFromStore" runat="server" DataTextField="LocationStore"
                                                    DataValueField="ID" >
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step VII. Selection of Status & Report Format</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlstatus" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>

                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStatus2" runat="server" CssClass="clsLabelAuto" Width="80px">Status</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStatus" runat="server">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Opened</asp:ListItem>
                                                            <asp:ListItem Value="2">Authorized</asp:ListItem>
                                                            <asp:ListItem Value="4">Canceled</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Width="80px" Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>'>Format</asp:Label>
                                                        <%--Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>--%>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true" Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>'>
                                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                        </asp:DropDownList>

                                                        <%--Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>--%>

                                                       
                                                    </td>
                                                    <%--<td width="75">
                                                    </td>--%>
                                                    <%--<td>
                                                        <asp:CheckBox ID="chkDetail" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Checked="True" Text="Detailed Report"></asp:CheckBox>
                                                        <asp:RadioButton ID="optPortrait" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Checked="True" Text="Portrait" GroupName="grOrientation"></asp:RadioButton>
                                                        <asp:RadioButton ID="optLandscape" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Landscape" GroupName="grOrientation"></asp:RadioButton>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="chkDetail" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                            Checked="True" Text="Detailed Report"></asp:CheckBox>
                                                        <asp:RadioButton ID="optPortrait" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Checked="True" Text="Portrait" GroupName="grOrientation"></asp:RadioButton>
                                                        <asp:RadioButton ID="optLandscape" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            Text="Landscape" GroupName="grOrientation"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td>
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Width="80px" Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>'>Format</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server"  AutoPostBack="true"  Visible= '<%#IIf(AppSettings("ClientCode") = "Taj", True, False) %>'>
                                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                        </asp:DropDownList>

                                                        <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued From Stores Only"
                                                            Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkShowInValuation" runat="server" Checked="false" TabIndex="22"
                                                            CssClass="clsCheckBox" Text="Consider Show in Valuation Only" />
                                                    </td>--%>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued From Stores Only"
                                                            Checked="True"></asp:CheckBox>
                                                        <asp:CheckBox ID="chkShowInValuation" runat="server" Checked="false" TabIndex="22"
                                                            CssClass="clsCheckBox" Text="Consider Show in Valuation Only" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <%--<td width="80">
                                                &nbsp;
                                            </td>
                                            <td width="190">
                                                &nbsp;
                                            </td>
                                            <td width="72">
                                                &nbsp;
                                            </td>--%>
                                            <td width="80">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlReportFormatSelection" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkWithRate" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                                        Enabled="false" Text="With rate"></asp:CheckBox>
                                                                    <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" Text="Base"
                                                                        GroupName="Gr1" Enabled="False"></asp:RadioButton>
                                                                    <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" Checked="True"
                                                                        Text="Landing" GroupName="Gr1" Enabled="False"></asp:RadioButton>
                                                                    <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" Text="Commercial"
                                                                        GroupName="Gr1" Enabled="False"></asp:RadioButton>
                                                                    <asp:CheckBox ID="chkZeroValueOnly" runat="server" CssClass="clsCheckBox" Enabled="false"
                                                                        Text="Zero Value Records"></asp:CheckBox>
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
                                    <asp:Label ID="lblStep8" runat="server" CssClass="clsLabelHeader">Step VIII. Selection of Part Number/Description</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto" Width="80px">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server"
                                                    AutoPostBack="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep9" runat="server" CssClass="clsLabelHeader">Step IX. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlCurrentSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblIssuetype" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style1">
                                                        <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style1">
                                                        <asp:Label ID="lblReleaseNoteNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style1">
                                                        <asp:Label ID="lblToStore" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="style1">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criteria" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" TabIndex="0"
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ToolTip="Click to Export report"
                                                            />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False" 
                                                            TabIndex="0" Text="Close" ToolTip="Click to close the Issue Register screen" />
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            var selectedDateIndex = $get("cmbDateRange").selectedIndex;
            if (selectedDateIndex == 6) {
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
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtCustomer.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Customer', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtAircraft.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Aircraft', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });

            $("#<%=txtWorkShop.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=WorkShop', {
                width: 252,
                autoFill: false,
                matchContains: true,

                delay: 0
            });
            $("#<%=txtWorkOrder.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=16', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtReceiptTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=2', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });


            $("#<%=txtIssueTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=3', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=chkSelectAll.ClientID %>").click(function () {
                var status = $("#<%=chkSelectAll.ClientID %>").attr("checked");
                $("#<%=ChkAircraftList.ClientID %>").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");
                    }

                });
            });
            return false;
        });
                                               
    </script>
</body>
</html>
