<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCommonPartList_Ajax.aspx.vb"
    Inherits="Flypal.wfCommonPartList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).closest('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    trParent.addClass('clslightColor')
                else
                    trParent.removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $(element).closest('tr').addClass('clslightColor');
                else
                    $(element).closest('tr').removeClass('clslightColor');
            });
        });
    </script>
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
                                            <span id="lblPartList" class="clsFormHeader">Part List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNewPart" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to add new part"
                                                                    Text="Add New Part"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the selected Item"
                                                                    Text="Ok"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous Page"
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
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--  <asp:UpdatePanel ID="upnlPartDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTransactionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                                    onchange="ValidateDateText(this,'TransactionDate_watermarkextender','true');"
                                                                    Text="" Width="100px"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtTransactionDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtTransactionDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="TransactionDate_watermarkextender" runat="server"
                                                                    TargetControlID="txtTransactionDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span id="lblSearch" class="clsLabelAuto">Search</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmblookin" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" onChange="ControlVisibility(this);">
                                                                    <asp:ListItem Value="0">All</asp:ListItem>
                                                                    <asp:ListItem Value="1">Part No.</asp:ListItem>
                                                                    <asp:ListItem Value="2">Description</asp:ListItem>
                                                                    <asp:ListItem Value="3">Nomenclature</asp:ListItem>
                                                                    <asp:ListItem Value="4">Category</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <asp:Label ID="lblFor" runat="server" CssClass="clsLabelMedium" Style="display: none">For</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                                    Style="display: none"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Part as per searching criteria"
                                                                    Text="Find Now"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                    ToolTip="Click to find list of Part as per searching criteria" />
                                                            </td>
                                                            <%--<td>
                                                                <asp:Button ID="btnAddNewPart" runat="server" CssClass="clsButton_Ajax" ToolTip=" Click to add new part"
                                                                    Text="Add New Part"></asp:Button>
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="upnlPartDetails" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Parts : 100 Record(s) found.</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="dgPartList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True"
                                                                    ShowHeaderWhenEmpty="true" DataKeyNames="ID" AutoGenerateColumns="False" PageSize="25"
                                                                    AllowSorting="True">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                    <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"></input>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
                                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="NomenclatureName" SortExpression="NomenclatureName" HeaderText="Nomenclature">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOk" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add the selected Item"
                                                        Text="Ok"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to the previous Page"
                                                        Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
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
    <script type="text/javascript">
        function ControlVisibility(elem) {
            var Index = $get("cmblookin").selectedIndex;
            var label = document.getElementById("<%= lblFor.ClientID %>");
            var txtSearch = document.getElementById("<%= txtSearch.ClientID %>");
            if (Index == 0) {
                label.style.display = 'none';
                txtSearch.style.display = 'none';
                $("#txtSearch").val('');

            }
            else {
                label.style.display = 'inline-block';
                txtSearch.style.display = 'inline-block';
                $("#txtSearch").val('');
            }
        }
    </script>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForCommonPartList();
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
                parent.IFrameCommonPartListStateComplete();
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
    </form>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, TobeReset) {

            var datevalue = $(elem).val();
            var resetTodaysDate = TobeReset;
            var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
</body>
</html>
