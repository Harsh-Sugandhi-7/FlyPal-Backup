<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobResourceDetail_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobResourceDetail_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resource Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css">
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clsTableListIn" id="tblInner">
                            <tr>

                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Resource Detail</asp:Label>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAddTop" runat="server" BorderStyle="Solid" CssClass="clsbtnH clsinfoH" CausesValidation="true"
                                                                ValidationGroup="a" Enabled="<%# mnWO.WOStatusID <> 3 %>" Text="Add" ToolTip="Click to Add Resource Detail" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseTop" runat="server" CausesValidation="True" CssClass="clsbtnH clsinfoH"
                                                                Text="Close" ToolTip="Click to close Resource Detail screen" />
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
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" Display="None"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvTotalTime" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="a" ControlToValidate="txtTotalTime" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvEndDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="a" ControlToValidate="txtEndDate" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <script type="text/javascript">
                                        function validateBlank(source, args) {
                                            args.IsValid = true;

                                            var ed = $get("txtEndDate");
                                            var sd = $get("txtStartDate");
                                            var tt = $get("txtTotalTime");
                                            if (ed.Text == '' & sd.Text == '' & tt.text == '') {
                                                args.IsValid = false;
                                                return;
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlResourceDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                <table id="Table3" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabelAuto">Designation</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDesignation" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch1"
                                                                ReadOnly="True" ToolTip="Designation"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <asp:Label ID="lblResource" runat="server" CssClass="clsLabelAuto">Resource</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtResource" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch1"
                                                                ReadOnly="True" Text="<%# mnWOJobResourceAllocation.ResourceName %>" ToolTip="Resource"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <asp:Label ID="lblStartDate" runat="server" CssClass="clsLabelAuto">Start Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagDateSearch" Width="150px" AutoCompleteType="None"
                                                                onblur="ValidateDateText(this,'TBWE1');" ClientIDMode="Static"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateTimeFormatLOG%>" TargetControlID="txtStartDate" ClientIDMode="Static"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="TBWE1" runat="server" TargetControlID="txtStartDate"
                                                                WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateTimeFormatLOG%>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right"></td>
                                                        <td>
                                                            <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabelAuto">End Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagDateSearch" Width="150px"
                                                                onblur="ValidateDateText(this,'TBWE2');" ClientIDMode="Static"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                Enabled="True" Format="<%$AppSettings:DateTimeFormatLOG%>" TargetControlID="txtEndDate" ClientIDMode="Static"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtEndDate"
                                                                WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateTimeFormatLOG%>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 28px"></td>
                                                        <td style="height: 28px">
                                                            <asp:Label ID="lblTotalTime" runat="server" CssClass="clsLabelAuto">Total Time</asp:Label>
                                                        </td>
                                                        <td style="height: 28px">
                                                            <asp:TextBox ID="txtTotalTime" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" Enabled="<%# mnWO.WOStatusID <> 3 %>"
                                                                MaxLength="7" ToolTip="Enter Total Time"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                      
                                                          <td align="right" style="height: 28px">
                                                           <table id="Table1" cellpadding="1" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddTop" runat="server" BorderStyle="Solid" CssClass="clsButton_Ajax"  CausesValidation="true"
                                                                ValidationGroup="a" Enabled="<%# mnWO.WOStatusID <> 3 %>" Text="Add" ToolTip="Click to Add Resource Detail" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                    Text="Close" ToolTip="Click to close Resource Detail screen" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblResourceDetaillist" runat="server" CssClass="clsLabelHeader">Resource Detail list</asp:Label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgResouceDet" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                CssClass="clsGridNewStyle" ToolTip="List of Resource Detail" Width="400px" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                        <HeaderStyle />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LicenceNo" HeaderText="Licence No." Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="StartDateTimeFormatted" HeaderText="Start Date">
                                                        <HeaderStyle Wrap="False" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EndDateTimeFormatted" HeaderText="End Date">
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalTime" HeaderText="Total Time">
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <%--<asp:ButtonField CommandName="EditRecord" HeaderText="Edit" Text="Edit"></asp:ButtonField>
                                                    <asp:ButtonField CommandName="DeleteRecord" HeaderText="Delete" Text="Delete"></asp:ButtonField>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%-- <span id="button">Login</span>--%>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <table id="Table2" border="0" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                            Text="Close" ToolTip="Click to close Resource Detail screen" Visible="false" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div>
            <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
        </div>
        <div>
            <script type="text/javascript">
                //Date validations
                function ValidateDateText(elem, extenderid) {

                    var datevalue = $(elem).val();
                    var params = { 'Date': datevalue, 'SetDefault': 'false' };
                    $.ajax({
                        type: "POST",
                        url: "DateTimeValidationHandler.ashx",
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
        </div>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForResourceAllocation();
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
                    parent.IFrameResourceAllocationStateComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
           <% End if %>
            }
            function ReSetPageLayout() {
                $("body,html").css({ 'background-color': 'transparent' });
                var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <%--End--%>
    </form>
</body>
</html>
