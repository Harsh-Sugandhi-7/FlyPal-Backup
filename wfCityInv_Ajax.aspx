<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCityInv_Ajax.aspx.vb"
    Inherits="Flypal.wfCityInv_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>City Inv</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <script type="text/javascript" language="javascript">
        function OpenLocation(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td class="clsFormHeader1Newstyle">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTitle" TabIndex="1" runat="server" CssClass="clsFormHeader">City Information [New]</asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to Add the City"
                                                                                    CausesValidation="False"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save City Information"></asp:Button>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnBackBottom" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                    Text="Close" ToolTip="Click to close City Information screen" CausesValidation="False"></asp:Button>

                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                            ValidationGroup="a"></asp:ValidationSummary>
                                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="City Required"
                                                            Display="None" ControlToValidate="txtName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvCity" runat="server" CssClass="clsLabelAuto" ErrorMessage="City Name too Long."
                                                            Display="None" ControlToValidate="txtName" OnServerValidate="customvalidate"
                                                            ValidationGroup="a"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvState" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select State"
                                                            Display="None" ControlToValidate="cmbStateName" OnServerValidate="customvalidate"
                                                            ValidationGroup="a"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <%--<td colspan="3">
                                                        <span id="spnAdd" class="clsLabelAuto">Click To Add New Record</span>
                                                    </td>--%>
                                                    <%--<td align="right">
                                                        <asp:Button ID="btnNew" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click to Add the City"
                                                            CausesValidation="False"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <span id="spnCityDetails" class="clsLabelHeader">City Details</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="spnName1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="spnName" class="clsLabelAuto">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCityInv.Name %>"
                                                            ToolTip="Enter WorkShop Name" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="spnState1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblStateName" class="clsLabelAuto">State Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbStateName" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                            DataValueField="ID" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <%--  <asp:Button ID="imgState" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                            ToolTip="Click to Add New State" CausesValidation="False"></asp:Button>--%>
                                                        <asp:ImageButton ID="imgbtnState" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                            Width="24px" ToolTip="Click to Add New State" CausesValidation="True"></asp:ImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <span id="spnCountry" class="clsLabelAuto">Country</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCityInv.Name %>"
                                                            ToolTip=" Country  Name" MaxLength="25" ReadOnly="True" BackColor="#E0E0E0">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td colspan="3">
                                                        <span id="spnSave" class="clsLabelAuto">Click To Save Current Record</span>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save City Information">
                                                        </asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">City List</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgCity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                             PagerSettings-Mode="NumericFirstLast" PageSize="25"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                            CausesValidation="false" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>

                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Remove" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
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
                                                                    DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td>
                                    <asp:UpdatePanel runat="server" ID="upnlClose" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnBackBottom" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close City Information screen" CausesValidation="False">
                                                        </asp:Button>
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
    </div>
    <div>
        <%--State--%>
        <asp:Panel runat="server" ID="pnlState" CssClass="clspanel1">
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyState" Text="Dummy State" />
            </div>
            <div style="width: 100%">
                <asp:UpdatePanel runat="server" ID="upnlState" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="Table1" class="clstablelistout">
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                        <table id="Table2" class="clstablelistin">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="clsFormHeader1Newstyle">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblStateTitle" runat="server" CssClass="clsFormHeader">State Information [New]</asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnNewState" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the new State"
                                                                                            CausesValidation="False" Text="New"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSaveState" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
                                                                                            ToolTip="Click to Save State Information" ValidationGroup="b"></asp:Button>
                                                                                    </td>

                                                                                    <td>
                                                                                        <asp:Button ID="btnCloseState" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                            Text="Close" ToolTip="Click to close State Information screen" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
                                                                    ValidationGroup="b"></asp:ValidationSummary>
                                                                <asp:RequiredFieldValidator ID="reqState" runat="server" CssClass="clsLabelAuto"
                                                                    ErrorMessage="State Name Required" ControlToValidate="txtStateName" Display="None"
                                                                    ValidationGroup="b"></asp:RequiredFieldValidator>
                                                                <asp:CustomValidator ID="cvStateName" runat="server" CssClass="clslabel" ErrorMessage="State Name Too long"
                                                                    ControlToValidate="txtStateName" Display="None" OnServerValidate="customvalidate"
                                                                    ValidationGroup="b"></asp:CustomValidator>
                                                                <asp:CustomValidator ID="cvCountry" runat="server" CssClass="clslabel" ErrorMessage="Select Country"
                                                                    ControlToValidate="cmbCountry" Display="None" ClientValidationFunction="ValidatecmbCountry"
                                                                    ClientIDMode="Static" ValidationGroup="b"></asp:CustomValidator>
                                                                <script type="text/javascript">
                                                                    function ValidatecmbCountry(source, args) {
                                                                        var dd = $get("cmbCountry");
                                                                        args.IsValid = true;
                                                                        if (dd.selectedIndex == 0) {
                                                                            args.IsValid = false;
                                                                            return;
                                                                        }
                                                                    }
                                                                </script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                       <%-- <tr>
                                                            <td colspan="3">
                                                                <span id="Span1" class="clsLabelAuto">Click To Add New Record</span>
                                                            </td>
                                                            <%--<td align="right">
                                                                <asp:Button ID="btnNewState" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the new State"
                                                                    CausesValidation="False" Text="New"></asp:Button>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td colspan="4">
                                                                <span id="spnStateDetails" class="clsLabelHeader">State Details</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Span2" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span3" class="clsLabelAuto">State Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStateName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter State Name"
                                                                    Text="<%# mState.Name %>" MaxLength="25"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="spnCountryName1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span4" class="clsLabelAuto">Country Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCountry" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgCountry" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
                                                                    Width="24px" ToolTip="Click to Add New Country" CausesValidation="True"></asp:ImageButton>
                                                            </td>
                                                        </tr>
                                                      <%--  <tr>
                                                            <td colspan="3">
                                                                <span id="Span5" class="clsLabelAuto">Click To Save Current Record</span>
                                                            </td>
                                                            <%--<td align="right">
                                                                <asp:Button ID="btnSaveState" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
                                                                    ToolTip="Click to Save State Information" ValidationGroup="b"></asp:Button>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader">State List</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--<div style="width: 360px;">
                                                                    <table class="clsGridNewStyle" style="width: 360px;" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                                                        <tr>
                                                                            <td width="120px" class="clsdgHeader">
                                                                                <span>Name</span>
                                                                            </td>
                                                                            <td width="90px" class="clsdgHeader">
                                                                                <span>Country</span>
                                                                            </td>
                                                                            <%--<td width="70px" class="clsdgHeader">
                                                                                <span>Edit/View</span>
                                                                            </td>
                                                                            <td width="50px" class="clsdgHeader">
                                                                                <span>Delete</span>
                                                                            </td>
                                                                            <td width="120px" class="clsdgHeader">
                                                                                <span>Action</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div style="max-height: 150px; overflow-y: auto; overflow-x: hidden; width: 350px">--%>
                                                                    <asp:GridView ID="dgState" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                        Style="width: 360px;" ShowHeader="True" PagerSettings-Mode="NumericFirstLast"
                                                                        PageSize="10" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle HorizontalAlign="Right" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                                                <HeaderStyle  HorizontalAlign="left" Width="120px"></HeaderStyle>
                                                                                <ItemStyle  HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="Country">
                                                                                <HeaderStyle HorizontalAlign="left" Width="50px"></HeaderStyle>
                                                                                <ItemStyle  HorizontalAlign="left" Width="120px" Wrap="true" />
                                                                            </asp:BoundField>
                                                                            <%--<asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                        CausesValidation="false" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>--%>

                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>

                                                                                    <div class="dropdown">
                                                                                        <div class="dropdownbtn-content">
                                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" Enabled='<%# IIf(Eval("IsSyncFromCRS") = True, False, True) %>' />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Remove" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" Enabled='<%# IIf(Eval("IsSyncFromCRS") = True, False, True) %>' />
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
                                                                                DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                        </Columns>
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                <%--</div>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnCloseState" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Close" ToolTip="Click to close State Information screen" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender runat="server" ID="mdlState" TargetControlID="btnDummyState"
            PopupControlID="pnlState" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <%--End State--%>
        <%--Country--%>
        <asp:Panel runat="server" ID="pnlCountry" CssClass="clspanel1">
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyCountry" Text="Dummy Country" />
            </div>
            <div style="width: 100%">
                <asp:UpdatePanel runat="server" ID="upnlCountry" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="Table3" class="clstablelistout">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCountryInformation" runat="server" CssClass="clspanel1">
                                        <table id="Table4" class="clstablelistin">
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="clsFormHeader1Newstyle">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblCountryTitle" CssClass="clsFormHeader" runat="server">Country Information [New]</asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Button ID="btnNewCountry" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                                                            ToolTip="Click to Add the new Country " Text="New"></asp:Button>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSaveCountry" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Country Information"
                                                                                            Text="Save" ValidationGroup="c"></asp:Button>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:Button ID="btnCloseCountry" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                                            Text="Close" ToolTip="Click to close Country screen" CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="clsValidationSummary"
                                                                    Height="40px" ValidationGroup="c" ClientIDMode="Static"></asp:ValidationSummary>
                                                                <asp:RequiredFieldValidator ID="reqCountryName" runat="server" CssClass="clsLabelAuto"
                                                                    ErrorMessage="Country Required" ControlToValidate="txtCountryName" Display="None"
                                                                    ValidationGroup="c"></asp:RequiredFieldValidator>
                                                                <%--  <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clslabel" ErrorMessage="Country Name Required."
                                                                    ControlToValidate="txtCountryName" Display="None" ClientValidationFunction="ValidateCountry" ClientIDMode="Static" 
                                                                    ValidationGroup="c"></asp:CustomValidator> 
                                                                  <script type="text/javascript">
                                                                        function ValidateCountry(source, args) {
                                                                            args.IsValid = false;
                                                                            var CountryName = $get("txtCountryName").value;
                                                                            if (CountryName !="") {
                                                                                args.IsValid = true;
                                                                                return;
                                                                            }
                                                                        }
                                                                    </script>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <%--<tr>
                                                            <td colspan="3">
                                                                <span id="Span6" class="clsLabelAuto">Click To Add New Record</span>
                                                            </td>
                                                            <%--<td align="right">
                                                                <asp:Button ID="btnNewCountry" CssClass="clsbtnH clsinfoH" runat="server" CausesValidation="False"
                                                                    ToolTip="Click to Add the new Country " Text="New"></asp:Button>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td colspan="4">
                                                                <span id="spnCountryDetails" class="clsLabelHeader">Country Details</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="Span7" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="Span8" class="clsLabelAuto">Country</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCountryName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Country"
                                                                    Text="<%# mCountry.Name %>" MaxLength="25">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td colspan="3">
                                                                <span id="Span9" class="clsLabelAuto">Click To Save Current Record</span>
                                                            </td>
                                                            <%--<td align="right">
                                                                <asp:Button ID="btnSaveCountry" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save Country Information"
                                                                    Text="Save" ValidationGroup="c"></asp:Button>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Country List</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--<div style="width: 325px;">
                                                                    <table class="clsGrid" style="width: 325px;" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                                                                        <tr>
                                                                            <td width="205px" class="clsdgHeader">
                                                                                <span>Country</span>
                                                                            </td>
                                                                            <td width="70px" class="clsdgHeader">
                                                                                <span>Edit/View</span>
                                                                            </td>
                                                                            <td width="50px" class="clsdgHeader">
                                                                                <span>Delete</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div style="max-height: 150px; overflow-y: auto; overflow-x: hidden; width: 343px">--%>
                                                                    <asp:GridView ID="dgCountry" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                         ShowHeader="True" PagerSettings-Mode="NumericFirstLast"
                                                                        PageSize="10" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                        <PagerStyle HorizontalAlign="Right" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                            <asp:BoundField DataField="Name" HeaderText="Country" SortExpression="Name">
                                                                                <HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="205px"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="left" Width="205px" Wrap="true" />
                                                                            </asp:BoundField>
                                                                            <%--<asp:TemplateField HeaderText="Edit/View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                        CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                        CausesValidation="false" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>--%>

                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>

                                                                                    <div class="dropdown">
                                                                                        <div class="dropdownbtn-content">
                                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" Enabled='<%# IIf(Eval("IsSyncFromCRS") = True, False, True) %>' />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" Enabled='<%# IIf(Eval("IsSyncFromCRS") = True, False, True) %>' />
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
                                                                                DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                        </Columns>
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                <%--</div>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnCloseCountry" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                    Text="Close" ToolTip="Click to close Country screen" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <cc2:ModalPopupExtender runat="server" ID="mdlCountry" TargetControlID="btnDummyCountry"
            PopupControlID="pnlCountry" BackgroundCssClass="clsModalPopupBGForSecondPage">
        </cc2:ModalPopupExtender>
        <%--End Country--%>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunction();
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
                        parent.IFrameCityStateComplete();
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
              var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
              var windowheight=$(window).height();
              if (tempMargtop>=windowheight)
              {
                $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
              }
              else
              {
              var margintop=(windowheight/2)-(tempMargtop/2);
               $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
              }
       
           }
        </script>
        <%--End--%>
    </div>
    </form>
</body>
</html>
