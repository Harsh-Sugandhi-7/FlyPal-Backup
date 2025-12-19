<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDeferredList_Ajax.aspx.vb"
    Inherits="Flypal.wfDeferredList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Deferred List</title>
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
                                                <span id="lblDeferredList" class="clsFormHeader">Deferred List</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlTopActionButton" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnTopAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Deferred Details"
                                                                        Text="Add New"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
                                                                        Text="Print"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnTopClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                                        Text="Close"></asp:Button>
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
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>
                                                                    <span id="lblModel" class="clsLabelAuto">Model </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="clsTextBoxTagSearch" onchange="SetModelIdonChange(this,'txtModel_Autocomplete')"
                                                                        ToolTip="Enter Model." AutoPostBack="True"></asp:TextBox>
                                                                    <cc2:AutoCompleteExtender ID="txtModel_Autocomplete" runat="server" DelimiterCharacters=""
                                                                        Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1" CompletionInterval="1"
                                                                        ServicePath="wfDeferredList_Ajax.aspx" ServiceMethod="GetModelList" TargetControlID="txtModel"
                                                                        UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                        CompletionListHighlightedItemCssClass="ac_over_Main" OnClientItemSelected="SetModelID">
                                                                    </cc2:AutoCompleteExtender>
                                                                </td>
                                                                <td>
                                                                    <span id="lblDescription" class="clsLabelAuto">Description </span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description."
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
                                                        <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to find list"
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
                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                        <div>
                                            <div style="float: left; vertical-align: middle; width: 100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                                class="clsLabelHeader">Advance Search</span>
                                                        </td>
                                                        <td align="right">
                                                            <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
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
                                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
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
                                                            <asp:DropDownList ID="cmbSubATAList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                AutoPostBack="true" DataTextField="SubATAChapter">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span id="lblRectificationInterval" class="clsLabelAuto">Category</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlRectificationInterval" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbDeviationCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
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
                                                    <span id="lblRevisionNo" class="clsLabelAuto">Issue No./Rev. No.</span>
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
                                    <asp:UpdatePanel ID="upnlDeviationLists" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgDeviationLists" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="True"
                                                            AllowPaging="true" PageSize="25" DataKeyNames="ID" AutoGenerateColumns="False"
                                                            AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="PrimaryModelName" SortExpression="PrimaryModelName" HeaderText="Model">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="True"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ATACodeSubATACode" SortExpression="ATACodeSubATACode"
                                                                    HeaderText="ATA">
                                                                    <HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="false"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemNo" SortExpression="ItemNo" HeaderText="Item Sequence No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" Font-Bold="true"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PageNo" SortExpression="PageNo" HeaderText="Page No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevisionNo" SortExpression="RevisionNo" HeaderText="Issue No./Rev. No.">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RevisionDateFormatted" HeaderText="Revision Date">
                                                                    <HeaderStyle HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="left" Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DeviationCategoryName" SortExpression="DeviationCategoryName" HeaderText="Category">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="QtyInstalled" HeaderText="Qty. Installed">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="HoursLimit" HeaderText="Hours Limit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CyclesLimit" HeaderText="Cycles Limit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DaysLimit" HeaderText="Days Limit" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="false"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax" dir="ltr">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlBottomActionButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnBottomAdd" runat="server" CssClass="clsbtnH clsinfoH"
                                                            Text="Add New" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBottomClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                            Text="Close" Visible="false"></asp:Button>
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
    </form>
</body>
</html>
