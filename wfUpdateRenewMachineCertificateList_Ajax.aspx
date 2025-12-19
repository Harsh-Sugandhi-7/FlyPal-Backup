<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateRenewMachineCertificateList_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateRenewMachineCertificateList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Renewal Certificate List</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFilel() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td colspan="4" class="clsFormHeader1Newstyle">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"> History for Certificates</asp:Label>
                            </td>

                            <td align="right" colspan="4">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Certificates"
                                                        Text="Save"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close Certificate List screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
                <td colspan="4">
                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            <asp:CustomValidator ID="cvDate" runat="server" ControlToValidate="txtAircraft" Display="None"
                                CssClass="clsLabelAuto"></asp:CustomValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblSearchCriteria" runat="server" CssClass="clsLabelHeader">Search Criteria</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurrencyStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                    </td>
                                    <td>
                                        <%-- <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsComboBox_Ajax"
                                            DataTextField="RegNo" DataValueField="MachineID" Enabled="False">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtAircraft" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="4">
                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="dgCertificateList" ToolTip="Aircraft Certificate List." runat="server"
                                CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"  DataKeyNames="ID" ShowHeaderWhenEmpty="True" 
                                AllowSorting="True" AutoGenerateColumns="False" PageSize="5">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                <Columns>
                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                    <asp:BoundField Visible="False" DataField="SerialNo" SortExpression="SerialNo" HeaderText="Sr. No.">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Name">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="No.">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="One Time Certificate">
                                        <HeaderStyle  HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsOneTimeCertificate" runat="server" CausesValidation="false" Enabled="false"
                                                Checked='<%# DataBinder.Eval(Container.DataItem, "OneTimeCertificate") %>' >
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Applicability">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsApplicable" Enabled="false" runat="server" CausesValidation="false" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                ></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EffectiveDateFormatted" HeaderText="Effective Date">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                       <asp:BoundField DataField="WarningDays" HeaderText="Warning Days">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ElapsedDays" SortExpression="ElapsedDays" HeaderText="Elapsed Days">
                                        <HeaderStyle  HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RemDays" SortExpression="RemDays" HeaderText="Remaining Days">
                                        <HeaderStyle  HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" ToolTip="Enter Remark"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>' TextMode="MultiLine"
                                                MaxLength="500"></asp:TextBox>
                                            <asp:CustomValidator ID="cvRemark" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate1"
                                                ControlToValidate="txtRemark" Display="None"></asp:CustomValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left"  />
                                    </asp:ButtonField>--%>

                                    <asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ViewRec" 
                                                ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("ImageSize")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ImageSize" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <%--<tr>
                <td align="right" colspan="4">
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Save Certificates"
                                            Text="Save"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" ToolTip=" Click to close Certificate List screen"
                                            Text="Close" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
        </table>
    </div>
    <div>
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
    </div>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForRenewHistory();
            return false;
        }
    </script>
    <%--End--%>
    <div>
        <%--Set page layout when open as popup aspx page--%>
        <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
            $(document).ready(function () {
            SetPageLayout();
                if ($.browser.msie) {
                    parent.IFrameRenewHistoryStateComplete();
                }
       
      
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
    </div>
    <%--End--%>
    </form>
</body>
</html>
