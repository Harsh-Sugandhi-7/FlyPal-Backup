<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfListOFEnquiries_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfListOFEnquiries_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>List of Enquires/Requisitions for Quotation Comparison</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblEnquiryList" runat="server" CssClass="clsFormHeader">List of Enquires/Requisitions for Quotation Comparison</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                    ToolTip="Click to Add New Enquiry" CausesValidation="False" Visible="false"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                    ToolTip="Click to close List of Enquiry screen" CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function showTextField() {
                                        var SearchIndex = $get("cmbSearch").selectedIndex;

                                        var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                        var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                        var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                        var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                        if (SearchIndex != 1) {
                                            txtFromDateobj.style.display = 'none';
                                            txtToDateobj.style.display = 'none';
                                            lblFromDateobj.style.display = 'none';
                                            lblToDateobj.style.display = 'none';
                                        }
                                        else {
                                            var DateIndex = $get("cmbDate").selectedIndex;
                                            if (DateIndex == 0) {
                                                txtFromDateobj.style.display = 'none';
                                                txtToDateobj.style.display = 'none';
                                                lblFromDateobj.style.display = 'none';
                                                lblToDateobj.style.display = 'none';
                                            }
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="6">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rAgainstEnquiry" runat="server" CssClass="clsRadioButton" Text="Against Enquiry"
                                                                    GroupName="a" AutoPostBack="true" Checked="true"></asp:RadioButton>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rbAgainstRequisition" runat="server" CssClass="clsRadioButton"
                                                                    Text="Against Requisition" GroupName="a" AutoPostBack="true"></asp:RadioButton>
                                                            </td>
                                                            <%-- <td>
                                                                <asp:CheckBox ID="chkDoneOrder" runat="server" CssClass="clsLabelAuto" Text="Done Order"
                                                                    TextAlign="right" AutoPostBack="true"></asp:CheckBox>
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSearch" class="clsLabel" style="height: 10px; width: 48px;">Search</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Width="170px"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <span id="L1" class="clsLabel" style="width: 20px;"></span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                    Visible="False">
                                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                    <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                    <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                    <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                    <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                    <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                    <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cmbEnquiryText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    AutoPostBack="True" Visible="False" DataValueField="Text" DataTextField="Text">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    AutoPostBack="True" Visible="False" DataTextField="Text" DataValueField="Text">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                    MaxLength="100"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                    MaxLength="8"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtVendorNo" runat="server" CssClass="clsTextBoxMedium_Ajax" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server">From Date </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                        ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:CheckBox ID="chkDoneOrder" runat="server" CssClass="clsLabelHeader" TextAlign="right"
                                                        AutoPostBack="true" Text="Done Order"></asp:CheckBox>
                                                    <span class="clsLabel">(Check to get enquiry or requisition against which purchase order
                                                        has been created.)</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                            ValidationGroup="a" ToolTip="Click to find list of Enquiry as per searching criteria"
                                            CausesValidation="true"></asp:Button>--%>

                                        <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                            ToolTip="Click to find list of Enquiry as per searching criteria" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Enquires for Quotation Comparison as per criteria : Record(s) found</asp:Label>
                                                    <asp:Label ID="lblReqResult" runat="server" CssClass="clsLabelAuto " Font-Bold="True">List of Requisitions for Quotation Comparison as per criteria : Record(s) found</asp:Label>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                            ToolTip="Click to Add New Enquiry" CausesValidation="False" Visible="false">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                            ToolTip="Click to close List of Enquiry screen" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgEnqList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID"
                                                        ShowHeaderWhenEmpty="true" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
                                                        PageSize="25">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="EnquiryNo" SortExpression="EnquiryNo" HeaderText="Enq. No.">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="VendorName" SortExpression="VendorName" HeaderText="Supplier"
                                                                HtmlEncode="false">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Width="500px" Wrap="true" CssClass="TextBreak" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="Status">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Authorized By">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="EnquiryType" SortExpression="EnquiryType" HeaderText="Enquiry Type">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:TemplateField HeaderText="Create Order" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IDCreateOrder" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        Text="Create Order" CommandName="CreateOrder"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--9--%>
                                                            <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IDView" runat="server" CommandArgument='<%# Eval("ID") %>' Text="View"
                                                                        CommandName="View"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="QuotationCount" SortExpression="QuotationCount" HeaderText="QuotationCount"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="IsQuotationCount" SortExpression="IsQuotationCount" HeaderText="IsQuotationCount"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgRequisitionList" runat="server" AllowPaging="True" AllowSorting="True"
                                                        AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="25" ShowHeaderWhenEmpty="true"
                                                        DataKeyNames="ID">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionTextNo" HeaderText="Req. No." SortExpression="RequisitionTextNo">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WorkShopName" HeaderText="WorkShop" SortExpression="WorkShopName">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReqTypeName" HeaderText="Type" SortExpression="ReqTypeName">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequisitionEngineeringBranch" HeaderText="Branch" SortExpression="RequisitionEngineeringBranch">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="Location">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Requested By" SortExpression="EmployeeName">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Create Order" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IDCreateOrder" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        Text="Create Order" CommandName="CreateOrder" Enabled='<%#  Eval("IsQuotationCount")%>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="IDView" runat="server" CommandArgument='<%# Eval("ID") %>' Text="View"
                                                                        CommandName="View"></asp:LinkButton>
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                        ToolTip="Click to Add New Enquiry" CausesValidation="False" Visible="false">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close"
                                                        CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td colspan="2" style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnCommonPartList" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            showTextField();
        });    
    </script>
    <!-- Common Part List Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCommonPartList" Text="Dummy Common Part List"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCommonPartList" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupCommonPartList" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCommonPartList" runat="server" TargetControlID="btnDummyCommonPartList"
        PopupControlID="pnlPopupCommonPartList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCommonPartListStateComplete() {
            $("#btnDummyCommonPartList").click();
            $get("AjaxLoader").style.visibility = "hidden";

        }

        function OpenPartsWindow(ID, ReqID, IsAgainstEnquiry, IsAgainstRequisition, DoneOrder) {

            try {
                $get("AjaxLoader").style.visibility = "visible";
                //$("#iPopupCommonPartList").attr("src", "wfCommonPartList_Ajax.aspx?EnqID=" + ID);
                $("#iPopupCommonPartList").attr("src", "wfQuotationListForComparison_Ajax.aspx?Type=pup&EnqID=" + ID + "&ReqID=" + ReqID + "&AgainstEnquiry=" + IsAgainstEnquiry + "&AgainstRequisition=" + IsAgainstRequisition + "&DoneOrders=" + DoneOrder);
                if (!$.browser.msie) {
                    $("#btnDummyCommonPartList").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForCommonPartList() {
            var CommonPartListWindow = $find("<%=mdlPopupCommonPartList.ClientID %>");
            //close Common Part List popup window
            CommonPartListWindow.hide();
            $("#iPopupCommonPartList").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnCommonPartList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
