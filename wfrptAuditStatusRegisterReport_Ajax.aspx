<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAuditStatusRegisterReport_Ajax.aspx.vb"
    Inherits="Flypal.wfrptAuditStatusRegisterReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Status Register</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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

                                    <td class="clsFormHeader1">
                                <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lbltitle" class="clsFormHeader">Audit Status Register</span>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlBottomButtons" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnDisplayBottom" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to print report "
                                                                            Text="Print"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseBottom" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close" />
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
                                        <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblFromDate" class="clsLabelAuto">From Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxTagDateSearch" ClientIDMode="Static"
                                                                AutoPostBack="true" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <span id="lblToDate" class="clsLabelAuto">To Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToDate" CssClass="clsTextBoxTagDateSearch" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                AutoPostBack="true" runat="server"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <span id="lblAuditNo" class="clsLabelAuto">Audit No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAuditNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Audit No."
                                                                BackColor="White" AutoPostBack="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblAuditType" class="clsLabelAuto">Audit Type</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAuditType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                AutoPostBack="true" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span id="lblAuditOn" class="clsLabelAuto">Audit On</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAuditOnList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                ClientIDMode="Static" AutoPostBack="true" DataTextField="Name" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAuditOnText" runat="server" AutoPostBack="True" Visible="false"
                                                                BackColor="White" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="Span1" class="clsLabelAuto">Status</span>
                                                            <asp:DropDownList ID="CmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                ClientIDMode="Static" AutoPostBack="true">
                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Open</asp:ListItem>
                                                                <asp:ListItem Value="2">Close</asp:ListItem>
                                                                <asp:ListItem Value="3">Schedule</asp:ListItem>
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
                                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlTopButtons" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find "
                                                                        Visible="false" Text="Find Now"></asp:Button>
                                                                    <asp:Button ID="btnDisplayTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print report "
                                                                        Text="Print" Visible="false"></asp:Button>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close" Visible="false"  />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:GridView ID="dgAuditStatusRegister" runat="server" AllowPaging="True" AllowSorting="True"
                                                                AutoGenerateColumns="False" CssClass="clsGridNewStyle" PageSize="25" ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="5">
                                                                <RowStyle CssClass="clsdgItem" />
                                                               <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="AuditScheduleDateFormatted" HeaderText="Schedule Date">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditNo" SortExpression="AuditNo" HeaderText="Audit No.">
                                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Type">
                                                                        <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditDescription" SortExpression="AuditDescription" HeaderText="Description">
                                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditScheduleLocation" SortExpression="AuditScheduleLocation"
                                                                        HeaderText="Location">
                                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditOnCostCenter" SortExpression="AuditOnCostCenter"
                                                                        HeaderText="Audit On" HtmlEncode="False">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditScheduleTaskCount" SortExpression="AuditScheduleTaskCount"
                                                                        HeaderText="# Task">
                                                                        <HeaderStyle  HorizontalAlign="right" />
                                                                        <ItemStyle HorizontalAlign="right" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditExecutionTaskFindingOpenCount" SortExpression="AuditExecutionTaskFindingOpenCount"
                                                                        HeaderText="# Open Findings ">
                                                                        <HeaderStyle  HorizontalAlign="right" />
                                                                        <ItemStyle HorizontalAlign="right" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditExecutionTaskFindingCloseCount" SortExpression="AuditExecutionTaskFindingCloseCount"
                                                                        HeaderText="# Close Findings ">
                                                                        <HeaderStyle  HorizontalAlign="right" />
                                                                        <ItemStyle HorizontalAlign="right" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditStatusName" SortExpression="AuditStatusName" HeaderText="Status">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CloseDateFormatted" SortExpression="CloseDateFormatted"
                                                                        HeaderText="Close Date">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AuditScheduleNote" SortExpression="AuditScheduleNote"
                                                                        HeaderText="Note">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
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
                                    <%-- <td align="right">
                                    <asp:UpdatePanel ID="upnlBottomButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnDisplayBottom" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print report "
                                                            Text="Print"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseBottom" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
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
                <div style="position: fixed; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
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
