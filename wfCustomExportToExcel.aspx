<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCustomExportToExcel.aspx.vb"
    Inherits="Flypal.wfCustomExportToExcel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Customized Export to Excel</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">

        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitle" CssClass="clstitle1" runat="server">Export To Excel</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlOtherChargeDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtQuery" runat="server" CssClass="clsTextBox_Ajax" Height="150px"
                                                        TextMode="MultiLine" ToolTip="Enter Query" Width="500px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtonsTop" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table align="right">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnExportToExcelTop" runat="server" CssClass="clsButton_Ajax" Text="Export To Excel"
                                                                            Width="110px"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnExportToGridTop" runat="server" CssClass="clsButton_Ajax" Text="Export To Grid"
                                                                            Width="110px"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnEncryptDecryptTextTop" runat="server" CssClass="clsButton_Ajax"
                                                                            Text="Encrypt/Decrypt Text" Width="140px"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Screen"
                                                                            CausesValidation="False" Text="Back"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgRecordList" runat="server" CssClass="clsGrid" AutoGenerateColumns="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
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
                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="clsButton_Ajax" Text="Export To Excel"
                                                        Width="110px"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExportToGrid" runat="server" CssClass="clsButton_Ajax" Text="Export To Grid"
                                                        Width="110px"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnEncryptDecryptText" runat="server" CssClass="clsButton_Ajax" Text="Encrypt/Decrypt Text"
                                                        Width="140px"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Screen"
                                                        CausesValidation="False" Text="Back"></asp:Button>
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
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnEncryptDecryptText" ClientIDMode="Static" runat="server"
                                            Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
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
    <!-- Encrypt/Decrypt Text Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEncryptDecryptText" Text="Dummy Encrypt/Decrypt Text"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupEncryptDecryptText" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="iPopupEncryptDecryptText" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEncryptDecryptText" runat="server" TargetControlID="btnDummyEncryptDecryptText"
        PopupControlID="pnlPopupEncryptDecryptText" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEncryptDecryptTextStateComplete() {
            $("#btnDummyEncryptDecryptText").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        function OpenWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupEncryptDecryptText").attr("src", "wfEncryptDecrypt.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEncryptDecryptText").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForEncryptDecryptText() {
            var EncryptDecryptTextWindow = $find("<%=mdlPopupEncryptDecryptText.ClientID %>");
            //close Encrypt/Decrypt Text popup window
            EncryptDecryptTextWindow.hide();
            $("#iPopupEncryptDecryptText").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnEncryptDecryptText").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
