<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateComplyHistoryCompMonitorModStatusList_AJAX.aspx.vb"
    Inherits="Flypal.wfUpdateComplyHistoryCompMonitorModStatusList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Component Modification Status History</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
                            <td colspan="1">
                                <span id="lbltitle" class="clstitle1">History for Component Modification Status</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlCompDetail" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdsCompdetail" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="ldCompdetail" style="font-weight: bold"><b>Component Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblATA" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtATA" runat="server" CssClass="clsTextBox1_Ajax" ReadOnly="True"
                                                                        BackColor="#E0E0E0" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPart" runat="server" CssClass="clsLabelAuto">Part</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBox1_Ajax" ReadOnly="True"
                                                                        BackColor="#E0E0E0" MaxLength="50" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox2_Ajax" ReadOnly="True"
                                                                        BackColor="#E0E0E0" TextMode="MultiLine" Height="25px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style1">
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                                </td>
                                                                <td class="style1">
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox1_Ajax" ReadOnly="True"
                                                                        BackColor="#E0E0E0" MaxLength="50" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="1" valign="top">
                                            <asp:UpdatePanel ID="upnlModInfo" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="Legend1" style="font-weight: bold"><b>Modification Information</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCodeFormNo" runat="server" CssClass="clsLabelAuto">Code/Form No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCodeFormNo" runat="server" CssClass="clsTextBox1_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblModNo" runat="server" CssClass="clsLabelAuto">Mod No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtModNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox1_Ajax"
                                                                        ReadOnly="True" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblMonitorInfo" runat="server" CssClass="clsLabelAuto">Monitor Info.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMonitorInfo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox1_Ajax"
                                                                        ReadOnly="True" Width="225px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFrequency" runat="server" CssClass="clsLabelAuto">Frequency</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBox2_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" TextMode="MultiLine" Height="25px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style1">
                                                                    <asp:Label ID="lblReference" runat="server" CssClass="clsLabelAuto">Reference</asp:Label>
                                                                </td>
                                                                <td class="style1">
                                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox1_Ajax" BackColor="#E0E0E0"
                                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="1">
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblCompInformation" class="clsLabelHeader">Component Information</span>
                                                </td>
                                                <td colspan="2">
                                                    <span id="lblServiceInformation" class="clsLabelHeader">Modification Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtATA" runat="server" CssClass="clsTextBox1_Ajax" ReadOnly="True"
                                                        BackColor="#E0E0E0"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                        BackColor="#E0E0E0" MaxLength="50"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                        BackColor="#E0E0E0" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblFrequency" class="clsLabelAuto">Frequency</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                        ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <span id="lblReference" class="clsLabelAuto">Reference</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReference" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="1" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveNewTop" runat="server" CssClass="clsButton_Ajax" Text="Save"
                                                        ToolTip="Click to Save Modification"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Compliance History"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close Component Modification Status screen" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgMonitorModStatusList" runat="server" CssClass="clsGrid" ToolTip="Component Modification Status History"
                                            ShowHeaderWhenEmpty="True" PageSize="5" AllowSorting="True" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="PartMonitorModID" HeaderText="PartMonitorModID">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA" Visible="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description"
                                                    Visible="False">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MachineName" HeaderText="Aircraft">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ModelNameSerialNo" HeaderText="Assembly">
                                                    <ItemStyle Wrap="true"></ItemStyle>
                                                    <HeaderStyle ForeColor="White" Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="False">
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Work Order No." HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtWONo" runat="server" CssClass="clsTextBoxDate_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "DoneWONo") %>'
                                                            MaxLength="100"></asp:TextBox>
                                                        <asp:CustomValidator ID="cvtxtWONo" runat="server" CssClass="clsLabelAuto" Display="None"
                                                            ControlToValidate="txtWONo" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actual Man Hours" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRequiredManHours" runat="server" CssClass="clsTextBoxRightAlignSmall1_Ajax" Enabled ="false"
                                                            ToolTip="Enter Required Man Hours" Text='<%# DataBinder.Eval(Container.DataItem, "TotalRequiredManHoursFormatted") %>'
                                                            MaxLength="8">
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvRequiredManHours" runat="server" CssClass="clsLabelAuto"
                                                            Display="None" ControlToValidate="txtRequiredManHours" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDoneRemark" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                            TextMode="MultiLine" ToolTip="Enter Remark" Text='<%# DataBinder.Eval(Container.DataItem, "DoneRemark") %>'
                                                            MaxLength="500">
                                                        </asp:TextBox>
                                                        <asp:CustomValidator ID="cvDoneRemark" runat="server" CssClass="clsLabelAuto" Display="None"
                                                            ControlToValidate="txtDoneRemark" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
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
                        </tr>
                        <tr>
                            <td colspan="1" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveNew" runat="server" CssClass="clsButton_Ajax" Text="Save"
                                                        ToolTip="Click to Save Modification"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print Compliance History"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close Component Modification Status screen" CausesValidation="False">
                                                    </asp:Button>
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
            parent.ParentCallBackFunctionForCompDirectiveHistory();
            return false;
        }
    </script>
    <%--End--%>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForCompDirectiveHistory();
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
                    parent.IFrameCompDirectiveHistoryStateComplete();
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
    </div>
    </form>
</body>
</html>
