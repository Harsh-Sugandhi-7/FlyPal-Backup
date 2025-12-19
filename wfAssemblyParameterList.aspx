<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAssemblyParameterList.aspx.vb"
    Inherits="Flypal.wfAssemblyParameterList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Assembly Parameter List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta name="vs_showGrid" content="True">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="tblMain" class="clstablelistout" border="0">
        <tr>
            <td class="clstablecell">
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table id="tblinner" class="clsTablelistin" border="0">
                        <tbody>
                            <tr class="clstitle1">
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">List of Parameters</asp:Label>
                                            </td>
                                            <td align="right" style="padding: 0px;">
                                                <asp:ImageButton runat="server" ID="imgHome"
													ImageUrl="~/images/Home_Button.png"
													ToolTip="Return to Machine Detail Page"
													CssClass="HomeICN" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvParameterList" runat="server" CssClass="clslabelauto"
                                        OnServerValidate="customvalidate" Display="None" ControlToValidate="cmbParameterList"
                                        ErrorMessage="Select Parameters form List."></asp:CustomValidator>
                                    <!--	<asp:customvalidator id="cvMin" runat="server" OnServerValidate="customvalidate" Display="None" ControlToValidate="txtMin"
												ClientValidationFunction="clslabelauto"></asp:customvalidator>
											<asp:customvalidator id="cvMax" runat="server" OnServerValidate="customvalidate" Display="None" ControlToValidate="txtMax"
												ClientValidationFunction="clslabelauto"></asp:customvalidator>-->
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 21px" colspan="4">
                                    <table id="Table3">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAssemblyDetails" TabIndex="0" runat="server" CssClass="clsButtonLong1"
                                                    ToolTip="Click to open Assembly Status Details page" CausesValidation="False"
                                                    Text="Aircraft Status"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnComponentList" runat="server" CssClass="clsButtonLong" ToolTip="Click to open Component List of Assembly"
                                                    Text="Component List"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnServiceList" runat="server" CssClass="clsButtonLong" ToolTip="Click to open the Service List of Assembly"
                                                    Text="Service List"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnInspectionList" TabIndex="0" runat="server" CssClass="clsButtonLong"
                                                    ToolTip="Click to open the Inspection list of Assembly" Text="Inspection List">
                                                </asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnModificationList" runat="server" CssClass="clsButtonLong" ToolTip="Click to open Directives list of Assembly"
                                                    Text="Directives"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblParameterList" runat="server" CssClass="clsLabelButton1" ToolTip="Current page of Parameter Status List ">Parameter List</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblParameterListInfo" runat="server" CssClass="clsLabelHeader">Aircraft Parameter Details</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Parameter</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbParameterList" runat="server" CssClass="clsComboBox" DataValueField="Id"
                                                    DataTextField="Name">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="imgbtnParameter" runat="server" CssClass="clsButtonGrid" ToolTip="Click to add new Parameter"
                                                    CausesValidation="False" Text="..."></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMin" runat="server" CssClass="clsLabelAuto">Min</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMin" runat="server" CssClass="clsTextBoxRightAlignSmall1" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMax" runat="server" CssClass="clsLabelAuto">Max</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMax" runat="server" CssClass="clsTextBoxRightAlignSmall1" MaxLength="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to add parameter in the List"
                                        Text="Add"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Parameter Details</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="4">
                                    <asp:DataGrid ID="dgParameterList" runat="server" CssClass="clsGrid" ToolTip="Assembly Parameter List."
                                        PageSize="3" AutoGenerateColumns="False" AllowSorting="True">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="ParameterName" SortExpression="ParameterName" HeaderText="Parameter Name">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ParameterDescription" SortExpression="ParameterDescription"
                                                HeaderText="Parameter Description ">
                                                <HeaderStyle ForeColor="White"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="MinValue" SortExpression="MinValue" HeaderText="Min.">
                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="MaxValue" SortExpression="MaxValue" HeaderText="Max.">
                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn Text="Edit" HeaderText="Edit" CommandName="Edit"></asp:ButtonColumn>
                                            <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <table id="Table1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to Print the list of Parameters"
                                                    CausesValidation="False" Text="Print" Visible="False"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton" ToolTip="Click to go Previous page"
                                                    CausesValidation="False" Text="Back"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
