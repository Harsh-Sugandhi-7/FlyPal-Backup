<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModelMonitorServiceList_Ajax.aspx.vb"
    Inherits="Flypal.wfModelMonitorServiceList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Model Service List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .maxGridWidth {
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
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Model Service List</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" CssClass="clsButton_Ajax" ToolTip='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Click to add new MPD", "Click to add new Model Service") %>'
                                                                                CausesValidation="False" Text="Add New"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" ToolTip='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Click to print MPD List", "Click to print Model Service List") %>'
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
                                                    <td colspan="2">
                                                        <asp:GridView ID="dgMonitorServiceList" runat="server" CssClass="clsGrid" AllowSorting="True"
                                                            DataKeyNames="ID" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" ToolTip="Model Service List">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                            <RowStyle CssClass="clsdgItem"></RowStyle>
                                                            <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                <asp:BoundField DataField="CodeTaskNo" SortExpression="CodeTaskNo" HeaderText="Code/Form No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField> 
                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference Doc.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="TypeCode" SortExpression="TypeCode" HeaderText="Type">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                </asp:BoundField>
                                                               

                                                                <asp:TemplateField HeaderText="Show In C of A" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkCOfA" runat="server" Enabled="False" Checked='<%# DataBinder.Eval(Container.DataItem, "ShowInCofA") %>'></asp:CheckBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="RequiredManHours" HeaderText="Estd. Man Hours" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Note" HeaderText="Note">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="true" CssClass="TextBreak maxGridWidth" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="FrequencyValue" HeaderText="Threshold" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Edit" HeaderText="Edit" CommandName="EditRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="View" HeaderText="View" CommandName="View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                    DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"></asp:BoundField>
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
                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Click to add new MPD", "Click to add new Model Service") %>'
                                                            CausesValidation="False" Text="Add New"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" ToolTip='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Click to print MPD List", "Click to print Model Service List") %>'
                                                            CausesValidation="False" Text="Print"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go back to previous page"
                                                            CausesValidation="False" Text="Back"></asp:Button>
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
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnModelServiceMaster" ClientIDMode="Static" runat="server" Text="----"
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
        <!--Model Service Master Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyModelServiceMaster" Text="Model Service Master"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlModelServiceMaster" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeModelServiceMaster" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupModelServiceMaster" runat="server" TargetControlID="btnDummyModelServiceMaster"
            PopupControlID="pnlModelServiceMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameModelServiceMasterStateComplete() {
                $("#btnDummyModelServiceMaster").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenModelServiceMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeModelServiceMaster").attr("src", "wfModelMonitorService_Ajax.aspx?Type=pup&GChildPage4=wfInstallAssembly_AJAX.aspx");

                    if (!$.browser.msie) {
                        $("#btnDummyModelServiceMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForModelServiceMaster() {
                var ModelServiceMasterwindow = $find("<%=mdlPopupModelServiceMaster.ClientID %>");
                //close Model Service Master popup window
                ModelServiceMasterwindow.hide();
                //           release resources
                $("#IframeModelServiceMaster").attr("src", "JavaScript:''");
                //call Model Service Master image button
                $("#hdnBtnModelServiceMaster").click();
            }
        </script>
        <!-- End-->
        <%--call parent function after completing subroutine..(when page open as popup)--%>
        <script type="text/javascript">
            function CallParentCallback() {
                parent.ParentCallBackFunctionForModelMonitorServiceList();
                return false;
            }
        </script>
        <!--Set page layout when open as popup aspx page-->
        <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

            $(document).ready(function () {
                SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameModelMonitorServiceListStateComplete();
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
                var tempMargtop = $("body #tblMain:eq(0),html #tblMain:eq(0)").outerHeight();
                var windowheight = $(window).height();
                if (tempMargtop >= windowheight) {
                    $("body #tblMain:eq(0),html #tblMain:eq(0)").css({ 'margin': 'auto' });
                }
                else {
                    var margintop = (windowheight / 2) - (tempMargtop / 2);
                    $("body #tblMain:eq(0),html #tblMain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                }

            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
