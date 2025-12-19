<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDDMeetingMinutes_Ajax.aspx.vb"
    Inherits="Flypal.wfDDMeetingMinutes_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Meeting</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Meeting Detail</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="1" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Please enter Title"
                                            Display="None" ControlToValidate="txtTitle" CssClass="clsLabelAuto" ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                            Display="None" OnServerValidate="CustomValidate" ValidationGroup="1" ErrorMessage="Scrap Date should be greater than Manufacturing Date "></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ValidationGroup="1" ErrorMessage=""></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlMROCompDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblPartNo1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabelAuto">Title</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTitle" autocomplete="off" runat="server" CssClass="clsTextBox_Ajax"
                                                        Height="30px" TextMode="MultiLine" Text="<%# mMeeting.Title %>" Width="270px"
                                                        ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="Span5" class="clsLabelAuto">Meeting Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMeetingDate" runat="server" ClientIDMode="Static" CssClass="clsTextBox_Ajax"
                                                        onchange="ValidateDateText(this,'txtMeetingDate_CalendarExtender')" Width="85px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtMeetingDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtMeetingDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="txtMeetingDate_watermarkextender" runat="server"
                                                        TargetControlID="txtMeetingDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">To Show</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkToShow" CssClass="clsCheckBox" runat="server" Checked="<%# mMeeting.InfoToShow %>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlMeetingAgendaDetails" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span class="clsLabelHeader">Meeting Agenda(s)</span>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnAddMeetingAgenda" runat="server" CausesValidation="true"
                                                                Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to Add New Meeting Agenda"
                                                                ValidationGroup="1" Width="24px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMeetingAgenda" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" PageSize="25">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HtmlEncode="false">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="20px" />
                                                                    <ItemStyle Wrap="True" Width="20px" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Agenda Details"
                                                                    ItemStyle-HorizontalAlign="Left">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle BorderStyle="None" HorizontalAlign="Left" Wrap="False" />
                                                                    <HeaderTemplate>
                                                                        <span id="lblAgendaDetailsStar" class="clsLabelStar">*</span> <span id="Span6" class="clsdgHeader">
                                                                            Agenda Details</span>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="upnlAgendaDetailsValidate" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:RequiredFieldValidator ID="rfvPart" runat="server" ControlToValidate="txtAgendaDetails"
                                                                                    CssClass="clsLabel" Display="dynamic" ErrorMessage="Agenda Details Required"
                                                                                    Font-Italic="true" ForeColor="Red" InitialValue="-1" SetFocusOnError="true" Text="* Agenda Details Required"
                                                                                    ValidationGroup='<%# string.Format("Group_{0}", Eval("SrNo")) %>'> </asp:RequiredFieldValidator>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <asp:TextBox ID="txtAgendaDetails" runat="server" CssClass="clsTextBox_Ajax" MaxLength="200"
                                                                            onchange="CheckDuplicate();" Text='<%# DataBinder.Eval(Container.DataItem,"AgendaDetails") %>'
                                                                            TextMode="MultiLine" ToolTip="Enter Agenda Details" Width="100%" Height="30px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Delete" runat="server" CausesValidation="false" CommandArgument='<%# Eval("SrNo") %>'
                                                                            CommandName="Del" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnMeetingMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                        <asp:Button ID="hdnBtnCompsImport" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" ValidationGroup="1" runat="server" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to save Meeting" Text="Save"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSaveNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save the Meeting and add New Meeting"
                                                        ValidationGroup="1" Text="Save &amp; New"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous page"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunction();
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
             parent.IFrameMeetingStateComplete();
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
    </form>
    <script type="text/javascript">
        function CheckDuplicate(sender, args) {
            var grid = document.getElementById("<%=dgMeetingAgenda.ClientID %>");
            var AgendaDetails = $('#<%=dgMeetingAgenda.ClientID %>').find('input[id$="txtAgendaDetails"]');

            var span = $('#<%=dgMeetingAgenda.ClientID %>').find('span[id$="lblDuplicatePart"]');

            for (var i = 0; i < AgendaDetails.length; i++) {
                AgendaDetails[i].style.backgroundColor = "";
                span[i].style.display = 'none';
            }
            for (var i = 0; i < AgendaDetails.length; i++) {
                for (var j = 0; j < AgendaDetails.length; j++) {
                    if (AgendaDetails[i] != AgendaDetails[j] && (AgendaDetails[i].value != "" || AgendaDetails[j].value != "") && AgendaDetails[i].value == AgendaDetails[j].value) {

                        AgendaDetails[i].style.backgroundColor = "Orchid";
                        AgendaDetails[j].style.backgroundColor = "Orchid";
                        span[i].style.display = 'block';
                        span[j].style.display = 'block';

                    }
                }
            }
        }
    </script>
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    <script type="text/javascript">

        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
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
</body>
</html>
