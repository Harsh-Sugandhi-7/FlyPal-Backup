<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDistributionCopy_Ajax.aspx.vb"
    Inherits="Flypal.wfDistributionCopy_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Distribution List Copy Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css"    />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
</head>
<body>
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlDistributionCopy" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="2" class="clsFormHeader1Newstyle">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitle" TabIndex="1" CssClass="clsFormHeader" runat="server">Copy Distribution List</asp:Label>
                                                    </td>
                                                    <td colspan="2" align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnCopy" runat="server" CssClass="clsbtnH clsinfoH" Enabled="<%# cmbFromModel.SelectedIndex > 0 %>"
                                                                        ToolTip="Click to Copy" Text="Copy"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvFromModel" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ErrorMessage="Select Source Model from the list."
                                                ClientValidationFunction="ValidateFromModel" ControlToValidate="cmbFromModel"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvToModel" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ErrorMessage="Select Destination Model from the list." 
                                                ClientValidationFunction="ValidateToModel" ControlToValidate="cmbToModel"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Different Destination Model from the list."
                                                ClientValidationFunction="ValidateCheckModel" Display="None"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader" DESIGNTIMEDRAGDROP="13">Copy Distribution List Details</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 3px">
                                            <table id="Table1" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFromModel" runat="server" CssClass="clsLabel">From Model</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbFromModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                            DataTextField="ModelName" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToModel" runat="server" CssClass="clsLabelAuto" Width="104px">To Model</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbToModel" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelName"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="height: 3px">
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td style="height: 3px">
                                        </td>
                                        <td style="height: 3px">
                                            <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>--%>


                                    <%--<td colspan="2" align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCopy" runat="server"CssClass="clsbtnH clsinfoH" Enabled="<%# cmbFromModel.SelectedIndex > 0 %>"
                                                            ToolTip="Click to Copy" Text="Copy"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server"CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>



                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblHeader" runat="server" CssClass="clsLabelHeader">List of Distribution(s)</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                             <asp:GridView ID="dgDistribution" runat="server" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" PageSize="25" 
                                                 CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                 ShowHeaderWhenEmpty="True">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                    NextPageText="" PreviousPageText="" />
                                                <PagerStyle HorizontalAlign="Right" />
                                                <RowStyle CssClass="clsdgItem" />
                                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"/>
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="ModelID" HeaderText="ModelID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No. " SortExpression="SrNo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                      <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                 </Columns>
                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td colspan="2" align="right">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server"CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
                                                            Text="Close" CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>--%>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <script type="text/javascript">


        function ValidateFromModel(source, args) {
            args.IsValid = false;
            var dd = document.getElementById("cmbFromModel");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
        function ValidateToModel(source, args) {
            args.IsValid = false;
            var dd = document.getElementById("cmbToModel");
            if (dd.selectedIndex != 0) {
                args.IsValid = true;
                return;
            }
        }
        function ValidateCheckModel(source, args) {
            args.IsValid = false;
            var cmbFrom = document.getElementById("cmbFromModel");
            var cmbTo = document.getElementById("cmbToModel");
            if (cmbFrom.selectedIndex != cmbTo.selectedIndex) {
                args.IsValid = true;
                return;
            }
        }
    </script>
    </form>
</body>
</html>
