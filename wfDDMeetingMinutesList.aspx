<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDDMeetingMinutesList.aspx.vb"
    Inherits="Flypal.wfDDMeetingMinutesList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Meeting Minutes List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">List of Meeting Minutes</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table id="Table1">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="78px">Date</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsComboBox1_Ajax" AutoPostBack="True">
                                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Width="78px">From Date</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBox_Ajax" Width="85px"
                                                                                            OnTextChanged="txtTodate_TextChanged" CausesValidation="true" ValidationGroup="a"
                                                                                            ClientIDMode="Static" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                            AutoPostBack="True"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="TextBoxWatermarkExtender1"
                                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                            WatermarkCssClass="clsDateTextBox">
                                                                                        </cc2:TextBoxWatermarkExtender>
                                                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="78px" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBox_Ajax" Width="85px"
                                                                                            OnTextChanged="txtTodate_TextChanged" CausesValidation="true" ValidationGroup="a"
                                                                                            ClientIDMode="Static" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                                            AutoPostBack="True"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="TextBoxWatermarkExtender2"
                                                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                            WatermarkCssClass="clsDateTextBox">
                                                                                        </cc2:TextBoxWatermarkExtender>
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
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkShowAllRecordsTop" runat="server" CssClass="clsLinkButton"
                                                                    Visible="false" ForeColor="Red" Text="View All"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table2" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                            ValidationGroup="1" Text="Add New" ToolTip="Click to Add MRO Component" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                            TabIndex="0" Text="Close" ToolTip="Click to close List of MRO Component screen" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgMeetingList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                    PageSize="5" ShowHeaderWhenEmpty="true" EnableViewState="True" CssClass="clsGrid">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                    <RowStyle CssClass="clsdgItem" />
                                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                                        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="IDateFormatted" HeaderText="Date" SortExpression="IDateFormatted">
                                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="To Show" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "InfoToShow") %>'
                                                                                    Enabled="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
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
                                                                                <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Remove"
                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton_Ajax" TabIndex="0" ValidationGroup="1"
                                                        Text="Add New" ToolTip="Click to Add MRO Component" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="0" Text="Close" ToolTip="Click to close List of MRO Component List screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnMeeting" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
            <!-- Meeting Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyMeeting" Text="Dummy Meeting" ClientIDMode="Static"
                    CausesValidation="false"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlMeeting" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeMeeting" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupMeeting" runat="server" TargetControlID="btnDummyMeeting"
                PopupControlID="pnlMeeting" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMeetingStateComplete() {
                    $("#btnDummyMeeting").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenMeetingWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeMeeting").attr("src", "wfDDMeetingMinutes_Ajax.aspx?Type=pup");
                        $('#IframeMeeting').animate({ top: '50px' }, 'slow');
                        //                        if (!$.browser.msie) {
                        $("#btnDummyMeeting").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                        //                        }


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunction() {
                    varMeetingwindow = $find("<%=mdlPopupMeeting.ClientID %>");
                    //close Meeting popup window
                    varMeetingwindow.hide();
                    //           release resources
                    $("#IframeMeeting").attr("src", "JavaScript:''");
                    //call Meeting image button
                    $("#hdnBtnMeeting").click();
                }
            </script>
            <!-- End-->
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        //Date validations
        function BetweenDatesValidation(source, args) {
            args.IsValid = false;
            var fromdate = $("#txtFromDate").val();
            var todate = $("#txtToDate").val();

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
