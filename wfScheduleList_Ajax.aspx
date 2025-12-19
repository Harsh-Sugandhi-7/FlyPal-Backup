<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfScheduleList_Ajax.aspx.vb"
    Inherits="Flypal.wfScheduleList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Enquiry List</title>
   <%-- <link id="MainStyle" type="text/css" rel="stylesheet" />--%>
     <link id="Link1" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <asp:UpdatePanel ID="upnlRouteScheduleList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblScheduleList" runat="server" CssClass="clstitle1">Schedule List</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSearch" class="clsLabel" style="height: 10px; width: 48px;">Search</span>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsComboBox_Ajax" Width="170px"
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem Value="0">All</asp:ListItem>
                                                                            <asp:ListItem Value="1" Selected="True">Date</asp:ListItem>
                                                                            <asp:ListItem Value="2">Route Name</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <span id="L1" class="clsLabel" style="width: 20px;"></span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True"
                                                                            Visible="False">
                                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                            <asp:ListItem Value="1" Selected ="True">Last 1 Week</asp:ListItem>
                                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRouteName" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                            MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox">
                                                            </cc2:TextBoxWatermarkExtender>
                                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                    ValidationGroup="a" ToolTip="Click to find list of Schedule as per searching criteria"
                                                    CausesValidation="true"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 3px;" colspan="2">
                                        <span id="lblInfo" class="clsLabelAuto" style="display: none"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Schedule as per criteria : Record(s) found</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" Text="Add New"
                                                                                    ToolTip="Click to Add New Schedule" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" Text="Print"
                                                                                    ToolTip="Click to Print List of  Schedule" CausesValidation="False" Visible ="false"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                                                    ToolTip="Click to close List of Schedule screen" CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:GridView ID="dgScheduleList" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                ShowHeaderWhenEmpty="true" EnableViewState="true" AllowSorting="True" AllowPaging="True"
                                                                AutoGenerateColumns="False" PageSize="5">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="RouteName" SortExpression="RouteName" HeaderText="Route Name">
                                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ValidFromFormatted" HeaderText="Valid From" SortExpression="ValidFromFormatted">
                                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ValidToFormatted" HeaderText="Valid To" SortExpression="ValidToFormatted">
                                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note"  HeaderText="Note">
                                                                        <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                CausesValidation="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" Text="Add New"
                                                                ToolTip="Click to Add New Schedule" CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="BtnPrint" runat="server" CssClass="clsButton_Ajax" Text="Print" ToolTip="Click to Print List of  Schedule"
                                                                CausesValidation="False" Visible ="false"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to close List of Schedule screen"
                                                                CausesValidation="False"></asp:Button>
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
    </form>
</body>
</html>
