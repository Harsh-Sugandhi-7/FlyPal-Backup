<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMPDMasterList.aspx.vb" Inherits="Flypal.wfMPDMasterList" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MPD List</title>
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
    <script language="JavaScript" type="text/javascript">

        function autoResizeCompList() {
            var newheight;
            var newwidth;

            if (document.getElementById) {
                newheight = document.getElementById('IframeCompList').contentWindow.document.body.scrollHeight;
                newwidth = document.getElementById('IframeCompList').contentWindow.document.body.scrollWidth;
            }
            document.getElementById('IframeCompList').height = (newheight + 25) + "px";
            document.getElementById('IframeCompList').width = (newwidth) + "px";
            document.getElementById('tbpnlCompList').height = (newheight) + "px";
            document.getElementById('tbpnlCompList').width = (newwidth) + "px";

            document.getElementById('TbContInst').height = (newheight) + "px";
            document.getElementById('TbContInst').width = (newwidth) + "px";


        }
    </script>
    <style type="text/css">
        .maxGridWidth {
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td style="width: 100%" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle" class="clsFormHeader">MPD List</span>
                                            </td>
                                          
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new MPD"
                                                                        CausesValidation="False" Text="Add New"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print List" visible="false"
                                                                        CausesValidation="False" Text="Print"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close screen"
                                                                        CausesValidation="False" Text="Close"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <cc2:TabContainer ID="TbContInst" runat="server" AutoPostBack="true">
                                                <cc2:TabPanel ID="tbpnlAssembly" runat="server" CssClass="clsPanel1">
                                                    <HeaderTemplate>
                                                        Assembly MPD
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                        <table class="clstablelistin" id="Table2" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table id="Table1">
                                                                                            <tr>
                                                                                                <%-- <td>
                                                                                                    <span id="lblAssemblyType" class="clsLabelAuto">Assembly Type</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                        DataValueField="ID" DataTextField="Name" AutoPostBack="True">
                                                                                                    </asp:DropDownList>
                                                                                                </td>--%>
                                                                                                <td>
                                                                                                    <span id="lblModel" class="clsLabelAuto">Model</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                        AutoPostBack="true" DataTextField="Name">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="Span1" class="clsLabelAuto">MPD No.</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtMPDNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter MPD No. to search"
                                                                                                        AutoPostBack="true" MaxLength="50"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <span id="lblMonitorType" class="clsLabelAuto">Type</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbMonitorType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                        AutoPostBack="True" DataValueField="ID" DataTextField="Name">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="lblATA" class="clsLabelAuto">ATA</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                                                        AutoPostBack="true" DataValueField="ID" DataTextField="ATAChapter">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <span id="lblDescription" class="clsLabelAuto">Description</span>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description to search"
                                                                                                        AutoPostBack="true" MaxLength="1000" TextMode="MultiLine" Width="275px"></asp:TextBox>
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
                                                                                    <td colspan="2">
                                                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:GridView ID="dgMPDMasterList" runat="server" AllowSorting="True" AllowPaging="true"
                                                                                            AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle" DataKeyNames="ID"
                                                                                            EnableViewState="true" GridLines="Horizontal"
                                                                                            PageSize="10" ShowHeaderWhenEmpty="True"
                                                                                            ToolTip="MPD List">
                                                                                            <RowStyle CssClass="clsdgItem" />
                                                                                            <FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
                                                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
                                                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                <asp:BoundField DataField="MPDTaskNumber" SortExpression="MPDTaskNumber" HeaderText="MPD Task No.">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                     <ItemStyle Wrap="false"  />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Description" HeaderText="Description">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DispATACode" SortExpression="DispATACode" HeaderText="ATA">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ServiceTypeName" SortExpression="ServiceTypeName" HeaderText="Type">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="TaskIntervalDescription" SortExpression="TaskIntervalDescription" HeaderText="Task Description">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="PrimaryModelName" SortExpression="PrimaryModelName" HeaderText="Primary Model">
                                                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MPDTypeNameCode" SortExpression="MPDTypeNameCode" HeaderText="MRB Categories"></asp:BoundField>
                                                                                                <asp:BoundField DataField="MPDSkillNameCode" HeaderText="Skill"></asp:BoundField>

                                                                                                <%--   <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>
                                                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>
                                                                                                <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:ButtonField>--%>
                                                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <%-- <span id="button">Login</span>--%>
                                                                                                        <div class="dropdown">
                                                                                                            <div class="dropdownbtn-content">
                                                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ViewRec" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("IsAttachmentAdded") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                                                        </div>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
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
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </cc2:TabPanel>
                                                <%--   <cc2:TabPanel ID="tbpnlCompList" runat="server" ClientIDMode="Static">
                                                <HeaderTemplate>
                                                    Component MPD
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <iframe id="IframeCompList" width="100%" height="200px" scrolling="no" marginheight="0"
                                                        frameborder="0" onload="autoResizeCompList()"></iframe>
                                                </ContentTemplate>
                                            </cc2:TabPanel>--%>
                                            </cc2:TabContainer>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
        <div>
            <script type="text/javascript">
                function CallCompMPDList() {
                    document.getElementById('IframeCompList').src = 'wfNewCompMPDList_Ajax.aspx'
                }


            </script>
        </div>
        <div>
            <!-- Part Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyPart" Text="Dummy Part" ClientIDMode="Static"></asp:Button>
            </div>
            <asp:Panel runat="server" ID="pnlPart" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframePart" frameborder="0" height="100%" allowtransparency="true" width="100%"
                    src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupPart" runat="server" TargetControlID="btnDummyPart"
                PopupControlID="pnlPart" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFramePartStateComplete() {
                    $("#btnDummyPart").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenPartWindowParent() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframePart").attr("src", "wfPart_AJAX.aspx?Type=pup");
                        // $("#IframePart").load(function () {
                        //                    var doc = IframePart.window;
                        //                    IframePart.SetPageLayout();

                        if (!$.browser.msie) {
                            $("#btnDummyPart").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }


                        //});


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForPart() {
                    var Partwindow = $find("<%=mdlPopupPart.ClientID %>");
                    //close Part popup window
                    Partwindow.hide();
                    //           release resources
                    $("#IframePart").attr("src", "JavaScript:''");
                    //call Part image button
                    $("#hdnBtnPart").click();
                }
            </script>
            <!-- End-->
        </div>
    </form>
    <script language="JavaScript" type="text/javascript">
        function CloseChildPage() {
            window.location.href = "index.aspx";
        }
    </script>
</body>
</html>
