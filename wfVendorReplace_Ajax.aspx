<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfVendorReplace_Ajax.aspx.vb"
    Inherits="Flypal.wfVendorReplace_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vendor Replace Utility</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="1800" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout" border="0">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlsearch" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                                <table id="tblInner" class="clstablelistin" border="0">
                                    <tr>
                                        <td class="clsFormHeader1">
                                            <span id="lbltitle" class="clsFormHeader">Vendor Replace Utility</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                        HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                    <asp:CustomValidator ID="cvVendor" runat="server" Display="None" ControlToValidate="cmbVendorList"
                                                        ErrorMessage="Select vendor from the list." OnServerValidate="CustomValidate" ValidationGroup="1"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="cvT" runat="server" Display="None" ControlToValidate="cmbForValidation"
                                                        ErrorMessage="Select vendor(s) to be replace from the list." OnServerValidate="CustomValidate" ValidationGroup="1"></asp:CustomValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlVendor" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblVendorStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto">Vendor</asp:Label>
                                                            </td>
                                                            <td>&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp 
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                    DataTextField="Name" DataValueField="ID" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cmbForValidation" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                    DataTextField="Name" DataValueField="ID" AutoPostBack="true" Style="display: none;">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlVendorsIsToBeReplaced" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblVendors" class="clsLabelAuto" runat="server" Text="Vendor(s) To Be Replace"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:ListBox ID="ListOfVendor" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                    DataTextField="Name" DataValueField="ID"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnReplaceNDelete" runat="server" CssClass="clsbtnH clsinfoH1" Text="Replace &amp; Delete"
                                                                    ToolTip="Click to Replace &amp;  Delete Vendor(s)." Width="120px" ValidationGroup="1"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                                    Text="Close" ToolTip="Click to close the Vendor Replace screen" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
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
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        function disableEnableOnPageLoad() {

            var x = document.getElementById("cmbVendorList").selectedIndex;
            if (x == 0) {
                $('[id*=ListOfVendor]').multiselect('clearSelection', true);
                $('[id*=ListOfVendor]').multiselect('disable', false);
                $('[id*=ListOfVendor]').multiselect('buttonWidth', '264px');
                $('[id*=ListOfVendor]').multiselect('nonSelectedText', 'Vendor');
                $('[id*=ListOfVendor]').multiselect('allSelectedText', 'Vendor');
            }
        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            RegNoMultiSelect();
        });
    </script>
    <script type="text/javascript">
        function RegNoMultiSelect() {
            $('[id*=ListOfVendor]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Vendor',
                selectAllJustVisible: false,
                buttonWidth: '264px',
                allSelectedText: 'Vendor',
                nSelectedText: 'Vendor',

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');
        }
    </script>
</body>
</html>
