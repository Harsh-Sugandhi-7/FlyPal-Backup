<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForMasterTimeControlList_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForMasterTimeControlList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Master Time Control List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <%--MultiSelection Control--%>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <%--End MultiSelection Control--%>
    <script language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script language="javascript" id="clientEventHandlersJS">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <style type="text/css">
        .btn {
            padding: 1px;
            font-size: 8pt;
        }

        .TextBox {
            box-sizing: Content-box;
        }

        .label {
            font-weight: normal !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManagers1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clstitle1">Search criteria for Master Time Control List</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                        CssClass="clsValidationSummary" ValidationGroup="valGrp1"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="As On Date Required"
                                        ValidationGroup="valGrp1"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="cmbAircraft" ErrorMessage="Please select the Aircraft" ValidationGroup="valGrp1"
                                        ClientValidationFunction="validateAircraft"></asp:CustomValidator>
                                    <asp:CustomValidator ID="cvSelection" runat="server" CssClass="clsLabelAuto" Display="None"
                                        ControlToValidate="cmbAircraft" ErrorMessage="Please select at least One Service,Inspection or Directive"
                                        ValidationGroup="valGrp1" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validateAircraft(source, args) {
                                            args.IsValid = false;
                                            var dd = $get("cmbAircraft");
                                            if (dd.selectedIndex != 0) {
                                                args.IsValid = true;
                                                return;
                                            }
                                        }
                                    </script>
                                    <script type="text/javascript">
                                        function validateSelection(source, args) {
                                            args.IsValid = false;
                                            var ServStatus = document.getElementById("chkService");
                                            var InspStatus = document.getElementById("chkInspection");
                                            var DirStatus = document.getElementById("chkDirective");

                                            var $items = $('.active').length;

                                            if ((ServStatus.checked || InspStatus.checked || DirStatus.checked) && $items > 0) {
                                                args.IsValid = true;
                                                return;
                                            }
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStep1" class="clsLabelHeader">Selection of As On Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'Calender_watermarkextender');" Height="25px"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" ClientIDMode="Static" runat="server"
                                                            CssClass="cal_Theme1" Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="Calender_watermarkextender"
                                                            runat="server" WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="lblStep2" class="clsLabelHeader">Selection of Aircraft</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <span id="lblAircraftStar" class="clsLabelStar">*</span> <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server"  DataValueField="ID"
                                                            DataTextField="RegNo" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="left">
                                                        <span id="Label3" class="clsLabelHeader">Selection of Assembly</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssembly" runat="server" DataValueField="ID"
                                                            DataTextField="ModelSerialNoPostion" >
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAirframeDueAsOf" runat="server" CssClass="clsCheckBox" Text="With Due As Of Airframe Values"
                                                            Visible='<%#  iif(AppSettings("ClientCode") = "Heligo" or AppSettings("ClientCode") = "UHPL",False,True) %>'></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblStep4" class="clsLabelHeader">Selection of Type</span>
                                                    </td>
                                                    <td></td>
                                                    <td colspan="1">
                                                        <asp:CheckBox ID="chkNotApplicable" runat="server" CssClass="clsCheckBox" />
                                                        <span class="clsLabel">With "NOT APPLICABLE" Records</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table2" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox Text="" ID="chkService" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <asp:PlaceHolder ID="phInspection" runat="server" Visible='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", False, True) %>'>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox Text="" ID="chkInspection" runat="server" ClientIDMode="Static" />
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                    DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </asp:PlaceHolder>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox Text="" ID="chkDirective" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#IIf(AppSettings("ShowCAMOOnlyForNewClients") = "True", False, True) %>'>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="clsLabelHeader">Estimated Flying Hours</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gdPerDayLimit" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle"
                                            GridLines="Horizontal" CellPadding="3">
                                            <AlternatingRowStyle CssClass="clsdgAltItem"/>
                                            <RowStyle CssClass="clsdgItem" />
                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True"/>
                                            <Columns>
                                                <asp:BoundField DataField="PeriodID" HeaderText="PeriodID" Visible="False"></asp:BoundField>
                                                <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Limit" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>            
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ID="txtLimitPerDay" runat="server" BackColor="White" style="background-color:White;height:25px;width:240px;"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td align="left">
                                    <span id="lblStep7" class="clsLabelHeader">Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAvgMnths1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left"></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnPreview" TabIndex="0" runat="server" 
                                                            Visible="False" Text="Preview" ToolTip="Click to Preview Report"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            Text="Current Criteria" ToolTip="Click to display Current Searching criterias"
                                                            ValidationGroup="valGrp1"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" runat="server" ClientIDMode="Static" 
                                                            ValidationGroup="valGrp1" TabIndex="0" Text="Export to Excel" ToolTip="Click to Export report"
                                                             Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            Text="Display" ToolTip="Click to Display Report" ValidationGroup="valGrp1"></asp:Button>
                                                    </td>
                                                    <%-- 'Added by Shital on 14-Sep-2016--%>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server"  Text="Report By Mail"
                                                            ToolTip="Click to receive Report through mail" ValidationGroup="valGrp1"  />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                            ToolTip="Click to close Search criteria for Master Time Control List screen "
                                                            CausesValidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                                <!-- Dummy panel to open modelpopup 'Added by Shital on 14-Sep-2016 -->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgLogBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
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
        <!-- Popup For Report By Mail 14-Sep-2016 -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
            PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function OpenByMaiWindow() {
                try {
                    $("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                    $("#btnDummyReceipt1").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSendMail() {
                var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
                //close popup window
                Receiptwindow1.hide();
                //           release resources
                $("#IframeReceipt1").attr("src", "JavaScript:''");
            }
            function ParentCallBackFunctionToSendMail() {
                var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
                //close popup window
                Receiptwindow1.hide();
                //           release resources
                $("#IframeReceipt1").attr("src", "JavaScript:''");
                //call image button
                $("#hdnimgLogBtnSendMail").click();
            }
        </script>
        <!---End-->
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
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

            //Service/inspection/Directive list checking
            function ControlvisibilityForCheckboxlist(elem, childid) {
                //if selected then enable and select checkboxlist else uncheck and disable list
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#' + childid).removeAttr('disabled');
                }
                else {
                    $('#' + childid).attr('disabled', 'disabled');
                }

                $('#' + childid).find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        $(this).removeAttr('disabled');
                    }
                    else {
                        $(this).removeAttr("checked");
                        $(this).attr('disabled', 'disabled');
                    }
                });
            }

        </script>
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("#chkService").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('selectAll', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                $('[id*=ListServiceType]').multiselect('disable', false);
                $('[id*=ListServiceType]').multiselect('refresh');
            }
        });
        $("#chkInspection").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('selectAll', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);
                $('[id*=ListInspectionType]').multiselect('refresh');
            }
        });
        $("#chkDirective").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListDirectiveType]').multiselect('enable', true);
                $('[id*=ListDirectiveType]').multiselect('selectAll', false);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                $('[id*=ListDirectiveType]').multiselect('disable', false);
                $('[id*=ListDirectiveType]').multiselect('refresh');
            }
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var ServStatus = document.getElementById("chkService");
                    if (ServStatus.checked == false) {
                        $('[id*=ListServiceType]').multiselect('clearSelection', true);
                        $('[id*=ListServiceType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>',
                nSelectedText: '<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Event", "Services") %>'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListDirectiveType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var DirStatus = document.getElementById("chkDirective");
                    if (DirStatus.checked == false) {
                        $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                        $('[id*=ListDirectiveType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Directive',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directive',
                nSelectedText: 'Directive'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListInspectionType]').multiselect({
                onDropdownShow: function (event) {
                    var i = 1;
                    var Inspstatus = document.getElementById("chkInspection");
                    if (Inspstatus.checked == false) {
                        $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                        $('[id*=ListInspectionType]').multiselect('refresh');
                    }
                },
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Inspection',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Inspection',
                nSelectedText: 'Inspection'
            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
</body>
</html>
