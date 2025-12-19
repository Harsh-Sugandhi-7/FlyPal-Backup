<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateRemovedCompHistory_AJAX.aspx.vb"
    Inherits="Flypal.wfUpdateRemovedCompHistory_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Removed Component List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFilel() {
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
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">History for Removed Component</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                                <asp:UpdatePanel ID="upnlRemovalDate" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet"  style="padding: 0px 4px 0px 0px; z-index: 10000;border-width: 1px">
                                            <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Component Removal Information</b></legend>
                                            <table id="Table3" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRemovalDate" runat="server" CssClass="clsLabel">Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="calDate" CssClass="clsTextBox_Ajax" Width="90px"
                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="calDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPart" runat="server" CssClass="clsLabelAuto">Part</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            ReadOnly="True" BackColor="#E0E0E0" Width="120px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                            ReadOnly="True" BackColor="#E0E0E0" Width="120px"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel ID="upnlHistoryCard" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkHistoryCard" runat="server" CssClass="clsLinkButton" Font-Italic="true"
                                                                                Font-Size="9pt">View History Card</asp:LinkButton>
                                                                        </td>
                                                                        <td align="right">
                                                                            <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblRemovedCompList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRemovedCompList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgRemovedCompList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            CssClass="clsGrid" DataKeyNames="CompStatusID" ShowHeaderWhenEmpty="True" TabIndex="7">
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField DataField="CompStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" SortExpression="PeriodNameForweb"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ValueFormatted" HeaderText="Value" SortExpression="ValueFormatted"
                                                    HtmlEncode="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:ButtonField>
                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                    HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn">
                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                    <ItemStyle CssClass="hideGridColumn" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close History of Removed Component screen" />
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
    <div>
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
    </div>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForRemHistory();
            return false;
        }
    </script>
    <%--End--%>
    <div>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
            SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameRemHistoryStateComplete();
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
    </div>
    </form>
</body>
</html>
