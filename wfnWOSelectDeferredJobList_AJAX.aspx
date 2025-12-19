<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOSelectDeferredJobList_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOSelectDeferredJobList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="HEAD1" runat="server">
    <title>Deferred Jobs</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="Link2" type="text/css" rel="stylesheet" />
    <script src="json2.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
    </script>
    <style type="text/css">
        .GbiHighlight {
            background-color: Aqua;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
        <script type="text/javascript">
            $(document).ready(function () {
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td align="Left">
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table class="clsTablelistin" id="tblinner">
                            <tr>

                                <td class="clsFormHeader1" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Deferred Job(s)</asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="UpnlDone1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnDone" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add checked records"
                                                                        Text="Done"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" CausesValidation="False"></asp:Button>
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
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpnlAsOnDat" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAsOnDat" runat="server" CssClass="clsLabel" Width="93px">As On Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Table10" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td></td>
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
                                <td align="right">
                                    <asp:UpdatePanel ID="UpnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table5" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton" Visible="False" ToolTip="Click to find as per searching criteria"
                                                            Text="Find Now"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table6" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Deferred Jobs as per criteria :  Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <%--<asp:UpdatePanel ID="UpnlDone" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table2" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnDoneTop" runat="server" CssClass="clsButton" Visible="<%# mnWODefferedJobs.Count >25 %>"
                                                        ToolTip="Click to add checked records" Text="Done"></asp:Button>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton" Visible="<%# mnWODefferedJobs.Count >25 %>"
                                                        Text="Back" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:UpdatePanel ID="UpnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgDeferredJob" runat="server" CssClass="clsGridNewStyle" ToolTip="Deferred  Job" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
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
                                                                <asp:BoundField DataField="WONumber" HeaderText="WO No.">
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobDescription" HeaderText="Description">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DueAsOf" HeaderText="Due As Of">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobAction" HeaderText="Action">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobEstimatedTime" HeaderText="Est. Man Hr">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobStartDateFormatted" HeaderText="Start Date">
                                                                    <HeaderStyle  HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                    <FooterStyle Wrap="False"></FooterStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobCloseDateFormatted" HeaderText="Close Date">
                                                                    <HeaderStyle  HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                    <FooterStyle Wrap="False"></FooterStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WOJobActualTime" HeaderText="Actual Man Hr.">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="WOJobTypeName" HeaderText="Job Type">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerStyle HorizontalAlign="Right" BorderStyle="Solid" />
                                                            <PagerSettings NextPageText="Next" PreviousPageText="Prev"></PagerSettings>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td valign="top" align="right" colspan="2">
                                    <asp:UpdatePanel ID="UpnlDone1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnDone" runat="server" CssClass="clsButton" ToolTip="Click to add checked records"
                                                            Text="Done"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Back" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
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
    </form>
</body>
</html>
