<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNRCPartOnOff_Ajax.aspx.vb"
    Inherits="Flypal.wfNRCPartOnOff_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NRC Part On Off</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table class="clstablelistin" id="tblLedgerList">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lblNRCPartOnOff" class="clsFormHeader">NRC Part On Off</span>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlACtionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add the NRC On Off Part"
                                                                    Text="Ok"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                    CausesValidation="false" Text="Back"></asp:Button>
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
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvPartNo" runat="server" OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="cmbOffPartNo" ErrorMessage="Defect should not be greater than 500 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOffSerNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtOffPartSerialNo" ErrorMessage="Off. Serial No. should not be greater than 100 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOnSerNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtOnPartSerialNo" ErrorMessage="On Serial No. should not be greater than 100 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRelNoteNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtReleaseNoteNo" ErrorMessage="Release Note No. should not be greater than 100 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtOffPartSerialNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 100) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtOnPartSerialNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 100) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtReleaseNoteNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 100) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlNRCPartOnOffDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span id="lblSrNo" class="clsLabelAuto">Sr. No.</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSrNo" runat="server" BorderColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
                                                                            Enabled="False" MaxLength="10" Text="<%# mNRC.NRCPartOnOffs.CurrentItem.SrNo %>"
                                                                            ToolTip="Enter Sr.  No."></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:UpdatePanel ID="upnlOffPart" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                    <legend><b>Off Part</b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOffPartNo" class="clsLabelAuto">Part No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbOffPartNo" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mNRC.NRCPartOnOffs.CurrentItem.OffPartID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOffPartDescription" class="clsLabelAuto">Description</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOffPartDescription" runat="server" ClientIDMode="Static" CCssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                    Enabled="false" Text="<%# mNRC.NRCPartOnOffs.CurrentItem.OffPartDescription %>"
                                                                                    ReadOnly="true" TextMode="MultiLine" Width="385px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOffPartSerialNo" class="clsLabelAuto">Serial No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOffPartSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.NRCPartOnOffs.CurrentItem.OffPartSerialNo %>">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlOnPart" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                    <legend><b>On Part</b></legend>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOnPart" class="clsLabelAuto">Part No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbOnPartNo" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                    DataTextField="Name" DataValueField="ID" SelectedValue="<%# mNRC.NRCPartOnOffs.CurrentItem.OnPartID %>">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOnPartDescription" class="clsLabelAuto">Description</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOnPartDescription" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                    Enabled="false" ReadOnly="true" TextMode="MultiLine" Width="385px" Text="<%# mNRC.NRCPartOnOffs.CurrentItem.OnPartDescription %>"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblOnPartSerialNo" class="clsLabelAuto">Serial No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtOnPartSerialNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mNRC.NRCPartOnOffs.CurrentItem.OnPartSerialNo %>">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblReleaseNoteNo" class="clsLabelAuto">Release Note No.</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"
                                                                                    Text="<%# mNRC.NRCPartOnOffs.CurrentItem.ReleaseNoteNo %>" ToolTip="Enter Release Note No."></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlACtionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add the NRC On Off Part"
                                                        Text="Ok"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="false" Text="Back"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForNRCPartOnOff();
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
                     parent.IFrameNRCPartOnOffStateComplete();
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
