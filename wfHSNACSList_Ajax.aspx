<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHSNACSList_Ajax.aspx.vb"
    Inherits="Flypal.wfHSNACSList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HSN/SAC List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblHSNACSList" runat="server" CssClass="clsFormHeader">HSN/SAC List</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New HSN/SAC"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print HSN/SAC List"
                                                                        Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of HSN/SAC screen."
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
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblCode" class="clsLabel">Code</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="8"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New HSN/SAC"
                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print HSN/SAC List"
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of HSN/SAC screen."
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of HSN/SAC as per criteria : Record(s) found</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AllowSorting="True"
                                                            DataKeyNames="ID" AutoGenerateColumns="False" PageSize="25"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" CssClass="TextBreak" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="GSTPercent" HeaderText="Percent" SortExpression="GSTPercent" DataFormatString="{0:0.00}">
                                                                    <HeaderStyle HorizontalAlign="right" />
                                                                    <ItemStyle HorizontalAlign="right" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Renew" HeaderStyle-HorizontalAlign="Center"
                                                                    Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Renew" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="RenewRecord"
                                                                            Style="height: 20px;" ImageUrl="~/images/Renew3.png" Visible="false" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="History" ImageUrl="~/images/History.png" Visible='<%#  Eval("HSNACSHistoryCount") > 0 %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HSNACSHistoryCount" HeaderStyle-CssClass="hideGridColumn"
                                                                    HeaderText="HSNACSHistoryCount" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
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
                                    <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New HSN/SAC"
                                                            Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to HSN/SAC List"
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close List of HSN/SAC screen."
                                                            Text="Close" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnHSNACS" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnHSNACSRenew" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
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
    </div>
    <!-- HSNACS Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHSNACS" Text="Dummy HSNACS" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHSNACS" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHSNACS" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHSNACS" runat="server" TargetControlID="btnDummyHSNACS"
        PopupControlID="pnlHSNACS" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHSNACSStateComplete() {
            $("#btnDummyHSNACS").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenHSNACSWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHSNACS").attr("src", "wfHSNACS_Ajax.aspx?Type=pup");
                $('#IframeHSNACS').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHSNACS").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunction() {
            varHSNACSwindow = $find("<%=mdlPopupHSNACS.ClientID %>");
            //close HSNACS popup window
            varHSNACSwindow.hide();
            //           release resources
            $("#IframeHSNACS").attr("src", "JavaScript:''");
            //call HSNACS image button
            $("#hdnBtnHSNACS").click();
        }
    </script>
    <!-- End-->
    <!-- HSNACS Renew Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHSNACSRenew" Text="Dummy HSNACSRenew" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHSNACSRenew" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHSNACSRenew" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHSNACSRenew" runat="server" TargetControlID="btnDummyHSNACSRenew"
        PopupControlID="pnlHSNACSRenew" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHSNACSRenewStateComplete() {
            $("#btnDummyHSNACSRenew").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenHSNACSRenewWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHSNACSRenew").attr("src", "wfHSNACSRenew_Ajax.aspx?Type=pup");
                $('#IframeHSNACSRenew').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHSNACSRenew").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForHSNACSRenew() {
            varHSNACSRenewwindow = $find("<%=mdlPopupHSNACSRenew.ClientID %>");
            //close HSNACSRenew popup window
            varHSNACSRenewwindow.hide();
            //           release resources
            $("#IframeHSNACSRenew").attr("src", "JavaScript:''");
            //call HSNACSRenew image button
            $("#hdnBtnHSNACSRenew").click();
        }
    </script>
    <!-- HSNACS Renew Popup Window End-->
    <!-- HSNACS History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHSNACSHistory" Text="Dummy HSNACSHistory"
            ClientIDMode="Static" CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHSNACSHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHSNACSHistory" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHSNACSHistory" runat="server" TargetControlID="btnDummyHSNACSHistory"
        PopupControlID="pnlHSNACSHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHSNACSHistoryStateComplete() {
            $("#btnDummyHSNACSHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenHSNACSHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHSNACSHistory").attr("src", "wfHSNACSHistoryList_Ajax.aspx?Type=pup");
                $('#IframeHSNACSHistory').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHSNACSHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForHSNACSHistory() {
            varHSNACSHistorywindow = $find("<%=mdlPopupHSNACSHistory.ClientID %>");
            //close HSNACSHistory popup window
            varHSNACSHistorywindow.hide();
            //           release resources
            $("#IframeHSNACSHistory").attr("src", "JavaScript:''");
            //call HSNACSHistory image button
            //$("#hdnBtnHSNACSHistory").click();
        }
    </script>
    <!-- HSNACS History Popup Window End-->
    </form>
</body>
</html>
