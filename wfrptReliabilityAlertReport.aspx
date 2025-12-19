<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptReliabilityAlertReport.aspx.vb"
    Inherits="Flypal.wfrptReliabilityAlertReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
    <title>Fleet Reliability Summary</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
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
    <style type="text/css">
        .btn
        {
            padding: 1px;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="clstitle1">Pireps/Maintenance Defects Alert Level</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvModel" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select the Model"
                                            ControlToValidate="cmbModel" Display="None" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvATA" runat="server" ControlToValidate="cmbATA" CssClass="clsLabelAuto"
                                            Display="None" ErrorMessage="Select the ATA" OnServerValidate="CustomValidate"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlMonthYear" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Month and Year</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto">Month and Year</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsComboBox1_Ajax">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="clsComboBox1_Ajax" Width="112px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="left">
                                                    <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step II. Selection of Model</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAircraftStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto">Model</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbModel" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="ModelName"
                                                        DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step III. Selection of ATA</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%-- <asp:Label ID="lblAircraftStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblATA" runat="server" CssClass="clsLabelAuto">ATA</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:ListBox ID="cmbATAList" runat="server" ClientIDMode="Static" DataTextField="ATAChapter"
                                                        DataValueField="ID" SelectionMode="Multiple"></asp:ListBox>
                                                    <asp:DropDownList ID="cmbATA" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="ATAChapter"
                                                        Visible="false" DataValueField="ID">
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
                                <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Pireps/Maintenance Defects</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbPireps" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                Checked="true" />
                                            <span class="clsLabel">Pireps</span>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbMaintenanceDefect" runat="server" CssClass="clsRadioButton"
                                                GroupName="a" />
                                            <span class="clsLabel">Maintenance Defect</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblStep4" runat="server" CssClass="clsLabelHeader">Step V. Display Report</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
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
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblATA1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" CssClass="clsButtonLong_Ajax"
                                                        ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                        ToolTip="Click to Display Report" Text="Display"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close the Fleet Reliability Summary screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </form>
    <script src="bootstrap/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrap/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=cmbATAList]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 250,
                nonSelectedText: '(ALL)',
                selectAllJustVisible: false
            });
        });
    </script>
</body>
</html>
