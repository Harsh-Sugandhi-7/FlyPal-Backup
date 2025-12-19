<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineFeatureList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfMachineFeatureList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Feature List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout" class="formBGColor">
    <form id="form1" runat="server" >
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
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlValidation" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" />
                                            <asp:CustomValidator ID="cvFeatureList" runat="server" ControlToValidate="cmbFeatureList"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="Select Features form List."
                                                ClientValidationFunction="Validation" ValidationGroup="a"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Value  Required"
                                                Display="None" ControlToValidate="txtValue" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <script type="text/javascript">
                                                function Validation(source, args) {
                                                    var Value = $get('cmbFeatureList');
                                                    if (Value.selectedIndex == 0) {
                                                        args.IsValid = false;
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
                                    <fieldset id="fdsAircraftFeatureDetails" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="lblAircraftFeatureDetails" style="font-weight: bold"><b>Aircraft Feature
                                            Details</b></legend>
                                        <asp:UpdatePanel ID="upnlAircraftFeatureDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0">
                                                    <tr>
                                                        <td>
                                                            <span id="lblStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblFeature" class="clsLabelAuto">Feature</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbFeatureList" runat="server" CssClass="clsComboBox_Ajax"
                                                                DataTextField="Name" DataValueField="Id">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgbtnFeature" runat="server" ImageUrl="~/images/plus1.png"
                                                                Height="22px" Width="24px" ToolTip="Click To Add New Feature" CausesValidation="False">
                                                            </asp:ImageButton>
                                                        </td>
                                                        <td>
                                                            <span id="lblStar1" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblValue" class="clsLabelAuto">Value</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtValue" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </fieldset>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnAdd" OnClientClick="return CheckValidation();" TabIndex="0" runat="server"
                                                CssClass="clsButton_Ajax" ToolTip=" Click to add Feature in the List" ValidationGroup="a"
                                                Text="Add"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Feature Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgFeatureList" runat="server" ClientIDMode="Static" PageSize="25"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
                                                            CssClass="clsGrid" AllowPaging="True" AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Sr. No.">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FeatureName" HeaderText="Feature ">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FeatureValue" HeaderText="Value">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="Remove" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
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
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlClose" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"
                                                CausesValidation="False" Text="Back"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeFeatureList();
        }
        function CheckValidation() {
            if (!Page_ClientValidate()) {
                // Call Your custom JS function and return value.
                CallParentFunction();
            }
        }
    </script>

    <%--Called parent function to open Feature master page--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFeatureFunction() {
            window.parent.OpenFeatureWindow();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
</body>
</html>
