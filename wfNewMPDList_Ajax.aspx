<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNewMPDList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfNewMPDList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>MPD List</title>
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
            document.getElementById('IframeCompList').height = (newheight + 25) + "px";
            document.getElementById('IframeCompList').width = (newwidth) + "px";
            document.getElementById('tbpnlCompList').height = (newheight) + "px";
            document.getElementById('tbpnlCompList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";


        }
    </script>
    <style type="text/css">
        .maxGridWidth {
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
                                <td style="width: 100%" class="clsFormHeader1Newstyle">
                                    <span id="lblTitle" class="clsFormHeader">MPD List</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <cc2:TabContainer ID="TbContInst" runat="server" AutoPostBack="true">
                                                <cc2:TabPanel ID="tbpnlAssembly" runat="server" CssClass="clsPanel1">
                                                    <HeaderTemplate>
                                                        Assembly MPD
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
                                                                                                    <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                        DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="lblModel" class="clsLabelAuto">Model</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                        AutoPostBack="true" DataTextField="ModelName">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="spMPDNo" runat="server" class="clsLabelAuto">MPD No.</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtMPDNo" runat="server" CssClass="clsTextBoxTagSearch" 
                                                                                                        AutoPostBack="true" MaxLength="50" Width="275px"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <asp:PlaceHolder ID="phInsp" runat="server">
                                                                                                    <td>
                                                                                                        <asp:Label runat="server" ID="lblMonitorType" class="clsLabelAuto">Monitor Type</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'
                                                                                                            AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </asp:PlaceHolder>

                                                                                                <td>
                                                                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                        AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description to search"
                                                                                                        AutoPostBack="true" MaxLength="1000" TextMode="MultiLine" Width="275px"></asp:TextBox>
                                                                                                </td>


                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnllabel1" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Label ID="spnService" runat="server" class="clsLabelAuto">Service Type</asp:Label>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbMonitorServiceType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                        AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                                                    </asp:DropDownList>
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
                                                                    <asp:UpdatePanel ID="upnlInternalTabs" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
                                                                        <ContentTemplate>
                                                                            <cc2:TabContainer ID="TbInspServiceMPD" runat="server" AutoPostBack="true">
                                                                                <cc2:TabPanel ID="TabPanel1" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblTbInspServiceMPD" runat="server"> Service</asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <table id="Table21" border="0" class="clstablelistin">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlGridService" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table width="100%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:UpdatePanel ID="upnlActionBtnTopService" runat="server" UpdateMode="Conditional">
                                                                                                                            <ContentTemplate>
                                                                                                                                <table cellspacing="0" width="100%">
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            <asp:Label ID="lblResultService" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                                                                        </td>
                                                                                                                                        <td align="right">
                                                                                                                                            <table>
                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:Button ID="btnAddNewTopService" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Select Part No. and then Click on Add New button to add new record"
                                                                                                                                                                CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:Button ID="btnPrintTopService" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print List"
                                                                                                                                                                CausesValidation="False" Text="Print"></asp:Button>
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:Button ID="btnBackTopService" runat="server" Visible="false" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen"
                                                                                                                                                                CausesValidation="False" Text="Close"></asp:Button>
                                                                                                                                                        </td>
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
                                                                                                                    <td colspan="1">
                                                                                                                        <asp:GridView ID="dgModelMonitorServiceList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowSorting="True"
                                                                                                                            EmptyDataText="No Records Found..." DataKeyNames="ID" AutoGenerateColumns="False"
                                                                                                                            ToolTip="MPD List">
                                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"></HeaderStyle>
                                                                                                                            <Columns>
                                                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField Visible="False" DataField="ModelName" HeaderText="Model">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference In MPD">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
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
                                                                                                                                <%-- <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>
                                                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>
                                                                                                                                <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>--%>
                                                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <%-- <span id="button">Login</span>--%>
                                                                                                                                        <div class="dropdown">
                                                                                                                                            <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="EditView" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                                                                                                ImageUrl="~/images/edit.png" />
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CausesValidation="false" CommandName="DeleteRec"
                                                                                                                                                                Style="height: 20px; width: 20px"
                                                                                                                                                                ImageUrl="~/images/delete.png" />
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="View" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CommandName="View" Style="height: 20px; width: 13px"
                                                                                                                                                                ImageUrl="icons/CLIP01.ICO"
                                                                                                                                                                Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </div>
                                                                                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                                                                Style="cursor: pointer" />
                                                                                                                                        </div>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                                                                                    <asp:UpdatePanel ID="upnlActionBtnService" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:Button ID="btnAddNewService" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Select Part No. and then Click on Add New button to add new record"
                                                                                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:Button ID="btnPrintService" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print List"
                                                                                                                            CausesValidation="False" Text="Print"></asp:Button>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:Button ID="btnBackService" runat="server"  CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen"
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

                                                                                <cc2:TabPanel ID="tbpnlInsp" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblInspTabPanel" runat="server" Text="Inspection"> </asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <table id="Table3" border="0" class="clstablelistin">
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
                                                                                                                                            <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add new Model Inspection"
                                                                                                                                                CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print List"
                                                                                                                                                CausesValidation="False" Text="Print"></asp:Button>
                                                                                                                                        </td>
                                                                                                                                        <td>
                                                                                                                                            <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen"
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
                                                                                                                        <asp:GridView ID="dgModelMonitorInspList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowSorting="True"
                                                                                                                            EmptyDataText="No Records Found..." DataKeyNames="ID" AutoGenerateColumns="False"
                                                                                                                            ToolTip="MPD List">
                                                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"></HeaderStyle>
                                                                                                                            <Columns>
                                                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField Visible="False" DataField="ModelName" HeaderText="Model">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference In AMP">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                                                                </asp:BoundField>
                                                                                                                                <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                                                                                    <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
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
                                                                                                                                <%-- <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>
                                                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>
                                                                                                                                <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                                </asp:ButtonField>--%>
                                                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <%-- <span id="button">Login</span>--%>
                                                                                                                                        <div class="dropdown">
                                                                                                                                            <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="EditView" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                                                                                                ImageUrl="~/images/edit.png" />
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CausesValidation="false" CommandName="DeleteRec"
                                                                                                                                                                Style="height: 20px; width: 20px"
                                                                                                                                                                ImageUrl="~/images/delete.png" />
                                                                                                                                                        </td>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:ImageButton ID="View" runat="server"
                                                                                                                                                                CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                                                CommandName="ViewRec" Style="height: 20px; width: 13px"
                                                                                                                                                                ImageUrl="icons/CLIP01.ICO"
                                                                                                                                                                Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </div>
                                                                                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                                                                Style="cursor: pointer" />
                                                                                                                                        </div>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                                                                                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add new Model Inspection"
                                                                                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Print List"
                                                                                                                            CausesValidation="False" Text="Print"></asp:Button>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen"
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
                                                                            </cc2:TabContainer>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlCompList" runat="server" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Component MPD
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
        <div>
            <script type="text/javascript">
                function CallCompMPDList() {
                    document.getElementById('IframeCompList').src = 'wfNewCompMPDList_Ajax.aspx'
                }


            </script>
        </div>
        <div>
            <!-- Part Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyPart" Text="Dummy Part" ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlPart" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframePart" frameborder="0" height="100%" allowtransparency="true" width="100%"
                    src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupPart" runat="server" TargetControlID="btnDummyPart"
                PopupControlID="pnlPart" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFramePartStateComplete() {
                    $("#btnDummyPart").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenPartWindowParent() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframePart").attr("src", "wfPart_AJAX.aspx?Type=pup");
                        // $("#IframePart").load(function () {
                        //                    var doc = IframePart.window;
                        //                    IframePart.SetPageLayout();

                        if (!$.browser.msie) {
                            $("#btnDummyPart").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }


                        //});


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForPart() {
                    var Partwindow = $find("<%=mdlPopupPart.ClientID %>");
                    //close Part popup window
                    Partwindow.hide();
                    //           release resources
                    $("#IframePart").attr("src", "JavaScript:''");
                    //call Part image button
                    $("#hdnBtnPart").click();
                }
            </script>
            <!-- End-->
        </div>
    </form>
    <script language="JavaScript" type="text/javascript">
        function CloseChildPage() {
            window.location.href = "dashboard.aspx";
        }
        function CallParentFunction() {
            window.parent.autoResizeCompList();
        }
        function CallCloseChildPage() {
            window.parent.CloseChildPage();
        }

    </script>
</body>
</html>
