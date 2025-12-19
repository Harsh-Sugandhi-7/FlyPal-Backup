<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeTrainingForRenewal_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeTrainingForRenewal_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Training Renewal</title>
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <script language="javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Employee Training Renewal</span>
                                        </td>
                                        <td valign="top" align="right">
                                            <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" 
                                                       ToolTip="Click to close Employee Training Renewal screen" CausesValidation="False"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataTextField="EmpNoName" DataValueField="ID" EnableViewState="false" onChange="setEmployeeValue()">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkUsedInFlightLog" runat="server" CssClass="clsCheckBox" Text="Used In Flight Log">
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkExpiredEntries" runat="server" CssClass="clsCheckBox" Text="Expired Entries Only">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table3" border="0">
                                                        <tr>
                                                            <td>
<%--                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                    ToolTip="Click to find Records as per searching criteria"></asp:Button>--%>

                                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" 
                                                                ToolTip="Click to find list as per searching criteria" />
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
                                <asp:UpdatePanel ID="upnlTrainingGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                         <tr>
                                                <td>
                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">Note : Following list shows records whose expiry date falls in next 2 months or below.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">The following Trainings are due for renewal</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:GridView ID="dgTrainingList" runat="server" AutoGenerateColumns="False"
                                                        ShowHeaderWhenEmpty="true" ClientIDMode="Static" AllowPaging="true" PageSize="25"
                                                        CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-Width="130px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesignationName" HeaderText="Designation" ItemStyle-Width="130px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TrainingName" HeaderText="Training" ItemStyle-Width="200px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No" ItemStyle-Width="160px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Date" HeaderText="Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField> 
                                                            <asp:BoundField DataField="Duration" HeaderText="Training Duration">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TrainingOrgName" HeaderText="Training Orgnisation" ItemStyle-Width="130px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MonthOfTrainingName" HeaderText="Month of Training">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="YearOfTraining" HeaderText="Year of Training">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" ItemStyle-Width="130px">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                          <%--  <asp:TemplateField HeaderText="Renew">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IDRenew" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="Renew" Style="width: 20px" ImageUrl="images/Renew1.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="View">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="View"
                                                                        Style="height: 20px; width: 13px"  ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="History">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="History" ImageUrl="~/images/History.png"  Visible='<%#  Eval("HistoryCount")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
                                                            <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                DataField="HistoryCount" HeaderText="HistoryCount"></asp:BoundField>

                                                            
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <div id="dropDownImg" class="dropdown">
                                                                        <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
                                                                        <div id="dropdownICN-content" class="dropdownbtn-content">
                                                                            <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                <tr>

                                                                                    <td>
                                                                                        <asp:ImageButton ID="IDRenew" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Renew"
                                                                                            Style="height: 20px; width: 20px" ImageUrl="~/images/Renew1.png" ToolTip="Click to Renew" CausesValidation="false"/>
                                                                                    </td>
                                                                                   
                                                                                    <td>
                                                                                        <asp:ImageButton ID="View" Style="height: 20px; width: 13px" runat="server"
                                                                                            CommandArgument='<%# Eval("ID") %>'
                                                                                            ToolTip="Click to View Attachment"
                                                                                            CommandName="View" ImageUrl="icons/CLIP01.ICO"
                                                                                            Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="IDHistory" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="History"
                                                                                            ImageUrl="~/images/History.png" Visible='<%#  Eval("HistoryCount")%>'
                                                                                            ToolTip="Click to View History" />

                                                                                    </td>
                                                                                    
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
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
                            <%--<td valign="top" align="right">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" CausesValidation="False">
                                        </asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                        </tr>
                        <!--Dummy hidden btn panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnEmpTraining" ClientIDMode="Static" runat="server" Text="Add"
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
    <!-- Employee Training Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpTraining" Text="Employee Training" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpTraining" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpTraining" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpTraining" runat="server" TargetControlID="btnDummyEmpTraining"
        PopupControlID="pnlEmpTraining" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
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
    <!-- hidden fields to set combobox selected value at client side -->
    <asp:HiddenField ID="EmployeeIDValue" runat="server" ClientIDMode="Static" />
    <!-- End-->
    <!-- javascript function to set combobox selected value to appropriate hidden field for Part Information-->
    <script type="text/javascript">
        function setEmployeeValue(elem, combo) {
            var id = $get("cmbEmployeeList").value;
            $("#EmployeeIDValue").val(id);
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
    <cc2:ModalPopupExtender ID="mdlPopupEmpTrainingHistory" runat="server" TargetControlID="btnDummyEmpTrainingHistory"
        PopupControlID="pnlEmpTrainingHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
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
    </form>
</body>
</html>
