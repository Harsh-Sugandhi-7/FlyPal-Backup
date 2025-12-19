<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAssemblyRemoveInstallSwapping.aspx.vb"
    Inherits="Flypal.wfAssemblyRemoveInstallSwapping" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBoxNew" Src="MSGBoxNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="viewport" content="width=device-width, initial-scale=1,shrink-to-fit=no" />
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <!-- CSS -->
    <link rel="stylesheet" href="wizard/css/style.css" />
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Roboto:400,100,300,500" />
    <link rel="stylesheet" href="wizard/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="wizard/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="wizard/css/form-elements.css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <!-- Favicon and touch icons -->
    <link rel="shortcut icon" href="wizard/ico/favicon.png" />
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="wizard/ico/apple-touch-icon-144-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="wizard/ico/apple-touch-icon-114-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="wizard/ico/apple-touch-icon-72-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" href="wizard/ico/apple-touch-icon-57-precomposed.png" />
    <style type="text/css">
        .clsCursorStyle
        {
            cursor: pointer;
        }
    </style>
    <script type="text/javascript" src="wizard/js/jquery-1.11.1.min.js"></script>
    <%--  <script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="wizard/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="wizard/js/jquery.backstretch.min.js"></script>
    <script type="text/javascript" src="wizard/js/retina-1.1.0.min.js"></script>
    <script type="text/javascript" src="wizard/js/scripts.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <%-- <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>--%>
    <script type="text/javascript" src="wizard/js/UploadFile.js"></script>
    <style type="text/css">
        .textheight
        {
            height: 34px;
            font-size: 8pt;
        }
    </style>
    <style type="text/css">
        .ajax__calendar
        {
            z-index: 5000;
        }
    </style>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .fileUpload1 input
        {
            position: absolute;
            top: 0;
            right: 0;
            margin: 0;
            padding: 0;
            font-size: 20px;
            cursor: pointer;
            opacity: 0;
            filter: alpha(opacity=0);
            width: 90px;
        }
        .uploadbtn
        {
            background: #E7E7E7;
            color: black;
            font-weight: 500;
            width: 90px;
            border: 1px solid gray;
            text-align: center;
            font-family: Calibri;
            font-size: 11pt;
            width: 90px;
            border: 1px solid gray;
            position: relative;
        }
    </style>
