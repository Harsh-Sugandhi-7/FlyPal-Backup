<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionItemSearch.aspx.vb"
    Inherits="Flypal.wfRequisitionItemSearch" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<html>
<head runat ="server" >
    <title>Requisition Search Item Engineer</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto'); 

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK    id="MainStyle" type="text/css" rel="stylesheet"><!-- #include file= "LocalFunction.htm" -->
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblLedgerList" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Requisition Part No. Selection</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblHeading" runat="server" CssClass="clsLabelHeader">Search for Part No. as per the Model of Airframe or Assembly as per the Job Description</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="Table1" class="clsTable1" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxAuto" MaxLength="50"
                                                ToolTip="Enter Part No."></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" ToolTip="Click to find Part No"
                                                Text="Find Now" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts :  Record(s) found.</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:DataGrid ID="dgPartList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                    PageSize="15" AllowPaging="True" AllowSorting="True">
                                    <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                    <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="ItemId" HeaderText="ItemId"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="PartNo" SortExpression="PartNo" ReadOnly="True" HeaderText="Part No.">
                                            <HeaderStyle Wrap="False" ForeColor="White"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Description" SortExpression="Description" ReadOnly="True"
                                            HeaderText="Description">
                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="AvailableQtyForItemGrid" SortExpression="AvailableQtyForItemGrid"
                                            HeaderText="Available Qty.">
                                            <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Enter Part no. and its description for new Part</asp:Label>
                                </td>
                            </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblPartCreate" runat="server" CssClass="clsLabelAuto">Part No</asp:Label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:TextBox ID="txtPartCreate" runat="server" CssClass="clsTextBox1" MaxLength="50"
                                    ToolTip="Enter Part No"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" CssClass="clsLabelAuto"
                                    ControlToValidate="txtPartCreate" Display="None" ErrorMessage="Part No Required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                            </td>
                            <td colspan="2" align="left">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox1" MaxLength="100"
                                    ToolTip="Enter Part Description"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" CssClass="clsLabelAuto"
                                    ControlToValidate="txtDescription" Display="None" ErrorMessage="Description Required"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <table class="clstableButton" border="0" align="right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnCreate" runat="server" CssClass="clsButton" ToolTip=" Click to add new part"
                                                Text="Create"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnOk" runat="server" CssClass="clsButton" ToolTip="Click to Add Parts"
                                                Text="Ok" CausesValidation="False"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" ToolTip="Click to close Requisition Part No. selection screen"
                                                Text="Close" CausesValidation="False"></asp:Button>
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
