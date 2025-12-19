<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Assembly Directive Status History</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
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
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
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
                            <td colspan="4">
                                <span id="lbltitle" class="clstitle1">History for Assembly Directive Status</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblCompInformation" class="clsLabelHeader">Assembly Information</span>
                                                </td>
                                                <td colspan="2">
                                                    <span id="lblServiceInformation" class="clsLabelHeader">Directive Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtATA" runat="server" CssClass="clsTextBox1_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblCodeFormNo" class="clsLabelAuto">Code/Form No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCodeFormNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblModel" class="clsLabelAuto">Model</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblMonitorInfo" class="clsLabelAuto">Monitor Info.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMonitorInfo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        ReadOnly="True" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblReference" class="clsLabelAuto">Reference</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <span id="Label1" class="clsLabelAuto">Mod No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtModNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabelAuto" style="display: none;">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        Visible="false" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <span id="lblFrequency" class="clsLabelAuto" style="display: none;">Frequency</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        Visible="false" BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table21" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Text="Save" ToolTip="Click to Save Directive" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Compliance History"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List of Assembly Directive Status screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgMonitorModStatusList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            CssClass="clsGrid" ShowHeaderWhenEmpty="True" PageSize="5" ToolTip="Assembly Directive Status History">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="ModelMonitorModID" HeaderText="ModelMonitorModID" Visible="False">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATA" HeaderText="ATA  Chapter" SortExpression="ATAChapter"
                                                    Visible="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="false">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="true" Width="200px" CssClass="TextBreak" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FrequencyValueFormatted" HeaderText="Frequency" HtmlEncode="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MachineName" HeaderText="Aircraft">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Work Order No." HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="100"
                                                            Text='<%# DataBinder.Eval(Container.DataItem, "DoneWONo") %>'>
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvtxtWONo" runat="server" ControlToValidate="txtWONo" CssClass="clsLabelAuto"
                                                            Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actual Man Hours" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax"
                                                            Enabled="false" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "TotalRequiredManHoursFormatted") %>'
                                                            ToolTip="Enter Required Man Hours">
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvRequiredManHours" runat="server" ControlToValidate="txtRequiredManHours"
                                                            CssClass="clsLabelAuto" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDoneRemark" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                            MaxLength="500" Text='<%# DataBinder.Eval(Container.DataItem, "DoneRemark") %>'
                                                            TextMode="MultiLine" ToolTip="Enter Remark">
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvDoneRemark" runat="server" ControlToValidate="txtDoneRemark"
                                                            CssClass="clsLabelAuto" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField Text="View" CommandName="ViewRec" HeaderText="View" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveNew" runat="server" CssClass="clsButton_Ajax" Text="Save"
                                                        ToolTip="Click to Save Directive" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Compliance History"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to close List of Assembly Directive Status screen" />
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
            parent.ParentCallBackFunctionForDirectiveHistory();
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
                    parent.IFrameDirectiveHistoryStateComplete();
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
