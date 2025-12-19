<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfChangeMasterData_Ajax.aspx.vb"
    Inherits="Flypal.wfChangeMasterData_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Master Data</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body>
    <form id="form1" runat="server">
    <%--AJAX- ScriptManager Added--%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="3" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span class="clsFormHeader">Change Master Data</span>
                                                    </td>

                                                    <td align="right" colspan="3">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
                                                            <ContentTemplate>
                                                                <table id="Table2" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnUpdate" CssClass="clsbtnH clsinfoH" runat="server" Text="Update"
                                                                                CausesValidation="true" ToolTip="Click to Update record"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to close screen"
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
                                        <td colspan="3">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required"
                                                Display="None" ControlToValidate="txtUpdateName"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="clsLabelHeader">Step I. Select Master to be modified from the list</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Master</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbMasterList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="true">
                                                <asp:ListItem Value="0">(SELECT)</asp:ListItem>
                                                <asp:ListItem Value="1">Currency</asp:ListItem>
                                               <%-- <asp:ListItem Value="2">Nomenclature</asp:ListItem>--%>
                                                <asp:ListItem Value="3">Manufacturer</asp:ListItem>
                                                <asp:ListItem Value="4">Category</asp:ListItem>
                                                <asp:ListItem Value="5">Item</asp:ListItem>
                                                <asp:ListItem Value="6">Vendor</asp:ListItem>
                                                <asp:ListItem Value="7">Employee</asp:ListItem>
                                                <asp:ListItem Value="8">Unit</asp:ListItem>
                                                <asp:ListItem Value="9">Model</asp:ListItem>
                                                <asp:ListItem Value="10">Store</asp:ListItem>
                                                <asp:ListItem Value="11">Place</asp:ListItem>
                                                <asp:ListItem Value="12">Workshop</asp:ListItem>
                                                <asp:ListItem Value="13">Training</asp:ListItem>
                                                <asp:ListItem Value="14">Training Org</asp:ListItem>
                                                <asp:ListItem Value="15">Task Card</asp:ListItem>
                                                <asp:ListItem Value="16">City Maintenance</asp:ListItem>
                                                <asp:ListItem Value="17">City Inventory</asp:ListItem>
                                                <asp:ListItem Value="18">ATA</asp:ListItem>
                                                <asp:ListItem Value="19">Company</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <span class="clsLabelHeader">Step II. Select Record to be modified and enter new Name
                                                in textbox and click on Update</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <fieldset id="fdsMonitoringDetails" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblList" runat="Server" CssClass="clsLabelAuto" Text="Currency List"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblName" runat="Server" CssClass="clsLabelAuto" Text="Name to be Updated"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbNoList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Enabled="false">
                                                                <asp:ListItem Value="0">(SELECT)</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbCurrency" runat="server" Visible="false" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbNomenclature" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                Visible="false" DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbManufacturer" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                Visible="false" DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbItem" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbEmployee" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbUnit" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="ModelName" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbPlace" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbWorkshop" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbATA" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="ATAChapter" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbCompany" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbTaskCard" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="TaskCardNo" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbCityMain" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbCityInv" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbTrainingOrg" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                Visible="false" DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="cmbTraining" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="false"
                                                                DataTextField="Name" DataValueField="ID" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUpdateName" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td align="right" colspan="3">
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
                                                <ContentTemplate>
                                                    <table id="Table2" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnUpdate" CssClass="clsbtnH clsinfoH" runat="server" Text="Update"
                                                                    CausesValidation="true" ToolTip="Click to Update record"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to close screen"
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    </form>
</body>
</html>
