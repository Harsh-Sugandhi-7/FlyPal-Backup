<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptApprovedPartList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptApprovedPartList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Approved Part List Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
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
                                    <span id="Label20" class="clsFormHeader">Approved Part List Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label15" class="clsLabelHeader">Step I.Selection of Part Number/Description</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label14" class="clsLabelAuto">Search</span>
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label19" class="clsLabelHeader">Step II. Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label18" class="clsLabelAuto">Category</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server" DataTextField="Name"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label17" class="clsLabelHeader">Step III. Selection of Serialized Status</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label16" class="clsLabelAuto">Status</span>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkserializedtatus" runat="server" CssClass="clsCheckBox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label1" class="clsLabelHeader">Step IV. Selection of Ground Equipment</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label21" class="clsLabelAuto">Ground Equipment</span>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkGroundequipmentstatus" runat="server" CssClass="clsCheckBox" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span1" class="clsLabelHeader">Step V. Selection of Part Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span2" class="clsLabelAuto">Part Type</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAltType" runat="server"  DataValueField="ID"
                                        ClientIDMode="Static" DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Label11" class="clsLabelHeader">Step VI. Display Report</span>
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
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSeralizedstatus" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblgroundEquipment" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td>
                                                        <asp:Label ID="lblPartType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" TabIndex="0"
                                                                Text="Export to Excel" ToolTip="Click to Export report" Width="100px" Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to Close Approved Prt List Report screen" />
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>
