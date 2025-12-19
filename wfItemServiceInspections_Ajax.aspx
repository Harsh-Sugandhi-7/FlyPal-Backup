<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfItemServiceInspections_Ajax.aspx.vb"
    Inherits="Flypal.wfItemServiceInspections_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Maintenance Done By Employee</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                        <table class="clstablelistin" id="tblLedgerList">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblPartList" class="clsFormHeader">Service Inspections</span>
                                            </td>

                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add the Item"
                                                                        Text="Add" CausesValidation="true" ValidationGroup="1"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous Page"
                                                                        CausesValidation="false" Text="Back"></asp:Button>
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
                                    <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" ValidationGroup="1" runat="server"
                                                CssClass="clsValidationSummary" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rqLicenceNo" runat="server" ControlToValidate="cmbServiceInspectionName"
                                                CssClass="clsLabelAuto" ForeColor="Red" ErrorMessage=" Description Required"
                                                ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvLicenseNo" runat="server" CssClass="clsLabelAuto" ErrorMessage="Description Required"
                                                Display="None" ControlToValidate="cmbServiceInspectionName" OnServerValidate="CustomValidate"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ValidationGroup="1"
                                                ValidateEmptyText="true" ControlToValidate="txtFrequency" ErrorMessage="Enter Frequency"
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvFrequencyIn" runat="server" Display="None" ValidationGroup="1"
                                                ValidateEmptyText="true" ControlToValidate="cmbServiceInspectionIntervalIn" ErrorMessage="Frequency Interval Required"
                                                OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3">
                                                <tr>
                                                    <td>
                                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <span class="clsLabelStar">*</span>
                                                                    </td>
                                                                    <td>
                                                                        <span id="Span1" class="clsLabelAuto">Description</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDescription" Visible="false" runat="server" CssClass="clsTextBoxTagSearch"
                                                                            ToolTip="Enter Description" MaxLength="200"></asp:TextBox>
                                                                        <asp:DropDownList ID="cmbServiceInspectionName" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                            DataTextField="ServiceInspectionName" DataValueField="ID" Width="185px" ClientIDMode="Static">
                                                                        </asp:DropDownList>
                                                                        &nbsp;&nbsp;
                                                                    <asp:ImageButton ID="imgServiceInspections" runat="server" ImageUrl="~/images/plus1.png"
                                                                        Height="22px" Width="24px" ToolTip="Click to Add New Service Inspections"></asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Label ID="lblRequiredmanHours" runat="server" CssClass="clsLabelAuto">Frequency</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtFrequency" runat="server" Width="55px" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                            ToolTip="Enter Frequency" MaxLength="8" ValidationGroup="1">
                                                                        </asp:TextBox>
                                                                        <asp:DropDownList ID="cmbServiceInspectionIntervalIn" runat="server" ClientIDMode="Static"
                                                                            CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Width="100px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"> List of Service Inspections Nos. : 0 Record(s) found.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgServiceInspectionsList" runat="server"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" CssClass="clsGridNewStyle"
                                                            DataKeyNames="ID" ForeColor="Black" GridLines="Horizontal" PageSize="10" ShowHeaderWhenEmpty="true">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader"></HeaderStyle>
                                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Frequency" SortExpression="Frequency" HeaderText="Frequency">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField Visible="false" DataField="FrequencyPeriod" SortExpression="FrequencyPeriod"
                                                                    HeaderText="FrequencyPeriod">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FrequencyPeriodIn" SortExpression="FrequencyPeriodIn"
                                                                    HeaderText="Frequency In">
                                                                    <HeaderStyle Wrap="False" ForeColor="Black" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <%-- <span id="button">Login</span>--%>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditRec" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                                                CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                                CausesValidation="false" />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                                                CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                                CausesValidation="false" />
                                                                                        </td>

                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" BackColor="white" ForeColor="Black"  />
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
                            </tr>
                            <!--Dummy panel to open modelpopup for Service Inspection Name-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnServiceInspactionsName" ClientIDMode="Static" runat="server"
                                                Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
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
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForServiceInspactions();
                return false;
            }
        </script>
        <%--End--%>
        <!-- Service Inspactions Name Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyServiceInspactions" />
        </div>
        <asp:Panel runat="server" ID="pnlServiceInspactions" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="IServiceInspactionsName" allowtransparency="true" frameborder="0" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupServiceInspactions" runat="server" TargetControlID="btnDummyServiceInspactions"
            PopupControlID="pnlServiceInspactions" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceInspactionNameStateComplete() {
                $("#btnDummyServiceInspactions").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }


            function AddServiceInspections() {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IServiceInspactionsName").attr("src", "wfServiceInspectionName.aspx?Type=pup&MaintTypeID=1");

                    //  if (!$.browser.msie) {
                    $("#btnDummyServiceInspactions").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    // }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForServiceInspactionName() {
                var ServiceInspactionswindow = $find("<%=mdlPopupServiceInspactions.ClientID %>");
                //close Ass Insp Maint Done By Emp popup window
                ServiceInspactionswindow.hide();
                //Free resources
                $("#IServiceInspactionsName").attr("src", "JavaScript:''");
                $("#hdnBtnServiceInspactionsName").click();

            }
        </script>
        <!-- End -->
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameServiceInspactionsStateComplete();
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
                var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
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
    </form>
</body>
</html>
