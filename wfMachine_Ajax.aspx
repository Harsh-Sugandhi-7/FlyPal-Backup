<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachine_Ajax.aspx.vb"
    Inherits="Flypal.wfMachine_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aircraft Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
    <script language="JavaScript" type="text/javascript">

        function autoResizeAssemblyList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeAssemblyList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeAssemblyList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeAssemblyList').height = (newheight + 2) + "px";
            document.getElementById('IframeAssemblyList').width = (newwidth) + "px";
            document.getElementById('tbpnlAssemblyList').height = (newheight) + "px";
            document.getElementById('tbpnlAssemblyList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";


        }
        function autoResizeTankList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMachineTank').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMachineTank').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeMachineTank').height = (newheight + 2) + "px";
            document.getElementById('IframeMachineTank').width = (newwidth) + "px";
            document.getElementById('tbpnlTankList').height = (newheight) + "px";
            document.getElementById('tbpnlTankList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeFeatureList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMachineFeature').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMachineFeature').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMachineFeature').height = (newheight + 7) + "px";
            document.getElementById('IframeMachineFeature').width = (newwidth) + "px";
            document.getElementById('tbpnlFeatureList').height = (newheight) + "px";
            document.getElementById('tbpnlFeatureList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeCertificateList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMachineCertificate').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMachineCertificate').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMachineCertificate').height = (newheight + 7) + "px";
            document.getElementById('IframeMachineCertificate').width = (newwidth) + "px";
            document.getElementById('tbpnlCertificateList').height = (newheight) + "px";
            document.getElementById('tbpnlCertificateList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeCerti() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMachineCertificate').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMachineCertificate').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMachineCertificate').height = (newheight + 60) + "px";
            document.getElementById('IframeMachineCertificate').width = (newwidth) + "px";
            document.getElementById('tbpnlCertificateList').height = (newheight) + "px";
            document.getElementById('tbpnlCertificateList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeMELList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMEL').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMEL').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeMEL').height = (newheight + 5) + "px";
            document.getElementById('IframeMEL').width = (newwidth) + "px";
            document.getElementById('tbpnlMEL').height = (newheight) + "px";
            document.getElementById('tbpnlMEL').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeBoardInfoList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeBoardInfo').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeBoardInfo').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeBoardInfo').height = (newheight + 7) + "px";
            document.getElementById('IframeBoardInfo').width = (newwidth) + "px";
            document.getElementById('tbpnlBoardInfo').height = (newheight) + "px";
            document.getElementById('tbpnlBoardInfo').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizePreviousRegList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframePreviousReg').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframePreviousReg').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframePreviousReg').height = (newheight + 12) + "px";
            document.getElementById('IframePreviousReg').width = (newwidth) + "px";
            document.getElementById('tbpnlPreviousRegList').height = (newheight) + "px";
            document.getElementById('tbpnlPreviousRegList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }

        function autoResizeLeaseInfoList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeLeaseInfo').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeLeaseInfo').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeLeaseInfo').height = (newheight + 7) + "px";
            document.getElementById('IframeLeaseInfo').width = (newwidth) + "px";
            document.getElementById('tbpnlLeaseInfo').height = (newheight) + "px";
            document.getElementById('tbpnlLeaseInfo').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeLease() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeLeaseInfo').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeLeaseInfo').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeLeaseInfo').height = (newheight + 65) + "px";
            document.getElementById('IframeLeaseInfo').width = (newwidth) + "px";
            document.getElementById('tbpnlLeaseInfo').height = (newheight) + "px";
            document.getElementById('tbpnlLeaseInfo').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }

        function autoResizeMaintPolicyList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMaintPolicy').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMaintPolicy').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeMaintPolicy').height = (newheight + 5) + "px";
            document.getElementById('IframeMaintPolicy').width = (newwidth) + "px";
            document.getElementById('tbpnlMaintPolicy').height = (newheight) + "px";
            document.getElementById('tbpnlMaintPolicy').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        //        Added by bhushan 02-Aug-2016
        function autoResizeZoneConfigurationList() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeZoneConfiguration').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeZoneConfiguration').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeZoneConfiguration').height = (newheight + 5) + "px";
            document.getElementById('IframeZoneConfiguration').width = (newwidth) + "px";
            document.getElementById('tbpnlZone').height = (newheight) + "px";
            document.getElementById('tbpnlZone').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }
        function autoResizeMPDRef() {
            var newheight;
            var newwidth;
            if (document.getElementById) {
                newheight = document.getElementById('IframeMPDRef').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeMPDRef').contentWindow.document.body.scrollWidth;

            }

            document.getElementById('IframeMPDRef').height = (newheight + 50) + "px";
            document.getElementById('IframeMPDRef').width = (newwidth) + "px";
            document.getElementById('tbpnlMPDRef').height = (newheight) + "px";
            document.getElementById('tbpnlMPDRef').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <table class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMachine" runat="server" CssClass="clstitle1">Aircraft</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <cc2:TabContainer ID="TbContInst" runat="server" AutoPostBack="true" ActiveTabIndex="10">
                                                <cc2:TabPanel ID="tbpnlMachine" runat="server" CssClass="clsPanel1">
                                                    <HeaderTemplate>
                                                        Aircraft Status
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <table class="clstablelistin" id="tblinner" border="0">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a" Width="100%"></asp:ValidationSummary>
                                                                                        <asp:CustomValidator ID="cvControlValidator" runat="server" ControlToValidate="txtMaxTakeOffWt"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Maximum Take Off Weight should be non zero positive numeric value."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvEmptyWt" runat="server" ControlToValidate="txtEmptyWt"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Empty Wt should be non-zero positive Numeric value and cannot be more than Max All up Wt."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvFuelCap" runat="server" ControlToValidate="txtFuelCap"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Fuel Capacity should be non zero positive numeric value."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvmaxtakeoffwt" runat="server" ControlToValidate="txtmaxtakeoffwt"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Max Take Off Weight should be non zero positive numeric value."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvmaxlandwt" runat="server" ControlToValidate="txtmaxlandwt"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Max Landing Weight should be non zero positive numeric value."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvmaxzerofuel" runat="server" ControlToValidate="txtmaxzerofuel"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="Max Zero Fuel Weight should be non zero positive numeric value."></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvMaxAllUpWt" runat="server" ControlToValidate="txtAllUpWt"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" OnServerValidate="CustomValidate"
                                                                                            ErrorMessage="All Up Wt. should be non zero positive numeric value."></asp:CustomValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RequiredFieldValidator ID="rfvRegNo" runat="server" ControlToValidate="txtRegNo"
                                                                                            ValidationGroup="a" CssClass="clsValidationSummary" Display="None" ErrorMessage="Registration no. required"></asp:RequiredFieldValidator>
                                                                                        <asp:CustomValidator ID="cvRegNo" runat="server" ControlToValidate="txtRegNo" CssClass="clsValidationSummary"
                                                                                            Display="None" ErrorMessage="Max Length of Reg.No. is 25 char." OnServerValidate="CustomValidate"
                                                                                            ValidationGroup="a">
                                                                                        </asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvCategory" runat="server" ControlToValidate="cmbCategory"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Select Category From List"
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvOwner" runat="server" ControlToValidate="txtOwner" CssClass="clsValidationSummary"
                                                                                            Display="None" ValidationGroup="a" ErrorMessage="Max. length should be 50 char. long."
                                                                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvCustomer" runat="server" ControlToValidate="cmbCustomer"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Select Customer from the list."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvHourType" runat="server" ControlToValidate="cmbHourTypeList"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Select Hour Type from the list."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvModelList" runat="server" ControlToValidate="cmbModel"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Select model from the list."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                                                                            CssClass="clsValidationSummary" ValidationGroup="a" Display="None" ErrorMessage="Serial No required."></asp:RequiredFieldValidator>
                                                                                        <asp:CustomValidator ID="cvAsOnDate" runat="server" ControlToValidate="calFromDate"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="As On date Required if Assembly is Airframe."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="calFromDate"
                                                                                            CssClass="clsValidationSummary" Display="None" ValidationGroup="a" ErrorMessage="As On date Required if Assembly is Airframe."
                                                                                            OnServerValidate="CustomValidate"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="calFromDate"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="As On date Required if Assembly is Airframe."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvNotInUseDate" runat="server" ControlToValidate="txtNotInUseDate"
                                                                                            CssClass="clsValidationSummary" ValidationGroup="a" Display="None" ErrorMessage="Enter Not In Use Date."
                                                                                            OnServerValidate="CustomValidate" ValidateEmptyText="true"></asp:CustomValidator>
                                                                                        <asp:CustomValidator ID="cvReadOnlyDate" runat="server" ControlToValidate="txtReadOnlyDate"
                                                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Enter ReadOnly Date."
                                                                                            OnServerValidate="CustomValidate" ValidationGroup="a" ValidateEmptyText="true">
                                                                                        </asp:CustomValidator>
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
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlAircraftRegInfo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsAircraftRegInfo" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblAircraftRegDetails" style="font-weight: bold"><b>Aircraft Registration
                                                                                            Details</b></legend>
                                                                                            <table id="Table6" border="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblRegNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td style="width: 111px">
                                                                                                        <asp:Label ID="lblRegNo" runat="server" CssClass="clsLabel">Reg No.</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                                                            Text="<%# mMachine.RegNo %>" ToolTip="Enter Registration Number" Width="180px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblCategoryStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td style="width: 111px">
                                                                                                        <asp:Label ID="lblCategory" runat="server" CssClass="clsLabel">Category</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                                                            DataValueField="ID" SelectedValue="<%# mMachine.MachineCategoryID %>" Width="185px">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 111px">
                                                                                                        <asp:Label ID="lblOwner" runat="server" CssClass="clsLabel">Owner</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtOwner" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                                                            Text="<%# mMachine.Owner %>" ToolTip="Enter Owner's Name" Width="180px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblHourTypeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblHourType" runat="server" CssClass="clsLabel">Hour Type </asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbHourTypeList" runat="server" AutoPostBack="True" CssClass="clsComboBox_Ajax"
                                                                                                            DataTextField="PeriodUnitName" DataValueField="ID" SelectedValue="<%# mMachine.HourType %>"
                                                                                                            Width="185px">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="2">
                                                                                                        <asp:CheckBox ID="chkIsCustomerMachine" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                                                                            Text="If Machine is owned by Customer" Visible="False"></asp:CheckBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblCustStar" runat="server" CssClass="clsLabelStar" Visible="False">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td style="width: 111px">
                                                                                                        <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabelAuto" Height="14px" Text='<%#IIf(AppSettings("ClientCode") = "Deccan", "Operator", "Operator/Customer") %>'></asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                                                            DataValueField="ID" SelectedValue="<%# mMachine.CustomerID %>" Width="185px"
                                                                                                            ClientIDMode="Static">
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
                                                                                <asp:UpdatePanel ID="upnlAirframeInfo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsAirframeInfo" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblAirframeDetail" style="font-weight: bold"><b>Airframe Details</b></legend>
                                                                                            <table id="Table7" border="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 110px">
                                                                                                        <asp:Label ID="lblManufacturer" runat="server" CssClass="clsLabelAuto">Manufacturer</asp:Label>
                                                                                                    </td>
                                                                                                    <td colspan="2">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtManufacturer" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Manufacturer"
                                                                                                                        Text="<%# mMachine.AssemblyStatus.Assembly.Model.Manufacturer %>" ReadOnly="True"
                                                                                                                        BackColor="#E0E0E0" Width="180px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblModelStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox_Ajax" SelectedValue="<%# mMachine.AssemblyStatus.Assembly.ModelID %>"
                                                                                                                        DataValueField="ID" DataTextField="ModelName" AutoPostBack="True" BackColor="White"
                                                                                                                        Enabled="<%# mMachine.AssemblyStatus.IsModelEnabled %>" Width="185px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="imgbtnModel" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                        Width="24px" ToolTip="Click to Add New Model" CausesValidation="False"></asp:ImageButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblSerialNoStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial No."
                                                                                                                        Text="<%# mMachine.AssemblyStatus.Assembly.SerialNo %>" MaxLength="50" Width="180px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="3">
                                                                                                        <asp:Label ID="lblServiceProvider" runat="server" CssClass="clsLabelAuto">Maintenance Service Program/Provider</asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="3">
                                                                                                        <asp:TextBox ID="txtServiceProvider" runat="server" CssClass="clsTextBoxMultiLine3_Ajax"
                                                                                                            ToolTip="Enter Maintenance Service Program/Provider" Text="<%# mMachine.ServiceProvider %>"
                                                                                                            MaxLength="250" TextMode="MultiLine" DESIGNTIMEDRAGDROP="16" Width="300px"></asp:TextBox>
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
                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsWarranty" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblWarrantyDet" style="font-weight: bold"><b>Warranty Details </b></legend>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="2">
                                                                                                        <asp:CheckBox ID="chkIsUnderWarranty" runat="server" CssClass="clsCheckBox" Text="Is Aircraft Under Warranty?"
                                                                                                            AutoPostBack="True" Checked="<%# mMachine.IsUnderWarranty %>" TextAlign="Left"></asp:CheckBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblWarrantyStartDate" runat="server" Width="120px" CssClass="clsLabel">Warranty Start Date</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtWarrantyStartDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                                                            onchange="ValidateDateText(this,'txtWarrantyStartDate_watermarkextender');" TabIndex="1"
                                                                                                            Width="90px"></asp:TextBox>
                                                                                                        <cc2:CalendarExtender ID="txtWarrantyStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                            Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtWarrantyStartDate"></cc2:CalendarExtender>
                                                                                                        <cc2:TextBoxWatermarkExtender ID="txtWarrantyStartDate_watermarkextender" runat="server"
                                                                                                            ClientIDMode="Static" Enabled="True" TargetControlID="txtWarrantyStartDate" WatermarkCssClass="clsDateTextBox"
                                                                                                            WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblWarrantyEndDate" runat="server" Width="120px" CssClass="clsLabel">Warranty End Date</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtWarrantyEndDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                                                            onchange="ValidateDateText(this,'txtWarrantyEndDate_watermarkextender');" TabIndex="1"
                                                                                                            Width="90px"></asp:TextBox>
                                                                                                        <cc2:CalendarExtender ID="txtWarrantyEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                            Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtWarrantyEndDate"></cc2:CalendarExtender>
                                                                                                        <cc2:TextBoxWatermarkExtender ID="txtWarrantyEndDate_watermarkextender" runat="server"
                                                                                                            ClientIDMode="Static" Enabled="True" TargetControlID="txtWarrantyEndDate" WatermarkCssClass="clsDateTextBox"
                                                                                                            WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                    </td>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlSector" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsTechLog" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="ldTechLog">
                                                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">* </asp:Label><b>Tech
                                                                                                Log Page</b></legend>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:RadioButton Text="Single Sector" ID="rdbSingle" GroupName="a" runat="server" AutoPostBack="true"
                                                                                                            Checked="<%#Not mMachine.IsTLP %>" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:RadioButton Text="Multiple Sector" ID="rdbMulti" GroupName="a" runat="server" AutoPostBack="true"
                                                                                                            Checked="<%# mMachine.IsTLP %>" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox Text="Only Airborne Time Entry" ID="chkAirBorneTime" runat="server"
                                                                                                            Visible="<%#Not mMachine.IsTLP %>" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td valign="top">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsWeightInfo" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblTotWtAndCapacity" style="font-weight: bold"><b>Total Weight And Capacity
                                                                                            </b></legend>
                                                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblEmptyWt" runat="server" CssClass="clsLabel">Empty Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtEmptyWt" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Empty Weight" Text="<%# mMachine.EmptyWt %>" MaxLength="8" Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbEmptyWtUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    SelectedValue="<%# mMachine.EmptyWtUnitID %>" DataValueField="ID" DataTextField="Name">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAllUpWt" runat="server" CssClass="clsLabel">All Up Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtAllUpWt" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter All Up Weight" Text="<%# mMachine.MaxAllUpWt %>" MaxLength="8"
                                                                                                                    Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbAllUpWtUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mMachine.UpWtUnitID %>">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMaxGrossPayLoad" runat="server" CssClass="clsLabelAuto">Gross PayLoad</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtMaxGrossPayLoad" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Max Gross Payload" Text="<%# mMachine.MaxGrossPayload %>" MaxLength="8"
                                                                                                                    AutoPostBack="True" Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbMaxGrossPayLoadUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    SelectedValue="<%# mMachine.MaxGrossPayloadUnitID   %>" DataValueField="ID" DataTextField="Name">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMaxTaxiWt" runat="server" CssClass="clsLabel">Taxi Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtmaxtaxiwt" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Max Taxi Weight." Text="<%# mMachine.MaxTaxiWt %>" MaxLength="8"
                                                                                                                    Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbMaxTaxiUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mMachine.MaxTaxiUnitID   %>">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMaxTakeOffWt" runat="server" Width="104px" CssClass="clsLabel"
                                                                                                                    s>Take Off Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtMaxTakeOffWt" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Max Take Off Weight." Text="<%# mMachine.MaxTakeOffWt %>" MaxLength="8"
                                                                                                                    Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbMaxTakeOffUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    SelectedValue="<%# mMachine.MaxTakeOffUnitID %>" DataValueField="ID" DataTextField="Name">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMaxZeroFuel" runat="server" CssClass="clsLabelAuto">Zero Fuel Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtMaxZeroFuel" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Max Zero Fuel Weight." Text="<%# mMachine.MaxZeroFuelWt %>" MaxLength="8"
                                                                                                                    Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbMaxZeroFuelUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mMachine.MaxZeroFuelUnitID %>">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblMaxLandingWt" runat="server" CssClass="clsLabelAuto">Landing Wt.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtmaxlandwt" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ToolTip="Enter Max Landing Weight." Text="<%# mMachine.MaxLandingWt %>" MaxLength="8"
                                                                                                                    Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbMaxLandingUnit" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                                                                    SelectedValue="<%# mMachine.MaxLandingUnitID   %>" DataValueField="ID" DataTextField="Name">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFuelCap" runat="server" CssClass="clsLabel">Fuel Cap.</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtFuelCap" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                    ClientIDMode="Static" ToolTip="Enter Fuel Capacity" Text="<%# mMachine.FuelCap %>"
                                                                                                                    MaxLength="8" Width="70px"></asp:TextBox>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lblFuelCapStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="cmbUnit" runat="server" CssClass="clsComboBoxsmall_Ajax" DataTextField="Name"
                                                                                                                    DataValueField="ID" SelectedValue="<%# mMachine.UnitID %>">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </fieldset>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="100%" valign="middle">
                                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsTSNInfo" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblTimesSinceNewValuesOfAircraft" style="font-weight: bold"><b>Times Since
                                                                                            New Values of Aircraft (TSN) </b></legend>
                                                                                            <table id="Table8" border="0" width="100%">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlValidationsummaryChild" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                                                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="bb" Width="100%"></asp:ValidationSummary>
                                                                                                                <asp:CustomValidator ID="cvChildList" runat="server" ControlToValidate="cmbModel"
                                                                                                                    CssClass="clsValidationSummary" Display="None" ErrorMessage="Select model from the list."
                                                                                                                    ValidationGroup="bb"></asp:CustomValidator>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table>

                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblAsOnDateStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblAsOnDate" runat="server" CssClass="clsLabel">As On Date</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="calFromDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'calFromDate_watermarkextender');"
                                                                                                                        TabIndex="1" Width="90px"></asp:TextBox>
                                                                                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="calFromDate"></cc2:CalendarExtender>
                                                                                                                    <cc2:TextBoxWatermarkExtender ID="calFromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                                                        Enabled="True" TargetControlID="calFromDate" WatermarkCssClass="clsDateTextBox"
                                                                                                                        WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:UpdatePanel ID="upnlCurrenntValue" runat="server" UpdateMode="Conditional">
                                                                                                                        <ContentTemplate>
                                                                                                                            <asp:GridView ID="dgCurrentPeriodValue" runat="server" AutoGenerateColumns="False"
                                                                                                                                Visible="true" CssClass="clsGrid" PageSize="3" ShowHeaderWhenEmpty="true">
                                                                                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                                                <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                                                                                                <Columns>
                                                                                                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblAsOnDateStar1" runat="server" CssClass="clsLabelStar" Visible='<%# DataBinder.Eval(Container.DataItem, "PeriodID") = 2  %>'>*</asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:BoundField DataField="ManufacturingDet" HeaderText="Periods" HeaderStyle-HorizontalAlign="Left"
                                                                                                                                        ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                                                                    <asp:TemplateField HeaderText="Value" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                                                                                ToolTip="Enter corresponding Period Value." Text='<%# DataBinder.Eval(Container.DataItem, "AssemblyCurrentValueFormatted") %>'>
                                                                                                                                            </asp:TextBox>
                                                                                                                                            <asp:CustomValidator ID="cvValue" runat="server" Display="None" ControlToValidate="txtValue"
                                                                                                                                                OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left"
                                                                                                                                        ItemStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                                                                                                </Columns>
                                                                                                                            </asp:GridView>
                                                                                                                        </ContentTemplate>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:ImageButton ID="btnAddPeroid" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                        Width="24px" ToolTip="Click to Add New period" CausesValidation="False"></asp:ImageButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="100%">
                                                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset id="fdsOtherInfo" class="clsFieldSet" style="border-width: 1px">
                                                                                            <legend id="lblOtherDetails" style="font-weight: bold"><b>Other Details</b></legend>
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="chkNotInUse" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                                                                            Text="Aircraft not in use" />
                                                                                                    </td>
                                                                                                    <td valign="top">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto" Width="94px">Not In Use Date</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtNotInUseDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                                                                        onchange="ValidateDateText(this,'txtNotInUseDate_watermarkextender');" TabIndex="1"
                                                                                                                        Width="90px"></asp:TextBox>
                                                                                                                    <cc2:CalendarExtender ID="txtNotInUseDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtNotInUseDate"></cc2:CalendarExtender>
                                                                                                                    <cc2:TextBoxWatermarkExtender ID="txtNotInUseDate_watermarkextender" runat="server"
                                                                                                                        ClientIDMode="Static" Enabled="True" TargetControlID="txtNotInUseDate" WatermarkCssClass="clsDateTextBox"
                                                                                                                        WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td style="width: 220px">
                                                                                                        <asp:CheckBox ID="chkIsReadOnly" runat="server" CssClass="clsCheckBox" Text="Mark this Aircraft as ReadOnly"
                                                                                                            AutoPostBack="True" />
                                                                                                    </td>
                                                                                                    <td valign="top">
                                                                                                        <table id="Table4" align="left" border="0">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Width="94px">ReadOnly Date</asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtReadOnlyDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"
                                                                                                                        Width="90px"></asp:TextBox>
                                                                                                                    <cc2:CalendarExtender ID="txtReadOnlyDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                                        Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtReadOnlyDate"></cc2:CalendarExtender>
                                                                                                                    <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" ClientIDMode="Static"
                                                                                                                        Enabled="True" TargetControlID="txtReadOnlyDate" WatermarkCssClass="clsDateTextBox"
                                                                                                                        WatermarkText="<%$ AppSettings:DateFormat %>"></cc2:TextBoxWatermarkExtender>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="chkIsUTC" runat="server" Checked="<%# mMachine.IsUTC %>" CssClass="clsCheckBox"
                                                                                                            Text="Is Flight Log Under UTC?" />
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="3" style="padding-left: 22px; height: 27px;">
                                                                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <input type="button" id="btnSelectFile" value="Attach Dent and Buckle Chart" style="width: 190px;"
                                                                                                                                runat="server" class="clsButton_Ajax" />
                                                                                                                        </td>
                                                                                                                        <td style="padding-left: 3px;">
                                                                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                                                        </td>
                                                                                                                        <td style="padding-left: 2px;">
                                                                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
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
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="2">
                                                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table id="Table3" cellspacing="0" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" Text="Add New"
                                                                                            Visible="False"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to save the current record"
                                                                                            CausesValidation="true" Text="Save" ValidationGroup="a"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the Details"
                                                                                            Text="Print" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Back to Previous Page"
                                                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlAssemblyList" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Assembly(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlAssemblyList" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeAssemblyList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeAssemblyList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlTankList" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Tank(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMachineTank" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMachineTank" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeTankList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlFeatureList" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Feature(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMachineFeature" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMachineFeature" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeFeatureList()"></iframe>
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlCertificateList" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Certificate(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMachineCertificate" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMachineCertificate" width="100%" height="200px" scrolling="no"
                                                                    marginheight="0" frameborder="0" onload="autoResizeCertificateList()"></iframe>
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlMEL" runat="server" Visible="false" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblMEL" runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD(s)", "MEL(s)") %>'
                                                            ForeColor="Blue"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMEL" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMEL" width="100%" scrolling="no" marginheight="0" frameborder="0"
                                                                    onload="autoResizeMELList()"></iframe>
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlBoardInfo" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Board Info(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlBoardInfo" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeBoardInfo" width="100%" scrolling="no" marginheight="0" frameborder="0"
                                                                    onload="autoResizeBoardInfoList()"></iframe>
                                                                </script>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlPreviousRegList" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Previous Reg.(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlPreviousReg" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframePreviousReg" width="100%" height="200px" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizePreviousRegList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlLeaseInfo" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Leased Info(s)
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlLeaseInfo" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeLeaseInfo" width="100%" scrolling="no" marginheight="0" frameborder="0"
                                                                    onload="autoResizeLeaseInfoList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlMaintPolicy" Visible="<%# Not mMachine.IsNew %>" runat="server"
                                                    ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        Maintenance Policy
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMaintPolicy" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMaintPolicy" width="100%" scrolling="no" marginheight="0" frameborder="0"
                                                                    onload="autoResizeMaintPolicyList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <%-- Added by bhushan 02-Aug-2016--%>
                                                <cc2:TabPanel ID="tbpnlZone" runat="server" Visible="<%# Not mMachine.IsNew %>" ClientIDMode="Static">
                                                    <%--Visible="<%# Not mMachine.IsNew %>"--%>
                                                    <HeaderTemplate>
                                                        Zone Configuration
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlZoneConfiguration" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeZoneConfiguration" width="100%" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeZoneConfigurationList()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tbpnlMPDRef" runat="server" ClientIDMode="Static" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True" And Not mMachine.IsNew, True, False) %>'>

                                                    <HeaderTemplate>
                                                        MPD/AMP Revision
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlMPDRef" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <iframe id="IframeMPDRef" width="100%" scrolling="no" marginheight="0"
                                                                    frameborder="0" onload="autoResizeMPDRef()"></iframe>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                            </cc2:TabContainer>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnTankMaster" OnClientClick="CallTankList()" runat="server" CausesValidation="False"
                                                ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnMaintProgramMaster" OnClientClick="CallMaintPolicyList()" runat="server"
                                                CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnProgramTypeMaster" OnClientClick="CallMaintPolicyList()" runat="server"
                                                CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnInformationBoardMaster" OnClientClick="CallBoardInfoList()"
                                                runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;"
                                                Text="Add" />
                                            <asp:Button ID="hdnBtnFeatureMaster" OnClientClick="CallFeatureList()" runat="server"
                                                CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnAddPeriod" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnModel" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                Style="display: none;" Text="Add" />
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
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
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
        <!-- Select Model popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModel" Text="TaskCard Tool" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModel" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModel" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModel" runat="server" TargetControlID="btnDummyModel"
            PopupControlID="pnlModel" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelStateComplete() {
                $("#btnDummyModel").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModel").attr("src", "wfModel_Ajax.aspx?OpenAs=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyModel").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModel() {
                var Modelwindow = $find("<%=mdlPopupModel.ClientID %>");
                //close Task Card Tool popup window
                Modelwindow.hide();
                //           release resources
                $("#IframeModel").attr("src", "JavaScript:''");
                //call image button

                $("#hdnBtnModel").click();
            }
        </script>
        <!-- End-->
        <div>
            <!-- Period Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyAddPeriod" Text="TaskCard Step" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlAddPeriod" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
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

                        if (!$.browser.msie) {
                            $("#btnDummyAddPeriod").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

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
        </div>
        <div>
            <!-- Maint Program Master Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyMaintProgramMaster" Text="Dummy Maint Program Master"
                    ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlMaintProgramMaster" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeMaintProgramMaster" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupMaintProgramMaster" runat="server" TargetControlID="btnDummyMaintProgramMaster"
                PopupControlID="pnlMaintProgramMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMaintProgramMasterStateComplete() {
                    $("#btnDummyMaintProgramMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenMaintProgramMasterWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeMaintProgramMaster").attr("src", "wfMaintenanceProgram_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyMaintProgramMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForMaintProgramMaster() {
                    var MaintProgramMasterwindow = $find("<%=mdlPopupMaintProgramMaster.ClientID %>");
                    //close MaintProgramMaster popup window
                    MaintProgramMasterwindow.hide();
                    //           release resources
                    $("#IframeMaintProgramMaster").attr("src", "JavaScript:''");
                    //call Maint Program Master image button
                    $("#hdnBtnMaintProgramMaster").click();

                }

            </script>
            <!-- End-->
        </div>
        <div>
            <!--Program Type Master Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyProgramTypeMaster" Text="Dummy Program Type Master"
                    ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlProgramTypeMaster" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeProgramTypeMaster" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupProgramTypeMaster" runat="server" TargetControlID="btnDummyProgramTypeMaster"
                PopupControlID="pnlProgramTypeMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameProgramTypeMasterStateComplete() {
                    $("#btnDummyProgramTypeMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenProgramTypeMasterWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeProgramTypeMaster").attr("src", "wfProgramType_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyProgramTypeMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForProgramTypeMaster() {
                    var ProgramTypeMasterwindow = $find("<%=mdlPopupProgramTypeMaster.ClientID %>");
                    //close Program Type Master popup window
                    ProgramTypeMasterwindow.hide();
                    //           release resources
                    $("#IframeProgramTypeMaster").attr("src", "JavaScript:''");
                    //call Program Type Master image button
                    $("#hdnBtnProgramTypeMaster").click();

                }

            </script>
            <!-- End-->
        </div>
        <div>
            <!-- TankMaster Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyTankMaster" Text="Dummy TankMaster" ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlTankMaster" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeTankMaster" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupTankMaster" runat="server" TargetControlID="btnDummyTankMaster"
                PopupControlID="pnlTankMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameTankMasterStateComplete() {
                    $("#btnDummyTankMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenTankMasterWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeTankMaster").attr("src", "wfTank_AJAX.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyTankMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForTankMaster() {
                    var TankMasterwindow = $find("<%=mdlPopupTankMaster.ClientID %>");
                    //close TankMaster popup window
                    TankMasterwindow.hide();
                    //           release resources
                    $("#IframeTankMaster").attr("src", "JavaScript:''");
                    //call TankMaster image button
                    $("#hdnBtnTankMaster").click();

                }

            </script>
            <!-- End-->
        </div>
        <div>
            <script type="text/javascript">
                function CallAssemblyList() {
                    document.getElementById('IframeAssemblyList').src = 'wfAssemblyStatusList_Ajax.aspx'
                }
                function CallTankList() {
                    document.getElementById('IframeMachineTank').src = 'wfMachineTankList_Ajax.aspx'
                }
                function CallFeatureList() {
                    document.getElementById('IframeMachineFeature').src = 'wfMachineFeatureList_Ajax.aspx'
                }
                function CallCertificateList() {
                    document.getElementById('IframeMachineCertificate').src = 'wfMachineCertificateList_Ajax.aspx'
                }
                function CallPrevRegList() {
                    document.getElementById('IframePreviousReg').src = 'wfMachinePreviousRegDetail_AJAX.aspx'
                }
                function CallMELList() {
                    document.getElementById('IframeMEL').src = 'wfMEL_Ajax.aspx'
                }
                function CallBoardInfoList() {
                    document.getElementById('IframeBoardInfo').src = 'wfBoardInformation_Ajax.aspx'
                }
                function CallLeaseInfoList() {
                    document.getElementById('IframeLeaseInfo').src = 'wfMachineLeaseInfo_Ajax.aspx'
                }
                function CallMaintPolicyList() {
                    document.getElementById('IframeMaintPolicy').src = 'wfMachineMaintenancePolicies_Ajax.aspx'
                }
                //            Added by bhushan 02-Aug-2016
                function CallZoneConfigurationList() {
                    document.getElementById('IframeZoneConfiguration').src = 'wfMachineZoneConfiguration_Ajax.aspx'

                }
                function CallMPDAMPRef() {
                    document.getElementById('IframeMPDRef').src = 'wfMPDAMPRef.aspx'

                }
            </script>
            <script language="JavaScript" type="text/javascript">
                function CloseChildPage() {
                    $find('<%=TbContInst.ClientID%>').set_activeTabIndex(0);
                }
            </script>
        </div>
        <div>
            <!-- InformationBoardMaster Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyInformationBoardMaster" Text="Dummy InformationBoardMaster"
                    ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlInformationBoardMaster" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeInformationBoardMaster" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupInformationBoardMaster" runat="server" TargetControlID="btnDummyInformationBoardMaster"
                PopupControlID="pnlInformationBoardMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameBoardInfoMasterStateComplete() {
                    $("#btnDummyInformationBoardMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenSelectInfoBoardWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeInformationBoardMaster").attr("src", "wfSelectInformationBoard_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyInformationBoardMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForBoardInfoMaster() {
                    var InformationBoardMasterwindow = $find("<%=mdlPopupInformationBoardMaster.ClientID %>");
                    //close InformationBoardMaster popup window
                    InformationBoardMasterwindow.hide();
                    //           release resources
                    $("#IframeInformationBoardMaster").attr("src", "JavaScript:''");
                    //call InformationBoardMaster image button
                    $("#hdnBtnInformationBoardMaster").click();

                }

            </script>
            <!-- End-->
        </div>
        <div>
            <!-- FeatureMaster Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyFeatureMaster" Text="Dummy FeatureMaster"
                    ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlFeatureMaster" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeFeatureMaster" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupFeatureMaster" runat="server" TargetControlID="btnDummyFeatureMaster"
                PopupControlID="pnlFeatureMaster" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameFeatureMasterStateComplete() {
                    $("#btnDummyFeatureMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenFeatureWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeFeatureMaster").attr("src", "wfFeature_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyFeatureMaster").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForFeature() {
                    var FeatureMasterwindow = $find("<%=mdlPopupFeatureMaster.ClientID %>");
                    //close FeatureMaster popup window
                    FeatureMasterwindow.hide();
                    //           release resources
                    $("#IframeFeatureMaster").attr("src", "JavaScript:''");
                    //call FeatureMaster image button
                    $("#hdnBtnFeatureMaster").click();

                }

            </script>
            <!-- End-->
        </div>
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
    </form>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'false';
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
            var ddCustomer = document.getElementById("cmbCustomer");
            if (ddCustomer != null) {
                if (ddCustomer.disabled == false) {
                    var j = 0;
              <% For Each item2 In mCustomerList%>
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
