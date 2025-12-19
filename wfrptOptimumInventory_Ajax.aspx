<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptOptimumInventory_Ajax.aspx.vb"
    Inherits="Flypal.wfrptOptimumInventory_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Optimum Inventory</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1">
                                <span class="clsFormHeader" id="lbltitle" >Optimum Inventory</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="lblStep4" class="clsLabelHeader">Step I. Selection of Category </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblCategory" class="clsLabel">Category</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyleLong" ID="cmbCategory" runat="server"  DataTextField="Name"
                                                        DataValueField="ID" TabIndex="7">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="Span1" class="clsLabelHeader">Step II. Selection of Inventory Type </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="Span2" class="clsLabel">Type</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbTypeOfInventory" runat="server" >
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Optimum Inventory</asp:ListItem>
                                                        <asp:ListItem Value="2">Excess Inventory</asp:ListItem>
                                                        <asp:ListItem Value="3">Below Optimum Inventory </asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CheckBox ID="chkCategoryWiseReport" runat="server" CssClass="clsCheckBox" Text="Category Wise" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="Span3" class="clsLabelHeader">Step III. Selection of One Time Purchase Inventory
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkOTPOnly" runat="server" CssClass="clsCheckBox" Text="Show One Time Purchase Inventory Report" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSerachCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCategory1" runat="server" CssClass="clsLabelAuto" Visible="False">
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server" 
                                                        CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server"  Text="Export to Excel" 
                                                        ToolTip="Click to Export report" Visible="false"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server"  TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnByMail" runat="server"   Text="Report By Mail"
                                                        ToolTip="Click to report by mail"   />
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnClose" runat="server" CausesValidation="False"  
                                                        TabIndex="0" Text="Close" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <asp:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </asp:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>
