<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPBHList.aspx.vb" Inherits="Flypal.wfPBHList"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title>FlyPal By Hour</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" src="DATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlList">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                                <table id="tblInner" class="clstablelistin">
                                    <tbody>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblPartsList" runat="server" CssClass="clstitle1">Flypal By Hour</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:Label ID="lblHeader" runat="server" CssClass="clsLabelHeader" Width="550px">Following is the list for Hours Subscription</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">FlyPal Hour List</asp:Label>
                                            </td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton" Text="ADD" ToolTip="Click to Add new Aircraft Subscrption"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:GridView ID="dgPBHList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                    ShowHeaderWhenEmpty="true" PageSize="25">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No">
                                                            <HeaderStyle ForeColor="White"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ModelDetails" HeaderText="Model Info.">
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastLogDetails" HeaderText="Last Flying Date">
                                                            <ItemStyle Wrap="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="StartDateFormatted" HeaderText="Start Date" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"> </asp:BoundField>
                                                        <asp:BoundField DataField="DaysFrequency" HeaderText="Subscribed Days" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                        <asp:BoundField DataField="EndDateFormatted" HeaderText="End Date" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"></asp:BoundField>
                                                        <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                        <asp:BoundField DataField="StartHours" HeaderText="Start Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="HoursFrequencyText" HeaderText="Subscribed Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="CarryForwardHoursText" HeaderText="Carry Forward Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="EndHoursText" HeaderText="End Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="CurrentHoursText" HeaderText="Current Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="ElapsedHoursText" HeaderText="Elapsed Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="RemainingHoursText" HeaderText="Remaining Hours"></asp:BoundField>
                                                        <asp:BoundField DataField="RemainingHoursDec" HeaderStyle-CssClass="hideGridColumn"
                                                            HeaderText="RemainingHoursDec" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        <asp:BoundField DataField="HoursFrequencyDec" HeaderStyle-CssClass="hideGridColumn"
                                                            HeaderText="HoursFrequencyDec" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Renew">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IDRenew" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="Renew" Style="width: 20px" ImageUrl="~/images/Renew1.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IDDelete" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="DeleteRec" Style="width: 20px" ImageUrl="~/images/delete.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Borrow">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IDBorrow" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="BorrowRec" Style="width: 20px" ImageUrl="~/images/borrow.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hrs. Extension">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="IDExtension" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                    CommandName="ExtensionRec" Style="width: 20px" ImageUrl="~/images/extension.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"></td>
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnADDBottom" runat="server" CssClass="clsButton" Text="ADD" ToolTip="Click to Add new Aircraft Subscrption"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%--  <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
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
        <!-- PBH Detail-->
        <script type="text/javascript">
            //event handler for end request i.e last event in client page cycle.
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            //event handler for begin request i.e before sending request to the server
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

            var element;
            var timerId;
            var timeoutforblink;
            var hideRowHighlight = false;

            function endRequestHandler(sender, args) {
                var tempval = parseInt($("#gridrowindex").val()); //row number ..0 is header row..
                if (tempval) {
                    $("#<%=dgPBHList.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
                if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
                    var elem;
                    var tempaction = $("#gridrowaction").val(); //action to be performed

                    //button close of popup windows
                    //remove highlight row class... and return from function
                    if (tempaction == "BorrowClose") {
                        $("#<%=dgPBHList.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        $("#gridrowaction").val('');
                        return;
                    }
                    //change location button ok event
                    //blink location column of the row for perticular interval
                    else if (tempaction == "Borrow") {
                        $("#<%=dgPBHList.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#<%=dgPBHList.ClientID %> tr:eq(" + tempval + ") td:eq(9)");
                            $("#gridrowaction").val('');
                        }


                        else {
                            return;
                        }
                        //blink column function
                        //                    timeoutforblink = setInterval(function () {

                        //                        if (elem.hasClass('activerow')) {
                        //                            elem.removeClass('activerow');
                        //                        }
                        //                        else {
                        //                            elem.addClass('activerow');
                        //                        }

                        //                    }, 500);
                        //                    //stop blink column
                        //                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
                    }


                }
            }

            function BeginRequestHandler(sender, args) {
                clearTimeout(timerId);
                element = args.get_postBackElement();
                //change BorrowPBH popup ok button event occur
                if (element.id == "btnBorrowPBH") {
                    hideRowHighlight = true;
                    $("#gridrowaction").val('Borrow');
                }
                //change BorrowPBH popup close button event occur 
                else if (element.id == "btnBorrowPBHClose") {
                    hideRowHighlight = true;
                    $("#gridrowaction").val('BorrowClose');
                }
                //change parttype ||change location link event occur
                //reset rowindex value if other grid event occurs
                else if (element.id == "dgPBHList") {
                    if ($("#gridrowaction").val() != "gridrow") {
                        $("#gridrowindex").val('');
                    }
                }
                //any other events
                else {
                    $("#gridrowindex").val('');
                }
            }

            //stop blinking
            function TimeOut(val, action) {
                var tempelem;
                if (action == "Borrow") {
                    tempelem = $("#dgPBHList tr:eq(" + val + ") td:eq(9)");
                    tempelem.removeClass('activerow');

                }
                else {
                    return;
                }
                $("#gridrowindex").val('');
                hideRowHighlight = false;
                clearInterval(timeoutforblink);
            }
        </script>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPBH" Text="Dummy Rate" />
        </div>
        <asp:Panel runat="server" ID="pnlChangePBH" Style="display: none">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlChangePBH">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlPBH" Visible="false">
                        <table class="clstablelistout" id="Table5">
                            <tr>
                                <td>
                                    <table class="clstablelistin" id="Table6">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblTitle" CssClass="clstitle1" runat="server">Pay By Hour</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlValidations">
                                                    <ContentTemplate>
                                                        <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="rate" runat="server"
                                                            CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                        <asp:CustomValidator ID="cvStartDate" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                            ValidationGroup="rate" Display="None" ControlToValidate="txtStartDate" ValidateEmptyText="true"
                                                            ErrorMessage="Enter atleast Start Date or Star Hours"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="reqstartDate" runat="server" CssClass="clsLabelAuto"
                                                            ValidationGroup="rate" ControlToValidate="txtDaysFreq" Text="Frequency in Days Required."></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="csStartHours" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
                                                            ValidationGroup="rate" Display="None" ControlToValidate="txtStartHours" ValidateEmptyText="true"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cvModelList" runat="server" ControlToValidate="cmbAircraftList"
                                                            CssClass="clsValidationSummary" Display="None" ErrorMessage="Select model from the list."
                                                            OnServerValidate="customvalidate" ValidationGroup="rate"></asp:CustomValidator>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <span id="Span16" class="clsLabel">Is Combined Hours? </span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsCombinedHrs" runat="server" CssClass="clsCheckBox" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="spnAircraftStar" runat="server" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblAlrcraft" class="clsLabel">Aircraft </span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:DropDownList ID="cmbAircraftList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                    DataTextField="RegNo" DataValueField="ID" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span7" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Span3" class="clsLabel">Start Date</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                    onchange="ValidateDateText(this,'txtStartDate_CalendarExtender');" Width="100px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtStartDate"
                                                    WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span2" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Span4" class="clsLabel">Days Frequency</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtDaysFreq" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span5" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Span6" class="clsLabel">End Date</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="True" CssClass="clsTextBox_Ajax"
                                                    BackColor="#E0E0E0" ReadOnly="True" onchange="ValidateDateText(this,'txtEndDate_CalendarExtender');"
                                                    Width="100px"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Format="<%$AppSettings:DateFormat%>" TargetControlID="txtEndDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtEndDate"
                                                    WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <span id="lblStartHours1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="lblStartHours" class="clsLabel">Start Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtStartHours" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblHoursFrequency1" runat="server" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblHoursFrequency" class="clsLabel">Hours Frequency</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtHoursFrequency" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblElaspedHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span1" class="clsLabel">Carry Forward Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCarryForwardHours" BackColor="#E0E0E0" ReadOnly="True" runat="server"
                                                    CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblEndHours1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="lblEndEndHours" class="clsLabel">End Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtEndHours" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                    ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="lblCurrentHours1" runat="server" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="lblCurrentHours" class="clsLabel">Current Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCurrentHours" runat="server" CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblElaspedHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="lbElaspedHrs" class="clsLabel">Elapsed Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtElaspedHrs" BackColor="#E0E0E0" ReadOnly="True" runat="server"
                                                    CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblRemainingHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="lblRemainingHrs" class="clsLabel">Remaining Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRemaining" BackColor="#E0E0E0" ReadOnly="True" runat="server"
                                                    CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top"></td>
                                            <td align="right" colspan="2" valign="top">
                                                <asp:UpdatePanel ID="upnlPBHBtns" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="tblNew">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPBH" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to Save"
                                                                        CausesValidation="true" ValidationGroup="rate" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPBHClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close screen" />
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
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopUpPBH" runat="server" TargetControlID="btnDummyPBH"
            PopupControlID="pnlChangePBH" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <!-- End PBH Detail -->
        <!-- PBH borrow Detail-->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyPBHBorrow" Text="Dummy PBH Borrow" />
        </div>
        <asp:Panel runat="server" ID="pnlBorrowPBH" Style="display: none">
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBorrowPBH">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlBorrowInnerPBH" Visible="false">
                        <table class="clstablelistout" id="Table1">
                            <tr>
                                <td>
                                    <table class="clstablelistin" id="Table2">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lbltitleborrow" CssClass="clstitle1" runat="server">Hour(s) Borrowing</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBorrowValidations">
                                                    <ContentTemplate>
                                                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="a" runat="server"
                                                            CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span id="Span8" class="clsLabelStar">*</span>
                                            </td>
                                            <td>
                                                <span id="Span9" class="clsLabel">Aircraft </span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:DropDownList ID="cmbMachineList" runat="server" AutoPostBack="true" CssClass="clsComboBox_Ajax"
                                                    DataTextField="RegNo" DataValueField="ID" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <span id="lblStartHours1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span13" class="clsLabel">Available Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtAvlHrs" BackColor="#E0E0E0" ReadOnly="True" runat="server" CssClass="clsTextBox_Ajax"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <span id="lblStartHours1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span10" class="clsLabel">Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtHours" runat="server" ToolTip="Enter Hours to be borrwed from above selected Aircraft"
                                                    CssClass="clsTextBox_Ajax" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblPbhHeader" runat="server" CssClass="clsLabelHeader">Details of PBH Aircraft</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblElaspedHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span14" class="clsLabel">Subscribed Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtHourFreq" BackColor="#E0E0E0" ReadOnly="True" runat="server"
                                                    CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <%--<span id="lblElaspedHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span11" class="clsLabel">Elapsed Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtElapsed" BackColor="#E0E0E0" ReadOnly="True" runat="server" CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<span id="lblRemainingHrs1" class="clsLabelStar">*</span>--%>
                                            </td>
                                            <td>
                                                <span id="Span12" class="clsLabel">Remaining Hours</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRem" BackColor="#E0E0E0" ReadOnly="True" runat="server" CssClass="clsTextBox_Ajax"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top"></td>
                                            <td align="right" colspan="2" valign="top">
                                                <asp:UpdatePanel ID="upnlPBHBorrowBtns" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="tblBorrowNew">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnBorrowPBH" runat="server" CssClass="clsButton_Ajax" Text="Ok"
                                                                        ToolTip="Click to Save" CausesValidation="true" ValidationGroup="a" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBorrowPBHClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                        TabIndex="0" Text="Close" ToolTip="Click to close screen" />
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
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopUpPBHBorrow" runat="server" TargetControlID="btnDummyPBHBorrow"
            PopupControlID="pnlBorrowPBH" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <!-- End PBH borrow Detail -->
        <input id="gridrowindex" type="hidden" value="" />
        <input id="gridrowaction" type="hidden" value="" />
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();

                var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
