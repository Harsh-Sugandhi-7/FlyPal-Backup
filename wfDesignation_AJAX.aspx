<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDesignation_AJAX.aspx.vb" EnableEventValidation="true"
    Inherits="Flypal.wfDesignation_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>Designation</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">



        <%-- 
           <script src="js/query-1.7.1.js" type="text/javascript"></script>
           <script type="text/javascript" language="javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                var gridHeader = $('#<%=dgDesignationList.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
                $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
                $('#<%=dgDesignationList.ClientID%> tr th').each(function (i) {
                    // Here Set Width of each th from gridview to new table(clone table) th 
                    $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                });
                $("#GHead").append(gridHeader);
                $('#GHead').css('position', 'absolute');
                $('#GHead').css('top', $('#<%=dgDesignationList.ClientID%>').offset().top);

            });
        </script>--%>
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout" style="width:750px">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table class="clstablelistin" id="tblInner">
                            <tr>
                                <td class="clsFormHeader1">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Designation [New]</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="New"
                                                            ToolTip="Click to add the new Designation" CausesValidation="true"></asp:Button>
                                                        <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" Text="Save" ToolTip="Click to save the Designation Information"
                                                            ValidationGroup="1"></asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close Designation screen" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" Width="440px" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Designation name required."
                                                Display="None" ControlToValidate="txtDesignation" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="Name should be less than or equal 50 Characters." Display="None"
                                                ControlToValidate="txtDesignation" ClientValidationFunction="validateName" ValidationGroup="1"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateName(source, args) {
                                                    var Value = $get(source.controltovalidate).value.length;
                                                    if (Value > 50) {
                                                        args.IsValid = false;
                                                        return
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDesignation" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Designation</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Designation"
                                                            Text="<%# mDesignation.Name %>" MaxLength="50">
                                                        </asp:TextBox>
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

                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="96%">
                                                <tr>
                                                    <td>
                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; width: 99%;">
                                                            <legend id="Legend1" runat="server" style="font-weight: bold">Designation List</legend>
                                                            <%--  <div id="GHead" style="overflow: auto; z-index: -1; position: relative; width : 99%;">
                                                </div>--%>
                                                            <div >
                                                                <asp:GridView ID="dgDesignationList" runat="server" CssClass="clsGridNewStyle" EnableViewState="true"
                                                                    DataKeyNames="ID" AutoGenerateColumns="False" AllowPaging="True" ShowHeaderWhenEmpty="true"
                                                                    GridLines="Horizontal" CellPadding="5" PageSize="10">
                                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                                    <Columns>
                                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="DesignationID"></asp:BoundField>
                                                                        <asp:BoundField DataField="Name" HeaderText="Name">
                                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                            <ItemStyle Wrap="False" />
                                                                        </asp:BoundField>
                                                                        <%--<asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:ButtonField>--%>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <%-- <span id="button">Login</span>--%> 
                                                                                <div class="dropdown">
                                                                                    <div class="dropdownbtn-content">
                                                                                        <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px; width: 15px"
                                                                                                        ImageUrl="~/images/edit.png" CausesValidation="false" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
                                                                                                        ImageUrl="~/images/delete.png" CausesValidation="false" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" />
                                                                                                </td>

                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                                                        Style="cursor: pointer; height: 20px; width: 20px" />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                            DataField="IsSyncFromCRS" HeaderText="IsSyncFromCRS"></asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </fieldset>

                                                    </td>

                                                </tr>

                                            </table>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div>
            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackFunctionForDesignation();
                }
            </script>
            <%--End--%>
            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameStateComplete();
                    }


                });
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();
                }

                function SetPageLayout() {
       <% Dim mopenas As String = Request.QueryString("Type") %>
          <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                    ReSetPageLayout();
                 //  onResize();//for Top bottom link
           <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight(true);
                    var windowheight = $(window).height();
                    if (tempMargtop >= windowheight) {
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                    }

                }
            </script>
            <%--End--%>
        </div>
    </form>
</body>
</html>
