<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOTool_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOTool_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tools Detail</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblmain" class="clstablelistout" border="0" cellspacing="1" cellpadding="1">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                                    <tr>

                                        <td colspan="4" class="clsFormHeader1">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> W.O. Tool Detail</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlAddButtons" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table3" border="0" cellspacing="1" cellpadding="1">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add Tool"
                                                                                ValidationGroup="b" Text="OK" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCancel" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous screen"
                                                                                Text="Back"></asp:Button>
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
                                        <td colspan="4">
                                            <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                        HeaderText="Fill Up The Following Fields" ValidationGroup="b"></asp:ValidationSummary>
                                                    <asp:CustomValidator ID="cvPart" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbItemList"
                                                        Display="None" ClientValidationFunction="validateItem" ErrorMessage="Select the Part name from the list"
                                                        ValidationGroup="b"></asp:CustomValidator><asp:CustomValidator ID="cvDescription"
                                                            runat="server" ErrorMessage="Description must not be greater than 150 characters."
                                                            ControlToValidate="txtDesc" Display="None" OnServerValidate="customvalidate"
                                                            ValidationGroup="b"></asp:CustomValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <script type="text/javascript">
                                                function validateItem(source, args) {
                                                    args.IsValid = false;

                                                    var dd = $get("cmbItemList");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }

                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                       <td colspan="4">
                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
                                                <legend><b>Work Order Details</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto">W. O. # </asp:Label> &nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelAuto"></asp:Label></td>
                                                    </tr>
                                                </table>

                                            </fieldset>
                                        </td>                                    
                                    </tr>
                                    <%--  <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblToolDetails" runat="server" CssClass="clsLabelHeader">Tool Details</asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="4">
                                            <asp:UpdatePanel ID="upnlPart" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label8" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPlaceName" runat="server" CssClass="clsLabel">Part No.</asp:Label>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbItemList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
                                                                                DataValueField="ID" AutoPostBack="True" Enabled="<%# mnWO.WOStatusID <> 3 %>"
                                                                                ValidationGroup="b">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Panel ID="pnl3" runat="server">
                                                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/expand_blue.jpg"
                                                                                    CausesValidation="False" ToolTip="Click to search Part" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:ImageButton>
                                                                                <%-- <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="False" ToolTip="Click to search Part"
                                                    Enabled="<%# mnWO.WOStatusID <> 3 %>">Search</asp:LinkButton>--%>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkGroundEquipment" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                                                Checked="true" ToolTip="Check to select Only Ground Equipment" Enabled="<%# mnWO.WOTools.CurrentItem.IsNew And mnWO.WOStatusID <> 3 %>"
                                                                                Text="Only Ground Equipment" Visible='<%# iif(AppSettings("ClientCode") = "IND", False, True) %>'></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" valign="top">
                                                                <asp:Panel ID="pnlInner" runat="server">
                                                                    <fieldset class="clsFieldSetNewStyle">
                                                                        <legend><b>Tool Search Engine </b></legend>
                                                                        <table id="Table7" class="clstablelistin">
                                                                            <tr>
                                                                                <td>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelMedium">Search</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" AutoPostBack="True">
                                                                                        <asp:ListItem Value="1" Selected="True">Part No.</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Description</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="true">For</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part No. to search"
                                                                                        Visible="true" MaxLength="100"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgBtnFind" runat="server" ToolTip="Click to search Part as per criteria" CssClass="clsSearch2btn1"
                                                                                        CausesValidation="False" ImageUrl="~/images/Search2.png"></asp:ImageButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="6">
                                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="6">
                                                                                    <%-- <asp:GridView ID="dgPartSearch" runat="server" ToolTip="List of Parts as per criteria"
                                                                        AllowSorting="True" AutoGenerateColumns="False" PageSize="5" AllowPaging="True"
                                                                        CssClass="clsGrid">--%>
                                                                                    <asp:GridView ID="dgPartSearch" runat="server" AllowPaging="True" AllowSorting="True"
                                                                                        AutoGenerateColumns="False" CssClass="clsGridNewStyleFixedWidth" GridLines="Horizontal" PageSize="10" ShowHeaderWhenEmpty="True" CellPadding="5">
                                                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                                        <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                                                                        <Columns>
                                                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part Number">
                                                                                                <HeaderStyle></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                <HeaderStyle></HeaderStyle>
                                                                                            </asp:BoundField>
                                                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
                                                                                        </Columns>
                                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </asp:Panel>
                                                                <cc2:CollapsiblePanelExtender ID="cpeSearch" runat="Server" TargetControlID="pnlInner"
                                                                    SuppressPostBack="true" CollapsedSize="0" Collapsed="true" ExpandControlID="pnl3"
                                                                    CollapseControlID="pnl3" AutoCollapse="False" AutoExpand="False" ExpandedImage="images/expand_blue.jpg"
                                                                    CollapsedImage="images/collapse_blue.jpg" ExpandDirection="Vertical" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:UpdatePanel ID="upnlDesc" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table10" cellspacing="0">
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblDesc" runat="server" CssClass="clsLabel">Description</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDesc" TabIndex="30" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2"
                                                                    ToolTip="Description" Text="<%# mnWO.WOTools.CurrentItem.Description %>" MaxLength="200"
                                                                    BackColor="#E0E0E0" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblReqQty" runat="server" CssClass="clsLabel">Required Qty.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReqQty" TabIndex="30" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
                                                                    ToolTip="Enter Required Quantity" Text="<%# mnWO.WOTools.CurrentItem.RequiredQty %>"
                                                                    MaxLength="5" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" ToolTip="Enter Remark"
                                                                    Text="<%# mnWO.WOTools.CurrentItem.WOToolRemark %>" MaxLength="500" TextMode="MultiLine"
                                                                    Enabled="<%# mnWO.WOStatusID <> 3 %>">
                                                                </asp:TextBox>
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
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForWOTool();
                return false;
            }

        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameWOToolStateComplete();
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" DynamicLayout="false" runat="server">
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
</body>
</html>
