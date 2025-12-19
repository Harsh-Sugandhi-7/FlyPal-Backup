<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptSearchReOderLevelItem_Ajax.aspx.vb"
    Inherits="Flypal.wfrptSearchReOderLevelItem_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Re-Order Level Item</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script type="text/javascript">
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
                                    <td class="clsFormHeader1">
                                        <span id="lbltitle" class="clsFormHeader">Re-Order Level Item</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <asp:PlaceHolder runat="server" ID="phStore" Visible='<%# IIf(AppSettings("ClientCode") = "Taj", True, False)%>'>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span id="lblStep" class="clsLabelHeader">Selection of Store</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblStore" class="clsLabel">Store</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataValueField="Name"
                                                                    DataTextField="LocationStore">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span id="lblStep1" class="clsLabelHeader">Selection of Category</span>
                                                            </td>
                                                        </tr>
                                                    </asp:PlaceHolder>
                                                    <tr>
                                                        <td>
                                                            <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                   
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Span2" class="clsLabelHeader">Selection of Model </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span3" class="clsLabelAuto">Model</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlModelList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtModelList" runat="server" CssClass="clsTextBox_Ajax" Width="275px"
                                                                        AutoPostBack="True" onfocus="GetAssemblyTypeID()"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                                        CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                                    </cc2:AutoCompleteExtender>
                                                                    <script type="text/javascript">
                                                                        function GetAssemblyTypeID() {
                                                                            var autoComplete = $find('txtModelList_AutoCompleteExtender');

                                                                            var str = $("#cmbAssemblyType :selected").val();
                                                                            autoComplete.set_contextKey(str);
                                                                        }
                                                                    </script>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Span4" class="clsLabelHeader">Check To Consider Alternate Parts Stock
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="87px"></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCheckForAlternatePart" runat="server" CssClass="clsLabelAuto"
                                                                Text="With Alternate Part"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="lblStep5" class="clsLabelHeader">Selection of Part Number/Description</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="False" CssClass="clsTextBox_Ajax"
                                                                Width="275px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <asp:PlaceHolder runat="server" ID="phFormat" Visible='<%# IIf(AppSettings("ClientCode") = "Taj", False, True)%>'>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Span1" class="clsLabelHeader">Selection of Format</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblFormat" class="clsLabelAuto">Format</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbFormat" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                <asp:ListItem Value="0">Format 1 </asp:ListItem>
                                                                <asp:ListItem Value="1">Format 2 </asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Span5" class="clsLabelHeader">Selection of Sort By Parameter</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span6" class="clsLabelAuto">Sort By</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbSortBy" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                                <asp:ListItem Value="1" Selected="True">Part No.</asp:ListItem>
                                                                <asp:ListItem Value="2">Group Alternate Parts Together</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                        </asp:PlaceHolder> 
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="lblStep6" class="clsLabelHeader">Display Report</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSelection" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                           
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
                                                        <td colspan="2">
                                                            <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                                TabIndex="0" Text="Current Criteria" ToolTip=" Click to display current searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                                TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report"
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                                TabIndex="0" Text="Close" ToolTip="Click on  Click to Re-Order Level Item screen" />
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
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
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
