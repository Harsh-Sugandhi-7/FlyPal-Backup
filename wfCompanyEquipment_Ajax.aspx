<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompanyEquipment_Ajax.aspx.vb"
    Inherits="Flypal.wfCompanyEquipment_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Company Equipment</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script id="clientEventHandlersJS" language="javascript">
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
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="7" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" TabIndex="1" runat="server" CssClass="clsFormHeader">Company Equipment Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td colspan="6" align="right">
                                            <asp:UpdatePanel ID="upnalSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ValidationGroup="valGroup1"
                                                                    ToolTip="Click to Save Company Equipment Information"></asp:Button>
                                                            </td>
                                                            <td align="right">
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
                            <td colspan="7">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvEquipment" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ControlToValidate="cmbEquipment" ErrorMessage="Select equipment from the list."
                                            ClientValidationFunction="validateEquipment" ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvIssuedDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Issue date  required." Display="None" ControlToValidate="calEquipmentIssuedDate"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDetails" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Details  required." Display="None" ControlToValidate="txtEquipmentDetails"
                                            ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvReturnDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Date required"
                                            Display="None" ControlToValidate="calEquipmentReturnDate" OnServerValidate="CustomValidate"
                                            ValidationGroup="valGroup1"></asp:CustomValidator>
                                        <!-- Client side validation for comboboxes-->
                                        <script type="text/javascript">
                                            //Nomenclature
                                            function validateEquipment(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbEquipment");
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
                            <td colspan="7">
                                <asp:UpdatePanel ID="upnlEquipmentDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="7">
                                                    <span id="lblDesignationDetails" class="clsLabelHeader">Company Equipment Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabel">Employee Name</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2" align="left">
                                                    <table id="Table4" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mEmployee.Name %>"
                                                                    MaxLength="25" ToolTip=" Employee Name" ReadOnly="True" BackColor="#E0E0E0">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblEquipment" class="clsLabelAuto">Equipment</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbEquipment" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Name" SelectedValue="<%# mCompanyEquipment.EquipmentID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgEquipment" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Equipment" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgEquipment" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                      Width="24px" ToolTip="Click to Add New Equipment" CausesValidation="False"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="DetailsStar" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDetails" class="clsLabelAuto">Details</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEquipmentDetails" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle"
                                                        Text="<%# mCompanyEquipment.EquipmentDetails %>" MaxLength="1999" ToolTip="Enter Equipment Details"
                                                        TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblEquipmentIssuedDate" class="clsLabel">Issue Date</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="calEquipmentIssuedDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calEquipmentIssuedDate_CalendarExtender" runat="server"
                                                        CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calEquipmentIssuedDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calEquipmentIssuedDate" ID="Calender_watermarkextender"
                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                </td>
                                                <td>
                                                    <span id="lblEquipmentReturnDate" class="clsLabel">Return Date</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="calEquipmentReturnDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                        runat="server" CausesValidation="true" AutoPostBack="true"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="calEquipmentReturnDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calEquipmentReturnDate" ID="TextBoxWatermarkExtender1"
                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table5" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mCompanyEquipment.Remark %>"
                                                                    MaxLength="1999" ToolTip="Enter Remark" TextMode="MultiLine">
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
                        <tr>
                            <td align="right">
                            </td>
                            <%--<td colspan="6" align="right">
                                <asp:UpdatePanel ID="upnalSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ValidationGroup="valGroup1"
                                                        ToolTip="Click to Save Company Equipment Information"></asp:Button>
                                                </td>
                                                <td align="right">
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpEquipment();
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
                parent.IFrameEmpEquipmentStateComplete();
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
    <!-- Equipment Master --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEquipmentMaster" Text="Dummy Equipment Master" />
    </div>
    <asp:Panel runat="server" ID="pnlEquipmentMaster" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table3">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlEquipmentMaster" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="TABLE6" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitleEquipmentMaster" runat="server" CssClass="clsFormHeader">Equipment [New]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnNewEquipmentMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        CausesValidation="False" ToolTip="Click to add the new Equipment" Text="New"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSaveEquipmentMaster" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        ToolTip="Click to save the Equipment" Text="Save" ValidationGroup="valGroup2"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseEquipmentMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        CausesValidation="False" ToolTip="Click to close Equipment" Text="Close"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RequiredFieldValidator ID="rfvEquipment" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Equipment required" ControlToValidate="txtEquipment" Display="Dynamic"
                                                ValidationGroup="valGroup2"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Label3" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="Label4" class="clsLabel">Equipment</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEquipment" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter equipment name"
                                                Text="<%# mEquipment.Name %>" MaxLength="50">
                                            </asp:TextBox>
                                        </td>
                                        <td align="right"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                        <%--<td align="right">
                                            <asp:Button ID="sdfdf" runat="server" CssClass="clsbtnH clsinfoH"
                                                ToolTip="Click to save the Equipment" Text="Save" ValidationGroup="valGroup2">
                                            </asp:Button>

                                            <asp:Button ID="btnCloseEqfdfduipmentMaster" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                CausesValidation="False" ToolTip="Click to close Equipment" Text="Close"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Equipment List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 310px;">
                                                <table cellpadding="0" cellspacing="0" class="clsGridNewStyle" style="width: 310px;">
                                                     <%--border-collapse: collapse;"
                                                    <tr>
                                                        <td class="clsdgHeader" width="190px">
                                                            <span>Equipment</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="120px">
                                                            <span>Action</span>
                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div style="max-height: 115px; overflow-y: auto; overflow-x: hidden; width: 331px;">
                                                <asp:GridView ID="dgEquipmentList" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 310px;" DataKeyNames="ID"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="DepartmentID"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Equipment">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="190px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:ButtonField>--%>

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
                                    <tr>
                                        <%--<td colspan="4" align="right">
                                            <asp:Button ID="btnCloseEquipmentMaster" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                CausesValidation="False" ToolTip="Click to close Equipment" Text="Close"></asp:Button>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpEquipmentMaster" runat="server" TargetControlID="btnDummyEquipmentMaster"
        PopupControlID="pnlEquipmentMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End -->
    </form>
</body>
</html>
