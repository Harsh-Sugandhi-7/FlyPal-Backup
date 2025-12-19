<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfEnquiryItem_Ajax.aspx.vb"
    Inherits="Flypal.wfEnquiryItem_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Enquiry Item Details</title>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblMain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                    <table id="tblinner" class="clsTablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Enquiry Item [New]</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Enquiry Item List"
                                                                    ValidationGroup="1"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                                    CausesValidation="False"></asp:Button>
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
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                    HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" Display="None" CssClass="clsLabelAuto"
                                    ControlToValidate="txtPartNo" ErrorMessage="Part No. Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvQty" ControlToValidate="txtQty" runat="server" Display="None"
                                    CssClass="clsLabelAuto" ValidationGroup="1" ErrorMessage="Quantity must be greater than Zero"
                                    ClientValidationFunction="validateQty" ValidateEmptyText="true" >
                                </asp:CustomValidator>
                                <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" Display="None" CssClass="clsLabelAuto"
                                    ControlToValidate="txtDescription" ErrorMessage="Part Description Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function validateQty(source, args) {
                                        var Value = $get(source.controltovalidate).value;
                                        if (Value == "0" || Value == "") {
                                            args.IsValid = false;
                                            return;
                                        }
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblOrderInfo" class="clsLabelHeader">Enquiry Item Information</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblNote1" class="clsLabelAuto">Enter the Details of Items Enquired by selecting
                                    the Part No. from list and mention the Qty</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlEnqItemDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblSrNo" class="clsLabel">Sr. No.</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSrNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEnquiry.EnquiryItems.CurrentItem.SrNo %>"
                                                        MaxLength="5" BackColor="#E0E0E0" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblPartNoStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlImgBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPartNo" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEnquiry.EnquiryItems.CurrentItem.ItemName %>" ClientIDMode="Static" 
                                                                            MaxLength="50" ReadOnly='<%# ((Session("Edit") AND (NOT mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty) ) )OR (NOT mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty) )) %>'>
                                                                        </asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:Button ID="imgbtnPartNo" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                            ToolTip="Click to Add New Part No." Enabled="<%# ((mEnquiry.IsNew) Or (mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty))) And mEnquiry.EnquiryItems.CurrentItem.EnquiryItemRequisitionItems.Count = 0 %>"
                                                                            CausesValidation="False"></asp:Button>--%>

                                                                        <asp:ImageButton ID="imgbtnPartNo" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                            ToolTip="Click to Add New Part No." CausesValidation="False"></asp:ImageButton>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblDescriptionStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDesc" class="clsLabel">Description</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEnquiry.EnquiryItems.CurrentItem.ItemDescription %>"
                                                      Width="300px"  MaxLength="200" ReadOnly='<%# ((Session("Edit") And (Not mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty))) Or (Not mEnquiry.EnquiryItems.CurrentItem.ItemID.Equals(Guid.Empty))) %>'
                                                        TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPartType" class="clsLabel">Part Type</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="cmbPartTypeList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                        SelectedValue="<%# mEnquiry.EnquiryItems.CurrentItem.ItemTypeID %>" DataTextField="Name"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="lblQuantityStar1" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblQuantity" class="clsLabel">Quantity</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchSmall" style="text-align:right"
                                                        Text="<%# mEnquiry.EnquiryItems.CurrentItem.Qty %>" MaxLength="8" ToolTip="Enter Quantity"
                                                        Enabled="<%# (CType(mEnquiry.TransTypeID, FlyPal.Util.Trans) <> FlyPal.Util.Trans.RequestingForQuotation) Or (mEnquiry.EnquiryItems.CurrentItem.RequisitionItemEnquiryItems.Count = 0) %>">
                                                    </asp:TextBox>
                                                    <asp:TextBox ID="txtUnit" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mEnquiry.EnquiryItems.CurrentItem.UnitName %>"
                                                        BackColor="#E0E0E0" ReadOnly="True">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span id="lblAppInfo" class="clsLabelAuto">Select the Model to which the Part is Applicable
                                                        [Eg. Aircraft/ Engine/ Ground Equipment]</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblApplicable" class="clsLabel">Applicable To</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="cmbApplicable" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                        SelectedValue="<%# mEnquiry.EnquiryItems.CurrentItem.ModelID %>" DataTextField="ModelAndTypeName"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblPriority" class="clsLabel">Priority</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" SelectedValue="<%# mEnquiry.EnquiryItems.CurrentItem.PriorityID %>"
                                                        DataTextField="Name" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRemark" class="clsLabel">Remark</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEnquiry.EnquiryItems.CurrentItem.Remark %>"
                                                        MaxLength="250" ToolTip="Enter Remark" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblNote" class="clsLabel">Note</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle" Text="<%# mEnquiry.EnquiryItems.CurrentItem.Note %>"
                                                        MaxLength="250" ToolTip="Enter Note" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblRequiredinDays" class="clsLabel">Required In Days</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReqDays" runat="server" style="text-align:right" CssClass="clsTextBoxTagSearch"
                                                        Text="<%# mEnquiry.EnquiryItems.CurrentItem.RequiredInDays %>" MaxLength="4">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <span id="lblIPCRefernce" class="clsLabel">IPC Reference</span>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtIPCRefer" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mEnquiry.EnquiryItems.CurrentItem.IPCReference %>">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok" ToolTip="Click to add Item in Enquiry Item List"
                                                        ValidationGroup="1"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnItemList" ClientIDMode="Static" runat="server" Text="----"
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
    <!-- ItemList Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyItemList" Text="Dummy ItemList" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupItemList" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="iPopupItemList" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupItemList" runat="server" TargetControlID="btnDummyItemList"
        PopupControlID="pnlPopupItemList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameItemListStateComplete() {
            $("#btnDummyItemList").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        $(document).ready(function () {
            $("#imgbtnPartNo").live("click", function () {
                try {
                    var PartNo = $("#txtPartNo").val();
                    $get("AjaxLoader").style.visibility = "visible";
                    $("#iPopupItemList").attr("src", "wfPartStockStatusListForEnquiry_Ajax.aspx?Type=pup&PartNo=" + escape(PartNo));
                    if (!$.browser.msie) {
                        $("#btnDummyItemList").click();
                        $get("AjaxLoader").style.visibility = "hidden";
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            });
        }); 
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForItemList() {
            var ItemListWindow = $find("<%=mdlPopupItemList.ClientID %>");
            //close ItemList popup window
            ItemListWindow.hide();
            $("#iPopupItemList").attr("src", "JavaScript:''");
            //call ItemList image button
            $("#hdnimgBtnItemList").click();
        }
    </script>
    <!-- End-->
    </form>
</body>
</html>
