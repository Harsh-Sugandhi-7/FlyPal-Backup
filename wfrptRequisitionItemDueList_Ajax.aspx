<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptRequisitionItemDueList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptRequisitionItemDueList_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Priority wise Requisition status</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmrptPartHitory" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">

                                    <span class="clsFormHeader" id="lbltitle">Priority wise Requisition status</span>



                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <span class="clsLabelHeader" id="lblStep1">Step I. Selection of As On Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabel" id="lblTo">As On Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtToDate"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <span class="clsLabelHeader" id="lblRequisition">Step II. Selection of Requisition</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="clsLabelAuto" id="lblRequisitionType">Requisition</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbRequisition" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">ALL</asp:ListItem>
                                                            <asp:ListItem Value="65">Engineering</asp:ListItem>
                                                            <asp:ListItem Value="71">Stores</asp:ListItem>
                                                            <asp:ListItem Value="72">WorkShop</asp:ListItem>
                                                            <asp:ListItem Value="77">Planning</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label CssClass="clsLabelHeader" ID="lblStep2" runat="server">Step III. Selection of Part Number/Description</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtPartDescription" runat="server"
                                                            Width="520px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server" Text="Display"
                                                            ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH" ID="btnCloseTop" runat="server" CausesValidation="False"
                                                            Text="Close" ToolTip="Click to Close" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=txtPartDescription.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                    width: 520,
                    autoFill: false,
                    matchContains: true,
                    delay: 0
                });
            });
        </script>
    </form>
</body>
</html>
