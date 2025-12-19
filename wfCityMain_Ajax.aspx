<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCityMain_Ajax.aspx.vb"
    Inherits="Flypal.wfCityMain_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>City</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <!-- #include file= "LocalFunctionAjax.htm" -->
    <style type="text/css">
        .hideGridColumn {
            display: none;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="form1" runat="server">
        <script src="js/query-1.7.1.js" type="text/javascript"></script>
        <script type="text/javascript" language="javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                var gridHeader = $('#<%=dgCityList.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
                $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
                $('#<%=dgCityList.ClientID%> tr th').each(function (i) {
                    // Here Set Width of each th from gridview to new table(clone table) th 
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                });
                $("#GHead").append(gridHeader);
                $('#GHead').css('position', 'absolute');
            });
        </script>
        <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
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
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%" cellpadding="0">
                                                    <tr>
                                                        <td class="clsFormHeader1Newstyle">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">City [New]</asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the new City"
                                                                                                Text="New" CausesValidation="False"></asp:Button>
                                                                                        </td>
                                                                                        <td align="right" colspan="2">
    <asp:Button ID="btnSave" ValidationGroup="a" runat="server" CssClass="clsbtnH clsinfoH"
        ToolTip="Click to save the City Information" Text="Save"></asp:Button>
</td>
                                                                                        <td align="right">
                                                                                            <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close City screen"
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
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                                ValidationGroup="a"></asp:ValidationSummary>
                                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required"
                                                                Display="None" ControlToValidate="txtName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvGMT" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select GMT from the list."
                                                                Display="None" ControlToValidate="cmbGMT" ClientValidationFunction="validateGMT"
                                                                ValidationGroup="a"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel runat="server" ID="upnlCityDetails" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -5px">
                                                                <table width="100%" cellpadding="0">
                                                                    <tr>
                                                                        <td>
                                                                            <span id="spName1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="spName" class="clsLabelAuto">Name</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter City Name"
                                                                                Text="<%# mCityMain.Name %>" MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="spGMT1" class="clsLabelStar">*</span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="spGMT" class="clsLabelAuto">GMT</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbGMT" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboSmall">
                                                                                <asp:ListItem Value="(SELECT)" Selected="True">(SELECT)</asp:ListItem>
                                                                                <asp:ListItem Value="-00:15">-00:15</asp:ListItem>
                                                                                <asp:ListItem Value="-00:30">-00:30</asp:ListItem>
                                                                                <asp:ListItem Value="-00:45">-00:45</asp:ListItem>
                                                                                <asp:ListItem Value="-01:00">-01:00</asp:ListItem>
                                                                                <asp:ListItem Value="-01:15">-01:15</asp:ListItem>
                                                                                <asp:ListItem Value="-01:30">-01:30</asp:ListItem>
                                                                                <asp:ListItem Value="-01:45">-01:45</asp:ListItem>
                                                                                <asp:ListItem Value="-02:00">-02:00</asp:ListItem>
                                                                                <asp:ListItem Value="-02:15">-02:15</asp:ListItem>
                                                                                <asp:ListItem Value="-02:30">-02:30</asp:ListItem>
                                                                                <asp:ListItem Value="-02:45">-02:45</asp:ListItem>
                                                                                <asp:ListItem Value="-03:00">-03:00</asp:ListItem>
                                                                                <asp:ListItem Value="-03:15">-03:15</asp:ListItem>
                                                                                <asp:ListItem Value="-03:30">-03:30</asp:ListItem>
                                                                                <asp:ListItem Value="-03:45">-03:45</asp:ListItem>
                                                                                <asp:ListItem Value="-04:00">-04:00</asp:ListItem>
                                                                                <asp:ListItem Value="-04:15">-04:15</asp:ListItem>
                                                                                <asp:ListItem Value="-04:30">-04:30</asp:ListItem>
                                                                                <asp:ListItem Value="-04:45">-04:45</asp:ListItem>
                                                                                <asp:ListItem Value="-05:00">-05:00</asp:ListItem>
                                                                                <asp:ListItem Value="-05:15">-05:15</asp:ListItem>
                                                                                <asp:ListItem Value="-05:30">-05:30</asp:ListItem>
                                                                                <asp:ListItem Value="-05:45">-05:45</asp:ListItem>
                                                                                <asp:ListItem Value="-06:00">-06:00</asp:ListItem>
                                                                                <asp:ListItem Value="-06:15">-06:15</asp:ListItem>
                                                                                <asp:ListItem Value="-06:30">-06:30</asp:ListItem>
                                                                                <asp:ListItem Value="-06:45">-06:45</asp:ListItem>
                                                                                <asp:ListItem Value="-07:00">-07:00</asp:ListItem>
                                                                                <asp:ListItem Value="-07:15">-07:15</asp:ListItem>
                                                                                <asp:ListItem Value="-07:30">-07:30</asp:ListItem>
                                                                                <asp:ListItem Value="-07:45">-07:45</asp:ListItem>
                                                                                <asp:ListItem Value="-08:00">-08:00</asp:ListItem>
                                                                                <asp:ListItem Value="-08:15">-08:15</asp:ListItem>
                                                                                <asp:ListItem Value="-08:30">-08:30</asp:ListItem>
                                                                                <asp:ListItem Value="-08:45">-08:45</asp:ListItem>
                                                                                <asp:ListItem Value="-09:00">-09:00</asp:ListItem>
                                                                                <asp:ListItem Value="-09:15">-09:15</asp:ListItem>
                                                                                <asp:ListItem Value="-09:30">-09:30</asp:ListItem>
                                                                                <asp:ListItem Value="-09:45">-09:45</asp:ListItem>
                                                                                <asp:ListItem Value="-10:00">-10:00</asp:ListItem>
                                                                                <asp:ListItem Value="-10:15">-10:15</asp:ListItem>
                                                                                <asp:ListItem Value="-10:30">-10:30</asp:ListItem>
                                                                                <asp:ListItem Value="-10:45">-10:45</asp:ListItem>
                                                                                <asp:ListItem Value="-11:00">-11:00</asp:ListItem>
                                                                                <asp:ListItem Value="-11:15">-11:15</asp:ListItem>
                                                                                <asp:ListItem Value="-11:30">-11:30</asp:ListItem>
                                                                                <asp:ListItem Value="-11:45">-11:45</asp:ListItem>
                                                                                <asp:ListItem Value="-12:00">-12:00</asp:ListItem>
                                                                                <asp:ListItem Value="+00:00">+00:00</asp:ListItem>
                                                                                <asp:ListItem Value="+00:15">+00:15</asp:ListItem>
                                                                                <asp:ListItem Value="+00:30">+00:30</asp:ListItem>
                                                                                <asp:ListItem Value="+00:45">+00:45</asp:ListItem>
                                                                                <asp:ListItem Value="+01:00">+01:00</asp:ListItem>
                                                                                <asp:ListItem Value="+01:15">+01:15</asp:ListItem>
                                                                                <asp:ListItem Value="+01:30">+01:30</asp:ListItem>
                                                                                <asp:ListItem Value="+01:45">+01:45</asp:ListItem>
                                                                                <asp:ListItem Value="+02:00">+02:00</asp:ListItem>
                                                                                <asp:ListItem Value="+02:15">+02:15</asp:ListItem>
                                                                                <asp:ListItem Value="+02:30">+02:30</asp:ListItem>
                                                                                <asp:ListItem Value="+02:45">+02:45</asp:ListItem>
                                                                                <asp:ListItem Value="+03:00">+03:00</asp:ListItem>
                                                                                <asp:ListItem Value="+03:15">+03:15</asp:ListItem>
                                                                                <asp:ListItem Value="+03:30">+03:30</asp:ListItem>
                                                                                <asp:ListItem Value="+03:45">+03:45</asp:ListItem>
                                                                                <asp:ListItem Value="+04:00">+04:00</asp:ListItem>
                                                                                <asp:ListItem Value="+04:15">+04:15</asp:ListItem>
                                                                                <asp:ListItem Value="+04:30">+04:30</asp:ListItem>
                                                                                <asp:ListItem Value="+04:45">+04:45</asp:ListItem>
                                                                                <asp:ListItem Value="+05:00">+05:00</asp:ListItem>
                                                                                <asp:ListItem Value="+05:15">+05:15</asp:ListItem>
                                                                                <asp:ListItem Value="+05:30">+05:30</asp:ListItem>
                                                                                <asp:ListItem Value="+05:45">+05:45</asp:ListItem>
                                                                                <asp:ListItem Value="+06:00">+06:00</asp:ListItem>
                                                                                <asp:ListItem Value="+06:15">+06:15</asp:ListItem>
                                                                                <asp:ListItem Value="+06:30">+06:30</asp:ListItem>
                                                                                <asp:ListItem Value="+06:45">+06:45</asp:ListItem>
                                                                                <asp:ListItem Value="+07:00">+07:00</asp:ListItem>
                                                                                <asp:ListItem Value="+07:15">+07:15</asp:ListItem>
                                                                                <asp:ListItem Value="+07:30">+07:30</asp:ListItem>
                                                                                <asp:ListItem Value="+07:45">+07:45</asp:ListItem>
                                                                                <asp:ListItem Value="+08:00">+08:00</asp:ListItem>
                                                                                <asp:ListItem Value="+08:15">+08:15</asp:ListItem>
                                                                                <asp:ListItem Value="+08:30">+08:30</asp:ListItem>
                                                                                <asp:ListItem Value="+08:45">+08:45</asp:ListItem>
                                                                                <asp:ListItem Value="+09:00">+09:00</asp:ListItem>
                                                                                <asp:ListItem Value="+09:15">+09:15</asp:ListItem>
                                                                                <asp:ListItem Value="+09:30">+09:30</asp:ListItem>
                                                                                <asp:ListItem Value="+09:45">+09:45</asp:ListItem>
                                                                                <asp:ListItem Value="+10:00">+10:00</asp:ListItem>
                                                                                <asp:ListItem Value="+10:15">+10:15</asp:ListItem>
                                                                                <asp:ListItem Value="+10:30">+10:30</asp:ListItem>
                                                                                <asp:ListItem Value="+10:45">+10:45</asp:ListItem>
                                                                                <asp:ListItem Value="+11:00">+11:00</asp:ListItem>
                                                                                <asp:ListItem Value="+11:15">+11:15</asp:ListItem>
                                                                                <asp:ListItem Value="+11:30">+11:30</asp:ListItem>
                                                                                <asp:ListItem Value="+11:45">+11:45</asp:ListItem>
                                                                                <asp:ListItem Value="+12:00">+12:00</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: 5px">
                                            <div style="width: 100%">
                                                <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>
                                                                    <span id="spPlace" class="clsLabelAuto">Name</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter City Name"
                                                                        MaxLength="50"></asp:TextBox>
                                                                </td>
                                                                <td align="right">
                                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                                ToolTip="Click to find the list of records as per searching criteria" Text="Find Now"></asp:Button>--%>
                                                                    <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find the list of records as per searching criteria" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div>
                                                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="dgCityList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                                                        CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle" PageSize="25" ShowHeaderWhenEmpty="True">
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle HorizontalAlign="Right" />
                                                                        <RowStyle CssClass="clsdgItem" />
                                                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                        <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="40" />
                                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
                                                                                ItemStyle-CssClass="hideGridColumn">
                                                                                <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="GMT" HeaderText="GMT">
                                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>

                                                                            <%--<asp:ButtonField CommandName="EditView" HeaderText="Edit/View" Text="Edit/View">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>
                                                                            <asp:ButtonField CommandName="Remove" HeaderText="Delete" Text="Delete">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:ButtonField>--%>
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <div class="dropdown">
                                                                                        <div class="dropdownbtn-content">
                                                                                            <table id="T1" class="clsGridNew_Ajax" style="z-index: 100; width: 65px; position: relative;">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
                                                                                                            CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                                            CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
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
                                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                        </Columns>
                                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                    </asp:GridView>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>

                        </asp:Panel>
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
        <script type="text/javascript">
            function validateGMT(source, args) {
                args.IsValid = false;
                var dd = $get("cmbGMT");
                if (dd.selectedIndex != 0) {
                    args.IsValid = true;
                    return;
                }
            }
        </script>
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunction();
                return false;
            }
        </script>
        <%--End--%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Typepup") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameCityMainComplete();
                }
            });

    <% End if %>
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
            function endRequestHandler() {
                SetPageLayout();
            }

            function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Typepup") %>
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
</body>
</html>
