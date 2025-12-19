<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptReliabilityReport.aspx.vb"
    Inherits="Flypal.wfrptReliabilityReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fleet Reliability Summary</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
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
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="1800" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnlsearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin" border="0">
                                <tr>
                                    <td colspan="3">
                                        <span id="lbltitle" class="clstitle1">Fleet Reliability Summary</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvModel" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Model or Aircraft"
                                                    ControlToValidate="cmbMonth" Display="None" ClientValidationFunction="validateSelection"></asp:CustomValidator><%-- OnServerValidate="CustomValidate"--%>
                                                <script type="text/javascript">

                                                    function validateSelection(source, args) {
                                                        args.IsValid = false;
                                                        var status;

                                                        var $items = $('.active').length;

                                                        if (($items > 0)) {
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
                                    <td colspan="3" align="left">
                                        <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Month and Year</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td align="left">
                                        <span id="lblYear" class="clsLabelAuto">Month and Year</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBox1_Ajax">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox1_Ajax" Width="112px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="1">
                                        <span id="spnlabel" runat="server" clientidmode="Static" visible='<%# iif(AppSettings("ClientCode") = "SAA" ,True,False) %>'
                                            class="clsLabelHeader">(Report will be displayed with previous 2 months)</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Model/Aircraft</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblAircraftStar1" class="clsLabelStar">*</span>
                                    </td>
                                    <td>
                                        <span id="lblModel" class="clsLabelAuto">Model</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbModel" runat="server" Visible="false" CssClass="clsComboBox3_Ajax"
                                            DataTextField="ModelName" DataValueField="ID">
                                        </asp:DropDownList>
                                        <asp:ListBox ID="ListModel" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                            CssClass="clsLabelAuto" DataTextField="ModelName" DataValueField="ID"></asp:ListBox>
                                        <asp:CheckBox ID="ChkAircraftwise" runat="server" Text="Aircraft-wise" CssClass="clsLabelAuto"
                                            ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAircraft" class="clsLabelAuto" runat="server" Text="Aircraft"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:ListBox ID="ListRegNo" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                            DataTextField="RegNo" DataValueField="ID"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <span id="lblStep4" class="clsLabelHeader">Step III. Display Report</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto" Visible="False">Your selection is as follows </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblyear1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                                CssClass="clsButtonLong_Ajax" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="0"
                                                                Text="Display" CausesValidation="true" ToolTip="Click to Display Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnByMail" runat="server" CssClass="clsButton_Ajax" TabIndex="25"
                                                                Text="Report By Mail" ToolTip="Click to report by mail" ValidationGroup="1" Width="96px" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                Text="Close" ToolTip="Click to close the Fleet Reliability Summary screen" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--Dummy panel to open modelpopup-->
                                <tr style="height: 0px;">
                                    <td style="height: 0px;" colspan="2" align="right">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                            <ContentTemplate>
                                                <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                    CausesValidation="false" Style="display: none;"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <!--End -->
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <!-- Popup For Reliability -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyReliability1" Text="Reliability1" ClientIDMode="Static"
            CausesValidation="false" />
    </div>
    <asp:Panel runat="server" ID="pnlReliability1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeReliability1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupReliability1" runat="server" TargetControlID="btnDummyReliability1"
        PopupControlID="pnlReliability1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeReliability1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyReliability1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Reliabilitywindow1 = $find("<%=mdlPopupReliability1.ClientID %>");
            //close popup window
            Reliabilitywindow1.hide();
            //           release resources
            $("#IframeReliability1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Reliabilitywindow1 = $find("<%=mdlPopupReliability1.ClientID %>");
            //close popup window
            Reliabilitywindow1.hide();
            //           release resources
            $("#IframeReliability1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        function disableEnableOnPageLoad() {
            var status = $("#ChkAircraftwise").attr('checked');
            if (status) {
                $('[id*=ListRegNo]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListRegNo]').multiselect('updateButtonText');
                $('[id*=ListModel]').multiselect('clearSelection', true);
                $('[id*=ListModel]').multiselect('disable', false);
            }
            else {
                $('[id*=ListModel]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListModel]').multiselect('updateButtonText');
                $('[id*=ListRegNo]').multiselect('clearSelection', true);
                $('[id*=ListRegNo]').multiselect('disable', false);
            }
        }

        $("#ChkAircraftwise").live("click", function () {

            disableEnableOnPageLoad();

        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            ModelMultiSelect();
            RegNoMultiSelect();
            disableEnableOnPageLoad();


        });
    </script>
    <script type="text/javascript">

        // Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function ModelMultiSelect() {
            $('[id*=ListModel]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Model',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Model',
                nSelectedText: 'Model'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

            //   });
        }
    </script>
    <script type="text/javascript">

        //  Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function RegNoMultiSelect() {
            $('[id*=ListRegNo]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Aircraft',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Aircraft',
                nSelectedText: 'Aircraft'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

            // });
        }
    </script>
</body>
</html>
