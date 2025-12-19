<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectDailyStatus_Ajax.aspx.vb" Inherits="Flypal.wfSelectDailyStatus_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat ="server" >
    <title>Aircraft Daily Status Selection</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
			function openledgersame(FileN./ame)
               {
                  window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

               }
    </script>
    
        <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
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
                                            <span id="lbltitle" class="clsFormHeader">Aircraft Daily Status Selection</span>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDone" TabIndex="0" runat="server" CausesValidation="False"
                                                                    Text="Done"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" TabIndex="0" runat="server" CausesValidation="False"
                                                                    Text="Close" ToolTip="Click to go back to the previous page"></asp:Button>
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
                                <asp:UpdatePanel ID="upnlGridTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblSelectInformation" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridMonitorTypeList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgSelectInformationList" runat="server"  DESIGNTIMEDRAGDROP="139"
                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                            AutoGenerateColumns="False" AllowSorting="True" AllowPaging="true" PageSize="25">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"/>
                                            <PagerStyle HorizontalAlign="Right"/>
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                    <HeaderStyle ></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                    <HeaderStyle ></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                    <HeaderStyle ></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive Number">
                                                    <HeaderStyle></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Show In C of A">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'>
                                                        </asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours"></asp:BoundField>
                                                <asp:BoundField DataField="Note" HeaderText="Note"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlGridCertificateList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgCertificateList" runat="server" Visible="False" AutoGenerateColumns="False"
                                        AllowSorting="True" ForeColor="White" CssClass="clsGrid" PageSize="25" AllowPaging="true" >
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ></HeaderStyle>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"/>
                                        <PagerStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField Visible="False" DataField="SerialNo" HeaderText="Sr. No.">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Name">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="No.">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ElapsedDays" HeaderText="Elapsed Days"></asp:BoundField>
                                            <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days"></asp:BoundField>
                                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:ButtonField Visible="False" Text="Select" HeaderText="Select" CommandName="Select">
                                            </asp:ButtonField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectCertificate" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                     <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                    </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right" >
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDone" TabIndex="0" runat="server" CausesValidation="False"
                                                        Text="Done"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" TabIndex="0" runat="server" CausesValidation="False"
                                                        Text="Close" ToolTip="Click to go back to the previous page"></asp:Button>
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
    </form>
</body>
</html>
