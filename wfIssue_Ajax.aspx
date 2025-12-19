<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssue_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfIssue_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Issue Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
    function OpenLocation(FileName) {
        window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');
    }
    </script>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="AlertMessage.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
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
    </script>
    <script type="text/javascript">
    function FireOnClickButton(e) {
        if (e.keyCode == 13 || e.keyCode == 9) {
            document.getElementById("btnAddBarcodeItem").click();
        }
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
    <%--<script type="text/javascript">
    function enterEvent(e) {
        if (e.keyCode == 13) {
            $("input[id=btnAddBarcodeItem]").click();
        }
    }
</script>--%>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspanel1" runat="server">
                        <table id="tblinner" class="clsTablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Issue Details [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                Width="100%" HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvWorkShop" runat="server" OnServerValidate="customvalidate"
                                                Display="None" ControlToValidate="cmbWorkShop" ErrorMessage="Select Work Shop from the list."
                                                CssClass="clsValidationSummary" ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clsValidationSummary"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvIssueTo" runat="server" ControlToValidate="cmbToType"
                                                Display="None" ErrorMessage="Select Issue To from the list." OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvToStore" runat="server" ControlToValidate="cmbLocationStore"
                                                Display="None" ErrorMessage="Select Issue to Store from the list." OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvVendorList" runat="server" ControlToValidate="cmbVendorList"
                                                Display="None" ErrorMessage="Select vendor from the list." OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAircraftList" runat="server" ControlToValidate="cmbAircraftList"
                                                Display="None" ErrorMessage="Select Aircraft from the list." OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPerson" runat="server" ControlToValidate="txtPerson" Display="None"
                                                ErrorMessage="Person field length must not be greater than 50 Character" OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvWorkOrder" runat="server" ControlToValidate="cmbWorkOrder"
                                                Display="None" ErrorMessage="Select Work Order from the list" OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvStoreList" runat="server" ControlToValidate="cmbStoreList"
                                                Display="None" ErrorMessage="Select store from the list." OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvEnquiryDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Select Issue Date." ControlToValidate="txtIssueDate" Display="None"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" Display="None"
                                                ErrorMessage="Remark field length must not be greater than 150 Character" OnServerValidate="customvalidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlIssueDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tabDetails" border="0" width="100%">
                                                <tr>
                                                    <td align="right" colspan="6">
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# mIssue.StatusName %>"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                            <legend id="ldwodetail" runat="server"><b>Issue Details</b></legend>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblIssueDate" class="clsLabelAuto">Date</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtIssueDate" runat="server" ClientIDMode="Static" CssClass=" clsTextBoxTagSearchDate"
                                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                                            Text=""></asp:TextBox>
                                                                        <cc2:calendarextender id="IssueDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                            enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtIssueDate">
                                                                        </cc2:calendarextender>
                                                                        <cc2:textboxwatermarkextender id="IssueDateWatermarkExtender" runat="server" targetcontrolid="txtIssueDate"
                                                                            watermarkcssclass="clsDateTextBox" watermarktext="<%$AppSettings:DateFormat%>">
                                                                        </cc2:textboxwatermarkextender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblStarIssueNo" class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblNo" class="clsLabelAuto">No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtText" runat="server" CssClass=" clsTextBoxTagSearch" MaxLength="25"
                                                                                        Text="<%# mIssue.Text %>" ToolTip="Enter Text">
                                                                                    </asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass=" clsTextBoxTagSearch" Width="60px"
                                                                                        MaxLength="4" Text="<%# mIssue.No %>" ToolTip="Enter No.">
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
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbStoreList" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboSmall"
                                                                            Width="280px" DataTextField="LocationStore" DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
                                                                            SelectedValue="<%# mIssue.StoreID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px"></td>
                                                                    <td style="height: 20px">
                                                                        <asp:Label ID="lblReferenceNo" runat="server" CssClass="clsLabelAuto" Visible="False">Reference No.</asp:Label>
                                                                    </td>
                                                                    <td style="height: 20px">
                                                                        <asp:TextBox ID="txtReferenceNo" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="199" Text="<%# mIssue.ReferenceNo %>"
                                                                            TextMode="MultiLine" Visible="False">
                                                                        </asp:TextBox>
                                                                        <%-- Enabled="<%# Not mIssue.StatusID = 2 %>" Ajay--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px">&nbsp;
                                                                    </td>
                                                                    <td style="height: 20px">
                                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                    </td>
                                                                    <td style="height: 20px">
                                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="150" Rows="2" Text="<%# mIssue.Remark %>"
                                                                            TextMode="MultiLine" ToolTip="Enter Remark">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                    <td valign="top">
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                            <legend id="Legend1" runat="server"><b>Destination Details</b></legend>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <span id="lblIssueTo" class="clsLabelAuto">Issue To</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbToType" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboSmall"
                                                                            DataTextField="Type" DataValueField="ID" Enabled="<%# mIssue.IsNew and mIssue.TransTypeID = 0 %>"
                                                                            SelectedValue="<%# mIssue.ToTypeID %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblSelectDetailsStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSelectDetails" runat="server" CssClass="clsLabelAuto">Select Details</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbLocationStore" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchCombo"
                                                                            DataTextField="LocationStore" DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
                                                                            SelectedValue="<%# mIssue.ToStoreID %>" Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                            DataTextField="Name" DataValueField="ID" Enabled="<%# mIssue.IsNew %>" SelectedValue="<%# mIssue.VendorID %>"
                                                                            Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="cmbAircraftList" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchCombo"
                                                                            DataTextField="RegNo" DataValueField="ID" Enabled="<%# mIssue.IsNew %>" SelectedValue="<%# mIssue.MachineID %>"
                                                                            Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="cmbWorkShop" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchCombo"
                                                                            DataTextField="LocationWorkShop" DataValueField="ID" Enabled="<%# mIssue.IsNew %>"
                                                                            SelectedValue="<%# mIssue.WorkShopID %>" Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="cmbWorkOrder" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchCombo"
                                                                            DataTextField="WONumber" DataValueField="ID" Enabled="<%# mIssue.IsNew %>" SelectedValue="<%# mIssue.nWOID %>"
                                                                            Visible="False">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <span id="lblPerson" class="clsLabelAuto">Person</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPerson" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="25" Rows="2" Text="<%# mIssue.Person %>"
                                                                            TextMode="MultiLine" ToolTip="Enter Person ">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblAWBNo" runat="server" CssClass="clsLabelAuto" Enabled="<%# mIssue.StatusID = 1 %>"
                                                                            Text='<%# IIf(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS", "Ship Out Via", "AWB No.") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAWBNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mIssue.AWBNo %>"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="50">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblBarcodeNo" runat="server" CssClass="clsLabelAuto" Visible="False">Barcode No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtBarcodeIssue" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                                            Text="<%# mIssue.BarcodeNo %>" Visible="False">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 20px"></td>
                                                                    <td style="height: 20px">
                                                                        <asp:Label ID="lblVoucherNo" runat="server" CssClass="clsLabelAuto" Enabled="<%# mIssue.StatusID = 1 %>"
                                                                            Text='<%# IIf(AppSettings("ClientCode") = "Taj", "Requisition #", "Voucher No.") %>'></asp:Label>
                                                                    </td>
                                                                    <td style="height: 22px">
                                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mIssue.VoucherNo %>"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" MaxLength="50">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblReqEmployeeName" runat="server" CssClass="clsLabelAuto" Visible="<%# (mIssue.ToTypeID = 18) %>">Employee</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:TextBox ID="txtReqEmployeeName" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                                        Text="<%# mIssue.ReqEmployeeName %>" Visible="<%# (mIssue.ToTypeID = 18) %>">
                                                                    </asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtReqEmployeeName" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                            OnTextChanged="txtReqEmployeeName_TextChanged" AutoPostBack="true" CssClass="clsTextBox_Ajax"
                                                                            Visible="<%# (mIssue.ToTypeID = 18) %>" Enabled="<%# mIssue.StatusID = 1 %>"
                                                                            onChange="SetEmpIdonChange('txtReqEmployeeName','txtReqEmployeeName_Autocomplete')"></asp:TextBox>
                                                                        <cc2:autocompleteextender clientidmode="Static" id="txtReqEmployeeName_Autocomplete"
                                                                            runat="server" delimitercharacters="" enabled="True" completionsetcount="20"
                                                                            minimumprefixlength="0" completioninterval="1" servicepath="wfIssue_Ajax.aspx"
                                                                            servicemethod="GetEmployeeList" targetcontrolid="txtReqEmployeeName" onclientitemselected="SetID"
                                                                            usecontextkey="False" contextkey="" completionlistcssclass="ac_results_Main"
                                                                            completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
                                                                            onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
                                                                            onclientshown="ClientHiding" onclientshowing="ClientShowing">
                                                                        </cc2:autocompleteextender>
                                                                        <asp:HiddenField ID="hdnIssuedToEmployeeId" runat="server" ClientIDMode="Static" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtRemark"
                                                                            Display="None" ErrorMessage="Remark field length must not be greater than 150 Character"
                                                                            OnServerValidate="customvalidate" ValidationGroup="1"></asp:CustomValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabelAuto" Visible="<%# (mIssue.TransTypeID = FlyPal.util.Trans.IssueToCustomer) %>">Reg. 
                                                    No.</asp:Label>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mIssue.RegNo %>"
                                                                            Visible="<%# (mIssue.TransTypeID = FlyPal.util.Trans.IssueToCustomer) %>">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <%-- Sankalp 29-09-25 --%>
                                                                <%--<tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
                                                                </td>
                                                                <td align="left" valign="top">
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
                                                            </tr>--%>
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
                                <td align="left" >
                                    <asp:UpdatePanel ID="upnlIssueItem" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2" border="0">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblParts" class="clsLabelHeaderItem">Issue Item(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddItem" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click to Add New Issue Part" ValidationGroup="1" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBarcodeNos" runat="server" CssClass="clsLabelAuto" Visible="False">Barcode No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBarcodeItem" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
                                                                        onkeydown="javascript:FireOnClickButton(event);"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddBarcodeItem" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ClientIDMode="Static" ValidationGroup="1" ToolTip="Click to Add Barcode No" Visible="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgIssueItems" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                                            ShowHeaderWhenEmpty="true" CellPadding="5" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SRNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="True" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="Itemdesc" HeaderText="Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="ItemTypeName" HeaderText="Part Type">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <%-- <asp:BoundField DataField="ReceiptIntReceiptNoWeb" HeaderText="Receipt No." HtmlEncode="false">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField> <%--4--%>
                                                                <%-- <asp:BoundField DataField="ReceiptDateFormatted" HeaderText="Receipt Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField> <%--5--%>
                                                                <asp:BoundField DataField="ReceiptInfo" HeaderText="Receipt Info" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <%-- <asp:BoundField DataField="OriginalReceiptTextNo" HeaderText="Original Receipt No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField> <%--6-- --5--%>
                                                                <asp:BoundField DataField="OriginalReceiptInfo" HeaderText="Original Receipt Info"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <%--6--%>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="VendorInvoiceInfo" HeaderText="Supp. Invoice No./Date"
                                                                    HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <%--7--%>
                                                                <%--6--%>
                                                                <%-- <asp:BoundField DataField="ReleaseNoteNo" HeaderText="Release Note No.">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>--%>
                                                                <%--9--%>
                                                                <%--8--%>
                                                                <%--7--%>
                                                                <%-- <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="Release Note Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>--%>
                                                                <asp:BoundField DataField="ReleaseNoteInfo" HeaderText="Release Note Info" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <%--9--%>
                                                                <%--8--%>
                                                                <asp:TemplateField HeaderText="Qty.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                            Enabled="<%# (mIssue.StatusID =1) %>" MaxLength="8" OnTextChanged="TextChanged"
                                                                            Text='<%# DataBinder.Eval(Container.DataItem,"DisplayQty") %>'>
                                                                        </asp:TextBox>
                                                                        <asp:CustomValidator ID="cvBrokenRules" runat="server" ControlToValidate="txtQty"
                                                                            ValidationGroup="1" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--11--%><%--10--%>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="DisplayUnitName" HeaderText="Unit">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--12--%>
                                                                <%--11--%>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="DiscardAmt" HeaderText="Discard Amt.">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <%--13--%>
                                                                <%--12--%>
                                                                <%--11--%>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--14--%>
                                                                <%--13--%>
                                                                <%--12--%>
                                                                <asp:BoundField DataField="ReceiptItemBinLocation" HeaderText="Location">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--15--%>
                                                                <%--14--%>
                                                                <%--13--%>
                                                                <asp:BoundField DataField="ExpiryQtrDateInfo" HeaderText="Expiry Date/Qtrs.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--16--%>
                                                                <%--15--%>
                                                                <%--14--%>
                                                                <asp:BoundField DataField="OutGoingReleaseNoteNo" HeaderText="Outgoing Release Note No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--17--%>
                                                                <%--16--%>
                                                                <%--15--%>
                                                                <%--<asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField> <%--18-- --17-- --16--%>
                                                                <%-- <asp:BoundField DataField="Note" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField> <%--19-- -18-- --17--%>
                                                                <asp:BoundField DataField="BatchNo" HeaderText="Batch No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--20--%>
                                                                <%--19--%>
                                                                <%--18--%>
                                                                <%--16--%>
                                                                <asp:BoundField DataField="WOReturnQty" HeaderText="WO. Return Qty">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <%--21--%>
                                                                <%--20--%>
                                                                <%--19--%><%--17--%>
                                                                <asp:TemplateField HeaderText="Main.Type">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="cmbRequisitionItemTypeList" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                            Enabled="<%# mIssue.StatusID = 1 %>" DataSource="<%# mRequisitionItemTypeList %>"
                                                                            DataTextField="Name" DataValueField="ID" SelectedValue='<%# DataBinder.Eval(Container.DataItem,"RequisitionItemTypeID") %>'>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <%--22--%>
                                                                <%--21--%>
                                                                <%--20--%>
                                                                <%--18--%>
                                                                <%--<asp:ButtonField CommandName="EditRec" HeaderText="Edit" Text="Edit">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField CommandName="DeleteRec" HeaderText="Remove" Text="Remove">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                                ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                                ImageUrl="~/images/delete.png" Enabled="<%# (mIssue.StatusID =1) %>" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--23--%>
                                                                <%--22--%>
                                                                <%--21--%>
                                                                <%--19--%>
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
                                <td></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel ID="upnlIssueTerms" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table id="Table1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblTerms" class="clsLabelHeaderItem">Issue Term(s)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddTerm" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                        ToolTip="Click to Add New Term" ValidationGroup="1" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddSupplierSpecificTerms" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                        Width="200px" Text="Add Supplier Specific Terms" ToolTip="Click To Add Supplier Specific Terms"
                                                                        Visible="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblOtherDetails" runat="server" CssClass="clsLabelHeader" Visible="<%# CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart %>">Other Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="70%">
                                                        <asp:GridView ID="dgIssueTerms" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                                            ShowHeaderWhenEmpty="true" CellPadding="5" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Terms" HeaderText="Term">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--  <asp:ButtonField CommandName="DeleteRec" HeaderText="Remove" Text="Remove">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>--%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Remove" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Remove" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                            ImageUrl="~/images/delete.png" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td valign="top" width="100%">
                                                        <table id="Table5" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="clsLabel" Visible="<%# CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart %>"
                                                                        Width="128px">Total Discard Amt.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTotal" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxRightAlign_Ajax"
                                                                        ReadOnly="True" Text="<%# mIssue.TotalDiscardAmt %>" Visible="<%# CType(mIssue.TransTypeID, FlyPal.Util.Trans) = FlyPal.Util.Trans.DisacrdPart %>">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%-- Sankalp 25-08-25 --%>
                            <table>
                            <tr>
                                
                                <td>
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
                                                                                <asp:TextBox ID="txtFileName" runat="server" CssClass="clsTextBox3_Ajax" MaxLength="100"
                                                                                    ClientIDMode="Static" ToolTip="Enter File Name To Be Attached" Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
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
                                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                                        CommandName="View" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" />
                                                                                                </td>

                                                                                                <td>
                                                                                                    <asp:ImageButton ID="Remove" runat="server" CommandArgument='<%# Eval("SrNo") %>' CausesValidation="false"
                                                                                                        CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
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
                                                                    </table>
                            <%-- End --%>
                            <tr>
                                <td align="right" >
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSentToBill" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send To Bill"
                                                            ToolTip="Click to send the Issue for billing" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH1" Text="Cancel"
                                                            ToolTip="Click to Cancel the Issue" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSaveAttachment" runat="server" Text="Save Attachment" class="clsbtnH clsinfoH1"
                                                            ToolTip="Click to Save Goods Receipt and Goods Receipt Item Attachments"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnLineMaintenanceReturn" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Enabled="False" Text="Edit" ToolTip="Click to Open Issue for Line Maintenance Return" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReturnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1"
                                                            Text="Return Authorize" ToolTip="Click to Open Issue for Work Order Return" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
                                                            ClientIDMode="Static" ToolTip="Click to Send Mail" Visible="<%# (mIssue.StatusID = 2) %>"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAuthorized" runat="server" CssClass="clsbtnH clsinfoH1" Text="Authorize"
                                                            ToolTip="Click to Authorize the Issue" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" Text="Save"
                                                            ToolTip="Click to Save the Issue" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReleaseNoteNo" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="<%# Not mIssue.IsNew %>"
                                                            Text="Release Note " ToolTip="Click to Print Release Note No." Visible="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnRequistionPrint" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="<%# Not mIssue.IsNew %>"
                                                            Width="130px" Text="Requisition Print" ToolTip="Click to Print the Requisition Details"
                                                            Visible="<%# (mIssue.ToTypeID = 18 and (mIssue.TransTypeID = 14 or mIssue.TransTypeID = 44 or mIssue.TransTypeID = 59)) %>" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="<%# Not mIssue.IsNew %>"
                                                            Text="Print" ToolTip="Click to Print the Issue" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnIssueTag" runat="server" CssClass="clsbtnH clsinfoH1" Enabled="<%# Not mIssue.IsNew %>"
                                                            Visible='<%# iif(AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan"  Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "Demo" Or AppSettings("ClientCode") = "SPZ" Or  ((AppSettings("ClientCode") = "IND" or AppSettings("ClientCode") = "IRM") and mIssue.TransTypeID = 19),True,False) %>'
                                                            Text="Issue Tag" ToolTip="Click to Print Issue Tag" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            ToolTip="Click to go back to the previous page" />
                                                    </td>
                                                    <%-- Sankalp 26-09-25 --%>
                                                    <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td >
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
                                                                            <asp:Label ID="lblAlertTitle" runat="server" CssClass="clsTitleAlertLabel" ClientIDMode="Static"></asp:Label>
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
                                                                                ClientIDMode="Static"></asp:Label>
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
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;" align="right">
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
        <cc2:modalpopupextender id="mdlPopupFileUpload" runat="server" targetcontrolid="btnDummyFileUpload"
            popupcontrolid="pnlFileUpload" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
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
        <!-- End -->
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
        <script type="text/javascript">
    function CallParentCallback() {
        parent.ParentCallBackFunctionForIssue();
        return false;
    }
        </script>
        <script type="text/javascript">
    $(document).ready(function () {
              <% Dim mOpenFrom As String = Request.QueryString("Type") %>
                <% If Not mOpenFrom Is Nothing AndAlso (mOpenFrom = "FromwfStockCard" or mOpenFrom = "FromReqItemStatusReport") Then %>  
                $('#btnCancel').attr('disabled', 'disabled');
                $('#btnLineMaintenanceReturn').attr('disabled', 'disabled');
                $('#btnReleaseNoteNo').attr('disabled', 'disabled');
                $('#btnPrint').attr('disabled', 'disabled');
                $('#btnRequistionPrint').attr('disabled', 'disabled');
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
        <cc2:modalpopupextender id="mdlPopupForByMail" runat="server" targetcontrolid="btnDummyForByMail"
            popupcontrolid="pnlForByMail" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
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
        <%--Autocomplete functions to set id--%>
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
        if (source._id == "txtIssuedToEmployee_Autocomplete") {
            textbox = document.getElementById('hdnIssuedToEmployeeId');
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
                if (cntrl == "txtIssuedToEmployee") {
                    textbox = document.getElementById('hdnIssuedToEmployeeId');
                }
                textbox.value = val.toString();
                return;
            }

        }
        if (cntrl == "txtIssuedToEmployee") {
            textbox = document.getElementById('hdnIssuedToEmployeeId');
        }

        textbox.value = '';
        return;
    }
        </script>
    </form>
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        var ddCustomer = document.getElementById("cmbVendorList");
        if (ddCustomer != null) {
            if (ddCustomer.disabled == false) {
                var j = 0;
              <% For Each item2 In mVendorList%>
                <% If item2.NotInUse = "True" Then%>
                    ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
                    j = j + 1;
             <% Next%>
                }
            }
        });
    </script>
    <!-- End Highlight DropDownList Item Color-->
</body>
</html>
