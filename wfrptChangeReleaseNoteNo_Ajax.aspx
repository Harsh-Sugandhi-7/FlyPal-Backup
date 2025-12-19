<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptChangeReleaseNoteNo_Ajax.aspx.vb"
    Inherits="Flypal.wfrptChangeReleaseNoteNo_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Change Release Note No./Date</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblmain" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Change Release Note No./Date</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to close Change Release Note No./Date screen"
                                                                        CausesValidation="False"></asp:Button>
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
                                <td>
                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <table id="Table1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblPartNo" class="clsLabelMedium">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblReceiptNo" style="width: 70px;" class="clsLabel">Receipt No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="5" onpaste="return false"></asp:TextBox>
                                                                    <%--onpaste="return false"--%>
                                                                </td>
                                                                <td>
                                                                    <span id="lblReleaseNoteNo" class="clsLabel">Release Note No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                                        MaxLength="200"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table4">
                                                            <tr>
                                                                <td>
<%--                                                                    <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        Text="Find Now" ToolTip="Click to find as per criteria"></asp:Button>--%>

                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                        ToolTip="Click to find list as  per searching criteria" />
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
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:GridView ID="dgPartSearch" runat="server" PageSize="25" AutoGenerateColumns="False"
                                                             AllowPaging="True" AllowSorting="True" DataKeyNames="ID" ClientIDMode="Static"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            EnableViewState="false" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="ItemID" HeaderText="ItemID"></asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False"  Width="125px" HorizontalAlign="Left">
                                                                    </HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
                                                                    <ItemStyle></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
                                                                    <HeaderStyle Wrap="False"  HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
                                                                    <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReleaseNoteDateFormatted" HeaderText="Release Note Date">
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Change" HeaderText="Change" CommandName="Change">
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:ButtonField>
                                                                <asp:BoundField Visible="False" DataField="ReceiptID" HeaderText="ReceiptID"></asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                    <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to close Change Release Note No./Date screen"
                                                            CausesValidation="False"></asp:Button>
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
    <!-- Change Release Note No-->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyChangeReleaseNoteNo" Text="Dummy Change Release Note No" />
    </div>
    <asp:Panel runat="server" ID="pnlChangeReleaseNoteNo" Style="display: none">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlChangeReleaseNoteNo">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlReleaseNoteNo" Visible="false">
                    <table class="clstablelistout" id="Table3" border="0">
                        <tr>
                            <td>
                                <table class="clstablelistin" id="TABLE5" border="0">
                                    <tr>
                                        <td colspan="5" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="Label1" class="clsFormHeader">Change Release Note No./Date</span>
                                                    </td>
                                                    <td align="right">
                                                        <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                                            <tr>
                                                                 <td>
                                                                    <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save new Release Note No./Date"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseChangeReleaseNoteNo" runat="server" CssClass="clsbtnH clsinfoH"
                                                                        Text="Close" ToolTip="Click to close Change Release Note No./Date screen" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary">
                                            </asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvChangeReleaseNoteDate" runat="server" CssClass="clslabelauto"
                                                ControlToValidate="txtChangeReleaseNoteDate" Display="None" ErrorMessage="Defect  required."
                                                OnServerValidate="customvalidate"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 14px">
                                        </td>
                                        <td>
                                            <span id="lblCurrentReleaseNoteNo" class="clsLabelAuto">Old Release Note No. </span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCurrentReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                ReadOnly="True" MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblCurrentReleaseNoteDate" class="clsLabelAuto">Old Release Note Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCurrentReleaseNoteDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                runat="server"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calCurrentReleaseNoteDate_CalendarExtender" runat="server"
                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtCurrentReleaseNoteDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtCurrentReleaseNoteDate" ID="CurrentReleaseNoteDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 14px">
                                            <span id="lblChangeLocation1" class="clsLabelStar" visible="False">*</span>
                                        </td>
                                        <td>
                                            <span id="lblChangeLocation" class="clsLabelAuto">New Release Note No.</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChangedReleaseNoteNo" runat="server" CssClass="clsTextBoxTagSearch"
                                                MaxLength="200" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblChangeReleaseNoteDate" class="clsLabelAuto">New Release Note Date</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChangeReleaseNoteDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
                                                runat="server"></asp:TextBox>
                                            <cc2:CalendarExtender ID="calChangeReleaseNoteDate_CalendarExtender" runat="server"
                                                CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtChangeReleaseNoteDate">
                                            </cc2:CalendarExtender>
                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtChangeReleaseNoteDate" ID="ChangeReleaseNoteDate_watermarkextender"
                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                WatermarkCssClass="clsDateTextBox">
                                            </cc2:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="5" align="right">
                                            <table id="Table6" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save new Release Note No./Date">
                                                        </asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCloseChangeReleaseNoteNo" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close Change Release Note No./Date screen" CausesValidation="False">
                                                        </asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopUpChangeReleaseNoteNo" runat="server" TargetControlID="btnDummyChangeReleaseNoteNo"
        PopupControlID="pnlChangeReleaseNoteNo" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <!-- End Change Part Expity Info -->
    <input id="gridrowindex" type="hidden" value="" />
    <input id="gridrowaction" type="hidden" value="" />
    <%-- Row Highlight--%>
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
                $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
                if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
                    var elem;
                    var tempaction = $("#gridrowaction").val(); //action to be performed

                    if (tempaction == "close") {
                        $("#dgPartSearch tr:eq(" + tempval + ")").removeClass('activerow');
                        $("#gridrowaction").val('');
                        return;
                    }
                    //change Expiry Info button ok event
                    //blink Expiry columns of the row for perticular interval
                    else if (tempaction == "ReleaseNote") {
                        $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
                        elem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(5),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(6)");
                        $("#gridrowaction").val('');
                    }
                    else {
                        return;
                    }
                    //blink column function
                    timeoutforblink = setInterval(function () {

                        if (elem.hasClass('activerow')) {
                            elem.removeClass('activerow');
                        }
                        else {
                            elem.addClass('activerow');
                        }

                    }, 500);
                    //stop blink column
                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
                }


            }
        }

        function BeginRequestHandler(sender, args) {
            clearTimeout(timerId);
            element = args.get_postBackElement();

            //change Release Note Info popup ok button event occur
            if (element.id == "MSGBoxCtrl_btnYes") {
                hideRowHighlight = true;
                $("#gridrowaction").val('ReleaseNote');
            }
            else if (element.id == "btnOk") {
                var ReleaseNoteNo = $get("txtChangedReleaseNoteNo").value;
                var ReleaseNoteDate = $get("txtChangeReleaseNoteDate").value;
                if (ReleaseNoteNo != '' && ReleaseNoteDate != '') {
                    hideRowHighlight = true;
                    $("#gridrowaction").val('ReleaseNote');
                }
            }

            //any of change popup close button event occur 
            else if (element.id == "btnCloseChangeReleaseNoteNo") {
                hideRowHighlight = true;
                $("#gridrowaction").val('close');
            }
            //change parttype ||change location link event occur
            //reset rowindex value if other grid event occurs
            else if (element.id == "dgPartSearch") {
                if ($("#gridrowaction").val() != "gridrow") {
                    $("#gridrowindex").val('');
                }
            }
            //any other events
            else {
                //$("#gridrowindex").val('');
            }
        }

        //stop blinking
        function TimeOut(val, action) {
            var tempelem;

            if (action == "ReleaseNote") {
                tempelem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(5),#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(6)");
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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dgPartSearch tr td a").live("click", function () {
                var temp = $(this).parent().parent()[0].rowIndex;
                $("#gridrowindex").val(temp);
                $("#gridrowaction").val('gridrow');
            });
        });
    </script>


    </form>
</body>
</html>
