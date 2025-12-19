<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceiptCumInvoice_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfReceiptCumInvoice_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Goods Receipt Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        //Sankalp 25-08-25
        function OpenFileUploadWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                return false;
            } catch (e) {
                alert(e);
            }
        }
    </script>
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
        <div>
            <table class="clstablelistout" id="tblMain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Goods Receipt Details [ New ]</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                                    ValidationGroup="a" ErrorMessage="ReceiptCumInvoice Date Required." ControlToValidate="txtReceiptCumInvoiceDate"
                                                    Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvWorkShop" runat="server" OnServerValidate="CustomValidate"
                                                    Display="None" ErrorMessage="Please Select WorkShop." ControlToValidate="cmbWorkShop"
                                                    ValidationGroup="a" CssClass="clsValidationSummary"></asp:CustomValidator><asp:RequiredFieldValidator
                                                        ID="rfvDate" runat="server" Display="None" ErrorMessage="ReceiptCumInvoice Date Required."
                                                        ValidationGroup="a" ControlToValidate="txtReceiptCumInvoiceDate" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                    ValidationGroup="a" Display="None" ControlToValidate="cmbVendor" ErrorMessage="Please Select Vendor."
                                                    CssClass="clsValidationSummary"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvCurrency" runat="server" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                    Display="None" ErrorMessage="Please Select Currency." ControlToValidate="cmbCurrency"
                                                    CssClass="clsValidationSummary"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtFactor"
                                                    ValidationGroup="a" ErrorMessage="Currency factor must be greater than zero."
                                                    CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                                <%--     <asp:CustomValidator ID="cvCustomer" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbCustomer" ErrorMessage="Select Customer from the list."
                                                CssClass="clsValidationSummary"></asp:CustomValidator>--%>
                                                <script type="text/javascript">
                                                    function ValidateVendor(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("cmbVendor");
                                                        if (dd.selectedIndex != 0) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                    function ValidateCurrency(source, args) {
                                                        args.IsValid = false;
                                                        var dd = $get("cmbCurrency");
                                                        if (dd.selectedIndex != 0) {
                                                            args.IsValid = true;
                                                            return;
                                                        }
                                                    }
                                                </script>
                                                <asp:CustomValidator ID="cvAircraft" runat="server" ControlToValidate="cmbAircraft"
                                                    CssClass="clsValidationSummary" Display="None" ErrorMessage="Please Select Aircraft."
                                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvStore" runat="server" ControlToValidate="cmbStore" CssClass="clsValidationSummary"
                                                    Display="None" ErrorMessage="Please Select Store." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvWorkOrder" runat="server" ControlToValidate="cmbWorkOrder"
                                                    CssClass="clsValidationSummaryNew" Display="None" ErrorMessage="Select Work Order from the list"
                                                    OnServerValidate="customvalidate"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text="<%# mReceiptCumInvoice.StatusName %>"
                                                    CssClass="clsLabelHeader">
                                                </asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlReceiptCumInvoiceDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblReceiptCumInvoiceDetails" runat="server" CssClass="clsLabelHeader">Goods Receipt Details</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarDate" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblDate" class="clsLabel">Date</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtReceiptCumInvoiceDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                                AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                Text="" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtReceiptCumInvoiceDate_CalendarExtender" runat="server"
                                                                CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReceiptCumInvoiceDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="txtReceiptCumInvoiceDateWatermarkExtender" runat="server"
                                                                TargetControlID="txtReceiptCumInvoiceDate" WatermarkCssClass="clsDateTextBox"
                                                                WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <%--<span id="lblNo" class="clsLabel">Invoice No.</span> Commented by Shital on 23-feb-2021--%>
                                                            <span id="lblNo" class="clsLabel">RCI No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInvoiceText" runat="server" Text="<%# mReceiptCumInvoice.InvText %>"
                                                                CssClass="clsTextBoxTagSearch" onfocus="SetContextKey();" ToolTip="Enter No." MaxLength="25"
                                                                Width="208px"> </asp:TextBox>
                                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtInvoiceText_Autocomplete"
                                                                runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfReceiptCumInvoice_Ajax.aspx"
                                                                ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtInvoiceText"
                                                                UseContextKey="False">
                                                            </cc2:AutoCompleteExtender>
                                                            <script>
                                                                function SetContextKey() {
                                                                    var autoComplete = $find('txtInvoiceText_Autocomplete');
                                                                    var TransTypeID = 'TransTypeID=<%=mReceiptCumInvoice.TransTypeID%>¿QuotationDate=<%=mReceiptCumInvoice.RecCumInvDate%>';
                                                                    autoComplete.set_contextKey(TransTypeID);
                                                                }
                                                            </script>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInvoiceNo" runat="server" Text="<%# mReceiptCumInvoice.InvNo %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8" ToolTip="Enter Goods Receipt No."> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblInternalReceiptNo" runat="server" CssClass="clsLabelAuto">Int. Recpt. No.</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtInternalReceiptNo" runat="server" Text="<%# mReceiptCumInvoice.IntReceiptNo %>"
                                                                CssClass="clsTextBoxTagSearch" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" MaxLength="50"
                                                                ToolTip="Enter Internal Receipt No.">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" 
																				value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsbtnH clsinfoH1" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server"
																				CssClass="clsbtnH clsinfoH1"
																				ToolTip="Remove the Attachment added."
																				Text="Remove Attachment"
																				Enabled="False" Width="140px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td colspan="3">
                                                            <div>
                                                                <asp:CheckBox ID="ChkIsReturnFromOHRepair" runat="server" Checked="<%# mReceiptCumInvoice.IsReturnFromOHRepair %>"
                                                                    CssClass="clsCheckBox" Enabled="<%#mReceiptCumInvoice.StatusID = 1%>" Text="Return From Overhaul/Repair"
                                                                    Visible="<%# mReceiptCumInvoice.TransTypeID=67 %>" TextAlign="Left" />
                                                                &nbsp;<a id="OpenHelp" onclick="OpenHelp()" style="font-weight: bold; cursor: pointer"
                                                                    title="Click to view help" class="clsHyperlink1" href="" runat="server" visible="<%# mReceiptCumInvoice.TransTypeID=67 %>">
                                                                (?) </a>
                                                            </div>
                                                            <!-- Info panel to be displayed as a Help when the checkbox is clicked -->
                                                            <div id="info" style="display: none; width: 350px; height: 130px; z-index: 2;" class="clsLoadMessage">
                                                                <div id="btnCloseParent" style="float: right; opacity: 0; filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);">
                                                                    <asp:LinkButton ID="btnClose" runat="server" OnClientClick="CloseHelp();return false;"
                                                                        Text="X" ToolTip="Close" Style="background-color: #666666; color: #FFFFFF; text-align: center; font-weight: bold; text-decoration: none; border: outset thin #FFFFFF; padding: 5px;" />
                                                                </div>
                                                                <div>
                                                                    <p>
                                                                        <b><u>Note:</u></b>
                                                                        <br />
                                                                        RCI marked as 'Return from Overhaul/Repair' allows to receive OH/Repair Item(s)
                                                                    without generating P.O.<br />
                                                                        If 'Return from Overhaul/Repair' checkbox is checked then each item 'Rate' will
                                                                    be considered in GRO Expense Calculation.
                                                                    </p>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <cc2:AnimationExtender ID="OpenAnimation" runat="server" TargetControlID="ChkIsReturnFromOHRepair">
                                                                <Animations>
                <OnMouseOver>
                    <Sequence>
                      <%-- Move the info panel on top of the wire frame, fade it in, and hide the frame --%>
                            <ScriptAction Script="Cover($get('ChkIsReturnFromOHRepair'), $get('info'),true);" />
                            <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                         <Parallel Duration="0.2" AnimationTarget="info" Fps="20">
                             <FadeIn Duration="0.2"/>
                              <Scale ScaleFactor="0.8" Center="true" FontUnit="px" />
                              <Resize Height="130" />
                              <StyleAction Attribute="overflow" Value="hidden"/>
                          </Parallel>
                         <%-- Flash the text/border red and fade in the "close" button --%>
                        <Parallel AnimationTarget="info" Duration=".3">
                          <Color PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                            <Color PropertyKey="borderColor" StartValue="#666666" EndValue="#000000" />
                            <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />
                        </Parallel>
                       <Parallel AnimationTarget="info" Duration=".3">
                        <Color PropertyKey="borderColor" StartValue="#000000" EndValue="#666666" />
                       </Parallel>
                    </Sequence>
                </OnMouseOver>
                <OnMouseOut>
                    <Sequence AnimationTarget="info">
                        <%--  Shrink the info panel out of view --%>
                        <StyleAction Attribute="overflow" Value="hidden"/>
                        <Parallel Duration=".2" Fps="15">
                            <Scale ScaleFactor="0.8" Center="true" />
                            <FadeOut />
                        </Parallel>
                        
                        <%--  Reset the sample so it can be played again --%>
                            <StyleAction Attribute="display" Value="none"/>
                            <StyleAction Attribute="width" Value="350px"/>
                            <StyleAction Attribute="height" Value="130px"/>
                        <%--<StyleAction Attribute="fontSize" Value="12px"/>--%>
                            <OpacityAction AnimationTarget="btnCloseParent" Opacity="0" />
                        
                        <%--  Enable the button so it can be played again --%>
                       <%-- <EnableAction AnimationTarget="ChkIsReturnFromOHRepair" Enabled="true" />--%>
                    </Sequence>
                </OnMouseOut>
                                                                </Animations>
                                                            </cc2:AnimationExtender>
                                                            <cc2:AnimationExtender ID="CloseAnimation" runat="server" TargetControlID="btnClose">
                                                                <Animations>
                <OnMouseOver>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FFFFFF" EndValue="#FF0000" />
                </OnMouseOver>
                <OnMouseOut>
                    <Color Duration=".2" PropertyKey="color" StartValue="#FF0000" EndValue="#FFFFFF" />
                </OnMouseOut>
                                                                </Animations>
                                                            </cc2:AnimationExtender>
                                                            <script type="text/javascript" language="javascript">
                                                                // Move an element directly on top of another element (and optionally
                                                                // make it the same size)
                                                                function Cover(bottom, top, ignoreSize) {
                                                                    var location = Sys.UI.DomElement.getLocation(bottom);
                                                                    top.style.position = 'absolute';
                                                                    top.style.top = location.y + 10 + 'px';
                                                                    top.style.left = location.x + 'px';
                                                                    if (!ignoreSize) {
                                                                        top.style.height = bottom.offsetHeight + 'px';
                                                                        top.style.width = bottom.offsetWidth + 'px';
                                                                    }
                                                                }
                                                            </script>
                                                            <script type="text/javascript">
                                                                function OpenHelp() {
                                                                    $find('OpenAnimation')._onMouseOver.play();
                                                                    return false;
                                                                }
                                                                function CloseHelp() {
                                                                    $find('OpenAnimation')._onMouseOut.play();
                                                                    return false;
                                                                }
                                                            </script>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBarcodeNo" runat="server" CssClass="clsLabelAuto" Visible="false">Barcode No.</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                ReadOnly="True" Text="<%# mReceiptCumInvoice.BarcodeNo %>" Visible="False">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblReturnInDays" runat="server" CssClass="clsLabel" Visible="<%# mReceiptCumInvoice.TransTypeID=10 %>">Return In</asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtReturnInDays" runat="server" CssClass="clsTextBoxTagSearch"
                                                                Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" MaxLength="3" Text="<%# mReceiptCumInvoice.ReturnInDays %>"
                                                                ToolTip="Enter Return Days" Visible="<%# mReceiptCumInvoice.TransTypeID=10 %>">
                                                            </asp:TextBox>
                                                            <asp:Label ID="lblDays" runat="server" CssClass="clsLabel" Width="48px" Visible="<%# mReceiptCumInvoice.TransTypeID=10 %>">Days</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlReceivedFrom" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Label ID="lblReceivedFrom1" runat="server" CssClass="clsLabelHeader">Received From</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblReceivedFrom" class="clsLabel">Received From</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:DropDownList ID="cmbReceivedFrom" runat="server" CssClass="clsTextBoxTagSearchComboSmall" Width="203px"
                                                                SelectedValue="<%# mReceiptCumInvoice.FromTypeID %>" DataTextField="Type" DataValueField="ID"
                                                                Enabled="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStarDetails" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSelectDetails" runat="server" CssClass="clsLabel">Select Details</asp:Label>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="455px" SelectedValue="<%# mReceiptCumInvoice.VendorID %>"
                                                                DataTextField="Name" DataValueField="ID" Enabled="<%# mReceiptCumInvoice.IsNew and (mReceiptCumInvoice.TransTypeID = 7 Or mReceiptCumInvoice.TransTypeID = 10) %>"
                                                                Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="455px"
                                                                SelectedValue="<%# mReceiptCumInvoice.AircraftID %>" DataTextField="RegNo" DataValueField="ID"
                                                                Enabled="<%# mReceiptCumInvoice.IsNew and (mReceiptCumInvoice.TransTypeID = 9 Or mReceiptCumInvoice.TransTypeID = 13) %>"
                                                                Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchCombo" SelectedValue="<%# mReceiptCumInvoice.StoreID %>" Width="455px"
                                                                DataTextField="Name" DataValueField="ID" Enabled="<%# mReceiptCumInvoice.IsNew and (mReceiptCumInvoice.TransTypeID = 8 Or mReceiptCumInvoice.TransTypeID = 11 Or mReceiptCumInvoice.TransTypeID = 12 Or mReceiptCumInvoice.TransTypeID = 18 ) %>"
                                                                Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbWorkShop" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="455px"
                                                                SelectedValue="<%# mReceiptCumInvoice.WorkShopID %>" DataTextField="LocationWorkShop"
                                                                DataValueField="ID" Enabled="<%# mReceiptCumInvoice.IsNew and (mReceiptCumInvoice.TransTypeID = 46 Or mReceiptCumInvoice.TransTypeID = 47 Or mReceiptCumInvoice.TransTypeID = 73) %>"
                                                                Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbWorkOrder" runat="server" CssClass="clsTextBoxTagSearchCombo" Width="455px"
                                                                SelectedValue="<%# mReceiptCumInvoice.WOID %>" DataTextField="WONumber" DataValueField="ID"
                                                                Enabled="<%# mReceiptCumInvoice.IsNew %>" Visible="False" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblDCNo" class="clsLabelAuto">D.C.No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDCNo" runat="server" Text="<%# mReceiptCumInvoice.DCNO %>" CssClass="clsTextBoxTagSearch"
                                                                Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" MaxLength="25" ToolTip="Enter D.C.No.">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblDCDate" class="clsLabel">D.C.Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDCDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                Text="<%# mReceiptCumInvoice.DCDateFormatted %>"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtDCDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDCDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDCDate" ID="txtDCDateTextBoxWatermarkExtender"
                                                                runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblVendorInvNo" runat="server" CssClass="clsLabelAuto">Supplier Inv. No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtVendorInvNo" runat="server" Text="<%# mReceiptCumInvoice.VendorInvoiceNo %>"
                                                                CssClass="clsTextBoxTagSearch" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" MaxLength="25"
                                                                ToolTip="Enter Supplier Invoice no.">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblInvDate" class="clsLabelAuto">Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtVendorInvDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                                Text="<%# mReceiptCumInvoice.VendorInvoiceDateFormatted %>"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtVendorInvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtVendorInvDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtVendorInvDate" ID="txtVendorInvDateWatermarkExtender"
                                                                runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblCurrencyStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCurrency" class="clsLabel">Currency</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCurrency" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                                Width="203px" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>" DataTextField="Name"
                                                                DataValueField="ID" SelectedValue="<%# mReceiptCumInvoice.CurrencyID %>" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span id="lblStarFactor" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblConvFactor" class="clsLabelauto">Factor</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFactor" runat="server" Text="<%# mReceiptCumInvoice.ConversionFactor %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
                                                                MaxLength="9" ToolTip="Enter Conversion Factor">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblAWBNo" class="clsLabelAuto">Custom Bill of Entry</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
                                                                MaxLength="50" Text="<%# mReceiptCumInvoice.AWBNo %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblRoundOffRequire" class="clsLabel">Round Off Required</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkIsRoundOff" runat="server" AutoPostBack="True" Checked="<%# mReceiptCumInvoice.IsRoundOff %>"
                                                                CssClass="clsLabelAuto" TextAlign="Right" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabel" Visible="<%# mReceiptCumInvoice.TransTypeID=50 %>">Reg. No.</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# mReceiptCumInvoice.StatusID = 1 %>"
                                                                MaxLength="25" Text="<%# mReceiptCumInvoice.RegNo %>" ToolTip="Enter RegNo."
                                                                Visible="<%# mReceiptCumInvoice.TransTypeID=50 %>">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlReceiptCumInvItems" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblReceiptCumInvItemCaption" class="clsLabelHeader">Receiving Part Detail(s):</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnAddItem" runat="server" class="clsbtnH clsinfoH1" Height="30px" Text="Add" ToolTip="Click To Add New Part."
                                                                            ValidationGroup="a"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgReceiptCumInvoiceItem" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                                AutoGenerateColumns="False" CellPadding="3" ForeColor="Black" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="View">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandName="ViewRec"
                                                                                CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' ImageUrl="icons/CLIP01.ICO"
                                                                                Text="" Height="20px" Width="20px" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="20px" Height="20px" />
                                                                    </asp:TemplateField>
                                                                    <%-- 0--%>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--1--%>
                                                                    <asp:BoundField DataField="ItemName" HeaderText="Part #">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                        <FooterStyle Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <%--2--%>
                                                                    <asp:BoundField DataField="ItemDescription" HeaderText="Description">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--3--%>
                                                                    <asp:BoundField DataField="ItemTypeName" HeaderText="Part Status">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <%--4--%>
                                                                    <asp:BoundField DataField="OrderIssueInfo" HeaderText="Order Info" HtmlEncode="false">
                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--5--%>
                                                                    <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--6--%>

                                                                    <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--7--%>

                                                                    <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--8--%>
                                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No">
                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--9--%>
                                                                    <asp:BoundField DataField="StoreLocInfo" HeaderText="Store Info">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--10--%>
                                                                    <asp:BoundField DataField="Location" HeaderText="Location" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--11--%>
                                                                    <asp:BoundField DataField="CureQtrDateInfo" HeaderText="Cure Info">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--12--%>
                                                                    <asp:BoundField DataField="ExpiryQtrDateInfo" HeaderText="Expiry Info">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--13--%>
                                                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--14--%>

                                                                    <asp:BoundField DataField="DisplayCRate" HeaderText="Rate">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--15--%>

                                                                    <asp:BoundField DataField="DisplayCEffRate" HeaderText="Effective Rate">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--16--%>
                                                                    <asp:BoundField DataField="GROCRate" HeaderText="GRO Rate">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <%--17--%>
                                                                    <asp:BoundField DataField="GROCEffRate" HeaderText="GRO Effe Rate">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <%--18--%>

                                                                    <asp:BoundField DataField="DisplayCCommercialRate" HeaderText="Commercial Rate">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--19--%>
                                                                    <asp:BoundField DataField="COtherCharges" HeaderText="Other Charge">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--20--%>

                                                                    <asp:BoundField DataField="DisplayCAmount" HeaderText="Amt.">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--21--%>
                                                                    <%-- <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%> <%--22--%>
                                                                    <%--  <asp:BoundField DataField="Note" HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%> <%--23--%>
                                                                    <asp:BoundField DataField="CodeNo" HeaderText="Code No.">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--22--%>
                                                                    <asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
                                                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--23--%>
                                                                    <asp:TemplateField HeaderText="CGST Per.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                                OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"CGSTPercentage") %>'></asp:TextBox>
                                                                            <asp:CustomValidator ID="cvCGSTPer" runat="server" ControlToValidate="txtCGSTPer"
                                                                                Display="None"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%--24--%>

                                                                    <asp:BoundField DataField="DisplayCGSTCAmount" HeaderText="CGST Amount">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--25--%>
                                                                    <asp:TemplateField HeaderText="SGST Per.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtSGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                                OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"SGSTPercentage") %>'
                                                                                Enabled="false"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%--26--%>

                                                                    <asp:BoundField DataField="DisplaySGSTCAmount" HeaderText="SGST Amount">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--27--%>
                                                                    <asp:TemplateField HeaderText="IGST Per.">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtIGSTPer" runat="server" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                                                OnTextChanged="TextChanged" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem,"IGSTPercentage") %>'></asp:TextBox>
                                                                            <asp:CustomValidator ID="cvIGSTPer" runat="server" ControlToValidate="txtIGSTPer"
                                                                                Display="None"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <%--28--%>

                                                                    <asp:BoundField DataField="IGSTCAmount" HeaderText="IGST Amount">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <%--29--%>
                                                                    <%--<asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%> <%--32--%>
                                                                    <%--<asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%> <%--33--%>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server" CommandName="EditView" Style="height: 15px; width: 15px"
                                                                                                    ImageUrl="~/images/edit.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRecord" Style="height: 20px; width: 20px"
                                                                                                    ImageUrl="~/images/delete.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                    Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <%--30--%>
                                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                    <%--31--%>
                                                                    <asp:ButtonField CommandName="Attach" HeaderText="Attach" Text="Attach" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <%--32--%>

                                                                    <asp:ButtonField CommandName="Remove" HeaderText="Remove Attachment" Text="Remove" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:ButtonField>
                                                                    <%--33--%>
                                                                </Columns>
                                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:UpdatePanel runat="server" ID="upnlRCICharges" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblRCIChargeCaption" class="clsLabelHeader">Other Charge(s):</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnAddCharge" runat="server" class="clsbtnH clsinfoH1" Height="30px" Text="Add"
                                                                            ToolTip="Click To Add Other Charge."></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnDocketCharge" runat="server" CssClass="clsbtnH clsinfoH1" Text="Docket Charge"
                                                                            ToolTip="Click To Add Docket Charge." Visible="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="dgReceiptCumInvoiceCharge" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" CellPadding="5" ForeColor="Black"
                                                                GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                    <asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
                                                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <%--  <asp:ButtonField CommandName="EditCharge" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteCharge" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <div class="dropdown">
                                                                                <div class="dropdownbtn-content">
                                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="EditView" runat="server" CommandName="EditCharge" Style="height: 15px; width: 15px"
                                                                                                    ImageUrl="~/images/edit.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteCharge" Style="height: 20px; width: 20px"
                                                                                                    ImageUrl="~/images/delete.png" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                    Style="cursor: pointer" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                     <table>
                                            <%-- Sankalp 25-08-25 --%>
                                            <tr align="right">
                                                <td colspan="1" valign="top">
                                                    <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                        <legend class="clsFieldSet1"><b>File Attachments</b></legend>
                                                        <asp:UpdatePanel ID="upnlItemAttachment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="height: 15px">
                                                                            <asp:UpdatePanel ID="upnldgItemAttachment" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="dgItemAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                                        AllowPaging="False" AutoGenerateColumns="false">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <Columns>
                                                                                            <%-- 0 --%>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <%-- 1 --%>
                                                                                            <asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
                                                                                            <%-- 2 --%>
                                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                                                <HeaderStyle Width="10px"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <%-- 3 --%>
                                                                                            <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <%-- 4 --%>
                                                                                            <asp:TemplateField HeaderText="File Name">
                                                                                                <HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtFileName" runat="server" 
																										CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                                                                        ClientIDMode="Static" 
																										ToolTip="Enter File Name To Be Attached"
																										Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
                                                                                                        Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <%-- 5 --%>
                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <%-- <span id="button">Login</span>--%>
                                                                                                    <div class="dropdown">
                                                                                                        <div id="divd" class="dropdownbtn-content" runat="server">
                                                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                                                <tr>

                                                                                                                    <td>
                                                                                                                        <asp:ImageButton ID="View" runat="server"
																															CommandArgument='<%# Eval("SrNo") %>'
																															CommandName="View" 
																															CssClass="FileAttachmentICN" 
																															ImageUrl="icons/CLIP01.ICO" />
                                                                                                                    </td>

                                                                                                                    <td>
                                                                                                                        <asp:ImageButton ID="Remove" runat="server"
																															CommandArgument='<%# Eval("SrNo") %>'
																															CausesValidation="false"
																															CommandName="Remove"
																															CssClass="largerActionICNS"
																															ImageUrl="~/images/delete.png"
																															Visible="true" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                        <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                                            Style="cursor: pointer" />
                                                                                                    </div>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false"></asp:ImageButton>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <%-- End --%>
                                    </table>
                                    </td>
                                    <td valign="top" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlOtherDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblGrandTotal" class="clsLabelAuto">Total</span>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtCTotal" runat="server" Text="<%# mReceiptCumInvoice.DisplayCTotalAmount %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Total " BackColor="#E0E0E0" ReadOnly="True" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblTotaolOtherCharges" class="clsLabelAuto">Total Other Charges</span>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtCTotalOtherCharge" runat="server" Text="<%# mReceiptCumInvoice.DisplayCTotalCharges %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Total Other Charges" BackColor="#E0E0E0"
                                                                ReadOnly="True" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabel">Total CGST</asp:Label>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtTotalCGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mReceiptCumInvoice.DisplayCTotalCGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblTotalSGST" runat="server" CssClass="clsLabel">Total SGST</asp:Label>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtTotalSGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mReceiptCumInvoice.DisplayCTotalSGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabel">Total IGST</asp:Label>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtTotalIGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mReceiptCumInvoice.DisplayCTotalIGSTAmount %>"
                                                                ReadOnly="True" BackColor="#E0E0E0" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblRemaining" class="clsLabelAuto">Grand Total</span>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtCGrandTotal" runat="server" Text="<%# mReceiptCumInvoice.DisplayCGrandTotal %>"
                                                                CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Grand Total" BackColor="#E0E0E0"
                                                                ReadOnly="True" Width="150px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblAmountInWords" class="clsLabelAuto">Amount In Words </span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtAmountInWords" runat="server" Text="<%# mReceiptCumInvoice.DisplayAmountINWords %>"
                                                                CssClass="clsTextBoxTagSearch" ToolTip="Amount In Words" ReadOnly="True" BackColor="#E0E0E0"
                                                                TextMode="MultiLine" Width="400px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalDocketCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Total Docket Charge</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtTotalDocketCharge" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                            ToolTip="Total Docket Charge" Visible="False" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblInvoiceDocketCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Invoice Docket Charge</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtInvoiceDocketCharge" runat="server" CssClass="clsTextBoxRightAlign_Ajax"
                                                                ToolTip="Invoice Docket Charge" Visible="False" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblTotalDocketCharge" runat="server" CssClass="clsLabelAuto" Visible="False">Total Docket Charge</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtRemark" runat="server" Text="<%# mReceiptCumInvoice.Remark %>"
                                                                CssClass="clsTextBoxTagSearchMultilineNewStyleLong" Width="400px" MaxLength="100" ToolTip="Enter Remark" TextMode="MultiLine"
                                                                Rows="5">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right"></td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSentToBill" runat="server" Text="Send To Bill" class="clsbtnH clsinfoH1"
                                                                ToolTip="Click to send the Goods Receipt for billing"></asp:Button>
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Cancel the Goods Receipt"></asp:Button>
                                                            <asp:Button ID="btnSaveAttachment" runat="server" Text="Save Attachment" class="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Save Goods Receipt and Goods Receipt Item Attachments"></asp:Button>
                                                            <asp:Button ID="btnSendMail" runat="server" class="clsbtnH clsinfoH1" Text="Send Mail"
                                                                ClientIDMode="Static" ToolTip="Click to Send Mail" Visible="<%# (mReceiptCumInvoice.StatusID = 2) %>"></asp:Button>
                                                            <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" class="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Authorize the Goods Receipt"></asp:Button>
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="clsbtnH clsinfoH1" ToolTip="Click to Save Goods Receipt"
                                                                ValidationGroup="a"></asp:Button>
                                                            <asp:Button ID="btnPrintTag" runat="server" Text="Print Acceptance Tag" class="clsbtnH clsinfoH1"
                                                                ToolTip="Click to Print Acceptance Tag " Visible="<%# Not mReceiptCumInvoice.StatusID=4 %>"
                                                                CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="btnPrint" runat="server" Text="Print" class="clsbtnH clsinfoH1" Enabled="<%# Not mReceiptCumInvoice.IsNew %>"
                                                                ToolTip="Click to print Goods Receipt" CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="btnBack" runat="server" Text="Close" class="clsbtnH clsinfoH1" ToolTip="Click to close Goods Receipt Details screen"></asp:Button>
                                                            <%-- Sankalp 26-09-25 --%>
                                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;" colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
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
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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

            function OpenFileUploadWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    $("#btnDummyFileUpload").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                    //                }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
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
        <!-- End -->
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForReceipt();
                return false;
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
              <% Dim mOpenFrom As String = Request.QueryString("Type") %>
                <% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" or mOpenFrom = "FromReqItemStatusReport") Then %>  
                $('#btnCancel').attr('disabled', 'disabled');
                $('#btnDocketCharge').attr('disabled', 'disabled');
                $('#btnPrintTag').attr('disabled', 'disabled');
                $('#btnPrint').attr('disabled', 'disabled');
                $('#btnSaveAttachment').attr('disabled', 'disabled');
                $('#btnSendMail').attr('disabled', 'disabled');
            <% End if %>  
            });

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
        <!--ReceiptCumInvoiceAttach Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAttachStateComplete() {
                $("#btnDummyAttach").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAttachWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAttach").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForAttach() {
                var Attachwindow = $find("<%=mdlAttach.ClientID %>");
                //close popup window
                Attachwindow.hide();
                //release resources
                $("#IframeAttach").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnAttach").click();
            }
        </script>
        <!-- End-->
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddSupplier = document.getElementById("cmbVendor");
            if (ddSupplier != null) {
                var i = 0;
                if (ddSupplier.disabled == false) {
              <% For Each item1 In mVendorList%>
                <% If item1.NotInUse = "True" Then%>
                    ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    i = i + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
