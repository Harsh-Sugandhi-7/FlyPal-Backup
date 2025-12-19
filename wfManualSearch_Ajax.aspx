<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualSearch_Ajax.aspx.vb"
    EnableEventValidation="true" Inherits="Flypal.wfManualSearch_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFileExcel() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="0" leftmargin="2" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="Table1" class="clstablelistout">
            <tr>
                <td>
                    <table id="Table2" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblManual" class="clsFormHeader">Manual Report</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table3" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                                    Text="Print" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnExport" TabIndex="0" runat="server" ToolTip="Click to Export to Excel the list of Manuals"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                    Text="Export to Excel"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close Manual Report screen."
                                                                    Text="Close" CausesValidation="False"></asp:Button>
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
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                <script type="text/javascript">
                                    function showTextField(elem) {

                                        var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                        var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                        var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                        var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                        if (elem.selectedIndex == 0) {
                                            txtFromDateobj.style.display = 'none';
                                            txtToDateobj.style.display = 'none';
                                            lblFromDateobj.style.display = 'none';
                                            lblToDateobj.style.display = 'none';
                                        }

                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 55px;">
                                                                <span id="lblManualName" class="clsLabelAuto">Manual Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtManualName" runat="server" CssClass="clsTextBoxTagSearch" Width="275px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>

                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 55px;">
                                                                            <span id="lblLookFor" class="clsLabel">Look For</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter a string to search through Manual &amp; Revision"
                                                                                MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Label1" class="clsLabelAuto">(Enter Text to search.)</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>


                                                        </tr>
                                                    </table>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 55px;">
                                                                <span id="lblDate" class="clsLabelAuto">Date</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" onchange="showTextField(this);"
                                                                    AutoPostBack="true">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                    <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                    <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                    <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                    <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                    <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 90px;"></td>
                                                            <td>
                                                                <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td></td>

                                                <td align="right">
                                                    <%--<asp:Button ID="btnSearch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Manuals as per searching criteria"
                                                    Text="Find Now"></asp:Button>--%>

                                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Manuals as per searching criteria" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <%--<td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 55px;">
                                                            <span id="lblLookFor" class="clsLabel">Look For</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter a string to search through Manual &amp; Revision"
                                                                MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="Label1" class="clsLabelAuto">(Enter Text to search.)</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>--%>
                                                <%--<td align="right">
                                                <%--<asp:Button ID="btnSearch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Manuals as per searching criteria"
                                                    Text="Find Now"></asp:Button>

                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Manuals as per searching criteria" />
                                            </td>--%>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblList" runat="server" CssClass="clsLabelHeader">List</asp:Label>
                                                </td>
                                                <%--<td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close Manual Report screen."
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgManualRevList" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                        DataKeyNames="RevisionID" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true"
                                                        PageSize="25" AutoGenerateColumns="False" AllowPaging="True" GridLines="Horizontal" CellPadding="3">
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="RevisionID" HeaderText="RevisionID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="ManualID" HeaderText="ManualID"></asp:BoundField>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="Manual Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ManualNo" HeaderText="Manual No." SortExpression="ManualNo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ApplicableFor" HeaderText="Applicable For">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MCategoryName" HeaderText="Category">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ShortDesc" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Note" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IsInUseTag" HeaderText="In Use">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="RevisionID" HeaderText="RevisionID">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="No" HeaderText="No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RevNo" HeaderText="Revision No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RevDate" HeaderText="Effective Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EffectiveDate" HeaderText="Next Revision Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RevNote" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField CommandName="ViewRec" HeaderText="Attach" Text="View" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsAttachmentAdded" HeaderText="RevImageSize"></asp:BoundField>
                                                        </Columns>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                        Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close Manual Report screen."
                                                        Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            var dd = document.getElementById("cmbPeriod");
            showTextField(dd);
        }
    </script>
</body>
</html>
