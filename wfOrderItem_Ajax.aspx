<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOrderItem_Ajax.aspx.vb"
    Inherits="Flypal.wfOrderItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Item Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout">
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Purchase Order Item [New]</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table1" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add Item in Order Item List"
                                                                            ValidationGroup="1" Text="OK"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                            Text="Back" CausesValidation="False"></asp:Button>
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
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" Display="None" CssClass="clsLabelAuto"
                                            ControlToValidate="txtPartNo" ErrorMessage="Part Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" Display="None" CssClass="clsLabelAuto"
                                            ControlToValidate="txtQty" ErrorMessage="Quantity Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" Display="None" CssClass="clsLabelAuto"
                                            ControlToValidate="txtDescription" ErrorMessage="Part can't be saved without Description."
                                            Width="72px" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCRate" runat="server" ControlToValidate="txtRate" Display="None"
                                            ErrorMessage="Rate Must be greater than Zero." OnServerValidate="customvalidate"
                                            CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" ControlToValidate="txtQty" Display="None"
                                            ErrorMessage="Quantity must be greater than Zero." OnServerValidate="customvalidate"
                                            CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDiscount" runat="server" ControlToValidate="txtPerDiscount"
                                            Display="None" ErrorMessage="Discount can not be greater than 100 %" OnServerValidate="customvalidate"
                                            CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvSelection" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ErrorMessage="Select Schedule Expense Or Not." ClientValidationFunction="ValidateScheduleExpenses"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <%-- <asp:CustomValidator ID="cvYesNo" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ErrorMessage="Select Schedule Expense Either Yes Or No." ClientValidationFunction="ValidateScheduleExpensesIfYesNoBoth"
                                            ValidationGroup="1"></asp:CustomValidator>--%>
                                        <script type="text/javascript">
                                           function ValidateScheduleExpenses(source, args) {
                                           if ('<%# AppSettings("ClientCode") %>'  == "BA"){
                                            if ((<%# mOrder.TransTypeID %>  == 31)|| (<%# mOrder.TransTypeID %>  == 38)){
                                                  args.IsValid = false;
                                                var statusYes = $('#chkScheduleExpensesYes').attr('checked');
                                                var statusNo = $('#chkScheduleExpensesNo').attr('checked');
                                                if (statusYes == "checked" || statusNo == "checked") {
                                                    args.IsValid = true;
                                                    return;
                                                }
											}
											else{
												args.IsValid = true;
                                                    return;
											}
                                            }
                                            }
                                        </script>
                                        <%-- <script type="text/javascript">
                                            function ValidateScheduleExpensesIfYesNoBoth(source, args) {
                                                args.IsValid = true;
                                                var statusYes = $('#chkScheduleExpensesYes').attr('checked');
                                                var statusNo = $('#chkScheduleExpensesNo').attr('checked');
                                                if (statusYes == "checked" && statusNo == "checked") {
                                                    args.IsValid = false;
                                                    return;
                                                }
                                            }
                                        </script>--%>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span id="lblNote1" class="clsLabelAuto">Enter the Details of Items Ordered by selecting
                        the Part No. from list and mention the Qty. and the Rate</span>
                </td>
            </tr>
            <!--**********************************************************-->
            <tr>
                <td valign="top">
                    <asp:Panel runat="server" ID="pnlOrderItemInformation" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlOrderItemInformation" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="Fieldset9" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader">
                                    <legend><b>Order Item Information</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="spnSrNo" class="clsLabel">Sr. No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                    MaxLength="4" ReadOnly="True" Text="<%# mOrder.OrderItems.CurrentItem.SrNo %>"
                                                    ToolTip="Sr. No." Width="36px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="spnPartNoStar" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="spnPartNo" class="clsLabel">Part No.</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                    ReadOnly='<%# Session("Edit") %>' Text="<%# mOrder.OrderItems.CurrentItem.ItemName %>"
                                                    ToolTip="Enter Part No.">
                                                </asp:TextBox>
                                                <%--<asp:Button ID="imgbtnPartNo" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                    Enabled='<%# Not Session("Edit") %>' Height="22px" Text="..." ToolTip="Click to Select New Part No." />--%>
                                                <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                    Enabled='<%# Not Session("Edit") %>' CausesValidation="False" Style="margin-top: 6px"
                                                    Width="24px" ToolTip="Click to Select New Part No."></asp:ImageButton>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH" Text="Alternate Part"
                                                    Visible="<%# mOrder.AgainstTypeID = 6 %>" ToolTip="Click to add Alternate Part" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="spnDescription" class="clsLabel">Description</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchLong"
                                                    MaxLength="200" ReadOnly="True" Text="<%# mOrder.OrderItems.CurrentItem.ItemDescription %>"
                                                    ToolTip="Part Description" Height="36px" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblPartType" class="clsLabel">Part Type</span>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mOrder.OrderItems.CurrentItem.ItemTypeID %>"
                                                    Width="200px" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
                <td valign="top">
                    <asp:Panel runat="server" ID="pnlInformation" Style="width: auto;">
                        <asp:UpdatePanel runat="server" ID="upnlInformation" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset id="fdsInformation" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                    runat="server" visible="<%# mOrder.AgainstTypeID <> 1 and mOrder.AgainstTypeID <> 3 %>"
                                    class="clsLabelHeader">
                                    <legend id="lgdInformation" runat="server"><b>Information</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSalesOrderNo" runat="server" CssClass="clsLabelAuto">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSalesOrderNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mOrder.OrderItems.CurrentItem.FromNo %>"
                                                    MaxLength="8" BackColor="#E0E0E0" ReadOnly="True" Enabled="false">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSalesOrderDate" runat="server" CssClass="clsLabelAuto">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSalesOrderDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                    Width="100px" BackColor="#E0E0E0" ReadOnly="true" ClientIDMode="Static" Text="<%# mOrder.OrderItems.CurrentItem.FromDateFormatted %>" Enabled="false"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtSalesOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtSalesOrderDate">
                                                </cc2:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <fieldset id="Fieldset2" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                            runat="server" visible="<%# mOrder.TransTypeID = Flypal.Util.Trans.PurchaseOrderForExchangeRepair OR mOrder.TransTypeID = Flypal.Util.Trans.OverHaulRepairOrder  %>"
                            class="clsLabelHeader">
                            <legend id="Legend1" runat="server"><b>Warranty Applicable</b></legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkWarrantyApplicable" runat="server" Checked="<%# mOrder.OrderItems.CurrentItem.IsWarrantyApplicable %>"
                                            CssClass="clsCheckBox" ToolTip="Check If Warranty Applicable" Text="Warranty Applicable" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel runat="server" ID="upnlReceivingInformation" UpdateMode="Conditional">
                        <ContentTemplate>
                            <fieldset id="Fieldset5" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                class="clsLabelHeader">
                                <legend><b>Values</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spnQtyStar" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="spnQty" class="clsLabel">Qty.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="9" Text="<%# mOrder.OrderItems.CurrentItem.Qty  %>" ToolTip="Enter Quantity."></asp:TextBox>
                                            <asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" SelectedValue="<%# mOrder.OrderItems.CurrentItem.UnitID %>"
                                                Width="140px" Enabled="False">
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mOrder.OrderItems.CurrentItem.UnitName %>"
                                                BackColor="#E0E0E0" ReadOnly="True">
                                            </asp:TextBox>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblRate" class="clsLabel">List Price</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="12" Text="<%# mOrder.OrderItems.CurrentItem.CRate %>" ToolTip="Enter Rate"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblPercentageDiscount" class="clsLabel">Discount</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPerDiscount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="5" Text="<%# mOrder.OrderItems.CurrentItem.PerDiscount %>" ToolTip="Enter Discount"
                                                Width="150px"></asp:TextBox>
                                            <span id="lblInPercentage" class="clsLabelAuto">%</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblNetRate" class="clsLabelAuto">Net Rate</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNetRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                BackColor="#E0E0E0" ReadOnly="True" MaxLength="12" Text="<%# mOrder.OrderItems.CurrentItem.NetRate %>"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="lblAmount" class="clsLabel">Amount</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                BackColor="#E0E0E0" ReadOnly="True" MaxLength="12" Text="<%# mOrder.OrderItems.CurrentItem.CAmount %>"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <span id="LblBillBackRate" class="clsLabel" runat="server" visible="<%# mOrder.TransTypeID  <> 5 %>">
                                                Bill back Rate</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbillBackRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                MaxLength="6" Visible="<%# mOrder.TransTypeID  <> 5 %>" Width="150px" Text="<%# mOrder.OrderItems.CurrentItem.CBillBackRate %>"
                                                ToolTip="Enter Bill Back Rate">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="top">
                    <asp:UpdatePanel runat="server" ID="upnlReceivingInformation1" UpdateMode="Conditional">
                        <ContentTemplate>
                            <fieldset id="Fieldset6" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader"
                                runat="server" visible="<%# mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38 %>">
                                <legend><b>Values</b></legend>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblScheduleExpensesStar" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span runat="server" id="lblScheduleExpenses" class="clsLabelAuto">Schedule Expenses</span>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkScheduleExpensesYes" runat="server" Checked="<%# mOrder.OrderItems.CurrentItem.IsScheduleExpensesYes %>"
                                                CssClass="clsCheckBox" ToolTip="Check For Schedule Expenses" Text="Yes" onclick="OnClickOfScheduleExpensesYes();" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkScheduleExpensesNo" runat="server" Checked="<%# mOrder.OrderItems.CurrentItem.IsScheduleExpensesNo %>"
                                                CssClass="clsCheckBox" ToolTip="Check For Schedule Expenses" Text="No" onclick="OnClickOfScheduleExpensesNo();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSerialNo" runat="server" CssClass="clsbtnH clsinfoH" Text="Serial No."
                                                ToolTip="Click here to select Stock Part" CausesValidation="False" />
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mOrder.OrderItems.CurrentItem.SerialNo %>"
                                                MaxLength="100" ToolTip="Enter Serial No.">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <asp:UpdatePanel ID="upnlTabDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <cc2:TabContainer ID="tabOrderItemDetailsContainer" runat="server" class="clstablelistin"
                                Visible="true">
                                <cc2:TabPanel ID="tabApplicableToDetails" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Applicable To" ID="lblApplicableTo"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlExpiryInformation" Style="width: auto;">
                                            <asp:UpdatePanel runat="server" ID="upnlExpiryInformation" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9000;"
                                                        class="clsLabelHeader">
                                                        <legend><b>[Eg. Aircraft/ Engine/ Ground Equipment]</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:CustomValidator ID="cvModelName" runat="server" ControlToValidate="cmbApplicable"
                                                                        Display="None" ErrorMessage="Select Applicable Model From the List." OnServerValidate="customvalidate"
                                                                        CssClass="clsLabelAuto" ValidationGroup="1"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblApplicable" class="clsLabel">Applicable To</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbApplicable" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                        SelectedValue="<%# mOrder.OrderItems.CurrentItem.ModelID %>" DataTextField="ModelAndTypeName"
                                                                        DataValueField="ID" Width="200px" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblPriority" class="clsLabel">Priority</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                        SelectedValue="<%# mOrder.OrderItems.CurrentItem.PriorityID %>" DataTextField="Name"
                                                                        DataValueField="ID" Width="200px" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblRequstedBy" class="clsLabel">Requested By</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRequestedBy" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="250"
                                                                        Text="<%# mOrder.OrderItems.CurrentItem.RequestedBy %>" ToolTip="Enter Requested By"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDeliveryInDays" class="clsLabel">Delivery In</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDeliveryInDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        Text="<%# mOrder.OrderItems.CurrentItem.DeliveryInDays %>" MaxLength="6" ToolTip="Enter Delivery in Days" ClientIDMode="Static" ></asp:TextBox>
                                                                    <span id="lblDays" class="clsLabelAuto">Days</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tabWarranty" runat="server" CssClass="clsPanel1" Visible="<%# mOrder.TransTypeID = Flypal.Util.Trans.PurchaseOrderForExchangeRepair %>">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Warranty Information" ID="Label1"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="pnlWarrantyInformation" Style="width: auto;">
                                            <asp:UpdatePanel runat="server" ID="upnlWarrantyInformation" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset8" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;"
                                                        runat="server" visible="<%# mOrder.TransTypeID = Flypal.Util.Trans.PurchaseOrderForExchangeRepair %>"
                                                        class="clsLabelHeader">
                                                        <%--<legend><b>Warranty Information</b></legend>--%>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblUnderWarranty" class="clsLabelAuto">Under Warranty</span>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsInWarranty" runat="server" Checked="<%# mOrder.OrderItems.CurrentItem.IsInWarranty %>"
                                                                        CssClass="clsLabelAuto" Text="" TextAlign="Left" Enabled ="false"/>                                                                   
                                                                </td>
                                                                <td>
                                                                    <span id="lblWarrantyDays" class="clsLabelAuto">Warranty Days</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWarrantyInDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                        BackColor="#E0E0E0" ReadOnly="True" Text="<%# mOrder.OrderItems.CurrentItem.WarrantyInDays %>"
                                                                        MaxLength="4">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span1" class="clsLabel">Start Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWarrantyStartDate" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        BackColor="#E0E0E0" ReadOnly="True" Text="<%# mOrder.OrderItems.CurrentItem.WarrantyStartDateFormatted %>"
                                                                        Width="100px" AutoPostBack="true" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtWarrantyStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyStartDate">
                                                                    </cc2:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblExpiryDate1" class="clsLabel">Expiry Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWarrantyExpiryDate" runat="server" AutoPostBack="true" BackColor="#E0E0E0"
                                                                        ClientIDMode="Static" CssClass="clsTextBoxTagSearch" ReadOnly="True" Text="<%# mOrder.OrderItems.CurrentItem.WarrantyExpiryDateFormatted  %>"
                                                                        Width="100px" Enabled="false"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtWarrantyExpiryDate_CalendarExtender" runat="server"
                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyExpiryDate">
                                                                    </cc2:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="tabRemark" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Remark/Note" ID="lblRemark1"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="Panel6" Style="width: auto;">
                                            <asp:UpdatePanel runat="server" ID="upnlRemarkNote" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader">
                                                        <%--<legend><b>Remark/Note</b></legend>--%>
                                                        <table>
                                                            <tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="spnRemark" class="clsLabel">
                                                                            <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" CssClass="clsLabelAuto"
                                                                                Display="None" ErrorMessage="Max. Length should be 100." OnServerValidate="CustomValidate"
                                                                                ValidationGroup="1"></asp:CustomValidator>
                                                                            <span id="Span6" class="clsLabelAuto">
                                                                                <asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtNote" CssClass="clsLabelAuto"
                                                                                    Display="None" ErrorMessage="Max. Length should be 150." OnServerValidate="CustomValidate"
                                                                                    ValidationGroup="1"></asp:CustomValidator>
                                                                            </span></span>
                                                                    </td>
                                                                </tr>
                                                                <td>
                                                                    <span id="lblRemark" class="clsLabel">Remark</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1" Height="36px" MaxLength="250"
                                                                        Text="<%# mOrder.OrderItems.CurrentItem.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark."></asp:TextBox>
                                                                    <span id="lblRemarkNote" class="clsLabelAuto">[Display On Print]</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblNote" class="clsLabelAuto">Note</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBox1" Height="36px" MaxLength="250"
                                                                        Text="<%# mOrder.OrderItems.CurrentItem.Note %>" TextMode="MultiLine" ToolTip="Enter Note."></asp:TextBox>
                                                                    <span id="lblNoteNote" class="clsLabelAuto">[For Internal Use]</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                                <cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1" Visible="<%# mOrder.AgainstTypeID  = 2 OR mOrder.AgainstTypeID  = 3 %>">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Selected Quotation" ID="Label2"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="upnlTSNTSOValues" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="fsTSNTSOValues" style="padding: 0px 4px 0px 0px; width: auto;" class="clsLabelHeader"
                                                    runat="server" visible="<%# mOrder.AgainstTypeID  = 2 OR mOrder.AgainstTypeID  = 3 %>">
                                                    <%--<legend><b>Selected Quotation</b></legend>--%>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgQuotaiontionItemList" runat="server" AutoGenerateColumns="False"
                                                                    CellPadding="5" CssClass="clsGridNewStyle" HeaderStyle-Wrap="false" ShowHeaderWhenEmpty="True"
                                                                    PageSize="3" Visible="<%# mOrder.AgainstTypeID  = 2 OR mOrder.AgainstTypeID  = 3 %>"
                                                                    GridLines="Horizontal">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="Id" HeaderText="Id"></asp:BoundField>
                                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr No">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="QuotationDateFormatted" HeaderText="Quotation Date">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="QuotationNo" HeaderText="Quotation No.">
                                                                            <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Qty.">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                    ToolTip="Enter Quantity." Text='<%# DataBinder.Eval(Container.DataItem,"Qty") %>'>
                                                                                </asp:TextBox>
                                                                                <asp:CustomValidator ID="cvReqQty" runat="server" ErrorMessage="Quantity must be greater than Zero."
                                                                                    ControlToValidate="txtReqQty" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:ButtonField CommandName="ForDelete" HeaderText="Remove" Text="Remove" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" ToolTip="Click to Add Store Appoval Item"
                                                                    Enabled="<%# (Not mOrder.OrderItems.CurrentItem.ItemID.Equals(Guid.Empty)) And CType(mOrder.TransTypeID,FlyPal.Util.Trans) = FlyPal.Util.Trans.PurchaseOrder %>"
                                                                    CausesValidation="False" Visible="<%# mOrder.AgainstTypeID  = 3 %>"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc2:TabPanel>
                            </cc2:TabContainer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
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
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
        PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUpload.aspx");
                    //                        $("#IFileUpload").ready(function () {
                    //                            $("#btnDummyFileUpload").click();
                    //                            $get("AjaxLoader").style.visibility = 'hidden';
                    //                        });
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
            //close File Upload popup window
            FileUpwindow.hide();
            //Free resources
            $("#IFileUpload").attr("src", "JavaScript:''");
            if (fileattached) {
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();
            }
        }
    </script>
    <!-- End File Upload Modal Dialog-->
    <script type="text/javascript">
        function OnClickOfScheduleExpensesYes() {
            var statusYes = document.getElementById("chkScheduleExpensesYes");
            var statusNo = document.getElementById("chkScheduleExpensesNo");
            if (statusYes.checked) {
                statusNo.checked = false;
            }
        }
        function OnClickOfScheduleExpensesNo() {
            var statusYes = document.getElementById("chkScheduleExpensesYes");
            var statusNo = document.getElementById("chkScheduleExpensesNo");
            if (statusNo.checked) {
                statusYes.checked = false;
            }
        }
    </script>
            
    </form>
</body>
</html>
