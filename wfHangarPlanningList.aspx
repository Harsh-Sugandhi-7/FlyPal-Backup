<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfHangarPlanningList.aspx.vb"
    Inherits="Flypal.wfHangarPlanningList" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Aircraft List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="script/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Script/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="Script/calendar-en.min.js" type="text/javascript"></script>
    <link href="script/styles/calendar-blue.css" rel="stylesheet" type="text/css" />
    <%-- script for report--%>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }


    </script>
    <link id="MainStyle" rel="stylesheet" type="text/css" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table class="clstablelistout" id="Table1">
            <tr>
                <td>
                    <asp:Panel ID="Panel1" CssClass="clsPanel1" runat="server">
                        <table id="tblLedgerList" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblLedgerList" runat="server" CssClass="clstitle1">Hangar Planning List</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                         
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="fdswodetail" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="ldwodetail" runat="server"><b>Search Information</b></legend>
                                        <table id="Table2" width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="lblSearch" class="clsLabel">Search</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True">
                                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                                <asp:ListItem Value="1" Selected="True">Date</asp:ListItem>
                                                                                <%-- <asp:ListItem Value="1">Date</asp:ListItem>--%>
                                                                                <asp:ListItem Value="2">Hangar</asp:ListItem>
                                                                                <asp:ListItem Value="3">Number</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span id="L1" class="clsLabel" style="width: 20px;"></span>
                                                                        </td>
                                                                        <td>
                                                                            <%-- <asp:TextBox ID="TxtAircraft" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                                    MaxLength="100"></asp:TextBox>--%>
                                                                            <%--    <asp:TextBox ID="TxtHangar" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                                    MaxLength="100"></asp:TextBox>--%>
                                                                            <asp:DropDownList ID="cmbHanger" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                                DataTextField="HHangerWithCity" DataValueField="HID">
                                                                            </asp:DropDownList>
                                                                            <asp:DropDownList ID="cmbText" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True"
                                                                                DataTextField="Text" DataValueField="Text">
                                                                            </asp:DropDownList>
                                                                            <%--<asp:TextBox ID="txtText" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                                MaxLength="100"></asp:TextBox>--%>
                                                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Visible="False">No.</asp:Label>
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" Visible="False"
                                                                                MaxLength="10"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDateTimeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                                                Height="16px">From</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                        <%-- <td align="right">
                                                                                <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server">To Date </asp:Label>
                                                                            </td>--%>
                                                                        <td>
                                                                            <asp:Label ID="lblDateTimeTo" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                                                Height="16px">To</asp:Label>
                                                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                            </cc2:CalendarExtender>
                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                WatermarkCssClass="clsDateTextBox">
                                                                            </cc2:TextBoxWatermarkExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ValidationGroup="a"
                                                                ToolTip="Click to find list of Hangar as per searching criteria" Text="Find Now">
                                                            </asp:Button>
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
            <asp:TextBox ID="txtAmend" runat="server" CssClass="clsTextBoxMedium_Ajax" Visible="False"></asp:TextBox>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblResults" runat="server" CssClass="clsLabelHeader">List of 
                                                        Hangar Planning as per criteria :Record(s) found.</asp:Label>
                                    </td>
                                    <td align="right">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Hangar Planning"
                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close Hangar List screen"
                                                        Text="Close"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="dgHangerList" ShowHeaderWhenEmpty="True" runat="server" AutoGenerateColumns="False"
                                            CssClass="clsGrid" AllowPaging="true" PageSize="25" AllowSorting="True">
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" Font-Underline="true" />
                                            <Columns>
                                                <asp:BoundField Visible="false" DataField="ID" HeaderText="ID" />
                                                <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Haircraft" HeaderText="Aircraft" SortExpression="Haircraft">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HHangerWithCity" HeaderText="Hangar" SortExpression="HHangerWithCity">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HdatetimmefromFormatted" HeaderText="From Date" SortExpression="HdatetimmefromFormatted">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="hdatetimetoFormatted" HeaderText="To Date" SortExpression="hdatetimetoFormatted">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Hremark" HeaderText="Remark">
                                                    <HeaderStyle ForeColor="#FFFFFF" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                
                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                            CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                            Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                            <ContentTemplate>
                                                <table align="right" class="clstableButton">
                                                    <tr>
                                                        <%-- <td>
                                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax " Text="Print"
                                                                                ToolTip="Click To Print Hangar List" />
                                                                        </td>--%>
                                                        <%-- <td>
                                                                            <asp:Button ID="Button1" runat="server" CssClass="clsButton_Ajax " Text="Show graph"
                                                                                ToolTip="Show Graph " />
                                                                        </td>--%>
                                                        <td>
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax " Text="Add New"
                                                                ToolTip="Click to Add New Hangar Planning " />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax " Text="Close"
                                                                ToolTip="Click to close Hangar List screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <caption>
                                    <tr>
                                        <td colspan="2">
                                            <table align="right" class="clstableButton">
                                                <tr>
                                                    <%-- <td>
                                                                        <asp:Button ID="btnAddNew" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            Text="show graph" />
                                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </caption>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <!--Dummy panel to open modelpopup for category/nomenclature-->
            <tr style="height: 0px;">
                <td style="height: 0px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Button ID="hdnBtnHanger" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                Style="display: none;"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <!--End -->
        </table>
        </asp:Panel> </td> </tr> </table>
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
    </div>
    <!-- Hanger Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyHanger" Text="Dummy Hanger" ClientIDMode="Static"
            CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlHanger" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeHanger" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupHanger" runat="server" TargetControlID="btnDummyHanger"
        PopupControlID="pnlHanger" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameHangerStateComplete() {
            $("#btnDummyHanger").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenHangerWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeHanger").attr("src", "wfHangarPlanning.aspx?Type=pup");
                $('#IframeHanger').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyHanger").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForHanger() {
            varHangerwindow = $find("<%=mdlPopupHanger.ClientID %>");
            //close Hanger popup window
            varHangerwindow.hide();
            //           release resources
            $("#IframeHanger").attr("src", "JavaScript:''");
            //call Hanger image button
            $("#hdnBtnHanger").click();
        }
    </script>
    <%-- <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForHanger();
            return false;
        }
    </script>--%>
    <!-- End-->
    <script type="text/javascript">


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
    <%-- <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForPropertyValue();
            return false;
        }
    </script>--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
     <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
       SetPageLayout();
         if ($.browser.msie) {
             parent.IFrameHangerStateComplete();
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
    </form>
</html>
