<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptIssueReceiptStatus_Ajax.aspx.vb"
    Inherits="Flypal.wfrptIssueReceiptStatus_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Loan Transactions</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Loan Transactions</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSearchType" class="clsLabel">Search Type</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearchType" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbSearchType" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                <asp:ListItem Value="1">Store</asp:ListItem>
                                                <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                <asp:ListItem Value="3">Supplier</asp:ListItem>
                                                <asp:ListItem Value="4">Customer</asp:ListItem>
                                                <asp:ListItem Value="5">Work Shop</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="cmbGivenTaken" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                Enabled="false">
                                                <asp:ListItem Value="1">Loan Given</asp:ListItem>
                                                <asp:ListItem Value="2">Loan Taken</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of From Store</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                        Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblFromStore" class="clsLabel">From Store</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbFromStore" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="Id"
                                        DataTextField="LocationStore">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlHeader" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblStep" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlLabel" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto">Vendor</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlType" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="LocationStore"
                                                DataValueField="Id" Visible="False">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="RegNo"
                                                DataValueField="Id" Visible="False">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="cmbSupplier" runat="server" CssClass="clsComboBox3_Ajax" Visible="False"
                                                DataValueField="Id" DataTextField="Name">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsComboBox3_Ajax" AutoPostBack="True"
                                                Visible="False" DataValueField="ID" DataTextField="LocationWorkShop">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Part Number/Description</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSearch" class="clsLabel">Search</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartDescription" runat="server" CssClass="clsTextBoxRemark_Ajax"
                                        Width="520px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblFromStore1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblVendor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                            CssClass="clsButtonLong_Ajax" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                        <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax" visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        Text="Export to Excel" ToolTip="Click to Export report" Width="100px"></asp:Button>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                            Text="Display" ToolTip="Click to Display Report" ValidationGroup="a" />
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Close" ToolTip="Click to Close" />
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
            $("#<%=txtPartDescription.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>
