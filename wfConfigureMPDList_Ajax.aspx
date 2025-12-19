<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfConfigureMPDList_Ajax.aspx.vb"
    Inherits="Flypal.wfConfigureMPDList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Configure MPD On Existing Assembly</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
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
            document.getElementById('tbpnlComponent').height = (newheight) + "px";
            document.getElementById('tbpnlComponent').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
        function autoResizeListComp() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeCompList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeCompList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeCompList').height = (newheight + 30) + "px";
            document.getElementById('IframeCompList').width = (newwidth) + "px";
            document.getElementById('tbpnlComponent').height = (newheight) + "px";
            document.getElementById('tbpnlComponent').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
        function autoResizeMPDAMPList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeMPDAMPList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMPDAMPList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMPDAMPList').height = (newheight + 2) + "px";
            document.getElementById('IframeMPDAMPList').width = (newwidth) + "px";
            document.getElementById('tbpnlService').height = (newheight) + "px";
            document.getElementById('tbpnlService').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
        function autoResizeListMPDAMP() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeMPDAMPList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMPDAMPList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMPDAMPList').height = (newheight + 30) + "px";
            document.getElementById('IframeMPDAMPList').width = (newwidth) + "px";
            document.getElementById('tbpnlService').height = (newheight) + "px";
            document.getElementById('tbpnlService').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
        function autoResizeCompMPDAMPList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeCompMPDAMPList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeCompMPDAMPList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeCompMPDAMPList').height = (newheight + 2) + "px";
            document.getElementById('IframeCompMPDAMPList').width = (newwidth) + "px";
            document.getElementById('tbpnlCompService').height = (newheight) + "px";
            document.getElementById('tbpnlCompService').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
        function autoResizeListCompMPDAMP() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeCompMPDAMPList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeCompMPDAMPList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeCompMPDAMPList').height = (newheight + 30) + "px";
            document.getElementById('IframeCompMPDAMPList').width = (newwidth) + "px";
            document.getElementById('tbpnlCompService').height = (newheight) + "px";
            document.getElementById('tbpnlCompService').width = (newwidth) + "px";

            document.getElementById('TbConfigAssemblyMPD').height = (newheight) + "px";
            document.getElementById('TbConfigAssemblyMPD').width = (newwidth) + "px";


        }
    </script>
