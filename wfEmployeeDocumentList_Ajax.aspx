<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDocumentList_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDocumentList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Document(s)</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script type="text/javascript">      
        function openFilel() {
            str = "wfFileView.aspx"
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
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Employee Document List</span>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel runat="server" ID="upnlbtnsTop" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" CausesValidation="False" ToolTip="Click to Add Employee Document"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" CausesValidation="False" ToolTip="Click to go back to previous page"></asp:Button>
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
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblCODE" class="clsLabelAuto">Emp No</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmpNo" runat="server" CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0"
                                                ReadOnly="True" ToolTip="Enter Code" Text="<%# mEmployee.EmpNo %>">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <span id="lblName" class="clsLabelAuto">Name</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" BackColor="#E0E0E0"
                                                ReadOnly="True" ToolTip="Enter Name" Text="<%# mEmployee.Name %>">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDocs" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <%--<td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlbtnsTop" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnBackTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" CausesValidation="False">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="dgDocumentList" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <Columns>
                                                            <%--0--%>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <%--1--%>
                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                            <%--2--%>
                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--3--%>
                                                            <asp:BoundField DataField="DocNo" HeaderText="Document No">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--4--%>
                                                            <asp:BoundField DataField="DateOfIssueFormatted" HeaderText="Date of Issue">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true">
                                                                </ItemStyle>
                                                            </asp:BoundField>
                                                            <%--5--%>
                                                            <asp:BoundField DataField="PlaceOfIssue" HeaderText="Place of Issue">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField DataField="Validity" HeaderText="Validity">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--7--%>
                                                            <asp:BoundField DataField="DateOfExpiryFormatted" HeaderText="Date of Expiry">
                                                                <HeaderStyle Wrap="False"></HeaderStyle>
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true">
                                                                </ItemStyle>
                                                            </asp:BoundField>
                                                            <%--8--%>
                                                            <asp:TemplateField HeaderText="Applicable">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                        Enabled="False"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--9--%>
                                                            <asp:BoundField DataField="IssuingAuthority" HeaderText="Issuing Authority">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--10--%>
                                                            <asp:BoundField DataField="WarningDays" HeaderText="Warning Days">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="95px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <%--12--%>
                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew" HeaderStyle-CssClass="hideGridColumn">
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                            </asp:ButtonField>
                                                            <%--13--%>
                                                             <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec" HeaderStyle-CssClass="hideGridColumn">
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                             </asp:ButtonField>
                                                            <%--14--%>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-CssClass="hideGridColumn">
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                            </asp:ButtonField>
                                                            <%--15--%>
                                                            <asp:TemplateField HeaderText="Attach" HeaderStyle-CssClass="hideGridColumn">
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lnkDocumentView" Text="View" CommandName="View"
                                                                        CausesValidation="false"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--16--%>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>
                                                            <%--17--%>
                                                            <asp:TemplateField HeaderText="History" HeaderStyle-CssClass="hideGridColumn">
                                                                <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDocumentHistory" runat="server" Text="History" CommandName="History"
                                                                        CausesValidation="false"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--18--%>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>
                                                            <%--19--%>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsApplicable" HeaderText="IsApplicable"></asp:BoundField>
                                                            <%--20--%>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="OneTimeDocument" HeaderText="OneTimeDocument"></asp:BoundField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                
                                                                <div class="dropdown">
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" />
                                                                                </td>
                                                                               <td>
                                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="View" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("ImageSize")>0 %>'/>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="History" 
                                                                                    ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>' 
                                                                                    ToolTip="Click to View History"/>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="Renew" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' CommandName="Renew" 
                                                                                    Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png" 
                                                                                    Visible='<%# Eval("IsApplicable") = True And Eval("OneTimeDocument") = False %>' ToolTip="Click to Renew"
                                                                                    />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

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
                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" CausesValidation="False" Visible="false">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" CausesValidation="False" Visible="false">
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
                                        <asp:Button ID="hdnBtnEmpDocument" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpDocumentHistory" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    <!-- Employee Document Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDocument" Text="Employee Document" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDocument" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDocument" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDocument" runat="server" TargetControlID="btnDummyEmpDocument"
        PopupControlID="pnlEmpDocument" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDocumentStateComplete() {
            $("#btnDummyEmpDocument").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDocumentWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDocument").attr("src", "wfEmployeeDocument_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDocument").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDocument() {
            var EmpDocumentwindow = $find("<%=mdlPopupEmpDocument.ClientID %>");
            //close kit popup window
            EmpDocumentwindow.hide();
            //           release resources
            $("#IframeEmpDocument").attr("src", "JavaScript:''");
            //call kit image button
            $("#hdnBtnEmpDocument").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Document History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDocumentHistory" Text="Employee Document History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDocumentHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDocumentHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDocumentHistory" runat="server" TargetControlID="btnDummyEmpDocumentHistory"
        PopupControlID="pnlEmpDocumentHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDocumentHistoryStateComplete() {
            $("#btnDummyEmpDocumentHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDocumentHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDocumentHistory").attr("src", "wfEmployeeDocumentHistoryList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDocumentHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDocumentHistory() {
            var EmpDocumentHistorywindow = $find("<%=mdlPopupEmpDocumentHistory.ClientID %>");
            //close popup window
            EmpDocumentHistorywindow.hide();
            //           release resources
            $("#IframeEmpDocumentHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpDocumentHistory").click();
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
