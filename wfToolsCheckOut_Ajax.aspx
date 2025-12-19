<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfToolsCheckOut_Ajax.aspx.vb"
    Inherits="Flypal.wfToolsCheckOut_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html  PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Tools CheckOut Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>

    <link rel="stylesheet" type="text/css" href="popup.css" />
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="AlertMessage.js"></script>

    <script type="text/javascript" id="clientEventHandlersJS">

        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
		function OpenLocation(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
        }

        function FireOnClickButton(e) {
            if (e.keyCode == 13 || e.keyCode == 9) {
                document.getElementById("btnAddBarcodeItem").click();
            }
        }

    </script>

</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmToolCheckOut" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                        <table id="tblinner" class="clsTablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Tools Details [New]</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
                                            </td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table border="0">
															<tr>
																<td>
																	<asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH" Text="Check Out"
																		Enabled="<%# mIssue.StatusID = 1 %>" ToolTip="Click to Check Out Tool" ValidationGroup="1" />
																</td>
																<td>
																	<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" 
                                                                        Enabled="<%# Not mIssue.IsNew %>"
																		Text="Print" ToolTip="Click to Print" />
																</td>
																<td>
																	<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" 
                                                                        Text="Close" ToolTip="Click to go back to the previous page." />
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
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Select Issue Date." ControlToValidate="txtIssueDate" Display="None"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvStoreList" runat="server" ControlToValidate="cmbStoreList"
                                                Display="None" ErrorMessage="Select store from the list." ClientValidationFunction="validateStore"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" Display="None"
                                                ErrorMessage="Remark field length must not be greater than 150 Character" ClientValidationFunction="validateNameLen"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clsValidationSummary"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <script type="text/javascript">

                                                function validateStore(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbStoreList");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }

                                                function validateNameLen(source, args) {
                                                    args.IsValid = false;

                                                    var nameLength = $get("txtRemark").value.length;
                                                    if (nameLength <= 150) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }
                                                textbox = document.getElementById('hdnIssuedToEmployeeId');
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlIssueDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tabDetails" border="0">
                                                <tr>
                                                    <td valign="top">
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                            <legend id="ldwodetail" runat="server"><b>Tools Issuing Details</b></legend>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblIssueDate" class="clsLabelAuto">Date</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtIssueDate" runat="server" ClientIDMode="Static"
																			CssClass="clsTextBoxTagSearchDate" Text="" Width="100px"
																			AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender');">
                                                                        </asp:TextBox>
                                                                        <cc2:CalendarExtender ID="IssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtIssueDate">
                                                                        </cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ID="IssueDateWatermarkExtender" runat="server" 
                                                                            TargetControlID="txtIssueDate"
                                                                            WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                        </cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblStarIssueNo" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblNo" class="clsLabelAuto">No.</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtText" runat="server" CssClass="clsTextBoxTagSearch" 
                                                                                        MaxLength="25" Text="<%# mIssue.Text %>" ToolTip="Enter Text">
                                                                                    </asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" 
                                                                                        MaxLength="4" Text="<%# mIssue.No %>" ToolTip="Enter No." Width="50px">
                                                                                    </asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblStoreStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblStore" class="clsLabelAuto">Store</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:DropDownList ID="cmbStoreList" runat="server" 
                                                                            CssClass="clsTextBoxTagSearchComboNewstyleLong" AutoPostBack="true" 
                                                                            ClientIDMode="Static" DataTextField="LocationStore" DataValueField="ID"
                                                                            SelectedValue="<%# mIssue.StoreID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span3" class="clsLabelAuto">Issued To</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtIssuedToEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
																		    OnTextChanged="IssuedToEmployee" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
																		    onChange="SetEmpIdonChange('txtIssuedToEmployee','txtIssuedToEmployee_Autocomplete')">
                                                                        </asp:TextBox>
                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtIssuedToEmployee_Autocomplete"
                                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfToolsCheckOut_Ajax.aspx"
                                                                            ServiceMethod="GetEmployeeList" TargetControlID="txtIssuedToEmployee" 
                                                                            OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" 
                                                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li" 
                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated" 
                                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                        </cc2:AutoCompleteExtender>
                                                                        <asp:HiddenField ID="hdnIssuedToEmployeeId" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span5" class="clsLabelAuto">Issued By</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtIssuedByEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
																		    OnTextChanged="IssuedByEmployee" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
																		    onChange="SetEmpIdonChange('txtIssuedByEmployee','txtIssuedByEmployee_Autocomplete')">
                                                                        </asp:TextBox>
                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtIssuedByEmployee_Autocomplete"
                                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfToolsCheckOut_Ajax.aspx"
                                                                            ServiceMethod="GetEmployeeList" TargetControlID="txtIssuedByEmployee" 
                                                                            OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" 
                                                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li" 
                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated" 
                                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                        </cc2:AutoCompleteExtender>
                                                                        <asp:HiddenField ID="hdnIssuedByEmployeeId" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span4" class="clsLabelAuto">Collected By</span>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtCollectedByEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
																		    OnTextChanged="CollectedByEmployee" AutoPostBack="true" CssClass="clsTextBoxTagSearch"
																		    onChange="SetEmpIdonChange('txtCollectedByEmployee','txtCollectedByEmployee_Autocomplete')"></asp:TextBox>
                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtCollectedByEmployee_Autocomplete"
                                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfToolsCheckOut_Ajax.aspx"
                                                                            ServiceMethod="GetEmployeeList" TargetControlID="txtCollectedByEmployee" 
                                                                            OnClientItemSelected="SetID" UseContextKey="False" ContextKey="" 
                                                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li" 
                                                                            CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated" 
                                                                            OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                        </cc2:AutoCompleteExtender>
                                                                        <asp:HiddenField ID="hdnCollectedByEmployeeId" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span1" class="clsLabelAuto">Aircraft</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            AutoPostBack="true" DataTextField="RegNo" DataValueField="ID" 
                                                                            SelectedValue="<%# mIssue.MachineID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span2" class="clsLabelAuto">Work Order</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbWorkOrder" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" 
                                                                            DataTextField="WONumber" DataValueField="ID" SelectedValue="<%# mIssue.nWOID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblRequisitionRef" runat="server" CssClass="clsLabelAuto">
                                                                            Requisition Ref.
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtRequisitionRef" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" ClientIDMode="Static" MaxLength="199" Rows="2"
                                                                            Text="<%# mIssue.ReferenceNo %>" TextMode="MultiLine" ToolTip="Enter Reference No.">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" 
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" ClientIDMode="Static" MaxLength="150" 
                                                                            Rows="2" Text="<%# mIssue.Remark %>" TextMode="MultiLine"
                                                                            ToolTip="Enter Remark">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="upnlIssueItem" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblParts" class="clsLabelHeaderItem">Issue Item(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddItem" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" 
                                                                        ToolTip="Click to Add New Issue Part"
                                                                        Enabled="<%# mIssue.StatusID = 1 %>" ValidationGroup="1" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBarcodeNos" runat="server" CssClass="clsLabelAuto"
                                                                        Enabled="<%# mIssue.StatusID = 1 %>"
                                                                        Visible="<%# mIssue.ToTypeID = 19 %>">Barcode No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBarcodeItem" runat="server" CssClass="clsTextBoxTagSearch" 
                                                                        onkeydown="javascript:FireOnClickButton(event);"
                                                                        Enabled="<%# mIssue.StatusID = 1 %>" Visible="<%# mIssue.ToTypeID = 19 %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddBarcodeItem" runat="server" CssClass="clsbtnH clsinfoH" Text="Add"
                                                                        Enabled="<%# mIssue.StatusID = 1 %>" Visible="<%# mIssue.ToTypeID = 19 %>" ClientIDMode="Static"
                                                                        ToolTip="Click to Add Barcode No" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgIssueItems" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SRNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CodeNo" SortExpression="CodeNo" HeaderText="GSE No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Itemdesc" HeaderText="Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CalibrationDueDateFormatted" HeaderText="Calibration Due Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ManufacturingDateFormatted" HeaderText="Manufacturing Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Location" HeaderText="Location">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" 
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CausesValidation="false"
                                                                            CommandArgument='<%# Eval("SrNo") %>' CommandName="DeleteRec" 
                                                                            ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                    </ItemTemplate>
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
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMessBox" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <!-- Alert Message -->
                                            <a class="poplight" href="#?w=450" rel="popup_name"></a>
                                            <div id="popup_name" class="popup_block" align="center">
                                                <div style="width: 400px; height: auto" align="left">
                                                    <table border="0" cellpadding="0">
                                                        <tr>
                                                            <td valign="middle" width="12%" align="left">
                                                                <img src="images\alert_icon.png" width="40" height="40">
                                                            </td>
                                                            <td>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblAlertTitle" runat="server" CssClass="clsTitleAlertLabel" 
                                                                                ClientIDMode="Static">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblAlertMessage" runat="server" CssClass="clsAlertLabel" Width="100%"
                                                                                ClientIDMode="Static">
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <!-- End-->
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>

		<!-- Ajax Loader -->
		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>


		<%--autocomplete css functions--%>
        <script type="text/javascript">
            //bold input value in list...
            function ClientPopulated(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
            //Alternate item style
            function ClientShowing(source, eventArgs) {
                $.elements = $(source.get_completionList());
                $.elements.find(".ac_results_li").each(function (i) {
                    if (i % 2 == 0) {
                        //$(this).addClass("ac_even");
                    }
                    else {
                        $(this).addClass("ac_odd");
                    }
                });
            }
            //add loader to textbox
            function ClientPopulating(source, e) {
                $("#" + source._element.id).addClass("ac_loading");
            }
            //remove loader from textbox
            function ClientHiding(source, eventArgs) {
                $("#" + source._element.id).removeClass("ac_loading");
            }
        </script>
        <%--End--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

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
        <%--
        Autocomplete functions to set id--%>
        <script type="text/javascript">
            function SetID(source, e) {
                //get id from autocomplete list
                var node;
                var value = e.get_value();

                if (value) node = e.get_item();
                else {
                    value = e.get_item().parentNode._value;
                    node = e.get_item().parentNode;
                }

                var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
                source.get_element().value = text;

                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtIssuedByEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnIssuedByEmployeeId');
                }
                if (source._id == "txtIssuedToEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnIssuedToEmployeeId');
                }
                if (source._id == "txtCollectedByEmployee_Autocomplete") {
                    textbox = document.getElementById('hdnCollectedByEmployeeId');
                }


                textbox.value = value.toString();
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetEmpIdonChange(cntrl, extender) {
                var cntrlName = '#' + cntrl;
                var popup = $find(extender);
                var complist = popup.get_completionList();
                var text = $(cntrlName).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;
                        if (cntrl == "txtIssuedByEmployee") {
                            var textbox = document.getElementById('hdnIssuedByEmployeeId');
                        }
                        if (cntrl == "txtIssuedToEmployee") {
                            textbox = document.getElementById('hdnIssuedToEmployeeId');
                        }
                        if (cntrl == "txtCollectedByEmployee") {
                            textbox = document.getElementById('hdnCollectedByEmployeeId');
                        }
                        textbox.value = val.toString();
                        return;
                    }

                }
                if (cntrl == "txtIssuedByEmployee") {
                    var textbox = document.getElementById('hdnIssuedByEmployeeId');
                }
                if (cntrl == "txtIssuedToEmployee") {
                    textbox = document.getElementById('hdnIssuedToEmployeeId');
                }
                if (cntrl == "txtCollectedByEmployee") {
                    textbox = document.getElementById('hdnCollectedByEmployeeId');
                }
                textbox.value = '';
                return;
            }
                                  
        </script>

    </form>
</body>
</html>
