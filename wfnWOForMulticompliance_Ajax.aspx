<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOForMulticompliance_Ajax.aspx.vb"
    Inherits="Flypal.wfnWOForMulticompliance_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Work Order Compliance</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />

    <script type="text/javascript" src="js\jquery-1.8.3.min.js"></script>

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

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
    
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="AutoComplete\autocomplete-setup.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="5400">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:msgbox id="MSGBoxCtrl" runat="server"></uc2:msgbox>
            </ContentTemplate>
        </asp:UpdatePanel>
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


            });

        </script>
        <!-- End-->
        <table id="tblMain" class="clstablelistout" border="0" cellspacing="1" cellpadding="1">
            <tr>
                <td>
                    <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Work Order Compliance</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" runat="server" />
                                        <asp:CustomValidator ID="cvWODate" runat="server" CssClass="clsLabelAuto"
                                            OnServerValidate="CustomValidate"
                                            Display="None" ControlToValidate="txtAsOnDate" />
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" CssClass="clsLabelAuto"
                                            ValidateEmptyText="true" OnServerValidate="CustomValidate" Display="None"
                                            ControlToValidate="txtHiddenActManHrs" />
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                            OnServerValidate="CustomValidate" Display="None" ControlToValidate="txtWOLabel" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" border="0">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">Compliance Date</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAsOnDate" runat="server" CssClass="clsTextBox_Ajax" AutoPostBack="true"
                                                                    OnTextChanged="txtAsOnDate_TextChanged" onchange="ValidateDateText(this,'txtAsOnDate_CalendarExtender');"
                                                                    Width="100px" />
                                                                <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate" />
                                                                <cc2:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="txtAsOnDate"
                                                                    WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto">Work Order</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtWOLabel" runat="server" BackColor="#E0E0E0" CssClass="clsTextBox_Ajax"
                                                                    ReadOnly="True" Text="<%# mnWO.WONumber %>" Width="164px" />
                                                                <asp:TextBox ID="txtHiddenActManHrs" runat="server"
                                                                    Style="display: none;" CssClass="clsTextBox_Ajax" Width="164px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rdbCompletedJobs" runat="server" AutoPostBack="True" Checked="True"
                                                                    CssClass="clsLabelAuto" Visible="false" GroupName="a" Text="Show ONLY &quot;COMPLETED JOBS&quot;"
                                                                    ToolTip="Check to see only &quot;COMPLETED JOBS&quot;  records" />
                                                            </td>
                                                            <td>&nbsp;
                                                            <asp:RadioButton ID="rdbALLJobs" Visible="false" runat="server" AutoPostBack="True"
                                                                CssClass="clsLabelAuto" GroupName="a" Text="Show &quot;ALL JOBS&quot;" ToolTip="Check to see &quot;ALL JOBS&quot;  records" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top" colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlCurrent" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table45" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader">Compliance On Values</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGridLog" AutoGenerateColumns="False"
                                                        PageSize="3" ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="PeriodName" HeaderText="Period"></asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values"></asp:BoundField>
                                                            <asp:BoundField DataField="PeriodID" HeaderText="Period" HeaderStyle-CssClass="hideGridColumn"
                                                                ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="right">
                                                    <asp:Button ID="btnSelectLog" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                                        Text="Select Log" ToolTip="Click to select the log"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table7" border="0">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" Text="Find Now"
                                                        ToolTip="Click To Find records as Searching criteria" Visible="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Due Jobs as per selected criteria : 0 Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlTopButton" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table6" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSaveTop" runat="server" CssClass="clsButton_Ajax" Text="Comply"
                                                        ToolTip="Click To Comply" Visible="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close Work Order Compliance screen" Visible="False" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlDueJob" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgDueJob" runat="server" CssClass="clsGrid" AutoGenerateColumns="False"
                                                        ToolTip="Due Job." ShowHeaderWhenEmpty="true">
                                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <HeaderTemplate>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <itemtemplate>
                                                                            <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                                                <img id="imageID-<%# Eval("ID") %>"
                                                                                    alt="Click to Show / Hide Type"
                                                                                    border="0" src="images/detail.gif" />
                                                                            </a>
                                                                        </itemtemplate>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" ClientIDMode="Static"
                                                                        Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                                </HeaderTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="LogBook" HeaderText="Assembly Info." HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Width="190px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="MaintenanceActivity" HeaderText="Maintenance Activity">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="JobDescriptionDetailWeb" HeaderText="Info" HtmlEncode="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Freq3" HeaderText="Frequency" HtmlEncode="false">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SinceNew" HeaderText="Since New" HtmlEncode="false">
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DoneAt2" HeaderText="Done At" HtmlEncode="false" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DueAsOf2" HeaderText="Due As Of" HtmlEncode="false" Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemainingTime2" HeaderText="Remaining Time" HtmlEncode="false"
                                                                Visible="false">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField Visible="False" DataField="EstimatedDate" HeaderText="Estimated Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="StartJobDate" HeaderText="Job Start Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="EndJobDate" HeaderText="Job Completion Date">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="License No." HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" BorderStyle="None"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <table style="border: 0">
                                                                        <tr>
                                                                            <td>
																				<asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="200"
																					OnTextChanged="LicenceNoChanged" ToolTip="Enter License No." AutoPostBack="true" />
																				<cc2:AutoCompleteExtender ID="txtLicenceNo_Autocomplete" runat="server" CompletionInterval="1"
																					CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
																					CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
																					Enabled="True" MinimumPrefixLength="0" ServiceMethod="GetLicenseNoList" ServicePath=""
																					EnableCaching="true" TargetControlID="txtLicenceNo" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server"
                                                                                    ImageUrl="~/images/plus1.png"
                                                                                    CommandName="EmployeeLicence"
                                                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                                                    Height="22px" Width="24px"
                                                                                    ToolTip="Click to select multiple Licence No."
                                                                                    CausesValidation="true" />
                                                                            </td>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="Label1" runat="server" Text="and More" 
                                                                                        Visible="false" CssClass="clsLabelHeader clsCursorStyle" />
                                                                                </td>
                                                                            </tr>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Label ID="lblLicenceCount" runat="server" Text="and More" 
                                                                        CssClass="clsLabelHeader clsCursorStyle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Place" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                                        Width="90px" ToolTip="Enter Place"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Actual Man Hrs." HeaderStyle-HorizontalAlign="right"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtActualManHrs" runat="server" CssClass="clsTextBoxSmall_Ajax"
                                                                        AutoPostBack="true" MaxLength="8" ToolTip="Actual Man Hours"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comply Remark" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAssemblyRemark" runat="server" CssClass="clsTextBoxMultiLine_Ajax"
                                                                        Width="200px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
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
                                                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="chkSelectLinkAll" ClientIDMode="Static"
                                                                                                    runat="server" onclick="SetCheckBox(this)"></asp:CheckBox>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("ID") %>"
                                                                                                    <%# NumeroChequeInclus(Eval("ID").ToString()) %>></input>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="MaintenanceActivityName" HeaderText="Linked with">
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
                                                                                        <asp:BoundField DataField="DirectiveNumber" SortExpression="DirectiveNo" HeaderText="Directive Number">
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
                                                                                        <asp:BoundField DataField="DoneRemark" SortExpression="Remark" HeaderText="Remark">
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
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:UpdatePanel ID="upnlNote" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table4">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server"
                                                        CssClass="clsButton_Ajax" Text="Comply"
                                                        ToolTip="Click To Comply"
                                                        Enabled="False"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server"
                                                        CssClass="clsButton_Ajax" Text="Close"
                                                        ToolTip="Click to close Work Order Compliance screen"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <div id="divSpinner">

            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                <ProgressTemplate>
                    <div class="clsAjaxLoader">
                    </div>
                    <div class="divAjaxLoader">
                        <div class="ext-el-mask-msg x-mask-loading">
                            <div class="clsLoad_ajax">
                                <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                    ImageAlign="Middle" CssClass="ajax-loader-gif" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

        </div>

        <div id="modalPopUps">

            <!-- Select SelectSelectLog popup Window -->
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummySelectLog" Text="Maintenance Activity" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
                Style="height: 100%; width: 100%;">
                <iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                    allowtransparency="true" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupSelectLog" runat="server" TargetControlID="btnDummySelectLog"
                PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameSelectLogStateComplete() {
                    $("#btnDummySelectLog").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                function OpenSelectLogWindow() {
                    try {

                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IframeSelectLog").attr("src", "wfSelectLog_Ajax.aspx?Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummySelectLog").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
                function ParentCallBackFunctionForSelectLog() {
                    var SelectLogwindow = $find("<%=mdlPopupSelectLog.ClientID %>");
                    //close Task Card Tool popup window
                    SelectLogwindow.hide();
                    //           release resources
                    $("#IframeSelectLog").attr("src", "JavaScript:''");
                    //call image button
                    $("#hdnBtnSelectLog").click();
                }
            </script>
            <!-- End-->

            <!-- Assembly Insp Maintenance Done By Employee Dialog-->
            <div style="display: none">
                <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
            </div>
            <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
                PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMaintDoneByStateComplete() {
                    $("#btnDummyMaintDoneBy").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                function AddEmployeeLicNo(MaintenanceActivityTypeID) {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?MaintTypeID=" + MaintenanceActivityTypeID + "&Type=pup");

                        if (!$.browser.msie) {
                            $("#btnDummyMaintDoneBy").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                }

            </script>

        </div>

        <script type="text/javascript">
            function ParentCallBackFunctionForMaintDoneBy() {
                var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
                //close Ass Insp Maint Done By Emp popup window
                MaintDoneBywindow.hide();
                //Free resources
                $("#IMaintDoneBy").attr("src", "JavaScript:''");
                $("#hdnBtnMaintDoneBy").click();

            }
        </script>
        <!-- End -->

        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var resetTodaysDate = 'true';
                var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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

    </form>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgDueJob tr:gt(0)").find(":checkbox").each(function () {
                    if (status == "checked") {
                        $(this).attr("checked", status);
                        SetRow($(this));
                    }
                    else {
                        $(this).removeAttr("checked");
                        SetRow($(this));
                    }

                });
            });


        });

        function SetRow(elem) {
            var status = $(elem).attr("checked");
            if (status == "checked") {
                $(elem).closest("tr").addClass('clslightColor');
            }
            else {
                $(elem).closest("tr").removeClass('clslightColor');
            }
        }

        function SetCheckBox(elem) {
            var status = $(elem).attr("checked");
            var grid = $(elem).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (status == "checked") {
                    $(this).attr("checked", status);
                    $("td", $(this).closest("tr")).addClass('clslightColor');
                }
                else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
                }

            });


        }
        function pageLoad() {
            var status;
            $("#dgDueJob tr:gt(0)").find(":checkbox").each(function () {
                status = $(this).attr("checked");
                if (status == "checked") {
                    SetRow($(this));
                }
                else {
                    //$(this).removeAttr("checked");
                    SetRow($(this));
                }

            });

        }
    </script>

</body>
</html>
