<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectLog_Ajax.aspx.vb"
    Inherits="Flypal.wfSelectLog_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Select Log</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="3" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Select Log</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSearchCriteria" runat="server" CssClass="clsLabelHeader">Search Criteria</asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlDet">
                                    <ContentTemplate>
                                        <table id="Table3" border="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAirCraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbMachineList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="RegNo"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                    <asp:CustomValidator ID="cvMachineList" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please Select Aircraft"
                                                        Display="None" ControlToValidate="cmbMachineList" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <asp:PlaceHolder id="phhide" runat="server" visible="false">                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                        Width="32px">From</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table4">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="CalFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                    Visible="False" onchange="ValidateDateText(this,'WOAsOnDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="CalFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="CalFromDate" ID="WOAsOnDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table5">
                                                        <tr>
                                                            <td>
                                                                <td style="height: 24px">
                                                                    <asp:TextBox runat="server" ID="CalToDate" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                        Visible="False" AutoPostBack="true" onchange="ValidateDateText(this,'CalToDate_watermarkextender');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="CalToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="CalToDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="CalToDate" ID="CalToDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                                        WatermarkCssClass="clsDateTextBox" Enabled="True">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            </asp:PlaceHolder>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="height: 54px" colspan="2" align="right">
                                <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
                                    Visible="False" ToolTip="Click to get the list as per the search criteria." Text="Find Now">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">Note: Please select Log after which installation/removal has been done.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlResult">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGrid">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgLogList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="List of logs."
                                            AutoGenerateColumns="False" AllowSorting="True">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                              <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="LogID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField DataField="LogDateFormatted" HeaderText="Log Date">
                                                    <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LogNo" SortExpression="LogNo" HeaderText="Log No.">
                                                    <HeaderStyle ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LogPageNoFormatted" SortExpression="LogPageNoFormatted"
                                                    HeaderText="TLP No.">
                                                    <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" wrap="false"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TimeInAir" SortExpression="TimeInAir" HeaderText="Time In Air">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Col1Diff" SortExpression="Col1Diff" HeaderText="Hours diff.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Col1Final" SortExpression="Col1Final" HeaderText="Hours final">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="Col2Diff" SortExpression="Col2Diff" HeaderText="Landings diff.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Col2Final" SortExpression="Col2Final" HeaderText="Landings/Cycles final">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="Col3Diff" SortExpression="Col3Diff" HeaderText="Cycles diff.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Col3Final" SortExpression="Col3Final" HeaderText="Cycles final">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="Col4Diff" SortExpression="Col4Diff" HeaderText="NG Cycles diff.">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Col4Final" SortExpression="Col4Final" HeaderText="NF Cycles final">
                                                    <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
                                                <asp:BoundField DataField="Col3DiffPeriodID" HeaderText="Col3DiffPeriodID" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="Table2" cellspacing="0" align="right">
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlCancel">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnCancel" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to close Select Log screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForSelectLog();
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
                    parent.IFrameSelectLogStateComplete();
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
               var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
                  {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
                  }
                  else
                  {
                  var margintop=(windowheight/2)-(tempMargtop/2);
                   $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                  }
            }
        </script>
        <%--End--%>
    </div>
    </form>
</body>
</html>
