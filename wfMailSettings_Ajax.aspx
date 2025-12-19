<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMailSettings_Ajax.aspx.vb" Inherits="Flypal.wfMailSettings_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Mail Setting</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
     <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
 </head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                    <table class="clstablelistin" id="tblInner">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Mail Settings</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
                                                                    ToolTip="Click to Save Mail settings" ValidationGroup="1" CausesValidation="true" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close" />
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
                                <%--<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvReqMailID" runat="server" ValidationGroup="1" Display="None"
                                    ErrorMessage="Please Enter at least one Valid Email-ID" ControlToValidate="txtMailIDs"
                                    CssClass="" ClientValidationFunction="validateEmailID" ValidateEmptyText="true"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvMailIDs" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtMailIDs"
                                    ErrorMessage="Please Enter Valid Email-ID" CssClass="" ClientValidationFunction="validateMultipleEmailsCommaSeparated"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCc" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtCCIDs"
                                    ErrorMessage="Please Enter Valid Cc Email-ID" CssClass="" ClientValidationFunction="validateMultipleCcEmailsCommaSeparated"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvBcc" runat="server" ValidationGroup="1" Display="None" ControlToValidate="txtBCCIDs"
                                    ErrorMessage="Please Enter Valid Bcc Email-ID" CssClass="" ClientValidationFunction="validateMultipleBccEmailsCommaSeparated"></asp:CustomValidator>
                                --%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSendMailDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            
                                            
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Smtp Host</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSmtpHost" runat="server" CssClass="clsTextBoxTagSearch" Width="265px"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Smtp Port</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSmtpPort" runat="server" Width="50px"  CssClass="clsTextBoxTagSearch"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Smtp User</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSmtpUser" runat="server"  CssClass="clsTextBoxTagSearch" Width="265px"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Smtp Password</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSmtpPassword" runat="server"  CssClass="clsTextBoxTagSearch" Width="265px" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                               <tr>
                                                <td>
                                                    <span class="clsLabelAuto">Module Type</span>
                                                </td>
                                                <td>
                                                   <asp:DropDownList ID="cmbModuleType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                                   <asp:ListItem Text="Transaction" Value="csTransType"></asp:ListItem>
                                                   <asp:ListItem Text="Module" Value="UM_csModule"></asp:ListItem>
                                            </asp:DropDownList>
                                              </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
                                                        ToolTip="Click to Send Requisition by Mail" ValidationGroup="1" CausesValidation="true" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to go back to the previous page" />
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
    
    </form>
</body>
</html>
