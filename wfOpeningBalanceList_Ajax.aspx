<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOpeningBalanceList_Ajax.aspx.vb"
    Inherits="Flypal.wfOpeningBalanceList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Opening Balance List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table class="clstablelistin" id="tblInner">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="lblTitle" class="clsFormHeader">Opening Balance Information For Part No. [<%=mItem.Name%>]</span>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlActionbtns" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="true"
                                                                            ValidationGroup="1" Text="Add New" ToolTip="Click to Add New Opening Balance"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                                            CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ValidationGroup="1" ID="Validationsummary" runat="server"
                                                    CssClass="clsValidationSummary" HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                                <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" Display="None" ErrorMessage="Select As On Date."
                                                    ControlToValidate="txtAsOnDate" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ValidationGroup="1" ID="cvAsOnDate" OnServerValidate="customvalidate"
                                                    runat="server" ControlToValidate="txtAsOnDate" Display="None" ErrorMessage="Select As On Date."
                                                    ClientValidationFunction="ValidateAsOnDate"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlTabs" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPartInformation" runat="server" CssClass="clsButtonLong1" EnableViewState="False"
                                                            Text="Part Information" ToolTip="Click to open the Part Information"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAlternatePart" runat="server" CssClass="clsButtonLong1" EnableViewState="False"
                                                            Text="Alternate Part" ToolTip="Click to open the Alternate Part List"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnApplicability" runat="server" CssClass="clsButtonLong1" EnableViewState="False"
                                                            Text="Applicability" ToolTip="Click to open the Applicability List"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOpeningStock" runat="server" CssClass="clsLabelButton1" ToolTip="Current page of Aircraft Status Detail">Opening Stock</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>--%>
                                <%-- <tr>
                                    <td colspan="2">
                                        <span id="lblNote" class="clsLabelHeader">Click On Add New Button To Fill-Up Opening
                                        Balance Information.</span>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlAsOnDate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table class="clstablelistin" id="Table1" cellspacing="0" cellpadding="0" width="300"
                                                    border="0">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblStarAsOnDate" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblAsOnDate" class="clsLabel">As On Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAsOnDate" ClientIDMode="Static" runat="server" AutoPostBack="true" Width="100px" AutoComplete="off"
                                                                            Text="<%# mItem.AsOnDateFormatted %>" CausesValidation="true" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="Calender_watermarkextender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
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
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="width: 100%; margin-bottom: 3px;">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Opening Balances : Record(s).</asp:Label>
                                                </div>
                                                <div style="width: 100%;">
                                                    <asp:GridView ID="gdvOpeningBalanceList" runat="server"
                                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                        DataKeyNames="ReceiptID" EnableViewState="True" ForeColor="Black" GridLines="Horizontal" PageSize="1000" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="InvoiceDateFormatted" HeaderText="Receipt Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FullInvoiceNo" SortExpression="FullInvoiceNo" HeaderText="Receipt No.">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromTypeName" SortExpression="FromTypeName" HeaderText="Source">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromName" SortExpression="FromName" HeaderText="Name">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="Release Note Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DisplayQty" SortExpression="Qty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="To Store">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StartDateFormatted" HeaderText="Cure Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CureQtrYear" SortExpression="CureQtrYear" HeaderText="Cure Qtrs.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpQtrYear" SortExpression="ExpQtrYear" HeaderText="Expiry Qtrs.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="BatchNo" SortExpression="BatchNo" HeaderText="Batch No.">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemTypeName" SortExpression="ItemTypeName" HeaderText="Part Type">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CAmount" SortExpression="CAmount" HeaderText="Amount">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="black"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%-- <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRecord"></asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRecord"></asp:ButtonField>--%>
                                                           
                                                           <%-- 19--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRecord" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRecord" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" runat="server" ToolTip="Print Acceptance Tag" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="PrintAcceptanceTag" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%# not Eval("IsNew") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                    </div>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        <%--    <asp:TemplateField HeaderText="Print Acceptance Tag">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPrintAcceptanceTag" runat="server" Text="Print Acceptance Tag"
                                                                        CommandName="PrintAcceptanceTag"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right"></PagerStyle>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            function ValidateAsOnDate(source, args) {
                args.IsValid = true;
                var tempval = $.trim($("#txtAsOnDate").val());
                if (!tempval) {
                    args.IsValid = false;
                    return;

                }
            }
        </script>
    </form>
</body>
</html>
