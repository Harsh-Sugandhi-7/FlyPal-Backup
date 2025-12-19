<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDesignation_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDesignation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html> 
<head runat="server">  
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Designation</title>
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
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
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
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Employee Designation Information [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Employee Designation Information"
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
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="valGroup1"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvDesignation" runat="server" ErrorMessage="Select Designation from the list."
                                    Display="None" ControlToValidate="cmbDesignationList" ClientValidationFunction="validateDesignation"
                                    ValidationGroup="valGroup1"></asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="Date Required." Display="None" ControlToValidate="txtDate" ValidationGroup="valGroup1"></asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function validateDesignation(source, args) {
                                        args.IsValid = false;
                                        var dd = $get("cmbDesignationList");
                                        if (dd.selectedIndex != 0) {
                                            args.IsValid = true;
                                            return;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDesgDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblDesignationDetails" class="clsLabelHeader">Employee Designation Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10px">
                                                </td>
                                                <td width="100px">
                                                    <span id="lblEmployeeName" class="clsLabel">Employee Name</span>
                                                </td>
                                                <td align="left">
                                                    <table id="Table4" cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                                <td align="left">
                                                    <span id="lblDate" class="clsLabel">Date</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static" runat="server"
                                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                                        </cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="Calender_watermarkextender"
                                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox">
                                                                        </cc2:TextBoxWatermarkExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span id="Label1" class="clsLabelStar" style="color: Red;">*</span>
                                                </td>
                                                <td align="left">
                                                    <span id="lblDesignation" class="clsLabel">Designation</span>
                                                </td>
                                                <td>
                                                    <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbDesignationList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataValueField="ID" DataTextField="Name" SelectedValue="<%# mEmployeeDesignation.DesignationID %>">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgDesignation" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Designation" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgDesignation" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                	Width="24px" ToolTip="Click to Add New Designation" CausesValidation="True"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPromoted" class="clsLabel">Promoted</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPromoted" runat="server" CssClass="clsCheckBox" Checked="<%# mEmployeeDesignation.IsPromoted %>">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                </td>
                                                <td>
                                                    <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxSearch_Ajax" Text="<%# mEmployeeDesignation.Remark %>"
                                                                    MaxLength="255" ToolTip="Enter Remark">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                                <input type="button" id="btnSelectFile" value="Select File" 
                                                                    class="clsbtnH clsinfoH1">
                                                            </td>
                                                            <td style="padding-left: 3px;">
                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                    Text="Remove Attachment" Enabled="False" ></asp:Button>
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
                                <asp:UpdatePanel ID="upnlSalaryDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <span id="Label2" class="clsLabelHeader">Salary Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10px">
                                                </td>
                                                <td width="100px">
                                                    <span id="lblSalaryHead" class="clsLabel">Salary Head</span>
                                                </td>
                                                <td>
                                                    <table id="Table8" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbSalaryHeadList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataValueField="ID" DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgSalaryHead" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Salary Head" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgSalaryHead" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                    Width="24px" ToolTip="Click to Add New Salary Head" CausesValidation="True"></asp:ImageButton>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblValue" class="clsLabel">Value</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="6"
                                                        ToolTip="Enter Value">0.0</asp:TextBox>
                                                </td>
                                                <td valign="top" align="right">
                                                    <table id="Table6" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnAddSalaryHead" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                    ToolTip="Click to add Salary Head values" ValidationGroup="valGroup1">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="dgEmployeeDesgSalaryList" runat="server" ShowHeaderWhenEmpty="true"
                                                        DataKeyNames="ID" AutoGenerateColumns="False"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="SalaryHeadName" HeaderText="Salary Head">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Value" HeaderText="Value">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="4">
                                                    <table id="Table10" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <span id="lblTotalValue" class="clsLabelAuto">Total Amount</span>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtTotalValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    MaxLength="6" ReadOnly="True" BackColor="#E0E0E0">0.0</asp:TextBox>
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
                                <asp:UpdatePanel ID="upnlAllowanceDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <span id="Label3" class="clsLabelHeader">Allowance Details</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10px">
                                                </td>
                                                <td width="100px">
                                                    <span id="lblAllowance" class="clsLabel">Allowance</span>
                                                </td>
                                                <td>
                                                    <table id="Table12" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAllowanceList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                                    DataValueField="ID" DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <%--<asp:Button ID="imgAllowance" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to Add New Allowance" CausesValidation="False"></asp:Button>--%>

                                                                <asp:ImageButton ID="imgAllowance" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                    Width="24px" ToolTip="Click to Add New Allowance" CausesValidation="True"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Label5" class="clsLabel">Value</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtAllowanceValue" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                        MaxLength="6" ToolTip="Enter Value">0.0</asp:TextBox>
                                                </td>
                                                <td valign="top" align="right">
                                                    <table id="Table9" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnAddAllowance" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
                                                                    ToolTip="Click to add Allowance values" ValidationGroup="valGroup1">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:GridView ID="dgEmployeeDesgAllowanceList" runat="server" 
                                                        DataKeyNames="ID" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="AllowanceName" HeaderText="Allowance">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Value" HeaderText="Value">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="right" colspan="3">
                                                    <table id="Table11" cellspacing="1" cellpadding="1" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                <span id="lblTotalAlowanceValue" class="clsLabelAuto">Total Amount</span>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtTotalAllowanceValue" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    MaxLength="6" ReadOnly="True" BackColor="#E0E0E0">0.0</asp:TextBox>
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
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save Employee Designation Information"
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
                        <!--Dummy panel to open File Upload modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnimgBtnDesignation" ClientIDMode="Static" runat="server" Text="----"
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
    <!--End -->
    <!-- Designation Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupDesignation" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupDesignation" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
        PopupControlID="pnlPopupDesignation" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameStateComplete() {
            $("#btnDummyDesignation").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgDesignation").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupDesignation").attr("src", "wfDesignation_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyDesignation").click();
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
        function ParentCallBackFunctionForDesignation() {
            var DesignationWindow = $find("<%=mdlPopupDesignation.ClientID %>");
            //close Designation popup window
            DesignationWindow.hide();
            $("#iPopupDesignation").attr("src", "JavaScript:''");
            //call Designation image button
            $("#hdnimgBtnDesignation").click();
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
    <!-- Salary Heads --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySalaryHeads" Text="Dummy Salary Heads" />
    </div>
    <asp:Panel runat="server" ID="pnlSalaryHeads" Style="display: none">
        <div>
            <table class="clstablelistout" id="Table7">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlSalaryHeads" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table13" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblTitleSalaryHeads" CssClass="clsFormHeader" runat="server">Salary Head Information [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGroupSalHead"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSalaryHeadName"
                                                Display="None" ErrorMessage="Salary Head Name Required." ValidationGroup="valGroupSalHead"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtSalaryHeadCode"
                                                Display="None" ErrorMessage="Salary Head Code Required." ValidationGroup="valGroupSalHead"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvAllNameLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Salary Head Name too long."
                                                Display="None" ControlToValidate="txtSalaryHeadName" ClientValidationFunction="validateName"
                                                ValidationGroup="valGroupSalHead"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAllCodeLen" runat="server" CssClass="clsLabelAuto" ErrorMessage="Salary Head Code too long."
                                                Display="None" ControlToValidate="txtSalaryHeadCode" ClientValidationFunction="validateName"
                                                ValidationGroup="valGroupSalHead"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateName(source, args) {
                                                    //args.IsValid = false;
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'txtSalaryHeadName':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 50) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'txtSalaryHeadCode':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 5) {
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
                                        <%--<td colspan="3">
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnNewSalaryHeads" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                ToolTip="Click to Add the Salary Head" Text="New"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblDocumentDetails" class="clsLabelHeader">Salary Head Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table>
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <span id="Label6" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td valign="middle">
                                                        <span id="Label7" class="clsLabelAuto">Code</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSalaryHeadCode" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                            ToolTip="Enter Salary Head Code" MaxLength="5">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <span id="Label8" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td valign="middle">
                                                        <span id="lblName" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSalaryHeadName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Salary Head Name"
                                                            MaxLength="50">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        
                                        <%--<td align="right">
                                            <asp:Button ID="btnSaveSalaryHeads" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Salary Head Information"
                                                Text="Save" ValidationGroup="valGroupSalHead"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblSearch" class="clsLabelHeader">Salary Heads List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 270px;">
                                                <table class="clsGrid" style="width: 270px;" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                                    <tr>
                                                        <td width="50px" class="clsdgHeader">
                                                            <span>Code</span>
                                                        </td>
                                                        <td width="100px" class="clsdgHeader">
                                                            <span>Name</span>
                                                        </td>
                                                        <td width="70px" class="clsdgHeader">
                                                            <span>Edit/View</span>
                                                        </td>
                                                        <td width="50px" class="clsdgHeader">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div>
                                                <asp:GridView ID="dgSalaryHeads" runat="server" 
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="true"
                                                    PageSize="5"
                                                    ShowHeader="true" AutoGenerateColumns="False" DataKeyNames="ID">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns> 
                                                        <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Code" HeaderText="Code">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="270px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
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
                                        <td align="right" colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnNewSalaryHeads" CssClass="clsbtnH clsinfoH1" runat="server" CausesValidation="False"
                                                            ToolTip="Click to Add the Salary Head" Text="New"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSaveSalaryHeads" CssClass="clsbtnH clsinfoH1" runat="server" ToolTip="Click to Save Salary Head Information"
                                                            Text="Save" ValidationGroup="valGroupSalHead"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseSalaryHeads" runat="server" CssClass="clsbtnH clsinfoH1" CausesValidation="False"
                                                            ToolTip="Click to close Salary Head Information screen" Text="Close"></asp:Button>
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
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpSalaryHeads" runat="server" TargetControlID="btnDummySalaryHeads"
        PopupControlID="pnlSalaryHeads" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End -->
    <!-- Allowance --ModalPopUp -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyAllowance" Text="Dummy Allowance" />
    </div>
    <asp:Panel runat="server" ID="pnlAllowance" Style="display: none">
        <div>
            <table class="clstablelistout" id="TABLE15">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upnlAllowance" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="TABLE16" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1Newstyle">
                                            <asp:Label ID="lblTitleAllowance" CssClass="clsFormHeader" runat="server">Allowance Information [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="valGroupAllowance"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvAllowanceName" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Allowance Name Required." Display="None" ControlToValidate="txtAllowanceName"
                                                ValidationGroup="valGroupAllowance"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvAllowanceCode" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Allowance Code Required." Display="None" ControlToValidate="txtAllowanceCode"
                                                ValidationGroup="valGroupAllowance"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvNameLength" runat="server" CssClass="clsLabelAuto" ErrorMessage="Allowance Name too long."
                                                Display="None" ControlToValidate="txtAllowanceName" ClientValidationFunction="validateName"
                                                ValidationGroup="valGroupAllowance"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCodeLength" runat="server" CssClass="clsLabelAuto" ErrorMessage="Allowance Code too long."
                                                Display="None" ControlToValidate="txtAllowanceCode" ClientValidationFunction="validateName"
                                                ValidationGroup="valGroupAllowance"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateName(source, args) {
                                                    //args.IsValid = false;
                                                    var ControlName = source.controltovalidate;
                                                    switch (ControlName) {
                                                        case 'txtAllowanceName':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 50) {
                                                                args.IsValid = false;
                                                                return
                                                            }
                                                            break;
                                                        case 'txtAllowanceCode':
                                                            var Value = $get(ControlName).value.length;
                                                            if (Value > 5) {
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
                                        <%--<td colspan="3">
                                            <span id="Label10" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnNewAllowance" CssClass="clsButton_Ajax" runat="server" Text="New"
                                                ToolTip="Click to Add the Allowance" CausesValidation="False"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblAllowanceDetails" class="clsLabelHeader">Allowance Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table>
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <span id="Label11" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td width="35px">
                                                        <span id="Label12" class="clsLabelAuto">Code</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtAllowanceCode" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                            ToolTip="Enter Allowance Code" MaxLength="5">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table>
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <span id="Label13" class="clsLabelStar" style="color: Red;">*</span>
                                                    </td>
                                                    <td width="35px">
                                                        <span id="Label14" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtAllowanceName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Allowance Name"
                                                            MaxLength="50">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="3">
                                            <span id="Label15" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSaveAllowance" CssClass="clsButton_Ajax" ValidationGroup="valGroupAllowance"
                                                runat="server" Text="Save" ToolTip="Click to Save Allowance Information"></asp:Button>
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span id="Label16" class="clsLabelHeader">Allowance List</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <%--<div style="width: 270px;">
                                                <table class="clsGrid" style="width: 270px;" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                                    <tr>
                                                        <td width="50px" class="clsdgHeader">
                                                            <span>Code</span>
                                                        </td>
                                                        <td width="100px" class="clsdgHeader">
                                                            <span>Name</span>
                                                        </td>
                                                        <td width="70px" class="clsdgHeader">
                                                            <span>Edit/View</span>
                                                        </td>
                                                        <td width="50px" class="clsdgHeader">
                                                            <span>Delete</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <div>
                                                <asp:GridView ID="dgAllowance" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5"
                                                     DataKeyNames="ID" ShowHeader="true"
                                                    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="Code" HeaderText="Code">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="270px" Wrap="true" />
                                                        </asp:BoundField>
                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                        </asp:ButtonField>
                                                        <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                            <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="50px" Wrap="true" />
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
                                        <td align="right" colspan="4">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnNewAllowance" CssClass="clsbtnH clsinfoH1" runat="server" Text="New"
                                                            ToolTip="Click to Add the Allowance" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSaveAllowance" CssClass="clsbtnH clsinfoH1" ValidationGroup="valGroupAllowance"
                                                            runat="server" Text="Save" ToolTip="Click to Save Allowance Information"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseAllowance" runat="server" CssClass="clsbtnH clsinfoH1" Text="Close"
                                                            ToolTip="Click to close Allowance Information screen" CausesValidation="False"></asp:Button>
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
            </table>
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpAllowance" runat="server" TargetControlID="btnDummyAllowance"
        PopupControlID="pnlAllowance" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End -->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForEmpDesg();
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
             parent.IFrameEmpDesgStateComplete();
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
