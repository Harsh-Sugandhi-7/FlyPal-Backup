<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateRemovedAssemblyHistory_AJAX.aspx.vb"
    Inherits="Flypal.wfUpdateRemovedAssemblyHistory_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Removed Assembly List</title>
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
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clsFormHeader">History for Removed Assembly</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close History of Removed Assembly screen" />
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
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlRemovalDate" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; z-index: 10000;">
                                                <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Assembly Removal Information</b></legend>
                                                <table id="Table3">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblRemovalDate" runat="server" CssClass="clsLabel">Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="calDate" CssClass="clsTextBoxTagDateSearch" Width="90px"
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="calDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBoxTagDateSearch" MaxLength="50"
                                                                ReadOnly="True" BackColor="#E0E0E0" Width="120px"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagDateSearch" MaxLength="50"
                                                                ReadOnly="True" BackColor="#E0E0E0" Width="120px"></asp:TextBox>
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
                                    <asp:Label ID="lblRemovedAssemblyList" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlRemovedAssemblyList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgRemovedAssemblyList" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="AssemblyStatusID"
                                                PageSize="5" ShowHeaderWhenEmpty="True" TabIndex="7">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:BoundField DataField="AssemblyStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info" SortExpression="AssemblyInfo"
                                                        Visible="False">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On"
                                                        HtmlEncode="False">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period"
                                                        SortExpression="PeriodNameForweb" HtmlEncode="False">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ValueFormatted" HeaderText="Value"
                                                        SortExpression="ValueFormatted" HtmlEncode="False">
                                                        <HeaderStyle  ForeColor="Black" HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                        <HeaderStyle  HorizontalAlign="Left" />
                                                          <ItemStyle ForeColor="blue" />
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

                function SetPageLayout() {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                    ReSetPageLayout();
                    onResize();//for Top bottom link
                <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                    var windowheight = $(window).height();
                    if (tempMargtop >= windowheight) {
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                    }

                }
            </script>
            <%--End--%>
        </div>
    </form>
</body>
</html>
