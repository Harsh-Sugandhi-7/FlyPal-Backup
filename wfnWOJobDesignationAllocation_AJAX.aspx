<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobDesignationAllocation_AJAX.aspx.vb"
    Inherits="Flypal.wfnWOJobDesignationAllocation_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout"
    style="font-size: small">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <script language="javascript" type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            $(document).ready(function () {
                function endRequestHandler() {

                    try {

                        //if (g_isTabPressed == 1) {
                        $get(g_CurrentTextBox).focus();
                        $get(g_CurrentTextBox).select();

                        g_isTabPressed = 0;
                        //}


                    }
                    catch (Error) { }

                }

            });
        </script>
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                function onTextFocus() {
                    g_CurrentTextBox = event.srcElement.id;

                }

                function onkeyPressed(keycode, obj) {

                    if (keycode == 9) {

                        g_isTabPressed = 1;
                    }

                }
            });
        </script>
        <%--AJAX- ScriptManager Added--%>
        <div>
            <table id="tblmain" class="clstablelistout">
                <tr>

                    <td colspan="3" class="clsFormHeader1Newstyle">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Designation Allocation Details</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpnlAddTop" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table3">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" Enabled="<%# mnWO.WOStatusID <> 3 %>"
                                                            Text="Add" ToolTip="Click to Add Designation Allocation Detail" ValidationGroup="a" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                            Text="Close" ToolTip="Click to close the Designation Allocation Detail screen" />
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
                    <td colspan="3">
                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsValidationSummary"
                                    Display="None" ValidationGroup="a"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvEstimatedTime" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtEstimatedTime" OnServerValidate="customvalidate"
                                    ValidationGroup="a"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvDesignation" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ErrorMessage="Designation Required." ControlToValidate="cmbDesignationList" ClientValidationFunction="validateDesignation"
                                    ValidationGroup="a"></asp:CustomValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <script type="text/javascript">
                            function validateDesignation(source, args) {
                                args.IsValid = false;

                                var dd = $get("cmbDesignationList");
                                if (dd.selectedIndex != 0) {
                                    args.IsValid = true;
                                    return;
                                }
                            }
                        </script>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                    <table width="99%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelStar" ForeColor="Red">*</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDesignation" runat="server" CssClass="clsLabel">Designation</asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <table id="Table2" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="cmbDesignationList" runat="server" CssClass="clsTextBoxTagSearchCombo"
                                                                DataTextField="Name" DataValueField="ID" Enabled="<%# mnWO.WOStatusID <> 3 %>">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgDesignation" runat="server" ImageUrl="~/images/plus1.png"
                                                                Enabled="<%# mnWO.WOStatusID <> 3 %>" Height="22px" Width="24px" ToolTip="Click to Add New Designation"
                                                                CausesValidation="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblEstimatedManHours" runat="server" CssClass="clsLabelAuto">Estimated Man Hours</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEstimatedTime" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
                                                    Enabled="<%# mnWO.WOStatusID <> 3 %>" MaxLength="7" ToolTip="Enter Estimated Time"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="lblRate" class="clsLabelAuto">Rate</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" MaxLength="12"
                                                    ToolTip="Enter Rate" Width="130px" Enabled="<%# mnWO.WOStatusID <> 3 %>" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span id="Span1" class="clsLabelAuto">Total</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotal" runat="server" CssClass="clsTextBoxTagSearchRightAlign1" MaxLength="12"
                                                    ToolTip="Enter Rate" Width="130px" ReadOnly="true" BackColor="Gainsboro"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblActualManHours" runat="server" CssClass="clsLabelAuto">Actual Man Hours</asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtActualTime" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchRightAlign1"
                                                    ReadOnly="True" ToolTip="Actual Time"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblDesignationlist" runat="server" CssClass="clsLabelHeader">Designation list</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="dgWOJobDesignationAllocation" runat="server" CssClass="clsGridNewStyle"
                                    ShowHeaderWhenEmpty="true" AllowSorting="True" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                    <HeaderStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                    <Columns>
                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                        <%--0--%>
                                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No."></asp:BoundField>
                                        <%--2--%>
                                        <asp:BoundField DataField="DesignationName" SortExpression="DesignationName" HeaderText="Designation">
                                            <HeaderStyle ForeColor="black"></HeaderStyle>
                                        </asp:BoundField>
                                        <%--3--%>
                                        <asp:BoundField DataField="EstimatedTime" HeaderText="Estimated Hr."></asp:BoundField>
                                        <%--4--%>
                                        <asp:BoundField DataField="WOTotalActualTime" HeaderText="Actual Hr."></asp:BoundField>
                                        <%--5--%>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate"></asp:BoundField>
                                        <%--6--%>
                                        <asp:BoundField DataField="Total" HeaderText="Total" />
                                        <%--7--%>
                                        <%--<asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRecord"></asp:ButtonField>--%><%--8--%>
                                        <%-- <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRecord"></asp:ButtonField>--%><%--9--%>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center"><%--8--%>
                                            <ItemTemplate>

                                                <div class="dropdown">
                                                    <div class="dropdownbtn-content">
                                                        <table id="T1" class="clsGridNew_Ajax">
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                        CommandName="EditRecord" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
                                                                        CommandName="DeleteRecord" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </div>

                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
                                                        Style="cursor: pointer" />
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:ButtonField Text="Assign Resource" HeaderText="Assign Resource" CommandName="AssignResource"></asp:ButtonField>
                                        <%--10--%><%--9--%>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td align="right">
                        <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <td align="right"></td>
                                <td align="right">
                                    <asp:UpdatePanel ID="UpnlClose" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCloseBottom" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                                Text="Close" ToolTip="Click to close the Designation Allocation Detail screen" Visible="false"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup for category/nomenclature-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgbtnDesignation" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnAddResourceAllocation" ClientIDMode="Static" runat="server"
                                                Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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
        </div>
        <div>
            <!-- Designation Popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyDesignation" Text="Dummy Designation" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlDesignation" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeDesignation" frameborder="0" height="100%" allowtransparency="true"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupDesignation" runat="server" TargetControlID="btnDummyDesignation"
                PopupControlID="pnlDesignation" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameStateComplete() {
                    $("#btnDummyDesignation").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenDesignationWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeDesignation").attr("src", "wfDesignation_Ajax.aspx?Type=pup");
                        // $("#IframeDesignation").load(function () {
                        //                    var doc = IframeDesignation.window;
                        //                    IframeDesignation.SetPageLayout();

                        if (!$.browser.msie) {
                            $("#btnDummyDesignation").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }


                        //});


                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForDesignation() {
                    var Designationwindow = $find("<%=mdlPopupDesignation.ClientID %>");
                    //close Designation popup window
                    Designationwindow.hide();
                    //           release resources
                    $("#IframeDesignation").attr("src", "JavaScript:''");
                    //call Designation image button
                    $("#hdnimgbtnDesignation").click();
                }
                function CallParentDesignationWindow() {
                    window.parent.OpenDesignationWindow();
                }

            </script>
            <!-- End-->
        </div>
        <!-- ResourceAllocation Popup Window -->
        <%-- 'Added by Saylee on 29-May-2019--%>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyResourceAllocation" Text="Dummy ResourceAllocation"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupResourceAllocation" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="iPopupResourceAllocation" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupResourceAllocation" runat="server" TargetControlID="btnDummyResourceAllocation"
            PopupControlID="pnlPopupResourceAllocation" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameResourceAllocationStateComplete() {
                $("#btnDummyResourceAllocation").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }

            function OpenToAddResourceAllocation() {
                try {
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupResourceAllocation").attr("src", "wfnWOJobResourceAllocation_AJAX.aspx?Type=pup");
                    if (!$.browser.msie) {
                        $("#btnDummyResourceAllocation").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }
            function CallParentOpenToAddResourceAllocation() {
                window.parent.OpenToAddResourceAllocation();
            }
            function autoWOJobDesignationAllocationList() {
                //  window.parent.autoWOJobDesignationAllocationList();
                window.parent.ParentRefresh();
            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForResourceAllocation() {
                var ResourceAllocationWindow = $find("<%=mdlPopupResourceAllocation.ClientID %>");
                //close ResourceAllocation popup window
                ResourceAllocationWindow.hide();
                $("#iPopupResourceAllocation").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddResourceAllocation").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForDesignaionAllocation();
                return false;
            }
            function CallCloseChildPage() {

                window.parent.CloseChildPage();
            }
        </script>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameDesignaionAllocationStateComplete();
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
    <script language="javascript">
        function SetTabCount(CountForTab) {
            //            if (CountForTab == -1) {
            //                var totalRowCount = 0;
            //                var rowCount = 0;
            //                var gridView = document.getElementById("<%=dgWOJobDesignationAllocation.ClientID %>");
            //                var rows = gridView.getElementsByTagName("tr")
            //                for (var i = 0; i < rows.length; i++) {
            //                    totalRowCount++;
            //                    if (rows[i].getElementsByTagName("td").length > 0) {
            //                        rowCount++;
            //                    }
            //                }
            //                parent.document.getElementById("Label3").innerHTML = rowCount;
            //            }
            //            else {
            parent.document.getElementById("Label3").innerHTML = CountForTab;
            //            }
        }
    </script>
</body>
</html>
