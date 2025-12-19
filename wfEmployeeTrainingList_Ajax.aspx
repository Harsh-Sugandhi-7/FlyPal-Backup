<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeTrainingList_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeTrainingList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Training(s)</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
                <uc1:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="3" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lbltitle" class="clsFormHeader">Employee Training List</span>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel runat="server" ID="upnlbtnsTop" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" CausesValidation="False" ToolTip="Click to Add Employee training"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" CausesValidation="False" ToolTip="Click to print"></asp:Button>
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
                                                        <asp:Label ID="lblTraining" runat="server" CssClass="clsLabelHeader"></asp:Label>
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
                                                        <asp:GridView ID="dgTrainingList" runat="server" AutoGenerateColumns="False"
                                                            ShowHeader="true" DataKeyNames="ID,EmployeeID"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                                <asp:BoundField DataField="TrainingName" HeaderText="Training Name">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="110px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="100px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EmployeeTrainingDate" HeaderText="Date">
                                                                    <HeaderStyle Wrap="False"></HeaderStyle>
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="90px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Duration" HeaderText="Duration">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TrainingOrgNameWithCity" HeaderText="Training Org Name">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="130px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MonthOfTrainingName" HeaderText="Month Of Training">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="125px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="YearOfTraining" HeaderText="Year of Training">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="115px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="150px" Wrap="true" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew" HeaderStyle-CssClass="hideGridColumn">
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec" HeaderStyle-CssClass="hideGridColumn">
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="70px" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec" HeaderStyle-CssClass="hideGridColumn">
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="View" HeaderText="Attach" CommandName="View" HeaderStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" Width="70px" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="50px" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <%--<asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>--%>

                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="ImageSize" HeaderText="ImageSize"></asp:BoundField>

                                                                <asp:ButtonField Text="History" HeaderText="History" CommandName="History" HeaderStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="left" Width="55px" Wrap="true" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>

                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="RecurringStatus" HeaderText="RecurringStatus"></asp:BoundField>

                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="IsAttachmentAdded" HeaderText="RecurringStatus"></asp:BoundField>

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
                                                                                            <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="View" ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("IsAttachmentAdded") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' CommandName="History"
                                                                                                ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>'
                                                                                                ToolTip="Click to View History" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="Renew" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>' CommandName="Renew"
                                                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png"
                                                                                                Visible='<%# Eval("RecurringStatus")  %>' ToolTip="Click to Renew" />
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
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" CausesValidation="False" Visible="false"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" CausesValidation="False" Visible="false"></asp:Button>
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
                                            <asp:Button ID="hdnBtnEmpTraining" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnEmpTrainingHistory" ClientIDMode="Static" runat="server" Text="Add"
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
        <!-- Employee Training Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpTraining" Text="Employee Training" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpTraining" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpTraining" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupEmpTraining" runat="server" targetcontrolid="btnDummyEmpTraining"
            popupcontrolid="pnlEmpTraining" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function IFrameEmpTrainingStateComplete() {
                $("#btnDummyEmpTraining").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpTrainingWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpTraining").attr("src", "wfEmployeeTraining_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpTraining").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function OpenTrainingGroupWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpTraining").attr("src", "wfTrainingGroupSelectionList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpTraining").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpTraining() {
                var EmpTrainingwindow = $find("<%=mdlPopupEmpTraining.ClientID %>");
                //close Training popup window
                EmpTrainingwindow.hide();
                //           release resources
                $("#IframeEmpTraining").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpTraining").click();
            }
        </script>
        <!-- End-->
        <!-- Employee Training History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyEmpTrainingHistory" Text="Employee Training History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlEmpTrainingHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeEmpTrainingHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:modalpopupextender id="mdlPopupEmpTrainingHistory" runat="server" targetcontrolid="btnDummyEmpTrainingHistory"
            popupcontrolid="pnlEmpTrainingHistory" backgroundcssclass="clsModalPopupBG">
        </cc2:modalpopupextender>
        <script type="text/javascript">
            function IFrameEmpTrainingHistoryStateComplete() {
                $("#btnDummyEmpTrainingHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenEmpTrainingHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeEmpTrainingHistory").attr("src", "wfEmployeeTrainingHistoryList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyEmpTrainingHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForEmpTrainingHistory() {
                var EmpTrainingHistorywindow = $find("<%=mdlPopupEmpTrainingHistory.ClientID %>");
                //close Training popup window
                EmpTrainingHistorywindow.hide();
                //           release resources
                $("#IframeEmpTrainingHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnEmpTrainingHistory").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForTrainingDetail();
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
                    parent.IFrameTrainingDetailStateComplete();
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
