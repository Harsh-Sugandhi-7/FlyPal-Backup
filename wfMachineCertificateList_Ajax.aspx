<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMachineCertificateList_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfMachineCertificateList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aircraft Certificate List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');

        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout"
    class="formBGColor">
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidation" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" />
                                            <asp:CustomValidator ID="cvDate" runat="server" Display="None" ControlToValidate="calExpiryDate"
                                                ValidationGroup="a" ErrorMessage="Expiry Date must be greater than Issue Date"
                                                CssClass="clsLabelAuto"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required."
                                                Display="None" ControlToValidate="txtName" ValidationGroup="a"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:UpdatePanel ID="upnlAircraftCertificateDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <fieldset id="fdsAircraftCertificateDetails" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="lblAircraftCertificateDetails" runat="server" style="font-weight: bold">
                                                    <b>Aircraft Certificate Details [NEW]</b></legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span id="lblStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblName" width="470px" class="clsLabelAuto">Name</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Certificate Name"
                                                                Width="370px" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblNo" class="clsLabelAuto">No.</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"
                                                                ToolTip="Enter Certificate Number"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:CheckBox ID="chkOneTimeCertificate" runat="server" CssClass="clsLabelAuto" Text="One Time Certificate (Then no need of Expiry Date else Expiry Date is compulsory)"
                                                                ToolTip="Check if certificate is one time " />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkApplicable" runat="server" CssClass="clsLabelauto" Text="Applicable"
                                                                ToolTip="Check to apply Applicability"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblIssueDateStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblIssueDate" class="clsLabel">Issue Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="calIssueDate" runat="server" AutoPostBack="true" CssClass="clsTextBox_Ajax"
                                                                onchange="ValidateDateText(this,'IssueDate_watermarkextender','true');" Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calIssueDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                OnClientShown="onClientShown" OnClientHidden="onClientHide" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="calIssueDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="IssueDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                TargetControlID="calIssueDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <span id="lblExpiryDateStar" class="clsLabelStar">*</span>
                                                        </td>
                                                        <td>
                                                            <span id="lblExpiryDate" class="clsLabelAuto">Expiry Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="calExpiryDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'ExpiryDate_watermarkextender','true');"
                                                                Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                OnClientShown="onClientShown" OnClientHidden="onClientHide" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="calExpiryDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="ExpiryDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                TargetControlID="calExpiryDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <%-- <span id="Label4" class="clsLabelStar" style="color: Red;">*</span>--%>
                                                        </td>
                                                        <td>
                                                            <span id="lblWarningDays" class="clsLabelAuto">Warning Days</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtWarningDays" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                ToolTip="Enter Warning Days" MaxLength="4">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <span id="lblEffectiveDate" class="clsLabelAuto">Effective Date</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="calEffectiveDate" runat="server" CssClass="clsTextBox_Ajax" onchange="ValidateDateText(this,'EffectiveDate_watermarkextender','true');"
                                                                Width="100px"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="calEffectiveDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                OnClientShown="onClientShown" OnClientHidden="onClientHide" Enabled="true" Format="<%$AppSettings:DateFormat%>"
                                                                TargetControlID="calEffectiveDate">
                                                            </cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender ID="EffectiveDate_watermarkextender" runat="server"
                                                                ClientIDMode="Static" TargetControlID="calEffectiveDate" WatermarkText="<%$AppSettings:DateFormat%>">
                                                            </cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLine1_Ajax" ToolTip="Enter Remark"
                                                                MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                    class="clsButton_Ajax" />
                                                                            </td>
                                                                            <td style="padding-left: 3px;">
                                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                            </td>
                                                                            <td style="padding-left: 2px;">
                                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                    Height="20px" Width="24px"></asp:ImageButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnAdd" TabIndex="0" runat="server" OnClientClick="return CheckValidation();"
                                                CssClass="clsButton_Ajax" ToolTip="Click to Add the Certificate" ValidationGroup="a"
                                                Text="Add"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="1">
                                    <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Aircraft Certificate Details</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgCertificateList" runat="server" ClientIDMode="Static" PageSize="25"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" EnableViewState="False"
                                                            CssClass="clsGrid" AllowPaging="False" AllowSorting="True">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
                                                                <asp:BoundField DataField="SerialNo" HeaderText="Sr. No." Visible="False" />
                                                                <asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Name">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="No.">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EffectiveDateFormatted" HeaderText="Effective Date">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WarningDays" HeaderText="Warning Days">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="One Time Certificate">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsOneTimeCertificate" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "OneTimeCertificate") %>'
                                                                            Enabled="False" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Applicability">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsApplicable") %>'
                                                                            Enabled="False" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:ButtonField CommandName="EditRec" HeaderText="Edit/View" Text="Edit/View">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="DeleteRec" HeaderText="Delete" Text="Delete">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField Text="View" HeaderText="View" CommandName="ViewRec">
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
                                    <asp:UpdatePanel ID="upnlBack" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                TabIndex="0" Text="Back" ToolTip="Click to go Previous page" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
        PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameFileUploadStateComplete() {
            $("#btnDummyFileUpload").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        $(document).ready(function () {
            $("#btnSelectFile").live("click", function () {
                try {
                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                    if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForFileUpload(fileattached) {
            var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
            //close File Upload popup window
            FileUpwindow.hide();
            //Free resources
            $("#IFileUpload").attr("src", "JavaScript:''");
            if (fileattached) {
                //call hidden button to set file upload content to object
                $("#hdnBtnFileUpload").click();
            }
        }
    </script>
    <!-- End File Upload Modal Dialog-->
    <%--Date Validations--%>
    <script type="text/javascript">

        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
    </form>
    <script language="JavaScript" type="text/javascript">
        function CallParentFunction() {

            window.parent.autoResizeCertificateList();
        }
        function CallCloseChildPage() {

            window.parent.CloseChildPage();
        }
        function CheckValidation() {
            if (!Page_ClientValidate()) {
                // Call Your custom JS function and return value.
                CallParentFunction();
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function onClientShown(sender, e) {
            window.parent.autoResizeCerti();
        }
        function onClientHide(sender, e) {
            window.parent.autoResizeCertificateList();
        }
    </script>
</body>
</html>
