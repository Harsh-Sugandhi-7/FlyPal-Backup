<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceTaskDetail_AJAX.aspx.vb"
    Inherits="Flypal.wfMaintenanceTaskDetail_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> Maintenance Task [New]</asp:Label>
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
                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="Task Required" ControlToValidate="txtTask" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvNote" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="txtNote" ErrorMessage="Note Should not be greater than 500 characters."
                                        OnServerValidate="customvalidate"></asp:CustomValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset id="fdsPartInfo" class="clsFieldSetNewStyle" style="border-width: 1px">
                                <legend id="lblPartInfo" runat="server" style="font-weight: bold"><b>Task Card </b>
                                </legend>
                                <asp:UpdatePanel ID="upnlPartInfo" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblSrNo" class="clsLabel">Sr.No. </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxSmall_Ajax " ToolTip="Enter Sr.  No."
                                                        Enabled="False" MaxLength="10" ReadOnly="True" BackColor="#E0E0E0" Text="<%# mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.SrNo %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblTaskCardNo" class="clsLabel">Task Card No.</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTaskCardNo" runat="server" CssClass="clsTextBoxAuto" Text="<%# mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.TaskCardNo %>"
                                                        MaxLength="8" BackColor="#E0E0E0" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblStarQty" class="clsLabelStar" visible="False">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblTask" class="clsLabel">Task </span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTask" runat="server" CssClass="clsTextBoxMultiLineAuto" MaxLength="400"
                                                        Text="<%# mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Task %>" TextMode="MultiLine"
                                                        BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblNote" class="clsLabel ">Note</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLineAuto" ToolTip="Enter Note"
                                                        MaxLength="500" Text="<%# mMaintenanceTask.MaintenanceTaskDetails.CurrentItem.Note %>"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblKitDetails" runat="server" cssclass="clsLabelHeader">Steps:</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="dgTaskSteps" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                        PageSize="3" ShowHeaderWhenEmpty="True">
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                         <HeaderStyle CssClass="clsdgHeader nodrag nodrop" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MPDNo" HeaderText="MPD. No.">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AMMNo" HeaderText="AMM. No.">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Zone" HeaderText="Zone">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
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
                        <td align="right" colspan="4">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
            parent.ParentCallBackFunctionForMaintenanceTasks();
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
             parent.IFrameMaintenanceTasksStateComplete();
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
