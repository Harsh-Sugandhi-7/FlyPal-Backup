<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfScheduleDetail_Ajax.aspx.vb"
    Inherits="Flypal.wfScheduleDetail_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Schedule Details</title>
    <%--<link id="MainStyle" type="text/css" rel="stylesheet" />--%>
        <link id="Link1" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <style>
        .ComboBoxPadding
        {
            margin-left: 12px;
        }
        .style1
        {
            height: 21px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--AJAX- New function added as Focus gets Lost when we use tabs in Grid--%>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Schedule Details [New]</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationSAummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationsummarySchedule" CssClass="clsValidationSummary"
                                            runat="server" HeaderText="Fill Up The Following Fields" ValidationGroup="1">
                                        </asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFlightNo" runat="server" ErrorMessage="Flight No. Required."
                                            Display="None" ControlToValidate="txtFlightNo" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromplace" runat="server" ErrorMessage="From Place Required."
                                            Display="None" ControlToValidate="TxtFromplace" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvToplace" runat="server" ErrorMessage="To Place Required."
                                            Display="None" ControlToValidate="TxtToplace" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDepartureTime" runat="server" ErrorMessage="Departure Time Required."
                                            Display="None" ControlToValidate="TxtDepartureTime" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvArrivalTime" runat="server" ErrorMessage="Arrival Time Required."
                                            Display="None" ControlToValidate="TxtArrivalTime" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate" ControlToValidate ="TxtArrivalTime"
                                            ValidationGroup="1" Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                             <asp:CustomValidator ID="CustValidator1" runat="server" OnServerValidate="CustomValidate1" ControlToValidate ="TxtArrivalTime"
                                            ValidationGroup="1" Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>
                                    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelHeader" Text="<%# mEnquiry.StatusName %>">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <table id="Table2" border="0" width="100%">
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlSchedule" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlSchedule" runat="server" CssClass="clspanel1">
                                                        <table id="Table13" class="clsTable1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <span id="Span3" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span4" class="clsLabelAuto">Flight No.</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxSmall_Ajax" MaxLength="25" Text="<%# mRoute.RouteSchedules.CurrentItem.FlightNo %>">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblStarEnquiryNo" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="lblNo" class="clsLabelAuto">Departure Place</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtFromplace" runat="server" CssClass="clsTextBoxDate_Ajax" Text="<%# mRoute.RouteSchedules.CurrentItem.FromPlace %>"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span2" class="clsLabelAuto">Arrival Place</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtToplace" runat="server" CssClass="clsTextBoxDate_Ajax" Text="<%# mRoute.RouteSchedules.CurrentItem.ToPlace %>"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="Span5" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span6" class="clsLabelAuto">Departure Time</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtDepartureTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                        MaxLength="10" ToolTip="Enter Departure Time." AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="Span7" class="clsLabelStar">*</span>
                                                                </td>
                                                                <td>
                                                                    <span id="Span8" class="clsLabelAuto">Arrival Time</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtArrivalTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                        MaxLength="10" ToolTip="Enter Arrival Time." AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span9" class="clsLabelAuto">WeekDays</span>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:CheckBoxList ID="ChckWeekDays" runat="server" RepeatDirection="Horizontal" CellPadding="6"
                                                                        CellSpacing="6" ValidationGroup="1" DataValueField="ID" DataTextField="Day" CssClass="clsCheckBox">
                                                                    </asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOK" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save Schedule"
                                                        ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Close" ToolTip="Click to go back to the previous page">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <%--<asp:Button ID="hdnimgBtnScheduleDetail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        --%></ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
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
    </form>
    <!-- Autocomplete for Source and Destination Place   -->
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=TxtFromplace.ClientID%>,#<%=TxtToplace.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
                width: 200,
                autoFill: true,
                matchContains: true,
                delay: 0


            });
        });
    </script>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForRouteSchedule();
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
             parent.IFrameScheduleStateComplete();
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
</body>
</html>
