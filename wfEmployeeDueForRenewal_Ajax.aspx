<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEmployeeDueForRenewal_Ajax.aspx.vb"
    Inherits="Flypal.wfEmployeeDueForRenewal_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Employee Document/Training Renewal</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:placeholder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:placeholder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td colspan="5">
                                <span id="lbltitle" class="clstitle1">Employee Document/Training Renewal</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table width="100%">
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table id="Table2" border="0" cellspacing="1" cellpadding="1">
                                                <tr>
                                                    <td>
                                                        <span id="lblEmployee" class="clsLabelAuto">Employee</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbEmployeeList" runat="server" CssClass="clsComboBox2_Ajax"
                                                            DataTextField="EmpNoName" DataValueField="ID" EnableViewState="false" onChange="setEmployeeValue()">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkUsedInFlightLog" runat="server" CssClass="clsCheckBox" Text="Used In Flight Log">
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkExpiredEntries" runat="server" CssClass="clsCheckBox" Text="Expired Entries Only">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right" colspan="2">
                                            <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table3" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                                    ToolTip="Click to find Records as per searching criteria"></asp:Button>
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
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlEmpMaster" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <tr>
                                            <td colspan="3" align="left">
                                                <span id="lblAdd" class="clsLabelAuto">Click To Add New or Edit existing Record</span>
                                            </td>
                                            <td colspan="2" align="right">
                                                <table id="Table7">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnEmployeeMaster" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                                Text="Employee Master" ToolTip="Click to add or Edit existing Employee" CausesValidation="False">
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlDocumentGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">Note : Following list shows records whose expiry date falls in next 2 months or below.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="5">
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">The following documents are due for renewal</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="5">
                                                    <asp:GridView ID="dgDocumentList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true" EnableViewState="false"
                                                        AllowPaging="true" PageSize="10">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesignationName" HeaderText="Designation">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DocumentName" HeaderText="Document">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DocNo" HeaderText="Doc No">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateOfIssue" HeaderText="Date of Issue">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PlaceOfIssue" HeaderText="Place of Issue">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Validity" HeaderText="Validity">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                 <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DocumentValidityInName" HeaderText="Validity In">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateOfExpiry" HeaderText="Expiry Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IssuingAuthority" HeaderText="Issuing Authority">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="WarningDays" HeaderText="Remaining Days">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew">
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
                            <td colspan="5">
                                <asp:UpdatePanel ID="upnlTrainingGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" colspan="5">
                                                    <asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader">The following Trainings are due for renewal</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="5">
                                                    <asp:GridView ID="dgTrainingList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        DataKeyNames="ID,EmployeeID" ShowHeaderWhenEmpty="true" EnableViewState="false"
                                                        ClientIDMode="Static" AllowPaging="true" PageSize="10">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="EmployeeID" HeaderText="EmployeeID"></asp:BoundField>
                                                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DesignationName" HeaderText="Designation">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TrainingName" HeaderText="Training">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CertificateNo" HeaderText="Certificate No">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Date" HeaderText="Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Duration" HeaderText="Duration">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TrainingOrgName" HeaderText="Training Orgnisation">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MonthOfTrainingName" HeaderText="Month of Training">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="YearOfTraining" HeaderText="Year of Training">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date">
                                                                <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <FooterStyle Wrap="False"></FooterStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
                                                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:ButtonField Text="Renew" HeaderText="Renew" CommandName="Renew">
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
                            <td valign="top" align="right" colspan="5">
                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" Text="Close" CausesValidation="False">
                                        </asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy hidden btn panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnEmpDocument" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnEmpTraining" ClientIDMode="Static" runat="server" Text="Add"
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
    <!-- Employee Document Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpDocument" Text="Employee Document" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpDocument" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpDocument" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpDocument" runat="server" TargetControlID="btnDummyEmpDocument"
        PopupControlID="pnlEmpDocument" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpDocumentStateComplete() {
            $("#btnDummyEmpDocument").click();
            //            'var EmpDocumentwindow = $find("<%=mdlPopupEmpDocument.ClientID %>");
            //            'EmpDocumentwindow.show();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpDocumentWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpDocument").attr("src", "wfEmployeeDocument_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpDocument").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpDocument() {
            var EmpDocumentwindow = $find("<%=mdlPopupEmpDocument.ClientID %>");
            //close popup window
            EmpDocumentwindow.hide();
            //           release resources
            $("#IframeEmpDocument").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpDocument").click();
        }
    </script>
    <!-- End-->
    <!-- Employee Training Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyEmpTraining" Text="Employee Training" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlEmpTraining" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeEmpTraining" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupEmpTraining" runat="server" TargetControlID="btnDummyEmpTraining"
        PopupControlID="pnlEmpTraining" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameEmpTrainingStateComplete() {
            $("#btnDummyEmpTraining").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenEmpTrainingWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeEmpTraining").attr("src", "wfEmployeeTraining_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyEmpTraining").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForEmpTraining() {
            var EmpTrainingwindow = $find("<%=mdlPopupEmpTraining.ClientID %>");
            //close Training popup window
            EmpTrainingwindow.hide();
            //           release resources
            $("#IframeEmpTraining").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnEmpTraining").click();
        }
    </script>
    <!-- End-->
    <!-- hidden fields to set combobox selected value at client side -->
    <asp:HiddenField ID="EmployeeIDValue" runat="server" ClientIDMode="Static" />
    <!-- End-->
    <!-- javascript function to set combobox selected value to appropriate hidden field for Part Information-->
    <script type="text/javascript">
        function setEmployeeValue(elem, combo) {
            var id = $get("cmbEmployeeList").value;
            $("#EmployeeIDValue").val(id);
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
