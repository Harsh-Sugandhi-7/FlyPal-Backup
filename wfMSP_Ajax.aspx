<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMSP_Ajax.aspx.vb" Inherits="Flypal.wfMSP_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Maintenance Support Plan</title>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
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
                                <td colspan="7" class="clsFormHeader1Newstyle">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Maintenance Support Plan </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
                                                ValidationGroup="a" ErrorMessage="Maintenance Support Plan Date Required." ControlToValidate="txtMSPDate"
                                                Display="None" CssClass="clsValidationSummary"></asp:CustomValidator>

                                            <asp:RequiredFieldValidator
                                                ID="rfvDate" runat="server" Display="None" ErrorMessage="Date Required."
                                                ValidationGroup="a" ControlToValidate="txtMSPDate" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvContractNo" runat="server" Display="None" ErrorMessage="Contract No. Required."
                                                ValidationGroup="a" ControlToValidate="txtContractNo" CssClass="clsValidationSummary">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator
                                                ID="rfvPlanName" runat="server" Display="None" ErrorMessage="Plan Name Required."
                                                ValidationGroup="a" ControlToValidate="txtPlanName" CssClass="clsValidationSummary"></asp:RequiredFieldValidator>

                                            <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ValidateVendor"
                                                ValidationGroup="a" Display="None" ControlToValidate="cmbVendor" ErrorMessage="Please Select Vendor."
                                                CssClass="clsValidationSummary"></asp:CustomValidator>


                                            <script type="text/javascript">
                                                function ValidateVendor(source, args) {
                                                    args.IsValid = false;
                                                    var dd = $get("cmbVendor");
                                                    if (dd.selectedIndex != 0) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }

                                            </script>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlMSPDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblStarDate" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDate" class="clsLabel">Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMSPDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                            Text="" Width="100px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtMSPDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtMSPDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="txtMSPDateWatermarkExtender" runat="server"
                                                            TargetControlID="txtMSPDate" WatermarkCssClass="clsDateTextBox"
                                                            WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarInvoiceNo" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblNo" class="clsLabel">MSP No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMSPText" runat="server" Text="<%# mMSP.Text %>"
                                                            CssClass="clsTextBoxTagSearch" ToolTip="Enter No." MaxLength="25"
                                                            Width="208px"> </asp:TextBox>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtMSPText_Autocomplete"
                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                            MinimumPrefixLength="0" CompletionInterval="1" ServicePath="wfMSP_Ajax.aspx"
                                                            ServiceMethod="GetDistinctTextListAutoComplete" TargetControlID="txtMSPText"
                                                            UseContextKey="False">
                                                        </cc2:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMSPNo" runat="server" Text="<%# mMSP.No %>"
                                                            CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="8" ToolTip="Enter No."> </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblContractNoStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblContractNo" runat="server" CssClass="clsLabelAuto">Contract No.</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtContractNo" runat="server" Text="<%# mMSP.ContractNo %>"
                                                            CssClass="clsTextBoxTagSearch" MaxLength="50"
                                                            ToolTip="Enter Contract No." Width="208px" autocomplete="off">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblStarDetails" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPlanName" runat="server" CssClass="clsLabel">Plan Name</asp:Label>
                                                    </td>
                                                    <td colspan="2">


                                                        <asp:TextBox ID="txtPlanName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="25" Text="<%# mMSP.PlanName %>" ToolTip="Enter Plan Name" Width="208px" autocomplete="off">
                                                        </asp:TextBox>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <span id="lblVendorStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto">Vendor</asp:Label>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsTextBoxTagSearchCombo" SelectedValue="<%# mMSP.VendorID %>"
                                                            DataTextField="Name" DataValueField="ID"
                                                            Width="225px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td><span id="lblFromStar" class="clsLabelStar">*</span></td>
                                                    <td><span id="lblFromDate" class="clsLabelAuto">From Date.</span> </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                        AutoPostBack="true"  Text="<%# mMSP.FromDateFormatted %>" autocomplete="off"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="txtFromDateWatermarkExtender"
                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td><span id="lblToDateStar" class="clsLabelStar">*</span></td>
                                                    <td><span id="lblToDate" class="clsLabelAuto">To Date.</span> </td>
                                                    <td colspan="2">
                                                         <asp:TextBox ID="txtToDate" runat="server" CssClass="clsTextBoxTagSearchDate" 
                                                                        AutoPostBack="true"  Text="<%# mMSP.ToDateFormatted %>" autocomplete="off"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="txtToDateWatermarkExtender"
                                                                        runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td>&nbsp;
                                                        <asp:UpdatePanel ID="upnlAttachFile" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                runat="server" class="clsbtnH clsinfoH1" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnDelAttach" runat="server" class="clsbtnH clsinfoH1" Enabled="False" Height="30px" Text="Remove Attachment" ToolTip="Click to Remove Attachment" Width="160px" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px" ImageUrl="icons/CLIP01.ICO" Width="20px" />
                                                                            <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static" Style="display: none;" Text="----" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <span id="lblNotApplicable" class="clsLabel">Not Applicable</span>&nbsp;

                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkNotApplicable" runat="server" Checked="<%# mMSP.IsNotApplicable %>" CssClass="clsLabelAuto" TextAlign="Right" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong" MaxLength="1000" Text="<%# mMSP.Remark %>" ToolTip="Enter Remark" Width="670px" TextMode="MultiLine" >
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="7">
                                    <asp:UpdatePanel runat="server" ID="upnlMSPAssembly" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblAssemblyAdd" class="clsLabelHeader">Applicable Assembly(s):</span>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAssemblyAdd" runat="server" class="clsbtnH clsinfoH1" Height="30px" Text="Add" ToolTip="Click To Add Assembly."
                                                                        ValidationGroup="a"></asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="dgMSPAssembly" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
                                                            AutoGenerateColumns="False" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                            <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
                                                            <Columns>
                                                                <%--0--%>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                <%--1--%>

                                                                <asp:BoundField DataField="AssemblyName" HeaderText="Applicable To">
                                                                    <HeaderStyle Wrap="False" HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                                    <FooterStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--2--%>
                                                                <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--3--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="dropdown">
                                                                            <div class="dropdownbtn-content">
                                                                                <table id="T1" class="clsGridNew_Ajax">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="EditView" runat="server" CommandName="EditView" Style="height: 15px; width: 15px"
                                                                                                ImageUrl="~/images/edit.png" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="DeleteRecord" runat="server" CommandName="DeleteRecord" Style="height: 20px; width: 20px"
                                                                                                ImageUrl="~/images/delete.png" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                   
                                                                                </table>
                                                                            </div>
                                                                            <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                                Style="cursor: pointer" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="7" align="right"></td>
                            </tr>
                            <tr>
                                <td colspan="7" align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>

                                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="clsbtnH clsinfoH1" ToolTip="Click to save"
                                                            ValidationGroup="a"></asp:Button>
                                                        <asp:Button ID="btnBack" runat="server" Text="Close" class="clsbtnH clsinfoH1" ToolTip="Click to close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;" colspan="7">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnMSPAssembly" ClientIDMode="Static" runat="server" Text="----"
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
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
        <!--MSPAssembly Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyMSPAssembly" Text="MSPAssembly" CausesValidation="false"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlMSPAssembly" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeMSPAssembly" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupMSPAssembly" runat="server" TargetControlID="btnDummyMSPAssembly"
            PopupControlID="pnlMSPAssembly" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameMSPAssemblyStateComplete() {
                $("#btnDummyMSPAssembly").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenMSPAssemblyWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeMSPAssembly").attr("src", "wfMSPAssembly_Ajax.aspx?Type=pup");

                    /*if (!$.browser.msie) {*/
                    $("#btnDummyMSPAssembly").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    //}
                    return false;
                } catch (e) {
                    alert(e);
                }
            }
            function ParentCallBackFunctionForMSPAssembly() {
                var MSPAssemblyWindow = $find("<%=mdlPopupMSPAssembly.ClientID %>");
                //close popup window
                MSPAssemblyWindow.hide();
                //release resources
                $("#IframeMSPAssembly").attr("src", "JavaScript:''");
                //call button click
                $("#hdnBtnMSPAssembly").click();
            }
        </script>
        <!-- End-->
    </form>
</body>
</html>
