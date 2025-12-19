<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPaymentAdvice_Ajax.aspx.vb"
    Inherits="Flypal.wfPaymentAdvice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <title>Payment Advice</title>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
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
    <script type="text/javascript">
        //window.onload = blinknow;
        function blinknow() {
            var e = document.getElementById("<%=lblStatus.ClientID%>");

            e.style.visibility = (e.style.visibility == 'visible') ? 'hidden' : 'visible';
            setTimeout("blinknow();", 750);
            //btnVisibility();

        }
    </script>
    <script type="text/javascript">
        function btnVisibility() {
            var Val1 = $get("cmbVendorList");
            var Val2 = $get("cmbcurrency");
            var UpdatePanel1 = '<%=upnlbtnAdd.ClientID%>';


            if (Val1.selectedIndex != 0 && Val2.selectedIndex != 0) {
                $get("btnAdd").style.visibility = "visible";
                __doPostBack(UpdatePanel1, '');
            }
            else {
                $get("btnAdd").style.visibility = "hidden";

            }
        }
    </script>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td valign="top">
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Payment Advice [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" ValidationGroup="1" runat="server"
                                                CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CustValidator" ValidationGroup="1" runat="server" OnServerValidate="CustomValidate"
                                                ValidateEmptyText="true" ControlToValidate="txtPaymentRef" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator1" ValidationGroup="1" runat="server" OnServerValidate="CustomValidate"
                                                ValidateEmptyText="true" ControlToValidate="txtBank" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator2" ValidationGroup="1" runat="server" OnServerValidate="CustomValidate"
                                                ValidateEmptyText="true" ControlToValidate="txtChequeNo" Display="None"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text="<%# mPaymentAdvice.StatusName %>"
                                                CssClass="clsLabelHeader"> </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlPaymentDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Payment Advice Details</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            <span id="lblPaymentDetails" class="clsLabelHeader"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblDateStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblDate" class="clsLabel">Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="calPaymentDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                                AutoPostBack="true" Enabled="<%# mPaymentAdvice.IsNew %>" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                Text="<%# mPaymentAdvice.PaymentAdviceDateFormatted %>" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calPaymentDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calPaymentDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="calPaymentDateWatermarkExtender" runat="server"
                                                                TargetControlID="calPaymentDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblRef" class="clsLabel">Ref</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRef" runat="server" Text="<%# mPaymentAdvice.Text %>" CssClass="clsTextBox_Ajax"
                                                                ToolTip="Enter No." MaxLength="25" Width="140px" Enabled="<%# mPaymentAdvice.IsNew %>"> </asp:TextBox>
                                                            <asp:TextBox ID="txtRefNo" runat="server" Text="<%# mPaymentAdvice.No %>" Enabled="<%# mPaymentAdvice.IsNew %>"
                                                                CssClass="clsTextBox_Ajax" ToolTip="Enter No." MaxLength="25" Width="40px"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="lblNo" class="clsLabel">To.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtToText" runat="server" Text="<%# mPaymentAdvice.PaymentTo %>"
                                                                Enabled="<%# Not mPaymentAdvice.StatusID=2  %>" CssClass="clsTextBoxMultiLine"
                                                                ToolTip="Enter No." MaxLength="25" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Span1" class="clsLabel">From.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFrom" runat="server" Text="<%# mPaymentAdvice.PaymentFrom %>"
                                                                Enabled="<%# Not mPaymentAdvice.StatusID=2  %>" CssClass="clsTextBoxMultiLine"
                                                                TextMode="MultiLine"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span2" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblSupplier" class="clsLabel">Supplier</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="true"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mPaymentAdvice.VendorID %>"
                                                                Enabled="<%#  mPaymentAdvice.PaymentAdviceItems.Count = 0  %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <span id="Span3" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCurrency" class="clsLabel">Currency</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbcurrency" runat="server" CssClass="clsComboBox_Ajax" Width="200px"
                                                                AutoPostBack="true" Enabled="<%#  mPaymentAdvice.PaymentAdviceItems.Count = 0  %>"
                                                                DataTextField="Name" DataValueField="ID" SelectedValue="<%# mPaymentAdvice.CurrencyID %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPaymentItems" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSet" style="border-width: 1px">
                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Order(s) For Payment</b></legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblPaymentItems" class="clsLabelHeader"></span>
                                                        </td>
                                                        <td align="right">
                                                            <asp:UpdatePanel runat="server" ID="upnlbtnAdd" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnAdd" Visible="false" TabIndex="0" runat="server" Text="Add" CssClass="clsButton_Ajax"
                                                                        ToolTip="Click to add Payment Items"></asp:Button>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel runat="server" ID="upnldgPaymentItems" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="dgPaymentItems" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="True"
                                                                        AutoGenerateColumns="False">
                                                                        <PagerSettings Mode="NextPreviousFirstLast" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
                                                                            <asp:BoundField DataField="OrderTextNoDate" HeaderText="Order Info." HtmlEncode="true">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <ItemStyle Wrap="true" />
                                                                                <FooterStyle Wrap="False" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvoiceTextNo" HeaderText="Invoice Info." HeaderStyle-Width="100px"
                                                                                ItemStyle-Width="100px" HtmlEncode="true" />
                                                                            <asp:BoundField DataField="COrderAmount" HeaderText="Value" HeaderStyle-HorizontalAlign="Right"
                                                                                ItemStyle-HorizontalAlign="Right" />
                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark " ItemStyle-Wrap="true" HeaderStyle-Width="150px"
                                                                                ItemStyle-Width="150px" />
                                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                        CommandName="DeleteRecord" Style="height: 17px; width: 17px" ImageUrl="~/images/remove.jpg" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle BackColor="ControlDark" />
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlTotalAmount" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblTotalAmount" class="clsLabel">Total Amount</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTotalAmt" runat="server" Text="<%# mPaymentAdvice.CTotalAmount %>"
                                                            CssClass="clsTextBoxRightAlign1_Ajax" ReadOnly="True" BackColor="#E0E0E0"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="Span4" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblModeofPayment" class="clsLabel">Mode of Payment</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbModeOfPayment" runat="server" DataTextField="Name" DataValueField="ID"
                                                            SelectedValue="<%# mPaymentAdvice.ModeOfPaymentID %>" Enabled="<%# Not mPaymentAdvice.StatusID=2  %>"
                                                            AutoPostBack="True" CssClass="clsComboBox_Ajax">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="lblNote" class="clsLabel">Note</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNote" runat="server" Text="<%# mPaymentAdvice.Note %>" Enabled="<%# Not mPaymentAdvice.StatusID=2  %>"
                                                            CssClass="clsTextBoxMultiLine1_Ajax" TextMode="MultiLine"> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <legend class="clsFieldSet1"><b>File Attachments</b></legend>
                                                            <asp:UpdatePanel ID="upnlPAAttachment" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="height: 15px">
                                                                                <asp:UpdatePanel ID="upnldgPAAttachment" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="dgPAAttachment" runat="server" AllowPaging="False" AllowSorting="True"
                                                                                            AutoGenerateColumns="false" CssClass="clsGrid" DataKeyNames="ID" ShowHeaderWhenEmpty="true"
                                                                                            ToolTip="List of File Attachment(s)">
                                                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                                                <asp:BoundField DataField="PaymentAdviceID" HeaderText="PaymentAdviceID" Visible="False" />
                                                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Width="10px" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName"
                                                                                                    Visible="False">
                                                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                                                </asp:BoundField>
                                                                                                <asp:TemplateField HeaderText="File Name">
                                                                                                    <HeaderStyle HorizontalAlign="Left" Width="350px" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtFileName" runat="server" ClientIDMode="Static" CssClass="clsTextBox3_Ajax"
                                                                                                            MaxLength="100" Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
                                                                                                            ToolTip="Enter File Name To Be Attached" Width="350px"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="View" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>' CommandName="View"
                                                                                                            ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="Remove" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                            CommandName="Remove" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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
                                                                                <asp:ImageButton ID="btnSelectFiles" runat="server" CausesValidation="true" Height="22px"
                                                                                    ImageUrl="~/images/plus1.png" ToolTip="Click to Add New Attachment" Width="24px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPaymentDoneDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlPaymentDetails" runat="server" Visible="False">
                                                <fieldset class="clsFieldSet" style="border-width: 1px">
                                                    <legend id="Legend2" class="clsFieldSet1" runat="server"><b>Payment Details</b></legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="3">
                                                                <asp:UpdatePanel runat="server" ID="unplPendingPADetails" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:ValidationSummary ID="Validationsummary1" ValidationGroup="2" runat="server"
                                                                            CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                        <asp:CustomValidator ID="CustomValidator3" ValidationGroup="2" runat="server" OnServerValidate="CustomValidate"
                                                                            ValidateEmptyText="true" ControlToValidate="txtPaymentRef" Display="None"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="CustomValidator4" ValidationGroup="2" runat="server" OnServerValidate="CustomValidate"
                                                                            ValidateEmptyText="true" ControlToValidate="txtBank" Display="None"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="CustomValidator5" ValidationGroup="2" runat="server" OnServerValidate="CustomValidate"
                                                                            ValidateEmptyText="true" ControlToValidate="txtChequeNo" Display="None"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="CustomValidator6" ValidationGroup="2" runat="server" OnServerValidate="CustomValidate"
                                                                            ValidateEmptyText="true" ControlToValidate="txtPaymentDate" Display="None"></asp:CustomValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Span6" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span5" class="clsLabel">Payment Ref</span>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPaymentRef" runat="server" Text="<%# mPaymentAdvice.PaymentReference %>"
                                                                                Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" CssClass="clsTextBox_Ajax"
                                                                                MaxLength="25" Width="140px"> </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span61" class="clsLabel">Date</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPaymentDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                                                Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                                Text="<%# mPaymentAdvice.PaymentDateFormatted %>" Width="100px"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtPaymentDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtPaymentDate"
                                                                                WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Span7" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span8" class="clsLabel">Bank</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBank" runat="server" Text="<%# mPaymentAdvice.Bank %>" CssClass="clsTextBox_Ajax"
                                                                    Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" MaxLength="25" Width="200px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Span9" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span10" class="clsLabel">Cheque/Swift No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtChequeNo" runat="server" Text="<%# mPaymentAdvice.ChequeNo %>"
                                                                    Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" CssClass="clsTextBox_Ajax"
                                                                    MaxLength="25" Width="200px"> </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td class="clsInnerTable">
                                                                <span id="Span91" class="clsLabel">Attach File</span>
                                                            </td>
                                                            <td>
                                                                <table id="Table1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Button ID="btnSelectFile3" Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>"
                                                                                                    runat="server" Text="Select File" CssClass="clsButton_Ajax" Width="120px" />
                                                                                            </td>
                                                                                            <td style="padding-left: 3px;">
                                                                                                <asp:Button ID="btnDelAttach1" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                            </td>
                                                                                            <td style="padding-left: 2px;">
                                                                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                    Height="20px" Width="20px"></asp:ImageButton>
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
                                                            <td align="right" colspan="3">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table align="right">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnSavePaymentDetails" runat="server" ValidationGroup="2" CausesValidation="true"
                                                                                        Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" CssClass="clsButton_Ajax" Text="Save"
                                                                                        ToolTip="Click to Save Payment Advice" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnClosePaymentDetails" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                        Enabled="<%# Not mPaymentAdvice.IsPaymentDone %>" Text="Close" ToolTip="Click to go back to the previous page" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Panel ID="pnlButtons" runat="server">
                                        <asp:UpdatePanel runat="server" ID="upnlbuttons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server" ClientIDMode="Static" CssClass="clsButton_Ajax"
                                                                Enabled="<%# Not mPaymentAdvice.IsNew And Not mPaymentAdvice.IsPaymentDone %>"
                                                                Text="Print" ToolTip="Click to Print Payment Advice" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" Visible="<%# Not mPaymentAdvice.StatusID=2  %>"
                                                                ToolTip="Click to Save Payment Advice" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSaveAttachmentApprove" runat="server" CssClass="clsButtonLong_Ajax"
                                                                Visible="<%# Not mPaymentAdvice.IsNew  And  mPaymentAdvice.StatusID=1  %>" Text="Authorize"
                                                                ToolTip="Click to Save Payment Advice & Approve" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSendMail" runat="server" CssClass="clsButton_Ajax" Text="Send Mail"
                                                                Visible="<%# mPaymentAdvice.StatusID=2 And Not mPaymentAdvice.IsPaymentDone  %>"
                                                                ToolTip="Click to Send Mail" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                Text="Close" ToolTip="Click to go back to the previous page" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnOrdersForPaymentAdvice" runat="server" CausesValidation="False"
                                                ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnPendingOrdersForPaymentAdvice" runat="server" CausesValidation="False"
                                                ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- Send Mail Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySendMail" Text="Dummy Send Mail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupSendMail" HorizontalAlign="Center" Style="height: 100%;
        width: 100%; vertical-align: Center;">
        <iframe id="iPopupSendMail" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSendMail" runat="server" TargetControlID="btnDummySendMail"
        PopupControlID="pnlPopupSendMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSendMailStateComplete() {
            $("#btnDummySendMail").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }

        function OpenPaymentAdviceSendMailWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#iPopupSendMail").attr("src", "wfSendMailForPaymentAdvice_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySendMail").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForSendMail() {
            var SendMailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
            //close Send Mail popup window
            SendMailWindow.hide();
            $("#iPopupSendMail").attr("src", "JavaScript:''");
            //call Send Mail image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!-- End-->
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
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
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
    <!-- Pending order Payment Advice Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyPendingOrdersPaymentAdvice" Text="TaskCard Spare"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPendingOrdersPaymentAdvice" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframePendingOrdersPaymentAdvice" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupPendingOrdersPaymentAdvice" runat="server" TargetControlID="btnDummyPendingOrdersPaymentAdvice"
        PopupControlID="pnlPendingOrdersPaymentAdvice" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFramePendingOrdersForPaymentAdviceStateComplete() {
            $("#btnDummyPendingOrdersPaymentAdvice").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenPendingOrdersPaymentAdviceWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframePendingOrdersPaymentAdvice").attr("src", "wfPendingOrdersForPaymentAdvice_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyPendingOrdersPaymentAdvice").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }

        function ParentPendingOrdersForPaymentAdvice() {
            var PendingOrdersPaymentAdvicewindow = $find("<%=mdlPopupPendingOrdersPaymentAdvice.ClientID %>");
            //close Payment Advice popup window
            PendingOrdersPaymentAdvicewindow.hide();
            //           release resources
            $("#IframePendingOrdersPaymentAdvice").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnPendingOrdersForPaymentAdvice").click();

        }
       
    </script>
    <!-- End-->
    <!-- Payment Advice Order Detail Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyOrdersForPaymentAdvice" Text="" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlOrdersForPaymentAdvice" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeOrdersForPaymentAdvice" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupOrdersForPaymentAdvice" runat="server" TargetControlID="btnDummyOrdersForPaymentAdvice"
        PopupControlID="pnlOrdersForPaymentAdvice" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameOrdersForPaymentAdviceStateComplete() {
            $("#btnDummyOrdersForPaymentAdvice").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenOrdersForPaymentAdviceWindow() {
            try {

                //$get("AjaxLoader").style.visibility = 'visible';
                $("#IframeOrdersForPaymentAdvice").attr("src", "wfOrderAndInvoiceDetail_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyOrdersForPaymentAdvice").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionOrdersForPaymentAdvice() {
            var OrdersForPaymentAdvicewindow = $find("<%=mdlPopupOrdersForPaymentAdvice.ClientID %>");
            //close Payment Advice popup window
            OrdersForPaymentAdvicewindow.hide();
            //           release resources
            $("#IframeOrdersForPaymentAdvice").attr("src", "JavaScript:''");

            //call image button

            $("#hdnBtnOrdersForPaymentAdvice").click();
        }
        function ParentCallBackFunction() {
            var OrdersForPaymentAdvicewindow = $find("<%=mdlPopupOrdersForPaymentAdvice.ClientID %>");
            //close Payment Advice popup window
            OrdersForPaymentAdvicewindow.hide();
            //           release resources
            $("#IframeOrdersForPaymentAdvice").attr("src", "JavaScript:''");

            //call image button

            $("#hdnBtnOrdersForPaymentAdvice").click();

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
