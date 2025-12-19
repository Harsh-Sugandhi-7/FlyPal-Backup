<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceKitDetail_Ajax.aspx.vb"
    Inherits="Flypal.wfMaintenanceKitDetail_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <table class="clstablelistin" id="tblInner">
                    <tr>
                        <td colspan="4"  class="clsFormHeader1Newstyle">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Maintenance Kit Item [New]</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
                                        CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvPartNo" runat="server" Display="None" ControlToValidate="cmbPartNo"
                                        ErrorMessage="Select Part No from the List" OnServerValidate="customvalidate"
                                        CssClass="clsLabelAuto"></asp:CustomValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset id="fdsInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                <legend id="lblInfo" runat="server" style="font-weight: bold"><b>Enter Part Number to
                                    Search</b></legend>
                                <asp:UpdatePanel ID="upnlInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSrNo" runat="server" CssClass="clsLabel"> Sr.No. </asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxSmall_Ajax" ToolTip="Enter Sr.  No."
                                                        Enabled="False" MaxLength="10" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mMaintenanceKit.MaintenanceKitDetails.CurrentItem.SrNo %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel"> Search </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                        ToolTip="Enter Part No to Search"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSearch" CssClass="clsbtnH clsinfoH1" runat="server" Text="Find Now"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset id="fdsPartInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                <legend id="lblPartInfo" runat="server" style="font-weight: bold"><b>Select Part to
                                    Add into Kit</b></legend>
                                <asp:UpdatePanel ID="upnlPartInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td style="height: 3px">
                                                    <asp:Label ID="lblStarParNo" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td style="height: 3px">
                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabel">Part No. </asp:Label>
                                                </td>
                                                <td style="height: 3px">
                                                    <asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
                                                         DataValueField="ID" SelectedValue="<%# mMaintenanceKit.MaintenanceKitDetails.CurrentItem.ItemID %>">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblQuality" runat="server" CssClass="clsLabel">Quantity</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                        ToolTip="Enter Quantity" MaxLength="8" Text="<%# mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Qty %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabel">Note</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="500"
                                                        Width="220px" ToolTip="Enter Note" Text="<%# mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Note %>"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRemark" runat="server" CssClass="clsLabel">Remark</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" MaxLength="500"
                                                        Width="220px" ToolTip="Enter Remark" Text="<%# mMaintenanceKit.MaintenanceKitDetails.CurrentItem.Remark %>"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to add Kit Item"
                                                    Text="Ok"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to go back to the previous page"
                                                    Text="Back" CausesValidation="False"></asp:Button>
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
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForMaintenanceKit();
            return false;
        }
    </script>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameMaintenanceKitStateComplete();
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
    </form>
</body>
</html>
