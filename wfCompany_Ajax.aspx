<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompany_Ajax.aspx.vb"
    Inherits="Flypal.wfCompany_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Company</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <asp:UpdatePanel runat="server" ID="pnlValidationSummary" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Company [New]</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="a">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required."
                                                Display="None" ControlToValidate="txtCompName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCompany" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Should not be greater than 50 characters."
                                                Display="None" ControlToValidate="txtCompName" ClientValidationFunction="Validate" ValidationGroup="a"></asp:CustomValidator>

                                                <script type="text/javascript">
                                                    function Validate(source, args) { 
                                                    args.IsValid = true;
                                                    if (document.getElementById("txtCompName").value.length > 50) {
                                                        args.IsValid = false;
                                                        return;
                                                    }
                                                    }
                                                </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblAdd" class="clsLabelAuto">Click To Add New Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsButton_Ajax" Text="New"
                                                ToolTip="Click to add the new company" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upnlCompanyDetails" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td colspan="4">
                                            <span id="lblCompanyDetails" class="clsLabelHeader">Company Details</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblNameStar1" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblName" class="clsLabel">Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCompName" runat="server" CssClass="clsTextBoxMultiLine" Text="<%# mCompany.Name %>"
                                                            ToolTip="Enter Company Name" MaxLength="50" TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span id="lblSave" class="clsLabelAuto">Click To Save Current Record</span>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSave" CssClass="clsButton_Ajax" runat="server" Text="Save" ToolTip="Click to save the Company Information" ValidationGroup="a">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="dgCompanyList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="clsGrid" EnableViewState="False" PagerSettings-Mode="NumericFirstLast"
                                                PageSize="25">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle HorizontalAlign="Right" CssClass="paging"/>
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                        <HeaderStyle HorizontalAlign="Left" Width ="10px" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField CommandName="Remove" HeaderText="Delete" Text="Delete">
                                                        <HeaderStyle HorizontalAlign="Left" Width ="10px" />
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnClose" CssClass="clsButton" runat="server" Text="Close" ToolTip="Click to close Company screen"
                                                CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                <div style="position: fixed; top: 50%; left: 50%; margin-left:-27px;margin-top:-27px; z-index: 100000;">
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
    </form>
</body>
</html>
