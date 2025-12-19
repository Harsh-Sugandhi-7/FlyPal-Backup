<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfVendor_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfVendor_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Vendor Information</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table id="tblinner" class="clsTablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblVendorInfo" runat="server" CssClass="clsFormHeader">Vendor Information [New]</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvName" runat="server" Display="None" ErrorMessage="Name is too long"
                                                ControlToValidate="txtName" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAddress" runat="server" Display="None" ErrorMessage="Address is too long !"
                                                ControlToValidate="txtAddress" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCity" runat="server" Display="None" ErrorMessage="Select City from the List"
                                                ControlToValidate="cmbCity" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCatagory" runat="server" Display="None" ErrorMessage="Select At least one Catagory."
                                                ControlToValidate="txtName" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDate" runat="server" Display="None" ErrorMessage="Not In Use Date should not be Blank."
                                                ControlToValidate="txtNotInUseDate" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" Display="None" CssClass="clsLabelAuto"
                                                ErrorMessage="Enter Code" ControlToValidate="txtVendorCode"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvID" runat="server" Display="None" CssClass="clsLabelAuto"
                                                ErrorMessage="Enter ID" ControlToValidate="txtVendorID"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cv2" runat="server" Display="None" ErrorMessage="Enter Valid GSTIN First Should be 2 Digist Numbers. E.g 22"
                                                ControlToValidate="txtFirstTwoDigits" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv10" runat="server" Display="None" ErrorMessage="Enter Valid GSTIN, Enter Alphabets And Numbers Only. E.g. AAAAA0000A "
                                                ControlToValidate="txt10Characters" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv13" runat="server" Display="None" ErrorMessage="Enter Valid GSTIN, Enter Single Alphabet OR Number Only. E.g. 1 Or B"
                                                ControlToValidate="txtThirteen" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv14" runat="server" Display="None" ErrorMessage="Enter Valid GSTIN, Enter Single Alphabet Only. E.g. B"
                                                ControlToValidate="txtFourteen" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cv15" runat="server" Display="None" ErrorMessage="Enter Valid GSTIN, Enter Single Alphabet OR Number Only. E.g. 5/X "
                                                ControlToValidate="txtFifteen" OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvMailIDs" runat="server" Display="None" ControlToValidate="txtEmail"
                                                ErrorMessage="Please Enter Valid Email-ID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateEmail(field) {
                                                    var regex = /^[a-zA-Z0-9._'-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
                                                    return (regex.test(field)) ? true : false;
                                                }
                                                function validateMultipleEmailsCommaSeparated(source, args) {
                                                    var text = $("#txtEmail").val();
                                                    var seperator = ',';
                                                    if (text != '') {
                                                        var result = text.split(seperator);
                                                        for (var i = 0; i < result.length; i++) {
                                                            if (result[i] != '') {
                                                                if (!validateEmail(result[i].trim())) {
                                                                    args.IsValid = false;
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset id="fdsMonitoringDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                        <legend id="ledMonitoringDetails">
                                            <span id="lblDetails" class="clsLabelHeader">Details
                                            </span>
                                        </legend>
                                        <asp:UpdatePanel ID="upnlVendorDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%-- <asp:Panel ID="pnlVendorDetails" runat="server" CssClass="">--%>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblName1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblVendorName" Class="clsLabel" Width="98px">Name</asp:Label>
                                                        </td>
                                                        <td colspan="4">

                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Name %>"
                                                                ToolTip="Enter Vendor Name" MaxLength="100" Width="590px">
                                                            </asp:TextBox>

                                                        </td>


                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="Span1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCategory" class="clsLabel">Category</span>
                                                        </td>
                                                        <td>
                                                            <table cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkSupplier" runat="server" CssClass="clsLabelAuto" Text="Supplier"
                                                                                Checked="<%# mVendor.IsSupplier %>" Enabled="<%# (mVendor.SupplierUsedInCount)=0 %>"></asp:CheckBox>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkCustomer" runat="server" CssClass="clsLabelAuto" Text="Customer"
                                                                                Checked="<%# mVendor.IsCustomer %>" Enabled="<%# (mVendor.CustomerUsedInCount)=0 %>" Visible='<%# iif(AppSettings("IsCustomerRequire") = "True",True,False) %>'></asp:CheckBox>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkIsServiceProvider" runat="server" CssClass="clsLabelAuto" Text="Is Service Provider"
                                                                                Checked="<%# mVendor.IsServiceProvider %>" Enabled="<%# (mVendor.ServiceProviderUsedInCount)=0 %>"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCodeStar" runat="server" CssClass="clsLabelStar" Visible='<%#IIf(AppSettings("ClientCode") = "7AR", False, True) %>'>*</asp:Label>
                                                        </td>
                                                        <td>

                                                            <asp:Label ID="lblCode" runat="server" CssClass="clsLabel" Text='<%#IIf(AppSettings("ClientCode") = "7AR", "Cage Code", "Code") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtVendorCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Code %>"
                                                                ToolTip="Enter code" MaxLength="10">
                                                            </asp:TextBox>
                                                        </td>




                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblVendorIDStar" runat="server" CssClass="clsLabelStar" Visible='<%#IIf(AppSettings("ClientCode") = "7AR", True, False) %>'>*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <span id="lblVendorID" class="clsLabel">ID</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtVendorID" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.VendorID %>"
                                                                ToolTip="Enter Vendor ID" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblRepairStationCertificate" class="clsLabel">Repair Station Cert.</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRepairStationCertificate" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.RepairStationCertificate %>"
                                                                ToolTip="Enter Repair Station Certificate" MaxLength="200">
                                                            </asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblAddress1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblAddress" class="clsLabel">Address</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mVendor.Address %>"
                                                                ToolTip="Enter Address" MaxLength="500" TextMode="MultiLine">
                                                            </asp:TextBox>

                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblVendorTypeList" class="clsLabel">Vendor Type</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbVendorTypeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                SelectedValue="<%# mVendor.VendorTypeID %>" DataTextField="Name" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<span id="lblGSTINStar" class="clsLabelStar">*</span>--%>
                                                        </td>
                                                        <td>
                                                            <span id="lblGSTIN" class="clsLabel" runat="server" visible='<%#IIf(AppSettings("IsGSTApplicable") = "True" Or AppSettings("ClientCode") = "ARA", True, False) %>'>GSTIN</span>
                                                            <%--visible="<%$AppSettings:IsGSTApplicable%>"--%>
                                                        </td>
                                                        <td colspan="4">
                                                            <table runat="server" visible='<%#IIf(AppSettings("IsGSTApplicable") = "True" Or AppSettings("ClientCode") = "ARA", True, False) %>'>
                                                                <%--visible="<%$AppSettings:IsGSTApplicable%>"--%>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFirstTwoDigits" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                            MaxLength="2" ToolTip="Enter 2 digit no." Width="30px" Text="<%# mVendor.TwoDigit %>"
                                                                            Enabled="<%# (mVendor.UsedInCount) = 0 %>"></asp:TextBox>
                                                                        <span id="Span8" class="clsLabel">/</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt10Characters" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="10"
                                                                            ToolTip="Enter PAN Number" Width="95px" Text="<%# mVendor.Characters10 %>" Enabled="<%# (mVendor.UsedInCount) = 0 %>"></asp:TextBox>
                                                                        <span id="Span7" class="clsLabel">/</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtThirteen" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                            MaxLength="1" ToolTip="Enter 1 digit no." Width="20px" Text="<%# mVendor.Thirteen %>"
                                                                            Enabled="<%# (mVendor.UsedInCount) = 0 %>"></asp:TextBox>
                                                                        <span id="Span6" class="clsLabel">/</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFourteen" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                            MaxLength="1" ToolTip="Enter 1 Character" Width="20px" onkeypress="return lettersOnly(event)"
                                                                            Text="<%# mVendor.Fourteen %>" Enabled="<%# (mVendor.UsedInCount) = 0 %>"></asp:TextBox>
                                                                        <span id="Label4" class="clsLabel">/</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFifteen" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                            MaxLength="1" ToolTip="Enter 1 digit no." Width="20px" Text="<%# mVendor.Fifteen %>"
                                                                            Enabled="<%# (mVendor.UsedInCount) = 0 %>"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <span id="Label6" class="clsLabel">22</span>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span2" class="clsLabel">AAAAA0000A</span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="center">
                                                                        <span id="Span3" class="clsLabel">1</span>
                                                                    </td>
                                                                    <td align="center">
                                                                        <span id="Span4" class="clsLabel">Z</span>
                                                                    </td>
                                                                    <td align="center">
                                                                        <span id="Span5" class="clsLabel">5/X</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblCity1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblCity" class="clsLabel">City </span>
                                                        </td>
                                                        <td>
                                                            <table id="Table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbCity" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                            DataTextField="Name" DataValueField="ID" Enabled="<%# (mVendor.UsedInCount) = 0 %>">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imgCity" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                            Width="24px" ToolTip="Click to Add New City" CausesValidation="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPhone1" class="clsLabel">Phone1</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhone1" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Phone1 %>"
                                                                ToolTip="Enter Phone1" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <span id="Label1" class="clsLabel">Zip Code</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtZipCode" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Zip %>"
                                                                ToolTip="Enter Zip code" MaxLength="10">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPhone2" class="clsLabel">Phone2</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhone2" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Phone2 %>"
                                                                ToolTip="Enter Phone2" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <span id="lblState" class="clsLabel">State</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter State"
                                                                BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblPhone3" class="clsLabel">Phone3</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhone3" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Phone3 %>"
                                                                ToolTip="Enter Phone3" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblCountry" class="clsLabel">Country</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Country"
                                                                BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblFax" class="clsLabel">Fax</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFax" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Fax %>"
                                                                ToolTip="Enter Fax" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblContactPerson" runat="server" CssClass="clsLabel">Contact Person</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.ContactPerson %>"
                                                                ToolTip="Enter Contact Person" MaxLength="100">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblNatureOfVendor" class="clsLabel">Nature Of Vendor</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNatureOfVendor" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.NatureOfVendor %>"
                                                                ToolTip="Enter Nature Of Vendor" MaxLength="100">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <span id="lblEmail" class="clsLabel">Email</span>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mVendor.Email %>"
                                                                MaxLength="500" Width="590px" ClientIDMode="Static">
                                                            </asp:TextBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--</asp:Panel>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <fieldset id="fdsNotInUseDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">

                                        <asp:UpdatePanel ID="upnlNotInUseDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="Label2" Class="clsLabel">Vendor not in use</asp:Label>
                                                        </td>
                                                        <td>

                                                            <asp:CheckBox ID="chkNotInUse" runat="server" AutoPostBack="True" Checked="<%# mVendor.NotInUse %>"
                                                                CssClass="clsLabelAuto" Width="240px" />

                                                        </td>

                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNotInUseDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                                runat="server" Width="100px" onchange="ValidateDateText(this,'NotInUseDate_watermarkextender','false');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calNotInUse_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNotInUseDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtNotInUseDate" ID="NotInUseDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset id="fdsDocumentApprovalRequired" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlApprovalButtons" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkIsApprovalRequired" runat="server" AutoPostBack="True" Checked="<%# mVendor.IsApprovalRequired %>" CssClass="clsLabelAuto" Enabled="false" Text="Document Approval Required" Width="184px" />
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button ID="btnAddNewApproval" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1" Enabled="false" Text="Add New" ToolTip="Click To Add New Approval" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlApprovalRequired" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="dgApprovalList" runat="server" AllowPaging="false" AllowSorting="True" AutoGenerateColumns="False"
                                                                            PageSize="3" ShowHeaderWhenEmpty="true" ToolTip="Vendor Approval List."
                                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID" ItemStyle-CssClass="hideGridColumn">
                                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ApprovalNo" HeaderText="Approval No." SortExpression="ApprovalNo">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle Wrap="False" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <div class="dropdown">
                                                                                            <div class="dropdownbtn-content">
                                                                                                <table id="T1" class="clsGridNew_Ajax" dir="ltr">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                        </td>

                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                                                                Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="IDRenew" runat="server" ToolTip="Click to renew" CommandArgument='<%# Eval("ID") %>'
                                                                                                                CommandName="RenewRec" Style="width: 20px" ImageUrl="images/Renew1.png" Visible='<%# Not Eval("IsOneTime")%>' />
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
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                            CommandName="HistoryRec" ImageUrl="~/images/History.png" Visible='<%#  Eval("HasHistory")%>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn" HeaderText="Size" ItemStyle-CssClass="hideGridColumn" />
                                                                                <asp:BoundField DataField="SortNo" HeaderStyle-CssClass="hideGridColumn" HeaderText="SortNo" ItemStyle-CssClass="hideGridColumn" />
                                                                                <asp:BoundField DataField="IsOneTime" HeaderStyle-CssClass="hideGridColumn" HeaderText="Size" ItemStyle-CssClass="hideGridColumn" />
                                                                                <asp:BoundField DataField="HasHistory" HeaderStyle-CssClass="hideGridColumn" HeaderText="HasHistory"
                                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
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
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlSaveNClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="tblButton">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" AccessKey="S" CssClass="clsbtnH clsinfoH1" Text="Save"
                                                            ToolTip="Click to save vendor" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                            Text="Close" ToolTip="Click to close" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnCity" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
        <!-- Vendor Approval Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyVendorApproval" Text="VendorApprova" ClientIDMode="Static" />
            <asp:Button ID="hdnBtnVendorApproval" ClientIDMode="Static" runat="server" Text="Add"
                CausesValidation="False" Style="display: none;"></asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlVendorApproval" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeVendorApproval" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupVendorApproval" runat="server" TargetControlID="btnDummyVendorApproval"
            PopupControlID="pnlVendorApproval" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameVendorApprovalStateComplete() {
                $("#btnDummyVendorApproval").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenVendorApprovalWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeVendorApproval").attr("src", "wfVendorApproval_Ajax.aspx?Type=pup");
                    //                if (!$.browser.msie) {
                    $("#btnDummyVendorApproval").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                    //                }
                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForVendorApproval() {
                var VendorApprovalwindow = $find("<%=mdlPopupVendorApproval.ClientID %>");
                //close Vendor Approval popup window
                VendorApprovalwindow.hide();
                //release resources
                $("#IframeVendorApproval").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnVendorApproval").click();
            }
        </script>
        <!-- End-->
        <%--Approval History--%>
        <asp:Panel runat="server" ID="pnlApprovalHistory" CssClass="clspanel1">
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyApprovalHistory" Text="Last 10 Purchases" />
            </div>
            <div style="width: 100%">
                <asp:UpdatePanel runat="server" ID="upnlApprovalHistory" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="clstablelistin" id="Table3">
                            <tr>
                                <td>
                                    <asp:GridView ID="dgApprovalHistoryList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                        PageSize="3" ShowHeaderWhenEmpty="true"
                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                ItemStyle-CssClass="hideGridColumn">
                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ApprovalNo" SortExpression="ApprovalNo" HeaderText="Approval No.">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <%--<asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                            <asp:BoundField DataField="SortNo" HeaderText="SortNo" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnApprovalHistoryClose" runat="server" CssClass="clsbtnH clsinfoH1"
                                        ToolTip="Click to go back to the previous page" Text="Back" CausesValidation="False"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender runat="server" ID="mdeApprovalHistory" TargetControlID="btnDummyApprovalHistory"
            PopupControlID="pnlApprovalHistory" BackgroundCssClass="clsModalPopupBGForSecondPage">
        </cc2:ModalPopupExtender>
        <%--End Of Approval History--%>
        <!-- City Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCity" Text="Dummy City" ClientIDMode="Static"
                CausesValidation="false" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupCity" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupCity" frameborder="0" allowtransparency="true" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCity" runat="server" TargetControlID="btnDummyCity"
            PopupControlID="pnlPopupCity" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCityStateComplete() {
                $("#btnDummyCity").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            $(document).ready(function () {
                $("#imgCity").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupCity").attr("src", "wfCityInv_Ajax.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyCity").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                });
            });
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var CityWindow = $find("<%=mdlPopupCity.ClientID %>");
                //close City popup window
                CityWindow.hide();
                $("#iPopupCity").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnCity").click();
            }
        </script>
        <!-- End-->
        <script type="text/javascript">
            function lettersOnly() {
                var charCode = event.keyCode;
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)
                    return true;
                else
                    alert("Enter Alphabet(s) Only ...");
                return false;
            }
        </script>
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
</body>
</html>
