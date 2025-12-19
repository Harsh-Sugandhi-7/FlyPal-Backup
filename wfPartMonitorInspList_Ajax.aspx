<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartMonitorInspList_Ajax.aspx.vb"
    Inherits="Flypal.wfPartMonitorInspList_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part Inspection List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .maxGridWidth
        {
            max-width: 350px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:Label ID="lblList" runat="server" CssClass="clstitle1">Part Inspection List</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to add new Part Inspection"
                                                        CausesValidation="False" Text="Add New"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to print List of Part Inspection"
                                                        CausesValidation="False" Text="Print"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBackTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                        CausesValidation="False" Text="Back"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:UpdatePanel ID="upnldgGrid" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="lbldgGridResult" CssClass="clsLabelHeader"></asp:Label>
                                                    <asp:GridView ID="dgPartMonitorInsp" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" DataKeyNames="ID" PageSize="5" ShowHeaderWhenEmpty="True">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/ Form No.">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" SortExpression="ATACode" HeaderText="ATA">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Show In C of A" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'
                                                                        CssClass="clsCheckBox"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours"></asp:BoundField>
                                                            <asp:BoundField DataField="Note" HeaderText="Note">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FrequencyValue" HeaderText="Frequency" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
                                                            <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec"></asp:ButtonField>
                                                            <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                                                            <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Add New Part Insp"
                                                                    CausesValidation="False" Text="Add New"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Print the list of Part Insps."
                                                                    CausesValidation="False" Text="Print "></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Back to Previous Page"
                                                                    CausesValidation="False" Text="Back"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="hdnBtnInspMaster" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                        Style="display: none;" Text="Add" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
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
    <div>
        <!-- InspMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspMaster" Text="Dummy InspMaster" ClientIDMode="Static">
            </asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlInspMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspMaster" runat="server" TargetControlID="btnDummyInspMaster"
            PopupControlID="pnlInspMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspMasterStateComplete() {
                $("#btnDummyInspMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspMasterWindow(GChildPage2, GChildPage4, GChildPage5, GChildPage6) {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    //$("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage4=wfInstallComp_AJAX.aspx");
                    //$("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage2=wfInstallAssembly_Ajax.aspx&GChildPage4=" + GChildPageTmp);
                    $("#IframeInspMaster").attr("src", "wfPartMonitorInsp_AJAX.aspx?Type=pup&GChildPage2=" + GChildPage2 + "&GChildPage4=" + GChildPage4 + "&GChildPage5=" + GChildPage5 + "&GChildPage6=" + GChildPage6);
                    // $("#IframeInspMaster").load(function () {
                    //                    var doc = IframeInspMaster.window;
                    //                    IframeInspMaster.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyInspMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspMaster() {
                var InspMasterwindow = $find("<%=mdlPopupInspMaster.ClientID %>");
                //close InspMaster popup window
                InspMasterwindow.hide();
                //           release resources
                $("#IframeInspMaster").attr("src", "JavaScript:''");
                //call InspMaster image button
                $("#hdnBtnInspMaster").click();
            }
        </script>
        <!-- End-->
    </div>
    </form>
</body>
</html>