</head>
<body style="background-color: #eee">
    <div class="row">
        <div class="col-sm-11 col-sm-offset-1 col-md-11 col-md-offset-2 col-lg-11 col-lg-offset-3 form-box"
            style="margin-left: 20px">
             <a href="Dashboard.aspx" class="close">
                <img src="images/remove.jpg" style="position: relative; margin: -58 -57 0 782; height: 30px;
                    width: 30px" class="btn_close" title="Close" alt="Close" /></a>
            <form id="Form1" role="form" action="#" method="post" class="f1" runat="server" enctype="multipart/form-data">
           <%-- <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/remove.jpg" Style="position: relative;
                margin: -58 -57 0 782; height: 30px; width: 30px" />--%>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                ScriptMode="Debug" EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBoxNew ID="MSGBoxCtrlNEW" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <h3>
                Swapping of Assemblies</h3>
            <div class="form-group">
                <div class="row">
                    <asp:UpdatePanel ID="upnlSwap" runat="server">
                        <ContentTemplate>
                            <table class="controlplace">
                                <tr>
                                    <td colspan="2">
                                        <h4>
                                            Tell us details of Aircraft from which Assemblies to be swapped</h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="custValidator" runat="server" ControlToValidate="cmbAircraft"
                                                    ErrorMessage="" Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAircraft" runat="server" CssClass="form-label" Style="margin-left: -25px;
                                            text-align: left">Aircraft</asp:Label>&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                            DataValueField="ID" DataTextField="RegNo" AutoPostBack="True" Style="margin-bottom: 5px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td width="50%" valign="top">
                                                    <asp:PlaceHolder ID="pl1" runat="server" Visible="false"><legend id="Legend1" runat="server"
                                                        style="font-weight: bold; font-size: medium">
                                                        <h4>
                                                            Assembly #1 Details</h4>
                                                    </legend>
                                                        <table width="99%" style="border: 1.3px solid silver; margin-top: -15px">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="lblRemovedDate1" runat="server" CssClass="form-label" Style="width: 30px">Removed Date #1</asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtRemovedOnDate1" CssClass="form-control textheight"
                                                                        AutoPostBack="true" AutoComplete="off" Width="130px" onchange="ValidateDateText(this,'txtRemovedOnDate1_watermarkextender');"
                                                                        TabIndex="1"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtRemovedOnDate1_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        ClientIDMode="Static" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRemovedOnDate1">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtRemovedOnDate1" ID="txtRemovedOnDate1_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="form-control textheight"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="control-label" Style="width: 40px">Assembly #1</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraftAssembly1" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                                                        Style="margin-top: 5px; margin-right: 5px;" AutoPostBack="true" DataValueField="ID"
                                                                        DataTextField="ModelSerialNoPostion">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReason1" runat="server" CssClass="form-label">Reason #1</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbReason1" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                                                        DataTextField="Name" Style="margin-top: 5px" DataValueField="ID" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblInstalledOn" class="form-label">Installed On #1</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtInstalledOnDate1" CssClass="form-control textheigh"
                                                                        Width="130px" Style="margin-top: 5px" AutoPostBack="true" onchange="ValidateDateText(this,'InstalledOnDate_watermarkextender1','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtInstalledOnDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstalledOnDate1">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtInstalledOnDate1" ID="InstalledOnDate_watermarkextender1"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:PlaceHolder>
                                                </td>
                                                <td width="50%" valign="top">
                                                    <asp:PlaceHolder ID="pl2" runat="server" Visible="false"><legend id="Legend2" runat="server"
                                                        style="font-weight: bold; font-size: medium">
                                                        <h4>
                                                            Assembly #2 Details</h4>
                                                    </legend>
                                                        <table width="100%" style="border: 1.3px solid silver; margin-top: -15px">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="lblRemovedDate2" runat="server" CssClass="form-label" Style="width: 30px">Removed Date #2</asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtRemovedOnDate2" CssClass="form-control textheight"
                                                                        AutoPostBack="true" AutoComplete="off" Width="130px" onchange="ValidateDateText(this,'txtRemovedOnDate2_watermarkextender');"
                                                                        TabIndex="1"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtRemovedOnDate2_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        ClientIDMode="Static" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRemovedOnDate2">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtRemovedOnDate2" ID="txtRemovedOnDate2_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="form-control textheight"></cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="lblAssembly2" runat="server" CssClass="form-label" Style="width: 20px">Assembly #2</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAircraftAssembly2" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                                                        Style="margin-top: 5px" AutoPostBack="true" DataValueField="ID" DataTextField="ModelSerialNoPostion">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblReason2" runat="server" CssClass="form-label left">Reason #2</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbReason2" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                                                        DataTextField="Name" Style="margin-top: 5px" DataValueField="ID" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="lblInstalledOn2" class="form-label">Installed On #2</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtInstalledOnDate2" CssClass="form-control textheigh"
                                                                        Width="130px" Style="margin-top: 5px" AutoPostBack="true" onchange="ValidateDateText(this,'InstalledOnDate_watermarkextender2','true');"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="txtInstalledOnDate_CalendarExtender2" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstalledOnDate2">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtInstalledOnDate2" ID="InstalledOnDate_watermarkextender2"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:PlaceHolder>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="f1-buttons">
                <button type="button" id="btnSwap" class="btn btn-next" runat="server">
                    SWAP</button>
            </div>
            </form>
        </div>
        <!-- Javascript -->
        <!--[if lt IE 10]>
            <script src="assets/js/placeholder.js"></script>
        <![endif]-->
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
    </div>
</body>
</html>
