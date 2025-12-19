<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfGroupTrainingRenewalList.aspx.vb"
    Inherits="Flypal.wfGroupTrainingRenewalList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Group Training Renewal List</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
      <script language="javascript" id="clientEventHandlersJS">
          function openledgersame(FileName) {
              window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
          }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox runat="server" ID="MSGBoxCntrl" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Group Training Renewal List</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right" valign="bottom" colspan="2">
                                            <%-- AJAX Update Panel --%>
                                            <table id="Table2" align="right" style="position: relative; width: 100%;">
                                                <tr>
                                                    <td valign="bottom" align="right">
                                                        <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnBack" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Group Training Renewal List screen"
                                                                    Text="Close" CausesValidation="False"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                            ValidationGroup="1"></asp:ValidationSummary>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Search</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearch" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4" border="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search By</asp:Label>
                                                </td>
                                                <td>
                                                    <%-- AJAX Update Panel --%>
                                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbSearchType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                            <asp:ListItem Value="1">Name</asp:ListItem>
                                                                            <asp:ListItem Value="2">Training Type</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFor" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="20"
                                                                            Visible="False" AutoPostBack="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <%-- AJAX Update Panel --%>
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnFindNow" CssClass="clsButton_Ajax" runat="server" ToolTip="Click to find the list of Training as per searching criteria"
                                            Text="Find Now" CausesValidation="False" Visible="False"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgTrainingList" runat="server" AllowSorting="True"
                                                        ClientIDMode="Static" AutoGenerateColumns="False" AllowPaging="true" PageSize="15"
                                                        ShowHeaderWhenEmpty="true" DataKeyNames="ID" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Training Name" ItemStyle-Width="200px">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TrainingTypeName" SortExpression="TrainingTypeName" HeaderText="Training Type" ItemStyle-Width="170px">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FreqInMonths" SortExpression="FreqInMonths" HeaderText="Freq In months">
                                                                <HeaderStyle HorizontalAlign="Right"  Wrap="true" Width="60px">
                                                                </HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right" Width="60px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WarningDays" SortExpression="WarningDays" HeaderText="Warning Days">
                                                                <HeaderStyle HorizontalAlign="Right"  Width="60px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right" Width="60px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="IsEmployeeTrainingExists" HeaderText="IsEmployeeTrainingExists"
                                                                HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                          
                                                            <asp:TemplateField HeaderText="Renew">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="IDRenew" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                        CommandName="Renew" Style="width: 20px" ImageUrl="images/Renew1.png" CausesValidation="false"/>
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
                            <%--<td align="right" valign="bottom" colspan="2">
                                <%-- AJAX Update Panel 
                                <table id="Table2" align="right" style="position: relative; width: 100%;">
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBack" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Training Information screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnEmployeeTraining" ClientIDMode="Static" runat="server" Text="----"
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
    <!--Employee Training Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmployeeTraining" Text="Model Mod Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmployeeTraining" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmployeeTraining" frameborder="0" height="100%" allowtransparency="true"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmployeeTraining" runat="server" TargetControlID="btnDummyEmployeeTraining"
        PopupControlID="pnlEmployeeTraining" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmployeeTrainingStateComplete() {
            $("#btnDummyEmployeeTraining").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenGroupEmpTrainingWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmployeeTraining").attr("src", "wfGroupTrainingRenewal.aspx?Type=pup");


                $("#btnDummyEmployeeTraining").click();
                $get("AjaxLoader").style.visibility = 'hidden';



                //});


                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpTraining() {
            var EmployeeTrainingwindow = $find("<%=mdlPopupEmployeeTraining.ClientID %>");
            //close Model Mod Master popup window
            EmployeeTrainingwindow.hide();
            //           release resources
            $("#IframeEmployeeTraining").attr("src", "JavaScript:''");
            //call Model Mod Master image button
            $("#hdnBtnEmployeeTraining").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
