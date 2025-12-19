<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceiptItem_Ajax.aspx.vb"
    ValidateRequest="false" Inherits="Flypal.wfReceiptItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receipt Item Details</title>
    <script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" AsyncPostBackTimeout="600">
        </asp:ScriptManager>
        <script type="text/javascript" language="javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
			function EndRequestHandler(sender, args) {
				if (args.get_error() != undefined) {
					args.set_errorHandled(true);
				}
			}
		</script>
        <script type="text/javascript">
			window.onload = blinknow;
			function blinknow() {
				var e = document.getElementById("<%=imgID.ClientID%>");
				if (e != null) {
					e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
					setTimeout("blinknow();", 750);
				}
			}
		</script>
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
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Receipt Item [New]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add Item in Receipt cum Invoice Item List"
                                                                                Text="OK"></asp:Button>
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
                                                HeaderText="Fill Up The Following Fields" />
                                            <asp:RequiredFieldValidator ID="rfvSrNo" runat="server" Display="None" ErrorMessage="Sr. No. Required"
                                                ControlToValidate="txtSrNo" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" Display="None" ErrorMessage="Part No. Required"
                                                ControlToValidate="txtPartNo" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvQty" runat="server" Display="None" ErrorMessage="Quantity Required"
                                                ControlToValidate="txtQuantity" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvRelNoteNo" runat="server" Display="None" ErrorMessage="Max lenght should be 50."
                                                ControlToValidate="txtReleaseNote" OnServerValidate="CustomValidate" CssClass="clsLabel"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvStore" runat="server" Display="None" ErrorMessage="Please Select Store."
                                                ControlToValidate="cmbStore" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvStore" runat="server" Display="None" ErrorMessage="Please select the Store."
                                                ControlToValidate="cmbStore" OnServerValidate="CustomValidate" CssClass="clsLabel"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvPartType" runat="server" Display="None" ErrorMessage="Please Select Part Type."
                                                ControlToValidate="cmbPartType" CssClass="clsLabel"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvPartType" runat="server" Display="None" ErrorMessage="Please select the Part Type."
                                                ControlToValidate="cmbPartType" OnServerValidate="CustomValidate" CssClass="clsLabel"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvQty" runat="server" Display="None" ErrorMessage="Should be Non-Zero Positive Value."
                                                ControlToValidate="txtQuantity" OnServerValidate="CustomValidate" Width="56px"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCodeNo" runat="server" ControlToValidate="txtCodeNo" Display="None"
                                                ErrorMessage="Code No. Required" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
                                                ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCustVal" runat="server" Display="None" OnServerValidate="CustomValidate1"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:CustomValidator ID="csWarStatus" runat="server" ControlToValidate="cmbWarrantyStatus"
                                                Display="None" ErrorMessage="Please Select Warranty Status As Accepted Or Rejected"
                                                OnServerValidate="CustomValidate" CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvRelNo" runat="server" ControlToValidate="txtReleaseNote"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="Release Note No. Require."
                                                OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:Button ID="hdnAddPeriod" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlAttentionInfo" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Image ID="imgID" runat="server" ImageUrl="~/images/Attention.ico" Visible="<%# mReceipt.ReceiptItems.CurrentItem.ItemTagID > 0 %>" />
                                            <asp:Label ID="lblImageTagName" runat="server" CssClass="clsLabel" Text='<%# " ATTENTION! " + mReceipt.ReceiptItems.CurrentItem.ItemTagName + " OBSERVE PRECAUTIONS FOR HANDLING." %>'
                                                Visible="<%# mReceipt.ReceiptItems.CurrentItem.ItemTagID > 0 %>" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblSerializedStatus" runat="server" CssClass="clsLabelAuto" Font-Bold="True"
                            Visible="False">Receiving Serialized Part</asp:Label>
                    </td>
                </tr>
                <!--**********************************************************-->
                <tr>
                    <td colspan="2">
                        <fieldset id="Fieldset2" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
                            class="clsFieldSet">
                            <legend class="clsFieldSet1"><b>Part Information</b></legend>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <asp:Panel runat="server" ID="Panel3" Style="width: auto;">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset9" style="padding: 0px 2px 0px 0px; width: auto; border-style: none"
                                                        class="clsFieldSetNewStyle">
                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <span id="spnSrNo" class="clsLabel">Sr. No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSrNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                                        MaxLength="4" ReadOnly="True" Text="<%# mReceipt.ReceiptItems.CurrentItem.SrNo %>"
                                                                        ToolTip="Sr. No." Width="36px"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="spnPartNoStar" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="spnPartNo" class="clsLabel">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                        ReadOnly='<%# Session("Edit") %>' Text="<%# mReceipt.ReceiptItems.CurrentItem.ItemName %>"
                                                                        ToolTip="Enter Part No.">
                                                                    </asp:TextBox>
                                                                    <%-- <asp:Button ID="imgbtnPartNo" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                                    Enabled='<%# Not Session("Edit") %>' Height="22px" Text="..." ToolTip="Click to Select New Part No." />--%>
                                                             
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="imgPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                        CausesValidation="False"  Width="24px" ToolTip="Click to Select New Part No."></asp:ImageButton>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAlternatePart" runat="server" CssClass="clsbtnH clsinfoH1" Text="Alternate Part"
                                                                        ToolTip="Click to add Alternate Part" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <span id="spnDescription" class="clsLabel">Description</span>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchLong"
                                                                        MaxLength="50" ReadOnly="True" Text="<%# mReceipt.ReceiptItems.CurrentItem.ItemDescription %>"
                                                                        ToolTip="Part Description"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                    <td valign="top">
                                        <asp:Panel runat="server" ID="Panel4" Style="width: auto;">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset10" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-style: none"
                                                        class="clsFieldSet">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabel" Text='<%# IIf(Not mReceipt.ReceiptItems.CurrentItem.IssueItemID.Equals(Guid.Empty), "Issue No.", IIf(Not mReceipt.ReceiptItems.CurrentItem.OrderItemID.Equals(Guid.Empty), "Order No.", "Ord./Iss.No.")) %>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOrderIssNo" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                        BackColor="#E0E0E0" Text="<%# mReceipt.ReceiptItems.CurrentItem.No %>" ToolTip="Order/Issue No.">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblOrdIssDate" runat="server" CssClass="clsLabel" Text='<%# IIf(Not mReceipt.ReceiptItems.CurrentItem.IssueItemID.Equals(Guid.Empty), "Issue Date", IIf(Not mReceipt.ReceiptItems.CurrentItem.OrderItemID.Equals(Guid.Empty), "Order Date", "Ord./Iss.Date")) %>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                        ReadOnly="true" Text="<%# mReceipt.ReceiptItems.CurrentItem.IODateFormatted %>"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtOrderDate"></cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtOrderDate" ID="txtOrderDateWatermarkExtender"
                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblRequestedBy" class="clsLabel">Requested By</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRequestedBy" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
                                                                        MaxLength="250" ReadOnly="True" Text="<%# mReceipt.ReceiptItems.CurrentItem.RequestedBy %>"
                                                                        ToolTip="Requested By">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblWarrantyStatus" class="clsLabel" runat="server" visible="<%# mReceipt.TransTypeID = 10 %>">Warranty Status</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbWarrantyStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        Visible="<%# mReceipt.TransTypeID = 10 %>" DataTextField="Name" DataValueField="ID"
                                                                        SelectedValue="<%# mReceipt.ReceiptItems.CurrentItem.WarrantyApplicableStatus %>"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <!--**********************************************************-->
                <tr>
                    <td colspan="2">
                        <fieldset id="Fieldset3" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
                            class="clsFieldSet">
                            <legend class="clsFieldSet1"><b>Receiving Information</b></legend>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <%--<asp:Panel runat="server" ID="pnlReceivingInformation" Style="width: auto;">--%>
                                        <asp:UpdatePanel runat="server" ID="upnlReceivingInformation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="Fieldset5" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-style: none;"
                                                    class="clsFieldSet">
                                                    <%--<legend><b>Receiving Information </b></legend>--%>
                                                    <table width="100%">
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span id="spnPartStatus" class="clsLabelAuto">Part Status</span>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mReceipt.ReceiptItems.CurrentItem.ItemTypeID %>"
                                                                                Width="200px" AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <%--  <asp:Button ID="ImgbtnPartType" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                                Style="margin-top: 5px" Height="20px" Text="..." ToolTip="Click to Add New Part Type" />--%>
                                                                            <asp:ImageButton ID="ImgPartType" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                CausesValidation="False" Style="margin-top: -2px" Width="24px" ToolTip="Click to Add New Part Type"></asp:ImageButton>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblColor" runat="server" CssClass="clsColorLabel" Style="margin-top: -2px"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPartStatus" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="spnQtyStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="spnQty" class="clsLabel">Qty.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    Style="margin-left: 4px" MaxLength="9" Text="<%# mReceipt.ReceiptItems.CurrentItem.DisplayQty %>"
                                                                    ToolTip="Enter Quantity." Width="50px"></asp:TextBox>
                                                                <asp:DropDownList ID="cmbUnitConverterList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                    DataTextField="ConvertUnitName" DataValueField="ConvertUnitID" SelectedValue="<%# mReceipt.ReceiptItems.CurrentItem.DisplayUnitID %>"
                                                                    Width="100px" Enabled="False">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblReleaseNoteNoStar" CssClass="clsLabelStar" Visible="<%$AppSettings:ReleaseNoteNoRequire%>">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <span id="lblReleaseNoteNo" class="clsLabel">Rele. Note No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReleaseNote" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
                                                                    Style="margin-left: 4px" Text="<%# mReceipt.ReceiptItems.CurrentItem.ReleaseNoteNo %>"
                                                                    ToolTip="Enter Release Note No."></asp:TextBox>
                                                                <asp:TextBox ID="txtReleaseNoteDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                    Width="100px" AutoPostBack="true" Text="<%# mReceipt.ReceiptItems.CurrentItem.ReleaseNoteDateFormatted %>"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtReleaseNoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReleaseNoteDate"></cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtReleaseNoteDate" ID="txtReleaseNoteDateWatermarkExtender"
                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="spnStoreStar" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="spnStore" class="clsLabelAuto">Store</span>
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="upnlStore" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                            Style="margin-left: 4px" DataTextField="LocationStore" AutoPostBack="true" DataValueField="ID"
                                                                            Enabled='<%#Not Session("Enable") = True %>' SelectedValue="<%# mReceipt.ReceiptItems.CurrentItem.StoreID %>">
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--</asp:Panel>--%>
                                    </td>
                                    <td valign="top">
                                        <%-- <asp:Panel runat="server" ID="pnlReceivingInformation1" Style="width: auto;">--%>
                                        <asp:UpdatePanel runat="server" ID="upnlReceivingInformation1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <fieldset id="Fieldset6" style="padding: 0px 4px 0px 0px; width: auto; border-style: none;"
                                                    class="clsFieldSet">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblBarcodeNo" runat="server" Visible="<%$AppSettings:Barcode%>" CssClass="clsLabel">Barcode No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    ReadOnly="True" Text="<%# mReceipt.ReceiptItems.CurrentItem.BarcodeNo %>" Visible="False">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    AutoPostBack="true" Text="<%# mReceipt.ReceiptItems.CurrentItem.SerialNo %>"
                                                                    ToolTip="Enter Serial No.">
                                                                </asp:TextBox>
                                                                <asp:CustomValidator ID="cvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Serial No Required. " OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblBinLocation" class="clsLabel">Bin Location</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    Text="<%# mReceipt.ReceiptItems.CurrentItem.Location %>" ToolTip="Enter Location of the Store.">
                                                                </asp:TextBox>
                                                                <asp:CustomValidator ID="cvLocation" runat="server" ControlToValidate="txtLocation"
                                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Max. Length should be 50."
                                                                    OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblBatchNo" runat="server" CssClass="clsLabel">Batch No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBatchNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    Text="<%# mReceipt.ReceiptItems.CurrentItem.BatchNo %>" ToolTip="Enter Batch No. for an Item">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCodeNo" runat="server" CssClass="clsLabel" Visible="false">Code No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="20"
                                                                    Visible="false" ToolTip="Code No." Text="<%# mReceipt.ReceiptItems.CurrentItem.CodeNo %>">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%--</asp:Panel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000; border-width: thin;"
                                            class="clsFieldSet">
                                            <legend class="clsFieldSet1"><b>Attachment(s)</b></legend>
                                            <asp:Panel runat="server" ID="pnlAttachment" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlAttachment" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="70%">
                                                            <%--Commented by Shital on 
                                          <tr>
                                            <td>
                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                    runat="server" class="clsButton_Ajax" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDelAttach1" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                <asp:Button ID="hdnBtnFileUpload"  runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </td>
                                        </tr>--%>
                                                            <tr>
                                                                <td style="height: 15px">
                                                                    <asp:UpdatePanel ID="upnldgReceiptAttachment" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <%-- <asp:GridView ID="dgReceiptAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                            CssClass="clsGridNewStyle" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
                                                                            AllowPaging="False" AutoGenerateColumns="false">--%>
                                                                            <asp:GridView ID="dgReceiptAttachment" ToolTip="List of File Attachment(s)" runat="server"
                                                                                CssClass="clsGridNewStyle" AutoGenerateColumns="False" DataKeyNames="ID" ShowHeaderWhenEmpty="True"
                                                                                AllowSorting="True" CellPadding="5" ForeColor="Black" GridLines="Horizontal"
                                                                                PageSize="5" AllowPaging="False">
                                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                <RowStyle CssClass="clsdgItem" />
                                                                                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                                        <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
                                                                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="File Name">
                                                                                        <HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                                                                ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>'
                                                                                                Width="350px" DESIGNTIMEDRAGDROP="767"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
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
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false" Enabled="<%# mReceipt.StatusID = 1 %>"></asp:ImageButton>
                                                                    <asp:Button ID="hdnBtnFileUpload" runat="server" Text="----" ClientIDMode="Static"
                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <!--**********************************************************-->
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upnlTabDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <cc2:TabContainer ID="tabReceiptDetailsContainer" runat="server" class="clstablelistin" AutoPostBack="true"
                                    Visible="true">
                                    <cc2:TabPanel ID="tabExpiryDetails" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Expiry(s)" ID="lblExpiry"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlExpiryDetails" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlExpiryInformation" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="Fieldset7" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9500; border-style: none;"
                                                            class="clsFieldSet">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td valign="top" colspan="4">
                                                                        <asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date should be Later to Start Date."
                                                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvExpiryDate" runat="server" ControlToValidate="txtExpiryDate"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Expiry Date Should be Later to Start Date."
                                                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvcureqtrs" runat="server" ControlToValidate="txtCureQtrs"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvCureYrs" runat="server" ControlToValidate="txtCureYear"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvExpQtrs" runat="server" ControlToValidate="txtExpQrts"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cvExpYrs" runat="server" ControlToValidate="txtExpYear"
                                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" align="left">
                                                                        <asp:Label ID="lblExpPeriod" runat="server" CssClass="clsLabelAuto" Text="<%# mReceipt.ReceiptItems.CurrentItem.ExpiryPeriod %>">
                                                                        </asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <table id="Table4" width="100%" border="0" cellpadding="0" cellspacing="0" runat="server"
                                                                            visible='<%#IIf(mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 Or mReceipt.ReceiptItems.CurrentItem.IsExpiryItem, True, False) %>'>
                                                                            <tr>
                                                                                <td>
                                                                                    <%-- <span id="lblOthers" class="clsLabel">Others</span>--%>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkIsExpiryNA" runat="server" AutoPostBack="True" Checked="<%# mReceipt.ReceiptItems.CurrentItem.IsExpiryNA %>"
                                                                                        CssClass="clsCheckBox" Text="N/A" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkIsExpiryUnlimited" runat="server" AutoPostBack="True" Checked="<%# mReceipt.ReceiptItems.CurrentItem.IsExpiryUnlimited %>"
                                                                                        CssClass="clsCheckBox" Text="Unlimited" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span3" class="clsLabel" style="height: 15px"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblStartDate" class="clsLabel">Cure Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" AutoPostBack="true"
                                                                            Text="<%# mReceipt.ReceiptItems.CurrentItem.StartDateFormatted %>"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="txtStartDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lstExpiryDate" class="clsLabel">Expiry Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="clsTextBoxTagSearch" Width="100px"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" Text="<%# mReceipt.ReceiptItems.CurrentItem.ExpiryDateFormatted %>"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtExpiryDate" ID="txtExpiryDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Label3" class="clsLabel" runat="server" visible="<%# ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) Or (mReceipt.ReceiptItems.CurrentItem.IsExpiryItem))%>">Cure Quarter</span>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table5" border="0" cellpadding="0" cellspacing="0" runat="server" visible="<%# ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) Or (mReceipt.ReceiptItems.CurrentItem.IsExpiryItem))%>">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCureQtrs" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        Enabled="<%# (mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter > 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) %>"
                                                                                        MaxLength="1" Text="<%# mReceipt.ReceiptItems.CurrentItem.CureQtrs %>" ToolTip="Enter Quarter."
                                                                                        Width="37px"></asp:TextBox>
                                                                                    <asp:Label ID="Label5" runat="server">/</asp:Label>
                                                                                    <asp:TextBox ID="txtCureYear" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        Enabled="<%# (mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter > 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) %>"
                                                                                        MaxLength="4" Text="<%# mReceipt.ReceiptItems.CurrentItem.CureYear %>" ToolTip="Enter Cure Year."
                                                                                        Width="64px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Label4" class="clsLabel" runat="server" visible="<%# ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) Or (mReceipt.ReceiptItems.CurrentItem.IsExpiryItem))%>">Expiry Quarter</span>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table2" border="0" cellpadding="0" cellspacing="0" runat="server" visible="<%# ((mReceipt.ReceiptItems.CurrentItem.ExpiryMonth <> 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter <> 0) Or (mReceipt.ReceiptItems.CurrentItem.ExpiryMonth = 0 And mReceipt.ReceiptItems.CurrentItem.ExpiryQuarter = 0) Or (mReceipt.ReceiptItems.CurrentItem.IsExpiryItem))%>">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtExpQrts" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        MaxLength="1" Text="<%# mReceipt.ReceiptItems.CurrentItem.ExpQtrs %>" ToolTip="Enter Expiry Quarter."
                                                                                        Width="35px"></asp:TextBox>
                                                                                    <asp:Label ID="Label6" runat="server">/</asp:Label>
                                                                                    <asp:TextBox ID="txtExpYear" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                                        MaxLength="4" Text="<%# mReceipt.ReceiptItems.CurrentItem.ExpYear %>" ToolTip="Enter Expiry Year."
                                                                                        Width="76px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabWarranty" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Warranty(s)" ID="Label2"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel2" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlWarrantyDetails" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="Fieldset8" style="padding: 0px 4px 0px 0px; width: auto; z-index: 9500; border-style: none"
                                                            class="clsFieldSet">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:CheckBox ID="chkIsInWarranty" runat="server" AutoPostBack="True" Checked="<%# mReceipt.ReceiptItems.CurrentItem.IsWarranty %>"
                                                                            CssClass="clsLabelAuto" Text="Under Warranty" TextAlign="Left" />
                                                                        &nbsp; &nbsp;<span id="spnIn" class="clsLabelAuto"><b>In</b></span> &nbsp; &nbsp;
                                                                    <asp:TextBox ID="txtWarrantyInDays" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                        MaxLength="4" Text="<%# mReceipt.ReceiptItems.CurrentItem.WarrantyInDays %>"
                                                                        Width="30px"></asp:TextBox>
                                                                        <span id="lblDays" class="clsLabelAuto">Days</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span1" class="clsLabel">Start Date</span>

                                                                        <asp:TextBox ID="txtWarrantyStartDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                            Text="<%# mReceipt.ReceiptItems.CurrentItem.WarrantyStartDateFormatted %>" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtWarrantyStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyStartDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtWarrantyStartDate" ID="txtWarrantyStartDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>

                                                                        <span id="lblExpiryDate1" class="clsLabel">End Date</span>

                                                                        <asp:TextBox ID="txtWarrantyExpiryDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                            Enabled="false" Text="<%# mReceipt.ReceiptItems.CurrentItem.WarrantyExpiryDateFormatted %>"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtWarrantyExpiryDate_CalendarExtender" runat="server"
                                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtWarrantyExpiryDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtWarrantyExpiryDate" ID="txtWarrantyExpiryDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:CheckBox ID="chkIsTransitDamage" runat="server" Checked="<%# mReceipt.ReceiptItems.CurrentItem.IsTransitDamage %>"
                                                                            CssClass="clsLabelAuto" Text="Transit Damage" TextAlign="Left" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabBenchCheck" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Benchcheck(s)/Calibration(s)" ID="Label7"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlCalibrationInfo" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlCalibrationInfo" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <fieldset id="Fieldset11" style="padding: 0px 0px 0px 0px; width: auto; z-index: 9000; margin-left: 5px;">
                                                            <legend class="clsFieldSet1"><b>Benchcheck/Calibration Information</b></legend>
                                                            <table>
                                                                <tr>
                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtConditionCheckDoneOnDate"
                                                                        Display="None" ErrorMessage="Condition date Required" OnServerValidate="CustomValidate"
                                                                        CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtServicedInspectedDoneOnDate"
                                                                        Display="None" ErrorMessage="Serviced/Inspected date Required" OnServerValidate="CustomValidate"
                                                                        CssClass="clsLabelAuto" ValidateEmptyText="true"></asp:CustomValidator>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="Span4" class="clsLabel">Calibration Start Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCalibrationDoneOnDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" Text="<%# mReceipt.ReceiptItems.CurrentItem.CalibrationDoneOnDateFormatted %>"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtCalibrationDoneOnDate_CalendarExtender" runat="server"
                                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCalibrationDoneOnDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtCalibrationDoneOnDate" ID="txtCalibrationDoneOnDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span2" class="clsLabel">Manufacturing Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtManufacturingDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" Text="<%# mReceipt.ReceiptItems.CurrentItem.ManufacturingDateFormatted %>"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtManufacturingDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtManufacturingDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtManufacturingDate" ID="txtManufacturingDateWatermarkExtender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="pnlConditionCheckInfo" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlConditionCheckInfo" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <fieldset id="Fieldset14" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;">
                                                                        <legend class="clsFieldSet1"><b>Condition Check Information</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span12" class="clsLabel">Start Date</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtConditionCheckDoneOnDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                                        Text="<%# mReceipt.ReceiptItems.CurrentItem.ConditionCheckDoneOnDateFormatted %>"
                                                                                        AutoPostBack="true"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtConditionCheckDoneOnDate_CalendarExtender" runat="server"
                                                                                        CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtConditionCheckDoneOnDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtConditionCheckDoneOnDate" ID="txtConditionCheckDoneOnDateWatermarkExtender"
                                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                                <td></td>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td style="width: 50%">
                                                                    <fieldset id="Fieldset15" style="padding: 0px 4px 0px 0px; width: auto; z-index: 8000;">
                                                                        <legend class="clsFieldSet1"><b>Serviced Inspected Information</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span14" class="clsLabel">Start Date</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServicedInspectedDoneOnDate" runat="server" CssClass="clsTextBoxTagSearchDate"
                                                                                        Text="<%# mReceipt.ReceiptItems.CurrentItem.ServiedInspectedCheckDoneOnDateFormatted %>"
                                                                                        AutoPostBack="true"></asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtServicedInspectedDoneOnDate"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtServicedInspectedDoneOnDate" ID="TextBoxWatermarkExtender1"
                                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                                </td>
                                                                                <td></td>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabExcessQty" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Excess,Short,Rejected Item(s)" ID="lblExcessHeader"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="upnlExcessQty" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span class="clsLabelAuto">Excess Qty.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtExcessQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    AutoPostBack="true" MaxLength="9" Text="<%# mReceipt.ReceiptItems.CurrentItem.ExcessQty %>"
                                                                    ToolTip="Enter Excess Quantity." Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span class="clsLabelAuto">Short Qty.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtShortQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    AutoPostBack="true" MaxLength="9" Text="<%# mReceipt.ReceiptItems.CurrentItem.ShortQty %>"
                                                                    ToolTip="Enter Short Quantity." Width="50px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <span class="clsLabelAuto">Rejected Qty.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRejectedQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    Enabled="<%# mReceipt.StatusID = 1 %>" MaxLength="9" Text="<%# mReceipt.ReceiptItems.CurrentItem.RejectedQty %>"
                                                                    ToolTip="Enter Rejected Quantity." Width="50px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <cc2:TabPanel ID="tabSinceOH" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Since New/Since Overhaul" ID="lblSinceOH"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel5" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="upnlTSNTSOValues" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%--<asp:GridView ID="dgPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                CellPadding="5" ForeColor="Black" CssClass="clsGridNewStyle" RowStyle-Wrap="false"
                                                                HeaderStyle-Wrap="false" ShowHeaderWhenEmpty="True" PageSize="13" PagerSettings-Mode="NextPreviousFirstLast">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />--%>
                                                                    <asp:GridView ID="dgPeriods" runat="server" CssClass="clsGridNewStyle" AllowPaging="false"
                                                                        AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True" CellPadding="5"
                                                                        ForeColor="Black" GridLines="Horizontal" PageSize="5">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Name" HeaderText="Periods"></asp:BoundField>
                                                                            <asp:TemplateField HeaderText="TSN Value">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTSNValue" runat="server" ToolTip="Enter corresponding Period Value."
                                                                                        CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "TSNValueFormatted") %>'>
                                                                                    </asp:TextBox>
                                                                                    <asp:CustomValidator ID="cvTSNValue" runat="server" Display="None" ControlToValidate="txtTSNValue"
                                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="TSOH Value">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTSOHValue" runat="server" ToolTip="Enter corresponding Period Value."
                                                                                        CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="80px" Text='<%# DataBinder.Eval(Container.DataItem, "TSOValueFormatted") %>'>
                                                                                    </asp:TextBox>
                                                                                    <asp:CustomValidator ID="cvTSOHValue" runat="server" Display="None" ControlToValidate="txtTSOHValue"
                                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField CommandName="ForDelete" HeaderText="Remove" Text="Remove" />
                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                    </asp:GridView>
                                                                </td>
                                                                <td valign="top">
                                                                    <%--  <asp:Button ID="btnAddPeroid" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                                    Height="20px" Text="..." ToolTip="Click to add New Period" />--%>
                                                                    <asp:ImageButton ID="ImgAddPeroid" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                        Style="margin-top: 10px" Width="24px" ToolTip="Click to Add New Peroid" CausesValidation="false"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <%--  <cc2:TabPanel ID="tabAttachment" runat="server" CssClass="clsPanel1">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Attachment(s)" ID="lblAttachment"></asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                      
                                    </ContentTemplate>
                                </cc2:TabPanel>--%>
                                    <cc2:TabPanel ID="tabRemark" runat="server" CssClass="clsPanel1">
                                        <HeaderTemplate>
                                            <asp:Label runat="server" Text="Remark/Note Information" ID="lblRemark"></asp:Label>
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="Panel6" Style="width: auto;">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="spnRemark" class="clsLabel">Remark<asp:CustomValidator ID="cvRemark" runat="server"
                                                                        ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" ErrorMessage="Remark Max. Length should be 500."
                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" MaxLength="500"
                                                                        Text="<%# mReceipt.ReceiptItems.CurrentItem.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark."></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span6" class="clsLabelAuto">Note<asp:CustomValidator ID="cvNote" runat="server"
                                                                        ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" ErrorMessage="Note Max. Length should be 500."
                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" MaxLength="500"
                                                                        Text="<%# mReceipt.ReceiptItems.CurrentItem.Note %>" TextMode="MultiLine" ToolTip="Enter Note."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span13" class="clsLabelAuto" style="display: none">Previous Work Scope<asp:CustomValidator
                                                                        ID="cvPreviousWorkScope" runat="server" ControlToValidate="txtPreviousWorkScope"
                                                                        CssClass="clsLabelAuto" Display="None" ErrorMessage="Previous Work Scope Max. Length should be 500."
                                                                        OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                    </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPreviousWorkScope" runat="server" CssClass="clsTextBox1" Height="36px"
                                                                        Visible="false" MaxLength="500" Text="<%# mReceipt.ReceiptItems.CurrentItem.PreviousWorkScope %>"
                                                                        TextMode="MultiLine" ToolTip="Enter Previous Work Scope." Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc2:TabPanel>
                                    <%--  <cc2:TabPanel ID="tabBenchCheck" runat="server" CssClass="clsPanel1" >
                                  <HeaderTemplate>
                                        <asp:Label runat="server" Text="Benchcheck/Calibration Information" ID="Label7"></asp:Label>
                                    </HeaderTemplate>
                                   <ContentTemplate>
                                   
                                   
                                   </ContentTemplate>
                                 </cc2:TabPanel>--%>
                                </cc2:TabContainer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <%-- <tr>
                <td valign="top" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlBenchcheckCalibrationInformation" 
                                        UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fsBenchcheckCalibrationInformation" style="padding: 0px 4px 0px 0px;
                                                width: auto; z-index: 10000;" class="clsLabelHeader">
                                                <legend><b>Benchcheck/Calibration Information</b></legend>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                </td>
            </tr>--%>
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
			function CallResize() {
				alert("Hello");
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
        <!-- End File Upload Modal Dialog-->
        <%-- Open period--%>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlAddPeriod" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IframeAddPeriod" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupTaskCardStep" runat="server" TargetControlID="btnDummyAddPeriod"
            PopupControlID="pnlAddPeriod" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
			function IFrameStateComplete() {
				$("#btnDummyAddPeriod").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenAddPeriodWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeAddPeriod").attr("src", "wfSelectPeriod_Ajax.aspx?Type=pup");

					//                if (!$.browser.msie) {
					$("#btnDummyAddPeriod").click();
					$get("AjaxLoader").style.visibility = 'hidden';
					//}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForAddPeriod() {
				var TaskCardStepwindow = $find("<%=mdlPopupTaskCardStep.ClientID %>");
				//close Task Card Step popup window
				TaskCardStepwindow.hide();
				//           release resources
				$("#IframeAddPeriod").attr("src", "JavaScript:''");
				//call image button
				$("#hdnAddPeriod").click();
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
</body>
</html>
