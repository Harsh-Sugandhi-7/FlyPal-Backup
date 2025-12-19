<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMultiComponentRemovalCriteria_Ajax.aspx.vb"
    MaintainScrollPositionOnPostback="false" Inherits="Flypal.wfMultiComponentRemovalCriteria_Ajax" %>

<%@ Import Namespace="Flypal" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <style type="text/css">
        .GbiHighlight
        {
            background-color: Aqua;
        }
        .WrapperDiv
        {
            border: 1px solid #CCCCCC;
            height: auto;
            max-height: 450px;
            width: 100%;
            overflow: auto;
        }
        
        .WrapperDiv TH
        {
            position: relative;
            font-size: 8pt;
            font-weight: bold;
        }
        
        .WrapperDiv TR
        {
            /*NeededForIe*/
            height: 0px;
        }
        
        
        .WrapperDiv td
        {
            font-size: 8pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //  FreezeGridViewHeader('dgInstalledList', 'WrapperDiv'); //'WrapperDiv'

            CallParentRemAutoResizeFunction();

        }
        function FreezeGridViewHeader(gridID, wrapperDivCssClass) {
            /// <summary>
            ///   Used to create a fixed GridView header and allow scrolling
            /// </summary>
            /// <param name="gridID" type="String">
            ///   Client-side ID of the GridView control
            /// </param>
            /// <param name="wrapperDivCssClass" type="String">
            ///   CSS class to be applied to the GridView's wrapper div element.  
            ///   Class MUST specify the CSS height and width properties.  
            ///   Example: width:800px;height:400px;border:1px solid black;
            /// </param>
            var grid = document.getElementById(gridID);
            if (grid != 'undefined') {
                grid.style.visibility = 'hidden';
                var div = null;
                if (grid.parentNode != 'undefined') {
                    //Find wrapper div output by GridView
                    div = grid.parentNode;
                    if (div.tagName == "DIV") {
                        div.className = wrapperDivCssClass;
                        div.style.overflow = "auto";
                        div.style.overflowX = "hidden";
                    }
                }
                //Find DOM TBODY element and remove first TR tag from 
                //it and add to a THEAD element instead so CSS styles
                //can be applied properly in both IE and FireFox
                var tags = grid.getElementsByTagName('TBODY');
                if (tags != 'undefined') {
                    var tbody = tags[0];
                    var trs = tbody.getElementsByTagName('TR');
                    var headerHeight = 8;
                    //code for fixed headers commented
                    //                    if (trs != 'undefined') {
                    //                        headerHeight += trs[0].offsetHeight;
                    //                        var headTR = tbody.removeChild(trs[0]);
                    //                        var head = document.createElement('THEAD');
                    //                        head.appendChild(headTR);
                    //                        grid.insertBefore(head, grid.firstChild);
                    //                    }
                    //Needed for Firefox
                    tbody.style.height =
                      (div.offsetHeight - headerHeight) + 'px';
                    tbody.style.overflowX = "hidden";
                    tbody.overflow = 'auto';
                    tbody.overflowX = 'hidden';
                    tbody.style.width = '100%';
                }
                grid.style.visibility = 'visible';

            }
        }
    
    </script>
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
    <table id="tblmain" style="background-color: WhiteSmoke; width: 100%">
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                            ValidationGroup="a" CssClass="clsValidationSummary"></asp:ValidationSummary>
                        <asp:CustomValidator ID="cvCustomer" runat="server" CssClass="clsLabelAuto" ErrorMessage="Select Aircraft from the list."
                            ValidationGroup="a" Display="None" ControlToValidate="cmbAircraft" OnServerValidate="CustomValidate"></asp:CustomValidator>
                        <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                            ValidationGroup="a" OnServerValidate="CustomValidate"></asp:CustomValidator>
                        <asp:CustomValidator ID="cvValidator" runat="server" CssClass="clsLabelAuto" Display="None"
                            ValidationGroup="a"></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table7">
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="lblEngineInfo" style="font-weight: bold" runat="server"><b>Search Criteria</b></legend>
                                        <table id="Table10">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblStep1" runat="server" CssClass="clsLabelHeader">Step I. Selection of Removal Date</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">Removal Date</asp:Label>
                                                </td>
                                                <td>
                                                    <table id="Table9">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtAsOnDate" CssClass="clsTextBox_Ajax" Width="100px"
                                                                    AutoPostBack="true" onchange="ValidateDateText(this,'txtAsOnDate_watermarkextender');"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtAsOnDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtAsOnDate" ID="txtAsOnDate_watermarkextender"
                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$ AppSettings:DateFormat %>"
                                                                    WatermarkCssClass="clsDateTextBox" Enabled="True">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step II. Selection of Aircraft</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsComboBox" AutoPostBack="True"
                                                        CausesValidation="true" DataTextField="RegNo" DataValueField="MachineID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblReadOnly" runat="server" CssClass="clsLabelAuto" ForeColor="Red"
                                                        Text="* Selected Aircraft is marked as ReadOnly" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">Step III. Selection of Assembly</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsComboBox3" AutoPostBack="True"
                                                        DataTextField="ModelSerialNoPostion" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step IV. Enter Part No./Serial No.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPart" runat="server" CssClass="clsLabelAuto">Part</asp:Label>
                                                </td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Part"
                                                                    AutoPostBack="true" MaxLength="50" Width="100px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSerialNo" runat="server" Width="60px" CssClass="clsLabelAuto">Serial No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Serial Number"
                                                                    AutoPostBack="true" MaxLength="50" Width="120px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left" style="width: 100%">
                            <asp:UpdatePanel ID="upnlValues" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table6">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCurrentValues" runat="server" CssClass="clsLabelHeader">Assembly Values</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="dgDoneOnValue" runat="server" CssClass="clsGrid" DataKeyNames="ID"
                                                    ShowHeaderWhenEmpty="true" EnableViewState="true" AllowSorting="True" AllowPaging="True"
                                                    AutoGenerateColumns="False" PageSize="5">
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssemblyCurrentValueFormatted" HeaderText="Values">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSelectLog" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to select the log"
                                                    ValidationGroup="a" Text="Select Log"></asp:Button>
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
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpnlInstalledCompList" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblInstalledComponents" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlRemoveEntry" runat="server" UpdateMode="Conditional" Visible="false">
                                        <ContentTemplate>
                                            <fieldset id="Fieldset2" class="clsFieldSet" style="border-width: 1px">
                                                <legend id="Legend1" style="font-weight: bold" runat="server"><b>Place
                                                    Details for Removal</b></legend>
                                                <table>
                                                    <tr>
                                                        <%--<td align="left">
                                                            <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Work Order No."></asp:TextBox>
                                                        </td>--%>
                                                        <td align="left">
                                                            <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Place"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upnlCheckedSelection" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblSelection" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgChecked" runat="server" ImageUrl="~/images/MyCart.png" Visible="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpnldgInstalledCompList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="gvholder" style="height: 400px; overflow: auto; display: none">
                                                <asp:GridView ID="dgInstalledList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    EnableViewState="true" CssClass="clsGrid" PageSize="5" ShowHeaderWhenEmpty="True">
                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                                <input type="checkbox" id="chkSelectAll" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="checkbox" id="chkCheckSelect" name="chkSelect" class="cbSelectRow" value="<%# Eval("CompStatusID") %>"
                                                                    onchange="CheckChange();" <%# NumeroChequeInclus(Eval("CompStatusID").ToString()) %>>
                                                                </input>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CompStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                        <%-- <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="false" />
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="true" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                            </asp:BoundField>--%>
                                                        <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" Wrap="True" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CompInfo" HeaderText="Comp Info" SortExpression="CompInfo"
                                                            HtmlEncode="false">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" HtmlEncode="false">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ValueFormatted" HeaderText="Value" HtmlEncode="false">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TSNFormatted" HeaderText="TSN" HtmlEncode="false">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TSOFormatted" HeaderText="TSO" HtmlEncode="false">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Reason" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left" BorderStyle="None"></ItemStyle>
                                                            <ItemTemplate>
                                                                <table style="border-style: none">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlReasonValidate" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:RequiredFieldValidator ID="rfReason" runat="server" ErrorMessage="Role Required"
                                                                                        ControlToValidate="txtReason" InitialValue="-1" ValidationGroup='<%# string.Format("Group_{0}", Eval("CompStatusID")) %>'
                                                                                        Display="dynamic" SetFocusOnError="true" Text="Please Select Reason" ForeColor="Red"
                                                                                        Font-Italic="true" CssClass="clsLabel">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                            <%--<asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox1_Ajax" 
                                                                                                      ClientIDMode="Static">
                                                                                    </asp:DropDownList>--%>
                                                                            <asp:TextBox ID="txtReason" runat="server" CssClass="clsTextBox_Ajax" MaxLength="200"
                                                                                Width="185px" ToolTip="Enter Reason."></asp:TextBox>
                                                                            <cc2:AutoCompleteExtender ID="txtReason_Autocomplete" runat="server" CompletionInterval="1"
                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetReasonList" ServicePath=""
                                                                                EnableCaching="true" TargetControlID="txtReason">
                                                                            </cc2:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnReason" runat="server" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnReason" runat="server" ImageUrl="~/images/plus1.png" CommandName="Reason"
                                                                                CommandArgument='<%# Container.DataItemIndex %>' Height="22px" Width="24px" ToolTip="Click to Add new removal Reasons."
                                                                                CausesValidation="true" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="License No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle Wrap="True" HorizontalAlign="Left" BorderStyle="None"></ItemStyle>
                                                            <ItemTemplate>
                                                                <table style="border-style: none">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlLicenceValidate" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:RequiredFieldValidator ID="rfLicenceValidator" runat="server" ErrorMessage="d"
                                                                                        ControlToValidate="txtReason" InitialValue="-1" ValidationGroup='<%# string.Format("Group_{0}", Eval("CompStatusID")) %>'
                                                                                        Display="dynamic" SetFocusOnError="true" Text="Please Select Reason" ForeColor="Red"
                                                                                        Font-Italic="true" CssClass="clsLabel">
                                                                                    </asp:RequiredFieldValidator>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                            <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" MaxLength="200"
                                                                                AutoPostBack="true" OnTextChanged="txtLicenceNo_TextChanged" ToolTip="Enter License No."></asp:TextBox>
                                                                            <cc2:AutoCompleteExtender ID="txtLicenceNo_Autocomplete" runat="server" CompletionInterval="1"
                                                                                CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" DelimiterCharacters=""
                                                                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetLicenseNoList" ServicePath=""
                                                                                TargetControlID="txtLicenceNo">
                                                                            </cc2:AutoCompleteExtender>
                                                                            <asp:HiddenField ID="hdnLicenceNo" runat="server" ClientIDMode="Static" />
                                                                            <asp:HiddenField ID="hdnLicenseEmpNo" runat="server" ClientIDMode="Static" />
                                                                            <asp:HiddenField ID="hdnEmployeeID" runat="server" ClientIDMode="Static" />
                                                                            <asp:HiddenField ID="hdnEmployeeName" runat="server" ClientIDMode="Static" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                CommandName="EmployeeLicence" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                Height="22px" Width="24px" ToolTip="Click to select multiple Licence No." CausesValidation="true" />
                                                                        </td>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:UpdatePanel ID="upnlLicenceCount" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblLicenceCount" Visible="false" runat="server" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Work Order No.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBox_Ajax"
                                                                             MaxLength="149"> </asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
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
                                                        <asp:Button ID="btnRemove" runat="server" CssClass="clsButton" ToolTip="Click to Remove multiple Components in one click"
                                                            ValidationGroup="a" Text="Remove" Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnCheckValues" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                            Style="display: none;" Text="Add" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to close the Multi Removal Components screen"
                                                            CausesValidation="False" Text="Close"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
        <!--Dummy panel to open modelpopup-->
        <tr>
            <td align="right">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:Button ID="hdnBtnRemovalReason" ClientIDMode="Static" runat="server" Text="Add"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <!--End -->
    </table>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" ClientIDMode="Static" DynamicLayout="false"
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
        X="100" Y="100" PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
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
                //                $('html,body').animate({ scrollTop: $(this.hash).offset().top }, 500);
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
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForComp();
            return false;
        }
    </script>
    <!-- End-->
    <!-- Removal Reason Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemovalReason" Text="Removal Reason" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemovalReason" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemovalReason" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemovalReason" runat="server" TargetControlID="btnDummyRemovalReason"
        PopupControlID="pnlRemovalReason" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemovalReasonStateComplete() {
            $("#btnDummyRemovalReason").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemovalReasonWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemovalReason").attr("src", "wfRemovalReason_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemovalReason").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemovalReason() {
            var RemovalReasonwindow = $find("<%=mdlPopupRemovalReason.ClientID %>");
            //close Removal Reason popup window
            RemovalReasonwindow.hide();
            //           release resources
            $("#IframeRemovalReason").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemovalReason").click();
        }
    </script>
    <!-- End-->
    <!-- hidden fields to set combobox selected values at client side -->
    <asp:HiddenField ID="hdnReasonIDValueList" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnReasonNameValueList" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnReasonList" runat="server" ClientIDMode="Static" />
    <script type="text/javascript">
        //        function setIDs() {
        //            var ReasonIDList = new Array();
        //            var ReasonNameList = new Array();
        //            var ReasonList = [];
        //            var myData = '';
        //            //            var myData = new Object();
        //            $("#dgInstalledList tr").each(function () {
        //                var checkBox = $(this).find("[id*=chkCheckSelect]");
        //                var cmbReason = $(this).find("[id*=cmbReason]");
        //                if ($(checkBox).is(':checked')) {
        //                    var myobj = new Object();
        //                    myobj.CompStatusID = $(":checked", this).val();
        //                    myobj.ReasonID = $(":selected", cmbReason).val();
        //                    myData = myData + JSON.stringify(myobj)
        //                    // ReasonList.push([$(":checked", this).val(), ID]);

        //                }

        //            });
        //            $("#hdnReasonList").val('');
        //            $("#hdnReasonList").val(myData);
        //        }
        //        function CheckReasonSelection() {
        //            $("#dgInstalledList tr").each(function () {
        //                var txtReason = $(this).find("[id*=txtReason]");
        //                var txtLicenceNo = $(this).find("[id*=txtLicenceNo]");
        //                var checkBox = $(this).find("[id*=chkCheckSelect]");
        //                var hdnReason = $(this).find("[id*=hdnReason]");
        //                var hdnLicenseEmpNo = $(this).find("[id*=hdnLicenseEmpNo]");

        //////                if ($(checkBox).is(':checked') & $(":selected", cmbReason).text() != "") {
        //                if ($(checkBox).is(':checked')  {
        //                    //                    $("#hdnReason").val(myData);
        //                    document.getElementById('hdnReason').value = document.getElementById('txtReason').value
        //                    document.getElementById('hdnLicenseEmpNo').value = document.getElementById('txtLicenceNo').value
        //                }
        //            });
        //        }
     
    </script>
    <!-- Done By Employee Dialog-->
    <div style="display: none">
        <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
    </div>
    <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%;
        width: 100%;">
        <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
        X="90" PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMaintDoneByStateComplete() {
            $("#btnDummyMaintDoneBy").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }


        function AddEmployeeLicNo() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=4");

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
    <script type="text/javascript">
        function SetLicenceNo(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtLicenceNo_Autocomplete") {
                textbox = document.getElementById('hdnLicenceNo');
            }


            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
    </script>
    <%--autocomplete css functions--%>
    <script type="text/javascript">
        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) {
                    //$(this).addClass("ac_even");
                }
                else {
                    $(this).addClass("ac_odd");
                }
            });
        }
        //add loader to textbox
        function ClientPopulating(source, e) {
            $("#" + source._element.id).addClass("ac_loading");
        }
        //remove loader from textbox
        function ClientHiding(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }
    </script>
    <%--End--%>
    </form>
    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            if ("<%= page.IsPostback%>" == "False") {
                $(".clstooltip").closest("tr").mousemove(function (event) {
                    $(this).find(".clstooltip").css({
                        "left": event.pageX + 1,
                        "top": event.pageY + 1
                    }).show();
                }).mouseout(function () { $(this).find(".clstooltip").hide(); }); ;
            }
        });
    </script>
    <%--Called parent function to open Tank master page--%>
    <script type="text/javascript">
        function CallParentRemAutoResizeFunction() {
            //            onLoad();
            gvholder.style.display = "block";
            window.parent.autoResizeRemovalComp();

        }
        function OnDateChange() {
            //            onLoad();
            gvholder.style.display = "none";
            window.parent.autoResizeRemovalComp();

        }
        function CheckChange() {
            ////            $("#hdnBtnCheckValues").click();
            var count = 0;
            var allcount = 0;
            var allcheckall = $(this).find("[id*=chkSelectAll]").is(':checked');
            $("#dgInstalledList tr").each(function () {
                var checkBox = $(this).find("[id*=chkCheckSelect]");

                if ($(checkBox).is(':checked')) {
                    count++;
                }

            });
            //  $('#lblSelection').text = count.tostring();


            $('#<%=lblSelection.ClientID%>').text(count);
            if (count == 0) {
                $('#<%=btnRemove.ClientID%>').hide();
                $('#<%=imgChecked.ClientID%>').hide();
                $('#<%=lblSelection.ClientID%>').hide();

            } else {
                $('#<%=btnRemove.ClientID%>').show();
                $('#<%=imgChecked.ClientID%>').show();
                $('#<%=lblSelection.ClientID%>').show();
            }

        }
    
     
    </script>
    <script type="text/javascript">
        function SetCheckBox(checkids) {
            var allVals = checkids.split(',');
            var count = 0;
            $("#dgInstalledList tr").each(function () {
                var checkBox = $(this).find("[id*=chkCheckSelect]");
                if (jQuery.inArray(checkBox.val(), allVals) < 0) {

                    checkBox.attr('checked', false);
                }
                else {
                    checkBox.attr('checked', true);
                    count++;
                }
            });
            $('#<%=lblSelection.ClientID%>').text(count);
            if (count == 0) {
                $('#<%=btnRemove.ClientID%>').hide();
                $('#<%=imgChecked.ClientID%>').hide();
                $('#<%=lblSelection.ClientID%>').hide();

            } else {
                $('#<%=btnRemove.ClientID%>').show();
                $('#<%=imgChecked.ClientID%>').show();
                $('#<%=lblSelection.ClientID%>').show();
            }
            CallParentRemAutoResizeFunction();
        }
    
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            $('.cbSelectRow').change(function () {
                // detect if the checkbox is checked
                var checked = $(this).prop('checked');
                // gets the table row indiect parent
                var trParent = $(this).parents('tr');
                // add or remove the css class according to the check state
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor')
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            })
            // the each is used when postback is triggered with checked rows
            .each(function (index, element) {
                var checked = $(element).prop('checked');
                if (checked == true)
                    $("td", $(this).closest("tr")).addClass('clslightColor');
                else
                    $("td", $(this).closest("tr")).removeClass('clslightColor');
            });
            // select all click
            $("#chkSelectAll").change(function () {
                var checked = $(this).prop('checked');
                //                $get("AjaxLoader").style.visibility = 'visible';
                $('.cbSelectRow').prop('checked', checked).trigger('change');
                CheckChange();
            });
        });
    </script>
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var resetTodaysDate = 'false';
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
</body>
</html>