</head>
<body leftmargin="0" rightmargin="0">
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td class="clsFormHeader1Newstyle">
                    <table width="100%">
                        <tr>
                            <td>
                                <span id="lblTitle" class="clsFormHeader">Configured MPD List</span>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close screen"
                                            CausesValidation="False" Text="Close"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc2:TabContainer ID="TbConfigAssemblyMPD" runat="server" AutoPostBack="true">
                                    <cc2:TabPanel ID="tbpnlAssembly" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            Assembly MPD
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table id="tblInner" class="clstablelistin">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table id="Table1" width="100%">
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
                                                                                        <span id="lblModel" class="clsLabelAuto">Assembly</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="AssemblyStatusID"
                                                                                            AutoPostBack="true" DataTextField="ModelSerialNoPostion">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <span id="Span1" class="clsLabelAuto">MPD No.</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtMPDNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter MPD No. to search"
                                                                                            AutoPostBack="true" MaxLength="50" Width="275px"></asp:TextBox>
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
                                                                                <tr>
                                                                                    <td>
                                                                                        <span id="lblFreq" runat="server" class="clsLabelAuto">Frequency</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Frequency. to search"
                                                                                            AutoPostBack="true" Width="275px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="padding-left: 4px" colspan="7">
                                                                                        <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                                            Text="* Selected Assembly is marked as ReadOnly" Visible="false" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <cc2:TabContainer ID="TbConfigNonConfig" runat="server" AutoPostBack="true">
                                                                                        <cc2:TabPanel ID="tbpnlNonConfig" runat="server">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblNonConfigTabPanel" runat="server" Text="Label">Tab 1 </asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ContentTemplate>
                                                                                                <asp:UpdatePanel ID="upnlNonConfig" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <table id="Table2" border="0" class="clstablelistin">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblNonConfigResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                                                </td>
                                                                                                                <td align="right">
                                                                                                                    <asp:Button ID="btnGroupConfigure" runat="server" CssClass="clsButtonLong_Ajax" ToolTip="Click to configure multiple MPD(s)"
                                                                                                                        CausesValidation="False" Text="Group Configure"></asp:Button>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="2">
                                                                                                                    <asp:GridView ID="dgNonConfigList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False" ClientIDMode="Static"
                                                                                                                        AllowSorting="true" EmptyDataText="No Records Found..." DataKeyNames="ID" ShowHeaderWhenEmpty="false"
                                                                                                                        PageSize="10">
                                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                                        <Columns>
                                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                                            <asp:TemplateField>
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server"></asp:CheckBox>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderTemplate>
                                                                                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server" onclick="CheckUncheck()"></asp:CheckBox>
                                                                                                                                </HeaderTemplate>
                                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField Visible="False" DataField="ModelName" HeaderText="Model">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference In MPD">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
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
                                                                                                                            <asp:ButtonField Text="Config" HeaderText="Configure" CommandName="Config">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:ButtonField>
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
                                                                                                                        </Columns>
                                                                                                                    </asp:GridView>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </ContentTemplate>
                                                                                        </cc2:TabPanel>
                                                                                        <cc2:TabPanel ID="tbpnlConfig" runat="server">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblConfigTabPanel" runat="server" Text="Label">Tab 1 </asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ContentTemplate>
                                                                                                <asp:UpdatePanel ID="upnlConfig" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <table id="Table23" border="0" class="clstablelistin">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblConfigResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:GridView ID="dgConfigList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                                                        EmptyDataText="No Records Found..." PageSize="5" ShowHeaderWhenEmpty="false"
                                                                                                                        CssClass="clsGrid">
                                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                                                                        <Columns>
                                                                                                                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="AssemblyID" HeaderText="AssemblyID" SortExpression="AssemblyID"
                                                                                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="AssemblyStatusID" HeaderText="AssemblyStatusID" SortExpression="AssemblyStatusID"
                                                                                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="HourType" HeaderText="HourType" SortExpression="HourType"
                                                                                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="ModelMonitorInspCode" HeaderText="Monitor Info." SortExpression="ModelMonitorInspCode">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="ATACode" HeaderText="ATACode" SortExpression="ATA">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                                                                HtmlEncode="false">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="DoneOnFormatted" HeaderText="Last Done On" SortExpression="DoneOnFormatted">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="DoneWONo" HeaderText="Work Order No." SortExpression="DoneWONo">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:BoundField DataField="DoneRemark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                                                                            </asp:BoundField>
                                                                                                                            <asp:TemplateField HeaderText="Frequency" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblFreqValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkFreqValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Effective From/DoneOn Value" ItemStyle-HorizontalAlign="Center"
                                                                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDoneOnValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDoneOnValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Current" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkCurrentValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Elapsed" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblElapsedValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkElapsedValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Extension" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblExtensionValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkExtensionValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Due At." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDueAtValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDueAtValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Due At Airframe" ItemStyle-HorizontalAlign="Center"
                                                                                                                                HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDueAtAirframeValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkDueAtAirframeValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Remaining" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblRemainingValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemainingValue" CommandName="ShowVal"
                                                                                                                                        runat="server" Text="View Values" ToolTip="Click to view Values"></asp:LinkButton>
                                                                                                                                </ItemTemplate>
                                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:TemplateField HeaderText="Is Applicable">
                                                                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                                                                                        Enabled="False"></asp:CheckBox>
                                                                                                                                </ItemTemplate>
                                                                                                                            </asp:TemplateField>
                                                                                                                            <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:ButtonField>
                                                                                                                            <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:ButtonField>
                                                                                                                            <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:ButtonField>
                                                                                                                            <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                                            <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                            </asp:ButtonField>
                                                                                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                                        </Columns>
                                                                                                                    </asp:GridView>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </ContentTemplate>
                                                                                        </cc2:TabPanel>
                                                                                    </cc2:TabContainer>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
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
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                                    CausesValidation="False" Text="Close"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnBtnModelInspMaster" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                <asp:Button ID="hdnBtnInspectionHistory" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
                                            </table>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tbpnlComponent" runat="server" ClientIDMode="Static">
                                        <HeaderTemplate>
                                            Component MPD
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <iframe id="IframeCompList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                frameborder="0" onload="autoResizeCompList()"></iframe>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabpnlService" runat="server" ClientIDMode="Static">
                                        <HeaderTemplate>
                                            <asp:Label ID="lbltabService" runat="server">Assembly Service</asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <iframe id="IframeMPDAMPList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                frameborder="0" onload="autoResizeMPDAMPList()"></iframe>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabpnlCompService" runat="server" ClientIDMode="Static">
                                        <HeaderTemplate>
                                            <asp:Label ID="lbltabCompService" runat="server">Component Service</asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <iframe id="IframeCompMPDAMPList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                frameborder="0" onload="autoResizeCompMPDAMPList()"></iframe>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                </cc2:TabContainer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </asp:Panel>
            </tr>
            <tr>
                <td></td>
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
                function CallCompMPDConfigureList() {
                    document.getElementById('IframeCompList').src = 'wfConfigureCompMPDList_Ajax.aspx'
                }
                function CallMPDAMPConfigureList() {
                    document.getElementById('IframeMPDAMPList').src = 'wfConfigureMPDAMPList_Ajax.aspx'
                }
                function CallCompMPDAMPConfigureList() {
                    document.getElementById('IframeCompMPDAMPList').src = 'wfConfigureCompMPDAMPList_Ajax.aspx'
                }
            </script>
        </div>
        <!--Inspection History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspectionHistory" Text="Inspection History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlInspectionHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspectionHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspectionHistory" runat="server" TargetControlID="btnDummyInspectionHistory"
            PopupControlID="pnlInspectionHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspectionHistoryStateComplete() {
                $("#btnDummyInspectionHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspectionHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeInspectionHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorInspStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyInspectionHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspectionHistory() {
                var InspectionHistorywindow = $find("<%=mdlPopupInspectionHistory.ClientID %>");
                //close Inspection History popup window
                InspectionHistorywindow.hide();
                //           release resources
                $("#IframeInspectionHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnInspectionHistory").click();
            }
        </script>
        <!-- End-->
    </form>
    <script type="text/javascript">
        function CloseChildPage() {
            window.location.href = "index.aspx";
        }
    </script>
    <script type="text/javascript">

        //        $("#chkSelectAll").click(function () {
        function CheckUncheck() {
            var status = $("#chkSelectAll").attr("checked");
            $("#dgNonConfigList tr:gt(0)").find(":checkbox").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                    SetRow($(this));
                }
                else {
                    $(this).removeAttr("checked");
                    SetRow($(this));
                }

            });
        }

        //        });

        //        $("#chkSelectAll").change(function () {
        //            var checked = $(this).prop('checked');
        //            $('.cbSelectRow').prop('checked', checked).trigger('change');
        //        });


        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }

        //        function pageLoad() {
        //            var status;
        //            $("#dgNonConfigList tr:gt(0)").find(":checkbox").each(function () {
        //                status = $(this).attr("checked");
        //                if (status == "checked") {
        //                    SetRow($(this));
        //                }
        //                else {
        //                    //$(this).removeAttr("checked");
        //                    SetRow($(this));
        //                }

        //            });

        //        }
    </script>
</body>
</html>
