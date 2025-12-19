<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfConditionCheckItemComply_Ajax.aspx.vb"
    Inherits="Flypal.wfConditionCheckItemComply_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Equipment Maintenance Item</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

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
            <uc2:msgbox id="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Equipment Maintenance Item</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table id="Table1">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save &amp; Close"
                                                                    ToolTip="Click to save and close" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                    Text="Back" ToolTip="Click to go back to previous page" />
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
                        <%--<tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvItem" runat="server" Display="None" ErrorMessage="Please Select Item."
                                            ControlToValidate="cmbItemList"></asp:RequiredFieldValidator><asp:CustomValidator
                                                ID="cvItem" runat="server" Display="None" ErrorMessage="Please select the Item."
                                                ControlToValidate="cmbItemList" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvSerialNo" runat="server" Display="None" ErrorMessage="Please select the Serial No."
                                            ControlToValidate="cmbSerialNo" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCalibrationNo" runat="server" Display="None" ErrorMessage="Max. Length should be 50."
                                            ControlToValidate="txtNo" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvDoneByAgency" runat="server" ControlToValidate="txtDoneByAgency"
                                            Display="None" ErrorMessage="Max. Length should be 150." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCertRef" runat="server" ControlToValidate="txtCertRef"
                                            Display="None" ErrorMessage="Max. Length should be 100." OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvRemark" runat="server" ControlToValidate="txtRemark" Display="None"
                                            ErrorMessage="Remark should not be greater than 1000 characters" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlCalibrationItemInformation" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSet" style="border-width: 1px">
                                            <legend><b>Equipment Maintenance Item Information</b></legend>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="Span6" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblPartNo" class="clsLabel">Part No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPartNo" runat="server" Text="<%# mConditionCheckItemChild.ItemName %>"
                                                            ToolTip="Part No." BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSerialNoStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblSerialNo" class="clsLabelAuto">Serial No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" Text="<%# mConditionCheckItemChild.SerialNo %>"
                                                            ToolTip="Serial No." BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax"
                                                            >
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDesc" class="clsLabelAuto">Description</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" Text="<%# mConditionCheckItemChild.Description %>"
                                                            ToolTip="Description" BackColor="#E0E0E0" ReadOnly="True" CssClass="clsTextBoxSearch_Ajax">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblLocation" class="clsLabelAuto">Location</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
                                                            BackColor="#E0E0E0" ToolTip="Item Description" Text="<%# mConditionCheckItemChild.Location %>"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblConditionCheckServicedInspected" class="clsLabelAuto" runat="server"
                                                            visible="false">Condition Check/Serviced Inspected</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConditionCheckServicedInspected" runat="server" CssClass="clsTextBox_Ajax"
                                                            ReadOnly="True" BackColor="#E0E0E0" Text="<%# mConditionCheckItemChild.ConditionCheckServicedInspected %>"
                                                            Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span class="clsLabelAuto" runat="server" id="lblListOfItemServiceInspections">Service
                                                            Inspections</span>
                                                    </td>
                                                    <td>
                                                        <%--<asp:DropDownList ID="cmbListOfItemServiceInspections" runat="server" CssClass="clsComboBox_Ajax" Enabled="false"
                                                            SelectedValue="<%# mConditionCheckItemChild.ItemServiceInspectionsID %>" DataValueField="ID"
                                                            DataTextField="ServiceInspectionName"  >
                                                        </asp:DropDownList>--%>
                                                        <asp:Label ID="lblItemServiceInspections" runat="server" CssClass="clsLabel" Text="<%# mConditionCheckItemChild.ItemServiceInspectionsDescription %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblInterval" class="clsLabelAuto">Interval</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFrequency" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                            Text="<%# mConditionCheckItemChild.Frequency %>" ToolTip="Item Interval" AutoPostBack="True"
                                                            Width="40px">
                                                        </asp:TextBox>
                                                        <asp:Label ID="lblMonths" runat="server" CssClass="clsLabel" Text="<%# mConditionCheckItemChild.ConditionCheckPeriodIn %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblConditionCheckNo" class="clsLabelAuto">Equipment Maintenance no.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNo" runat="server" BackColor="White" CssClass="clsTextBoxTagSearch"
                                                            Text="<%# mConditionCheckItemChild.ConditionCheckNo %>" ToolTip="Enter Condition Check No"
                                                            MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblDoneOnDateStar" class="clsLabelStar">*</span>
                                                    </td>
                                                    <td>
                                                        <span id="lblDoneOnDate" class="clsLabelAuto">Done On Date.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDoneOnDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate" 
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc2:calendarextender id="CalendarExtender1" runat="server" cssclass="cal_Theme1"
                                                            enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtDoneOnDate"></cc2:calendarextender>
                                                        <cc2:textboxwatermarkextender id="TextBoxWatermarkExtender1" runat="server" targetcontrolid="txtDoneOnDate"
                                                            watermarktext="<%$AppSettings:DateFormat%>"></cc2:textboxwatermarkextender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblIsApplicable" class="clsLabelAuto">Applicable</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" AutoPostBack="True" Checked="True"
                                                            CssClass="clsCheckBox" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblNextDueDate" class="clsLabelAuto">Next Due Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNextDueDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate" 
                                                            Text="<%# mConditionCheckItemChild.NextDueDate %>" Enabled="false"></asp:TextBox>
                                                        <cc2:calendarextender id="CalendarExtender2" runat="server" cssclass="cal_Theme1"
                                                            enabled="True" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtNextDueDate"></cc2:calendarextender>
                                                        <cc2:textboxwatermarkextender id="TextBoxWatermarkExtender2" runat="server" targetcontrolid="txtNextDueDate"
                                                            watermarktext="<%$AppSettings:DateFormat%>"></cc2:textboxwatermarkextender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblDonebyAgency" class="clsLabelAuto">Done by Agency</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDoneByAgency" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="150"
                                                            Text="<%# mConditionCheckItemChild.DonebyAgency %>" ToolTip="Enter Agency Name"
                                                            Width="500px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblCertRef" class="clsLabelAuto">Certificate Reference</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCertRef" runat="server" CssClass="clsTextBoxSearch_Ajax" MaxLength="100"
                                                            Width="500px" Text="<%# mConditionCheckItemChild.CertificateReference %>" ToolTip="Enter Certificate Reference"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRemark" class="clsLabelAuto">Remark</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                            ToolTip="Enter Remark" Width="500px" Text="<%# mConditionCheckItemChild.Remark %>"
                                                            MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="1">
                                                        <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="button" id="btnSelectFile" value="Select File"  
                                                                                runat="server" class="clsbtnH clsinfoH1" />
                                                                        </td>
                                                                        <td style="padding-left: 3px;">
                                                                            <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                        </td>
                                                                        <td style="padding-left: 2px;">
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                Height="20px" Width="20px"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;">
                                                        <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
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
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnPartMaster" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                        <asp:Button ID="hdnBtnCompsImport" runat="server" CausesValidation="false" ClientIDMode="Static"
                                            Style="display: none;" Text="Add" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <%--<td align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" Text="Save &amp; Close"
                                                        ToolTip="Click to save and close" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        Text="Back" ToolTip="Click to go back to previous page" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
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
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForConditionCheckItemComply();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
             
        $(document).ready(function () {
       SetPageLayout();
       if ($.browser.msie) {
             parent.IFrameConditionCheckItemComplyStateComplete();
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
          var tempMargtop=$("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
          var windowheight=$(window).height();
          if (tempMargtop>=windowheight)
          {
            $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto'});
          }
          else
          {
          var margintop=(windowheight/2)-(tempMargtop/2);
           $("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
          }
       }
    </script>
    <%--End--%>
    <%--End--%>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="PartID" />
    <!-- File Upload Modal Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
    </div>
    <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:modalpopupextender id="mdlPopupFileUpload" runat="server" targetcontrolid="btnDummyFileUpload"
        popupcontrolid="pnlFileUpload" backgroundcssclass="clsModalPopupBG">
    </cc2:modalpopupextender>
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
    <script type="text/javascript">
        //Date validations
        function BetweenDatesValidation(source, args) {
            var IsScrapDateBlank = Sys.Extended.UI.TextBoxWrapper.get_Wrapper($get("txtScrapDate"))._isWatermarked;
            if (!IsScrapDateBlank) {
                args.IsValid = false;
                var fromdate = $("#txtManufacturingDate").val();
                var todate = $("#txtScrapDate").val();
                if (!todate) {
                    rfvToDate.isvalid = false;
                    return;
                }
                if (!fromdate) {
                    rfvFromDate.isvalid = false;
                    return;
                }
                var param = { 'FromDate': fromdate, 'ToDate': todate };
                $.ajax({
                    type: "POST",
                    url: "BetweenDateValidationHandler.ashx",
                    cache: false,
                    data: param,
                    async: false,
                    beforeSend: OnBeforeSnd,
                    success: onSuces,
                    error: onErr
                });

                function onSuces(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    if (result == "True") {
                        args.IsValid = true;
                        return;
                    }

                }

                function onErr(result) {
                    $get("AjaxLoader").style.visibility = 'hidden';
                    source.errormessage = result;
                    return;
                }
                function OnBeforeSnd() {
                    $get("AjaxLoader").style.visibility = 'visible';
                }
            }
        }

        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'false' };
            $.ajax({
                type: "POST",
                url: "DateValidationHandler.ashx",
                //        contentType: "application/json",
                cache: false,
                data: params,
                async: false,
                beforeSend: OnBeforeSend,
                success: onSuccess,
                error: onError
            });

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
</body>
</html>
