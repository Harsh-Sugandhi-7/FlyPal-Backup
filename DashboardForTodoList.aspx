<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashboardForTodoList.aspx.vb"
    Inherits="Flypal.DashboardForTodoList" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE HTML>
<html>
<head runat="server">
    <title>Dashboard</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9,10,11" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <style type="text/css">
        .contentStickyNote
        {
            position: absolute;
            top: 40px;
            padding: 0px;
            margin: 0px;
            height: 300px;
            left: 300px;
        }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
   
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlDashBoard" Style="z-index: 102; left: 16px; position: absolute;
        top: 8px" runat="server">
        <table id="Table4" cellspacing="1" cellpadding="1" width="810" border="0">
            <tr>
                <td>
                    <asp:UpdatePanel ID="pnlReports" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                <tr>
                                    <td>
                                        <table>
                                            <%-- Added by Shital on 18-Mar-2021--%>
                                            <tr>
                                                <%-- Added by Prashant on 23-Mar-2021--%>
                                                <asp:PlaceHolder ID="phOpenRequisitionList" runat="server" Visible="false">
                                                    <td>
                                                        <fieldset id="Fieldset1" style="border-width: 1px;">
                                                            <span class="clsLabelHeader">Requisition(s) for Authorization</span>
                                                            <asp:UpdatePanel ID="upnlOpenRequisitionList" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgRequisitionList" runat="server" AllowPaging="True" 
                                                                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" DataKeyNames="ID" PageSize="10"
                                                                        ShowHeaderWhenEmpty="true">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RequisitionTextNo" HeaderText="Requisition No." SortExpression="RequisitionTextNo" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RequisitionEngineeringBranch" HeaderText="Branch" SortExpression="RequisitionEngineeringBranch" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                                  <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="Location" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False"/>
                                                                                  <ItemStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Requested By" SortExpression="EmployeeName" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle  HorizontalAlign="Left"  Wrap="False"  />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="StatusName" HeaderText="Status" SortExpression="StatusName" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                           
                                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="Black">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                                </asp:PlaceHolder>
                                                <%-- END--%>
                                           </tr>
                                            <asp:PlaceHolder ID="PhPendingOrderforAuthorization" runat="server" Visible="false">
                                                    <td valign="top">
                                                        <fieldset id="fdsOpenRequisitionList" style="border-width: 1px;">
                                                            <asp:UpdatePanel runat="server" ID="upnlPendingOrder" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblLedgerList" runat="server" CssClass="clsLabelAuto" Style="text-align: center;
                                                                        font-weight: bold">List Of Pending Orders for Athorization</asp:Label>
                                                                    <asp:GridView ID="grdPendingOrders" runat="server" Width="100%" AllowPaging="true" CssClass="table table-striped table-bordered table-hover"
                                                                        PageSize="10" AutoGenerateColumns="False" DataKeyNames="ID" EmptyDataText="There are no data records to display.">
                                                                           <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                            <Columns>
                                                                            <%--0--%>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--1--%>
                                                                            <asp:BoundField DataField="OrderDateFormatted" HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <%--2--%>
                                                                            <asp:BoundField DataField="OrderNo" HeaderText="Number" SortExpression="OrderNo" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <%--3--%>
                                                                            <asp:BoundField DataField="OrderType" HeaderText="Type" SortExpression="OrderType" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left"  Wrap="False"  />
                                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                            </asp:BoundField>

                                                                            <%--4--%>
                                                                            <asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total" SortExpression="CGrandTotal" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                            <%--5--%>
                                                                            <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--6--%>
                                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <%--7--%>
                                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="Black" >
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <%--8--%>
                                                                            <asp:BoundField DataField="TransID" HeaderStyle-CssClass="hideGridColumn" HeaderText="TransTypeID"
                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <PagerSettings Position="Bottom" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right"  />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                            </asp:PlaceHolder>
                                            <tr>
                                                <asp:PlaceHolder ID="phPendingToReceiptERO" runat="server" Visible="false">
                                                    <td>
                                                        <fieldset id="Fieldset9" style="border-width: 1px;">
                                                            <asp:UpdatePanel runat="server" ID="upnlPendingToReceiptERO" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto" Style="text-align: center;
                                                                        font-weight: bold">List Of Pending Receipt for ERO</asp:Label>
                                                                    <asp:GridView ID="grdPendingToReceiptforERO" runat="server" CssClass="table table-striped table-bordered table-hover" AllowPaging="true"
                                                                        PageSize="10" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No." >
                                                                                <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Qty" SortExpression="Qty" HeaderText="Qty." HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="PendingItemQty" SortExpression="PendingItemQty" HeaderText="Balance Qty." >
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No." >
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"  Wrap="False" ></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RequestedBy" SortExpression="RequestedBy" HeaderText="Requested By" >
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"  Wrap="False" ></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                                </asp:PlaceHolder>
                                           </tr>
                                            <tr>
                                            <asp:PlaceHolder ID="PhPendingToIssueERO" runat="server" Visible="false">
                                                    <td valign="top">
                                                        <fieldset id="Fieldset7" style="border-width: 1px;">
                                                            <asp:UpdatePanel runat="server" ID="UpnlPendingToIssueforERO" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Style="text-align: center;
                                                                        font-weight: bold">List Of Pending Issue for ERO</asp:Label>
                                                                    <asp:GridView ID="grdPendingToIssueforERO" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover" AllowPaging="true"
                                                                        PageSize="10" AutoGenerateColumns="False" EmptyDataText="There are no data records to display.">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                         <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                          <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Date" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ReceiptText" SortExpression="ReceiptText" HeaderText="Number" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part Number" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField Visible="False" DataField="LoanTakenQty" SortExpression="LoanTakenQty"
                                                                                HeaderText="Loan Taken Qty." HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="LoanQty" SortExpression="LoanQty" HeaderText="ERO Qty." HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Right"  Wrap="False" ></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="OrderType" SortExpression="OrderType" HeaderText="Order Type" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left"  Wrap="False" ></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectPart" HeaderStyle-ForeColor="Black">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                        <PagerSettings Position="Bottom" />
                                                                        <PagerStyle CssClass="paging" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                            </asp:PlaceHolder>
                                            </tr>
                                            <tr>
                                                <asp:PlaceHolder ID="phPendingtoRecover" runat="server" Visible="false">
                                                    <td>
                                                        <fieldset id="Fieldset2" style="border-width: 1px;">
                                                            <asp:UpdatePanel runat="server" ID="UpnlPendingtoRecoverLoan" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto" Style="text-align: center;
                                                                        font-weight: bold">List Of Loan Pending to Recover</asp:Label>
                                                                    <asp:GridView ID="dgPendingTORecoverLoan" runat="server" CssClass="table table-striped table-bordered table-hover"  AllowPaging="true"  PageSize="10" 
                                                                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                                <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ItemDescription"  HeaderText="Description">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                             <asp:BoundField DataField="IssueTextNo"  HeaderText="Issue No">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                             <asp:BoundField DataField="IssueDateFormatted"  HeaderText="Issue Date">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Qty"  HeaderText="Qty.">
                                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="PendingItemQty" HeaderText="Balance Qty.">
                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="SerialNo"  HeaderText="Serial No.">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="RequestedBy" HeaderText="Requested By">
                                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
                                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="Black"/>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                                </asp:PlaceHolder>
                                            </tr>
                                            <tr>
                                            <asp:PlaceHolder ID="phPendingtoReturn" runat="server" Visible="false">
                                                 <td valign="top">
                                                        <fieldset id="Fieldset3" style="border-width: 1px;">
                                                            <asp:UpdatePanel runat="server" ID="UpnlPendingtoReturnLoan" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelAuto" Style="text-align: center;
                                                                        font-weight: bold">List Of Loan Pending to Return</asp:Label>
                                                                    <asp:GridView ID="dgPendingListToReturnLoan" runat="server" CssClass="table table-striped table-bordered table-hover" 
                                                        ShowHeaderWhenEmpty="True" EnableViewState="false" AllowPaging="True" PageSize="10"
                                                         AutoGenerateColumns="False">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ReceiptItemID" HeaderText="ReceiptItemID">
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part Number">
                                                                <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Part Desc.">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                        
                                                            <asp:BoundField DataField="ReceiptTextReceiptNo" SortExpression="ReceiptTextReceiptNo"
                                                                HeaderText="Receipt No." HtmlEncode="false" >
                                                                <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptDateFormatted" SortExpression="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanTakenQty" SortExpression="LoanTakenQty" HeaderText="Loan Taken Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" SortExpression="LoanQty" HeaderText="Loan To Return Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="Black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FromStoreName" SortExpression="FromStoreName" HeaderText="From">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ToStoreName" SortExpression="ToStoreName" HeaderText="To Store">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                        
                                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpQtrYear" SortExpression="ExpQtrYear" HeaderText="Expiry Qtrs.">
                                                                <HeaderStyle ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                        
                                                          
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select" HeaderStyle-ForeColor="Black"></asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                            </asp:PlaceHolder>
                                            </tr>
                                            <%-- END--%>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </asp:Panel>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgServiceableList tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");

                    }
                });
            });

            $("#chkUnServiceableSelectAll").live("click", function () {
                var status = $("#chkUnServiceableSelectAll").attr("checked");
                $("#dgUnServiceableList tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");

                    }
                });
            });

            $("#chkReturnableSelectAll").live("click", function () {
                var status = $("#chkReturnableSelectAll").attr("checked");
                $("#dgReturnableList tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                    }
                    else {
                        $(this).removeAttr("checked");

                    }
                });
            });


        });
    </script>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': true };
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
    <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    <script type="text/javascript">
        function OpenGreetingsWindow() {
            window.open("wfGreetings.aspx", "Open", "top=30,left=200,width=960,height=690,toolbar=no,menubar=no,location=no,toolbar=no");
            return true;
        }
    </script>
    <!-- Quotation --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyQuotationDetail" Text="Dummy Quotation Detail" />
    </div>
    <asp:Panel runat="server" ID="Panl1" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table8">
                <tr>
                    <td colspan="1">
                        <div style="margin: 3px; padding: 3px;">
                            <asp:UpdatePanel ID="upnlQuotationDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="Table9">
                                        <tr>
                                            <td>
                                                <span id="Span12" runat="server" class="clstitle1">Quotation(s) for Invoice</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblQuotationList" runat="server" CssClass="clsLabelHeader">List of Quotation as per criteria :  Record(s) found.</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="max-height: 150px; overflow-y: auto; overflow-x: hidden;">
                                                    <asp:GridView ID="dgQuotationDetailList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        DataKeyNames="ID,CWPTextNo" ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true"
                                                        PageSize="10">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="MROQuotationDateFormatted" HeaderText="Date">
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" Wrap="False" />
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="QuotationText" HeaderText="Quotation" SortExpression="QuotationText">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CWPTextNo" HeaderText="Work Order No." SortExpression="CWPTextNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MROProjectNumber" HeaderText="Project" SortExpression="MROProjectNumber">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CustomerRONo" HeaderText="Customer RO No." SortExpression="CustomerRONo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PartDescription" HeaderText="Part No." SortExpression="PartDescription">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                    Font-Underline="False" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField CommandName="SelectRec" HeaderStyle-HorizontalAlign="Left" HeaderText="Select"
                                                                Text="Select" />
                                                            <asp:BoundField DataField="IsOEM" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsOEM"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle CssClass="hideGridColumn" />
                                                                <ItemStyle CssClass="hideGridColumn" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="bottom">
                                                <table id="Table14" height="100%" cellspacing="0" cellpadding="0" align="right" border="0">
                                                    <tr>
                                                        <td valign="bottom" align="right">
                                                            <asp:Button ID="btnBackQuotationDet" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Quotation Detail screen"
                                                                Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="lnkQuotationDetail_ModalPopupExtender" runat="server"
        TargetControlID="btnDummyQuotationDetail" PopupControlID="Panl1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- OEM Quotation --ModalPopUp -->
    <div style="display: none;">
        <asp:Button runat="server" ID="btnDummyOEMQuotationDetail" Text="Dummy OEM Quotation Detail" />
    </div>
    <asp:Panel runat="server" ID="Panel1" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table1">
                <tr>
                    <td colspan="1">
                        <div style="margin: 3px; padding: 3px;">
                            <asp:UpdatePanel ID="upnlOEMQuotationDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="clstablelistin" id="Table2">
                                        <tr>
                                            <td>
                                                <span id="Span13" runat="server" class="clstitle1">Selection of OEM</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbOEMQuote" GroupName="X" runat="server" Checked="true" />
                                                            <asp:Label ID="lblOEMCustomer" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbNormalQuote" GroupName="X" runat="server" />
                                                            <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            <asp:Label ID="lblProjectID" runat="server" CssClass="clsLabelHeader" Style="display: none;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="bottom">
                                                <table id="Table3" cellspacing="1">
                                                    <tr>
                                                        <td valign="bottom" align="right">
                                                            <asp:Button ID="btnOEMOk" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Create Quotation"
                                                                Text="Create" CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td valign="bottom" align="right">
                                                            <asp:Button ID="btnBackOEMQuotationDet" runat="server" CssClass="clsButton_Ajax"
                                                                ToolTip="Click to close screen" Text="Close" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="lnkOEMQuotationDetail_ModalPopupExtender" runat="server"
        TargetControlID="btnDummyOEMQuotationDetail" PopupControlID="Panel1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    </form>
    <script type="text/javascript">
		if("<%= not HttpContext.Current.Session("StyleSheet") is  nothing %>"=="True")
			{
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
			}
    </script>
</body>
</html>
