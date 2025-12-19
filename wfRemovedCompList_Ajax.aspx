<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemovedCompList_Ajax.aspx.vb"
    Inherits="Flypal.wfRemovedCompList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Removed Component List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .aspNetDisabled
        {
            color: Black !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">Component Installation</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtInstallationDate"
                                    ErrorMessage="Install Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtInstallationDate" ErrorMessage="Install Date Required"
                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Component Search Information</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblInstallationDate" class="clsLabelAuto">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtInstallationDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'InstallationDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtInstallationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstallationDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtInstallationDate" ID="InstallationDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                  <asp:PlaceHolder ID="placeHolder1" runat="server">
                                                    <td>
                                                       <%-- <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>--%>
                                                         <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataTextField="RegNo"
                                                            AutoPostBack="true" DataValueField="ID" Width="100px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    </asp:PlaceHolder>
                                                    <td>
                                                        <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox_Ajax" DataTextField="ModelSerialNoPositionBA"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkSpareComponent" runat="server" Text="Install Spare Component" Visible="false"></asp:LinkButton>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                    Visible="true" Text="Find Now" ToolTip="Click to find list of Component's as per searching criteria"
                                                                    ValidationGroup="a" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblPart" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="50"
                                                            ToolTip="Enter Part No." AutoCompleteType="DisplayName"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="50"
                                                            ToolTip="Enter Serial Number"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 4px" colspan="8">
                                                        <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                            Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="8">
                                                        <span class="clsLabelHeader">Note : After change in search criteria,Click on Find Now
                                                            button to get respective component's.</span>
                                                    </td>
                                                </tr>
                                        </fieldset>
                                        </table> </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnRemovedTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"   >
                                                            <td>
                                                                <span id="lblInstalledOn" class="clsLabelAuto">Installed On</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbInstalledOnAssembly" runat="server" CssClass="clsComboBox_Ajax"
                                                                    DataTextField="RegNo" DataValueField="ID" AutoPostBack="true" Width="100px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            </asp:PlaceHolder>
                                                            <td>
                                                                <span id="Label1" class="clsLabelAuto">On Assembly</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbInstalledOnAssemblyList" runat="server" CssClass="clsComboBox_Ajax"
                                                                    DataTextField="ModelSerialNoPositionBA" DataValueField="ID">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="lblReadOnlyInstalledOn" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                    Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                    Text="Install" ToolTip="Click to add new Component" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrintRemovedTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                    Visible="false" Text="Print" ToolTip="Click to print List of Removed Component" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                    Text="Close" ToolTip="Click to close List of Removed Components screen" />
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
                                <asp:UpdatePanel ID="upnlRemovalGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRemovedComponents" runat="server" CssClass="clsLabelHeader">List of Removed Components as of [Date] : Record(s)</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkRemCompLoadMoreTop" runat="server" CssClass="clsLinkButton"
                                                                    Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgRemovedList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        DataKeyNames="ID" ShowHeaderWhenEmpty="true" CssClass="clsGrid" PageSize="5"
                                                        OnRowDataBound="dgRemovedList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID " Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                                <ItemStyle Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode" HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompInfo" HeaderText="Component Info." SortExpression="CompInfo"
                                                                HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Width="130px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemTSOValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemTSOValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:ButtonField CommandName="InstallSelected" HeaderText="Install Selected" Text="Install Selected">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkRemCompLoadMore" runat="server" CssClass="clsLinkButton" ForeColor="Red"
                                                        Visible="<%$AppSettings:IsShowAllRecordsVisible%>" Text="Show All Records"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnRemoved" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Text="Install" ToolTip="Click to add new Component" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintRemoved" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Visible="false" Text="Print" ToolTip="Click to print List of Removed Component" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlInstallationGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblInstalledComponents" runat="server" CssClass="clsLabelHeader">List of Installed Components as of [Date] : Record(s)</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkInstCompLoadMoreTop" runat="server" CssClass="clsLinkButton"
                                                                    Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgInstalledList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true" CssClass="clsGrid" PageSize="5" OnRowDataBound="dgInstalledList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID " Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                                <ItemStyle Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode" HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompInfo" HeaderText="Component Info." SortExpression="CompInfo"
                                                                HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Width="130px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInstValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkInstValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSNValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkTSNValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSOValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkTSOValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Revert Installation" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="RevertRemoval" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        Style="height: 19px; width: 20px" ToolTip="Click for Revert Installation of Component"
                                                                        CommandName="RevertInst" ImageUrl="~/images/Revert.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="History" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="History" ImageUrl="~/images/History.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstCompLoadMore" runat="server" CssClass="clsLinkButton"
                                                        Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
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
                                        <table id="Table2" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintInstalled" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                        Visible="false" Text="Print" ToolTip="Click to print List of Installed Component" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" TabIndex="0" Text="Close"
                                                        ToolTip="Click to close List of Removed Components screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup for city-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnInstallationHistory" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnSpareCompInstallList" ClientIDMode="Static" runat="server"
                                            Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
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
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
    <!-- Installation History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInstallationHistory" Text="Installation History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlInstallationHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInstallationHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInstallationHistory" runat="server" TargetControlID="btnDummyInstallationHistory"
        PopupControlID="pnlInstallationHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInstallationHistoryStateComplete() {
            $("#btnDummyInstallationHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInstallationHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeInstallationHistory").attr("src", "wfUpdateInstalledCompHistory_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyInstallationHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInstallationHistory() {
            var InstallationHistorywindow = $find("<%=mdlPopupInstallationHistory.ClientID %>");
            //close Installation History popup window
            InstallationHistorywindow.hide();
            //           release resources
            $("#IframeInstallationHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnInstallationHistory").click();
        }
    </script>
    <!-- End-->
    <!--Spare Comp Install List Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySpareCompInstallList" Text="Comp Inspection List New"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSpareCompInstallList" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSpareCompInstallList" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSpareCompInstallList" runat="server" TargetControlID="btnDummySpareCompInstallList"
        PopupControlID="pnlSpareCompInstallList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSpareCompInstallListStateComplete() {
            $("#btnDummySpareCompInstallList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSpareCompInstallListWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSpareCompInstallList").attr("src", "wfSpareCompListForInstallation_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySpareCompInstallList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSpareCompInstallList() {
            var SpareCompInstallListwindow = $find("<%=mdlPopupSpareCompInstallList.ClientID %>");
            //close Comp Inspection List New popup window
            SpareCompInstallListwindow.hide();
            //           release resources
            $("#IframeSpareCompInstallList").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSpareCompInstallList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
