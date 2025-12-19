<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineMaintenancePolicies_Ajax.aspx.vb"
    Inherits="Flypal.wfMachineMaintenancePolicies_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aircraft Maintenance Policy List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        //Nomenclature
        function validateComboBox(source, args) {
            args.IsValid = false;
            var ControlName = source.controltovalidate;
            switch (ControlName) {
                case 'cmbMaintProgram':
                    var dd = $get("cmbMaintProgram");
                    if (dd.selectedIndex != 0) {
                        args.IsValid = true;
                        return;
                    }
                    break;

                case 'cmbProgramType':
                    var dd = $get("cmbProgramType");
                    if (dd.selectedIndex != 0) {
                        args.IsValid = true;
                        return;
                    }
                    break;
            }
        }

        function validateNameLength(source, args) {
            var ControlName = source.controltovalidate;
            switch (ControlName) {
                case 'txtRemark':
                    var Value = $get(ControlName).value.length;
                    if (Value > 250) {
                        args.IsValid = false;
                        return
                    }
                    break;
                case 'txtDescription':
                    var Value = $get(ControlName).value.length;
                    if (Value > 250) {
                        args.IsValid = false;
                        return
                    }
                    break;
            }
        }
    </script>
</head>
<body class="formBGColor" >
    <form id="form1" runat="server" >
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidation1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvPolicyDescription" runat="server" ControlToValidate="cmbMaintProgram"
                                            ErrorMessage="Maintenance Program Description Required." ClientValidationFunction="validateComboBox"
                                            Display="None" ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Remark should not be greater than 250 Characters."
                                            Display="None" ControlToValidate="txtRemark" ClientValidationFunction="validateNameLength"
                                            ValidationGroup="1"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                    <legend id="Legend1" runat="server"><b>Aircraft Maintenance Policy Details</b></legend>
                                    <asp:UpdatePanel ID="upnlMaintPolicyDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                <td>
                                                                    <span id="Label5" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td style="width: 85px;">
                                                                    <span id="lblName" class="clsLabelAuto">Description</span>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbMaintProgram" runat="server" Width="400px" CssClass="clsComboBoxLong_Ajax"
                                                                                    DataTextField="Name" DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnMaintProgram" runat="server" ImageUrl="~/images/plus1.png"
                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New Maintenance Program" CausesValidation="False">
                                                                                </asp:ImageButton>
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
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtRemark" runat="server" ToolTip="Enter Remark" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                                    Width="400px" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" Text="Applicable"
                                                                                    ToolTip="Check to apply Applicability" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="right">
                                                                    &nbsp;
                                                                    <asp:Button ID="btnAddMaintPolicy" runat="server" CssClass="clsButton_Ajax" OnClientClick="return CheckValidation(1);"
                                                                        Text="Add" ToolTip="Click to Add the Maintenance Policy" ValidationGroup="1" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Maintenance Policy Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMaintenancePolicyList" runat="server" CssClass="clsGrid" ToolTip="Aircraft Maintenance Policy List"
                                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" PageSize="3"
                                                            AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MachineID" HeaderText="Machine ID">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MaintProgramID" HeaderText="MaintProgramID">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaintProgramName" SortExpression="MaintProgramName" HeaderText="Program Description">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Is Applicable">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                            Enabled="False" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remarks">
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
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidation2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="2" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="cmbProgramType"
                                            ErrorMessage="Program Type Required." ClientValidationFunction="validateComboBox"
                                            Display="None" ValidationGroup="2"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="Description should not be greater than 250 Characters." Display="None"
                                            ControlToValidate="txtDescription" ClientValidationFunction="validateNameLength"
                                            ValidationGroup="2"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                    <legend id="Legend2" runat="server"><b>Aircraft Additional Structural Inspection Details</b></legend>
                                    <asp:UpdatePanel ID="upnlStructInspDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table cellspacing="1" cellpadding="1" style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <span id="Label6" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td style="width: 85px;">
                                                                    <span id="lblProgramType" class="clsLabelAuto">Program Type</span>
                                                                </td>
                                                                <td>
                                                                    <table cellspacing="1" cellpadding="1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbProgramType" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                                    Width="400px" DataTextField="Name" DataValueField="ID">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnProgramType" runat="server" ImageUrl="~/images/plus1.png"
                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New Program Type" CausesValidation="False">
                                                                                </asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" ToolTip="Enter Description" CssClass="clsTextBoxMultiLine1_Ajax"
                                                                        Width="400px" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnAddMachineInsp" runat="server" Text="Add" ToolTip="Click to Add Structural Inspection"
                                                                        ValidationGroup="2" OnClientClick="return CheckValidation(2);" CssClass="clsButton_Ajax">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Aircraft Structural Inspection Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMachineStructuralInspList" runat="server" CssClass="clsGrid"
                                                            DataKeyNames="ID" ToolTip="Aircraft Structural Inspection List" AutoGenerateColumns="False"
                                                            PageSize="3" ShowHeaderWhenEmpty="true" AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="MachineID" HeaderText="Machine ID">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="ProgramTypeID" HeaderText="ProgramTypeID">
                                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ProgramTypeName" SortExpression="ProgramTypeName" HeaderText="Program Type">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak" />
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
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go Previous page"
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
    <%--  Call parent AutoResize function to resize the form--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeMaintPolicyList();
        }
    </script>
    <%--Called parent function to open  master pages--%>
    <script language="JavaScript" type="text/javascript">
        function CallParentMaintProgramMasterFunction() {

            //call TankMaster image button
            window.parent.OpenMaintProgramMasterWindow();
        }
        function CallParentProgramTypeMasterFunction() {

            //call TankMaster image button
            window.parent.OpenProgramTypeMasterWindow();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
        function CheckValidation(valGroup) {
            if (!Page_ClientValidate(valGroup)) {
                // Call Your custom JS function and return value.
                CallParentFunction();
            }
            if (!Page_ClientValidate(valGroup)) {
                // Call Your custom JS function and return value.
                CallParentFunction();
            }
        }
    </script>
</body>
</html>
