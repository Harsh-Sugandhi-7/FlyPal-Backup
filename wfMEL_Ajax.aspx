<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMEL_Ajax.aspx.vb" EnableEventValidation="false"
    Inherits="Flypal.wfMEL_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Minimum Equipment</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body class="formBGColor">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clsTableListIn">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                        </asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvFrequency" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFrequencyInDay"
                                            Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvFreqHours" runat="server" CssClass="clsLabelAuto" ErrorMessage="Frequency In Hours Required."
                                            ControlToValidate="txtFrequencyInHours" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnNew" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlMELDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnNew" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                        CssClass="clsButton_Ajax" Text="New" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Click to add the new ADD Entry","Click to add the new MEL Entry") %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="lbltitle" runat="server" style="font-weight: bold"><b>Minimum Equipment
                                                            Details [New]</b></legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <span id="lblPartNoStar1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblComponent" class="clsLabelAuto">Component</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:DropDownList ID="cmbComponent" runat="server" AutoPostBack="True" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="Name" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="clsLabel">Description</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMultiLine1_Ajax "
                                                                        ReadOnly="True" Text="<%# mMEL.Description %>" TextMode="MultiLine" ToolTip=" Description">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMakeMELQty" runat="server" class="clsLabelAuto" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Make ADD Qty.","Make MEL Qty.") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMakeMELQty" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                        MaxLength="50" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Enter Manufacturer ADD Qty.","Enter Manufacturer MEL Qty.") %>'></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblFlyMELQty" runat="server" class="clsLabelAuto" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Dispatch ADD Qty.","Dispatch MEL Qty.") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFlyMELQty" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                    MaxLength="50" ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True","Enter Dispatch ADD Qty.","Enter Dispatch MEL Qty.") %>'></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <span id="Label4" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblMELCategory" class="clsLabelAuto">Category</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:UpdatePanel ID="upnlCategory" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="cmbMELCategory" runat="server" AutoPostBack="True" CssClass="clsComboBox_Ajax"
                                                                                DataTextField="Name" DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblFrequency" class="clsLabel">Frequency</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:UpdatePanel ID="upnlFreq" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:TextBox ID="txtFrequencyInDay" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                Enabled="False" MaxLength="4" Text="<%# mMEL.FrequencyInDays %>" ToolTip="Enter Frequency In Days">
                                                                            </asp:TextBox>
                                                                            <span id="lblDays" class="clsLabel">Days</span>
                                                                            <asp:TextBox ID="txtFrequencyInHours" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                                                Enabled="False" MaxLength="5" Text="<%# mMEL.FrequencyInHours %>" ToolTip="Enter Frequency In Hours">
                                                                            </asp:TextBox>
                                                                            <span id="lblHours" class="clsLabel">Hours</span>
                                                                            <asp:CheckBox ID="chkIsInHours" runat="server" AutoPostBack="True" Checked="<%# mMEL.IsHours %>"
                                                                                CssClass="clsCheckBox" Enabled="False" Text="(Select if Frequency is in Hours e.g. 11:59)" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" MaxLength="500"
                                                                        TextMode="MultiLine" ToolTip="Enter Remark"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" ToolTip="Click to save Minimum Equipment information"
                                                        OnClientClick="CallParentFunction();" Text="Save" CssClass="clsButton_Ajax">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Minimum Equipment List</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgMELList" runat="server" CssClass="clsGrid" ToolTip="List of Minimum Equipment."
                                                        ShowHeaderWhenEmpty="true" DataKeyNames="ID" AllowPaging="true" PageSize="25"
                                                        AutoGenerateColumns="False" AllowSorting="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <RowStyle CssClass="clsdgAltItem TextBreak"></RowStyle>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="Id"></asp:BoundField>
                                                            <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Component">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle Wrap="False" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MELCategoryName" SortExpression="MELCategoryName" HeaderText="Category">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MakeMELQty" HeaderText="Make MEL Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FlyMELQty" HeaderText="Dispatch MEL Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyInDays" HeaderText="Frequency In Days">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="White"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyInHours" HeaderText="Frequency In Hours">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="#FFFFFF"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec" HeaderStyle-HorizontalAlign="Left">
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-HorizontalAlign="Left">
                                                            </asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose" CssClass="clsButton_Ajax" runat="server" CausesValidation="False"
                                            ToolTip="Click to go back to the previous page" Text="Back"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="PartID" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="PartName" ClientIDMode="Static" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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
    </form>
    <script type="text/javascript">
        function setComboBoxValue(elem) {
            var id = $(":selected", elem).val();
            var Name = $(":selected", elem).text();
            $("#PartID").val(id);
            $("#PartName").val(Name);
        }
    </script>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeMELList();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
    </script>
</body>
</html>
