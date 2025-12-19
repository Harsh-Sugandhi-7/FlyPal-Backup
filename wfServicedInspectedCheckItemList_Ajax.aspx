<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfServicedInspectedCheckItemList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfServicedInspectedCheckItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Equipment Maintenance Item List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblConditionCheckItemList" runat="server" CssClass="clsFormHeader">Equipment Maintenance Item List</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Record"
                                                                        Text="Add New" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print"
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblSearch" class="clsLabel">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtItemName" runat="server" CssClass="clsTextBoxTagSearchDate"  MaxLength="100"
                                                            AutoPostBack="true" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblDescription" class="clsLabel">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearchDate"  MaxLength="100"
                                                            AutoPostBack="true" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabel">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxTagSearchDate"  MaxLength="100"
                                                            AutoPostBack="true" Width="200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto" id="lblListOfItemServiceInspections">Service Inspections</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbListOfItemServiceInspections" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataValueField="ID" DataTextField="ServiceInspectionName" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnTopButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Record"
                                                            Text="Add New" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print"
                                                            Text="Print" CausesValidation="False"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">Serviced
                                                            Inspected Item List as per criteria : Record(s) found</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:GridView ID="dgGridView" runat="server" AllowPaging="True" AllowSorting="True"
                                                            DataKeyNames="ID" AutoGenerateColumns="False" PageSize="25"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            ShowHeaderWhenEmpty="True">
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                    ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ConditionCheckItemID" HeaderStyle-CssClass="hideGridColumn"
                                                                    HeaderText="ConditionCheckItemID" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemName" HeaderText="Part No." SortExpression="ItemName">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." SortExpression="SerialNo">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ItemServiceInspectionsDescription" HeaderText="Service Inspection"
                                                                    SortExpression="ItemServiceInspectionsDescription">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ReceiptItemLocation" HeaderText="Location" SortExpression="ReceiptItemLocation">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Frequency" HeaderText="Interval" SortExpression="Frequency">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ConditionCheckInterval" HeaderText="Interval In" SortExpression="ConditionCheckInterval">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ConditionCheckNo" HeaderText="No." SortExpression="ConditionCheckNo">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneOnDate" HeaderText="Done On Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="NextDueDate" HeaderText="Next Due Date">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IsApplicableTag" HeaderText="Applicable" SortExpression="IsApplicableTag">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DonebyAgency" HeaderText="Done by Agency" SortExpression="DonebyAgency">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateReference" HeaderText="Certificate Reference"
                                                                    SortExpression="CertificateReference">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                                                    <HeaderStyle  HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--    <asp:BoundField DataField="ConditionCheckServicedInspected" HeaderText="Condition Check/Serviced Inspected"
                                                                    SortExpression="ConditionCheckServicedInspected">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <%--  <asp:ButtonField CommandName="EditView" HeaderText="Comply" Text="Comply">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>--%>



                                                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Comply" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        
                                                                        
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        
                                                                        
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                       
                                                                       
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditViewRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="EditView" Style="height: 20px; width: 20px" ImageUrl="~/images/Comply.jpg"
                                                                                                Enabled='<%#  Eval("IsApplicable")%>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </td>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                    CommandName="History" ImageUrl="~/images/History.png" Enabled='<%#  Eval("HistoryStatus")%>' />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                                                    Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                            </td>
                                                                                        </tr>
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
                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded">
                                                                    <HeaderStyle CssClass="hideGridColumn" />
                                                                    <ItemStyle CssClass="hideGridColumn" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IsApplicable" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsApplicable"
                                                                    ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                            </Columns>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
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
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnBottomButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add New Record"
                                                            Text="Add New" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBottomPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print"
                                                            Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnBottomClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close"
                                                            Text="Close" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnConditionCheckItem" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnConditionCheckItemComply" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--End -->
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
    <!-- Condition Check Item Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyConditionCheckItem" Text="Dummy ConditionCheckItem"
            ClientIDMode="Static" CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlConditionCheckItem" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeConditionCheckItem" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupConditionCheckItem" runat="server" TargetControlID="btnDummyConditionCheckItem"
        PopupControlID="pnlConditionCheckItem" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameConditionCheckItemStateComplete() {
            $("#btnDummyConditionCheckItem").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenConditionCheckItemWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeConditionCheckItem").attr("src", "wfConditionCheckItem_Ajax.aspx?Type=pup&OpenFrom=ServiceInspectionList");
                $('#IframeConditionCheckItem').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyConditionCheckItem").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunction() {
            varConditionCheckItemwindow = $find("<%=mdlPopupConditionCheckItem.ClientID %>");
            //close ConditionCheckItem popup window
            varConditionCheckItemwindow.hide();
            //           release resources
            $("#IframeConditionCheckItem").attr("src", "JavaScript:''");
            //call ConditionCheckItem image button
            $("#hdnBtnConditionCheckItem").click();
        }
    </script>
    <!-- End-->
    <!-- Condition Check Comply Item Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyConditionCheckItemComply" Text="Dummy ConditionCheckItemComply"
            ClientIDMode="Static" CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlConditionCheckItemComply" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframeConditionCheckItemComply" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupConditionCheckItemComply" runat="server" TargetControlID="btnDummyConditionCheckItemComply"
        PopupControlID="pnlConditionCheckItemComply" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameConditionCheckItemComplyStateComplete() {
            $("#btnDummyConditionCheckItemComply").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenConditionCheckItemComplyWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeConditionCheckItemComply").attr("src", "wfConditionCheckItemComply_Ajax.aspx?Type=pup&OpenFrom=ServiceInspectionList");
                $('#IframeConditionCheckItemComply').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyConditionCheckItemComply").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForConditionCheckItemComply() {
            varConditionCheckItemComplywindow = $find("<%=mdlPopupConditionCheckItemComply.ClientID %>");
            //close ConditionCheckItemComply popup window
            varConditionCheckItemComplywindow.hide();
            //           release resources
            $("#IframeConditionCheckItemComply").attr("src", "JavaScript:''");
            //call ConditionCheckItemComply image button
            $("#hdnBtnConditionCheckItemComply").click();
        }
    </script>
    <!-- Condition Check Comply Item Popup Window End-->
    <!-- Condition Check History Item Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyConditionCheckItemHistory" Text="Dummy ConditionCheckItemHistory"
            ClientIDMode="Static" CausesValidation="false"></asp:Button>
    </div>
    <asp:Panel runat="server" ID="pnlConditionCheckItemHistory" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframeConditionCheckItemHistory" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupConditionCheckItemHistory" runat="server" TargetControlID="btnDummyConditionCheckItemHistory"
        PopupControlID="pnlConditionCheckItemHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameConditionCheckItemHistoryStateComplete() {
            $("#btnDummyConditionCheckItemHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenConditionCheckItemHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeConditionCheckItemHistory").attr("src", "wfConditionCheckItemHistoryList_Ajax.aspx?Type=pup");
                $('#IframeConditionCheckItemHistory').animate({ top: '50px' }, 'slow');
                if (!$.browser.msie) {
                    $("#btnDummyConditionCheckItemHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForConditionCheckItemHistory() {
            varConditionCheckItemHistorywindow = $find("<%=mdlPopupConditionCheckItemHistory.ClientID %>");
            //close ConditionCheckItemHistory popup window
            varConditionCheckItemHistorywindow.hide();
            //           release resources
            $("#IframeConditionCheckItemHistory").attr("src", "JavaScript:''");
            //call ConditionCheckItemHistory image button
            //            $("#hdnBtnConditionCheckItemHistory").click();
        }
    </script>
    <!-- Condition Check History Item Popup Window End-->
    </form>
</body>
</html>
