<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCompanyDocumentList_Ajax.aspx.vb"
    Inherits="Flypal.wfCompanyDocumentList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Organisation Approval(s)</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript">
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">Organisation Approval List</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <span id="lblDocument" class="clsLabelAuto">Document</span>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbDocumentList" runat="server" CssClass="clsComboBox_Ajax"
                                                                                                    DataTextField="Name" DataValueField="ID">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                                    ToolTip="Click to find as per searching criteria" />
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDocs" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlbtnsTop" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddTop" runat="server" CssClass="clsButton" Text="Add New" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton" Text="Print" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgDocumentList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        DataKeyNames="ID,VendorID" ShowHeaderWhenEmpty="True" AllowSorting="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="VendorID" HeaderText="VendorID"></asp:BoundField>
                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" SortExpression="DocumentName">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssuingAuthority" HeaderText="Issuing Authority" SortExpression="IssuingAuthority">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DocNo" HeaderText="Document No" SortExpression="DocNo">
                                                                <HeaderStyle HorizontalAlign="Left" ForeColor="White" />
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateOfIssueFormatted" HeaderText="Date of Issue">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true">
                                                                </ItemStyle>
                                                            </asp:BoundField>
                                                            <%--  <asp:BoundField DataField="Validity" HeaderText="Periodicity">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="Right" Width="55px" Wrap="true" />
                                                            </asp:BoundField>--%>
                                                            <asp:BoundField DataField="DateOfExpiryFormatted" HeaderText="Date of Expiry">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true">
                                                                </ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                        CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                        Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" CausesValidation="false" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="View"
                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("ImageSize")>0 %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="ImageSize" HeaderText="ImageSize">
                                                                <HeaderStyle CssClass="hideGridColumn" />
                                                                <ItemStyle CssClass="hideGridColumn" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Renew">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IDRenew" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="Renew" Style="width: 20px" ImageUrl="images/Renew1.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="History">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="History" ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="HistoryCount" HeaderText="HistoryCount">
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
                            <td align="right">
                                <asp:UpdatePanel ID="upnlbtns" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsButton" Text="Add New" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsButton" Text="Print" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton" Text="Close" CausesValidation="False">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnCompanyDocument" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnCompanyDocumentHistory" ClientIDMode="Static" runat="server"
                                            Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
    <!-- Company Document Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompanyDocument" Text="Organisation Approval" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompanyDocument" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompanyDocument" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompanyDocument" runat="server" TargetControlID="btnDummyCompanyDocument"
        PopupControlID="pnlCompanyDocument" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompanyDocumentStateComplete() {
            $("#btnDummyCompanyDocument").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompanyDocumentWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompanyDocument").attr("src", "wfCompanyDocument_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompanyDocument").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompanyDocument() {
            var CompanyDocumentwindow = $find("<%=mdlPopupCompanyDocument.ClientID %>");
            //close kit popup window
            CompanyDocumentwindow.hide();
            //           release resources
            $("#IframeCompanyDocument").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnCompanyDocument").click();
        }
    </script>
    <!-- End-->
    <!-- Company Document History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCompanyDocumentHistory" Text="Organisation Approval History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCompanyDocumentHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCompanyDocumentHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCompanyDocumentHistory" runat="server" TargetControlID="btnDummyCompanyDocumentHistory"
        PopupControlID="pnlCompanyDocumentHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCompanyDocumentHistoryStateComplete() {
            $("#btnDummyCompanyDocumentHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCompanyDocumentHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCompanyDocumentHistory").attr("src", "wfCompanyDocumentHistoryList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCompanyDocumentHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCompanyDocumentHistory() {
            var CompanyDocumentHistorywindow = $find("<%=mdlPopupCompanyDocumentHistory.ClientID %>");
            //close popup window
            CompanyDocumentHistorywindow.hide();
            //           release resources
            $("#IframeCompanyDocumentHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCompanyDocumentHistory").click();
        }
    </script>
    <!-- End-->
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForDocDetail();
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
             parent.IFrameDocDetailStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       
       }
    </script>
    <%--End--%>
    </form>
</body>
</html>
