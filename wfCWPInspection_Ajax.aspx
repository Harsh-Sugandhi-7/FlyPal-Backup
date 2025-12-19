<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPInspection_Ajax.aspx.vb"
    Inherits="Flypal.wfCWPInspection_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>CWP Inspection</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
                            <td>
                                <span id="lblListEnquiry" class="clstitle1">Inspection Sheet</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a" />
                                <asp:RequiredFieldValidator ID="rfvDefect" runat="server" ControlToValidate="txtDefect"
                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Defect required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvWorkDone" runat="server" ControlToValidate="txtWorkDone"
                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Work Done required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvDefect" runat="server" ClientValidationFunction="validateName"
                                    ControlToValidate="txtDefect" CssClass="clsValidationSummary" Display="None"
                                    ErrorMessage="Defect should not be greater than 500 characters" ValidationGroup="a"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvWorkDOne" runat="server" ClientValidationFunction="validateName"
                                    ControlToValidate="txtWorkDone" CssClass="clsValidationSummary" Display="None"
                                    ErrorMessage="Work Done should not be greater than 500 characters" ValidationGroup="a"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvTechLicenseNoList" runat="server" CssClass="clsValidationSummary"
                                    Display="None" ClientValidationFunction="validateName" ControlToValidate="cmbTechEmployeeList"></asp:CustomValidator>
                                <script type="text/javascript">

                                    function validateName(source, args) {
                                        var ControlName = source.controltovalidate;
                                        switch (ControlName) {
                                            case 'txtDefect':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 500) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;
                                            case 'txtWorkDone':
                                                var Value = $get(ControlName).value.length;
                                                if (Value > 500) {
                                                    args.IsValid = false;
                                                    return
                                                }
                                                break;

                                        }
                                    }
                                 
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlCWPInspectionDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblStarSrNo" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblSrNo" class="clsLabelAuto">Sr. No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSrNo" runat="server" BorderColor="#E0E0E0" CssClass="clsTextBoxSmall_Ajax"
                                                                        Enabled="False" MaxLength="10" Text="<%# mCWP.CWPInspections.CurrentItem.SrNo %>"
                                                                        ToolTip="Enter Sr.  No."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                        <legend><b>Defect/Work Required Info.</b></legend>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span3" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:TextBox ID="txtDefect" runat="server" CssClass="clsTextBoxMultiLine_Ajax" ClientIDMode="Static"
                                                                                        Width="270px" ToolTip="Enter Defect" Text="<%# mCWP.CWPInspections.CurrentItem.Defect %>"
                                                                                        TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span1" class="clsLabelAuto">Tech. Employee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbInspSheetDefectEngEmployeeList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="EmpNoName" DataValueField="ID" AutoPostBack="true" SelectedValue="<%# mCWP.CWPInspections.CurrentItem.InspSheetDefectEngEmployeeID %>">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span2" class="clsLabelAuto">Eng. License No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbInspSheetDefectEngLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td>
                                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                        <legend><b>Work Done Info.</b></legend>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span4" class="clsLabelStar">*</span>
                                                                                </td>
                                                                                <td colspan="2">
                                                                                    <asp:TextBox ID="txtWorkDone" runat="server" CssClass="clsTextBoxMultiLine_Ajax"
                                                                                        Width="270px" ToolTip="Enter Work Done" ClientIDMode="Static" Text="<%# mCWP.CWPInspections.CurrentItem.WorkDone %>"
                                                                                        TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span6" class="clsLabelAuto">Tech. Employee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbTechEmployeeList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="EmpNoName" DataValueField="ID" AutoPostBack="true" SelectedValue="<%# mCWP.CWPInspections.CurrentItem.TechEmployeeID %>">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span8" class="clsLabelAuto">Tech. License No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbTechLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span11" class="clsLabelAuto">Eng. Employee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbEngEmployeeList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CWPInspections.CurrentItem.EngEmployeeID %>">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span10" class="clsLabelAuto">Eng. License No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbEngLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to Add the Inspection"
                                                                        ValidationGroup="a" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to the previous page" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%-- <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    </asp:UpdateProgress>--%>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForInspection();
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
                     parent.IFrameInspectionStateComplete();
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
