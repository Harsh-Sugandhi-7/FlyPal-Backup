<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingAuditScheduleListForExecution_AJAX.aspx.vb"
    Inherits="Flypal.wfPendingAuditScheduleListForExecution_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Pending Audit Schedule List For Compliance</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
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
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                  <td colspan="2" class="clsFormHeader1">
                                <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Audit Schedule List For Compliance</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2" cellspacing="1" cellpadding="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"
                                                                        ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlAuditDate" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblAuditDate" runat="server" CssClass="clsLabelAuto">Compliance Date </asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtAuditDate" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                                        ClientIDMode="Static" onchange="ValidateDateText(this,'AuditDate_watermarkextender');"
                                                                        AutoPostBack="True"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtAuditDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAuditDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtAuditDate" ID="AuditDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDays" class="clsLabel">Show Scheduled Audits in next upcoming</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtUpcomingDays" runat="server" CssClass="clsTextBoxMegaSmall_Ajax">30</asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDays1" class="clsLabel">days</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3">
                                                <tr>
                                                    <td align="right">
                                                       <%-- <asp:Button ID="btnFindNow" runat="server" Text="Find Now" ToolTip="Click to find Audit Schedule List as per searching criteria"
                                                            CssClass="clsButton"></asp:Button>--%>
                                                           <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find Audit Schedule List as per searching criteria"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnBackTop" TabIndex="0" runat="server" CssClass="clsButton" Text="Back"
                                                            ToolTip="Click to go back to the previous page" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgAuditScheduleList" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                ShowHeaderWhenEmpty="true" PageSize="25" AllowPaging="true" GridLines="Horizontal" CellPadding="5"
                                                AutoGenerateColumns="False">
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="ScheduleDateFormatted" HeaderText="Schedule Date">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AuditText" SortExpression="AuditText" HeaderText="Audit No.">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AuditTypeName" SortExpression="AuditTypeName" HeaderText="Audit Type">
                                                        <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference No.">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        <FooterStyle Wrap="False"></FooterStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField Visible="False" DataField="Note" SortExpression="Note" HeaderText="Note">
                                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%-- <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton" Text="Back"
                                                        ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
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
</body>
</html>
