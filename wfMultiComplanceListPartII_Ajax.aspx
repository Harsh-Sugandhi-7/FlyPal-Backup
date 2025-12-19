<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMultiComplanceListPartII_Ajax.aspx.vb"
    Inherits="Flypal.wfMultiComplanceListPartII_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Maintenance Activity</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" language="javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script type="text/javascript" language="javascript" id="clientEventHandlersJS">
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
        .style1 {
            height: 24px;
        }

        .style2 {
            height: 28px;            
        }

        .style3 {
            width: 465px;
        }
    </style>
    <script src="jquery.tooltip.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "images/close.gif";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "images/detail.gif";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--Added by Saylee on 11-Mar-2014 for ALL11032014-->
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('.cbSelectRow').change(function () {
                    // detect if the checkbox is checked
                    var checked = $(this).prop('checked');
                    // gets the table row indiect parent
                    var trParent = $(this).closest('tr');
                    // add or remove the css class according to the check state
                    if (checked == true)
                        trParent.addClass('clslightColor')
                    else
                        trParent.removeClass('clslightColor');
                })
                    // the each is used when postback is triggered with checked rows
                    .each(function (index, element) {
                        var checked = $(element).prop('checked');
                        if (checked == true)
                            $(element).closest('tr').addClass('clslightColor');
                        else
                            $(element).closest('tr').removeClass('clslightColor');
                    });
                // select all click
                $("#chkSelectAll").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });

                // select all click
                $("#chkSelectAllComp").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });
            });

        </script>
        <!-- End-->
        <div>
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <table class="clstablelistout" id="tblmain">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Multi Compliance List</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="Table6" class="clsTable1" cellpadding="0" designtimedragdrop="427">
                                            <tr>
                                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <td valign="top" colspan="2">
                                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                                                CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                        </td>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                        <tr>
                                                            <td valign="top">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="1" width="430px">
                                                                            <fieldset id="fdswodetail" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                                                <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Searching Criteria</b></legend>
                                                                                <asp:UpdatePanel ID="upnlRadioButtons" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table>
                                                                                            <tr class="style2">
                                                                                                <td colspan="3">
                                                                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step I : Selection of Maintenance Activity</asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="3">
                                                                                                    <table id="Table4" cellspacing="0">
                                                                                                        <tr class="style2">
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbAssemblyService" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Maintenance Program", "Assembly Service") %>' GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="Label2" runat="server" Visible="False" Width="36px">L1</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbComponentService" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Component TBO/SLL", "Component Service") %>' GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="style2">
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbAssemblyInspection" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text="Assembly Inspection" GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbComponentInspection" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text="Component Inspection" GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="style2">
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbAssemblyDirective" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "AD/SB Management", "Assembly Directive") %>' GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rdbComponentDirective" runat="server" CssClass="clsRadioButton"
                                                                                                                    Text='<%#IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Component AD/SB", "Component Modification") %>' GroupName="a" AutoPostBack="True"></asp:RadioButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="style2">
                                                                                                <td colspan="3">
                                                                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step II : Search for Note / Interval / Reference/ Zone</asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="style2">
                                                                                                <%--  <td>
                                                                                                <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto" Width="95px">Note / Interval</asp:Label>
                                                                                            </td>--%>
                                                                                                <td class="style1" colspan="2">
                                                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Note/Interval/Reference/Zone"></asp:TextBox>
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                                                        Text="Find Now"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td valign="top">
                                                                <fieldset id="Fieldset1" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                                    <legend id="Legend1" class="clsFieldSet1" runat="server"><b>Compliance Details</b></legend>
                                                                    <table id="Table1" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left" class="style3">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td valign="top" align="right">
                                                                                            <asp:UpdatePanel ID="upnlDet" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <table id="Table2" cellspacing="0">
                                                                                                        <tr class="style2">
                                                                                                            <td align="left" class="style2">
                                                                                                                <asp:Label ID="lblComplianceDate" runat="server" CssClass="clsLabelAuto" Width="100px">Compliance Date</asp:Label>
                                                                                                            </td>
                                                                                                            <td valign="top" align="left" class="style2">
                                                                                                                <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"></asp:TextBox>
                                                                                                                <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                                                    Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtAsOnDate"></cc2:CalendarExtender>
                                                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                                                                                    WatermarkCssClass="clsDateTextBox" Enabled="True"></cc2:TextBoxWatermarkExtender>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="style2">
                                                                                                            <td align="left">
                                                                                                                <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                                                                            </td>
                                                                                                            <td valign="top" align="left">
                                                                                                                <asp:TextBox ID="txtAircraft" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                                                                                    BackColor="#E0E0E0"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="style2">
                                                                                                            <td align="left">
                                                                                                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                                                            </td>
                                                                                                            <td valign="top" align="left">
                                                                                                                <asp:TextBox ID="txtAssembly" runat="server" CssClass="clsTextBox_Ajax" ReadOnly="True"
                                                                                                                    BackColor="#E0E0E0"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr class="style2">
                                                                                                            <td align="left">
                                                                                                                <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label>
                                                                                                            </td>
                                                                                                            <td valign="top" align="left">
                                                                                                                <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Work Order No."></asp:TextBox>

                                                                                                                <asp:CustomValidator ID="cvWorkOrderNo" runat="server" ControlToValidate="txtWorkOrderNo"
                                                                                                                    Display="None" OnServerValidate="customvalidate1" CssClass="clsTextBox_Ajax"></asp:CustomValidator>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Place"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>

                                                                                        </td>
                                                                                        <td valign="top" align="left">
                                                                                            <asp:UpdatePanel ID="upnlCurrentValues" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader" Height="17px">Compliance On</asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                                                                    ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AutoGenerateColumns="False">
                                                                                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                                                                    <Columns>
                                                                                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period"></asp:BoundField>
                                                                                                                        <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundField>
                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
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
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" valign="top">
                                                    <fieldset id="Fieldset2" style="padding: 0px 4px 0px 0px; width: auto; z-index: 10000;">
                                                        <legend id="Legend2" class="clsFieldSet1" runat="server"><b>Your Cart</b></legend>
                                                        <asp:UpdatePanel ID="upnlCart" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="lblCart" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox3_Ajax"
                                                                    ReadOnly="True" Width="100%"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlButtonsTop" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="Table3" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddToCartTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            ToolTip="Click to add into the Cart" Visible="False" Text="Add To Cart"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnNextTop" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go onto next Page"
                                                                            Text="View Cart"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnCloseTop" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            ToolTip="Back to Previous Page" Visible="False" Text="Back" CausesValidation="False"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlInstList" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgInstalledList" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AllowPaging="True"
                                                                AutoGenerateColumns="False" PageSize="5">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectInstalledList" runat="server" CssClass="cbSelectRow"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField Visible="False" DataField="CompStatusID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp Info">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ValueFormatted" SortExpression="ValueFormatted" HeaderText="Value">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Removal Reason">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                                DataTextField="Name">
                                                                            </asp:DropDownList>
                                                                            <asp:CustomValidator ID="cvReason" runat="server" OnServerValidate="customvalidate1"
                                                                                Display="None" ControlToValidate="cmbReason" ErrorMessage="Reason Required"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Note">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInstalledNote" runat="server" CssClass="clsTextBoxMultiline"
                                                                                ToolTip="Enter Note" TextMode="MultiLine"></asp:TextBox>
                                                                            <asp:CustomValidator ID="cvNote" runat="server" ControlToValidate="txtInstalledNote"
                                                                                ErrorMessage="Max Lenght of Note should be 200 Chars." Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="False" HeaderText="Is Expired">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsExpired" runat="server" CssClass="clsCheckBox"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Done By Agency">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtInstalledDoneByAgency" runat="server" CssClass="clsTextBox2_Ajax"
                                                                                ToolTip="Enter Done By Agency Name" MaxLength="100"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlRemList" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgRemovedList" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                ShowHeaderWhenEmpty="true" EnableViewState="false" AllowSorting="True" AllowPaging="True"
                                                                AutoGenerateColumns="False" PageSize="5">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="CompStatusID" HeaderText="CompStatusID "></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectRemovedList" runat="server" CssClass="cbSelectRow"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Aircraft Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ATAChapter" SortExpression="ATAChapter" HeaderText="ATA Chapter">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Component Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PeriodNameForWeb" SortExpression="PeriodNameForWeb" HeaderText="Period">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ValueFormatted" SortExpression="ValueFormatted" HeaderText="Value">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Done By">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemovedDoneByAgency" runat="server" CssClass="clsTextBox2_Ajax"
                                                                                ToolTip="Enter Done By Agency Name" MaxLength="100"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlDueList" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgDueMonitoringList" runat="server" CssClass="clsGrid" ShowHeaderWhenEmpty="True"
                                                                EnableViewState="True" AllowSorting="True" AutoGenerateColumns="False">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" id="chkSelectAll" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <div class="clstooltip" style="display: none;">
                                                                                    <b>Monitor Info:</b>&nbsp;
                                                                                <%# Eval("TypeDet")%>
                                                                                </div>
                                                                                <input type="checkbox" name="chkSelectAssemblyList" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                                                    <img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                                                </a>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle CssClass="hideGridColumn" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
                                                                        HeaderText="Machine Info.">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
                                                                        HeaderText="Assembly Type">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                                        HeaderText="Assembly Info.">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ModelMonitorCode" SortExpression="ModelMonitorCode" HeaderText="Monitior Type">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Code_No_Desc" SortExpression="Code_No_Desc" HeaderText="Description" HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FrequencyValueFormatted" SortExpression="FrequencyValueFormatted"
                                                                        HeaderText="Frequency" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnValueFormatted" SortExpression="DoneOnValueFormatted"
                                                                        HeaderText="Done On Value " HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CurrentValueFormatted" SortExpression="CurrentValueFormatted"
                                                                        HeaderText="Current" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ElapsedValueFormatted" SortExpression="ElapsedValueFormatted"
                                                                        HeaderText="Elapsed" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExtensionValueFormatted" SortExpression="ExtensionValueFormatted"
                                                                        HeaderText="Extension" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueOnValueFormatted" SortExpression="DueOnValueFormatted"
                                                                        HeaderText="Due At" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RemainingValueFormatted" SortExpression="RemainingValueFormatted"
                                                                        HeaderText="Remaining" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                        <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Zone" HeaderText="Zone">
                                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>

                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td colspan="100%" bgcolor="White" width="0px">
                                                                                    <div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                                                        <asp:GridView ID="grdLinkActivity" runat="server" AutoGenerateColumns="False" Width="95%"
                                                                                            BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                                                            AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                            SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                <asp:BoundField DataField="LinkedMaintenanceTypeName" HeaderText="Linked with">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField Visible="False" DataField="MonitorType" SortExpression="MonitorType"
                                                                                                    HeaderText="Monitor Type">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DirectiveNo" SortExpression="DirectiveNo" HeaderText="Directive Number">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                    <HeaderStyle ForeColor="White" Wrap="true" Width="330px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="330px" CssClass="TextBreak" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MaintenanceActionName" SortExpression="MaintenanceActionName"
                                                                                                    HeaderText="Action Type">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel ID="upnlDueCompList" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="dgDueMonitoringCompList" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                                ShowHeaderWhenEmpty="True" EnableViewState="True" AllowSorting="True" AutoGenerateColumns="False">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <HeaderTemplate>
                                                                            <input type="checkbox" id="chkSelectAllComp" />
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <div class="clstooltip" style="display: none;">
                                                                                <b>Monitor Info:</b>&nbsp;
                                                                            <%# Eval("TypeDet")%>
                                                                            </div>
                                                                            <div>
                                                                                <input type="checkbox" name="chkSelectCompList" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <div>
                                                                                <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                                                    <img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                                                </a>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
                                                                        ItemStyle-CssClass="hideGridColumn">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                        <ItemStyle CssClass="hideGridColumn" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="MachineInfo" SortExpression="MachineInfo"
                                                                        HeaderText="Aircraft Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="AssemblyType" SortExpression="AssemblyType"
                                                                        HeaderText="Assembly Type">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo"
                                                                        HeaderText="Assembly Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CompInfo" SortExpression="CompInfo" HeaderText="Comp. Info.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PartMonitorCode" SortExpression="PartMonitorCode" HeaderText="Monitor Type">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Code_No_Desc" SortExpression="Code_No_Desc" HeaderText="Description" HtmlEncode="false">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnFormatted" HeaderText="Done On">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnWONo" SortExpression="DoneOnWONo" HeaderText="Work Order No.">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PeriodUnitName" SortExpression="PeriodUnitName" HeaderText="Period">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="FrequencyValueFormatted" SortExpression="FrequencyValueFormatted"
                                                                        HeaderText="Frequency" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DoneOnValueFormatted" SortExpression="DoneOnValueFormatted"
                                                                        HeaderText="Done On Value " HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CurrentValueFormatted" SortExpression="CurrentValueFormatted"
                                                                        HeaderText="Current" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ElapsedValueFormatted" SortExpression="ElapsedValueFormatted"
                                                                        HeaderText="Elapsed" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ExtensionvalueFormatted" SortExpression="ExtensionvalueFormatted"
                                                                        HeaderText="Extension" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DueOnvalueFormatted" SortExpression="DueOnvalueFormatted"
                                                                        HeaderText="Due At." HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RemainingValueFormatted" SortExpression="RemainingValueFormatted"
                                                                        HeaderText="Remaining" HtmlEncode="False">
                                                                        <HeaderStyle ForeColor="White"></HeaderStyle>
                                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" SortExpression="Note" HeaderText="Note">
                                                                        <HeaderStyle ForeColor="White" HorizontalAlign="left"></HeaderStyle>
                                                                    </asp:BoundField>

                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td colspan="100%" bgcolor="White" width="0px">
                                                                                    <div id="ID-<%# Eval("ID") %>" style="display: none; position: relative; left: 25px;">
                                                                                        <asp:GridView ID="grdLinkActivityComp" runat="server" AutoGenerateColumns="False"
                                                                                            Width="95%" BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                                                                            AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                            SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                            <HeaderStyle CssClass="clsdgHeader" />
                                                                                            <Columns>
                                                                                                <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                                <asp:BoundField DataField="LinkedMaintenanceTypeName" HeaderText="Linked with">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code/Form No.">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField Visible="False" DataField="MonitorType" SortExpression="MonitorType"
                                                                                                    HeaderText="Monitor Type">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="ATA" SortExpression="ATA" HeaderText="ATA Chapter">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="DirectiveNo" SortExpression="DirectiveNo" HeaderText="Directive Number">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
                                                                                                    <HeaderStyle ForeColor="White" Wrap="true" Width="330px" HorizontalAlign="Left"></HeaderStyle>
                                                                                                    <ItemStyle HorizontalAlign="Left" Wrap="true" Width="330px" CssClass="TextBreak" />
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="MaintenanceActionName" SortExpression="MaintenanceActionName"
                                                                                                    HeaderText="Action Type">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
                                                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnAddToCart" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                                            ToolTip="Click to add into the Cart" Text="Add To Cart"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnNext" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to go onto next Page"
                                                                            Text="View Cart"></asp:Button>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" TabIndex="0" runat="server" CssClass="clsButton_Ajax" ToolTip="Back to Previous Page"
                                                                            Text="Back" CausesValidation="False"></asp:Button>
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
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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

        </div>
        <!--Date Validations-->
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
        <!--call parent function after completing subroutine..(when page open as popup)-->
        <!-- <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForTaskCardSpare();
            return false;
        }
    </script>-->
        <div>
            <!--UPDATEPANEL -->
            <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  
                $(document).ready(function () {
                    SetPageLayout();
                    if ($.browser.msie) {
                        parent.IFrameMaintenanceActivityStateComplete();
                    }


                });
        <% End if %>
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
                function endRequestHandler() {
                    SetPageLayout();

                }

                function SetPageLayout() {
            <% Dim mopenas As String = Request.QueryString("Type") %>
                <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
                ReSetPageLayout();
                onResize();//for Top bottom link
                <% End if %>
                }
                function ReSetPageLayout() {
                    $("body,html").css({ 'background-color': 'transparent' });
                    var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
                    var windowheight = $(window).height();
                    if (tempMargtop >= windowheight) {
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto' });
                    }
                    else {
                        var margintop = (windowheight / 2) - (tempMargtop / 2);
                        $("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
                    }

                }
            </script>
        </div>
    </form>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ("<%= page.IsPostback%>" == "False") {
                $(".clstooltip").closest("tr").mousemove(function (event) {
                    $(this).find(".clstooltip").css({
                        "left": event.pageX + 1,
                        "top": event.pageY + 1
                    }).show();
                }).mouseout(function () { $(this).find(".clstooltip").hide(); });;
            }
        });
    </script>
</body>
</html>
