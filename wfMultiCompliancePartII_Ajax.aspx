<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMultiCompliancePartII_Ajax.aspx.vb"
    Inherits="Flypal.wfMultiCompliancePartII_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

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
        .GbiHighlight
        {
            background-color: Aqua;
        }
    </style>
    <script type="text/javascript">
        function autoResizeRemovalComp() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeRemovalComp').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeRemovalComp').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeRemovalComp').height = (newheight + 2) + "px";
            document.getElementById('IframeRemovalComp').width = (newwidth) + "px";
            document.getElementById('tbpnlRemovalCompList').height = (newheight) + "px";
            document.getElementById('tbpnlRemovalCompList').width = (newwidth) + "px";

            document.getElementById('TabContainer1').height = (newheight) + "px";
            document.getElementById('TabContainer1').width = (newwidth) + "px";


        }
    </script>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                $('.cbSelectRow').prop('checked', checked).trigger('change');
            });

        });

    </script>
    <!-- End-->
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Multi Compliance</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>
                                        <cc2:TabContainer ID="TabContainer1" runat="server" class="clstablelistin" AutoPostBack="true">
                                            <cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1">
                                                <HeaderTemplate>
                                                    Maintenance Activity
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table id="tblmain" class="clstablelistin">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                                            ValidationGroup="a" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                                        <asp:CustomValidator ID="cvCustomer" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Aircraft from the list."
                                                                            ValidationGroup="a" Display="None" ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                            ValidationGroup="a" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="Table7" class="clsTable1" border="0" cellspacing="1" cellpadding="1">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table id="Table10" border="0" cellspacing="1" cellpadding="1">
                                                                                        <tr>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Compliance Date</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <table id="Table9" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                        </td>
                                                                                                        <td style="height: 24px">
                                                                                                            <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                                                AutoPostBack="true" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"></asp:TextBox>
                                                                                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                                                Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtAsOnDate">
                                                                                                            </cc2:CalendarExtender>
                                                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                                                                                WatermarkCssClass="clsDateTextBox" Enabled="True">
                                                                                                            </cc2:TextBoxWatermarkExtender>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox" AutoPostBack="True"
                                                                                                    DataTextField="RegNo" DataValueField="MachineID">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                                <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                                                    Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3" AutoPostBack="True"
                                                                                                    DataTextField="ModelSerialNoPostion" DataValueField="ID">
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
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlValues" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table id="Table6" class="clsTable1" cellpadding="0" designtimedragdrop="427">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <table id="Table8" cellspacing="0">
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On Values</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top">
                                                                                                <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                                                    ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True"
                                                                                                    AutoGenerateColumns="False" PageSize="5">
                                                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td valign="top" align="right">
                                                                                    <table id="Table2" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton" ToolTip="Click to select the log"
                                                                                                    ValidationGroup="a" Text="Select Log"></asp:Button>
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
                                                            <td align="right">
                                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnNext" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go onto next Page"
                                                                                        ValidationGroup="a" Text="Next"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close the Multi Compliance screen"
                                                                                        CausesValidation="False" Text="Close"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="TabPanel2" runat="server">
                                                <HeaderTemplate>
                                                    Work Order
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table id="tblmain1" class="clstablelistin">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlWOValidationSummary" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Fields"
                                                                            ValidationGroup="b" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                                        <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" ValidationGroup="b"
                                                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <table id="Table13" border="0" cellspacing="1" cellpadding="1">
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Compliance Date</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlWOAsOnDate" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox runat="server" ID="txtWOAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                        onchange="ValidateDateText(this,'WOAsOnDate_watermarkextender');"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtWOAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWOAsOnDate">
                                                                                    </cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtWOAsOnDate" ID="WOAsOnDate_watermarkextender"
                                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                        WatermarkCssClass="clsDateTextBox">
                                                                                    </cc2:TextBoxWatermarkExtender>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 21px" colspan="4">
                                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Step II. Selection of Work Order</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label5" runat="server" CssClass="clsLabel">Search for</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxDate_Ajax"></asp:TextBox>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlFindNow">
                                                                                <ContentTemplate>
                                                                                    <table id="Table1" border="0">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click To Find records as Searching criteria"
                                                                                                    Text="Find Now"></asp:Button>
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
                                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="100px" Width="100%">
                                                                    <asp:UpdatePanel ID="upnlWOLIst" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgWOList" runat="server" CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                                                AutoGenerateColumns="False">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle CssClass="hideGridColumn" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="WODateFormatted" HeaderText="Date">
                                                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="WONumber" SortExpression="WONumber" HeaderText="W. O. No.">
                                                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                                        <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec"></asp:ButtonField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <td valign="top" colspan="2" align="right">
                                                        <table id="Table45" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label6" runat="server" CssClass="clsLabelHeader">Compliance On Values</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlComplianceValues" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgDoneOnValuesWO" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                                ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AutoGenerateColumns="False">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period"></asp:BoundField>
                                                                                    <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle" align="right">
                                                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlSelectLogWO">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="btnSelectLogWO" TabIndex="0" runat="server" CssClass="clsButton"
                                                                                ValidationGroup="b" ToolTip="Click to select the log" Text="Select Log"></asp:Button>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlResult">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Due Jobs as per selected criteria : 0 Record(s) found.</asp:Label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td colspan="2" align="right">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlButtonsSaveTop">
                                                                <ContentTemplate>
                                                                    <table id="Table3" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnSaveTop" runat="server" CssClass="clsButton" ToolTip="Click To Comply"
                                                                                    Text="Comply" Visible="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton" ToolTip="Click to close Work Order Compliance screen"
                                                                                    Text="Close" CausesValidation="False" Visible="False"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlWOGrid">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgDueJob" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                                        ToolTip="Due Job." ShowHeaderWhenEmpty="true" ShowHeader="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                        <HeaderStyle CssClass="clsdgHeader nodrag nodrop" HorizontalAlign="Left"></HeaderStyle>
                                                                        <PagerStyle HorizontalAlign="Right" BorderStyle="Solid" />
                                                                        <PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Select">
                                                                                <HeaderTemplate>
                                                                                    <input type="checkbox" id="chkSelectAll" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <div class="clstooltip" style="display: none;">
                                                                                        <b>Monitor Info:</b>&nbsp;
                                                                                        <%# Eval("TypeDet")%>
                                                                                    </div>
                                                                                    <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                        <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <div>
                                                                                        <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                                                            <img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                                                        </a>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle CssClass="hideGridColumn" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="LogBook" HeaderText="Assembly Info." HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="OnAssemblyOrComponent" HeaderText="On Assembly / Component"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ModelMonitorCode" HeaderText="Monitior Type" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="JobDescriptionDetailWeb" HeaderText="Info" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" Width="330px" />
                                                                                <ItemStyle Wrap="False" Width="330px" CssClass="TextBreak"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Freq3" HeaderText="Frequency" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SinceNew" HeaderText="Since New" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DoneAt2" HeaderText="Done At" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="DueAsOf2" HeaderText="Due As Of" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RemainingTime2" HeaderText="Remaining Time" HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField Visible="False" DataField="EstimatedDate" HeaderText="Estimated Date"
                                                                                HtmlEncode="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="StartJobDate" HeaderText="Start Date" HtmlEncode="false"
                                                                                Visible="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="EndJobDate" HeaderText="Completion Date" HtmlEncode="false"
                                                                                Visible="false">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Comply Remark">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtComplyRemark" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="200"
                                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td colspan="100%" bgcolor="White" width="0px">
                                                                                            <div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                                                                <asp:GridView ID="grdLinkActivity" runat="server" AutoGenerateColumns="False" Width="95%"
                                                                                                    BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                                                                    AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                                    SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                                                    <Columns>
                                                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                        <asp:BoundField DataField="LinkedMaintenanceTypeName" HeaderText="Linked with">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField Visible="False" DataField="MonitorType" SortExpression="MonitorType"
                                                                                                            HeaderText="Monitor Type">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="DirectiveNo" SortExpression="DirectiveNo" HeaderText="Directive Number">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                            <HeaderStyle ForeColor="White" Wrap="true" Width="330px" HorizontalAlign="Left">
                                                                                                            </HeaderStyle>
                                                                                                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="330px" CssClass="TextBreak" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="MaintenanceActionName" SortExpression="MaintenanceActionName"
                                                                                                            HeaderText="Action Type">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" align="right">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlbuttonsSave">
                                                                <ContentTemplate>
                                                                    <table id="Table4">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton" ToolTip="Click To Comply"
                                                                                    Text="Comply" Enabled="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnClose1" runat="server" CssClass="clsButton" ToolTip="Click to close Work Order Compliance screen"
                                                                                    Text="Close" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                        <!--Dummy panel to open modelpopup-->
                                                                        <tr>
                                                                            <td colspan="2" align="right">
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
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
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="tbpnlRemovalCompList" runat="server" ClientIDMode="Static">
                                                <HeaderTemplate>
                                                    Component Removal
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <asp:UpdatePanel ID="upnlRemovalComp" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <iframe id="IframeRemovalComp" scrolling="no" marginheight="0" frameborder="0" onload="autoResizeRemovalComp()">
                                                            </iframe>
                                                            </script>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                        </cc2:TabContainer>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div>
        <script type="text/javascript">
            function CallRemovalCompList() {
                document.getElementById('IframeRemovalComp').src = 'wfMultiComponentRemovalCriteria_Ajax.aspx'
            }
            function ParentCallBackFunctionForComp() {
                window.location.href = "dashboard.aspx";
            }
        </script>
    </div>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
    <!-- End-->
    <!-- Select SelectSelectLog popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySelectLog" Text="Maintenance Activity" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSelectLog" runat="server" TargetControlID="btnDummySelectLog"
        PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSelectLogStateComplete() {
            $("#btnDummySelectLog").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSelectLogWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSelectLog").attr("src", "wfSelectLog_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySelectLog").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSelectLog() {
            var SelectLogwindow = $find("<%=mdlPopupSelectLog.ClientID %>");
            //close Task Card Tool popup window
            SelectLogwindow.hide();
            //           release resources
            $("#IframeSelectLog").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSelectLog").click();
        }
    </script>
    <!-- End-->
    <!-- End-->
    </form>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ("<%= page.IsPostback%>" == "False") {
                $(".clstooltip").closest("tr").mousemove(function (event) {
                    $(this).find(".clstooltip").css({
                        "left": event.pageX + 1,
                        "top": event.pageY + 1
                    }).show();
                }).mouseout(function () { $(this).find(".clstooltip").hide(); }); ;
            }
        });
    </script>
</body>
</html>
