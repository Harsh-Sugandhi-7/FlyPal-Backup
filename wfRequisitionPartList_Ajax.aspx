<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionPartList_Ajax.aspx.vb"
    Inherits="Flypal.wfRequisitionPartList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Requisition Part List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Requisition Part List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOk" runat="server" ToolTip="Click to add the selected Requisition Part"
                                                                    Text="Ok" CssClass="clsbtnH clsinfoH"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Requisition Part List screen"
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
                                <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="lblPartNumber" class="clsLabel">Part Number</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPartNumber" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="50"
                                                        TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="right">
                                                    <table id="Table2">
                                                        <tr>
                                                            <td>
                                                                <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" TabIndex="6"
                                                                    ToolTip="Click to find the list of records as per searching criteria" Text="Find Now"
                                                                    ></asp:Button>--%>


                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn"
                                                                    ToolTip="Click to find list of records as per searching criteria" CausesValidation="False"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="false">From Date</asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate"
                                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');" Visible="false"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="false">To</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate"
                                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" Visible="false"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                    WatermarkCssClass="clsDateTextBox">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTransactionDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                        TabIndex="2" AutoPostBack="true" OnTextChanged="txtTransactionDate_TextChanged"
                                                        onchange="ValidateDateText(this,'TransactionDate_watermarkextender','true');"
                                                        Text="" ></asp:TextBox>
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
                                                    <span id="Span1" class="clsLabel">Requisition Type</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="cmbRequisition" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                        TabIndex="3">
                                                        <asp:ListItem Selected="True" Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="65">Engineering</asp:ListItem>
                                                        <asp:ListItem Value="71">Stores</asp:ListItem>
                                                        <asp:ListItem Value="72">WorkShop</asp:ListItem>
                                                        <asp:ListItem Value="77">Planning</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblSupplier" class="clsLabel">Requisition No.</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtRequisitionText" runat="server" CssClass="clsTextBoxTagSearch"
                                                        TabIndex="4" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                                                    <asp:TextBox ID="txtRequisitionNo" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                        ClientIDMode="Static" TabIndex="5" MaxLength="5"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span id="lblInfo" class="clsLabelAuto">Click on Check Box to Select Part Information
                                                        or click on Close button to Close the screen.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="dgRequisitionItemList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowSorting="True"
                                                        AllowPaging="True" PageSize="25" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                       DataKeyNames="PartNo,Description">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server" 
                                                                        onclick="CheckUncheck(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'
                                                                        Enabled='<%# Eval("IsItemIDEmpty")%>'></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="Part No.">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="ReqDateFormatted" HeaderText="Date">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="RequisitionNo" SortExpression="RequisitionNo" HeaderText="Number">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="EnquiryBalQty" SortExpression="EnquiryBalQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="QuotationBalQty" SortExpression="QuotationBalQty" HeaderText="Qty.">
                                                                <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:TemplateField HeaderText="Add Part">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAddPart" runat="server" Text="Add Part" CausesValidation="False"
                                                                       CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" Enabled='<%# Not Eval("IsItemIDEmpty")%>' Visible='<%# Not Eval("IsItemIDEmpty")%>'
                                                                        CommandName="AddPart">Add Part</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="ItemID" HeaderText="ItemID" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" />
                                                            <ItemStyle CssClass="hideGridColumn" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="IsItemIDEmpty" HeaderText="IsItemIDEmpty" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn">
                                                            <HeaderStyle CssClass="hideGridColumn" />
                                                            <ItemStyle CssClass="hideGridColumn" />
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
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOk" runat="server" ToolTip="Click to add the selected Requisition Part"
                                                        Text="Ok" CssClass="clsbtnH clsinfoH"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server"CssClass="clsbtnH clsinfoH" ToolTip="Click to close Requisition Part List screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForReqPartList();
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
                parent.IFrameReqPartListStateComplete();
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
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtRequisitionText.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=18', {
                width: 185,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });       
    </script>
    <script type="text/javascript">
        function CheckUncheck(chkBoxAll) {
            var str = chkBoxAll.id;
            var status = $("#" + str).attr("checked");
            $("#dgRequisitionItemList" + " tr:gt(0)").find(":checkbox[id*=" + str.substring(0, 'chkSelect') + "]").each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                }
                else {
                    $(this).removeAttr("checked");
                }
            });
        }
    </script>
</body>
</html>
