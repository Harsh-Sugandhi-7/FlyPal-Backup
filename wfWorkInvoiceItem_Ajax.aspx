<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWorkInvoiceItem_Ajax.aspx.vb"
    Inherits="Flypal.wfWorkInvoiceItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Invoice Item Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
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
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <table class="clstablelistout" id="tblMain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                    <table class="clsTablelistin" id="tblinner" border="0">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Work Invoice Item [New]</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="Description Required"
                                            ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvUnit" runat="server" ErrorMessage="Unit Required"
                                            ControlToValidate="cmbUnit" CssClass="clsLabelAuto" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvAMEQty" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtAMEQty" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDescription" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtDescription" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtRemark" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvNote" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtNote" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvTechnicianQty" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtTechQty" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvHelperQty" runat="server" ErrorMessage="Rate must be greater than Zero."
                                            ControlToValidate="txtHelperQty" CssClass="clsLabelAuto" Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblQuotationInfo" class="clsLabelHeader">Enter Work Invoice Item </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblNote1" class="clsLabelAuto">Enter the Details of Items by entering the
                                    Description,Unit and mention the Qty and the Rate</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblSrNo" class="clsLabel">Sr. No.</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxSmall_Ajax" ReadOnly="True"
                                    BackColor="#E0E0E0" MaxLength="5" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.SrNo %>">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDescriptionStar1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblDesc" class="clsLabel">Description</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBox1_Ajax" BackColor="White"
                                    MaxLength="2000" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.TaskDescription %>"
                                    Height="39px" Width="382px" ToolTip="Enter Description" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label4" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblUnit" class="clsLabel">Unit</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbUnit" runat="server" CssClass="clsComboBox_Ajax" SelectedValue="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.UnitID %>">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblValues" class="clsLabelHeader">Values For</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblAME" class="clsLabelHeader">I.AME</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label1" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblQuantity" class="clsLabel">AME</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAMEQty" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="4" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.AMEQty %>"
                                    ToolTip="Enter No. Of AME">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblRate" class="clsLabel">Per Rate</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAMERate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="12" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.AMECRate %>"
                                    ToolTip="Enter AME Rate">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="Label5" class="clsLabelHeader">II.Technician</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label6" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblTechQty" class="clsLabel">Technician</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTechQty" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="4" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.TechQty %>"
                                    ToolTip="Enter No. Of Technician">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblTechRate" class="clsLabel">Per Rate</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTechRate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="12" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.TechCRate %>"
                                    ToolTip="Enter Technician Rate">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="lblHelper" class="clsLabelHeader">III.Helper</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label8" class="clsLabelStar">*</span>
                            </td>
                            <td>
                                <span id="lblHelperQty" class="clsLabel">Helper</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHelperQty" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="4" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.HelperQty %>"
                                    ToolTip="Enter No. Of Helper">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblHelperRate" class="clsLabel">Per Rate</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHelperRate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    MaxLength="12" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.HelperCRate %>"
                                    ToolTip="Enter Helper Rate">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span id="Label2" class="clsLabelHeader">Total Rate</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblTotalRate" class="clsLabel">Total Rate</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalRate" runat="server" CssClass="clsTextBoxRightAlign1_Ajax"
                                    ReadOnly="True" BackColor="Silver" MaxLength="12" Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.CAmount %>"
                                    ToolTip="Total Rate">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="3">
                                <span id="lblRN" class="clsLabelHeader">Remark/Note</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblRemark" class="clsLabel">Remark</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxLong_Ajax" MaxLength="250"
                                    Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.Remark %>" Height="39px"
                                    Width="382px" ToolTip="Enter Remark" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblNote" class="clsLabel">Note</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxLong_Ajax" MaxLength="250"
                                    Text="<%# mWorkInvoice.WorkInvoiceItems.CurrentItem.Note %>" Height="39px" Width="382px"
                                    ToolTip="Enter Note" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <span id="lblAttachFile" class="clsLabel">Attach File</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                        causesvalidation="false" runat="server" class="clsButton_Ajax" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                        Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                        Height="20px" Width="20px"></asp:ImageButton>
                                                    <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right" colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Ok" ToolTip="Click to Add Work Invoice Item">
                                                    </asp:Button>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" Text="Back" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
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

        function OpenFileUploadWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx?Type=pup");
                //                if (!$.browser.msie) {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = "hidden";
                //                }
                return false;
            } catch (e) {
                alert(e);
            }

        }

       
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
    <!-- End -->
    </form>
</body>
</html>
