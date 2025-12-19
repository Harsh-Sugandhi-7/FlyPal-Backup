<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfInstalledCompList_AJAX.aspx.vb"
    Inherits="Flypal.wfInstalledCompList_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Installed Component List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
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
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .aspNetDisabled
        {
            color: Black !important;
            width: auto !important;
        }
        .cssLabelDisplay
        {
            display: none;
        }
    </style>
    <script src="JQUIModalDialog/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JQUIModalDialog/jquery-ui.min.js" type="text/javascript"></script>
    <link href="JQUIModalDialog/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        // Builds the HTML Table out of myList json data from Ivy restful service.
        function buildHtmlTable(JsonString) {
            var row$ = '';
            var columns = addAllColumnHeaders();
            var arrData = JSON.parse(JsonString);
            row$ = $('<tr/>');

            for (var i = 0; i < arrData.length; i++) {
                var row = "";

                //2nd loop will extract each column and convert it in string comma-seprated
                for (var index in arrData[i]) {
                    if (index == "TextFormatted" || index == "TSNFormatted" || index == "TSOFormatted") {

                        row$.append($('<td nowrap/>').html(arrData[i][index]));
                    }
                }
                //row$.slice(0, row.length - 1);
            }
            $("#dialogbox").append(row$);
        }

        // Adds a header row to the table and returns the set of columns.
        // Need to do union of keys from all records as some records may not contain
        // all records
        function addAllColumnHeaders() {
            var columnSet = [];
            var headerTr$ = $('<tr/>');
            var myList = new Array("Installed On Values", "TSN", "TSO");
            for (var i = 0; i < myList.length; i++) {
                columnSet.push(myList[i]);
                headerTr$.append($('<th nowrap/>').html(myList[i]));
            }
            $("#dialogbox").append(headerTr$);

            return columnSet;
        }
        $(function () {
            $("#dialogbox").dialog({
                modal: true,
                autoOpen: false,
                title: "Component Values",
                width: 'auto', // overcomes width:'auto' and maxWidth bug
                maxWidth: 900,
                height: 'auto',
                fluid: true, //new option
                show: {
                    effect: "blind",
                    duration: 500
                },
                hide: {
                    effect: "explode",
                    duration: 500
                },
                closeOnEscape: true,
                draggable: false,
                resizable: false

            });

        });
        function ShowValues(JsonString, CompInfo) {
            var top = $(this).offset();
            $('#dialogbox tr').remove();
            buildHtmlTable(JsonString);
            $("#dialogbox").dialog("open");
            $("#dialogbox").dialog("option", "title", "Values For " + CompInfo);
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="frmgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" TabIndex="1" runat="server" CssClass="clstitle1">Component Removal</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td colspan="1" valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="lblRemovalInfo" runat="server"><b>Removal Information</b></legend>
                                                        <table id="Table3">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRemovalDate" runat="server" Width="94px" CssClass="clsLabelAuto">Removal Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="calRemovalDate" CssClass="clsTextBox_Ajax" Width="90px"
                                                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"
                                                                        AutoPostBack="true"></asp:TextBox>
                                                                    <cc2:CalendarExtender ID="calRemovalDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calRemovalDate">
                                                                    </cc2:CalendarExtender>
                                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calRemovalDate" ID="FromDate_watermarkextender"
                                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                        WatermarkCssClass="clsDateTextBox">
                                                                    </cc2:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <fieldset id="fdswodetail" class="clsFieldSet" style="border-width: 1px">
                                                        <legend id="ldwodetail" runat="server"><b>Search Criteria</b></legend>
                                                        <table id="Table2" width="100%">
                                                            <tr>
                                                                <asp:PlaceHolder ID="placeHolder1" runat="server">
                                                                    <td>
                                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                            DataTextField="RegNo" Width="100px" TabIndex="2" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </asp:PlaceHolder>
                                                                <td>
                                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                                        DataTextField="ModelSerialNo" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td style="padding-left: 4px" colspan="3">
                                                                    <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                                        Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPart" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Part"
                                                                        MaxLength="50" Width="120px" TabIndex="4"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" Width="60px" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial Number"
                                                                        MaxLength="50" Width="120px" TabIndex="5"></asp:TextBox>
                                                                </td>
                                                                <td align="right" colspan="1">
                                                                    <table id="Table4" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="btnFindNow" TabIndex="6" runat="server" CssClass="clsButton_Ajax"
                                                                                            ToolTip="Click to find list of Component as per searching criteria" Text="Find Now">
                                                                                        </asp:Button>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <span class="clsLabelHeader">Note : After change in search criteria,Click on Find Now
                                                                        button to get respective component's.</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlInstalledCompHeader" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblInstalledComponents" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkInstCompShowAllRecordsTop" runat="server" CssClass="clsLinkButton"
                                                                    Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnBackTop" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                Text="Close" ToolTip="Click to close List of Installed Component screen" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpnlInstalledCompList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgInstalledList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" DataKeyNames="ID" PageSize="5" ShowHeaderWhenEmpty="True"
                                                        OnRowDataBound="dgInstalledList_RowDataBound" TabIndex="7">
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompInfo" HeaderText="Comp Info" SortExpression="CompInfo"
                                                                HtmlEncode="False">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Inst. Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInstValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkInstValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSOValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkTSOValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSN" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSNValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkTSNValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="RemoveRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="RemoveRec" Style="height: 20px; width: 17px" ImageUrl="~/images/remove.jpg" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Show Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ShowVal" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="ShowVal" Style="height: 20px; width: 17px" ImageUrl="~/images/ShowValue2.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkInstCompShowAllRecords" runat="server" CssClass="clsLinkButton"
                                                        Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlPrintInstalledCompList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table1">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnPrintInstalled" runat="server" CssClass="clsButton_Ajax" Enabled="False"
                                                        Visible="false" TabIndex="9" Text="Print" ToolTip="Click to print List of Installed Component" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="upnlRemovedCompHeader" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRemovedComponents" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkRemCompShowAllRecordsTop" runat="server" CssClass="clsLinkButton"
                                                        Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlRemovedCompList" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="dgRemovedList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                        CssClass="clsGrid" DataKeyNames="ID" PageSize="5" ShowHeaderWhenEmpty="True"
                                                        TabIndex="8" OnRowDataBound="dgRemovedList_RowDataBound">
                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                        <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                        <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                        <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                            <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info" SortExpression="AssemblyInfo">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CompInfo" HeaderText="Comp Info" SortExpression="CompInfo"
                                                                HtmlEncode="False">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                                <ItemStyle Width="180px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On" HtmlEncode="False">
                                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Values" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemTSOValues" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                    <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lnkRemTSOValue" CommandName="ShowVal"
                                                                        runat="server" Text="View Values" ToolTip="Click to view Component Values"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Revert" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="RevertRemoval" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        Style="height: 19px; width: 20px" ToolTip="Click for Revert Removal of Component"
                                                                        CommandName="RevertRemoval" ImageUrl="~/images/Revert.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="History" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="History" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                        CommandName="History" ImageUrl="~/images/History.png" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
                                                                        Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsRemAttachmentAdded")%>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkRemCompShowAllRecords" runat="server" CssClass="clsLinkButton"
                                                        Visible="<%$AppSettings:IsShowAllRecordsVisible%>" ForeColor="Red" Text="Show All Records"></asp:LinkButton>
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
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnPrintRemoved" runat="server" CssClass="clsButton_Ajax" Enabled="False"
                                                        Visible="false" TabIndex="10" Text="Print" ToolTip="Click to Print List of Removed Component" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="11" Text="Close" ToolTip="Click to close List of Installed Component screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnRemHistory" ClientIDMode="Static" runat="server" Text="Add"
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
    <table id="dialogbox" cellpadding="0" cellspacing="0" border="1">
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
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
    <!-- Removal History Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemHistory" Text="TaskCard Tool" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemHistory" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemHistory" runat="server" TargetControlID="btnDummyRemHistory"
        PopupControlID="pnlRemHistory" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemHistoryStateComplete() {
            $("#btnDummyRemHistory").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemHistoryWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemHistory").attr("src", "wfUpdateRemovedCompHistory_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemHistory").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemHistory() {
            var RemHistorywindow = $find("<%=mdlPopupRemHistory.ClientID %>");
            //close Removal History popup window
            RemHistorywindow.hide();
            //           release resources
            $("#IframeRemHistory").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemHistory").click();
        }
    </script>
    <!-- End-->
    <%--<script type="text/javascript">
        $(document).ready(function () {
            $("#dialogbox").dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                    "Confirm": function () {
                        alert("You have confirmed!");
                    },
                    "Cancel": function () {
                        $(this).dialog("close");
                    }
                }
            });
        });
       
    </script>--%>
    </form>
</body>
</html>
