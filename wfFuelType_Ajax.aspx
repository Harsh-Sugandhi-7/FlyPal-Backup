<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFuelType_Ajax.aspx.vb" Inherits="Flypal.wfFuelType_Ajax" %>

<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Fuel Type Master</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Fuel Type [New]</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="New" ToolTip="Click To Add The New Fuel Type "
                                                            CausesValidation="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click To Save The Fuel Type Information"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlCloseBottom" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            TabIndex="0" Text="Close" ToolTip="Click To Close Fuel Type Master screen" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtFuelType"
                                                Display="None" ErrorMessage="Fuel Type Name Required"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <%--<asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnAdd" runat="server" CssClass="clsButton" Text="New" ToolTip="Click To Add The New Fuel Type "
                                                CausesValidation="False"></asp:Button>
                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDet" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <%--<tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblCityDetails" runat="server" CssClass="clsLabelHeader">Fuel Type Details</asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td style="width: 13px">
                                                        <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFuelType" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                                        <%--  </td>
                                                    <td>
                                                        <table id="Table2">
                                                            <tr>
                                                                <td>--%>
                                                        <asp:TextBox ID="txtFuelType" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFuelType.Name %>"
                                                            ToolTip="Enter Fuel Type" MaxLength="15"></asp:TextBox>
                                                        <%--   </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <%-- <asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSave" CssClass="clsButton" runat="server" Text="Save" ToolTip="Click To Save The Fuel Type Information"></asp:Button>                                  
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlsearchfueltypeheading" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblsearchfueltypeheading" runat="server" CssClass="clsLabelauto" Font-Bold="True">Search By Fuel Type</asp:Label>
                                                    </td>
                                                    <td align="right"></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlsearchfueltype" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td style="width: 13px"></td>
                                                    <td>
                                                        <asp:Label ID="lblsearchfueltype" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Tablefueltype">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtsearchfueltype" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mFuelType.Name %>"
                                                                        ToolTip="Enter Fuel Type To Search" MaxLength="15"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <%--  <asp:Button ID="btnFindNow" CssClass="clsButton" runat="server" Text="Find Now" ToolTip="Click To Find The List Of Fuel types As Per Searching Criteria"
                                                            CausesValidation="False"></asp:Button>--%>
                                                        <asp:ImageButton ID="btnFindNow1" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" CausesValidation="false"  ToolTip="Click To Find The List Of Fuel types As Per Searching Criteria" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%-- <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsButton"
                                                Text="Close" ToolTip="Click To Close  Fuel Type Screen" Visible="<%# mFuelTypeList.Count>25 %>" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px;">
                                                <legend id="Legend4"><b>
                                                    <asp:Label ID="lblListInfo" runat="server" CssClass="clsLabelHeader">

                                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:Label></b></legend>
                                                <asp:GridView ID="dgFuelTypeList1" runat="server" AllowPaging="True" AllowSorting="False"
                                                    AutoGenerateColumns="False" CssClass="clsGridNewStyle" PageSize="1000" ToolTip="Fuel Type List" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                                            <HeaderStyle />
                                                        </asp:BoundField>
                                                        <%--<asp:BoundField CommandName="View" HeaderText="Edit/View" Text="Edit/View"></asp:BoundField>
                                                    <asp:BoundField CommandName="Delete" HeaderText="Delete" Text="Delete"></asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <%-- <span id="button">Login</span>--%>
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgEditView" runat="server" CommandName="ViewRec" Style="height: 15px; width: 15px"
                                                                                        ImageUrl="~/images/edit.png" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                        ImageUrl="~/images/delete.png" CausesValidation="false" CommandArgument='<%# Eval("ID") %>' />
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <%--  <PagerStyle HorizontalAlign="Right" NextPageText="Next" PrevPageText="Prev" />--%>
                                                </asp:GridView>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right" valign="bottom">
                                    <asp:UpdatePanel ID="upnlCloseBottom" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton"
                                                TabIndex="0" Text="Close" ToolTip="Click To Close Fuel Type Master screen" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
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
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForFuelOil();
                return false;
            }
        </script>
        <%--<asp:Button ID="btnValue" runat="server" CssClass="clsButtonGrid" Text="..." ToolTip="Click to Refresh the the Values in the Grid and to check the Validations."
                                                CommandName="Value"></asp:Button>--%>

        <%--UPDATEPANEL --%>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameFuelOilStateComplete();
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
</body>
</html>
