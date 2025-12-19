<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOpenAuthorizedTransactions.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfOpenAuthorizedTransactions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Re-Open Authorized Transaction</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.js"></script>
</head>
<body>
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
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table>
                                        <tr>
                                            <td style="width: 99%" valign="middle">
                                                <span id="lblList" class="clsFormHeader">Re-Open Authorized Transaction</span>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnActionButton" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close List."
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvTransType" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="cmbTransactionType" ErrorMessage="Please select Transaction Type"
                                        ValidationGroup="1" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvTrasText" runat="server" CssClass="clslabelauto"
                                        ErrorMessage="Transaction Text Required." ControlToValidate="txtTransactionText"
                                        Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                        ErrorMessage="Transaction No Required." validateEmptyText="true" ControlToValidate="txtTransactionNo"
                                        Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                    <script type="text/javascript">
                                        function validateSelection(source, args) {
                                            args.IsValid = false;
                                            var status;
                                            status = $get("cmbTransactionType").selectedIndex;
                                            if (status > 0) {
                                                args.IsValid = true;
                                                return;
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelStar" id="lblStarTranType">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabelAuto">Transaction</span>
                                                    </td>
                                                    <td colspan="6">
                                                        <asp:DropDownList ID="cmbTransactionType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelStar" id="Span1">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span2" class="clsLabelAuto">Text</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransactionText" runat="server" CssClass="clsTextBoxTagSearch"
                                                            ClientIDMode="Static" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabelAuto">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTransactionNo" runat="server" CssClass="clsTextBoxTagSearch" Width="55px"
                                                            ClientIDMode="Static" MaxLength="6"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblAmend" CssClass="clsLabelAuto">Amend</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxTagSearch" Width="55px" ClientIDMode="Static"
                                                            MaxLength="4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right" valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" Text="Find Now"
                                                ValidationGroup="1" CausesValidation="true" ToolTip="Click to find list" />--%>

                                            <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list." ValidationGroup="1" CausesValidation="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResultReceipt" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Goods Receipt as per criteria: Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgReceiptList" runat="server" AllowPaging="True" DataKeyNames="TransactionID,TransactionDateFormatted"
                                                            EmptyDataText="No Records..." AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                                            PageSize="25" ShowHeaderWhenEmpty="false" CellPadding="7" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="TransactionID" HeaderText="TransactionID"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TransactionDateFormatted" HeaderText="Date">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TransactionTextNo" SortExpression="TransactionTextNo"
                                                                    HeaderText="Transaction No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RCIType" SortExpression="RCIType" HeaderText="From">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Issue Type">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceType" SortExpression="InvoiceType" HeaderText="Invoice Type">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Issue To">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReqType" SortExpression="ReqType" HeaderText="Requisition Type">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RequestedBy" SortExpression="RequestedBy" HeaderText="Requested By">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AircraftName" SortExpression="AircraftName" HeaderText="Reg.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Customer">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="W.O.Status">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOClosingDateFormatted" HeaderText="Closing Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOClosedBy" SortExpression="WOClosedBy" HeaderText="Closed By">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="ReOpen" HeaderText="Re-Open" Text="Re-Open">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" ForeColor="blue" Wrap="False" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
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
    </form>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var SelectedTransTypeIndex = $get("cmbTransactionType").selectedIndex;
            var Doc_TextType;
            //Receipts
            if (SelectedTransTypeIndex == 1) {
                Doc_TextType = "21";
            }
            //Issue
            else if (SelectedTransTypeIndex == 2) {
                Doc_TextType = "3";
            }
            //Order
            else if (SelectedTransTypeIndex == 3) {
                Doc_TextType = "1";
            }
            //Requisition
            else if (SelectedTransTypeIndex == 4) {
                Doc_TextType = "18";
            }
            //Work Order
            else if (SelectedTransTypeIndex == 5) {
                Doc_TextType = "16";
            }
            //Purchase Invoice
            else if (SelectedTransTypeIndex == 6) {
                Doc_TextType = "15";
            }
            //Purchase Enquiry
            else if (SelectedTransTypeIndex == 7) {
                Doc_TextType = "7";
            }
            //Purchase Quotation
            else if (SelectedTransTypeIndex == 8) {
                Doc_TextType = "8";
            }
            //Export Invoice
            else if (SelectedTransTypeIndex == 9) {
                Doc_TextType = "30";
            }
            //Audit
            else if (SelectedTransTypeIndex == 10) {
                Doc_TextType = "31";
            }
            //Discrepancy
            else if (SelectedTransTypeIndex == 11) {
                Doc_TextType = "33";
            }
            else {
                Doc_TextType = "";
            }


            $("#<%=txtTransactionText.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text' + '&TextType=' + Doc_TextType, {
                width: 185,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0

            });
        });
    </script>
</body>
</html>
