<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfReceipt_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfReceipt_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receipt against Purchase Order Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
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
                                            <asp:Label ID="lblReceipt" runat="server" CssClass="clsFormHeader">Receipt Detail [New]</asp:Label>
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
                                                ValidationGroup="a" ErrorMessage="ReceiptCumInvoice Date Required." ControlToValidate="txtReceiptDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvReceiptDate" runat="server" ErrorMessage="Receipt Date Required."
                                                Display="None" ControlToValidate="txtReceiptDate" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvReceiptDate" runat="server" ErrorMessage="Enter Receipt Date"
                                                Display="None" ControlToValidate="txtReceiptDate" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvVendor" runat="server" ErrorMessage="Please Select Supplier."
                                                Display="None" ControlToValidate="cmbVendorName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvVendorList" runat="server" ErrorMessage="Please Select Supplier."
                                                Display="None" ControlToValidate="cmbVendorName" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvIntrectNo" runat="server" ErrorMessage="Max Length of Internal Receipt No should be 50."
                                                Display="None" ControlToValidate="txtIntReceiptNo" OnServerValidate="CustomValidate"
                                                ValidationGroup="a"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function ValidateVendor(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbVendor");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }
                                            </script>
                                            <asp:CustomValidator ID="cvDCNo" runat="server" ControlToValidate="txtDCNo" Display="None"
                                                ErrorMessage="Max Lenght of DC no should be 25" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mReceipt.StatusName %>" CssClass="clsLabelHeader">
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
                                                        <span id="lblReceiptDetails" class="clsLabelHeader">Receipt Details</span>
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
                                                        <asp:TextBox ID="txtReceiptDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtReceiptDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReceiptDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtReceiptDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtReceiptDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabel">No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtText" runat="server" Text="<%# mReceipt.Text %>" CssClass="clsTextBoxTagSearch"
                                                            onfocus="SetContextKey();" ToolTip="Enter Receipt Text" MaxLength="25" Width="208px"> </asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="wfReceipt_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
                                                            TargetControlID="txtText" UseContextKey="False">
                                                        </cc2:AutoCompleteExtender>
                                                        <script>
                                                            function SetContextKey() {
                                                                var autoComplete = $find('txtText_Autocomplete');
                                                                var TransTypeID = 'TransTypeID=<%=mReceipt.TransTypeID%>¿QuotationDate=<%=mReceipt.RecdDate%>';
                                                                autoComplete.set_contextKey(TransTypeID);
                                                            }
                                                        </script>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" Text="<%# mReceipt.No %>" CssClass="clsTextBoxTagSearch"
                                                            Width="60" MaxLength="8" ToolTip="Enter Receipt No."> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblInternalReceiptNo" runat="server" CssClass="clsLabelAuto">Int. Recpt. No.</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtIntReceiptNo" runat="server" Text="<%# mReceipt.IntReceiptNo %>"
                                                            CssClass="clsTextBoxTagSearch" Enabled="<%# mReceipt.StatusID = 1 %>" MaxLength="50"
                                                            ToolTip="Enter Internal Receipt No.">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
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
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsbtnH clsinfoH1" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
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
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblBarcodeNo" runat="server" CssClass="clsLabelAuto" Visible="false">Barcode No.</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtBarcodeNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                            ReadOnly="True" Text="<%# mReceipt.BarcodeNo %>" Visible="False">
                                                        </asp:TextBox>
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
                                                        <asp:Label ID="lblReceivedFrom1" runat="server" CssClass="clsLabelHeader">Supplier Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblReceivedFrom" class="clsLabel">Received From</span>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:DropDownList ID="cmbVendorName" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
                                                            Enabled="<%# mReceipt.IsNew %>" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mReceipt.VendorID %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDCNo" class="clsLabelAuto">D.C.No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDCNo" runat="server" Text="<%# mReceipt.DCNO %>" CssClass="clsTextBoxTagSearch"
                                                            Enabled="<%# mReceipt.StatusID = 1 %>" MaxLength="25" ToolTip="Enter D.C.No.">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDCDate" class="clsLabel">D.C.Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDCDate" runat="server" CssClass="clsTextBoxTagDateSearch" Width="100px"
                                                            onchange="ValidateDateText(this,'Date_watermarkextender','false');" ClientIDMode="Static"
                                                            Text="<%# mReceipt.DCDateFormatted %>"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtDCDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDCDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDCDate" ID="txtDCDateTextBoxWatermarkExtender"
                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblAWBNo" class="clsLabelAuto">Custom Bill of Entry</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# mReceipt.StatusID = 1 %>"
                                                            MaxLength="50" Text="<%# mReceipt.AWBNo %>">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlReceiptItems" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblReceiptItemCaption" class="clsLabelHeader">Receipt Item(s): </span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddItem" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click to Add New Receipt Item" ValidationGroup="a"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgReceiptItems" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Font-Bold="true"  HeaderText="View" HeaderStyle-Height="15px" HeaderStyle-Width="15px"  >
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ViewAttachment" runat="server" CausesValidation="false" CommandName="ViewRec"
                                                                            CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' ImageUrl="icons/CLIP01.ICO"
                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' Text="" Height="15px" Width="15px" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--0--%>
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
                                                               <%-- <asp:BoundField DataField="No" HeaderText="Order No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>--%>
                                                                <%--5--%>
                                                               <%-- <asp:BoundField DataField="IODateformatted" HeaderText="Order Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>--%>
                                                                 <asp:BoundField DataField="CodeNo" HeaderText="Code No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                               <%-- ========Ajay 4 ============--%>
                                                                  <asp:BoundField DataField="OrderIssueInfo" HeaderText="Order Info" HtmlEncode="false"> 
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField> 
                                                               
                                                                <%--6--%>  <%--5--%>  
                                                                <%--<asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No.">
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>--%>
                                                                <%--7--%>  <%--6--%>  
                                                               <%-- <asp:BoundField DataField="ReleaseNoteDateformatted" HeaderText="Release Note Date">
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>--%>
                                                                 
                                                                 <%-- ========Ajay 5 ============--%>
                                                               <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info">
                                                                    <ItemStyle Wrap="true"></ItemStyle>
                                                                </asp:BoundField> 
                                                                <%--8--%>   <%--6--%>   
                                                                <%-- <asp:BoundField DataField="Qty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Unit" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="DisplayQty" HeaderText="Qty.">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--9--%>  <%--7--%> 
                                                                <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--10--%>  <%--8--%> 
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--11--%>  <%--9--%> 
                                                                <%--<asp:BoundField DataField="StoreName" HeaderText="Store">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <%--12--%>  
                                                               <%-- <asp:BoundField DataField="Location" HeaderText="Location">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                  <%-- ========Ajay 9 ============--%>
                                                                  <asp:BoundField DataField="StoreLocInfo" HeaderText="Store Info">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                     <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--13--%>   <%--10--%>
                                                                <%--<asp:BoundField DataField="StartDateformatted" HeaderText="Cure Date">
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>--%>
                                                                <%--14--%>  <%--11--%>
                                                                <%--<asp:BoundField DataField="ExpiryDateformatted" HeaderText="Expiry Date">
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>--%>
                                                                <%--15--%>  <%--12--%>
                                                                <%-- <asp:BoundField DataField="CureQtrYear" HeaderText="Cure Qtrs.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <%--16--%>  <%--13--%>
                                                                <%--<asp:BoundField DataField="ExpQtrYear" HeaderText="Expiry Qtrs.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                  <%-- ========Ajay 10 ============--%>
                                                                    <asp:BoundField DataField="Cure" HeaderText="Cure Info">
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                                                    
                                                                </asp:BoundField> 
                                                                 <%-- ========Ajay 11 ============--%>
                                                                    <asp:BoundField DataField="Expiry" HeaderText="Expiry Info">
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField> 
                                                                <%--17--%>  <%--12--%> 
                                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--18--%>  <%--13--%>  
                                                               <%-- <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <%--19--%>  <%--14--%>  
                                                              <%--  <asp:BoundField DataField="Note" HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                 <%-- ========Ajay 13 ============--%>
                                                                <%--20--%>  <%--13--%>  
                                                               <%-- <asp:BoundField DataField="CodeNo" HeaderText="Code No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>


                                                                <%--21--%>   
                                                                <%-- <asp:ButtonField CommandName="EditView" HeaderText="Edit" Text="Edit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>
                                                                <%--22--%>
                                                                <%--<asp:ButtonField CommandName="DeleteRecord" HeaderText="Remove" Text="Remove">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>
                                                                <%--23--%>
                                                                <%--Ajay--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditView" Style="height: 15px;
                                                                                                width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRecord" Style="height: 20px;
                                                                                                width: 20px" ImageUrl="~/images/delete.png" />
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
                                                                <%--22--%>
                                                                <asp:ButtonField CommandName="Attach" HeaderText="Attach" Text="Attach" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:ButtonField>
                                                                <%--23--%>
                                                                <asp:ButtonField CommandName="Remove" HeaderText="Remove Attachment" Text="Remove"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:ButtonField>
                                                                <%--24--%>
                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                <%--25--%>
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
                                <td colspan="2" align="right">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsbtnH clsinfoH1"
                                                            ToolTip="Click to cancel Receipt"></asp:Button>
                                                        <asp:Button ID="btnSaveAttachment" runat="server" Text="Save Attachment" CssClass="clsbtnH clsinfoH1"
                                                            ToolTip="Click to Save Receipt and Receipt Item Attachments"></asp:Button>
                                                        <asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
                                                            Visible="false" ClientIDMode="Static" ToolTip="Click to Send Mail"></asp:Button>
                                                        <asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsbtnH clsinfoH1"
                                                            ToolTip="Click to authorize the Receipt"></asp:Button>
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH1" ToolTip="Click to save Receipt"
                                                            ValidationGroup="a"></asp:Button>
                                                        <asp:Button ID="btnPrintTag" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print Acceptance Tag"
                                                            ToolTip="Click to Print Acceptance Tag " Visible="<%# Not mReceipt.StatusID=4 %>"
                                                            CausesValidation="False"></asp:Button>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" ToolTip="Click to Print the Receipt"
                                                            Enabled="<%# mReceipt.IsNew %>" CausesValidation="False"></asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close" ToolTip="Click to close the Receipt against Purchase Order screen">
                                                        </asp:Button>
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
            parent.ParentCallBackFunctionForReceipt1();
            return false;
        }
    </script>
    <script type="text/javascript">
              $(document).ready(function () {
              <% Dim mOpenFrom As String = Request.QueryString("Type") %>
                <% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" or mOpenFrom = "FromReqItemStatusReport")  Then %>  
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
    <!--ReceiptAttach Popup Window -->
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
</body>
</html>
