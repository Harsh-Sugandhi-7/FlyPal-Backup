<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftCertificationDocument_AJAX.aspx.vb"
    Inherits="Flypal.wfrptAircraftCertificationDocument_AJAX" %>

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

            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td class="clsFormHeader1">

                        <asp:Label CssClass="clsFormHeader" ID="lblTitle" runat="server">List of Aircraft Certificates</asp:Label>

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
                        <asp:Panel CssClass="clspnl1" ID="pnlMain" runat="server">
                            <table id="tblinner" width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label CssClass="clsLabelHeader" ID="lblSearchCriteria" runat="server">Search Criteria</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label CssClass="clsLabelStar" ID="lblAircraftStar1" runat="server" Visible="false">*</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label CssClass="clsLabelAuto" ID="lblAircraft" runat="server">Aircraft </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraftList" runat="server"
                                                                    DataValueField="ID" DataTextField="RegNo">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox CssClass="clsLabelAuto" ID="chkWithoutExpiryDdate" runat="server" ToolTip='Check to see Without Expiry Date records'
                                                                    Text='With Expiry Date only'></asp:CheckBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label CssClass="clsLabelAuto" ID="lblReadOnly" runat="server" ForeColor="Red"
                                                                    Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <span class="clsLabelAuto" id="Label2">Name </span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCertificateName" runat="server" AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelAuto" ToolTip='Check to see only "NOT APPLICABLE"  certificates'
                                                                    AutoPostBack="true" Text='Show ONLY "NOT  APPLICABLE" certificates'></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label CssClass="clsLabelAuto" ID="lblStepFormat" runat="server">Format</asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                                                    <asp:ListItem Value="0">Format 1 </asp:ListItem>
                                                                    <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right" valign="top">


                                            <asp:UpdatePanel runat="server" ID="upnlFindnow" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <%--<asp:Button ID="btnFindNow" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Find the list of aircraft certificate Document as per searching criteria"
                                                Text="Find Now" CausesValidation="False"></asp:Button>--%>
                                                    <asp:ImageButton CssClass="clsSearch2btn" ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" ToolTip="Click to Find the list of aircraft certificate Document as per searching criteria." />
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
                                <asp:Label CssClass="clsLabelHeader" ID="lblResult" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="4">
                        <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal" ID="dgCertificateList" runat="server" ToolTip="Aircraft Certificate List."
                                    DataKeyNames="ID" AllowSorting="True" PageSize="3" AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="True" OnRowDataBound="OnRowDataBound">
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                    <RowStyle CssClass="clsdgItem" />
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                    <PagerSettings FirstPageText="First" LastPageText="Last" />
                                    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
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
                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:BoundField>

                                        <%--8--%>
                                        <asp:TemplateField HeaderText="One Time Certificate">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsOneTimeCertificate" runat="server" CausesValidation="false"
                                                    Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "OneTimeCertificate") %>'></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--9--%>
                                        <asp:BoundField DataField="WarningDays" SortExpression="WarningDays" HeaderText="Warning Days">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <%--10--%>
                                        <asp:BoundField DataField="ElapsedDays" SortExpression="ElapsedDays" HeaderText="Elapsed Days">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <%--11--%>
                                        <asp:BoundField DataField="RemDays" SortExpression="RemDays" HeaderText="Remaining Days">
                                            <HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <%--12--%>
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                        </asp:BoundField>
                                        <%--13--%>
                                        <%-- <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="RenewRec">
                                        <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                    </asp:ButtonField>
                                  
                                    <asp:ButtonField Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
                                 
                                    <asp:ButtonField Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
                               
                                    <asp:ButtonField CommandName="ViewRec" HeaderText="View" Text="View">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>
                                   
                                    <asp:ButtonField CommandName="HistoryRec" HeaderText="History" Text="History">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:ButtonField>--%>
                                        <%--18--%>
                                        <asp:BoundField DataField="ImageSize" HeaderText="Size" HeaderStyle-CssClass="hideGridColumn"
                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                        <%--19--%>
                                        <asp:BoundField DataField="IsMaster" HeaderText="IsMaster" HeaderStyle-CssClass="hideGridColumn"
                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table id="Table1">
                                    <tr>
                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnPrint" TabIndex="0" runat="server" Text="Print"
                                                ToolTip="Click to Print the list of Certificates" CausesValidation="False"></asp:Button>
                                        </td>

                                        <td>
                                            <asp:Button CssClass="clsbtnH" ID="btnBack" TabIndex="0" runat="server" Text="Close"
                                                ToolTip=" Click to close Certificate List screen" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
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
