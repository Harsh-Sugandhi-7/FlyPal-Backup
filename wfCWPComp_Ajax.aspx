<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCWPComp_Ajax.aspx.vb"
    Inherits="Flypal.wfCWPComp_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>CWP Comp</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
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
                            <td>
                                <span id="lblListEnquiry" class="clstitle1">Sub-Assemblies/Parts Replaced</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvPartNo" runat="server" OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="cmbPartList" ErrorMessage="Defect should not be greater than 500 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvQty" runat="server" OnServerValidate="CustomValidate"
                                            ValidateEmptyText="true" Display="None" ControlToValidate="txtQty" ErrorMessage="Quantity required"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOffSerNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtOffSerialNo" ErrorMessage="Off. Serial No. should not be greater than 50 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvOnSerNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtOnSerialNo" ErrorMessage="On Serial No. should not be greater than 50 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRelNoteNo" runat="server" ClientValidationFunction="validateName"
                                            Display="None" ControlToValidate="txtReleaseNoteNo" ErrorMessage="Release Note No. should not be greater than 50 characters"
                                            CssClass="clsValidationSummary"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function validateName(source, args) {
                                                var ControlName = source.controltovalidate;
                                                switch (ControlName) {
                                                    case 'txtOffSerialNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 50) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;
                                                    case 'txtOnSerialNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 50) {
                                                            args.IsValid = false;
                                                            return
                                                        }
                                                        break;

                                                    case 'txtReleaseNoteNo':
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 50) {
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
                                <asp:UpdatePanel ID="upnlCWPCompDetail" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarSrNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblSrNo" class="clsLabelAuto">Sr. No.</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxSmall_Ajax" ToolTip="Enter Sr.  No."
                                                            Text="<%# mCWP.CWPComps.CurrentItem.SrNo %>" Enabled="False" BorderColor="#E0E0E0"
                                                            MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="cmbPartList" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                            SelectedValue="<%# mCWP.CWPComps.CurrentItem.PartID %>" DataTextField="Name"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span9" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span2" class="clsLabelAuto">Part Name</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBox_Ajax" ClientIDMode="Static"
                                                            ToolTip="Enter Part No." Text="<%# mCWP.CWPComps.CurrentItem.PartNo %>" MaxLength="500"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Span3" class="clsLabelAuto">Part Description</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPartDescription" runat="server" CssClass="clsTextBoxMultiLine1_Ajax"
                                                            Width="385px" ClientIDMode="Static" TextMode="MultiLine" ToolTip="Enter Part Description"
                                                            Text="<%# mCWP.CWPComps.CurrentItem.Description %>"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span10" class="clsLabelStar"></span>
                                                    </td>
                                                    <td>
                                                        <span id="Span4" class="clsLabelAuto">Off. Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOffSerialNo" runat="server" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                            ToolTip="Enter Serial No. of Part to be removed" Text="<%# mCWP.CWPComps.CurrentItem.OffSerialNo %>"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span11" class="clsLabelStar"></span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span5" class="clsLabelAuto">On Serial No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOnSerialNo" runat="server" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                                        ToolTip="Enter Serial No. of Part to be Installed" Text="<%# mCWP.CWPComps.CurrentItem.OnSerialNo %>"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span12" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="Span6" class="clsLabelAuto">Qty.</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxRightAlignSmall_Ajax"
                                                            ClientIDMode="Static" ToolTip="Enter Quantity" Text="<%# mCWP.CWPComps.CurrentItem.Qty %>"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="Span7" class="clsLabelAuto">Release Note No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReleaseNoteNo" runat="server" CssClass="clsTextBoxDate_Ajax"
                                                            ClientIDMode="Static" ToolTip="Enter Release Note No." Text="<%# mCWP.CWPComps.CurrentItem.ReleaseNoteNo %>"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span8" class="clsLabelAuto">Release Note Date</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtReleaseNoteDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'txtReleaseNoteDate_CalendarExtender','false');"
                                                                        Width="100px"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtReleaseNoteDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtReleaseNoteDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender ID="txtReleaseNoteDate_Watermarkextender" runat="server"
                                                                        TargetControlID="txtReleaseNoteDate" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
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
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Technical Personnel Info.</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span13" class="clsLabelAuto">Employee</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbTechEmployeeList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CWPComps.CurrentItem.TechEmployeeID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTechLicenseNoStar" runat="server" CssClass="clsLabelStar"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span id="Span14" class="clsLabelAuto">License No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbTechLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSet" style="border-width: 1px">
                                                        <legend><b>Engineering Personnel Info.</b></legend>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span15" class="clsLabelAuto">Employee</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbEngEmployeeList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="EmpNoName" DataValueField="ID" SelectedValue="<%# mCWP.CWPComps.CurrentItem.EngEmployeeID %>">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEngLicenseNoStar" runat="server" CssClass="clsLabelStar"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <span id="Span16" class="clsLabelAuto">License No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbEngLicenseNoList" runat="server" CssClass="clsComboBox_Ajax"
                                                                        DataTextField="LicenseNo" DataValueField="LicenseNo">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlACtionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add the CWP Component"
                                                        Text="Ok"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="false" Text="Back"></asp:Button>
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
            parent.ParentCallBackFunctionForCWPComp();
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
                     parent.IFrameCWPCompStateComplete();
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
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, DefaultValue) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': DefaultValue };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
    </form>
</body>
</html>
