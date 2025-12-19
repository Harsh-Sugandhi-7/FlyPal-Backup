<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemovedCompListForWO_Ajax.aspx.vb"
    Inherits="Flypal.wfRemovedCompListForWO_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Removed Component List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
        .aspNetDisabled
        {
            color: Black !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
                    <table id="tblInner">
                        <tr>
                            <td>
                                <span id="lbltitle" class="clstitle1">Component Installation</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="Legend1" runat="server"><b>Component Search Information</b></legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblPart" class="clsLabelAuto">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="50"
                                                            AutoPostBack="true" ToolTip="Enter Part No." AutoCompleteType="DisplayName"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="50"
                                                            AutoPostBack="true" ToolTip="Enter Serial Number"></asp:TextBox>
                                                    </td>
                                                </tr>
                                        </fieldset>
                                        </table> </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRemovalGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRemovedComponents" runat="server" CssClass="clsLabelHeader">List of Removed Components as of [Date] : Record(s)</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkRemCompLoadMoreTop" runat="server" CssClass="clsLinkButton"
                                                                    Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgRemovedList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        DataKeyNames="ID" ShowHeaderWhenEmpty="true" CssClass="clsGrid" PageSize="25"
                                                        OnRowDataBound="dgRemovedList_RowDataBound">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                        <RowStyle CssClass="clsdgItem" />
                                                        <HeaderStyle CssClass="clsdgHeader" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID " Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Wrap="false" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                                <ItemStyle Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode" HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompInfo" HeaderText="Component Info." SortExpression="CompInfo"
                                                                HtmlEncode="false">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Width="130px" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemTSOValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemTSOValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:ButtonField CommandName="InstallSelected" HeaderText="Install Selected" Text="Install Selected">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:ButtonField>
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
                                        <table id="Table2" border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" TabIndex="0" Text="Close"
                                                        ToolTip="Click to close List of Removed Components screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup for city-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnInstallationHistory" ClientIDMode="Static" runat="server" Text="..."
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
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
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                cache: false,
                async: false,
                data: params,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });
            return false;
            function onSuccess(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val(result);
                $find(extenderid).set_Text(result);
            }

            function onError(result) {
                $(elem).removeClass('ac_loading');
                $(elem).val('');
                $find(extenderid).set_Text('');
            }
            function OnBeforeSend() {
                $(elem).addClass('ac_loading');
            }
        }
    </script>
    <!-- Installation History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyInstallationHistory" Text="Installation History"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlInstallationHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeInstallationHistory" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupInstallationHistory" runat="server" TargetControlID="btnDummyInstallationHistory"
        PopupControlID="pnlInstallationHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameInstallationHistoryStateComplete() {
            $("#btnDummyInstallationHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenInstallationHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeInstallationHistory").attr("src", "wfUpdateInstalledCompHistory_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyInstallationHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForInstallationHistory() {
            var InstallationHistorywindow = $find("<%=mdlPopupInstallationHistory.ClientID %>");
            //close Installation History popup window
            InstallationHistorywindow.hide();
            //           release resources
            $("#IframeInstallationHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnInstallationHistory").click();
        }
    </script>
    <!-- End-->
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForInstallSelected();
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
                    parent.IFrameInstallSelectedStateComplete();
//                }
       
      
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
                var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
                var windowheight=$(window).height();
                if (tempMargtop>=windowheight)
                {
                $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
                }
                else
                {
                var margintop=(windowheight/2)-(tempMargtop/2);
                $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
                }
       
            }
    </script>
    </form>
</body>
</html>
