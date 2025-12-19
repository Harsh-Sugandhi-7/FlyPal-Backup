<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSearchExpiryDate_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSearchExpiryDate_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Expiry Date</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript">

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
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3">
                                    <span id="lbltitle" class="clstitle1">Expiry Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStepI" class="clsLabelHeader">Step I. Selection of Date and Range</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBox_Ajax" Width="100px"></asp:TextBox>
                                            <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                            </cc2:CalendarExtender>
                                             <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                            </cc2:TextBoxWatermarkExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbRange" runat="server" Width="192px" CssClass="clsComboBox"
                                        Height="20px">
                                        <asp:ListItem Value="0">0 Days - 1 Month</asp:ListItem>
                                        <asp:ListItem Value="1">0 Days - 2 Month</asp:ListItem>
                                        <asp:ListItem Value="2">0 Days - 3 Month</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStepIII" class="clsLabelHeader">Step II. Selection of Store</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="left" colspan="2">
                                    <asp:Label ID="lblStoreCount" runat="server" class="clsLabelAuto" Font-Bold="true"
                                        Font-Size="XX-Small" ForeColor="DarkBlue"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStore" class="clsLabelAuto">Store</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbStoreList" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="LocationStore"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblCategory" class="clsLabelAuto">Category</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbCategoryList" runat="server" CssClass="clsComboBox3_Ajax"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Nomenclature </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblNomenclature" class="clsLabelAuto">Nomenclature</span>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="cmbNomenclatureList" runat="server" CssClass="clsComboBox3_Ajax"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep5" class="clsLabelHeader">Step V. Selection of Part Number Or Description</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSearch" class="clsLabelAuto">Search</span>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox_Ajax" Width="275px"
                                        AutoPostBack="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep6" class="clsLabelHeader">Step VI. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRangeDisp" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNomenclatureName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                            Text="Current Criteria" ToolTip=" Click to display current searching criterias">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                            Text="Export to Excel" ToolTip="Click to Export report" Width="100px" Visible="<%$AppSettings:ShowExportToExcelButton%>">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                            Text="Display" ToolTip="Click to display report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                            ToolTip="Click to Close Expiry Date screen" CausesValidation="False"></asp:Button>
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
    </div>
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
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>
