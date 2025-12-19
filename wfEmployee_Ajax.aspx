<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployee_Ajax.aspx.vb" Inherits="Flypal.wfEmployee_Ajax" %>

<%--Modified by Harsh on 15th July 2024--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Details</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="tooltip.css" />
    <script type="text/javascript" src="tooltip.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.tooltip1').tooltip({
                borderColor: "DarkGrey",
                borderSize: 3
            });
        });
    </script>

    <style type="text/css">
        #arrowICN {
            cursor: pointer;
        }

        #dropdown-content {
            z-index: 7;
            position: relative;
        }

        .actionICNS {
            height: 15px;
            width: 15px;
        }

        .actionICNSEdit {
            height: 20px;
            width: 20px;
        }

        .ajax__tab_header {
            background-image: none !important;
        }

        .ajax__tab_body {
            border: 0 !important;
            border-top: 0 !important;
            border-top-color: Whitesmoke !important;
            background: none !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                        <table id="tblinner" class="clsTablelistin" border="0">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Employee [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right" colspan="3">

                                                <asp:Button ID="btnClose" runat="server"
                                                    CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                    CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <%-- **************** Ajay RND **************************--%>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlEmployeeDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <cc2:TabContainer ID="tabEmployeeDetailsContainer" runat="server" class="clstablelistin"
                                                AutoPostBack="true">
                                                <cc2:TabPanel ID="tabEmployeeDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Employee Details" ID="Label1"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <%--<table>--%>
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                                                HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                                            <asp:RequiredFieldValidator ID="rfvEmpNo" runat="server" Display="None" ControlToValidate="txtEmpNo"
                                                                                ErrorMessage="Employee No. Required." ValidationGroup="1">
                                                                            </asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="None" ControlToValidate="txtName"
                                                                                ErrorMessage="Employee Name Required." ValidationGroup="1">
                                                                            </asp:RequiredFieldValidator>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <fieldset id="fdsMonitoringDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
                                                                        <legend id="ledMonitoringDetails">
                                                                            <table>
                                                                                <tr>
                                                                                    <td colspan="1">
                                                                                        <span id="lblEmployeeDetails" class="clsLabelHeader">Employee Details
                                                                                        </span>
                                                                                        <asp:CheckBox ID="chkUseInFlightLog" runat="server" CssClass="clsCheckBox"
                                                                                            Enabled="<%# mEmployee.EmployeeCountInFlightLog = 0 %>"
                                                                                            Checked="<%# mEmployee.IsUseInFlightLog %>" Text="Flight Crew" ToolTip="Used while creating Flight logs"></asp:CheckBox>
                                                                                        <asp:CheckBox ID="chkIsTechnicalCrew" runat="server" CssClass="clsCheckBox"
                                                                                            Checked="<%# mEmployee.IsTechnicalCrew %>" Text="Technical Staff" ToolTip="For compliance and work orders"></asp:CheckBox>
                                                                                        <asp:CheckBox ID="chkIsOthers" runat="server" CssClass="clsCheckBox"
                                                                                            Checked="<%# mEmployee.IsOthers %>" Text="Others" ToolTip="For training  records only"></asp:CheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </legend>

                                                                        <table id="Table1" border="0" width="100%">
                                                                            <tr>
                                                                                <td valign="top">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="width: 14px">
                                                                                                <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                                                            </td>
                                                                                            <td width="95px">
                                                                                                <span id="lblEmpNo" class="clsLabelAuto">Emp No.</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtEmpNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.EmpNo %>"
                                                                                                    ToolTip="Enter Emp No.">
                                                                                                </asp:TextBox>
                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 14px;"></td>
                                                                                            <td>
                                                                                                <span id="lblDepartment" class="clsLabelAuto" style="width: 104px">Department</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtDepartment" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Designation Name"
                                                                                                                BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="imgDepartment" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                Width="24px" ToolTip="Click to Add New Department" CausesValidation="True"></asp:ImageButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>


                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 14px"></td>
                                                                                            <td>
                                                                                                <span id="lblDesignation" class="clsLabelAuto">Designation</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtDesignationName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.DesignationName %>"
                                                                                                                ToolTip="Designation Name" BackColor="#E0E0E0" ReadOnly="True">
                                                                                                            </asp:TextBox>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:ImageButton ID="imgDesignationName" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                Width="24px" ToolTip="Click to Add New Location" CausesValidation="True"></asp:ImageButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <span id="lblCAT" class="clsLabelAuto">CAT</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCAT" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="CAT Name"
                                                                                                    Text="<%# mEmployee.CAT %>" MaxLength="10"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="width: 14px">
                                                                                                <span id="Span1" class="clsLabelStar" style="color: Red;">*</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <span id="lblName" class="clsLabelAuto">Name</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.Name %>"
                                                                                                    ToolTip="Enter Name">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 14px">
                                                                                                <span id="Label7" class="clsLabelStar" style="color: Black;">*</span>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <span id="lblLicenceNo" class="clsLabelAuto">License No.</span>
                                                                                            </td>
                                                                                            <td valign="middle">
                                                                                                <span class="tooltip1" title="For Multiple License No(s) Add License No. separated by ,(Comma). EX. 123,ABC,XYZ ">
                                                                                                    <asp:TextBox ID="txtLicenceNo" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                                                                        Width="185px" Text="<%# mEmployee.LicenseNo %>" ToolTip="Enter License No." TextMode="MultiLine"
                                                                                                        MaxLength="500">
                                                                                                    </asp:TextBox></span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td></td>
                                                                                            <td>
                                                                                                <span id="lblGender" class="clsLabelAuto">Gender</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbGenderList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                    DataTextField="Name" SelectedValue="<%# mEmployee.GenderID %>">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td valign="top">
                                                                                    <table style="margin-top: -20px">
                                                                                        <tr>
                                                                                            <td valign="top" align="right">

                                                                                                <asp:UpdatePanel ID="upnllblEmployeeDetails" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <cc2:TabContainer ID="tabintabEmployeeDetailsContainer" runat="server" class="clstablelistin"
                                                                                                            AutoPostBack="true">

                                                                                                            <cc2:TabPanel ID="tabEmployeePhoto" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                                                                                <HeaderTemplate>
                                                                                                                    <asp:Label runat="server" Text="Employee Photo" ID="lblEmployeePhoto"></asp:Label>
                                                                                                                </HeaderTemplate>
                                                                                                                <ContentTemplate>

                                                                                                                    <table style="margin-top: -18px">
                                                                                                                        <tr>
                                                                                                                            <td valign="top">
                                                                                                                                <asp:UpdatePanel ID="upnlAttachment" runat="server" UpdateMode="Conditional">
                                                                                                                                    <ContentTemplate>
                                                                                                                                        <table>
                                                                                                                                            <tr>
                                                                                                                                                <td valign="middle" align="left">
                                                                                                                                                    <table style="z-index: 0; width: 123px; height: 112px" id="Table4" class="clsTable1"
                                                                                                                                                        border="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td valign="top">
                                                                                                                                                                <asp:Image ID="MyImage" runat="server" Width="150px" Height="150px" Style="border-radius: 50%;"></asp:Image>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                                <td valign="top">

                                                                                                                                                    <table id="Table12" border="0" style="margin-top: 40px">
                                                                                                                                                        <tr>
                                                                                                                                                            <td class="clsInnerTable">
                                                                                                                                                                <asp:ImageButton ID="btnSelectFile" runat="server" CausesValidation="False" ImageUrl="~/icons/upload.png"
                                                                                                                                                                    ToolTip="Upload Photo" Height="20px" Width="20px"></asp:ImageButton>
                                                                                                                                                                <%--<span id="lblAttachFile" class="clsLabel">Attach File</span>--%>
                                                                                                                                                                <span id="lblAttachFile" class="clsLabel">Upload</span>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td>
                                                                                                                                                                <asp:ImageButton ID="btnDelAttach" runat="server" ToolTip="Remove Photo" ImageUrl="~/images/delete.png" Style="height: 25px; width: 25px" />
                                                                                                                                                                <span id="lblRemoveFile" class="clsLabel">Remove</span>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>

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
                                                                                                            <cc2:TabPanel ID="tabEmployeeDigtalSignature" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                                                                                <HeaderTemplate>
                                                                                                                    <asp:Label runat="server" Text="Digital Signature" ID="lblEmployeeDigtalSignature"></asp:Label>
                                                                                                                </HeaderTemplate>
                                                                                                                <ContentTemplate>
                                                                                                                    <div>
                                                                                                                        <table style="margin-top: -18px">
                                                                                                                            <tr>
                                                                                                                                <td valign="middle">
                                                                                                                                    <asp:UpdatePanel ID="upnlDigitalFileupload" runat="server" UpdateMode="Conditional">
                                                                                                                                        <ContentTemplate>
                                                                                                                                            <table valign="top">
                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <table style="width: 100%">
                                                                                                                                                            <tr>
                                                                                                                                                                <td valign="middle" align="center" style="width: 100%">
                                                                                                                                                                    <table style="z-index: 0;" id="Table7" class="clsTable1" border="0">
                                                                                                                                                                        <tr>
                                                                                                                                                                            <td valign="top">
                                                                                                                                                                                <asp:Image ID="imgMyDigitalSignature" runat="server" Width="70px" Height="70px"></asp:Image>
                                                                                                                                                                            </td>
                                                                                                                                                                        </tr>
                                                                                                                                                                    </table>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>
                                                                                                                                                            <tr>
                                                                                                                                                                <td valign="middle" align="center" style="width: 100%">
                                                                                                                                                                    <span id="Span4" class="clsLabelAuto">Image Size : 640 X 480 pixel</span>
                                                                                                                                                                </td>
                                                                                                                                                            </tr>

                                                                                                                                                            <tr>
                                                                                                                                                                <td valign="middle" align="center" style="width: 100%">
                                                                                                                                                                    <table id="TableDigsig" border="0">
                                                                                                                                                                        <tr>

                                                                                                                                                                            <td>
                                                                                                                                                                                <asp:ImageButton ID="btnSelectDigitalSignature" runat="server" CausesValidation="False" ImageUrl="~/icons/upload.png"
                                                                                                                                                                                    ToolTip="Upload Photo" Height="20px" Width="20px"></asp:ImageButton>
                                                                                                                                                                                <span id="lblUploadFileSignature" class="clsLabel">Upload</span>
                                                                                                                                                                            </td>

                                                                                                                                                                            <td>
                                                                                                                                                                                <asp:ImageButton ID="btnDelDigitalAttach" runat="server" ToolTip="Remove Photo" ImageUrl="~/images/delete.png" Style="height: 25px; width: 25px" />
                                                                                                                                                                                <span id="lblRemoveFileSignature" class="clsLabel">Remove</span>
                                                                                                                                                                            </td>
                                                                                                                                                                        </tr>
                                                                                                                                                                        <tr>
                                                                                                                                                                        </tr>
                                                                                                                                                                    </table>
                                                                                                                                                                </td>
                                                                                                                                                                <td></td>
                                                                                                                                                            </tr>
                                                                                                                                                        </table>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </ContentTemplate>
                                                                                                                                    </asp:UpdatePanel>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </div>
                                                                                                                </ContentTemplate>
                                                                                                            </cc2:TabPanel>
                                                                                                        </cc2:TabContainer>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <div class="clsLabelHeader">
                                                                                        * For Multiple License No(s) Add License No. separated by ,(Comma). EX. 123,ABC,XYZ
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                        <table>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:UpdatePanel ID="upnlEmployeeDetailsContact" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <cc2:TabContainer ID="tabEmployeeDetailsContactContainer" runat="server" class="clstablelistin"
                                                                                AutoPostBack="true">
                                                                                <cc2:TabPanel ID="tabEmployeeDetailsContact" runat="server" CssClass="clsPanel1" ClientIDMode="Static">

                                                                                    <HeaderTemplate>
                                                                                        <asp:Label runat="server" Text="Contact Details" ID="lblEmployeeDetailsContact"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlValidationSummarycontact" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:CustomValidator ID="cvCountry" runat="server" Display="None" ControlToValidate="txtCountry"
                                                                                                                ErrorMessage="Enter Country" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
                                                                                                                ValidationGroup="1">
                                                                                                            </asp:CustomValidator>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>

                                                                                                <td valign="top" style="padding-top: 6px">

                                                                                                    <asp:UpdatePanel ID="upnlPermContactDetails" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <fieldset id="Fieldset5" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                                <legend id="Legend5"><b>Permanent Contact Details</b></legend>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td width="95px">
                                                                                                                            <span id="lblAddress1" class="clsLabelAuto">Address Line 1</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEmployee.Address1 %>"
                                                                                                                                ToolTip="Enter Building/Society name" TextMode="MultiLine" Width="185px">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblAddress2" class="clsLabelAuto">Address Line 2</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtAddress2" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEmployee.Address2 %>"
                                                                                                                                ToolTip="Enter Street name" TextMode="MultiLine" Width="185px">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCity" class="clsLabelAuto">City</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table6" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:DropDownList ID="cmbCityList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                                                            DataTextField="Name" SelectedValue="<%# mEmployee.CityID %>" AutoPostBack="True">
                                                                                                                                        </asp:DropDownList>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="imgCity" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                                            Width="24px" ToolTip="Click to Add New City" CausesValidation="True"></asp:ImageButton>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblZip" class="clsLabelAuto">Zip Code</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table66" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtZip" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.Zip %>"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblState" class="clsLabelAuto">State</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.StateName %>" ToolTip="State  Name" BackColor="#E0E0E0" ReadOnly="True" MaxLength="25">                                                                                            
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="Label8" class="clsLabelAuto">Country</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.CountryName %>"
                                                                                                                                ToolTip="Country  Name" BackColor="#E0E0E0" ReadOnly="True" MaxLength="25">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblPointOfOrigin" class="clsLabelAuto">Point of Origin</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtPointOfOrigin" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                                                                Text="<%# mEmployee.PointOfOrigin %>" ToolTip="Enter Point of Origin">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td></td>
                                                                                                                        <td valign="top">
                                                                                                                            <span id="Label12" class="clsLabelAuto">(Nearest Airport)</span>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 14px;"></td>
                                                                                                                        <td>
                                                                                                                            <span id="Label9" class="clsLabelAuto" style="width: 100px;">Phone No</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table9" border="0" cellspacing="1" cellpadding="1">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.PhoneNo %>"
                                                                                                                                            ToolTip="Enter Phone No" Width="100px">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span id="Label11" class="clsLabelAuto">Mobile</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.Mobile %>"
                                                                                                                                            ToolTip="Enter Mobile number" Width="102px">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr style="height: 28px;">
                                                                                                                        <td style="width: 14px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblEmail" class="clsLabelAuto">Email</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.Email %>"
                                                                                                                                ToolTip="Enter Email id">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </fieldset>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>

                                                                                                </td>

                                                                                                <td valign="top">
                                                                                                    <asp:UpdatePanel ID="upnlCurrentContactDetails" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <fieldset id="Fieldset6" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                                <legend id="Legend6">
                                                                                                                    <table>
                                                                                                                        <tr>
                                                                                                                            <td colspan="1">
                                                                                                                                <span id="Label15" class="clsLabelHeader">Current Contact Details</span>
                                                                                                                                <asp:CheckBox ID="chkSameAddress" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                                                                                                                    Text="Is same as Permanent address?"></asp:CheckBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </legend>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td width="95px">
                                                                                                                            <span id="lblCurrAddress1" class="clsLabelAuto">Address Line 1</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrAddress1" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEmployee.CurrAddress1 %>"
                                                                                                                                ToolTip="Enter Current Building/Society name" TextMode="MultiLine" Width="185px">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrAddress2" class="clsLabelAuto">Address Line 2</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrAddress2" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEmployee.CurrAddress2 %>"
                                                                                                                                ToolTip="Enter Current Street name" TextMode="MultiLine" Width="185px">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrCity" class="clsLabelAuto">City</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table5" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:DropDownList ID="cmbCurrCityList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                                                            DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployee.CurrCityID %>"
                                                                                                                                            AutoPostBack="True">
                                                                                                                                        </asp:DropDownList>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Button ID="Button3" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                                                                                            ToolTip="Click to Add New City" CausesValidation="False" Visible="False"></asp:Button>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrZip" class="clsLabelAuto">Zip Code</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table55" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtCurrZip" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.CurrZip %>"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                    <td></td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="height: 22px"></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrState" class="clsLabelAuto">State</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrState" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.CurrStateName %>"
                                                                                                                                ToolTip="Current State Name" BackColor="#E0E0E0" ReadOnly="True" MaxLength="25">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrCountry" class="clsLabelAuto">Country</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrCountry" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.CurrCountryName %>"
                                                                                                                                ToolTip="Current Country Name" BackColor="#E0E0E0" ReadOnly="True" MaxLength="25">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrPointOfOrigin" class="clsLabelAuto">Point of Origin</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrPointOfOrigin" runat="server" CssClass="clsTextBoxTagSearch"
                                                                                                                                Text="<%# mEmployee.CurrPointOfOrigin %>" ToolTip="Enter Current Point of Origin">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="Label4" class="clsLabelAuto">(Nearest Airport)</span>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrPhoneNo" class="clsLabelAuto" style="width: 68px;">Phone No</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table id="Table10" border="0" cellspacing="1" cellpadding="1">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtCurrPhoneNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.CurrPhoneNo %>"
                                                                                                                                            ToolTip="Enter Current Phone No" Width="102px">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span id="lblCurrMobile" class="clsLabelAuto">Mobile</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtCurrMobile" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.CurrMobile %>"
                                                                                                                                            ToolTip="Enter Current Mobile number" Width="102px">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblCurrEmail" class="clsLabelAuto">Email</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtCurrEmail" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.CurrEmail %>"
                                                                                                                                ToolTip="Enter Current Email id">
                                                                                                                            </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </fieldset>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>


                                                                                    </ContentTemplate>
                                                                                </cc2:TabPanel>
                                                                                <cc2:TabPanel ID="tabEmployeeOtherDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label runat="server" Text="Transaction & Other Details" ID="lblEmployeeOtherDetails"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:CustomValidator ID="cvDay" runat="server" Display="None" ControlToValidate="txtDay"
                                                                                                                            ErrorMessage="Enter Day" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
                                                                                                                            ValidationGroup="1">
                                                                                                                        </asp:CustomValidator>
                                                                                                                        <asp:CustomValidator ID="cvMonth" runat="server" Display="None" ControlToValidate="txtMonth"
                                                                                                                            ErrorMessage="Enter Month" OnServerValidate="CustomValidate" CssClass="clsLabelAuto"
                                                                                                                            ValidationGroup="1">
                                                                                                                        </asp:CustomValidator>
                                                                                                                        <asp:CustomValidator ID="cvYear" runat="server" Display="None" ControlToValidate="txtYear"
                                                                                                                            OnServerValidate="CustomValidate" CssClass="clsLabelAuto" ValidationGroup="1">
                                                                                                                        </asp:CustomValidator>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>

                                                                                                            <td valign="top">
                                                                                                                <asp:UpdatePanel ID="upnlOtherDetails" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <fieldset id="Fieldset7" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                                            <legend id="Legend7"><b>Other Details (As mentioned in Passport)</b></legend>
                                                                                                                            <table>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <span id="Label5" class="clsLabelAuto">Nationality</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtNationality" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.Nationality %>"
                                                                                                                                            ToolTip="Enter Nationality">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <span id="Label6" class="clsLabelAuto" style="width: 81px;">Date of Birth</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <table id="Table11" border="0" cellspacing="1" cellpadding="1">
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <asp:TextBox ID="txtDay" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.Day %>"
                                                                                                                                                        ToolTip="Enter Day" Width="40px">
                                                                                                                                                    </asp:TextBox>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:TextBox ID="txtMonth" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.Month %>"
                                                                                                                                                        ToolTip="Enter Month" Width="40px">
                                                                                                                                                    </asp:TextBox>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:TextBox ID="txtYear" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEmployee.Year %>"
                                                                                                                                                        ToolTip="Enter Year" Width="40px" MaxLength="4">
                                                                                                                                                    </asp:TextBox>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <span id="Label3" class="clsLabelAuto" style="width: 89px;">(DD-MM-YYYY)</span>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td colspan="2">
                                                                                                                                        <span id="Label19" class="clsLabelHeader">Hiring Agency</span>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <span id="Label18" class="clsLabelAuto">Contractor</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <table id="Table17" border="0" cellspacing="0" cellpadding="0">
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <asp:DropDownList ID="cmbContractorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                                                                        DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployee.ContractorID %>">
                                                                                                                                                    </asp:DropDownList>
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <asp:ImageButton ID="btnContractor" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                                                        Width="24px" ToolTip="Click to Add New Contractor" CausesValidation="True"></asp:ImageButton>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <%--Added by Shital As IND Requirement--%>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <span id="lblBasestation" class="clsLabelAuto">Employee Base Station </span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                                                                                                            DataValueField="ID" SelectedValue="<%# mEmployee.LocationID %>">
                                                                                                                                        </asp:DropDownList>
                                                                                                                                        <%--&nbsp;--%>
                                                                                                                                        <asp:ImageButton ID="imgLocation" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                                                                            Width="24px" ToolTip="Click to Add New Location" CausesValidation="True"></asp:ImageButton>
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
                                                                                                                <asp:UpdatePanel ID="upnlValidationSummaryEmployeeTransactionDetails" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:RegularExpressionValidator ID="cvAccoutnNo" ControlToValidate="txtAccountNo"
                                                                                                                            ValidationGroup="1" ValidationExpression="\d+" Display="None" EnableClientScript="true"
                                                                                                                            ErrorMessage="Enter only Digits in Account No" runat="server" />

                                                                                                                        <asp:RegularExpressionValidator ID="revPanNo" ControlToValidate="txtPanNo" ValidationGroup="1"
                                                                                                                            ValidationExpression="^[a-zA-Z0-9]*$" Display="None" EnableClientScript="true"
                                                                                                                            ErrorMessage="Enter Alphabets and/or Digits in PAN No" runat="server" />
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td valign="top" style="width: 100%">
                                                                                                                <asp:UpdatePanel ID="upnlEmployeeTransactionDetails" runat="server" UpdateMode="Conditional">
                                                                                                                    <ContentTemplate>
                                                                                                                        <fieldset id="Fieldset8" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                                            <legend id="Legend8"><b>Transaction Details</b></legend>
                                                                                                                            <table>
                                                                                                                                <tr>
                                                                                                                                    <td style="height: 22px"></td>
                                                                                                                                    <td>
                                                                                                                                        <span id="Span6" class="clsLabelAuto">Bank Name</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:TextBox ID="txtBankName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.BankName %>"
                                                                                                                                            ToolTip="Bank Name" MaxLength="50">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                    <td>
                                                                                                                                        <span id="Span10" class="clsLabelAuto" style="width: 68px;">Account No.</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.AccountNo %>"
                                                                                                                                                        ToolTip="Enter Account No." MaxLength="25">
                                                                                                                                                    </asp:TextBox>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td></td>
                                                                                                                                    <td>
                                                                                                                                        <span id="Span3" class="clsLabelAuto" style="width: 68px;">PAN No.</span>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <asp:TextBox ID="txtPanNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEmployee.PanNo %>"
                                                                                                                                                        ToolTip="Enter PAN No." MaxLength="20">
                                                                                                                                                    </asp:TextBox>
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
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>



                                                                                    </ContentTemplate>
                                                                                </cc2:TabPanel>

                                                                                <cc2:TabPanel ID="tabEmployeeExpatAndWorkingDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label runat="server" Text="Expat And Working Details" ID="lblEmployeeExpatAndWorkingDetails"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlEmployeeExpatAndWorkingDetailsValidationSummary" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:CustomValidator ID="cvDate" runat="server" Display="None" ControlToValidate="txtDateOfLeaving"
                                                                                                                ErrorMessage="Date should not be blank." ValidateEmptyText="true" OnServerValidate="CustomValidate"
                                                                                                                CssClass="clsLabelAuto" ValidationGroup="1">
                                                                                                            </asp:CustomValidator>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <fieldset id="Fieldset2" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                        <legend id="Legend2"><b>Expat Details</b></legend>
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td></td>
                                                                                                                <td width="95px">
                                                                                                                    <span id="Label14" class="clsLabelAuto">EXPAT</span>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CheckBox ID="chkExpatStatus" runat="server" CssClass="clsCheckBox" Checked="<%# mEmployee.ExpatStatus %>"
                                                                                                                        Text="(Check in case the Employee is Foreigner)"></asp:CheckBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </fieldset>
                                                                                                </td>

                                                                                                <td valign="top">
                                                                                                    <asp:UpdatePanel ID="upnlWorkingStatus" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <fieldset id="Fieldset3" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                                                                                <legend id="Legend3"><b>Working Status</b></legend>
                                                                                                                <table width="100%">
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="Label39" class="clsLabelAuto">Work Status</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:CheckBox ID="chkWorkingStatus" runat="server" CssClass="clsCheckBox" Checked="<%# mEmployee.IsWorking %>"
                                                                                                                                AutoPostBack="True" Text="(Check in case the Employee is Working)"></asp:CheckBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="Label17" class="clsLabelAuto" style="width: 96px; height: 11px;">Date of Leaving</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:TextBox ID="txtDateOfLeaving" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                                                                                                runat="server" onchange="ValidateDateText(this,'txtDateOfLeaving_watermarkextender');"></asp:TextBox>
                                                                                                                            <cc2:CalendarExtender ID="txtDateOfLeaving_CalendarExtender" ClientIDMode="Static"
                                                                                                                                runat="server" CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                                                                                TargetControlID="txtDateOfLeaving"></cc2:CalendarExtender>
                                                                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtDateOfLeaving" ID="txtDateOfLeaving_watermarkextender"
                                                                                                                                runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td></td>
                                                                                                                        <td>
                                                                                                                            <span id="lblContractedEmployee" class="clsLabelAuto">Contracted Employee</span>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:CheckBox ID="chkContractedEmployee" runat="server" CssClass="clsCheckBox" Checked="<%# mEmployee.IsContractedEmployee %>"
                                                                                                                                Text="(Check in case the Employee is on Contract)"></asp:CheckBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </fieldset>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </cc2:TabPanel>
                                                                            </cc2:TabContainer>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>


                                                        </table>

                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right" colspan="3">
                                                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnEmpDetails" runat="server" CssClass="clsbtnH clsinfoH1" Text="Details"
                                                                                            Visible="False" ValidationGroup="1"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ValidationGroup="1"
                                                                                            Text="Save" ToolTip="Click to save Employee"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" CausesValidation="False" ToolTip="Click to Print"></asp:Button>
                                                                                    </td>
                                                                                    <td></td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabDepartment" runat="server" CssClass="clsPanel1" ClientIDMode="Static"
                                                    Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="Employee Information" ID="lblDepartmentRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlDepartment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlEmployeeDepartmentInfoList" runat="server">
                                                                    <table id="Table13" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandDepartment" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table id="Table8" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td></td>
                                                                                                        <td align="right">
                                                                                                            <asp:UpdatePanel ID="upnlAddDeptChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnEmployeeDepartmentInfoList" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                                                                        ToolTip="Click to Add New Department Info" Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                        <!--CHK-->
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="width: 100%;">
                                                                                                    <asp:GridView ID="dgEmployeeDepartmentInfoList" runat="server"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                                                                        DataKeyNames="ID" AutoGenerateColumns="False" ShowHeader="true" ShowHeaderWhenEmpty="true"
                                                                                                        Style="width: 100%;" AllowPaging="True" PageSize="10">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="EmployeeDepartmentName" HeaderText="Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="200px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="200px" Wrap="true" />
                                                                                                            </asp:BoundField>

                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <ItemTemplate>
                                                                                                                    <div class="dropdown">
                                                                                                                        <div class="dropdownbtn-content">
                                                                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="View" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("ImageSize") > 0 %>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                        <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="Size"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabContactInfo" runat="server" CssClass="clsPanel1"
                                                    ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblContactRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlContactInfo1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlContactInfoResult" runat="server">
                                                                    <table id="Table23" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandContactInfo" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table id="Table14" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="right">
                                                                                                            <asp:UpdatePanel ID="upnlAddContactInfoChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnContactInfoAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Next To Kin Info"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                    </td>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                            <!--CHK-->
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="width: 100%;">
                                                                                                    <asp:GridView ID="dgContactInfoList" runat="server" ShowHeaderWhenEmpty="true"
                                                                                                        AutoGenerateColumns="False" ShowHeader="true" Style="width: 100%;" AllowPaging="True"
                                                                                                        DataKeyNames="ID" PageSize="10" CssClass="clsGridNewStyle" GridLines="Horizontal"
                                                                                                        CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Relation" HeaderText="Relation">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Address" HeaderText="Address">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Wrap="true" Width="170px" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="CityName" HeaderText="City">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="60px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="StateName" HeaderText="State">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="CountryName" HeaderText="Country">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="80px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="PhoneNo1" HeaderText="PhoneNo1">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="PhoneNo2" HeaderText="PhoneNo2">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Email" HeaderText="Email">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%# Eval("ImageSize") > 0%>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabDesignation" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False" Height="100%">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblDesignationRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlDesignation" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlDesignationResult" runat="server">
                                                                    <table id="Table15" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandDesignation" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <asp:UpdatePanel ID="upnlAddEmpDesgChild" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Button ID="btnDesignationAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Designation"
                                                                                                            Text="Add" CausesValidation="False"></asp:Button>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>

                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>

                                                                                                <div style="width: 100%">
                                                                                                    <asp:GridView ID="dgDesignationList" runat="server" AutoGenerateColumns="False"
                                                                                                        Width="100%" ShowHeader="true" DataKeyNames="ID,DesignationName"
                                                                                                        ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                                                        <RowStyle CssClass="clsdgItem" />
                                                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
                                                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="DesignationName" HeaderText="Designation">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderText="Promoted">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="60px" Wrap="true" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkIsPromoted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPromoted") %>'
                                                                                                                        Enabled="False"></asp:CheckBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="200px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%# Eval("ImageSize") > 0%>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn"
                                                                                                                ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabService" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblServiceRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlService" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlServiceResult" runat="server">
                                                                    <table id="Table18" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandService" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table id="Table19" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="right">
                                                                                                            <asp:UpdatePanel ID="upnlAddServiceChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnServiceAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Service"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                            <!--CHK-->
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="width: 100%">
                                                                                                    <asp:GridView ID="dgServiceList" runat="server" AutoGenerateColumns="False"
                                                                                                        DataKeyNames="ID" Style="width: 100%;" ShowHeader="True" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="EmployeeServiceDateFormatted" HeaderText="Date">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="ServiceName" HeaderText="Service Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="250px" Wrap="true" />
                                                                                                            </asp:BoundField>

                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%# Eval("ImageSize") > 0%>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>


                                                                                                            <asp:BoundField DataField="ImageSize" HeaderText="ImageSize" HeaderStyle-CssClass="hideGridColumn"
                                                                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabDocument" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblDocumentRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel runat="server" ID="upnlDocument" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlDocumentResult" runat="server">
                                                                    <table id="Table20" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandDocument" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table id="Table21" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="right">
                                                                                                            <asp:UpdatePanel runat="server" ID="upnlAddDocumentChild" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnDocumentAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Document"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="width: 100%;">
                                                                                                    <asp:GridView ID="dgDocumentList" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                                                                        Style="width: 100%;" ShowHeader="true" DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <%--0--%>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <%--1--%>
                                                                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                                                            <%--2--%>
                                                                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--3--%>
                                                                                                            <asp:BoundField DataField="DocNo" HeaderText="Document No">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--4--%>
                                                                                                            <asp:BoundField DataField="DateOfIssueFormatted" HeaderText="Date of Issue">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <%--5--%>
                                                                                                            <asp:BoundField DataField="PlaceOfIssue" HeaderText="Place of Issue">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--6--%>
                                                                                                            <asp:BoundField DataField="Validity" HeaderText="Validity">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--7--%>
                                                                                                            <asp:BoundField DataField="DateOfExpiryFormatted" HeaderText="Date of Expiry">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true"></ItemStyle>
                                                                                                            </asp:BoundField>
                                                                                                            <%--8--%>
                                                                                                            <asp:TemplateField HeaderText="Applicability">
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemStyle HorizontalAlign="Center" CssClass="TextBreak" Width="75px" Wrap="true"></ItemStyle>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                                                                        Enabled="False"></asp:CheckBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <%--9--%>
                                                                                                            <asp:BoundField DataField="IssuingAuthority" HeaderText="Issuing Authority">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--10--%>
                                                                                                            <asp:BoundField DataField="WarningDays" HeaderText="Warning Days" HeaderStyle-Wrap="true">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--11--%>
                                                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="220px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <%--12--%>
                                                                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                                                            </asp:ButtonField>
                                                                                                            <%--13--%>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%# Eval("ImageSize") > 0%>' />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="History"
                                                                                                                                            ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>'
                                                                                                                                            ToolTip="Click to View History" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="Renew" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' CommandName="Renew"
                                                                                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png"
                                                                                                                                            Visible='<%# Eval("IsApplicable") = True And Eval("OneTimeDocument") = False %>' ToolTip="Click to Renew" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <%--14--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                            <%--15--%>
                                                                                                            <asp:TemplateField HeaderText="History" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lnkDocumentHistory" runat="server" Text="History" CommandName="History"
                                                                                                                        CausesValidation="false"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <%--16--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                                                            <%--17--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="IsApplicable" HeaderText="IsApplicable"></asp:BoundField>
                                                                                                            <%--18--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="OneTimeDocument" HeaderText="OneTimeDocument"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabTraining" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblTrainingRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlTraining" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlTrainingResult" runat="server">
                                                                    <table id="Table22" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandTraining" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table id="Table24" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="upnlAddTrainingChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnTrainingAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Training"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>

                                                                                                <div>
                                                                                                    <asp:GridView ID="dgTrainingList" runat="server" AutoGenerateColumns="False"
                                                                                                        ShowHeader="True" DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="TrainingName" HeaderText="Training Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="180px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No">
                                                                                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="EmployeeTrainingDate" HeaderText="Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Duration" HeaderText="Training Duration">
                                                                                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="FreqInMonths" HeaderText="Freq In Months">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="TrainingOrgNameWithCity" HeaderText="Training Org Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="180px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="MonthOfTrainingName" HeaderText="Month Of Training" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="125px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="YearOfTraining" HeaderText="Year of Training" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="180px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderText="NOT Applicable" ItemStyle-Width="80px">
                                                                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkIsNOTApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsNotApplicable") %>'
                                                                                                                        Enabled="False"></asp:CheckBox>
                                                                                                                </ItemTemplate>
                                                                                                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="20px" Wrap="true" />
                                                                                                            </asp:ButtonField>
                                                                                                            <%--12--%>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="History"
                                                                                                                                            ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>'
                                                                                                                                            ToolTip="Click to View History" />

                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="Renew" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="Renew"
                                                                                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png" Visible='<%# Eval("IsNotApplicable") = False%>' ToolTip="Click to Renew" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <%--13--%>


                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                                                                            <%--14--%> <%--16--%>
                                                                                                            <asp:ButtonField Text="History" HeaderText="History" CommandName="History" HeaderStyle-CssClass="hideGridColumn">
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="20px" Wrap="true" />
                                                                                                            </asp:ButtonField>
                                                                                                            <%--15--%> <%--17--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                                                                            <%--16--%> <%--18--%>
                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="IsNOTApplicable" HeaderText="IsNOTApplicable"></asp:BoundField>
                                                                                                            <%--17--%> <%--19--%>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabSkill" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblSkillRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlSkill" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlSkillResult" runat="server">
                                                                    <table id="Table25" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandSkill" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table id="Table26" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="upnlAddSkillChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnSkillAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add New Skill"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>

                                                                                                <div style="width: 100%;">
                                                                                                    <asp:GridView ID="dgSkillList" runat="server" AutoGenerateColumns="False"
                                                                                                        DataKeyNames="ID" Style="width: 100%;" ShowHeader="true" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <%--'Added by Shital on 18-Aug-2016--%>
                                                                                                            <asp:BoundField DataField="SkillCode" HeaderText="Skill Code">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="SkillName" HeaderText="Skill Name">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="200px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Value" HeaderText="Value" Visible="false">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="80px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderText="Skill" Visible="false">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="50px" Wrap="true" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkIsSkill" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSkill") %>'
                                                                                                                        Enabled="False"></asp:CheckBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" Visible="false">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="Center" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server" Visible="false"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" Style="height: 20px; width: 20px" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" Style="height: 20px; width: 13px" runat="server" Visible="false"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO" />

                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>


                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="ImageSize" Visible="false"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabDisciplinary" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblDisciplinaryRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlDisciplinary" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlDisciplinaryResult" runat="server">
                                                                    <table id="Table27" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandDisciplinary" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table id="Table28" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="upnlAddDisciplinaryChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnDisciplinaryAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Disciplinary "
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                                    <asp:GridView ID="dgDisciplinaryList" runat="server" AutoGenerateColumns="False"
                                                                                                        DataKeyNames="ID" Style="width: 100%;" ShowHeader="true" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="IncidentDateFormatted" HeaderText="Incident Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="ReportedBy" SortExpression="ReportedBy" HeaderText="Reported By">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="DisciplinaryName" HeaderText="Disciplinary Action">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Comments" SortExpression="Comments" HeaderText="Comments">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="FeedBack" SortExpression="FeedBack" HeaderText="FeedBack">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%#  Eval("ImageSize") > 0 %>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabLeaves" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" ID="lblLeaveRecCount">Leave Record</asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlLeaves" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlLeaveResult" runat="server">
                                                                    <table id="Table29" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandLeaveRecord" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table id="Table30" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel ID="upnlAddLeaveChild" runat="server" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnLeaveAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Leave"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                                    <asp:GridView ID="dgLeaveRecordList" runat="server" AutoGenerateColumns="False"
                                                                                                        DataKeyNames="ID" Style="width: 100%;" ShowHeader="true" ShowHeaderWhenEmpty="true"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="ClassificationName" HeaderText="Classification">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="NoOfDays" HeaderText="No Of Days">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField Visible="False" DataField="ToDateFormatted" HeaderText="To Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="140px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="ReJoiningDateFormatted" HeaderText="Re-Joining Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="140px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="Note" HeaderText="Note">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="viewICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to View Attachment"
                                                                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                                                                            Visible='<%#  Eval("ImageSize") > 0 %>' />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                                                DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <cc2:TabPanel ID="tabCompanyEquipment" runat="server" CssClass="clsPanel1" ClientIDMode="Static" Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="" ID="lblEquipmentRecCount"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="upnlCompanyEquipment" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="pnlCompanyEquipment" runat="server">
                                                                    <table id="Table31" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlExpandCompanyEquipment" runat="server">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="right">
                                                                                                <table id="Table32" border="0" cellspacing="0" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:UpdatePanel runat="server" ID="upnlEquipmentAdd" UpdateMode="Conditional">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:Button ID="btnCompanyEquipment" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Company Equipment"
                                                                                                                        Text="Add" CausesValidation="False"></asp:Button>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 100%;">
                                                                                                    <asp:GridView ID="dgCompanyEquipmentList" runat="server" Style="width: 100%;"
                                                                                                        ShowHeader="true" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
                                                                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                                        <Columns>
                                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                            <asp:BoundField DataField="EquipmentName" HeaderText="Equipment">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="EquipmentDetails" HeaderText="Details">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="EquipmentIssuedDateFormatted" HeaderText="Issued Date">
                                                                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="EquipmentReturnedDateFormatted" HeaderText="Returned Date">
                                                                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <ItemTemplate>
                                                                                                                    <div id="dropDownImg" class="dropdown">
                                                                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                                                            ToolTip="Click to Edit record"
                                                                                                                                            CommandName="EditRec" ImageUrl="~/images/edit.png" />
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:ImageButton ID="deleteICN" class="actionICNS" runat="server"
                                                                                                                                            CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                                                            ToolTip="Click to Delete record"
                                                                                                                                            CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                            </cc2:TabContainer>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnContractor" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnimgBtnCity" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnEmpDept" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpContactInfo" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpDesg" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpService" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpDocument" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpDocumentHistory" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpTraining" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpTrainingHistory" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpSkill" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpDisciplinary" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpLeave" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpCompanyEquipment" ClientIDMode="Static" runat="server" Text="Add"
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

		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

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

            $(document).ready(function () {
                $("#btnSelectFile").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileUpload").attr("src", "wfFileUpload.aspx");

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
            function OpenDigitalSignature() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");

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
        <!-- Contractor Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyContractor" Text="Dummy Contractor" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupContractor" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupContractor" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupContractor" runat="server" TargetControlID="btnDummyContractor"
            PopupControlID="pnlPopupContractor" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameContractorStateComplete() {
                $("#btnDummyContractor").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            $(document).ready(function () {
                $("#btnContractor").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupContractor").attr("src", "wfContractor_Ajax.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyContractor").click();
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
            function ParentCallBackFunctionForContractor() {
                var ContractorWindow = $find("<%=mdlPopupContractor.ClientID %>");
                //close Contractor popup window
                ContractorWindow.hide();
                $("#iPopupContractor").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnimgBtnContractor").click();
            }
        </script>
        <!-- End-->
        <!-- City Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCity" Text="Dummy City" ClientIDMode="Static" />
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
        <!-- Date Validations -->
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

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
        <!-- End -->
        <%-- ************************** Ajay--%>
        <!-- Employee Department Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpDept" Text="Employee Department" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpDept" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpDept" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpDept" runat="server" TargetControlID="btnDummyEmpDept"
            PopupControlID="pnlEmpDept" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpDeptStateComplete() {
                $("#btnDummyEmpDept").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpDeptWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpDept").attr("src", "wfEmployeeDepartmentInfo_Ajax.aspx?Type=pup");
                    // $("#IframeKit").load(function () {
                    //                    var doc = IframeKit.window;
                    //                    IframeKit.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyEmpDept").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpDept() {
                var EmpDeptwindow = $find("<%=mdlPopupEmpDept.ClientID %>");
                //close kit popup window
                EmpDeptwindow.hide();
                //           release resources
                $("#IframeEmpDept").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnEmpDept").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Contact Info Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpContactInfo" Text="Employee Contact Info"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpContactInfo" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpContactInfo" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpContactInfo" runat="server" TargetControlID="btnDummyEmpContactInfo"
            PopupControlID="pnlEmpContactInfo" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpContactInfoStateComplete() {
                $("#btnDummyEmpContactInfo").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpContactInfoWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpContactInfo").attr("src", "wfEmployeeContactInfo_Ajax.aspx?Type=pup");


                    if (!$.browser.msie) {
                        $("#btnDummyEmpContactInfo").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpContactInfo() {
                var EmpContactInfowindow = $find("<%=mdlPopupEmpContactInfo.ClientID %>");
                //close kit popup window
                EmpContactInfowindow.hide();
                //           release resources
                $("#IframeEmpContactInfo").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnEmpContactInfo").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Designation Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpDesg" Text="Employee Designation" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpDesg" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpDesg" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpDesg" runat="server" TargetControlID="btnDummyEmpDesg"
            PopupControlID="pnlEmpDesg" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpDesgStateComplete() {
                $("#btnDummyEmpDesg").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpDesgWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpDesg").attr("src", "wfEmployeeDesignation_Ajax.aspx?Type=pup");


                    if (!$.browser.msie) {
                        $("#btnDummyEmpDesg").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpDesg() {
                var EmpDesgwindow = $find("<%=mdlPopupEmpDesg.ClientID %>");
                //close kit popup window
                EmpDesgwindow.hide();
                //           release resources
                $("#IframeEmpDesg").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnEmpDesg").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Service Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpService" Text="Employee Service" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpService" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpService" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpService" runat="server" TargetControlID="btnDummyEmpService"
            PopupControlID="pnlEmpService" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpServiceStateComplete() {
                $("#btnDummyEmpService").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpServiceWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpService").attr("src", "wfEmployeeService_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpService").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpService() {
                var EmpServicewindow = $find("<%=mdlPopupEmpService.ClientID %>");
                //close kit popup window
                EmpServicewindow.hide();
                //           release resources
                $("#IframeEmpService").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnEmpService").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Document Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpDocument" Text="Employee Document" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpDocument" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpDocument" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpDocument" runat="server" TargetControlID="btnDummyEmpDocument"
            PopupControlID="pnlEmpDocument" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpDocumentStateComplete() {
                $("#btnDummyEmpDocument").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpDocumentWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpDocument").attr("src", "wfEmployeeDocument_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpDocument").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpDocument() {
                var EmpDocumentwindow = $find("<%=mdlPopupEmpDocument.ClientID %>");
                //close kit popup window
                EmpDocumentwindow.hide();
                //           release resources
                $("#IframeEmpDocument").attr("src", "JavaScript:''");
                //call kit image button
                $("#hdnBtnEmpDocument").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Document History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpDocumentHistory" Text="Employee Document History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpDocumentHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpDocumentHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpDocumentHistory" runat="server" TargetControlID="btnDummyEmpDocumentHistory"
            PopupControlID="pnlEmpDocumentHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpDocumentHistoryStateComplete() {
                $("#btnDummyEmpDocumentHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpDocumentHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpDocumentHistory").attr("src", "wfEmployeeDocumentHistoryList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpDocumentHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpDocumentHistory() {
                var EmpDocumentHistorywindow = $find("<%=mdlPopupEmpDocumentHistory.ClientID %>");
                //close popup window
                EmpDocumentHistorywindow.hide();
                //           release resources
                $("#IframeEmpDocumentHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpDocumentHistory").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Training Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpTraining" Text="Employee Training" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpTraining" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpTraining" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpTraining" runat="server" TargetControlID="btnDummyEmpTraining"
            PopupControlID="pnlEmpTraining" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpTrainingStateComplete() {
                $("#btnDummyEmpTraining").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpTrainingWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpTraining").attr("src", "wfEmployeeTraining_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpTraining").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function OpenTrainingGroupWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';

                    $("#IframeEmpTraining").attr("src", "wfTrainingGroupSelectionList_Ajax.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyEmpTraining").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpTraining() {
                var EmpTrainingwindow = $find("<%=mdlPopupEmpTraining.ClientID %>");
                //close Training popup window
                EmpTrainingwindow.hide();
                //           release resources
                $("#IframeEmpTraining").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpTraining").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Training History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpTrainingHistory" Text="Employee Training History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpTrainingHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpTrainingHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpTrainingHistory" runat="server" TargetControlID="btnDummyEmpTrainingHistory"
            PopupControlID="pnlEmpTrainingHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpTrainingHistoryStateComplete() {
                $("#btnDummyEmpTrainingHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpTrainingHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpTrainingHistory").attr("src", "wfEmployeeTrainingHistoryList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpTrainingHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpTrainingHistory() {
                var EmpTrainingHistorywindow = $find("<%=mdlPopupEmpTrainingHistory.ClientID %>");
                //close Training popup window
                EmpTrainingHistorywindow.hide();
                //           release resources
                $("#IframeEmpTrainingHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpTrainingHistory").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Skill Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpSkill" Text="Employee Skill" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpSkill" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpSkill" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpSkill" runat="server" TargetControlID="btnDummyEmpSkill"
            PopupControlID="pnlEmpSkill" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpSkillStateComplete() {
                $("#btnDummyEmpSkill").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpSkillWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpSkill").attr("src", "wfEmployeeSkill_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpSkill").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpSkill() {
                var EmpSkillwindow = $find("<%=mdlPopupEmpSkill.ClientID %>");
                //close Skill popup window
                EmpSkillwindow.hide();
                //           release resources
                $("#IframeEmpSkill").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpSkill").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Disciplinary Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpDisciplinary" Text="Employee Disciplinary"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpDisciplinary" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpDisciplinary" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpDisciplinary" runat="server" TargetControlID="btnDummyEmpDisciplinary"
            PopupControlID="pnlEmpDisciplinary" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpDisciplinaryStateComplete() {
                $("#btnDummyEmpDisciplinary").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpDisciplinaryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpDisciplinary").attr("src", "wfEmployeeDisciplinary_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpDisciplinary").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpDisciplinary() {
                var EmpDisciplinarywindow = $find("<%=mdlPopupEmpDisciplinary.ClientID %>");
                //close Disciplinary popup window
                EmpDisciplinarywindow.hide();
                //           release resources
                $("#IframeEmpDisciplinary").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpDisciplinary").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Leave Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpLeave" Text="Employee Leave" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpLeave" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpLeave" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpLeave" runat="server" TargetControlID="btnDummyEmpLeave"
            PopupControlID="pnlEmpLeave" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpLeaveStateComplete() {
                $("#btnDummyEmpLeave").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpLeaveWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpLeave").attr("src", "wfEmployeeLeaves_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpLeave").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpLeave() {
                var EmpLeavewindow = $find("<%=mdlPopupEmpLeave.ClientID %>");
                //close Leave popup window
                EmpLeavewindow.hide();
                //           release resources
                $("#IframeEmpLeave").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpLeave").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Equipment Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpEquipment" Text="Employee Equipment" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpEquipment" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpEquipment" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupEmpEquipment" runat="server" TargetControlID="btnDummyEmpEquipment"
            PopupControlID="pnlEmpEquipment" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameEmpEquipmentStateComplete() {
                $("#btnDummyEmpEquipment").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpEquipmentWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpEquipment").attr("src", "wfCompanyEquipment_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpEquipment").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpEquipment() {
                var EmpEquipmentwindow = $find("<%=mdlPopupEmpEquipment.ClientID %>");
                //close Equipment popup window
                EmpEquipmentwindow.hide();
                //           release resources
                $("#IframeEmpEquipment").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpCompanyEquipment").click();
            }
        </script>
        <!-- End-->
        <%-- ******************* Ajay End--%>
    </form>
</body>
</html>
