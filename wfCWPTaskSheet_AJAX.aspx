<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPTaskSheet_AJAX.aspx.vb" EnableEventValidation="false" 
    Inherits="Flypal.wfCWPTaskSheet_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet">
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
                            <td width="100%">
                                <span id="lblListEnquiry" class="clstitle1">Task Sheet</span>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%">
                                <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields" ValidationGroup="a" />
                                        <asp:CustomValidator ID="cvFunction" runat="server" CssClass="clsLabelAuto" ErrorMessage="Please select the Function." ValidationGroup="a"
                                            ControlToValidate="cmbFunction" Display="None" ClientValidationFunction="ValidateFunction"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function ValidateFunction(source, args) {
                                                args.IsValid = false;
                                                var dd = $get("cmbFunction");
                                                if (dd.selectedIndex != 0) {
                                                    args.IsValid = true;
                                                    return;
                                                }
                                            }
                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlCWPTaskDetail" runat="server" UpdateMode="Conditional">
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
                                                                        Enabled="False" MaxLength="10" Text="<%# mCWP.CWPTaskSheets.CurrentItem.SrNo %>"
                                                                        ToolTip="Enter Sr.  No."></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Function</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbFunction" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                    DataTextField="Name" BackColor="White" Width="185px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnFunction" runat="server" ImageUrl="~/images/plus1.png"
                                                                                    Height="22px" Width="24px" ToolTip="Click to Add New Function" CausesValidation="True">
                                                                                </asp:ImageButton>
                                                                                <asp:Button ID="hdnimgBtnFunction" ClientIDMode="Static" runat="server" Text="..."
                                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                        <legend><b>Technical Personnel Info.</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span6" class="clsLabelAuto">Employee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbTechEmployeeList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CWPTaskSheets.CurrentItem.TechEmployeeID %>">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblTechLicenseNoStar" runat="server" CssClass="clsLabelStar"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <span id="Span8" class="clsLabelAuto">License No.</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbTechLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td>
                                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                                        <legend><b>Engineering Personnel Info.</b></legend>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span11" class="clsLabelAuto">Employee</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbEngEmployeeList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                                                        DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CWPTaskSheets.CurrentItem.EngEmployeeID %>">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <span id="Span10" class="clsLabelAuto">License No.</span>
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
                                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to Add the Task"
                                                                        CausesValidation="true" ValidationGroup="a" />
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
            parent.ParentCallBackFunctionForTaskSheet();
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
                     parent.IFrameTaskSheetStateComplete();
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
    <!-- Function Main Popup -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyFunctionMain" Text="Dummy Function Main" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlFunctionMain" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupFunctionMain" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFunctionMain" runat="server" TargetControlID="btnDummyFunctionMain"
        PopupControlID="pnlFunctionMain" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFunctionStateComplete() {
            $("#btnDummyFunctionMain").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        function OpenFunctionMasterWindow() {

            try {
                $get("AjaxLoader").style.visibility = "visible";
                $("#iPopupFunctionMain").attr("src", "wfCWPFunctionMaster_AJAX.aspx?&Type=pup");
                if (!$.browser.msie) {
                    $("#btnDummyFunctionMain").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
    </script>
    <script type="text/javascript">
        function ParentCallBackFunction() {
            var atawindow = $find("<%=mdlPopupFunctionMain.ClientID %>");
            //close ata popup window
            atawindow.hide();
            $("#iPopupFunctionMain").attr("src", "JavaScript:''");
            //call ata image button
            $("#hdnimgBtnFunction").click();
        }
    </script>
    <!-------------------->
    </form>
</body>
</html>
