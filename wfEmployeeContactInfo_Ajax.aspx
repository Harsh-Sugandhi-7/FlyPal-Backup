<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeContactInfo_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeContactInfo_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Next To Kin Information</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Employee Next To Kin Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Next To Kin Information"
                                                                    Text="Save" ValidationGroup="valGroup1"></asp:Button>
                                                            </td>
                                                            <td align="right">
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
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" Width="440px" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Contact Person Name Required."
                                            Display="None" ControlToValidate="txtName" ValidationGroup="valGroup1">Contact Person Name Required</asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvRelation" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Relation Required." Display="None" ControlToValidate="txtRelation"
                                            ValidationGroup="valGroup1">Relation Required</asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select City from the list."
                                            Display="None" ControlToValidate="cmbCityInvList" ClientValidationFunction="validateCity"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name must not be greater than 50 characters."
                                            Display="None" ControlToValidate="txtName" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRelationLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Relation must not be greater than 25 characters."
                                            Display="None" ControlToValidate="txtRelation" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAddr1Len" runat="server" CssClass="clsLabelAuto" ErrorMessage="Building / Society address must not be greater than 250 characters."
                                            Display="None" ControlToValidate="txtAddress1" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAddr2Len" runat="server" CssClass="clsLabelAuto" ErrorMessage="Street Name must not be greater than 250 characters."
                                            Display="None" ControlToValidate="txtAddress2" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvAddr3Len" runat="server" CssClass="clsLabelAuto" ErrorMessage="Area / Landmark must not be greater than 250 characters."
                                            Display="None" ControlToValidate="txtAddress3" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPhone1Len" runat="server" CssClass="clsLabelAuto" ErrorMessage="Phone1 must not be greater than 20 characters."
                                            Display="None" ControlToValidate="txtPhone1" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvPhone2Len" runat="server" CssClass="clsLabelAuto" ErrorMessage="Phone2 must not be greater than 20 characters."
                                            Display="None" ControlToValidate="txtPhone2" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvMobLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Mobile Number must not be greater than 50 characters."
                                            Display="None" ControlToValidate="txtMobile" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvMailLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Email must not be greater than 50 characters."
                                            Display="None" ControlToValidate="txtEmail" ClientValidationFunction="validateName"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                                                              
                                                                             
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <script type="text/javascript">
                                    function validateCity(source, args) {
                                        args.IsValid = false;
                                       
                                        var dd = $get("cmbCityInvList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }

                                    function validateName(source, args) {
                                        //args.IsValid = false;
                                        var ControlName = source.controltovalidate;
                                        switch (ControlName) {
                                            case 'txtName':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 50) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtRelation':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 25) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtAddress1':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 250) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtAddress2':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 250) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtAddress3':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 250) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtPhone1':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 20) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtPhone2':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 20) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtMobile':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 50) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtEmail':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 50) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlContactInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3" >
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lblDocumentDetails" class="clsLabelHeader">Employee Document Detail</span>
                                                            </td>
                                                        </tr>
                                                    </table>


                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="15px">
                                                </td>
                                                <td width="110px">
                                                    <span id="lblEmployeeName" class="clsLabelAuto">Employee Name</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxSearch_Ajax"
                                                        MaxLength="25" ToolTip="Employee Name" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mEmployee.Name %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblName" class="clsLabelAuto">Name</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxSearch_Ajax" ToolTip="Enter Name (Upto 50 characters)"
                                                        Text="<%# mEmployeeContactInfo.Name %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblRelation" class="clsLabelAuto">Relation</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRelation" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Relation (Upto 25 characters)"
                                                        Text="<%# mEmployeeContactInfo.Relation %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <span id="lblContactDetails" class="clsLabelHeader">Contact Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAddress1" class="clsLabelAuto">Building / Society</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Building/Society (Upto 250 characters)"
                                                        Text="<%# mEmployeeContactInfo.Address1 %>" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAddress2" class="clsLabelAuto">Street Name</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress2" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Street Name (Upto 250 characters)"
                                                        Text="<%# mEmployeeContactInfo.Address2 %>" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAddress3" class="clsLabelAuto">Area / Landmark</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAddress3" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Area/Landmark (Upto 250 characters) "
                                                        Text="<%# mEmployeeContactInfo.Address3 %>" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:UpdatePanel ID="upnlCity" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center" width="11px">
                                                                        <span id="Label2" class="clsLabelStar" style="color: Red;">*</span>
                                                                    </td>
                                                                    <td width="110px">
                                                                        <span id="lblCity" class="clsLabelAuto">City</span>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                            <tr>
                                                                                <td align="right">
                                                                                      <asp:DropDownList ID="cmbCityInvList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                        SelectedValue="<%# mEmployeeContactInfo.CityID %>" DataTextField="Name" DataValueField="ID"
                                                                                        AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <%--<asp:Button ID="imgCity" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                                        ToolTip="Click to Add New Document" CausesValidation="False"></asp:Button>--%>
                                                                                    <asp:ImageButton ID="imgCity" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                                        Width="24px" ToolTip="Click to Add New City" CausesValidation="True"></asp:ImageButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Label4" class="clsLabelAuto">State</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtState" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                            ToolTip="State  Name" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mEmployeeContactInfo.StateName %>">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Label8" class="clsLabelAuto">Country</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25"
                                                                            ToolTip="Country  Name" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mEmployeeContactInfo.CountryName %>">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label9" class="clsLabelAuto">Phone1</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhone1" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Phone 1 (Upto 20 characters)"
                                                        Text="<%# mEmployeeContactInfo.PhoneNo1 %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label10" class="clsLabelAuto">Phone2</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhone2" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Phone 2 (Upto 20 characters)"
                                                        Text="<%# mEmployeeContactInfo.PhoneNo2 %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label11" class="clsLabelAuto">Mobile</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Mobile number (Upto 50 characters)"
                                                        Text="<%# mEmployeeContactInfo.Mobile %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmail" class="clsLabelAuto">Email</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="clsTextBoxSearch_Ajax" ToolTip="Enter Email (Upto 50 characters)"
                                                        Text="<%# mEmployeeContactInfo.Email %>" Height="16px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblAttach" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                    class="clsbtnH clsinfoH1">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                            </td>
                                                            <td style="padding-left: 2px;">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                    Height="20px" Width="20px"></asp:ImageButton>
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
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Next To Kin Information"
                                                        Text="Save" ValidationGroup="valGroup1"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                        Text="Back" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <!--Dummy panel to open File Upload modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnCity" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
                                            Style="display: none;"></asp:Button>
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
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
    <!-- City Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCity" Text="Dummy City" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupCity" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpContactInfo();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
            <% Dim mopen As String = Request.QueryString("Type") %>
            <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
               SetPageLayout();
                 if ($.browser.msie) {
                     parent.IFrameEmpContactInfoStateComplete();
                 }
       
      
            });
            <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
             }

               function SetPageLayout()
               {
               <% Dim mopenas As String = Request.QueryString("Type") %>
                  <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                  ReSetPageLayout();
                  onResize();//for Top bottom link
                   <% End if %>
               }
               function ReSetPageLayout()
               {
               $("body,html").css({ 'background-color': 'transparent' });
                  var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
                  var windowheight=$(window).height();
                  if (tempMargtop>=windowheight)
                  {
                    $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
                  }
                  else
                  {
                  var margintop=(windowheight/2)-(tempMargtop/2);
                   $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                  }
       
               }
    </script>
    <%--End--%>
    </form>
</body>
</html>
