<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNewADSBList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfNewADSBList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>AD/SB List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script language="JavaScript" type="text/javascript">

        function autoResizeCompList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeCompList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeCompList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeCompList').height = (newheight + 2) + "px";
            document.getElementById('IframeCompList').width = (newwidth) + "px";
            document.getElementById('tbpnlCompList').height = (newheight) + "px";
            document.getElementById('tbpnlCompList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";


        }
    </script>
    <style type="text/css">
        .maxGridWidth
        {
            max-width: 1000px;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td style="width: 100%">
                                <span id="lblTitle" class="clstitle1">AD/SB List</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <cc2:TabContainer ID="TbContInst" runat="server" AutoPostBack="true">
                                            <cc2:TabPanel ID="tbpnlAssembly" runat="server" CssClass="clsPanel1">
                                                <HeaderTemplate>
                                                    Assembly AD/SB
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <table class="clstablelistin" id="Table2" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table id="Table1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <span id="lblAssemblyType" class="clsLabelAuto">Assembly Type</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                    DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span id="lblModel" class="clsLabelAuto">Model</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                                    AutoPostBack="true" DataTextField="ModelName">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span id="Span1" class="clsLabelAuto">Directive No.</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtModNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Directive No. to search"
                                                                                                    AutoPostBack="true" MaxLength="150" Width="275px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <span id="lblMonitorType" class="clsLabelAuto">Monitor Type</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                                                    AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                                                    AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Description to search"
                                                                                                    AutoPostBack="true" MaxLength="1000" TextMode="MultiLine" Width="275px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <table cellspacing="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Model Directive"
                                                                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List"
                                                                                                            Visible="false" CausesValidation="False" Text="Print"></asp:Button>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                                                                            CausesValidation="False" Text="Close"></asp:Button>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:GridView ID="dgModelMonitorDirectiveList" runat="server" CssClass="clsGrid"
                                                                                        AllowSorting="True" EmptyDataText="No Records Found..." DataKeyNames="ID" AutoGenerateColumns="False"
                                                                                        ToolTip="AD/SB List">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="CodeNumber" SortExpression="CodeNumber" HeaderText="Code/Form No.">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Directive No.">
                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours">
                                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Note" HeaderText="Rev. Status">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:BoundField>
                                                                                            <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                            </asp:ButtonField>
                                                                                            <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                            </asp:ButtonField>
                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                DataField="ModelMonitorModType" HeaderText="ModelMonitorModType"></asp:BoundField>
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
                                                            <td align="right">
                                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Model Directive"
                                                                                        CausesValidation="False" Text="Add New"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List"
                                                                                        Visible="false" CausesValidation="False" Text="Print"></asp:Button>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                                                        CausesValidation="False" Text="Close"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                            <cc2:TabPanel ID="tbpnlCompList" runat="server" ClientIDMode="Static">
                                                <HeaderTemplate>
                                                    Component AD/SB
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <iframe id="IframeCompList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                        frameborder="0" onload="autoResizeCompList()"></iframe>
                                                </ContentTemplate>
                                            </cc2:TabPanel>
                                        </cc2:TabContainer>
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
    <div>
        <script type="text/javascript">
            function CallCompADSBList() {
                document.getElementById('IframeCompList').src = 'wfNewCompADSBList_Ajax.aspx'
            }
            
           
        </script>
    </div>
    </form>
    <script language="JavaScript" type="text/javascript">
        function CloseChildPage() {
            window.location.href = "index.aspx";
        }
    </script>
</body>
</html>
