<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfNewCompADSBList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfNewCompADSBList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>AD/SB List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <style type="text/css">
        .maxGridWidth
        {
            max-width: 1000px;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <span id="lblAssemblyType" class="clsLabelAuto">Assembly Type</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox_Ajax"
                                                                    DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <span id="lblModel" class="clsLabelAuto">Model</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                    AutoPostBack="true" DataTextField="ModelName">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <span id="lblPartNo" class="clsLabelAuto">Part No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPartDescription" autocomplete="off" runat="server" CssClass="clsTextBox1_Ajax"
                                                                    AutoPostBack="True" onchange="SetPartIdonChange(this,'txtPartDescription_AutoCompleteExtender')"></asp:TextBox>
                                                                <!-- AutoComplete Extender-->
                                                                <cc2:AutoCompleteExtender ID="txtPartDescription_AutoCompleteExtender" runat="server"
                                                                    DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="1"
                                                                    CompletionInterval="1" ServicePath="" ServiceMethod="GetPartNoDescriptionList"
                                                                    TargetControlID="txtPartDescription" UseContextKey="True" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                    CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                    OnClientItemSelected="SetID">
                                                                </cc2:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                                <span id="Span1" class="clsLabelAuto">Mod No.</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtModNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Mod No. to search"
                                                                    AutoPostBack="true" MaxLength="150" Width="275px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblMonitorType" class="clsLabelAuto">Monitor Type</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsComboBoxDouble_Ajax"
                                                                    AutoPostBack="True" DataValueField="ID" DataTextField="CodeType">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBoxLong_Ajax"
                                                                    AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                                </asp:DropDownList>
                                                            </td>
                                                            
                                                            <td >
                                                                <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Description to search"
                                                                    AutoPostBack="true" MaxLength="1000" TextMode="MultiLine" Width="370px"></asp:TextBox>
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
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Select Part No. and then Click on Add New button to add new Part Directive"
                                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List" Visible="false"
                                                                            CausesValidation="False" Text="Print"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                                            CausesValidation="False" Text="Close"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgPartMonitorModList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                        EmptyDataText="No Records Found..." DataKeyNames="ID" AutoGenerateColumns="False"
                                                        ToolTip="AD/SB List">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="PartName" SortExpression="PartName" HeaderText="Part No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Number" SortExpression="Number" HeaderText="Mod No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Note" HeaderText="Note">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Select Part No. and then Click on Add New button to add new Part Directive"
                                                        CausesValidation="False" Text="Add New"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print List" Visible="false"
                                                        CausesValidation="False" Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close screen"
                                                        CausesValidation="False" Text="Close"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
    <%-- Autocomplete functions to set id--%>
    <script type="text/javascript">
        function SetID(source, e) {
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
            if (source._id == "txtPartDescription_AutoCompleteExtender") {
                textbox = document.getElementById('PartID');
            }
            textbox.value = value;
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function SetPartIdonChange(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (extenderid == "txtPartDescription_AutoCompleteExtender") {
                    textbox = document.getElementById('PartID');
                }
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    textbox.value = val;
                    return;
                }
                else {
                    textbox.value = '';
                    return;
                }

            }

            if (extenderid == "txtPartDescription_AutoCompleteExtender" && text == "") {
                document.getElementById('PartID').value = '';
            }
        }
        
    </script>
    </form>
    <script type="text/javascript">
        function CallParentFunction() {
            window.parent.autoResizeCompList();
        }
        function CallCloseChildPage() {
            window.parent.CloseChildPage();
        }
    </script>
</body>
</html>
