<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfManualSubscription_Ajax.aspx.vb"
    Inherits="Flypal.wfManualSubscription_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual Revision Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="0" leftmargin="2" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="Table1">
        <tr>
            <td>
                <table class="clstablelistin" id="Table2">
                    <tr>
                        <td class="clsFormHeader1Newstyle">

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblManual" runat="server" CssClass="clsFormHeader">Manual Report</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>

                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table id="Table3" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                                Text="Print" CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close screen."
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
                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 85px;">
                                                            <span id="lblManualName" class="clsLabelAuto">Manual Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtManualName" runat="server" CssClass="clsTextBoxTagSearch" Width="275px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 20px;"></td>
                                                            <td>
                                                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                    DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 85px;">
                                                            <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                ToolTip="Enter Limit" ClientIDMode="Static" MaxLength="4">30</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="right">
                                                <%--<asp:Button ID="btnSearch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find List of Manuals as per searching criteria"
                                                    Text="Find Now"></asp:Button>--%>

                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Manuals as per searching criteria" />
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
                                                <asp:Label ID="lblList" runat="server" CssClass="clsLabelHeader">List</asp:Label>
                                            </td>
                                            <%--<td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table id="Table3" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                                        Text="Print" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close screen."
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="dgManualRevList" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true"
                                                    PageSize="25" AutoGenerateColumns="False" AllowPaging="True"
                                                    GridLines="Horizontal" CellPadding="3">
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"/>
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ManualID" HeaderText="ManualID"></asp:BoundField>
                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No." ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Manual Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="ApplicableFor" HeaderText="Applicable For">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ShortDesc" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MCategoryName" HeaderText="Category">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="Note" HeaderText="Note">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="IsInUseTag" HeaderText="In Use">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="RevisionID" HeaderText="RevisionID">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="No" HeaderText="No.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="RevNo" HeaderText="Revision No.">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="RevDate" HeaderText="Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ValidityString" HeaderText="Subscription">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EffectiveDate" HeaderText="Due Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="75px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HardCopyString" HeaderText="Hard Copy">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="Remark" HeaderText="Remark">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField Visible="False" DataField="Note" HeaderText="Note">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SoftCopyString" HeaderText="Soft Copy">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Visible="False" Text="View " HeaderText="Attach" CommandName="Select Revision">
                                                        </asp:ButtonField>

                                                         <asp:BoundField DataField="DueStatus" HeaderText="DueStatus" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField>
                                                            <ItemStyle CssClass="clsColorLabel" Width="7px" Height="7px" />
                                                        </asp:BoundField>


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
                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table4" border="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Print the list of Manuals"
                                                    Text="Print" CausesValidation="False" Visible="false"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close screen."
                                                    Text="Close" CausesValidation="False" Visible="false"></asp:Button>
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
    </form>
</body>
</html>
