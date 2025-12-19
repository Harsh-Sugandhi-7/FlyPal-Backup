<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfATAAlertCalculation_Ajax.aspx.vb"
    Inherits="Flypal.wfATAAlertCalculation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>ATA</title>
    <script language="javascript" type="text/jscript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <style type="text/css">
        .btn
        {
            padding: 1px;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">ATA Alert Level</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%" align="top">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="ModelName"
                                                DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
                                        </td>
                                        <td>
                                            <asp:ListBox ID="cmbATAList" runat="server" ClientIDMode="Static" DataTextField="ATAChapter"
                                                DataValueField="ID" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:UpdatePanel ID="upnlReCalculate" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnReCalculate" CssClass="clsButton_Ajax" Width="100px" Font-Size="9pt"
                                                        ClientIDMode="Static" OnClientClick="ShowProgress();" Style="font-weight: 600;
                                                        font-style: italic" runat="server" ToolTip="Click to Re-Calculate Alert Level"
                                                        Text="Re-Calculate" CausesValidation="True"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblSearchByChapter" class="clsLabelHeader">Search by Chapter</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblSearch" class="clsLabel">Chapter </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Chapter"
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="Span1" class="clsLabel">Model </span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbModelSearchList" runat="server" CssClass="clsComboBox3_Ajax"
                                                DataTextField="ModelName" DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" colspan="1">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to Find the list of ATA as per searching Criteria"
                                            Text="Find Now" CausesValidation="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgATAList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" AllowPaging="True" PageSize="10" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="Code" SortExpression="ATACode">
                                                                <HeaderStyle ForeColor="White" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATANomenclature" HeaderText="Chapter" SortExpression="ATANomenclature">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReliabilityAlertLevelPireps" HeaderText="Alert Level Pireps"
                                                                SortExpression="ReliabilityAlertLevelPireps">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReliabilityAlertLevelMaintDefect" HeaderText="Alert Level Maintenance Defect"
                                                                SortExpression="ReliabilityAlertLevelMaintDefect">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReliabilityAlertLevelDate" HeaderText="Alert Level Date"
                                                                SortExpression="ReliabilityAlertLevelDate">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Re-Calculate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Wrap="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ReCalculate" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="ReCalculate" Style="height: 15px; width: 15px" ImageUrl="~/images/revert.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table height="100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Close" ToolTip="Click to close ATA Chapter screen" />
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
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
    <asp:UpdateProgress ID="AjaxLoader" DynamicLayout="false" ClientIDMode="Static" runat="server">
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
    <asp:UpdateProgress ID="LoaderForImportExcel" runat="server" AssociatedUpdatePanelID="upnlReCalculate">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    </form>
    <script src="bootstrap/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrap/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {


            $('[id*=cmbATAList]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 250,
                nonSelectedText: '(ALL)',
                selectAllJustVisible: false
            });
        });
    </script>
    <script type="text/javascript">
        function ShowProgress() {
            document.getElementById('<% Response.Write(LoaderForImportExcel.ClientID) %>').style.display = "inline";
        }
    </script>
</body>
</html>
