<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTaskCardHistory_Ajax.aspx.vb"
    Inherits="Flypal.wfTaskCardHistory_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Task Card History</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
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
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">History for Task Card</span>
                                        </td>
                                        <%--<td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server" ToolTip="Click to Print Task Card History"
                                                                    Text="Print"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnBack" runat="server" Text="Close" ToolTip="Click to close Task Card History screen"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSetNewStyle" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Search Criteria</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTaskCardNo" runat="server" CssClass="clsLabelAuto">Task Card No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtTaskNo" runat="server" MaxLength="100"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftList" runat="server" AutoPostBack="true" 
                                                                        DataTextField="RegNo" DataValueField="ID" Width="100px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="Span1" class="clsLabelAuto">Assembly</span>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftAssembly" runat="server" AutoPostBack="true"
                                                                        DataTextField="ModelSerialNoPostion" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblPart" class="clsLabelauto">Part No.</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtPart" runat="server" ToolTip="Enter Part"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabelauto">Serial No. </asp:Label>
                                                                </td>
                                                                <td colspan="4">
                                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSerialNo" runat="server" ToolTip="Enter Serial Number"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                    Text="Find Now" ToolTip="Click to find list of History of Task Card as per searching criteria"
                                                                    ValidationGroup="1" />--%>

                                                               

                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                ToolTip="Click to find list of History of Task Card as per searching criteria" ></asp:ImageButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtTaskNo_Autocomplete" runat="server"
                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                            CompletionInterval="1" ServicePath="wfTaskCardHistory_Ajax.aspx" ServiceMethod="GetTaskCardList"
                                            TargetControlID="txtTaskNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                        </cc2:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblCompInformation" class="clsLabelHeader">Task Card Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchMultilineNewstyle" ID="txtDescription" runat="server" 
                                                        ReadOnly="True" BackColor="#E0E0E0" TextMode="MultiLine" Width="225px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="Span2" class="clsLabelAuto">INSP Type / Interval</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtFreq" runat="server" ReadOnly="True"
                                                        BackColor="#E0E0E0" Width="225px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span3" class="clsLabelAuto">ATA</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtATA" runat="server" BackColor="#E0E0E0"
                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <span id="lblReference" class="clsLabelAuto">Reference</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtReference" runat="server" BackColor="#E0E0E0"
                                                        Width="225px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td style="height: 43px">
                                            <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">History of Task Card as per criteria :  Record(s) found.</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrintTop" runat="server"  ToolTip="Click to Print Task Card History"
                                                                    Visible="False" Text="Print"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnBackTop" runat="server"  Text="Close"
                                                                    Visible="False" ToolTip="Click to close Task Card History screen" CausesValidation="False">
                                                                </asp:Button>
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
                                <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgMonitorInspStatusList" runat="server"  ToolTip="Task Card History"
                                            CellPadding="5" CssClass="clsGridNewStyle"  ForeColor="Black" GridLines="Horizontal"
                                            ShowHeaderWhenEmpty="true" PageSize="5" AllowSorting="True" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                            <HeaderStyle  BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            <Columns>
                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="ModelMonitorInspID" HeaderText="ModelMonitorInspID">
                                                </asp:BoundField>
                                                <asp:BoundField Visible="False" DataField="ATA" SortExpression="ATAChapter" HeaderText="ATA  Chapter">
                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MachineName" HeaderText="Aircraft">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ModelNameSerialNo" HeaderText="Maintenance Info" HtmlEncode="false">
                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MonitorInfo" HeaderText="Type" HtmlEncode="false">
                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneWONo" HeaderText="Work Order No.">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneOnValueFormatted" HeaderText="Done On Value" HtmlEncode="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DoneRemark" HeaderText="Remark" HtmlEncode="false">
                                                    <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPrint" runat="server" ToolTip="Click to Print Task Card History"
                                                        Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnBack" runat="server" Text="Close" ToolTip="Click to close Task Card History screen"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForInspectionHistory();
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
                    parent.IFrameInspectionHistoryStateComplete();
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
    <%--End--%>
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
    </form>
</body>
</html>
