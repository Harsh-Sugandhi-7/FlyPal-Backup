<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateInstalledAssemblyHistory_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateInstalledAssemblyHistory_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Installed Assembly List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
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
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">History for Installed Assembly</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: 480px; z-index: 10000;">
                                            <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Assembly Installation Information</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblRemovalDate" class="clsLabel">Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" Width="80px"
                                                Enabled="false" onchange="ValidateDateText(this,'Date_watermarkextender');"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Date_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                        <td>
                                            <span id="lblModel" class="clsLabelAuto">Model</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                ReadOnly="True" BackColor="#E0E0E0" Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                Width="120px" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                 </fieldset>
                                    
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblInstalledAssemblyList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgInstalledList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true" CssClass="clsGrid">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Reg No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                                HeaderText="Assembly Info.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InstalledOnFormatted" SortExpression="InstalledOnFormatted"
                                                                HeaderText="Installed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period "
                                                                HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ValueFormatted" SortExpression="ValueFormatted" HeaderText="Value"
                                                                HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField HeaderText="View" Text="View" CommandName="ViewRec" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close History of Installed Assembly screen"
                                            Text="Close" CausesValidation="False"></asp:Button>
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
            parent.ParentCallBackFunctionForInstallationHistory();
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
                    parent.IFrameInstallationHistoryStateComplete();
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
