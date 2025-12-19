<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlightLogClassification.aspx.vb" Inherits="Flypal.wfFlightLogClassification" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html>
<head runat="server">
    <title>Flight Log Classification</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <!-- #include file= "LocalFunctionAjax.htm" -->

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
             <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                        <asp:UpdatePanel runat="server" ID="upnlMain" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="tblInner" class="clstablelistin">
                                    <tr>
                                        <td colspan="4" class="clsFormHeader1">
                                            <table width="100%">

                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Flight Log Classification [New]</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False" ToolTip="Click to add the new Flight Log Classification."
                                                                        Text="New"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
                                                                        ToolTip="Click to close Flight Log Classification screen" Text="Close"></asp:Button>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" Width="376px"></asp:ValidationSummary>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblCityDetails" runat="server" CssClass="clsLabelHeader">Flight Log Classification Details</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label></td>
                                        <td style="width: 73px">
                                            <asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label></td>
                                        <td>
                                            <table id="Table2">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Flight Log Classification name" Text="<%# mFlightLogClassification.Name %>" MaxLength="50">
                                                        </asp:TextBox></td>
                                                    <td align="right">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to save Flight Log Classification"
                                                            Text="Save"></asp:Button></td>
                                                </tr>
                                            </table>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required"
                                                Display="None" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name too Long."
                                                Display="None" ControlToValidate="txtName" OnServerValidate="customvalidate"></asp:CustomValidator></td>
                                        <td align="right"></td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblSearchByCity" runat="server" CssClass="clsLabelHeader">Search by Flight Log Classification</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td style="width: 73px">
                                            <asp:Label ID="lbFlightLogClassificationName" runat="server" CssClass="clsLabelAuto">Name </asp:Label></td>
                                        <td>
                                            <table id="Table4">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Flight Log Classification name"
                                                            MaxLength="50"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <%-- <asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH1" CausesValidation="False" ToolTip="Click to find the list of records as per searching criteria."
                                        Text="Find Now"></asp:Button></td>--%>
                                            <asp:ImageButton ID="btnImgFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" CausesValidation="false"  ToolTip="Click to find the list of records as per searching criteria." />
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="gvFlightLogClassificationList" runat="server" CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle" ToolTip="Flight Log Classification List"
                                                AutoGenerateColumns="False">
                                                <AlternatingRowStyle CssClass="clsdgAltRow"></AlternatingRowStyle>
                                                <RowStyle CssClass="clsdgRow"></RowStyle>
                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="Name" HeaderText="Name"></asp:BoundField>
                                                    <%-- <asp:ButtonColumn Text="Edit/View" HeaderText="Edit/View" CommandName="View"></asp:ButtonColumn>
                                            <asp:ButtonColumn Text="Delete" HeaderText="Delete" CommandName="Delete"></asp:ButtonColumn>--%>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                        <%--6--%>
                                                        <ItemTemplate>
                                                            <div class="dropdown">
                                                                <div class="dropdownbtn-content">
                                                                    <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="EditRecord" runat="server" CausesValidation="false" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CausesValidation="false" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                                    CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                    Style="cursor: pointer" />
                                                            </div>
                                                       </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <itemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView></td>

                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
         <script type="text/javascript">
             function CallParentCallback() {
                 parent.ParentCallBackClassificationFunction();
                 return false;
             }
         </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Typepup") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameFlightLogClassificationComplete();
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
              //  onResize();//for Top bottom link
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
