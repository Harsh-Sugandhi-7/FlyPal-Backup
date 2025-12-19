<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogListToImport_Ajax.aspx.vb"
    Inherits="Flypal.wfLogListToImport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Import Logs</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .GbiHighlight
        {
            background-color: Aqua;
        }
    </style>
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
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">Import Logs</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the Aircraft"
                                    ValidateEmptyText="true" ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="validateAircraft"
                                    ValidationGroup="1"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function validateAircraft(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbAircraft");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlLastLogDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                    CausesValidation="true" ValidationGroup="1" ClientIDMode="Static" DataTextField="RegNo"
                                                                    DataValueField="ID">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelHeader">Last Log Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgLastLogDetails" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                        CssClass="clsGrid" PageSize="25">
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogTextNo" HeaderText="Log No." SortExpression="LogTextNo">
                                                                <HeaderStyle ForeColor="White" Wrap="False" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LogPageNoFormatted" HeaderText="Log Page No." SortExpression="LogPageNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FlightNo" HeaderText="Flight No." SortExpression="FlightNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SouLocalDateTimeFormatted" HeaderText="Departure (Date Time)"
                                                                SortExpression="SouLocalDateTimeFormatted">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SouUniverseDateTimeFormatted" HeaderText="Departure UTC (Date Time)"
                                                                SortExpression="SouUniverseDateTimeFormatted">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SouPlaceName" HeaderText="From" SortExpression="SouPlaceName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesLocalDateTimeFormatted" HeaderText="Arrival (Date Time)"
                                                                SortExpression="DesLocalDateTimeFormatted">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesUniverseDateTimeFormatted" HeaderText="Arrival UTC (Date Time)"
                                                                SortExpression="DesUniverseDateTimeFormatted">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesPlaceName" HeaderText="To" SortExpression="DesPlaceName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time" SortExpression="TimeInAir">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AirframeTotalCyclesOrLandings" HeaderText="Cycles/Landings"
                                                                SortExpression="AirframeTotalCyclesOrLandings">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
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
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                            CausesValidation="true" ValidationGroup="1" Text="Import" ToolTip="Click to Import" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgImportLogList" runat="server" AutoGenerateColumns="true" ShowHeaderWhenEmpty="True"
                                                        AllowPaging="true" CssClass="clsGrid" PageSize="25">
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                    <input type="checkbox" id="chkSelectAll" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="checkbox" runat="server" clientidmode="Static" id="chkSelect" name="chkSelect"
                                                                        class="cbSelectRow"></input>
                                                                </ItemTemplate>
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        CausesValidation="true" ValidationGroup="1" Text="Import" ToolTip="Click to Import" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List" />
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForImportLogs();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
            SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameImportLogsStateComplete();
                }
       
      
        });
        <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
                    
            }

            function SetPageLayout()
            {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
                <% End if %>
            }
            function ReSetPageLayout()
            {
            $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
                var windowheight=$(window).height();
                if (tempMargtop>=windowheight)
                {
                $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
                }
                else
                {
                var margintop=(windowheight/2)-(tempMargtop/2);
                $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                }
       
            }
    </script>
    <%--End--%>
    </form>
</body>
</html>
