<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingSalesQuotations.aspx.vb" Inherits="Flypal.wfPendingSalesQuotations" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Pending Quotation List</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;
        }
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>

    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td colspan="5" class="clsFormHeader1">
                                    <asp:Label ID="lblLedgerList" runat="server" CssClass="clsFormHeader">Pending Quotation List</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Sale Order Date </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtSalesOrderDate" CssClass="clsTextBoxTagSearchDateWOList"
                                                    CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" onchange="ValidateDateText(this,'SalesOrderDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtSalesOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtSalesOrderDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtSalesOrderDate" ID="SalesOrderDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbFromLastQuotation" runat="server" CssClass="clsRadioButton"
                                                    Text="From Last Quotation" GroupName="a"></asp:RadioButton>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdbFromAllPendingQuotation" runat="server" CssClass="clsRadioButton"
                                                    Text="From All Pending Quotation(s)" GroupName="a" Checked="True"></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="right">
                                    <table id="Table2">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH1" Text="Find Now"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:DataGrid ID="dgTransList" runat="server" CssClass="clsGridNewStyle" AllowPaging="True" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
                                        AutoGenerateColumns="False" AllowSorting="True">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DateFormatted" HeaderText="Date">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="QuotationTextNo" SortExpression="QuotationTextNo" HeaderText="Quotation No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CurrencyName" SortExpression="CurrencyName" HeaderText="Currency">
                                                <HeaderStyle></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ConversionFactor" SortExpression="ConversionFactor" HeaderText="Conversion Factor">
                                                <HeaderStyle></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CGrandTotal" SortExpression="CGrandTotal" HeaderText="Grand Total">
                                                <HeaderStyle></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:ButtonColumn Text="Select" HeaderText="Select" CommandName="Select" HeaderStyle-ForeColor="Blue" ItemStyle-ForeColor="blue"></asp:ButtonColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:DataGrid ID="dgTransItemList" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="ItemName" HeaderText="Part No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="ItemDescription" HeaderText="Part Description">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="QuotationQty" HeaderText="Balance Qty.">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CRate" HeaderText="Rate">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="QuotationDateFormatted" HeaderText="Quotation Date"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="QuotationTextNo" HeaderText="Quotation No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="RequisitionDateFormatted" HeaderText="Requisition Date"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="RequisitionTextNo" HeaderText="Requisition No.">
                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="COtherCharges" HeaderText="Other Charges">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="CAmount" HeaderText="Amount">
                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" colspan="5">
                                    <table id="tblNew">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnDone" runat="server" CssClass="clsbtnH clsinfoH1" Text="Done" ToolTip="Click to add selected Item(s)"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Back" ToolTip="Click To Go Back To Previous Page"></asp:Button>
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForQuotationList();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
    <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameQuotationListStateComplete();
                }


            });
    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
    <% Dim mopenas As String = Request.QueryString("Type") %>
        <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
        <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
</body>
</html>
