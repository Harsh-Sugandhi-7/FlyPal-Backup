<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOInvoice_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfnWOInvoice_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
    <%--  <script type="text/javascript" src="jquery-1.6.1.min.js"></script>--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <%--  <link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />--%>

    <%-- <style type="text/css">
        .clsFieldSetNewStyle legend {
            font-family:'Inter', sans-serif;
            font-size: 13px;
            color: Black;
            font-weight: 500;
            border-style: solid;
            padding: 2 2 2 2;
            margin: 2 2 2 2;
            width: auto; /*   height: auto; vertical-align:middle;*/
            text-align: left;
            margin-left: 10px;
            background-color: WhiteSmoke;
            border-width: 1.8;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server" method="post" autocomplete="off">
        <script type="text/javascript">

            function WaterMark(txt, evt) {
                var defaultText = "Select your prefix";
                if (txt.value.length == 0 && evt.type == "blur") {
                    txt.style.color = "gray";
                    txt.value = defaultText;
                }
                if (txt.value == defaultText && evt.type == "focus") {
                    txt.style.color = "black";
                    txt.value = "";
                }
            }
            $(document).ready(function () {
                var txt = document.getElementById("<%=txtWOInvoiceText.ClientID%>");
                var defaultText = "Select your prefix";
                if (txt.value.length == 0) {
                    txt.style.color = "gray";
                    txt.value = defaultText;
                }
            });


        </script>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
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
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <span id="lblTitle" class="clsFormHeader"
                                                            runat="server">List Of Job Order(s)</span>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnCancel" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH"
                                                            Text="Cancel" ToolTip="Click to Cancel the WO Invoice" />
                                                        <asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Send Mail" ClientIDMode="Static" ToolTip="Click to Send Invoice by Mail"
                                                            Visible="false"></asp:Button>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Print" ClientIDMode="Static"
                                                            ToolTip="Click to Print Invoice"></asp:Button>
                                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Authorize" ToolTip="Click to authorize WO Invoice" />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Save" CausesValidation="true" ValidationGroup="a" ToolTip="Click to Save WO Invoice" />
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to go back to the previous page" />
                                                        <asp:Button ID="hdnBtnCharge" ClientIDMode="Static" runat="server" Text="----" CausesValidation="false"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnJobItemSelection" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="false" Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvWOInvoiceDate" runat="server" Display="None" ControlToValidate="txtWOInvoiceDate"
                                                ValidationGroup="a" ErrorMessage="Select WOInvoice Date" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCurrency" runat="server" Display="None" ControlToValidate="cmbCurrencyList"
                                                ValidationGroup="a" ErrorMessage="Select Currency from the list." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
                                                ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
                                                ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Select Invoice Date" ControlToValidate="txtWOInvoiceDate"
                                                Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="RemarkValidator" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Lenght of Remark should not greater than 200 Characters"
                                                ControlToValidate="txtRemark" Display="None"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblStatus" runat="server" Text="<%# mWOInvoice.StatusName %>" Style="margin-right: 5px"
                                                            CssClass="clsLabelAuto" Font-Bold="true"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlWOInvoiceDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="60%" style="border-width: 1px; margin-left: 5px" valign="top">
                                                <tr>
                                                    <td>
                                                        <%-- <fieldset class="clsFieldSetNewStyle">
                                                            <legend class="clsFieldSet1">Detail </legend>--%>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblDateStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDate" class="control-label">Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWOInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                        Enabled="false" AutoPostBack="true"
                                                                        onchange="ValidateDateText(this,'txtWOInvoiceDateWatermarkExtender','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtWOInvoiceDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWOInvoiceDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtWOInvoiceDateWatermarkExtender" runat="server"
                                                                        TargetControlID="txtWOInvoiceDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNoStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="control-label">No.</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtWOInvoiceText" runat="server" Text="<%# mWOInvoice.Text %>" CssClass="clsTextBoxTagSearch"
                                                                                    onfocus="WaterMark(this, event);" onblur="WaterMark(this, event);"
                                                                                    ToolTip="Enter No." MaxLength="25"> </asp:TextBox>

                                                                                <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtWOInvoiceText_Autocomplete"
                                                                                    runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="0"
                                                                                    CompletionInterval="1" ServicePath="wfnWOInvoice_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                                                    CompletionSetCount="0" TargetControlID="txtWOInvoiceText" UseContextKey="False"
                                                                                    ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                    CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
                                                                                    OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding" OnClientShown="ClientHiding"
                                                                                    OnClientShowing="ClientShowing">
                                                                                </cc2:AutoCompleteExtender>
                                                                                <script type="text/jscript">
                                                                                    function SetContextKey() {
                                                                                        var autoComplete = $find('txtText_Autocomplete');
                                                                                        var TransTypeID = 'TransTypeID=<%=mWOInvoice.TransTypeID%>¿Date=<%=mWOInvoice.Date%>';
                                                                                        autoComplete.set_contextKey(TransTypeID);
                                                                                    }
                                                                                </script>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtWOInvoiceNo" runat="server" Text="<%# mWOInvoice.No %>" CssClass="clsTextBoxTagSearchSmall" Width="60px" MaxLength="8"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="lblWO" style="width: 200px" class="control-label">Work Order No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtWO" runat="server" CssClass="clsTextBoxTagSearch1" ToolTip="Job Order"
                                                                        MaxLength="25" Text="<%# mWOInvoice.WOTextNo %>" Style="margin-bottom: 10px;"
                                                                        Enabled="false"> </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCurrencyStar" class="control-label clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblCurrency" class="control-label">Currency/Factor</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    Enabled="false" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mWOInvoice.CurrencyID %>"
                                                                                    AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mWOInvoice.ConversionFactor %>" Enabled="false"
                                                                                    Style="margin-left: 5px" CssClass="clsTextBoxTagSearchSmall" Width="70px" ToolTip="Enter Conversion Factor" MaxLength="9"> </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelAuto">Customer
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomer" runat="server" CssClass="clsTextBoxTagSearch1"
                                                                        ToolTip="Customer" MaxLength="25" Text="<%# mWOInvoice.CustomerName %>" Style="margin-bottom: 10px;"
                                                                        Enabled="false"> </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%--    <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBillingLocation" runat="server" CssClass="clsLabelAuto">Billing Location
                                                        </asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbBillingLocation" runat="server" CssClass="input-sm" Enabled="<%# mWOInvoice.StatusID <> 2 %>"
                                                                        AutoPostBack="true" SelectedValue="<%# mWOInvoice.BillingLocationID %>" DataTextField="Location"
                                                                        DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>--%>
                                                        </table>
                                                        <%-- </fieldset>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel ID="upnlWOInvoiceJobsAndSpares" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="99%" style="border-width: 1px; margin-top: 10px; margin-left: 5px">
                                                <tr>
                                                    <td width="50%" style="margin-top:20px;">
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                            <legend class="clsFieldSet1">Invoice Job(s) </legend>
                                                            <asp:UpdatePanel ID="upnlWOInvoiceJobs" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgWOInvoiceJobs" runat="server" DataKeyNames="ID"
                                                                        ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                                        AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">

                                                                        <Columns>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="True"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="CustomerDescription" HeaderText="Job Desc." HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:TemplateField HeaderText="Capability" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:DropDownList ID="cmbCapability" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                        DataValueField="ID" DataSource="<%# mCapabilityTaskList %>" DataTextField="TaskDescriptionLen15" AutoPostBack="true" OnSelectedIndexChanged="TextChanged"
                                                                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem, "CapabilityTaskID") %>'>
                                                                                    </asp:DropDownList>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Man Hrs." HeaderStyle-ForeColor="black" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtManHrs" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        MaxLength="12" Text='<%# DataBinder.Eval(Container.DataItem, "ManHour") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rate" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" Width="60px"
                                                                                        MaxLength="12" AutoPostBack="true" OnTextChanged="TextChanged"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CRate") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>

                                                                            <%--5--%>
                                                                            <asp:BoundField DataField="CAmount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"
                                                                                HeaderStyle-ForeColor="black" />
                                                                            <asp:TemplateField HeaderText="Tax(%)" HeaderStyle-ForeColor="black" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTax" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        AutoPostBack="true" OnTextChanged="TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "TaxPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="TaxCAmount" HeaderText="Tax Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                HeaderStyle-ForeColor="black" />

                                                                            <%--8--%>
                                                                            <asp:TemplateField HeaderText="CGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" 
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "CGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--9--%>
                                                                            <asp:TemplateField HeaderText="CGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtWCGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "CGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--10--%>
                                                                            <asp:TemplateField HeaderText="SGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" 
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "SGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--11--%>
                                                                            <asp:TemplateField HeaderText="SGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtWSGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "SGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--12--%>
                                                                            <asp:TemplateField HeaderText="IGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtIGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" 
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "IGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--13--%>
                                                                            <asp:TemplateField HeaderText="IGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtWIGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "IGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>

                                                                            <%--14--%>
                                                                            <asp:BoundField DataField="TotalCAmount" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Right"
                                                                                HeaderStyle-ForeColor="black" />
                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                                    <td valign="top" width="48%">
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-left: 5px">
                                                            <legend class="clsFieldSet1">
                                                                <table align="top">
                                                                    <tr>
                                                                        <td>Invoice Spare(s)
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImgAddWOInvoiceSpare" runat="server" CausesValidation="true"
                                                                                Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add WO Invoice Spares"
                                                                                Width="24px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </legend>
                                                            <asp:UpdatePanel ID="upnlWOInvoiceSpares" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgWOInvoiceSpares" runat="server" DataKeyNames="ID" ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                                        AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

                                                                        <Columns>
                                                                            <%--0--%>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <%--1--%>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Part No./Description"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblSparesPartNoStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Part No./Description</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="upnlSparesPartNoValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:CustomValidator ID="cvSpare" runat="server" ControlToValidate="txtSparesPartNo"
                                                                                                SetFocusOnError="true" CssClass="clsLabelAuto" Visible="false" ErrorMessage="Enter whole part no. and description"
                                                                                                Font-Italic="true" ForeColor="Red" ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                            <asp:RequiredFieldValidator ID="rfvSpare" runat="server" ControlToValidate="txtSparesPartNo"
                                                                                                CssClass="clsLabelAuto" Display="dynamic" ErrorMessage="Part No. Required" Font-Italic="true"
                                                                                                ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Part No. Required"
                                                                                                ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:Label ID="lblDuplicateSpare" runat="server" ForeColor="Red" class="control-label"
                                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                    <asp:TextBox ID="txtSparesPartNo" runat="server" CssClass="clsTextBoxTagSearch1"
                                                                                        MaxLength="200" AutoPostBack="True" OnTextChanged="txtSparesPartNo_TextChanged"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ItemNameDescription") %>' ToolTip="Enter Part No."
                                                                                        Width="185px"></asp:TextBox>
                                                                                    <cc2:AutoCompleteExtender ID="txtSparesPartNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                                        CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                        Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetPartNoDescriptionList"
                                                                                        UseContextKey="false" ContextKey="" TargetControlID="txtSparesPartNo">
                                                                                    </cc2:AutoCompleteExtender>

                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <%--2--%>
                                                                            <asp:TemplateField HeaderText="Qty." HeaderStyle-ForeColor="black">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblSparesQtyStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                    <span id="Span6">Qty.</span>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:UpdatePanel ID="upnlSparesPartNoQtyValidate" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:RequiredFieldValidator ID="rfvSpareQty" runat="server" ControlToValidate="txtSpareQty"
                                                                                                CssClass="clsLabelAuto" Display="dynamic" ErrorMessage="Qty. Required" Font-Italic="true"
                                                                                                ForeColor="Red" InitialValue="0" SetFocusOnError="true" Text="Qty. Required"
                                                                                                ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:RequiredFieldValidator>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                    <asp:TextBox ID="txtSpareQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        AutoPostBack="true" OnTextChanged="TextChanged" MaxLength="8" onKeyPress="validateText('D');"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--3--%>
                                                                            <asp:BoundField DataField="SourceStoreName" HeaderText="Source" HeaderStyle-HorizontalAlign="Left"
                                                                                Visible="false" HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                            <%--4--%>
                                                                            <asp:TemplateField HeaderText="Rate" HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareRate" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" Width="60px"
                                                                                        MaxLength="12" AutoPostBack="true" OnTextChanged="TextChanged"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CRate") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>

                                                                            <%--5--%>
                                                                            <asp:BoundField DataField="CAmount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right"
                                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Right" />


                                                                            <%--6--%>
                                                                            <asp:TemplateField HeaderText="Tax(%)" HeaderStyle-ForeColor="black" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTax" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        MaxLength="12" AutoPostBack="true" OnTextChanged="TextChanged"
                                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "TaxPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--  <asp:TemplateField HeaderText="Tax Amt." HeaderStyle-ForeColor="black">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtTaxAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                     Text='<%# DataBinder.Eval(Container.DataItem,"TaxCAmount") %>'> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Right" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>--%>

                                                                            <%--7--%>
                                                                            <asp:BoundField DataField="TaxCAmount" HeaderText="Tax Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                HeaderStyle-ForeColor="black" />

                                                                            <%--8--%>
                                                                            <asp:TemplateField HeaderText="CGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareCGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "CGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--9--%>

                                                                            <asp:TemplateField HeaderText="CGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareCGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "CGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--10--%>

                                                                            <asp:TemplateField HeaderText="SGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareSGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "SGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--11--%>
                                                                            <asp:TemplateField HeaderText="SGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareSGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "SGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--12--%>
                                                                            <asp:TemplateField HeaderText="IGST(%)">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareIGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "IGSTPercentage") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <%--13--%>
                                                                            <asp:TemplateField HeaderText="IGST Amt.">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSpareIGSTAmt" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="60px"
                                                                                        ReadOnly="true" BackColor="#E0E0E0" Text='<%# DataBinder.Eval(Container.DataItem, "IGSTCAmount") %>'> </asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>

                                                                            <%--14--%>
                                                                            <asp:BoundField DataField="TotalCAmount" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Right"
                                                                                HeaderStyle-ForeColor="black" />
                                                                            <%--15--%>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderStyle-ForeColor="black">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>


                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <%--    <tr>
                                                <td valign="top" width="50%" align="right">
                                                    <asp:UpdatePanel ID="upnlWOInvoiceJobsTaxTotalAmount" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblJobTaxTotal" runat="server" CssClass="clsLabelAuto"  >Tax Total</asp:Label>
                                                            <asp:TextBox ID="txtWOInvoiceJobsTaxTotalAmount" runat="server" BackColor="#E0E0E0"   Style="margin-top: 5px; font-size: 9pt" 
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ReadOnly="True" Text="<%# mWOInvoice.CTotalJobTax %>"> </asp:TextBox>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td valign="top" width="50%" align="right">
                                                    <asp:UpdatePanel ID="upnlWOInvoiceSparesTaxTotalAmount" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblSpareTaxTotal" runat="server" CssClass="clsLabelAuto"  >Tax Total</asp:Label>
                                                            <asp:TextBox ID="txtWOInvoiceSparesTaxTotalAmount" runat="server" BackColor="#E0E0E0"    Style="margin-top: 5px; font-size: 9pt" 
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ReadOnly="True" Text="<%# mWOInvoice.CTotalSpareTax %>"> </asp:TextBox>
                                                            </td>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>>--%>
                                                <tr>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceJobsTotalAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span1" class="control-label">Jobs Amount Total</span>
                                                                <asp:TextBox ID="txtWOInvoiceJobsTotalAmount" runat="server" BackColor="#E0E0E0"
                                                                    CssClass="clsTextBoxTagSearchRightAlign1" ReadOnly="True" Width="100px"
                                                                    Text="<%# mWOInvoice.CTotalJobAmount %>"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceSparesTotalAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span2" class="control-label">Spares Amount Total</span>
                                                                <asp:TextBox ID="txtWOInvoiceSparesTotalAmount" runat="server" BackColor="#E0E0E0" Width="100px"
                                                                    Style="margin-top: 5px; font-size: 9pt" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                    ReadOnly="True" Text="<%# mWOInvoice.CTotalSpareAmount %>"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceWorkOtherCharges" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                    <legend class="clsFieldSet1">
                                                                        <table>
                                                                            <tr>
                                                                                <td>Other Charges Job(s)
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgAddWOInvoiceJobOtherCharges" runat="server" CausesValidation="true"
                                                                                        Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add WO Invoice Job Other Charges"
                                                                                        Width="24px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </legend>
                                                                    <asp:UpdatePanel ID="upnlWOInvoiceJobOtherCharges" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgWOInvoiceJobOtherCharges" runat="server" DataKeyNames="ID"
                                                                                ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                                                AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                                        HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                                    <%--  <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="black"
                                                                                    ItemStyle-HorizontalAlign="Left" />--%>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Charge" HeaderStyle-ForeColor="black"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblChargeStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                            <span id="Span6">Charge(s)</span>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:UpdatePanel ID="upnlChargeValidate" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:CustomValidator ID="cvCharge" runat="server" ControlToValidate="txtCharge" SetFocusOnError="true"
                                                                                                        CssClass="clsLabelAuto" Visible="false" ErrorMessage="Select Charge" Font-Italic="true"
                                                                                                        ForeColor="Red" ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                                    <asp:RequiredFieldValidator ID="rfvCharge" runat="server" ControlToValidate="txtCharge"
                                                                                                        CssClass="clsLabelAuto" Display="dynamic" ErrorMessage="Charge Required" Font-Italic="true"
                                                                                                        ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Charge Required"
                                                                                                        ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:Label ID="lblDuplicateCharge" runat="server" ForeColor="Red" class="control-label"
                                                                                                Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                            <asp:TextBox ID="txtCharge" runat="server" CssClass="clsTextBoxTagSearch1" MaxLength="200"
                                                                                                CauseValidation="false" Width="100%" AutoPostBack="true"
                                                                                                OnTextChanged="txtCharge_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ChargeName") %>'
                                                                                                ToolTip="Enter Charge" ></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtCharge_Autocomplete"  runat="server" CompletionInterval="1"
                                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                EnableCaching="false" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetChargeList"
                                                                                                UseContextKey="false" ContextKey="Charge" ServicePath="" TargetControlID="txtCharge"
                                                                                                OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                                OnClientShown="ClientHiding" OnClientShowing="ClientShowing" OnClientItemSelected="DoTextChangedPostBackJobCharge" >
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Percentage" HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                ReadOnly='<%# Not (Eval("PercentageTypeID") = 3) %>' MaxLength="12"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>' AutoPostBack="true"
                                                                                                OnTextChanged="txtCharge_TextChanged"> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Charge Amount" HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                ReadOnly='<%# Not (Eval("PercentageTypeID") = 1) %>' MaxLength="12"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "CChargeAmount") %>' AutoPostBack="true"
                                                                                                OnTextChanged="txtCharge_TextChanged"> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-HorizontalAlign="Center"
                                                                                    HeaderStyle-ForeColor="black">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="EditCharge" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                            CommandName="EditCharge" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>--%>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-HorizontalAlign="Center"
                                                                                        HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="DeleteCharge" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                CommandName="DeleteCharge" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="PercentageTypeID" HeaderText="PercentageTypeID"></asp:BoundField>
                                                                                </Columns>
                                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td valign="top">
                                                        <%--Spare charges--%>
                                                        <asp:UpdatePanel ID="upnlWOInvoiceSpareOtherCharges1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-left: 5px">
                                                                    <legend class="clsFieldSet1">
                                                                        <table>
                                                                            <tr>
                                                                                <td>Other Charges Spare(s)
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgAddWOInvoiceSpareOtherCharges" runat="server" CausesValidation="true"
                                                                                        Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to add WO Invoice Spare Other Charges"
                                                                                        Width="24px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </legend>
                                                                    <asp:UpdatePanel ID="upnlWOInvoiceSpareOtherCharges" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgWOInvoiceSpareOtherCharges" runat="server" DataKeyNames="ID"
                                                                                ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                                                AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

                                                                                <Columns>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                                        HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left" />
                                                                                    <%--  <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="black"
                                                                                    ItemStyle-HorizontalAlign="Left" />--%>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Charge" HeaderStyle-ForeColor="black"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblChargeStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                                            <span id="Span6">Charge(s)</span>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:UpdatePanel ID="upnlSpareChargeValidate" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:CustomValidator ID="cvSpareCharge" runat="server" ControlToValidate="txtSpareCharge"
                                                                                                        SetFocusOnError="true" CssClass="clsLabelAuto" Visible="false" ErrorMessage="Select Charge"
                                                                                                        Font-Italic="true" ForeColor="Red" ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                                                    <asp:RequiredFieldValidator ID="rfvSpareCharge" runat="server" ControlToValidate="txtSpareCharge"
                                                                                                        CssClass="clsLabelAuto" Display="dynamic" ErrorMessage="Charge Required" Font-Italic="true"
                                                                                                        ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Charge Required"
                                                                                                        ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:Label ID="lblDuplicateSpareCharge" runat="server" ForeColor="Red" class="control-label"
                                                                                                Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                                            <asp:TextBox ID="txtSpareCharge" runat="server" CssClass="clsTextBoxTagSearch1"
                                                                                                Width="100%" MaxLength="200" AutoPostBack="true" OnTextChanged="txtSpareCharge_TextChanged"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "ChargeName") %>' ToolTip="Enter Charge"></asp:TextBox>
                                                                                            <cc2:AutoCompleteExtender ID="txtSCharge_Autocomplete" runat="server" CompletionInterval="1"
                                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                                EnableCaching="false" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetChargeList"
                                                                                                UseContextKey="false" ContextKey="Charge" ServicePath="" TargetControlID="txtSpareCharge"
                                                                                                OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                                OnClientShown="ClientHiding" OnClientShowing="ClientShowing" OnClientItemSelected="DoTextChangedPostBackSpareCharge" >
                                                                                            </cc2:AutoCompleteExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Percentage" HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                ReadOnly='<%# Not (Eval("PercentageTypeID") = 3) %>' MaxLength="12"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "Percentage") %>' AutoPostBack="true"
                                                                                                OnTextChanged="txtSpareCharge_TextChanged"> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Charge Amount" HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtSpareChargeAmount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                                ReadOnly='<%# Not (Eval("PercentageTypeID") = 1) %>' MaxLength="12"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "CChargeAmount") %>' AutoPostBack="true"
                                                                                                OnTextChanged="txtSpareCharge_TextChanged"> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-HorizontalAlign="Center"
                                                                                        HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="DeleteCharge" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                CommandName="DeleteCharge" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="PercentageTypeID" HeaderText="PercentageTypeID"></asp:BoundField>
                                                                                </Columns>
                                                                                <SelectedRowStyle BackColor="ControlDark" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceJobOtherChargesAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span5" class="clsLabel">Total Charge(s)</span>
                                                                <asp:TextBox ID="txtWOInvoiceJobOtherChargesTotalAmount" runat="server" BackColor="#E0E0E0"
                                                                    Style="margin-top: 5px" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ReadOnly="True" Width="100px" Text="<%# mWOInvoice.CTotalJobCharges %>"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceSparesOtherChargesAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span6" class="clsLabel">Total Charge(s)</span>
                                                                <asp:TextBox ID="txtWOInvoiceSparesOtherChargesTotalAmount" runat="server" BackColor="#E0E0E0"
                                                                    Style="margin-top: 5px" Width="100px" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ReadOnly="True" Text="<%# mWOInvoice.CTotalSpareCharges %>"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceTotalJobEstimationAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span7" class="control-label" style="font-size: 9pt">Total Job Cost</span>
                                                                <asp:TextBox ID="txtTotalJobEstimation" runat="server" Style="font-weight: bold; margin-top: 5px; font-size: 9pt"
                                                                    BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    Width="100px" ReadOnly="True"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:UpdatePanel ID="upnlWOInvoiceTotalspareEstimationAmount" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span8" class="control-label" style="font-size: 9pt">Total Spares Cost</span>
                                                                <asp:TextBox ID="txtTotalSparesEstimation" runat="server" Style="font-weight: bold; margin-top: 5px; font-size: 9pt"
                                                                    BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    Width="100px" ReadOnly="True"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:UpdatePanel ID="upnlGrandTotal" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <span id="Span9" class="control-label" style="font-size: 10pt">Grand Total</span>
                                                                <asp:TextBox ID="txtGrandTotal" runat="server" BackColor="#E0E0E0" Style="font-weight: bold; margin-top: 5px; font-size: 10pt"
                                                                    CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ReadOnly="True" Width="100px" Text="<%# mWOInvoice.CGrandTotal %>"> </asp:TextBox>
                                                                </td>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlWOInvoiceTerms" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-left: 5px">
                                                <legend class="clsFieldSet1">
                                                    <table>
                                                        <tr>
                                                            <td>Invoice Term(s)
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="ImgWOInvoiceTerms" runat="server" CausesValidation="true" Height="22px"
                                                                    ImageUrl="~/images/plus1.png" ToolTip="Click To Add Term" Width="24px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </legend>
                                                <table width="100%">
                                                    <asp:GridView ID="dgWOInvoiceTerms" runat="server" DataKeyNames="ID" ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
                                                        AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left">
                                                                <HeaderStyle Width="20px" />
                                                                <ItemStyle Width="20px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-ForeColor="black" ItemStyle-HorizontalAlign="Left">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblTermStar" runat="server" Visible="false" class="clsLabelStar">*</asp:Label>
                                                                    <span id="Span6">Terms and Conditions</span>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:UpdatePanel ID="upnlTermValidate" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:CustomValidator ID="cvTerm" runat="server" ControlToValidate="txtTerm" SetFocusOnError="true"
                                                                                CssClass="clsLabelAuto" Visible="false" ErrorMessage="Select Term" Font-Italic="true"
                                                                                ForeColor="Red" ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'></asp:CustomValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvTerm" runat="server" ControlToValidate="txtTerm"
                                                                                CssClass="clsLabelAuto" Display="dynamic" ErrorMessage="Term Required" Font-Italic="true"
                                                                                ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Term Required"
                                                                                ValidationGroup='<%#String.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:Label ID="lblDuplicateTerm" runat="server" ForeColor="Red" class="control-label"
                                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                                    <asp:TextBox ID="txtTerm" runat="server" CssClass="clsTextBoxTagSearch"  height="25px"
                                                                        Width="100%" MaxLength="200" AutoPostBack="true" OnTextChanged="txtTerm_TextChanged"
                                                                        Text='<%# DataBinder.Eval(Container.DataItem, "WOTerm") %>' ToolTip="Enter Term"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ID="txtTerm_Autocomplete" runat="server" CompletionInterval="1"
                                                                        CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                        CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                        EnableCaching="true" Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetTermList"
                                                                        UseContextKey="True" ContextKey="Term" ServicePath="" TargetControlID="txtTerm">
                                                                    </cc2:AutoCompleteExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-ForeColor="black" ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="DeleteTerm" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                                        CommandName="DeleteTerm" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                    </asp:GridView>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 100%; margin-top: 5px; margin-left: 5px">
                                                <tr>
                                                    <td>
                                                        <span id="Span10" class="clsLabelHeader">Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultilineOpening_Ajax"
                                                            TextMode="MultiLine" Enabled="<%# mWOInvoice.StatusID <> 2 %>" Text="<%# mWOInvoice.Remark %>"
                                                            MaxLength="200" ToolTip="Enter Remark"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
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
        <!--Duplicates Highlight -->
        <script type="text/javascript">
            function CheckDuplicateSpares(sender, args) {
                var grid = document.getElementById("<%=dgWOInvoiceSpares.ClientID %>");
                var inputs = $('#<%=dgWOInvoiceSpares.ClientID %>').find('input[id$="txtSparesPartNo"]');
                var span = $('#<%=dgWOInvoiceSpares.ClientID %>').find('span[id$="lblDuplicateSpare"]');

                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].style.backgroundColor = "";
                    span[i].style.display = 'none';
                }
                for (var i = 0; i < inputs.length; i++) {
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {
                            inputs[i].style.backgroundColor = "Orchid";
                            inputs[j].style.backgroundColor = "Orchid";
                            span[i].style.display = 'block';
                            span[j].style.display = 'block';
                        }
                    }
                }
            }
            function CheckDuplicateTerms(sender, args) {
                var grid = document.getElementById("<%=dgWOInvoiceTerms.ClientID %>");
                var inputs = $('#<%=dgWOInvoiceTerms.ClientID %>').find('input[id$="txtTerm"]');
                var span = $('#<%=dgWOInvoiceTerms.ClientID %>').find('span[id$="lblDuplicateTerm"]');


                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].style.backgroundColor = "";
                    span[i].style.display = 'none';
                }
                for (var i = 0; i < inputs.length; i++) {
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {
                            inputs[i].style.backgroundColor = "Orchid";
                            inputs[j].style.backgroundColor = "Orchid";
                            span[i].style.display = 'block';
                            span[j].style.display = 'block';

                        }
                    }
                }
            }
            function CheckDuplicateCharges(sender, args) {
                var grid = document.getElementById("<%=dgWOInvoiceJobOtherCharges.ClientID %>");
                var inputs = $('#<%=dgWOInvoiceJobOtherCharges.ClientID %>').find('input[id$="txtCharge"]');
                var span = $('#<%=dgWOInvoiceJobOtherCharges.ClientID %>').find('span[id$="lblDuplicateCharge"]');
                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].style.backgroundColor = "";
                    span[i].style.display = 'none';
                }
                for (var i = 0; i < inputs.length; i++) {
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {
                            inputs[i].style.backgroundColor = "Orchid";
                            inputs[j].style.backgroundColor = "Orchid";
                            span[i].style.display = 'block';
                            span[j].style.display = 'block';

                        }
                    }
                }
            }
            function CheckDuplicateSpareCharges(sender, args) {
                var grid = document.getElementById("<%=dgWOInvoiceSpareOtherCharges.ClientID %>");
                var inputs = $('#<%=dgWOInvoiceSpareOtherCharges.ClientID %>').find('input[id$="txtSpareCharge"]');
                var span = $('#<%=dgWOInvoiceSpareOtherCharges.ClientID %>').find('span[id$="lblDuplicateSpareCharge"]');
                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].style.backgroundColor = "";
                    span[i].style.display = 'none';
                }
                for (var i = 0; i < inputs.length; i++) {
                    for (var j = 0; j < inputs.length; j++) {
                        if (inputs[i] != inputs[j] && (inputs[i].value != "" || inputs[j].value != "") && inputs[i].value == inputs[j].value) {
                            inputs[i].style.backgroundColor = "Orchid";
                            inputs[j].style.backgroundColor = "Orchid";
                            span[i].style.display = 'block';
                            span[j].style.display = 'block';

                        }
                    }
                }
            }
        </script>
        <script type="text/javascript" >
            function DoTextChangedPostBackJobCharge() {
                __doPostBack("txtCharge", "TextChanged");
            }
            function DoTextChangedPostBackSpareCharge() {
                __doPostBack("txtSpareCharge", "TextChanged");
            }
        </script>
    </form>
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
</body>
</html>
