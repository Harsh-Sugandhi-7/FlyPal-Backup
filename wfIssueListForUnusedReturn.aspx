<%@ Register TagPrefix="obout" Namespace="OboutInc.Calendar" Assembly="obout_Calendar_Pro_Net" %>
<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTlkkt" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfIssueListForUnusedReturn.aspx.vb"
    Inherits="Flypal.wfIssueListForUnusedReturn" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Issue List</title>   
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

    <style type="text/css">

        #lblTitle{
            display: block;
            min-width: 600px;  
        }

		#bottom,
        #top{
			display: none !important;
		}

    </style>

</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="frmIssueList" method="post" runat="server">

	    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
		    EnablePageMethods="true">
	    </asp:ScriptManager>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
											    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
                                                    List Of Issue
											    </asp:Label>
                                            </td>
                                            <td align="right">
											    <asp:Button ID="BtnPrint" runat="server" 
                                                    CssClass="clsbtnH clsinfoH" ToolTip="Click to print list of Issues"
												    Text="Print" CausesValidation="False">
											    </asp:Button>
											    <asp:Button ID="btnClose" runat="server" 
                                                    CssClass="clsbtnH clsinfoH" ToolTip="Click to close List of Issue screen"
												    Text="Close" CausesValidation="False">
											    </asp:Button>
                                            </td>
                                        </tr>
                                    </table>                                
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary" runat="server" 
                                        HeaderText="Fill Up The Following Information" CssClass="clsValidationSummary">
                                    </asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table1">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel" Width="55px" Height="8px">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
                                                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                    <asp:ListItem Value="1">Date</asp:ListItem>
                                                    <asp:ListItem Value="2">Issue</asp:ListItem>
                                                    <asp:ListItem Value="3">Part Number</asp:ListItem>
                                                    <asp:ListItem Value="4">Aircraft</asp:ListItem>
                                                    <asp:ListItem Value="5">Serial No.</asp:ListItem>
                                                    <asp:ListItem Value="6">WorkShop</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbDate" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                    Visible="False">
                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                    <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                    <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                    <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                    <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                    <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                    <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cmbIssueText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                    Visible="False" DataTextField="Text" DataValueField="Text">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch"
													Visible="False" MaxLength="100" Height="25px">
                                                </asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <asp:Label ID="lblNo" runat="server" CssClass="clsLabel" Width="32px" Height="8px"
                                                    Visible="False">No.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNo" runat="server" Height="25px" Width="110px"
													CssClass="clsTextBoxTagSearch" Visible="False" MaxLength="10">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
														    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Visible="False">From Date </asp:Label>
                                                        </td>
                                                        <td>
														    <asp:TextBox runat="server" ID="FromDate_Txt" CssClass="clsTextBoxTagSearchDate"
															    Width="100px" Height="25px" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
														    </asp:TextBox>
															<ajaxTlkkt:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"
																CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
																TargetControlID="FromDate_Txt">
															</ajaxTlkkt:CalendarExtender>
															<ajaxTlkkt:TextBoxWatermarkExtender TargetControlID="FromDate_Txt"
																ID="FromDate_watermarkextender" WatermarkCssClass="clsDateTextBox"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
															</ajaxTlkkt:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabel" Visible="False">To Date </asp:Label>
                                                        </td>
                                                        <td>
														    <asp:TextBox runat="server" ID="ToDate_Txt" CssClass="clsTextBoxTagSearchDate"
																Width="100px" Height="25px" onchange="ValidateDateText(this,'ToDate_WatermarkExtender');">
														    </asp:TextBox>
														    <ajaxTlkkt:CalendarExtender ID="ToDate_CalendarExtender" runat="server"
															    CssClass="cal_Theme1" Enabled="true" Format="<%$AppSettings:DateFormat%>"
															    TargetControlID="ToDate_Txt"></ajaxTlkkt:CalendarExtender>
														    <ajaxTlkkt:TextBoxWatermarkExtender TargetControlID="ToDate_Txt"
																ID="ToDate_WatermarkExtender" ClientIDMode="Static" runat="server"
																WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox">
														    </ajaxTlkkt:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Visible="false">
                                        Select Issue from the list. Click On Edit Link To Modify The Selected Issue. 
                                        Click On Delete Link To Delete The Selected Issue. Click On Add New button To Add A New Issue.
                                    </asp:Label>
                                </td>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
											    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
												    ToolTip="Click to search as per searching Criteria."
												    ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
                                        List of Issue as per criteria :  Record(s) found.
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:GridView ID="gvIssueList" runat="server" AllowSorting="True"
									    ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
									    CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
									    <AlternatingRowStyle cssclass="clsdgAltItem" />
									    <rowstyle cssclass="clsdgItem" />
									    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
									    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
									    <pagersettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" />
									    <PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" Height="50px"/>
                                        <Columns>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField DataField="ILDateFormatted" HeaderText="Date">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IssueNo" SortExpression="IssueNo" HeaderText="Number">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Issue Type">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Issue To">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AuthorizedByName" SortExpression="AuthorizedByName" HeaderText="Authorized By ">
                                                <HeaderStyle Wrap="False" ></HeaderStyle>
                                            </asp:BoundField>
										    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action"
											    ItemStyle-HorizontalAlign="Center">
											    <HeaderStyle HorizontalAlign="Center" />
											    <ItemStyle HorizontalAlign="Center" />
											    <ItemTemplate>
												    <div id="dropDownImg" class="dropdown">
													    <asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
														    CssClass="clsActionbtn" />
													    <div id="dropdownICN-content" class="dropdownbtn-content">
														    <table id="dropdown-content" class="clsGridNew_Ajax">
															    <tr>
																    <td>
																	    <asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																		    CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																		    ToolTip="Click to Edit record"
																		    CommandName="EditRecord" ImageUrl="~/images/edit.png" />
																    </td>
															    </tr>
														    </table>
													    </div>
												    </div>
											    </ItemTemplate>
										    </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>

		<!-- Ajax Loader -->
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

		<script type="text/javascript" id="dateValidation">

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
