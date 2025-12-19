<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualAvailabilityStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfManualAvailabilityStatus_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual Revision Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="0" leftmargin="2" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="Table1">
        <tr>
            <td>
                <table class="clstablelistin" id="Table2" >
                    <tr>
                        <td class="clsFormHeader1Newstyle">
                             <table width="100%">
                                 <tr>
                                     <td>
                                         <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                             <ContentTemplate>
                                                 <asp:Label ID="lblManual" runat="server" CssClass="clsFormHeader">Manual Report</asp:Label>
                                             </ContentTemplate>
                                         </asp:UpdatePanel>
                                     </td>
                                     <td align="right">
                                         <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                             <ContentTemplate>
                                                 <table id="Table3" border="0">
                                                     <tr>
                                                         <td>
                                                             <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrintTop" runat="server" ToolTip="Click to print."
                                                                 Text="Print" CausesValidation="False"></asp:Button>
                                                         </td>
                                                         <td>
                                                             <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCloseTop" runat="server" ToolTip=" Click to close screen."
                                                                 Text="Close" CausesValidation="False"></asp:Button>
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
                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 85px;">
                                                            <span id="lblManualName" class="clsLabelAuto">Manual Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtManualName" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="lblDueRange" class="clsLabelAuto">Due Range</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ID="txtDueRange" runat="server" 
                                                                MaxLength="3">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="lblDays" class="clsLabel">Days</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td align="right">
                                                <%--<asp:Button ID="btnSearch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Manual Availability Status as per searching criteria"
                                                    Text="Find Now"></asp:Button>--%>

                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find List of Manual Availability Status as per searching criteria" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <%--<td align="right">
                                                <%--<asp:Button ID="btnSearch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Manual Availability Status as per searching criteria"
                                                    Text="Find Now"></asp:Button>

                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find List of Manual Availability Status as per searching criteria" />
                                            </td>--%>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblList" runat="server" CssClass="clsLabelHeader">List</asp:Label>
                                            </td>
                                           <%-- <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrintTop" runat="server" ToolTip="Click to print."
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCloseTop" runat="server" ToolTip=" Click to close screen."
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="dgManualAvailabilityStatusList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                    DataKeyNames="LastRevAttachmentCount" ShowHeaderWhenEmpty="True" PageSize="25"
                                                    AutoGenerateColumns="False" AllowPaging="True">
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ManualName" HeaderText="Manual Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ManualApplicableFor" HeaderText="Applicable For">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ManualDescription" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RevNo" HeaderText="Rev. No.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Rev. Remark">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RevNote" HeaderText="Rev. Note">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EffectiveDate" HeaderText="Rev. Valid Up To">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FromDate" HeaderText="Sub. Valid From">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ToDate" HeaderText="Sub. Valid Till">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField CommandName="ViewAttachments" HeaderText="Attachments" Text="View" >
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>--%>


                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="View" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="ViewAttachments"
                                                                    Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"
                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        
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
                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table4" border="0">
                                        <tr>
                                            <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server"  ToolTip="Click to print."
                                                    Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server"  ToolTip=" Click to close screen."
                                                    Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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
    <!--ManualRevisionAttach Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyManualRevisionAttach" Text="ManualRevisionAttach"
            CausesValidation="false" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlManualRevisionAttach" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeManualRevisionAttach" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlManualRevisionAttach" runat="server" TargetControlID="btnDummyManualRevisionAttach"
        PopupControlID="pnlManualRevisionAttach" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameManualRevisionAttachStateComplete() {
            $("#btnDummyManualRevisionAttach").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenManualRevisionAttachWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeManualRevisionAttach").attr("src", "wfManualLastRevAttachmentList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyManualRevisionAttach").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForManualRevisionAttach() {
            var ManualRevisionAttachwindow = $find("<%=mdlManualRevisionAttach.ClientID %>");
            //close popup window
            ManualRevisionAttachwindow.hide();
            //release resources
            $("#IframeManualRevisionAttach").attr("src", "JavaScript:''");
            //call button click
            $("#hdnBtnManualRevisionAttach").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
