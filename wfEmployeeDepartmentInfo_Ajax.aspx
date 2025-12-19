<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDepartmentInfo_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDepartmentInfo_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Department</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet">
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
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Employee Department Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSaveClose" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Employee Department Information"
                                                                    ValidationGroup="valGroup1"></asp:Button>
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
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvDepartment" runat="server" ErrorMessage="Select Department from the list."
                                            Display="None" ControlToValidate="cmbEmployeeDepartmentList" ClientValidationFunction="validateDepartmentList"
                                            ValidationGroup="valGroup1" CssClass="clsLabelAuto"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Date  Required." Display="None" ControlToValidate="txtDate" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                        <!-- Client side validation for comboboxes-->
                                        <script type="text/javascript">
                                            //Nomenclature
                                            function validateDepartmentList(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbEmployeeDepartmentList");
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
                                <asp:UpdatePanel ID="upnlEmpDeptDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblDesignationDetails" class="clsLabelHeader">Employee Department Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblEmployeeName" class="clsLabel ">Employee Name</span>
                                                </td>
                                                <td align="left">
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
                                                    <span id="lblName1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDate" class="clsLabel">Date</span>
                                                </td>
                                                <td>
                                                    <table id="Table3" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static" runat="server"
                                                                    CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender');"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtDate_CalendarExtender" ClientIDMode="Static" runat="server"
                                                                    CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Calender_watermarkextender"
                                                                    runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
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
                                                    <span id="lblDesignation" class="clsLabel">Department</span>
                                                </td>
                                                <td>
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbEmployeeDepartmentList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployeeDepartmentInfo.EmployeeDepartmentID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgDepartment" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Department" CausesValidation="False"></asp:Button>--%>
                                                                <asp:ImageButton ID="imgDepartment" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                Width="24px" ToolTip="Click to Add New Department" CausesValidation="True"></asp:ImageButton>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                </td>
                                                <td>
                                                    <table id="Table5" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass=" clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEmployeeDepartmentInfo.Remark %>"
                                                                    MaxLength="300" ToolTip="Enter Remark" Height="39px" Width="382px" TextMode="MultiLine">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="clsInnerTable">
                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
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
                          <%--  <td align="right">
                                <asp:UpdatePanel ID="upnlSaveClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to Save Employee Department Information"
                                                        ValidationGroup="valGroup1"></asp:Button>
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
                        <!--Dummy panel to open File modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- Department Master --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyDeptMaster" Text="Dummy Department Master" />
    </div>
    <asp:Panel runat="server" ID="pnlDeptMaster" Style="display: block">
        <div>
            <table class="clstablelistout" id="Table8">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlDeptMaster" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="TABLE9" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitleDeptMaster" runat="server" CssClass="clsFormHeader">Employee Department [New]</asp:Label>
                                                    </td>

                                                    <td align="right">
                                                        <table id="Table10">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnNewDeptMaster" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                        ToolTip="Click to add the new Employee Department" Text="New"></asp:Button>
                                                                </td>

                                                                <td>
                                                                    <asp:Button ID="btnSaveDeptMaster" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save the New Employee Department"
                                                                        Text="Save" ValidationGroup="valGroupChild" CausesValidation="true"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseDeptMaster" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                        Text="Close" ToolTip="Click to close Employee Department screen" />
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
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGroupChild"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Department required"
                                                ControlToValidate="txtDepartment" Display="None" ValidationGroup="valGroupChild"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>--%>
                                        <%--<td align="right">
                                            <asp:Button ID="btnNewDeptMaster" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                ToolTip="Click to add the new Employee Department" Text="New"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Label3" class="clsLabelStar">*</span>
                                        </td>
                                        <td>
                                            <span id="lblDepartment" class="clsLabel">Department</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDepartment" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Department"
                                                MaxLength="25">
                                            </asp:TextBox>
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                       


                                        <td align="right">
                                            <table id="Table10">
                                                <tr>
                                                     <td>
                                            <asp:Button ID="btnSaveDeptMaster" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to save the Employee Department Information"
                                                Text="Save" ValidationGroup="valGroupChild" CausesValidation="true"></asp:Button>
                                        </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseDeptMaster" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close Employee Department screen" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>


                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Employee Department List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 400px;">
                                                <table cellpadding="0" cellspacing="0" class="clsGrid" style="width: 400px; border-collapse: collapse;">
                                                    <tr>
                                                        <td class="clsdgHeader" width="270px">
                                                            <span>Department</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="80px">
                                                            <span>Edit/View</span>
                                                        </td>
                                                        <td class="clsdgHeader" width="50px">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <%--<div style="max-height: 200px; overflow-y: auto; overflow-x: hidden; width: 421px;">--%>
                                                <div >
                                                <asp:GridView ID="dgEmployeeDepartmentList" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeader="true" ShowHeaderWhenEmpty="true" Style="width: 400px;">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="DepartmentID" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Department">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="270px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField CommandName="EditRec" HeaderText="Edit/View" Text="Edit/View">
                                                            <ItemStyle HorizontalAlign="left" Width="80px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                            <ItemStyle HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:ButtonField>--%>

                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
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
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="4">
                                            <table id="Table10" align="right" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseDeptMaster" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close Employee Department screen" />
                                                    </td>
                                                </tr>
                                            </table>
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
    <cc2:ModalPopupExtender ID="mdlPopUpDeptMaster" runat="server" TargetControlID="btnDummyDeptMaster"
        PopupControlID="pnlDeptMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpDept();
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
                    parent.IFrameEmpDeptStateComplete();
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
    </form>
</body>
</html>
