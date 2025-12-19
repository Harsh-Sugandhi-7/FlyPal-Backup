<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Maintenance Advice From QC</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

     <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css" rel="stylesheet" /> 
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>  

    <script id="clientEventHandlersJS" type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
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
    </script>
    <style type="text/css">
        .style1
        {
            width: 62px;
            height: 23px;
        }
          .btn
        {
            padding: 1px;
            font-size: 8pt;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
        .label
        {
            font-weight: normal !important;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1">
                                <span id="lbltitle" class="clsFormHeader">Search criteria for Maintenance Advice From QC</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                            Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtDate"
                                            ErrorMessage="As On Date Required."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                            Display="None" ControlToValidate="txtDate" ErrorMessage="As On Date Required."></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvAircraft" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Aircraft from the list."
                                            ControlToValidate="cmbAircraft" Display="None" ClientValidationFunction="ClientValidation"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                            ErrorMessage="Please select the Type" ClientValidationFunction="ClientValidation"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlDate" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td width="75px">
                                                    <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static" runat="server" Height ="24px"
                                                        AutoPostBack="true" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                        ClientIDMode="Static" DataTextField="RegNo" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <table id="Table6">
                                                        <tr>
                                                            <td valign="top">
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rbForCustomer" ClientIDMode="Static" runat="server" CssClass="clsRadioButton"
                                                                    GroupName="a" Text="For Customer"></asp:RadioButton>&nbsp;&nbsp;
                                                                <asp:RadioButton ID="rbEngineeringOrder" runat="server" ClientIDMode="Static" CssClass="clsRadioButton"
                                                                    GroupName="a" Text="For Engineering Order"></asp:RadioButton>
                                                                <asp:CheckBox ID="chkTaskCard" runat="server" Text="With Task Card" ClientIDMode="Static"
                                                                    CssClass="clsCheckBox" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblSortBy" class="clsLabelAuto">Sort By</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbSordBy" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static">
                                                        <asp:ListItem Value="0">Remaining Value</asp:ListItem>
                                                        <asp:ListItem Value="1">Maintenance Type</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblFormat" class="clsLabelAuto">Format</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbFormat" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static">
                                                        <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                        <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <span id="Label3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblAssembly" class="clsLabelAuto">Assembly</span>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Description"
                                                        DataValueField="AssemblyID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span id="Span1" class="clsLabelHeader">Step IV. Selection of Type</span>
                            </td>
                        </tr>
                            <tr>
                                <td align="left">
                                    <table id="Table2" border="0" width="100%">
                                        <tr>
                                            <td width="225px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkService" runat="server" Text="" />
                                                        </td>
                                                         <td>&nbsp;</td>
                                                        <td width="100%">
                                                           <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                            DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="225px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkInspection" runat="server" Text="" />
                                                        </td>

                                                          <td>&nbsp;</td>
                                                         <td>
                                                                       <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                                      </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="225px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkDirective" runat="server" Text="" />
                                                        </td>
                                                         <td>&nbsp;</td>
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
                            <tr>
                                <td align="left">
                                    <span id="lblStep5" class="clsLabelHeader">Step V. Selection of Due Limits / Percentage
                                        Life Remaining</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:RadioButton ID="rbdDueLimits" runat="server" AutoPostBack="True" Checked="True"
                                                                        CssClass="clsRadioButton" Font-Bold="True" GroupName="StepIII" Text="Due Limits" />
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rbdPercent" runat="server" AutoPostBack="True" CssClass="clsRadioButton"
                                                                        Font-Bold="True" GroupName="StepIII" Text="Percent Life Remaining" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearch" Enabled="False"
                                                                        MaxLength="4" ToolTip="Enter Percentage" Height ="24px" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1" Width="100%">
                                                            <asp:GridView ID="dgDuePeriodLimits" runat="server" AutoGenerateColumns="False" CssClass="clsGridNewStyle" CellPadding="5" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                                <RowStyle CssClass="clsdgItem" />
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                <Columns>
                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Limit">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLimit" runat="server" BackColor="White" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Height ="24px" Width="185px"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value.">
                                                                            </asp:TextBox>
                                                                            <asp:CustomValidator ID="cvPeriodLimitsValue" runat="server" ControlToValidate="txtLimit"
                                                                                Display="None" ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblStep7" class="clsLabelHeader">Step VI. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" colspan="2">
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
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAvgMnths1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias." />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPreview" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Visible="false" Text="Preview" ToolTip="Click to Preview Report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Text="Display" ToolTip="Click to Display Report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Back to Previous Page" />
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

    </script>
    <script type="text/javascript">
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

        //Client Validation Function
        function ClientValidation(source, args) {
            if (source.controltovalidate == "cmbAircraft") {
                args.IsValid = false;
                var dd = $get("cmbAircraft");
                if (dd.selectedIndex != 0) {
                    args.IsValid = true;
                    return;
                }
            }
            else {
                //                args.IsValid = false;
                //                var ser = false;
                //                var insp = false;
                //                var dir = false;
                //                var status = '';
                //                $('#chkListDirectiveType').find(":checkbox").each(function () {
                //                    status = $(this).attr('checked');
                //                    if (status == "checked") {
                //                        dir = true;
                //                        //break;
                //                    }
                //                });
                //                status = '';
                //                $('#chkListInspectionType').find(":checkbox").each(function () {
                //                    status = $(this).attr('checked');
                //                    if (status == "checked") {
                //                        insp = true;
                //                        //break;
                //                    }
                //                });
                //                status = '';
                //                $('#chkListServiceType').find(":checkbox").each(function () {
                //                    status = $(this).attr('checked');
                //                    if (status == "checked") {
                //                        ser = true;
                //                        //break;
                //                    }
                //                });

                //                if (insp || ser || dir) {
                //                    args.IsValid = true;
                //                    return;
                //                }

                var ServStatus = document.getElementById("chkService");
                var InspStatus = document.getElementById("chkInspection");
                var DirStatus = document.getElementById("chkDirective");
                var $items = $('.active').length;
                if ((ServStatus.checked || InspStatus.checked || DirStatus.checked) && ($items > 0)) {
                    args.IsValid = true;
                    return;

                }
            }
        }
        </script>

    <script type="text/javascript">
        $("#rbForCustomer").live("click", function () {
            var status = $(this).attr('checked');
            if (status)
                ControlVisibilityForFormat('c');
        });
        $("#rbEngineeringOrder").live("click", function () {
            var status = $(this).attr('checked');
            if (status)
                ControlVisibilityForFormat('e');
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ($("#rbForCustomer").attr('checked'))
                ControlVisibilityForFormat('c');
            else
                ControlVisibilityForFormat('e');
        });

        function ControlVisibilityForFormat(status) {
            switch (status) {
                case 'e': //For Engineering Order 
                    //disable crew 2 and Duty Type As 2 controls and set values to defualt
                    $('#cmbSordBy').attr('disabled', 'disabled');
                    $('#cmbFormat').attr('disabled', 'disabled');
                    var dd2 = $get("cmbFormat");
                    //Set Format combo as format 2
                    dd2.selectedIndex = 1;
                    $('#chkTaskCard').show();
                    $('#chkTaskCard').next().show();
                    $('#chkTaskCard').removeAttr('checked');
                    break;
                case 'c': //For Customer
                    //enable crew 2 and Duty Type As 2 controls and set values to defualt
                    $('#cmbSordBy').removeAttr('disabled');
                    $('#cmbFormat').removeAttr('disabled');
                    //Set Format combo as format 1
                    var dd1 = $get("cmbFormat");
                    dd1.selectedIndex = 0;
                    $('#chkTaskCard').removeAttr('checked');
                    $('#chkTaskCard').hide();
                    $('#chkTaskCard').next().hide();
                    break;
            }
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
            }
        });

    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Services',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Services',
                nSelectedText: 'Services'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListDirectiveType]').multiselect({

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
