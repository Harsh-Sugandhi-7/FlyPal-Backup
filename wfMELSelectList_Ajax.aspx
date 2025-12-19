<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELSelectList_Ajax.aspx.vb"
    Inherits="Flypal.wfMELSelectList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MEL List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <style type="text/css">
        #divCollapsiblePnl {
            float: left;
            vertical-align: middle;
            width: 100%;
            cursor: pointer;
        }

        #lblMastersSelection {
            vertical-align: middle;
            margin-left: 2px;
            width: 100%;
        }

        #divCollapsiblePnlImg {
            float: right;
            vertical-align: middle;
            margin-right: 5px;
            cursor: pointer;
        }

        #pnlAdvancedSearch {
            border: none;
        }
    </style>
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
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMELList" CssClass="clsFormHeader" runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD List", "MEL List") %>'></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                                        Text="Close" Visible="true"></asp:Button>
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
                                    <table width="100%">
                                        <tr style="display: block; margin-top: 05px;">
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <span id="Span4" class="clsLabelAuto">Model </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="4"
                                                                        onchange="SetModelIdonChange(this,'txtModel_Autocomplete')" ToolTip="Enter Model."
                                                                        AutoPostBack="True" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ID="txtModel_Autocomplete" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                                        ServicePath="wfMELList_Ajax.aspx" ServiceMethod="GetModelList" TargetControlID="txtModel"
                                                                        UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetModelID">
                                                                    </cc2:AutoCompleteExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="clsLabelAuto">Description </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server"
                                                                        CssClass="clsTextBoxTagSearch" ToolTip="Enter Description."
                                                                        AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to find list of ADD as per searching criteria", "Click to find list of MEL as per searching criteria") %>'
                                                            Text="Find Now" Visible="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
                                        <div>
                                            <div id="divCollapsiblePnl">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lblMastersSelection" class="clsLabelHeader">Advance Search
                                                            </span>
                                                        </td>
                                                        <td align="right">
                                                            <div id="divCollapsiblePnlImg">
                                                                <image id="imgMasters" src="images/collapse_blue.jpg"
                                                                    alternatetext="(Show Details...)" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblATA" class="clsLabel">ATA</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                                                DataTextField="ATAChapter" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span id="lblSubATA" class="clsLabel">Sub ATA</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
                                                                AutoPostBack="true" DataTextField="SubATAChapter">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span id="lblRectificationInterval" class="clsLabelAuto">Rectification Interval</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlRectificationInterval" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbMELCategory" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
                                                                DataValueField="ID" DataTextField="Name" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblItemSequenceNo" class="clsLabelAuto">Item Sequence No.</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlItemSequenceNo" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtItemSequenceNo" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                                ToolTip="Enter Item Sequence No."></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span id="lblRevisionNo" class="clsLabelAuto">Revision No.</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:UpdatePanel ID="upnlRevisionNo" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtRevisionNo" runat="server" CssClass="clsTextBoxTagSearch" AutoPostBack="true"
                                                                ToolTip="Revision No."></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMELList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMELList" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            PageSize="10" DataKeyNames="ID" AutoGenerateColumns="False" AllowSorting="True"
                                                            CellPadding="5" GridLines="Horizontal" AllowPaging="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <%--1--%>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="ATACodeSubATACode" SortExpression="ATACodeSubATACode"
                                                                    HeaderText="ATA-SubATA">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="Item Sequence No.">
                                                                    <HeaderStyle Wrap="true" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" Font-Bold="true"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:BoundField DataField="PageNo" SortExpression="PageNo" HeaderText="Page No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--5--%>
                                                                <asp:BoundField DataField="MELDescription" SortExpression="MELDescription" HeaderText="Description">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--6--%>
                                                                <asp:BoundField DataField="RevisionNo" SortExpression="RevisionNo" HeaderText="Revision No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="True"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--7--%>
                                                                <asp:BoundField DataField="RevisionDateFormatted" HeaderText="Revision Date">
                                                                    <HeaderStyle HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="left" Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--8--%>
                                                                <asp:BoundField DataField="MELCategoryName" SortExpression="MELCategoryName" 
                                                                    HeaderText="Rectification Interval">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <%--9--%>
                                                                <asp:BoundField DataField="MakeMELQty" HeaderText="Number Installed">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--10--%>
                                                                <asp:BoundField DataField="FlyMELQty" HeaderText="Number Dispatched">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="true"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--11--%>
                                                                <asp:BoundField DataField="FrequencyInDays" HeaderText="Frequency In Days">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--12--%>
                                                                <asp:BoundField DataField="FrequencyInHours" HeaderText="Frequency In Hours">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--13--%>
                                                                <asp:BoundField DataField="FrequencyInCycles" HeaderText="Frequency In Cycles">
                                                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--14--%>
                                                                <asp:TemplateField HeaderText="Applicable">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="IsApplicable" runat="server"
                                                                            Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                            Enabled="False" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--15--%>
                                                                <asp:ButtonField CommandName="SelectRec" HeaderText="Select"
                                                                    Text="Select" HeaderStyle-HorizontalAlign="Left"></asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
            ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
            CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
            CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
            SuppressPostBack="false" />
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
            runat="server">
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
        <%-- Autocomplete functions to set id--%>
        <asp:HiddenField ID="hdnModelId" runat="server" ClientIDMode="Static" />
        <script type="text/javascript">
            function SetModelID(source, e) {
                //get id from autocomplete list
                var node;
                var value = e.get_value();

                if (value) node = e.get_item();
                else {
                    value = e.get_item().parentNode._value;
                    node = e.get_item().parentNode;
                }
                //Set id to relevent hidden field 
                var textbox;
                if (source._id == "txtModel_Autocomplete") {
                    textbox = document.getElementById('hdnModelId');
                }
                textbox.value = value;
            }
            //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
            function SetModelIdonChange(source, extenderid) {
                var popup = $find(extenderid);
                var complist = popup.get_completionList();
                var text = $(source).val().toLowerCase();
                for (var i = 0; i < complist.childNodes.length; i++) {
                    var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                    if (text == texttocompare) {
                        var val = complist.childNodes[i]._value;

                        if (extenderid == "txtModel_Autocomplete") {
                            textbox = document.getElementById('hdnModelId');
                        }
                        textbox.value = val;
                        return;
                    }

                }

                if (extenderid == "txtModel_Autocomplete") {
                    document.getElementById('hdnModelId').value = '';
                }
            }
        </script>
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
                    parent.IFrameMELMasterStateComplete();
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
