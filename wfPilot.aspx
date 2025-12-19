<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPilot.aspx.vb" Inherits="Flypal.wfPilot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Flying Crew</title>

    <link id="MainStyle" type="text/css" rel="stylesheet">

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <script src="js/query-1.7.1.js" type="text/javascript"></script>

        <%--Modified by Harsh Sugandhi on 5th Feb 2025 => Resolved Multiple Header Column Issue of GridView--%>
        <script type="text/javascript">

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                console.log("Page loaded. Starting the header fix process.");

                // Check if the header is already added to avoid repeating
                if ($('#GHead').children().length === 0) {
                    console.log("Header is not found. Cloning the GridView header.");

                    var gridHeader = $('#<%=dgPilot.ClientID%>').clone(true); // Clone the GridView
                    console.log("GridView Header Cloned.");

                    $(gridHeader).find("tr:gt(0)").remove(); // Remove all rows except the first one (header row)
                    console.log("Removed all rows except the Header.");

                    $('#<%=dgPilot.ClientID%> tr th').each(function (i) {
                        console.log("Setting width for header column " + (i + 1));
                        // Set the width of each th in the cloned header
                        $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width() + 1).toString() + "px");
                    });

                    // Append the header only if it hasn't been added before
                    console.log("Appending the header to #GHead.");
                    $("#GHead").append(gridHeader);

                    // Set CSS styles for the header
                    console.log("Setting position and top for the header.");
                    $('#GHead').css('position', 'absolute');
                    $('#GHead').css('top', $('#<%=dgPilot.ClientID%>').offset().top);

                    console.log("Header fix process completed.");
                } else {
                    console.log("Header already exists. No need to clone again.");
                }
            });

        </script>

        <%-- AJAX ScriptManager --%>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%-- AJAX Update Panel FOr Message Box --%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox runat="server" id="MSGBoxCtrl" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td colspan="1">
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="clstablelistin" id="tblInner">
                                    <tr>
                                        <td class="clsFormHeader1">
                                            <asp:UpdatePanel ID="upnltitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td width="100%">
                                                                <asp:Label ID="lbltitle" CssClass="clsFormHeader" runat="server">Flying Crew [New]</asp:Label></td>
                                                            <td>
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" ClientIDMode="Static" ToolTip="Click to add the new Flying Crew"
                                                                    Text="New" CausesValidation="False"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" ClientIDMode="Static" runat="server" ToolTip="Click to save the Flying Crew Information"
                                                                    Text="Save"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to close Flying Crew screen"
                                                                    Text="Close" CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required" ValidationGroup="a"
                                                Display="None" ControlToValidate="txtPilotName">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" CssClass="clsLabelAuto" ErrorMessage="Code Required" ValidationGroup="a"
                                                Display="None" ControlToValidate="txtCode">
                                            </asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvDesignation" runat="server" ErrorMessage="Select Designation From List" Display="None" ValidationGroup="a"
                                                ControlToValidate="cmbDesignationList" OnServerValidate="CustomValidate">
                                            </asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDet" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; margin-top: -5px">
                                                        <legend id="Legend3" runat="server" style="font-weight: bold">Flying Crew Details</legend>
                                                        <table>

                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblName1" runat="server" CssClass="clsLabelStar">*</asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblName" runat="server" CssClass="clsLabel">Name</asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPilotName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Pilot's Name" Text="<%# mEmployee.Name %>" MaxLength="50">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td align="right"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCode1" runat="server" CssClass="clsLabelStar">*</asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblCode" runat="server" CssClass="clsLabel">Short Name</asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Code" Text="<%# mEmployee.EmpNo %>" MaxLength="4">
                                                                    </asp:TextBox>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblDesignation" runat="server" CssClass="clslabel">Designation</asp:Label></td>
                                                                <td>
                                                                    <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbDesignationList" runat="server"
                                                                                    CssClass="clsTextBoxTagSearchComboSmall"
                                                                                    SelectedValue="<%# mEmployee.DesignationID %>"
                                                                                    DataTextField="Name" DataValueField="ID" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgBtnAddDesignation" runat="server"
                                                                                    CausesValidation="False" ValidationGroup="b"
                                                                                    ImageUrl="~/images/plus1.png" CssClass="AddNewICN"
                                                                                    ToolTip="Click to Add New Designation" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Flying Crew List</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="GHead" style="overflow: auto; z-index: 1; position: relative;">
                                                    </div>
                                                    <div style="height: 275px; overflow: auto; width: 100%">
                                                        <asp:GridView ID="dgPilot" runat="server" CssClass="clsGridNewStyle"
                                                            AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="true"
                                                            CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--1--%>
                                                                <asp:BoundField DataField="Name" HeaderText="Name">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="EmpNo" HeaderText="Short Name">
                                                                    <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:BoundField DataField="DesignationName" HeaderText="Designation">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--4--%>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax"
                                                                                    style="z-index: 7; position: relative;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgEditView" runat="server"
                                                                                                CommandName="View" CssClass="actionICNS"
                                                                                                ImageUrl="~/images/edit.png" CausesValidation="false"
                                                                                                CommandArgument='<%# Eval("ID") %>'
                                                                                                Visible='<%# Eval("IsWorking") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ImgDeleteRecord" runat="server"
                                                                                                CommandName="DeleteRec" CssClass="largerActionICNS"
                                                                                                ImageUrl="~/images/delete.png" CausesValidation="false"
                                                                                                CommandArgument='<%# Eval("ID") %>'
                                                                                                Visible='<%# Eval("IsWorking") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png"
                                                                                runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <%--                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="Note">
                                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto"
                                                    Text="Highlighted Crew Members are currently marked as Not Working and cannot be Edited." />
                                            </div>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:Button ID="hdnimgBtnDesignation" ClientIDMode="Static"
                                                runat="server" Text="----" CausesValidation="False"
                                                Style="display: none;" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <div id="ScriptsAndPopUps">

            <%--call parent function after completing subroutine..(when page open as popup)--%>
            <script type="text/javascript">
                function CallParentCallback() {
                    parent.ParentCallBackPilotFunction();
                    return false;
                }
            </script>
            <%--End--%>

            <%--Set page layout when open as popup aspx page--%>
            <script type="text/javascript">

                <% Dim mopen As String = Request.QueryString("Typepup") %>
                <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFramePilotComplete();
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
                    //   onResize();//for Top bottom link
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

            <div id="popup">

                <!-- Designation Popup -->
                <div style="display: none">
                    <asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static" />
                </div>
                <asp:Panel runat="server" ID="pnlDesignation" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                    <iframe id="iPopupDesignation" frameborder="0" allowtransparency="true" height="100%"
                        width="100%" src="JavaScript:''" scrolling="auto"></iframe>
                </asp:Panel>
                <cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
                    PopupControlID="pnlDesignation" BackgroundCssClass="clsModalPopupBG">
                </cc2:ModalPopupExtender>
                <script type="text/javascript">
                    function IFrameDesignationComplete() {
                        $("#btnDummyDesignation").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    function OpenDesignationWindow() {
                        try {
                            $get("AjaxLoader").style.visibility = "visible";
                            $("#iPopupDesignation").attr("src", "wfDesignation_AJAX.aspx?Type=pup");
                            if (!$.browser.msie) {
                                $("#btnDummyDesignation").click();
                                $get("AjaxLoader").style.visibility = "hidden";
                            }

                            return false;
                        } catch (e) {
                            alert(e);
                        }

                    }
                </script>
                <script type="text/javascript">
                    function ParentCallBackFunctionForDesignation() {
                        var atawindow = $find("<%=mdlPopupDesignation.ClientID %>");
                        //close ata popup window
                        atawindow.hide();
                        $("#iPopupDesignation").attr("src", "JavaScript:''");
                        //call ata image button
                        $("#hdnimgBtnDesignation").click();
                    }
                </script>
                <!-------------------->

            </div>

        </div>

    </form>
</body>
</html>
