<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfPartList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>Part List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame


    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table id="tblLedgerList" class="clstablelistin">
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td class="clsFormHeader1Newstyle">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <span id="lblPartList" class="clsFormHeader">Part List</span>

                                                            </td>
                                                            <td align="right">
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                                                    <ContentTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add New"
                                                                                        ToolTip="Click to Add New Part"></asp:Button>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                                        ToolTip="Click to close Part List screen"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnAddTop" EventName="click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnCloseTop" EventName="click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                        class="fa fa-star fa-spin fa-5x circle-icon"
                                                        title="Mark As Favourites"></i>
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table1" width="100%">
                                                    <tr>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td><span class="clsLabelAuto">Part No.</span> </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part No."></asp:TextBox>
                                                                    </td>
                                                                    <td><span class="clsLabelAuto">Description</span> </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Description"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right" valign="top" >
                                                                 <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:ImageButton ID="btnSearch" runat="server" CssClass="clsSearch2btn" ImageUrl="~/images/Search2.png" ToolTip="Click to find the list of Part as per searching criteria" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" valign="top">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                                        <div>
                                                                            <div style="float: left; vertical-align: middle; width: 100%">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td><span id="lblMastersSelection" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px; width: 100%">Advance Search</span> </td>
                                                                                        <td align="right">
                                                                                            <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                                                                <image id="imgMasters" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" valign="top">
                                                            <asp:UpdatePanel ID="upnlMoreSearch" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" DefaultButton="btnSearch" Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td><span class="clsLabelAuto">Category</span> </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCategory" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Category"></asp:TextBox>
                                                                                            </td>
                                                                                            <td><span class="clsLabelAuto">Location</span> </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Location"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td><span class="clsLabelAuto">Unit</span> </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Unit"></asp:TextBox>
                                                                                            </td>
                                                                                            <td><span class="clsLabelAuto">Serialized Status</span> </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbSerialisedStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" EnableViewState="false">
                                                                                                    <asp:ListItem Selected="true" Value="0">ALL</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Serialized</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Non Serialized</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <cc2:CollapsiblePanelExtender ID="clpAdvancedSearch" runat="Server" BehaviorID="clpMastersBehaviour" ClientIDMode="Static" CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" CollapsedImage="~/images/expand_blue.jpg" CollapsedSize="0" CollapsedText="(Show Details...)" ExpandControlID="ClpnlAdvancedSearch" ExpandedImage="~/images/collapse_blue.jpg" ExpandedText="(Hide Details...)" ImageControlID="imgMasters" SuppressPostBack="false" TargetControlID="pnlAdvancedSearch" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <span id="lblInfo" class="clsLabelAuto">Select Part from the list. Click on Edit link
                                        to modify the selected Part. Click on Delete link to delete the selected Part. Click
                                        on Add New button to add a new Part. Click on View link to view the file of selected
                                        Part.</span>
                                    </td>
                                    <td align="right"></td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlResult" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">List of Part as per criteria : Record(s) found.</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlShowEntries" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall" ID="cmbShowE" runat="server" Width="55px"
                                                    AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                                                    <asp:ListItem Value="0">5</asp:ListItem>
                                                    <asp:ListItem Value="1">10</asp:ListItem>
                                                    <asp:ListItem Value="2">15</asp:ListItem>
                                                    <asp:ListItem Value="3">20</asp:ListItem>
                                                    <asp:ListItem Value="4" Selected="True">25</asp:ListItem>
                                                    <asp:ListItem Value="5">30</asp:ListItem>
                                                    <asp:ListItem Value="6">40</asp:ListItem>
                                                    <asp:ListItem Value="7">45</asp:ListItem>
                                                    <asp:ListItem Value="8">50</asp:ListItem>
                                                    <asp:ListItem Value="9">55</asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div style="width: 100%">
                                                    <asp:GridView ID="gdvItem" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                        DataKeyNames="ID" EnableViewState="True" ForeColor="Black" GridLines="Horizontal" PageSize="25" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="Id" HeaderText="Id"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AltTypeName" SortExpression="AltTypeName" HeaderText="Part Type">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MinStockLevel" SortExpression="MinStockLevel" HeaderText="Min Stock Level">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Rate" SortExpression="Rate" HeaderText="Rate">
                                                                <HeaderStyle HorizontalAlign="Right" ForeColor="black"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UnitName" SortExpression="UnitName" HeaderText="Unit">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--<asp:BoundField DataField="PartSerialisedStatus" SortExpression="PartSerialisedStatus"
                                                                HeaderText="Serialized Status">
                                                                <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Serialized Status" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" CssClass="clsLabelAuto" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "SerialisedStatus") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                            <%--   <asp:ButtonField HeaderText="Edit/View" Text="Edit/View" CommandName="EditView">
                                                             <HeaderStyle Wrap="False"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="Del">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:ButtonField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("AttachmentCount") > 0 %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <%-- <span id="button">Login</span>--%>
                                                                    <div class="dropdown">
                                                                        <div class="dropdownbtn-content">
                                                                            <table id="T1" class="clsGridNew_Ajax">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditView" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Del" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ViewRec" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("AttachmentCount") > 0 %>' />
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
                                                            <asp:BoundField DataField="AttachmentCount" HeaderText="AttachmentCount" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <%--  <asp:Panel ID="PnlPaging" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr>
                                            <td>
                                                <div style="width: 100%;">
                                                    <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                    class="letterbox" />
                                                            </td>
                                                            <td>
                                                                <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First"></span>
                                                            </td>
                                                            <td>
                                                                <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous"></span>
                                                            </td>
                                                            <td align="center">
                                                                <div align="center">
                                                                    <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                    </asp:TextBox>
                                                                    <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                        Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                        Length="300" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                            </td>
                                                            <td>
                                                                <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                            </td>
                                                            <td>
                                                                <span>of </span>
                                                            </td>
                                                            <td>
                                                                <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <!--End-->
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" align="right">
                        <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td><%--Ajay 27-Dec-2022--%>
                                            <asp:Button ID="hdnBtnMarkFav" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                            <asp:Button ID="hdnBtnRemoveFav" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
        </div>
        <!--Attachment Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAttachStateComplete() {
                $("#btnDummyAttach").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }
            function OpenAttachWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAttach").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForAttach() {
                var Attachwindow = $find("<%=mdlAttach.ClientID %>");
                //close popup window
                Attachwindow.hide();
                //release resources
                $("#IframeAttach").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnAttach").click();
            }
        </script>
        <!-- End-->
          <script type="text/javascript">
              function FunctionFav(x) {
                  if (x.classList.contains("fa-star")) {
                      x.classList.remove("fa-star");
                      x.classList.add("fa-star-o");
                      x.style.color = 'black';
                      x.style.border = 'black';
                      $("#hdnBtnRemoveFav").click();
                  }
                  else {
                      x.classList.remove("fa-star-o");
                      x.classList.add("fa-star");
                      x.style.color = '#fff';
                      x.style.border = 'black';
                      $("#hdnBtnMarkFav").click();
                  }
              }
              function MarkFav() {
                  var redstar = document.getElementById("<%=FavIClk.ClientID%>");
          redstar.classList.add("fa-star");
          redstar.classList.remove("fa-star-o");
          redstar.style.color = '#fff';
          redstar.style.border = 'black';

      }
      function RemoveFav() {
          var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                  redstar.classList.add("fa-star-o");
                  redstar.classList.remove("fa-star");
                  redstar.style.border = 'black';
              }
          </script>
    </form>
    <!-- Slider control events  -->
    <%--  <script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }


    </script>--%>
    <%-- <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }


    </script>--%>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <%--<script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>--%>
    <!-- End  -->
</body>
</html>
