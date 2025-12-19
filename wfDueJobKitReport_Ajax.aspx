<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDueJobKitReport_Ajax.aspx.vb"
    Inherits="Flypal.wfDueJobKitReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Inspection Spares Required Report</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td colspan="3" class="clsFormHeader1Newstyle">
                                <span id="lblTitle" class="clsFormHeader">Inspection Spares Required</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td colspan="2" align="left">
                                <span id="lblDuePeriodList" class="clsLabelHeader">Due Period List</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table id="Table3" class="clsTable1" cellpadding="0">
                                    <tr>
                                        <td>
                                            <span id="lblAsOnDat" class="clsLabel">As On Date</span>
                                        </td>
                                        <td>
                                            <table id="Table10" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="Table16" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAsOnDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'AsOnDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="AsOnDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Aircraft" class="clsLabel">Aircraft</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataValueField="ID"
                                                DataTextField="RegNo">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblStore" class="clsLabel">Store</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataValueField="ID"
                                                DataTextField="Name">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="dgDuePeriod" runat="server" AutoGenerateColumns="False"
                                   CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Due List.">
                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Limit">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLimit" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top" align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0" bordercolorlight="#0">
                                            <tr>
                                                <td align="right">
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Due Jobs as per searching criteria"
                                                        Text="Find Now"></asp:Button>--%>

                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Due Jobs as per searching criteria"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="3" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" cellspacing="0" bordercolorlight="#0">
                                            <tr>
                                                <td align="right">
                                                    <asp:RadioButton ID="rbSummary" runat="server" CssClass="clsRadioButton" Text="Summary"
                                                        GroupName="a" Checked="True"></asp:RadioButton>
                                                </td>
                                                <td align="right">
                                                    <asp:RadioButton ID="rbDetail" runat="server" CssClass="clsRadioButton" Text="Detail"
                                                        GroupName="a"></asp:RadioButton>
                                                </td>
                                                <td align="right">
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrint" runat="server" ToolTip="Click to Display "
                                                        Text="Display"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to close List of Inspection Spares Required screen"
                                                        Text="Close"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Due Jobs as per criteria :  Record(s) found.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgDueJob" runat="server" AutoGenerateColumns="False"
                                                      CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Due Job." AllowSorting="True" EnableViewState="false">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="LogBook" SortExpression="LogBook" HeaderText="Assembly Info.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OnAssemblyOrComponent" SortExpression="OnAssemblyOrComponent"
                                                                HeaderText="On Assembly / Component">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DataType" SortExpression="DataType" HeaderText="Data Type">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="JobDescriptionDetailWeb" SortExpression="JobDescriptionDetailWeb"
                                                                HeaderText="Info" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Freq3" SortExpression="Freq3" HeaderText="Frequency">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SinceNew" SortExpression="SinceNew" HeaderText="Since New">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                 <ItemStyle Width ="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DoneAt2" SortExpression="DoneAt2" HeaderText="Done At">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DueAsOf2" SortExpression="DueAsOf2" HeaderText="Due At">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Width ="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingTime2" SortExpression="RemainingTime2" HeaderText="Remaining">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Width ="75px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
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
    </form>
</body>
</html>
