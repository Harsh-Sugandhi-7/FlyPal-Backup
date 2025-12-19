<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSchedule_Ajax.aspx.vb"
    Inherits="Flypal.wfSchedule_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Schedule</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <script type="text/javascript" id="clientEventHandlersJS">
        //        function openTranDetail() {
        //            str = "wfReports.aspx"
        //            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        //        }
        //        function openTranDetail1() {
        //            str = "webform1.aspx"
        //            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        //        }
        //        function openFile() {
        //            str = "wfFileView.aspx"
        //            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        //        }
        //        function openDetail() {
        //            str = "wfDetail.aspx"
        //            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        //        }
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
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            height: 25px;
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspanel1">
                    <asp:UpdatePanel ID="upnlRouteScheduleDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tblinner" class="clsTablelistin">
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Schedule [New]</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationSAummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Route Name must not be greater than 25 characters."
                                                    Display="None" ControlToValidate="txtRouteName" ClientValidationFunction="validateName"
                                                    ValidationGroup="1"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvRouteName" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Route Name Required" ControlToValidate="txtRouteName" Display="None"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="Valid From Date Required" ControlToValidate="txtValidFrom" Display="None"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtValidTo"
                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Valid To Date Required"
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ClientValidationFunction="BetweenDatesValidation" ValidationGroup="1" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                                <script type="text/javascript">

                                                    function validateName(source, args) {
                                                        var ControlName = source.controltovalidate;
                                                        var Value = $get(ControlName).value.length;
                                                        if (Value > 25) {
                                                            args.IsValid = false;
                                                            return;
                                                        }
                                                    }
                                                </script>
                                                <%--<asp:CustomValidator ID="cvCustomer" runat="server" ErrorMessage="Select Customer from the list."
                                            ControlToValidate="cmbCustomer" Display="None" ClientValidationFunction="valiDateCustomer"
                                            ValidationGroup="1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvVendor" runat="server" ErrorMessage="Select Customer from the list"
                                            ControlToValidate="cmbVendorList" Display="None" ClientValidationFunction="validateVendorForSalesEnquiry"
                                            ValidationGroup="1"></asp:CustomValidator>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table id="Table2" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <asp:Panel ID="pnlsSchedule" runat="server" CssClass="clspanel1">
                                                        <asp:UpdatePanel ID="upnlSchedule" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table13" class="clsTable1" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <span id="Span3" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span4" class="clsLabelAuto">Route Name</span>
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <asp:TextBox ID="txtRouteName" runat="server" CssClass="clsTextBox1_Ajax" MaxLength="150"
                                                                                Text="<%# mRoute.RouteName %>">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblDateStar1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="lblDate" class="clsLabelAuto">Valid From</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtValidFrom" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                onchange="ValidateDateText(this,'ScheduleStart_watermarkextender');" Text="<%# mRoute.ValidFromFormatted %>"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtValidFrom">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtValidFrom" ID="ScheduleStart_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span2" class="clsLabelAuto">Valid To</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtValidTo" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                onchange="ValidateDateText(this,'ScheduleEnd_watermarkextender');" Text="<%# mRoute.ValidToFormatted %>"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtValidTo">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtValidTo" ID="ScheduleEnd_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span5" class="clsLabelAuto">Total Weekly Time</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTotalWeeklyTime" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="true"
                                                                                BackColor="Gainsboro" Text="<%# mRoute.TotalWeeklyTime %>">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Hrs.</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            <span id="Span6" class="clsLabelAuto">Note</span>
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <asp:TextBox ID="TxtNote" runat="server" CssClass="clsTextBoxMultiLineLong" MaxLength="500"
                                                                                Text="<%# mRoute.Note %>" TextMode="MultiLine"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Schedule Details as per criteria : Record(s) found</asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Button ID="btnAddScheduleDetail" runat="server" CssClass="clsButton_Ajax" Text="Add Schedule"
                                                                ToolTip="Click to Add Schedule Detail" ValidationGroup="1" Enabled="<%# Not mRoute.IsNew %>">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save" ToolTip="Click to save Schedule"
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
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="dgScheduleDetailList" ShowHeaderWhenEmpty="True" runat="server"
                                            EnableViewState="true" AutoGenerateColumns="False" CssClass="clsGrid" AllowPaging="true"
                                            PageSize="15" AllowSorting="True">
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Font-Underline="true" />
                                            <Columns>
                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                    DataField="ID" HeaderText="ID" Visible="false" />
                                                <asp:BoundField DataField="SrNo" HeaderText="SrNo" SortExpression="SrNo">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FlightNo" HeaderText="Flight No." SortExpression="FlightNo">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FromPlace" HeaderText="From Place" SortExpression="FromPlace">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ToPlace" HeaderText="To Place" SortExpression="ToPlace">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DepartureTimeUTC" HeaderText="From Date" SortExpression="DepartureTimeUTC">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ArrivalTimeUTC" HeaderText="To Date" SortExpression="ArrivalTimeUTC">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="WeekDaysID" HeaderText="WeekDays">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                            Visible="false" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                            CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;" colspan="2">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnBtnSchedule" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
    <!-- Schedule Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySchedule" Text="Dummy Schedule" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlSchedule" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSchedule" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSchedule" runat="server" TargetControlID="btnDummySchedule"
        PopupControlID="pnlSchedule" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameScheduleStateComplete() {
            $("#btnDummySchedule").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenScheduleWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSchedule").attr("src", "wfScheduleDetail_Ajax.aspx?Type=pup");
                $('#IframeSchedule').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummySchedule").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRouteSchedule() {
            varSchedule = $find("<%=mdlPopupSchedule.ClientID %>");
            //close HangerMaster popup window
            varSchedule.hide();
            //           release resources
            $("#IframeSchedule").attr("src", "JavaScript:''");
            //call HangerMaster image button
            $("#hdnBtnSchedule").click();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForSchedule();
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
             parent.IFrameScheduleComplete();
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
          //onResize();//for Top bottom link
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
    <%-- <!-- Schedule Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySchedule" Text="Dummy Schedule" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlSchedule"  HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSchedule" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSchedule" runat="server" TargetControlID="btnDummySchedule"
        PopupControlID="pnlSchedule" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>--%>
    <%--<script type="text/javascript">
        function IFrameScheduleStateComplete() {
            $("#btnDummySchedule").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenScheduleWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSchedule").attr("src", "wfScheduleDetail_Ajax.aspx?Type=pup");
                $('#IframeSchedule').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummySchedule").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRouteSchedule() {
            varSchedule = $find("<%=mdlPopupSchedule.ClientID %>");
            
            varSchedule.hide();
           
            $("#IframeSchedule").attr("src", "JavaScript:''");
            
            $("#hdnBtnSchedule").click();
        }
    </script>--%>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <%--<script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForSchedule();
            return false;
        }
    </script>--%>
    <%--Set page layout when open as popup aspx page--%>
    <%--<script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
       SetPageLayout();
         if ($.browser.msie) {
             parent.IFrameScheduleComplete();
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
          //onResize();//for Top bottom link
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

    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                //                beforeSend: function (xhr, settings) {
                //                    $("[id$=processing]").dialog();
                //                },
                success: onSuccess,
                error: onError
            });

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
    --%>
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtValidFrom").val();
            var todate = $("#txtValidTo").val();
            if (!todate) {
                rfvToDate.isvalid = false;
                return;
            }
            if (!fromdate) {
                rfvFromDate.isvalid = false;
                return;
            }
            var param = { 'FromDate': fromdate, 'ToDate': todate };
            $.ajax({
                type: "POST",
                url: "BetweenDateValidationHandler.ashx",
                cache: false,
                data: param,
                async: false,
                beforeSend: OnBeforeSnd,
                success: onSuces,
                error: onErr
            });

            function onSuces(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                if (result == "True") {
                    args.IsValid = true;
                    return;
                }

            }

            function onErr(result) {
                $get("AjaxLoader").style.visibility = 'hidden';
                source.errormessage = result;
                return;
            }
            function OnBeforeSnd() {
                $get("AjaxLoader").style.visibility = 'visible';
            }

        }

        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
