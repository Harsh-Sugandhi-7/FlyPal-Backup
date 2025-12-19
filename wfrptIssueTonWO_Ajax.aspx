<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptIssueTonWO_Ajax.aspx.vb" Inherits="Flypal.wfrptIssueTonWO_Ajax" EnableEventValidation="false"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>List of Work Order</title>
    
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <TABLE class="clstablelistout" id="Table1" border="0">
        <tr>
            <td>
                <TABLE class="clstablelistin" id="Table2" border="0">
                    <TR>
						<TD colSpan="3" class="clsFormHeader1Newstyle">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:label id="lblTitle" runat="server" CssClass="clsFormHeader"> List of Work Order</asp:label>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </TD>
					</TR>
                    <TR>
						<TD colSpan="3">
                            <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                ErrorMessage="To Date Required."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                           
                            
                        </TD>
					</TR>
                    <TR>
						<TD colSpan="2">
                            <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <TABLE id="Table5">
										<TR>
											<TD style="WIDTH: 69px">
												<span id="lblSearch" Class="clsLabel" style="width:55px;height:8px;">Search</span>
                                            </TD>
											<TD>
												<asp:dropdownlist CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbSearch" runat="server" AutoPostBack="True">
													<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
													<asp:ListItem Value="1">Date</asp:ListItem>
													<asp:ListItem Value="2">W.O.</asp:ListItem>
													<asp:ListItem Value="3">Aircraft</asp:ListItem>
													<asp:ListItem Value="4">Model</asp:ListItem>
													<asp:ListItem Value="5">Status</asp:ListItem>
													<asp:ListItem Value="6">Doc status</asp:ListItem>
													<asp:ListItem Value="7">Issue To WO Type</asp:ListItem>
												</asp:dropdownlist>
                                            </TD>
											<TD>
												<span id="L1" Class="clsLabel" style="width:20px;"></span></TD>
											<TD>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbDate" runat="server" Visible="False" AutoPostBack="True">
													<asp:ListItem Value="0">(All)</asp:ListItem>
													<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
													<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
													<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
													<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
													<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
													<asp:ListItem Value="6">Between Dates</asp:ListItem>
												</asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbWO" runat="server"  Visible="False" AutoPostBack="True"
													DataTextField="WOText" DataValueField="WOText">
													
												</asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbAircraft" runat="server" Visible="False" Height="24px"
													DataTextField="RegNo" DataValueField="ID">
													
												</asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbModel" runat="server" Visible="False" 
													DataTextField="Name" DataValueField="Name"></asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbStatus" runat="server" Visible="False" DataTextField="Name"
													DataValueField="ID"></asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbDocStatus" runat="server" Visible="False">
													<asp:ListItem Value="0">(All)</asp:ListItem>
													<asp:ListItem Value="1">Opened</asp:ListItem>
													<asp:ListItem Value="2">Submitted</asp:ListItem>
													<asp:ListItem Value="4">Canceled</asp:ListItem>
												</asp:DropDownList>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbIssueType" runat="server" Visible="False">
													<asp:ListItem Value="0">(All)</asp:ListItem>
													<asp:ListItem Value="1">Tools</asp:ListItem>
													<asp:ListItem Value="2">Spares</asp:ListItem>
												</asp:DropDownList>
                                            </TD>
											<TD vAlign="middle">
												<asp:Label id="lblNo" runat="server" CssClass="clsLabel" Visible="False" Width="32px" Height="8px">No.</asp:Label>
                                            </TD>
											<TD>
												<asp:TextBox CssClass="clsTextBoxTagSearch" id="txtNo" runat="server" ToolTip="Enter Number" Visible="False"
													Width="184px" MaxLength="4"></asp:TextBox>
                                            </TD>
											<TD>
												<asp:Label id="lblFromDate" CssClass="clsLabel" Visible="False" Width="78px" Runat="server">From Date</asp:Label></TD>
											<TD align="left">
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
                                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                    ClientValidationFunction="BetweenDatesValidation"
                                                Display="None"></asp:CustomValidator>
										    </TD>
											<TD>
												<asp:Label id="lblToDate" CssClass="clsLabel" Visible="False" Width="68px" Runat="server">To Date </asp:Label></TD>
											<TD align="left">
                                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                    runat="server" CausesValidation="true"></asp:TextBox>
                                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox">
                                                </cc2:TextBoxWatermarkExtender>
										    </TD>
										</TR>
									</TABLE>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </TD>
                        <TD align="right" > 
                            <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <TABLE id="Table6">
										<TR>
											<TD>
                                                <%--<asp:button id="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find list of Work Order as per searching criteria" 
													Text="Find Now"></asp:button>--%>
                                                <asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png" CssClass="clsSearch2btn" ToolTip="Click to find list of Work Order as  per searching criteria" />
                                            </TD>
										</TR>
									</TABLE>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                        </TD>
					</TR>
                    <TR>
						<TD colspan="3">
                            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:label id="lblResult" runat="server" CssClass="clsLabelHeader"> List of  Work Order(s)</asp:label>    
                                            </td>
                                        </tr>
                                        <TR>
								            <TD>
									            <asp:GridView id="dgWOList" runat="server"  CssClass="clsGridNewStyle" AutoGenerateColumns="False" PageSize="25"
										            AllowSorting="True" AllowPaging="True" ShowHeaderWhenEmpty="true" DataKeyNames="ID" GridLines="Horizontal" CellPadding="5">
                                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle HorizontalAlign="Right" />
										            <AlternatingRowStyle  CssClass="clsdgAltItem"></AlternatingRowStyle>
										            <RowStyle CssClass="clsdgItem"></RowStyle>
										            <HeaderStyle  CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
										            
										            <Columns>
											            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
											            <asp:BoundField DataField="WODate" HeaderText="Date">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left" ></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
												            <FooterStyle Wrap="False"></FooterStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="WONumber" SortExpression="WONumber" HeaderText="W. O. No.">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg. No.">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="ModelName" SortExpression="ModelName" HeaderText="Model">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="WOStartDate" HeaderText="Start Date">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
												            <FooterStyle Wrap="False"></FooterStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="WOBy" SortExpression="WOBy" HeaderText="Created  By ">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="DOC. Status">
												            <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="AuthorizedBy" SortExpression="AuthorizedBy" HeaderText="Submitted By">
												            <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="WOStatusName" SortExpression="WOStatusName" HeaderText="WO Status">
												            <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="WOCloseDate" HeaderText="Closing Date">
												            <HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
												            <ItemStyle Wrap="False"></ItemStyle>
												            <FooterStyle Wrap="False"></FooterStyle>
											            </asp:BoundField>
											            <asp:BoundField DataField="ClosedBy" SortExpression="ClosedBy" HeaderText="Closed By">
												            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
											            </asp:BoundField>
											            <asp:ButtonField Text="Print" HeaderText="Print" CommandName="EditRec">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
										            </Columns>
										             <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                            <PagerStyle CssClass="paging" HorizontalAlign="Right" />
									            </asp:GridView>
                                            </TD>
							            </TR>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
                            
                        </TD>
                    </TR> 
                    <TR>
					    <TD align="right" colSpan="3">
                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <TABLE id="Table4">
							            <TR>
								            <TD><asp:button CssClass="clsbtnH clsinfoH1" id="btnClose" runat="server" ToolTip="Click to close List of Work Order screen"
										            Text="Close" CausesValidation="False"></asp:button></TD>
							            </TR>
						            </TABLE>
                                </ContentTemplate>
                            </asp:UpdatePanel> 
						</TD>
				    </TR>
                </TABLE>
            </td>
        </tr>
    </TABLE>
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
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

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            
            var selectedSearchIndex = $get("cmbSearch").selectedIndex;
            if (selectedSearchIndex == 1) {
                var selectedDateIndex = $get("cmbDate").selectedIndex;
                if (selectedDateIndex == 6){
                    args.IsValid = false;
                    var fromdate = $("#txtFromDate").val();
                    var todate = $("#txtToDate").val();
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

        }

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
    </form>
</body>
</html>
