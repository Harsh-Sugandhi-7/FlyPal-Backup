<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForCofA_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForCofA_Ajax" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>C of A Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 09-Nov-2022--%>
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
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
    <style type="text/css">
        .style1 {
            height: 26px;
        }
    </style>
    <style type="text/css">
        .btn {
            padding: 1px;
            font-size: 8pt;
        }

        .TextBox {
            box-sizing: Content-box;
        }

        .label {
            font-weight: normal !important;
            font-style: normal;
        }
    </style>
    <style type="text/css">
        .clsHoverDrowDownWidth {
            width: 100%;
            height: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="clsFormHeader1">
                                                                <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Search criteria for C of A</asp:Label>
                                                            </td>
                                                            <td style="width: 1%" align="center">
                                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                                    class="fa fa-star fa-spin fa-5x circle-icon"
                                                                    title="Mark As Favourites"></i>
                                                                    <%--  Ajay 12-Nov-2022--%>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage=""
                                                                    ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ValidateAircraft"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                    ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate" ClientValidationFunction="ValidateType"></asp:CustomValidator>
                                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                                    ErrorMessage="As On Date Required" ControlToValidate="txtFromDate" Display="None"></asp:RequiredFieldValidator>
                                                                <script type="text/javascript">
                                                                    function ValidateAircraft(source, args) {
                                                                        args.IsValid = false;
                                                                        source.errormessage = 'Please select the Aircraft and Assembly.'
                                                                        var dd = $get("cmbAircraft");
                                                                        if (dd.selectedIndex != 0) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }
                                                                    }
                                                                    function ValidateType(source, args) {

                                                                        var hdopn = $('#hdnOpen').val();

                                                                        args.IsValid = false;
                                                                        debugger;
                                                                        if ('<%# AppSettings("ShowMaintenanceForNewClients") %>' == "True") {
                                                                            source.errormessage = 'Please select the type.'
                                                                        }
                                                                        else {

                                                                            if (hdopn == "3") {
                                                                                source.errormessage = 'Please select the Directive.'
                                                                            }
                                                                            else {

                                                                                source.errormessage = 'Please select the Service/Inspection.'
                                                                            }
                                                                        }
                                                                        var $items = $('.active').length;
                                                                        if ($items != 0) {
                                                                            args.IsValid = true;
                                                                            return;
                                                                        }

                                                                    }
                                                                </script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlAsOnDate" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td colspan="3">
                                                                <span id="lblStep1" class="clsLabelHeader">Selection of As On Date</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="10px"></td>
                                                            <td>
                                                                <span id="lblFromDate" class="clsLabel">As On Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagDateSearch TextBox" Width="100px"
                                                                    onchange="ValidateDateText(this,'txtFromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:calendarextender id="txtFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                    enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate">
                                                                </cc2:calendarextender>
                                                                <cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="txtFromDate_watermarkextender"
                                                                    clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                    watermarkcssclass="clsDateTextBox">
                                                                </cc2:textboxwatermarkextender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlSelectionofAircraft" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep2" class="clsLabelHeader">Selection of Aircraft</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblAircraft" class="clsLabel">Aircraft</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" AutoPostBack="True"
                                                                                DataTextField="RegNo" DataValueField="MachineID">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep3" class="clsLabelHeader">Selection of Assembly</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabel">Assembly</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataValueField="ID"
                                                                                DataTextField="ModelSerialNoPostion">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <span id="lblStep4" class="clsLabelHeader">Selection of ATA</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblATAChapter" class="clsLabel">ATA Chapter</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbATAChapter" runat="server"
                                                                                DataValueField="ID" DataTextField="ATAChapter">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkAssembly" Checked="true" runat="server" CssClass="clsCheckBox"
                                                                                Text="Show Assembly Inspections" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkComponent" Checked="true" runat="server" CssClass="clsCheckBox"
                                                                                Text="Show Component Inspections" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkAirframeDueAsOf" Text="Show Due As Of Airframe Values" runat="server"
                                                                                Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA", False, True) %>'
                                                                                CssClass="clsCheckBox" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStep5" class="clsLabelHeader">Selection of Type</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblTypeStar1" class="clsLabelStar" runat="server">*</span>

                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel runat="server" ID="upnType" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:CheckBoxList ID="cmbType" runat="server" CssClass="clsComboBox_Ajax clsLabel"
                                                                    AutoPostBack="True" DataValueField="ID" DataTextField="Name" ClientIDMode="Static" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                                </asp:CheckBoxList>
                                                                <span id="Span1" class="clsLabel" runat="server" visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", True, False) %>'>Type</span>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlServiceType" runat="server" CssClass="clsPanel1" Visible="<%# mOpen = 1 Or mOpen = 5 %>">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel runat="server" ID="upnlServiceType" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="pnlModificationType" runat="server" CssClass="clsPanel1" Visible="<%# mOpen = 3 %>">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel runat="server" ID="upnlModificationType" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:PlaceHolder ID="phInspection" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>

                                                                        <asp:Panel ID="pnlInspectionType" runat="server" CssClass="clsPanel1" Visible="<%# mOpen = 1 Or mOpen = 2 %>">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:UpdatePanel runat="server" ID="upnlInspectionType" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                                    DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </asp:PlaceHolder>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkNotApplicable" Text="With &quot;Not Applicable&quot;" runat="server"
                                                            CssClass="clsCheckBox" Visible="<%# mOpen = 2 Or mOpen = 5 %>" />
                                                    </td>
                                                     <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClientsWithTaskCard") = "True", True, False) %>'>
                                                    <td>
                                                        <asp:CheckBox ID="chkTaskCard" runat="server" CssClass="clsCheckBox" AutoPostBack="true"
                                                            Text="With Task Cards" Visible="false" />
                                                    </td>
                                                         </asp:PlaceHolder>
                                                </tr>
                                            </table>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlShowCofA" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Selection of Show C of A</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblShowCofA" runat="server" CssClass="clsLabel">Show C of A </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbShowCofA" runat="server" AutoPostBack="True">
                                                                                    <asp:ListItem Value="0">All Records</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Show C of A</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkNotMonitoredValues" runat="server" CssClass="clsCheckBox" Text="With Non-Monitored Period Values" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                        <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True" Or mOpen = 1 Or mOpen = 3 Or mOpen = 2 Or mOpen = 5, True, False) %>'>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Bottom Line of Report</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Enter Line which you want to print at the bottom of the report.</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtBottomLine" runat="server" CssClass="clsTextBoxMultiLine_Ajax  TextBox"
                                                                        MaxLength="500" TextMode="MultiLine" ToolTip="Enter Note"
                                                                        Width="552px" Height="45px">I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblStepFormat" runat="server" CssClass="clsLabelHeader" Visible="<%# mOpen = 1 Or mOpen = 2 Or mOpen = 5  %>">Select Format of Report</asp:Label>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFormat" runat="server" CssClass="clsLabel" Visible="<%# mOpen = 1 Or mOpen = 2 Or mOpen = 5 %>">Format</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbFormat" runat="server" Visible="<%# mOpen = 1 Or mOpen = 2 Or mOpen = 5 %>"
                                                                                    AutoPostBack="true">
                                                                                    <asp:ListItem Value="0">Format 1 (without TSN and TSO)</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Format 2 (with TSN and TSO)</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>


                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="clsLabelHeader" Visible="false">Estimated Flying Hours</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:DataGrid ID="gdPerDayLimit" runat="server" AutoGenerateColumns="False" Visible="false" CellPadding="5" GridLines="Horizontal"
                                                                        CssClass="clsGridNewStyle">
                                                                        <AlternatingItemStyle CssClass="clsdgAltItem" />
                                                                        <ItemStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="PeriodID" HeaderText="PeriodID" Visible="False"></asp:BoundColumn>
                                                                            <asp:BoundColumn DataField="PeriodName" HeaderText="Period"></asp:BoundColumn>
                                                                            <asp:TemplateColumn HeaderText="Limit">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtLimitPerDay" runat="server" BackColor="White" CssClass="clsTextBoxRightAlign_Ajax TextBox"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "PeriodLimit") %>' ToolTip="Enter corresponding Limit Value."></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                        </Columns>
                                                                    </asp:DataGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <span id="lblCMPRefHeader" class="clsLabelHeader" visible="<%# (mOpen = 2 Or mOpen = 5) %>"
                                                                        runat="server">Enter the CMP Reference</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left"></td>
                                                                <td align="left">
                                                                    <span id="lblCMPREfLine" class="clsLabelAuto" runat="server" visible="<%# (mOpen = 2 Or mOpen = 5) %>">CMP Reference</span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtCMPRef" runat="server" CssClass="clsTextBoxTagSearch TextBox" Visible="<%# (mOpen = 2 Or mOpen = 5) %>"
                                                                        ToolTip="Enter CMP Reference" MaxLength="500"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>

                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upnlCurrentCriteria" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel runat="server" ID="pnlCriteria" Visible="false">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Display Report</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblATAChapter1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReportType" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                                CssClass="clsbtnH" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias." />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                                                TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report" Width="140px"
                                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                                                Text="Display" ToolTip="Click to Display Report" />
                                                                        </td>
                                                                        <%-- 'Added by Shital on 14-Sep-2016--%>
                                                                        <td>
                                                                            <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" Text="Report By Mail"
                                                                                ToolTip="Click to receive Report through mail" Width="140px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                                                TabIndex="0" Text="Close" ToolTip="Click to Close" />
                                                                        </td>
                                                                        <td>
                                                                            <%--Ajay 10-Nov-2022--%>
                                                                            <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                                Style="display: none;"></asp:Button>
                                                                            <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <!-- Dummy panel to open modelpopup 'Added by Shital on 14-Sep-2016 -->
                                                                    <tr style="height: 0px;">
                                                                        <td style="height: 0px;" colspan="2" align="right">
                                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                                <ContentTemplate>
                                                                                    <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                    <asp:HiddenField ID="hdnService" runat="server" />
                                                                                    <asp:HiddenField ID="hdnInspection" runat="server" />
                                                                                    <asp:HiddenField ID="hdnDirective" runat="server" />
                                                                                    <asp:HiddenField ID="hdnOpen" runat="server" />
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <!--End -->
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
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
        <!-- Popup For Report By Mail 14-Sep-2016-->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupReceipt1" runat="server" targetcontrolid="btnDummyReceipt1"
            popupcontrolid="pnlReceipt1" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function OpenByMaiWindow() {
                try {
                    $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                    $("#btnDummyReceipt1").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSendMail() {
                var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
                //close popup window
                Receiptwindow1.hide();
                //           release resources
                $("#IframeReceipt1").attr("src", "JavaScript:''");
            }
            function ParentCallBackFunctionToSendMail() {
                var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
                //close popup window
                Receiptwindow1.hide();
                //           release resources
                $("#IframeReceipt1").attr("src", "JavaScript:''");
                //call image button
                $("#hdnimgLogBtnSendMail").click();
            }
        </script>
        <!---End-->
        <!--Ajay S 10-Nov-2022 -->
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
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        function disableEnable() {

            var hvService = $('#hdnService').val();
            var hvInsp = $('#hdnInspection').val();
            var hvMod = $('#hdnDirective').val();

            ServiceMultiSelect();
            InspMultiSelect();
            DirectiveMultiSelect();

            if (hvService == 'True') {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('selectAll', false);
                //  $('[id*=ListServiceType]').multiselect('refresh');
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            else if (hvService == 'False' || hvService == '') {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                // $('[id*=ListServiceType]').multiselect('refresh');
                $('[id*=ListServiceType]').multiselect('disable', false);

            }

            if (hvInsp == 'True') {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('selectAll', false);
                //    $('[id*=ListInspectionType]').multiselect('refresh');
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

            else if (hvInsp == 'False' || hvInsp == '') {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);

            }

            if (hvMod == 'True') {
                $('[id*=ListDirectiveType]').multiselect('enable', true);
                $('[id*=ListDirectiveType]').multiselect('selectAll', false);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
            else if (hvMod == 'False' || hvMod == '') {
                $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                $('[id*=ListDirectiveType]').multiselect('disable', false);

            }
        }

    </script>
    <script type="text/javascript">
        function disableEnableOnPageLoad() {

            var hvService = $('#hdnService').val();
            var hvInsp = $('#hdnInspection').val();
            var hvMod = $('#hdnDirective').val();

            ServiceMultiSelect();
            InspMultiSelect();
            DirectiveMultiSelect();

            if (hvService == 'True') {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            else if (hvService == 'False' || hvService == '') {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                $('[id*=ListServiceType]').multiselect('disable', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }

            if (hvInsp == 'True') {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

            else if (hvInsp == 'False' || hvInsp == '') {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }

            if (hvMod == 'True') {
                $('[id*=ListDirectiveType]').multiselect('enable', true);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
            else if (hvMod == 'False' || hvMod == '') {
                $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                $('[id*=ListDirectiveType]').multiselect('disable', false);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
        }

    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            ServiceMultiSelect();
            InspMultiSelect();
            DirectiveMultiSelect();
            // disableEnable();
            disableEnableOnPageLoad();
            var fvclkbtn = document.getElementById("<%=FavIClk.ClientID%>"); <%--$find("<%=FavIClk.ClientID %>");--%>
           /* FunctionFav(fvclkbtn);*/

        });

        //   Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function ServiceMultiSelect() {
            $('[id*=ListServiceType]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                nSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');

            //   });
        }
    </script>
    <script type="text/javascript">

        //Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function DirectiveMultiSelect() {
            $('[id*=ListDirectiveType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;

                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Directive',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directives',
                nSelectedText: 'Directives'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            //        });
        }
    </script>
    <script type="text/javascript">

        // Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function InspMultiSelect() {
            $('[id*=ListInspectionType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;


                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Inspection',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Inspections',
                nSelectedText: 'Inspections'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            //  });
        }
    </script>
</body>
</html>
