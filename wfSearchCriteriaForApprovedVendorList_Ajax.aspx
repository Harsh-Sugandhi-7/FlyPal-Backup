<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForApprovedVendorList_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForApprovedVendorList_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Approved Vendor List Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table id="Table1" class="clstablelistin" border="0">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="Label20" class="clsFormHeader">Approved Vendor List Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="spanStepI" class="clsLabelHeader">Step I. Selection of Vendor</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label14" class="clsLabelAuto">Vendor</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtVendorList" runat="server" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="spanStepII" class="clsLabelHeader">Step II. Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label18" class="clsLabelAuto">Category</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategoryList" runat="server"
                                        DataValueField="MachineID" DataTextField="RegNo">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                        <asp:ListItem Value="2">Supplier</asp:ListItem>
                                        <asp:ListItem Value="3">Service Provider</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="spanStepIII" class="clsLabelHeader">Step III. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label10" class="clsLabelAuto">Your selection is as follows :</span>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3" border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVendor1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCategory1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table2" border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" Text="Export to Excel"
                                                               ToolTip="Click to Export report" Visible="<%$AppSettings:ShowExportToExcelButton%>" ></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to Close Approved Vendor List Report screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
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
        $(document).ready(function () {
            $("#<%=txtVendorList.ClientID %>").autocomplete('wfAutoInventoryList.aspx?Type=Vendor', {
                width: 275,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>
