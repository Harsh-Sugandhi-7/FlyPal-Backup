<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnIssuedWOSpares_AJAX.aspx.vb"
    Inherits="Flypal.wfnIssuedWOSpares_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Issued Spares</title>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }

        //this function takes a value (ltext) and transmits that to the left hand frame

        function tranRight(ltext) {
            parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

        }
    </script>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css">
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <form id="Form1" method="post" runat="server">
        <%--AJAX- ScriptManager Added--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%--AJAX- Add MSGBox Control--%>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblLedgerList" class="clstablelistin" width="100%">
                            <tr>
                                <td class="clsFormHeader1">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblListOrder" runat="server" CssClass="clsFormHeader">Issued Spares</asp:Label>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td align="right">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table class="clstableButton" align="right">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to Save W.O Pending Issued Spares List"
                                                                        CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to Close to Go back to the Previous Screen"></asp:Button>
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
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvControlValidator" runat="server" CssClass="clsLabelAuto"
                                                Display="None"></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgIssuedSpares" runat="server" CssClass="clsGridNewStyle" AllowSorting="True"
                                                ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"  PageSize="3" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" Height="50px" />
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                <Columns>
                                                    <%--0--%>
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <%--1--%>
                                                    <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issued Date">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--2--%>
                                                    <asp:BoundField DataField="IssueNumber" SortExpression="IssueNumber" HeaderText="Issued No.">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--3--%>
                                                    <asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="PartNo">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--4--%>
                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--5--%>
                                                    <asp:BoundField DataField="IssuedQty" HeaderText="Issued Qty">
                                                        <HeaderStyle ></HeaderStyle>
                                                    </asp:BoundField>
                                                    <%--6--%>
                                                    <asp:TemplateField HeaderText="Used Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBox" runat="server" CssClass="clsTextBoxRightAlignSmall" ToolTip="Enter Used Qty." Height="15px"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "UsedQty") %>' MaxLength="5">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--7--%>
                                                    <asp:BoundField DataField="ReturnQty" SortExpression="ReturnQty" HeaderText="Return Qty.">
                                                        <HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--8--%>
                                                    <asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="ReleaseNote No.">
                                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <%--9--%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                                CommandName="ViewRec" Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO"
                                                                Visible='<%#  Eval("IsAttachmentAdded")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%--10--%>
                                                    <asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                        HeaderText="IsAttachmentAdded" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                    <%--11--%>
                                                    <asp:BoundField DataField="ReceiptItemID" HeaderText="ReceiptItemID" HeaderStyle-CssClass="hideGridColumn"
                                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td align="right">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table class="clstableButton" align="right">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton" Text="Save" ToolTip="Click to Save W.O Pending Issued Spares List"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton" Text="Close" ToolTip="Click to Close to Go back to the Previous Screen">
                                                    </asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForIssuedSpares();
                return false;
            }
        </script>
        <%--UPDATEPANEL --%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
                SetPageLayout();
                //                if ($.browser.msie) {
                parent.IFrameIssuedSparesStateComplete();
                //                }


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
        <!--ReceiptCumInvoiceAttach Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyAttach" Text="Attach" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlAttach" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeAttach" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlAttach" runat="server" TargetControlID="btnDummyAttach"
            PopupControlID="pnlAttach" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameAttachStateComplete() {
                $("#btnDummyAttach").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenAttachWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeAttach").attr("src", "wfAttachmentList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyAttach").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForAttach() {
                var Attachwindow = $find("<%=mdlAttach.ClientID %>");
                //close popup window
                Attachwindow.hide();
                //release resources
                $("#IframeAttach").attr("src", "JavaScript:''");
                //call button click
                //  $("#hdnBtnAttach").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
