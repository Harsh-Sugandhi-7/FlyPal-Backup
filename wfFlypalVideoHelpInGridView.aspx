<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlypalVideoHelpInGridView.aspx.vb"
    Inherits="Flypal.wfFlypalVideoHelpInGridView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Flypal Video Help</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblSearch" class="clsLabel">Search</span>
                            </td>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTextSearch_Ajax" MaxLength="1000"
                                            AutoPostBack="true"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="dgGridView" runat="server" AutoGenerateColumns="False" Width="400px"
                                            Height="80px" DataKeyNames="id" PageSize="1" AllowPaging="True">
                                            <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <video width="1000" controls controlslist="nodownload" src="<%# Eval("VideoPath") %>">
                          
                          </video>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous" />
                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--<td>
                                <asp:UpdatePanel runat="server" ID="upnlVideo" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <video width="1000" controls id="Vediosource" runat="server">
                                <%--<source  src="Vedio/PO-Final.mp4"" type="video/mp4">--%>
                            <%--<source type="video/mp4" >
                                </video>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td> --%>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
     <script type="text/javascript">
         document.addEventListener("contextmenu", function (e) {
             e.preventDefault();
         }, false);
 
    </script>
    </form>
</body>
</html>
