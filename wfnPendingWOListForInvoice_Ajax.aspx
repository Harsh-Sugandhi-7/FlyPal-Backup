<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnPendingWOListForInvoice_Ajax.aspx.vb"
    Inherits="Flypal.wfnPendingWOListForInvoice_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List W.O. for Invoice</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />  
    <link id="MainStyle" type="text/css" rel="stylesheet" />    
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script src="jquery-1.11.1.min.js" type="text/javascript"></script>
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>	
</head>
<body>
    <form id="WOInvoiceListForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain" style="margin-top: 5px; margin-left: 5px;">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<span id="lblTitle" class="clsFormHeader">
                                                List Of Work Order(s)
											</span>
										</td>
										<td align="right">
											<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip="Click to go back to WO Invoice List screen." Text="Back">
													</asp:Button>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields." ValidationGroup="a">
                                </asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" 
                                    ErrorMessage="Issue Date is Required." ControlToValidate="txtDate" Display="None" 
                                    ValidationGroup="a">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlWODetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblDate">Invoice Date </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearchDate"
														AutoPostBack="true" onchange="ValidateDateText(this,'txtDate_watermarkextender');">
                                                    </asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <span id="Span4">Work Order</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbWorkOrder" 
                                                        runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                        DataValueField="ID" DataTextField="WONumber">
                                                    </asp:DropDownList>
                                                </td>
                                                  <td align="right" >
													  <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
														  ToolTip="Click to search as per searching Criteria."
														  ValidationGroup="a" CausesValidation="false" CssClass="clsSearch2btn" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustomer" runat="server" >Customer
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbCustomerList" 
                                                        runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
														DataTextField="Name" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                              
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="true">
                                                        List of WO as per criteria : 0 Record(s) found.
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:GridView ID="dgWOList" runat="server" DataKeyNames="ID" EnableViewState="false" 
                                                        ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" ToolTip="List of Work Order(s)."
														AllowSorting="True" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" 
                                                        AllowPaging="True" PageSize="10">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="WONumber" SortExpression="WONo" HeaderText="WO. No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WODateFormatted" HeaderText="WO. Date">
                                                                <HeaderStyle HorizontalAlign="Left"  />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WOBy" SortExpression="WOBy" HeaderText="Created By">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
    </form>
</body>
</html>
