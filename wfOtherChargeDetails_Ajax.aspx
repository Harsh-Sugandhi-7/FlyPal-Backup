<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfOtherChargeDetails_Ajax.aspx.vb"
    Inherits="Flypal.wfOtherChargeDetails_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Charge Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" title="Charge Informaton" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <table class="clstablelistin" id="tblInner">
                    <tr>
                        <td class="clsFormHeader1Newstyle">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Other Charge Detail</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>

                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ValidationGroup="1"
                                                                ToolTip="Click to Save the Charge Information"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                                CausesValidation="False"></asp:Button>
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
                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                        CssClass="clsValidationSummary" ValidationGroup="1"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvVendor" runat="server" ControlToValidate="cmbVendorList"
                                        ErrorMessage="Service Provider Required." Display="None" ValidationGroup="1"
                                        ClientValidationFunction="validateVendor"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvCurrency" runat="server" ControlToValidate="cmbCurrencyList"
                                        ErrorMessage="Currency Required." Display="None" ClientValidationFunction="validateCurrency"
                                        ValidationGroup="1"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvCharge" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbCharge"
                                        ErrorMessage="Charge Name Required." Display="None" ClientValidationFunction="validateChargeName"
                                        ValidationGroup="1"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvChargeAmount" runat="server" ControlToValidate="txtChargeAmount"
                                        ErrorMessage="Charge Amount Required" Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                        ErrorMessage="Currency factor must be greater than zero." Display="None" OnServerValidate="customvalidate"
                                        ValidationGroup="1"></asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="rfvFactor" runat="server" ControlToValidate="txtConversionFactor"
                                        ErrorMessage="Currency factor must be greater than zero." Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvServiceCharge" runat="server" CssClass="clsLabelAuto"
                                        ControlToValidate="txtCSeriveCharge" ErrorMessage="Service Charge Can not be Negative."
                                        Display="None" OnServerValidate="customvalidate" ValidationGroup="1"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvAmount" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtChargeAmount"
                                        ErrorMessage="Amount should be Greater than 0" Display="None" OnServerValidate="customvalidate"
                                        ValidationGroup="1"></asp:CustomValidator>
                                    <!-- Client side validation for comboboxes-->
                                    <script type="text/javascript">
                                        //Charge Name
                                        function validateChargeName(source, args) {
                                            args.IsValid = false;
                                            var dd = $get("cmbCharge");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;

                                            }
                                        }

                                        //Vendor
                                        function validateVendor(source, args) {
                                            args.IsValid = false;
                                            var dd = $get("cmbVendorList");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;

                                            }
                                        }
                                        //Currency
                                        function validateCurrency(source, args) {
                                            args.IsValid = false;
                                            var dd = $get("cmbCurrencyList");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;

                                            }
                                        }
                                    </script>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upnlChargeDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblOtherChargeDetails" runat="server" CssClass="clsLabelHeader">Other Charge Details</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblVendorStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Label1" class="clsLabelAuto">Service Provider</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 4px;">
                                                <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                    DataTextField="Name" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.VendorID %>">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Label3" class="clsLabelAuto">Invoice No.</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtInvNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.InvoiceNo %>"
                                                                MaxLength="50" ToolTip="Enter Invoice No.">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="Label4" class="clsLabelAuto">Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInvDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                                AutoPostBack="true" onchange="ValidateDateText(this,'InvDate_watermarkextender','false');"
                                                                Text="" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="InvDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInvDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="InvDate_watermarkextender" runat="server" TargetControlID="txtInvDate"
                                                                WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Label2" class="clsLabelAuto">Charge Type</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 4px;">
                                                <asp:DropDownList ID="cmbChargeType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                    DataTextField="OtherChargeName" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.OtherChargeTypeID %>">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblCurrencyStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Label5" class="clsLabelAuto">Currency</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlCurrency" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                        DataValueField="ID" DataTextField="Name" SelectedValue="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CurrencyID %>"
                                                                        AutoPostBack="True" Enabled="<%# mOtherCharge.OtherChargeDetails.CurrentItem.IsNew %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <span id="lblFactorStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Label6" class="clsLabelAuto">Factor</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtConversionFactor" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                                                        Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.ConversionFactor %>" MaxLength="50"
                                                                        ToolTip="Enter Conversion Factor" Enabled="<%# mOtherCharge.OtherChargeDetails.CurrentItem.IsNew %>"
                                                                        ReadOnly='<%# Session("Edit") %>'></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblChargeNameStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <table id="Table3">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="cmbCharge" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <%--<asp:Button ID="imgbtnCharge" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                ToolTip="Click to Add New Charge" CausesValidation="False"></asp:Button>--%>

                                                            <asp:ImageButton ID="imgbtnCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                            ToolTip="Click to Add New Charge" CausesValidation="False"></asp:ImageButton>


                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblChargeAmountStar1" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblChargeAmount" class="clsLabelAuto">Charge Amount </span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                    Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CAmount %>" MaxLength="12"
                                                    ToolTip="Enter Charge Amount">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Label7" class="clsLabelAuto">GST/Charge</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtCSeriveCharge" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                    Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CServiceCharges %>" MaxLength="12"
                                                    ToolTip="Enter Service Charge">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Label8" class="clsLabelAuto">Total Amount</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="clsTextBoxTagSearch" style="text-align:right"
                                                    Text="<%# mOtherCharge.OtherChargeDetails.CurrentItem.CGrandTotal %>" ReadOnly="True"
                                                    BackColor="#E0E0E0" ToolTip="Total Amount">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                            </td>
                                            <td>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <input type="button" id="btnSelectFile" value="Select File" 
                                                                        runat="server" class="clsbtnH clsinfoH1" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                        Text="Remove Attachment" Enabled="False" ></asp:Button>
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
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <%--<td align="right">
                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ValidationGroup="1"
                                                    ToolTip="Click to Save the Charge Information"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>--%>
                    </tr>
                </table>
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
    <!-- End File Upload Modal Dialog-->
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
    </form>
    <!-- Highlight DropDownList Item Color-->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var ddSupplier = document.getElementById("cmbVendorList");
             if  (ddSupplier != null) {
             var i = 0;
              if  (ddSupplier.disabled ==false)
             {
              <% For Each item1 In mVendorList%>
                <% If  item1.NotInUse ="True" Then%>
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
