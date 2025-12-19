<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfConfigureCompMPDAMPList_Ajax.aspx.vb" Inherits="Flypal.wfConfigureCompMPDAMPList_Ajax" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configure MPD on Existing Component</title>
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
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
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
                                                                    <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblModel" class="clsLabelAuto">Assembly</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="AssemblyStatusID"
                                                                        AutoPostBack="true" DataTextField="ModelSerialNoPostion">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Component</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbComponent" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="PartNoSerialNo"
                                                                        AutoPostBack="true" DataValueField="CompID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblMonitorType" runat="server" class="clsLabelAuto">Monitor Type</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCodeTaskNo" runat="server" class="clsLabelAuto">Task No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMPDNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Task No. to search"
                                                                        AutoPostBack="true" MaxLength="50" Width="185px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description to search"
                                                                        AutoPostBack="true" MaxLength="1000" TextMode="MultiLine" Width="275px"></asp:TextBox>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen" Visible="false" 
                                                                                CausesValidation="False" Text="Close"></asp:Button>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblFreq" runat="server" class="clsLabelAuto">Frequency</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Frequency. to search"
                                                                        AutoPostBack="true" Width="185px"></asp:TextBox>
                                                                </td>
                                                            </tr>
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
                                                    <td colspan="2">
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
                                                                                                <asp:Button ID="btnGroupConfigure" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to configure multiple MPD(s)" visible="false"
                                                                                                    CausesValidation="False" Text="Group Configure"></asp:Button>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:GridView ID="dgNonConfigList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AutoGenerateColumns="False"
                                                                                                    AllowSorting="true" EmptyDataText="No Records Found..." DataKeyNames="ID" ShowHeaderWhenEmpty="false"
                                                                                                    PageSize="10">
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                    <HeaderStyle CssClass="clsdgHeader nodrag nodrop" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                                                    <Columns>
                                                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                        <asp:TemplateField  HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server"></asp:CheckBox>
                                                                                                            </ItemTemplate>
                                                                                                            <HeaderTemplate>
                                                                                                                <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server" onclick="CheckUncheck(this);"></asp:CheckBox>
                                                                                                            </HeaderTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="CodeTaskNo" SortExpression="CodeTaskNo" HeaderText="Code/Form No.">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
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
                                                                                                        <asp:ButtonField Text="Config" HeaderText="Configure" CommandName="Config">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                        <%-- <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                        <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>--%>
                                                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                            DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                                                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                    <RowStyle CssClass="clsdgItem" />
                                                                                                    <HeaderStyle CssClass="clsdgHeader nodrag nodrop" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                                                    <Columns>
                                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="CompID" HeaderText="CompID" SortExpression="CompID" HeaderStyle-CssClass="hideGridColumn"
                                                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="CompStatusID" HeaderText="CompStatusID" SortExpression="CompStatusID"
                                                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference"
                                                                                                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Reference" HeaderText="Aircraft" SortExpression="Reference" HeaderStyle-CssClass="hideGridColumn"
                                                                                                            ItemStyle-CssClass="hideGridColumn">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="TaskNo" SortExpression="TaskNo" HeaderText="Task No.">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                                            <ItemStyle Wrap="false" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Reference" HeaderText="Reference" SortExpression="Reference">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Code" HeaderText="Monitor Info." SortExpression="Code">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>

                                                                                                        <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Code_Desc" HeaderText="Code/Form No./Description" SortExpression="Code_Desc"
                                                                                                            HtmlEncode="false">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="DoneOnFormatted" HeaderText="Last Done On" SortExpression="DoneOnFormatted">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                            <ItemStyle Wrap="false" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="DoneOnWONo" HeaderText="Work Order No." SortExpression="DoneOnWONo">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="DoneRemark" HeaderText="Remark" SortExpression="DoneRemark">
                                                                                                            <HeaderStyle ForeColor="Black" HorizontalAlign="Left" />
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
                                                                                                        <%-- <asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                        <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>
                                                                                                        <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>--%>
                                                                                                        <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                                                                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                        <%-- <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                                        </asp:ButtonField>--%>
                                                                                                        <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                                                                                                                    <asp:ImageButton ID="History" runat="server"
                                                                                                                                        CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                        CommandName="History" Style="height: 20px; width: 20px"
                                                                                                                                        ImageUrl="~/images/History.png"
                                                                                                                                        Visible='<%#  Eval("IsMaster")%>' />
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
                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to close screen" Visible="false" 
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
                                            <asp:Button ID="hdnBtnServiceHistory" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
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
        <!-- Comply History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
            PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameRemHistoryStateComplete() {
                $("#btnDummyRemHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeRemHistory").attr("src", "wfUpdateComplyHistoryCompMonitorServiceStatusList_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyRemHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForRemHistory() {
                var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
                //close Removal History popup window
                RemHistorywindow.hide();
                //           release resources
                $("#IframeRemHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnServiceHistory").click();
            }
        </script>
        <!-- End-->
    </form>
    <script type="text/javascript">
        function CallParentFunction() {
            window.parent.autoResizeCompMPDAMPList();
        }
        function CallCloseChildPage() {
            window.parent.CloseChildPage();
        }
    </script>
    <script type="text/javascript" language="javascript">
        function CallParentFunctionForIntTab() {
            window.parent.autoResizeListCompMPDAMP();
        }

    </script>
     <script type="text/javascript">

         //        $("#chkSelectAll").click(function () {
         function CheckUncheck(elem) {
             var status = $(elem).attr("checked");
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

         function SetRow(elem) {
             var status = $(elem).attr("checked");
             if (status == "checked") {
                 $(elem).closest("tr").addClass('HighLightRow');
             }
             else {
                 $(elem).closest("tr").removeClass('HighLightRow');
             }
         }

     </script>
</body>
</html>
