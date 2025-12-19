<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfComponentReservationList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfComponentReservationList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>List of Reserved Component</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td width="100%">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblList" runat="server" CssClass="clstitle1">List of Reserved Component</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
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
                                                                            </td>
                                                                            <td>
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
                                                                            <td align="left">
                                                                                <span id="Span1" class="clsLabel">Part No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBox_Ajax" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkClosed" runat="server" CssClass="clsLabel" TextAlign="right"
                                                                                    AutoPostBack="true" Text="Closed Reserved Component"></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblSearch" class="clsLabel">Receipt No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbReceiptText" runat="server" CssClass="clsComboBox_Ajax"
                                                                                    AutoPostBack="True" DataValueField="Text" DataTextField="Text">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="clsTextBoxSmall_Ajax" MaxLength="6"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <span id="Span2" class="clsLabel">Serail No.</span>
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <asp:TextBox ID="txtSerailNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="100">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                    ToolTip="Click to find list as per searching criteria" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Record"
                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print "
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close "
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Reserved Component as per criteria: Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgComponentReservationList" runat="server" AllowPaging="True" AllowSorting="True"
                                                            AutoGenerateColumns="False" CssClass="clsGrid" PageSize="25" ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="ReserveForDateFormatted" HeaderText="Reserved Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Reserved For Reg.No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="ReservedBy" HeaderText="Reserved By">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--9--%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--10--%>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="EnabledDisabled" HeaderText="EnabledDisabled"></asp:BoundField>
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
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlComponentUnscheduleUsedList" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblComponentUnscheduleUsedResult" runat="server" CssClass="clsLabelAuto"
                                                            Font-Bold="True">List of Unscheduled Issued Component as per criteria: Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgComponentUnscheduleUsedList" runat="server" AllowPaging="True"
                                                            AllowSorting="True" AutoGenerateColumns="False" CssClass="clsGrid" PageSize="25"
                                                            ShowHeaderWhenEmpty="True" DataKeyNames="ID,PartNo,SerialNo">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="ReserveForDateFormatted" HeaderText="Reserved Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="RegNo" HeaderText="Reserved For Reg.No.">
                                                                    <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="PartNo" HeaderText="Part No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="ReservedBy" HeaderText="Reserved By">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="IssueNo" HeaderText="Issue No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="IssueTo" HeaderText="Issue To">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--11--%>
                                                                <asp:TemplateField HeaderText="Reallocate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Reallocate" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                            CommandName="Reallocate" Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--12--%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
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
                                    <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Record"
                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print"
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close"
                                                            Text="Close" CausesValidation="False"></asp:Button>
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
