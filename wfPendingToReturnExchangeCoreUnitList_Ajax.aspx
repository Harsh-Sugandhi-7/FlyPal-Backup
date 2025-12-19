<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPendingToReturnExchangeCoreUnitList_Ajax.aspx.vb"
    Inherits="Flypal.wfPendingToReturnExchangeCoreUnitList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending To Return To Supplier Exchange Core Unit</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
							<td class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<span id="lbltitle" class="clsFormHeader">
                                                Pending To Return To Supplier Exchange Core Unit
											</span>
										</td>
										<td align="right">
											<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="tblActionBtns">
														<tr>
															<td>
																<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
																	runat="server" ToolTip="Click to close this screen."
																	Text="Close" CausesValidation="False"></asp:Button>
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
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Issue Date Required"
                                    ControlToValidate="txtDate" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <span id="lblLabel" class="clsLabelAuto" style="padding-left: 3px;">Enter Issue Date,
                                                        Part No For Search .</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <span id="lblIssueDate" class="clsLabelAuto">Issue Date</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                    onchange="ValidateDateText(this,'txtDate_watermarkextender');"
																	AutoPostBack="true" Width="100px">
                                                                </asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="txtDate_watermarkextender" runat="server"
																	ClientIDMode="Static" TargetControlID="txtDate"
																	WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td align="left">
                                                                <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																	AutoPostBack="true" ToolTip="Enter Part No"></asp:TextBox>
                                                            </td>                                                            
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgPendingList" runat="server" AutoGenerateColumns="False"
														ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" 
                                                        CellPadding="5" AllowPaging="True" PageSize="10">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
															ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Date">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReceiptText" HeaderText="Number" SortExpression="ReceiptText">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemName" HeaderText="Part Number" SortExpression="ItemName">
                                                                <HeaderStyle  HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ItemDesc" HeaderText="Part Desc." SortExpression="ItemDesc">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LoanQty" HeaderText="ERO Qty." SortExpression="LoanQty">
                                                                <HeaderStyle  HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="ReleaseNoteNo" HeaderText="R.N. No." SortExpression="ReleaseNoteNo">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StoreName" HeaderText="Store" SortExpression="StoreName">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="R.N. Date">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpQtrYear" HeaderText="Expiry Qtrs" SortExpression="ExpQtrYear">
                                                                <HeaderStyle  HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField CommandName="SelectForDiscard" HeaderText="Discard" Text="Discard">
                                                                <HeaderStyle HorizontalAlign="Left" />
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
