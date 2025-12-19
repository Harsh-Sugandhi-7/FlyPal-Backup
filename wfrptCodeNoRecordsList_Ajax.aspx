<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCodeNoRecordsList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptCodeNoRecordsList_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Tooling List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
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
                                <span class="clsFormHeader" id="lbltitle" >Tooling List</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Part Number/Description</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSearch" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                         onChange="SetPartIdonChange()"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Serial No.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtSerialNo" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                        Visible="true">

                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="lblStep3" class="clsLabelHeader">Step III. Selection of Code No.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblCodeNo" class="clsLabel">Code No.</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchLong" ID="txtCodeNo"    runat="server" Visible="true">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span id="lblStep4" class="clsLabelHeader">Step IV. Selection of Category </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblCategory" class="clsLabel">Category</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbCategory"  CssClass="clsTextBoxTagSearchComboNewstyleLong" runat="server" TabIndex="7"
                                                        DataTextField="Name" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" colspan="2">
                                                    <span id="Span1" class="clsLabelHeader">Step V. Select Sort By </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="Span2" class="clsLabel">Sort By</span>
                                                </td>
                                                <td align="left">
                                                   <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall1" ID="cmbSortBy" runat="server" >
                                                        <asp:ListItem Value="0">Code No.</asp:ListItem>
                                                        <asp:ListItem Value="1">Bin Card No.</asp:ListItem>
                                                        <asp:ListItem Value="2">Tool Type</asp:ListItem>
                                                   </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:AutoCompleteExtender ClientIDMode="Static" ID="txtSearch_Autocomplete" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                            CompletionInterval="1" ServicePath="wfrptCodeNoRecordsList_Ajax.aspx" ServiceMethod="GetPartNoDescriptionList"
                                            TargetControlID="txtSearch" OnClientItemSelected="" UseContextKey="False" ContextKey=""
                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                            OnClientShowing="ClientShowing">
                                        </asp:AutoCompleteExtender>
                                        <asp:AutoCompleteExtender ID="txtSerialNo_AutoCompleteExtender" runat="server" ClientIDMode="Static"
                                            CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" ContextKey=""
                                            DelimiterCharacters="" EnableCaching="false" Enabled="True" MinimumPrefixLength="0"
                                            OnClientHiding="ClientHiding" OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
                                            OnClientShowing="ClientShowing" OnClientShown="ClientHiding" ServiceMethod="GetSerialNo"
                                            ServicePath="wfrptCodeNoRecordsList_Ajax.aspx" TargetControlID="txtSerialNo"
                                            UseContextKey="True">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step VI. Display Report</asp:Label>
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
                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblSerialNo1" runat="server" CssClass="clsLabelAuto" Visible="False">
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCodeNo1" runat="server" CssClass="clsLabelAuto" Visible="False">
                                                    </asp:Label>
                                                </td>
                                            </tr>
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
                                                    <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server"   Text="Export to Excel"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                          ToolTip="Click to Export report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server"   TabIndex="0"
                                                        Text="Display" ToolTip="Click to display report" />
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
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    </form>
</body>
</html>
