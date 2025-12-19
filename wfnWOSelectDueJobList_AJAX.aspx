<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOSelectDueJobList_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOSelectDueJobList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Due Jobs</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script src="json2.js" type="text/javascript"></script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

	<style type="text/css">
        .GbiHighlight {
            background-color: Aqua;
        }
    </style>

</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js">
            function openledgersame(FileName) {
                window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
            }
        </script>        
        <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
        <script type="text/javascript">


            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.cbSelectRow').change(function () {
                    // detect if the checkbox is checked
                    var checked = $(this).prop('checked');
                    // gets the table row indiect parent
                    var trParent = $(this).closest('tr');
                    // add or remove the css class according to the check state
                    if (checked == true)
                        trParent.addClass('clslightColor')
                    else
                        trParent.removeClass('clslightColor');
                })
                    // the each is used when postback is triggered with checked rows
                    .each(function (index, element) {
                        var checked = $(element).prop('checked');
                        if (checked == true)
                            $(element).closest('tr').addClass('clslightColor');
                        else
                            $(element).closest('tr').removeClass('clslightColor');
                    });
                // select all click
                $("#chkSelectAll").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });

            });

        </script>
        <!-- End-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td align="right">
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                        <table class="clsTablelistin" id="tblinner">
                            <tr>

                                <td colspan="2" class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Due Job(s)</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="UpnlDone" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2" border="0" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnDoneTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Done" ToolTip="Click to add checked records"
                                                                       />
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Back"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="imgFindNow" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvControlValidator" runat="server" Display="None" CssClass="clslabelAuto"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="2">
                                    <table id="Table1" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblDuePeriodList" runat="server" CssClass="clsLabelHeader"> Due Period List</asp:Label>
                                            </td>
                                            <td align="right" valign="top">
                                                <asp:UpdatePanel ID="UpnlFindNow" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table5" cellpadding="1" cellspacing="1">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:ImageButton ID="imgFindNow" runat="server" 
                                                                        ImageUrl="~/images/Search2.png" 
                                                                        CssClass="clsSearch2btn"
                                                                        ToolTip="Click to find as per searching criteria" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:UpdatePanel ID="UpnlAsOnDat" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" cellpadding="0" class="clsTable1">
                                                            <tr>
                                                                <td valign="top">
                                                                    <table id="Table10" border="0" cellpadding="1" cellspacing="1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblAsOnDat" runat="server" CssClass="clsLabel">As On Date</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAsOnDate" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearch"
                                                                                    Width="100px"></asp:TextBox>
                                                                                <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
                                                                                <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtAsOnDate"
                                                                                    WatermarkCssClass="watermarked" WatermarkText="<%$AppSettings:DateFormat%>" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpnlDuePeriodList" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table16" cellpadding="1" cellspacing="1" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgDuePeriod" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                                                        ToolTip="Due List." GridLines="Horizontal" CellPadding="7">
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="PeriodName" HeaderText="Period" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Limit" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtLimit" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width="70px"
                                                                                        MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>'>
                                                                                    </asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                                <td valign="top">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <span id="lblNote" class="clsLabel">Note / Interval / Reference/ Zone</span>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkZeroFrequency" runat="server" CssClass="clsLabelAuto" AutoPostBack="true"
                                                                                    CausesValidation="true" ToolTip='Check to see only "NOT APPLICABLE"  records'
                                                                                    Text='Show "ZERO FREQUENCY" records'></asp:CheckBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <span id="Span1" class="clsLabel">(Shows all ON Condition (No Limit) and No Frequency
                                                                                Maintenance Activities)</span>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table6" align="left" border="0" cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Jobs as per criteria :  Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" colspan="2">
                                    <asp:UpdatePanel ID="UpnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div style="height: 420px; overflow: auto;">
                                                            <asp:GridView ID="dgDueJob" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False"
                                                                ToolTip="Due Job." PageSize="25" ShowHeaderWhenEmpty="True" GridLines="Horizontal" CellPadding="7">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" id="chkSelectAll" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="TaskNo" HeaderText="Task No./Directive No."></asp:BoundField>
                                                                    <asp:BoundField DataField="LogBook" HeaderText="Assembly Info." HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OnAssemblyOrComponent" HeaderText="On Assembly / Component">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CodeType" HeaderText="Type">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Reference" HeaderText="Reference">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="JobDescriptionDetail" HeaderText="Info" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" HeaderText="Note">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Freq3ForGrid" HeaderText="Frequency" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Right" Width="70px"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SinceNewForGrid" HeaderText="Since New" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Right" Width="70px"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneAt2ForGrid" HeaderText="Done At" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Right" Width="70px"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueAsOf2ForGrid" HeaderText="Due As Of Airframe"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RemainingTime2ForGrid" HeaderText="Remaining Time" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="EstimatedDate" HeaderText="Estimated Date">
                                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="WONumber" HeaderText="WO Number" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
																	<asp:BoundField DataField="Zone" HeaderText="Zone">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
																	</asp:BoundField>
                                                                    <asp:BoundField DataField="EstimatedHoursforGrid" HeaderText="Estimated Man Hrs."
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="View Required Spare List" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnImageButton" runat="server" CommandArgument='<%# Eval("StatusMasterID") %>'
                                                                                CommandName="ViewSpareList" Style="height: 30px; width: 30px" ImageUrl="~/icons/iconfinder_-_Eye-Show-View-Watch-See_3844411.ico" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="StatusMasterID" HeaderText="Status Master ID" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <PagerStyle HorizontalAlign="Right" BorderStyle="Solid" />
                                                                <PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:UpdatePanel ID="UpnlDone1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <%--<td align="right">
                                                        <asp:Button ID="btnDone" runat="server" CssClass="clsButton" ToolTip="Click to add checked records"
                                                            Text="Done"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Back"></asp:Button>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="imgFindNow" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <!--Dummy panel to open modelpopup-->
                                <td>
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnSpareList" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnImportLogs" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <!--End -->
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
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
        <!-- SpareList popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySpareList" Text="Maintenance Activity" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlSpareList" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSpareList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSpareList" runat="server" TargetControlID="btnDummySpareList"
            PopupControlID="pnlSpareList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSpareListStateComplete() {
                $("#btnDummySpareList").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSpareListWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSpareList").attr("src", "wfSpareList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummySpareList").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSpareList() {
                var SpareListwindow = $find("<%=mdlPopupSpareList.ClientID %>");
                //close Task Card Tool popup window
                SpareListwindow.hide();
                //           release resources
                $("#IframeSpareList").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnSpareList").click();
            }
        </script>
        <!-- End-->
    </form>

</body>
</html>
