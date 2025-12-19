<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineCertificateRenewList_AJAX.aspx.vb"
    Inherits="Flypal.wfMachineCertificateRenewList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Aircraft Renewal Certificate List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
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
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td class="clsFormHeader1Newstyle">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">List of Aircraft Certificates</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>

                            <td colspan="4" align="right">
                                <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                                        ToolTip="Click to Print the list of Certificates" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                        ToolTip="Click to report by mail" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                        ToolTip=" Click to close Certificate List screen" CausesValidation="False"></asp:Button>
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
                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            <asp:CustomValidator ID="cvMachine" runat="server" Display="None" ControlToValidate="cmbAircraftList"
                                ValidationGroup="a" OnServerValidate="CustomValidate" CssClass="clsValidationSummary"></asp:CustomValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table id="tblinner">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSearchCriteria" runat="server" CssClass="clsLabelHeader">Search Criteria</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cmbAircraftList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                                DataValueField="ID" AutoPostBack="true" DataTextField="RegNo">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="height: 28px">
                                                            <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  certificates'
                                                                AutoPostBack="true" Text='Show ONLY "NOT  APPLICABLE" certificates'></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 4px" colspan="4">
                                                            <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="Label2" class="clsLabelAuto">Name </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCertificateName" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
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
                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="dgCertificateList" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Aircraft Certificate List."
                                DataKeyNames="ID" AllowSorting="True" PageSize="3" AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="True" OnRowDataBound="dgCertificateList_RowDataBound">
                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                <Columns>
                                    <%--0--%>
                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                    <%--1--%>
                                    <asp:BoundField Visible="False" DataField="SerialNo" SortExpression="SerialNo" HeaderText="Sr. No.">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--2--%>
                                    <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--3--%>
                                    <asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Name">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--4--%>
                                    <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="No.">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--5--%>
                                    <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--6--%>
                                    <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>
                                    <%--7--%>
                                    <asp:BoundField DataField="EffectiveDateFormatted" HeaderText="Effective Date">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:BoundField>

                                    <%--8--%>
                                    <asp:TemplateField HeaderText="One Time Certificate">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsOneTimeCertificate" runat="server" CausesValidation="false"
                                                Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "OneTimeCertificate") %>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--9--%>
                                    <asp:BoundField DataField="WarningDays" HeaderText="Warning Days">
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <%--10--%>
                                    <asp:BoundField DataField="ElapsedDays" SortExpression="ElapsedDays" HeaderText="Elapsed Days">
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <%--11--%>
                                    <asp:BoundField DataField="RemDays" SortExpression="RemDays" HeaderText="Remaining Days">
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <%--12--%>
                                    <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <%--13--%>
                                    <%--<asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="RenewRec"
                                        >
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:ButtonField>
                                    <%--14
                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                    <%--15
                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"
                                        ></asp:ButtonField>
                                    <%--16
                                    <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                    <%--17
                                    <asp:ButtonField CommandName="HistoryRec" HeaderText="History" Text="History">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                    <%--18--%>
                                    <asp:BoundField DataField="ImageSize" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                    <%--19--%>
                                    <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn"></asp:BoundField>

                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%-- <span id="button">Login</span>--%>
                                            <div class="dropdown" style="margin-right:30px">
                                                <div class="dropdownbtn-content" >
                                                    <table id="T1" class="clsGridNew_Ajax">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="EditView" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                    CommandName="EditRec" ImageUrl="~/images/edit.png" Style="height: 15px; width: 15px" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                                                    CommandName="DeleteRec" ImageUrl="~/images/delete.png" Style="height: 20px; width: 20px" 
                                                                    Visible=<%# Not chkApplicable.Checked %> />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="View" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="ViewRec" 
                                                                    ImageUrl="icons/CLIP01.ICO" Style="height: 20px; width: 13px" Visible='<%#  Eval("ImageSize")%>' />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="IDRenew" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                    CommandName="RenewRec" Style="width: 20px" ImageUrl="images/Renew1.png" 
                                                                    Visible=<%# Not chkApplicable.Checked %>/>
                                                            </td>

                                                            <td>
                                                                <asp:ImageButton ID="IDHistory" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                    CommandName="HistoryRec" ImageUrl="~/images/History.png"  Visible='<%#IIf(Eval ("IsMaster") = "True", False, True) %>' />
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Image ID="lnkArrow" runat="server" CssClass="clsActionbtn" ImageUrl="~/images/Arrowup.png" Style="cursor: pointer" />
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <%--<td colspan="4" align="right">
                    <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="Table1">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                            ToolTip="Click to Print the list of Certificates" CausesValidation="False"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                            ToolTip="Click to report by mail" Width="96px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                            ToolTip=" Click to close Certificate List screen" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <!--Dummy panel to open modelpopup-->
                <td style="height: 0px;">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                        <ContentTemplate>
                            <asp:Button ID="hdnBtnMachineCertificate" ClientIDMode="Static" runat="server" Text="Add"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                            <asp:Button ID="hdnBtnRenewHistory" ClientIDMode="Static" runat="server" Text="Add"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                            <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <!--End -->
            </tr>
        </table>
    </div>
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
    <!-- Machine Certificate Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyMachineCertificate" Text="TaskCard Spare"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlMachineCertificate" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeMachineCertificate" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupMachineCertificate" runat="server" TargetControlID="btnDummyMachineCertificate"
        PopupControlID="pnlMachineCertificate" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMachineCertificateStateComplete() {
            $("#btnDummyMachineCertificate").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenMachineCertificateWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeMachineCertificate").attr("src", "wfMachineCertificateRenew_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyMachineCertificate").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForMachineCertificate() {
            var MachineCertificatewindow = $find("<%=mdlPopupMachineCertificate.ClientID %>");
            //close Task Card Spare popup window
            MachineCertificatewindow.hide();
            //           release resources
            $("#IframeMachineCertificate").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnMachineCertificate").click();
        }
    </script>
    <!-- End-->
    <!-- History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRenewHistory" Text="Log Fuel Oil" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRenewHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRenewHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRenewHistory" runat="server" TargetControlID="btnDummyRenewHistory"
        PopupControlID="pnlRenewHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRenewHistoryStateComplete() {
            $("#btnDummyRenewHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRenewHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRenewHistory").attr("src", "wfUpdateRenewMachineCertificateList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRenewHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRenewHistory() {
            var RenewHistorywindow = $find("<%=mdlPopupRenewHistory.ClientID %>");
            //close Renew History popup window
            RenewHistorywindow.hide();
            //           release resources
            $("#IframeRenewHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRenewHistory").click();
        }
    </script>
    <!-- End-->
    <!-- Popup For By Mail -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
        PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyForByMail").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
            //close popup window
            ForByMailwindow.hide();
            //           release resources
            $("#IframeForByMail").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>
