<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSearchMinLevelItem_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptSearchMinLevelItem_Ajax" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Minimum/Maximum Level Item</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
     
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
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
                            <td colspan="3">
                                <span id="lbltitle" class="clstitle1">Minimum/Maximum Level Item</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Category</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="Name"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Nomenclature</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblNomenclature" class="clsLabelAuto">Nomenclature</span>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="cmbNomenclature" runat="server" CssClass="clsComboBox3_Ajax"
                                    EnableViewState="false" DataTextField="Name" DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="Span4" class="clsLabelHeader">Step III. Check To Consider Alternate Patrs Stock
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                            <td align="left" colspan="2">
                                <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                            Text="With Alternate Part"></asp:CheckBox>
                            </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Part Number/Description</span>
                                </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="lblSearch" class="clsLabelAuto">Search</span>
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtPartDescription" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                    Width="520px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3">
                                <span id="Label4" class="clsLabelHeader">Step V. Selection of Minimum/Maximum Stock
                                    Level</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left">
                                <asp:RadioButton ID="rbMinimum" runat="server" Checked="True" CssClass="clsRadioButton"
                                    GroupName="a" Text="Minimum Level" />
                            </td>
                            <td align="left">
                                &nbsp;
                                <asp:RadioButton ID="rbMaximum" runat="server" CssClass="clsRadioButton" GroupName="a"
                                    Text="Maximum Level" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left">
                                <span id="Label2" class="clsLabelHeader">(Shows inventory below or equals to min level)</span>
                            </td>
                            <td align="left">
                                &nbsp; <span id="Label3" class="clsLabelHeader">(Shows inventory above or equals to
                                    max level)</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="lblStep5" class="clsLabelHeader">Step VI. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlCriteria" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0" border="0">
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNomenclatureName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <tr>
                                <td align="right" colspan="3">
                                    <asp:UpdatePanel ID="upnlBtns" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report" Width="100px" Visible="<%$AppSettings:ShowExportToExcelButton%>"/>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                            Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            TabIndex="0" Text="Close" ToolTip="Click om  Click to Min- Level -Items screen" />
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#txtPartDescription").autocomplete('wfAutoItemList.aspx?', {
                width: $("#txtPartDescription").outerWidth(),
                autoFill: false,
                matchContains: true,
                max: 50,
                delay: 0
            });
        });
    </script>
    </form>
</body>
</html>
