<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWebConfigSettings.aspx.vb"
    Inherits="Flypal.wfWebConfigSettings" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat="server">
    <title>Web Config Settings</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <asp:Label ID="lblTitle" TabIndex="1" CssClass="clstitle1" runat="server">Web Config Setting</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                                <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ErrorMessage="Enter Name"
                                    Display="None" OnServerValidate="Customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvOptions" runat="server" ControlToValidate="txtOption"
                                    ErrorMessage="CustomValidator" Display="None" OnServerValidate="Customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvValue" runat="server" ControlToValidate="txtValue" ErrorMessage="CustomValidator"
                                    Display="None" OnServerValidate="Customvalidate"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Enter Name" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvOption" runat="server" ControlToValidate="txtOption"
                                    ErrorMessage="Enter Options" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblAdd" runat="server" CssClass="clsLabelAuto">Click To Add New Record</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnNew" CssClass="clsbtnH clsinfoH" runat="server" Text="New" ToolTip="Click to Add the New Key"
                                    CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblKeyDetails" runat="server" CssClass="clsLabelHeader" Visible="False">Key Details</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto" Visible="False">Name</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox" Text="<%# mConfigurationKey.Name %>"
                                    ToolTip="Enter Key Name" Visible="False">
                                </asp:TextBox>
                            </td>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOption" runat="server" CssClass="clsLabelAuto" Visible="False">Options</asp:Label>
                            </td>
                            <td>
                                <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtOption" runat="server" CssClass="clsTextBoxMultiLine" Text="<%# mConfigurationKey.Value_Options %>"
                                                ToolTip="Enter Options" Visible="False" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblValue" runat="server" CssClass="clsLabelAuto" Visible="False">Value</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBox" Text="<%# mConfigurationKey.Value %>"
                                    ToolTip="Enter Value" Visible="False" BackColor="White">
                                </asp:TextBox>
                            </td>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSave" runat="server" CssClass="clsLabelAuto" Visible="False">Click To Save Current Record</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnAdd" CssClass="clsbtnH clsinfoH" runat="server" Text="Add" ToolTip="Click to Save key Information"
                                    Visible="False"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Key List</asp:Label>
                            </td>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgConfigurationKeys" runat="server" CssClass="clsGrid" AllowSorting="True"
                                    AutoGenerateColumns="False">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:TextBox  ID="txtKeyValue" runat="server" CssClass="clsTextBoxRightAlignSmall1New"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Value") %>'>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="Value_Options" HeaderText="Options"></asp:BoundColumn>
                                        <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="Edit"></asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                            <td colspan="2">
                                <table id="Table1" border="0" cellspacing="0" cellpadding="0" align="right" height="100%">
                                    <tr>
                                        <td valign="top" align="right">
                                            <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Key Information screen"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                ToolTip="Click to close Key Information screen" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td colspan="4" align="right">
                                <table id="Table3" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnGenerate" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                CausesValidation="False" ToolTip="Click to generate web config file " Text="Generate File">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
