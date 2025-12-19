<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCrewTimeLogRegister_Ajax.aspx.vb" Inherits="Flypal.wfrptCrewTimeLogRegister_Ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Crew Time Register</title>
    
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
</head>
<body bottomMargin="5" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout">
    <form id="wfgroup" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout">
	    <tr>
		    <td><asp:panel id="pnlmain" CssClass="clspanel1" Runat="server">
				    <TABLE id="tblInner" class="clstablelistin">
                        <TR>
                            <td colspan="5" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <span id="lbltitle" class="clsFormHeader">Search Criteria For Crew Time Register</span>
                                        </td>
                                        <%--<td colspan="5" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnCurrentSearchCriteria" runat="server"
                                                                    Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias"></asp:Button></td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnDisplay" runat="server" Text="Display"
                                                                    ToolTip="Click to Display Report"></asp:Button></td>
                                                            <td>
                                                                <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close" CausesValidation="False"
                                                                    ToolTip="Click to Close Search Criteria For Crew Log Book screen"></asp:Button></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </td>--%>
                                    </tr>
                                </table>

                            </td>
					    </TR>
					    <TR>
						    <TD colSpan="5">
							    <asp:ValidationSummary id="Validationsummary2" Runat="server" HeaderText="Fill Up The Following Fields"
								    Cssclass="clsValidationSummary"></asp:ValidationSummary>
							    
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation"
                                    Display="None"></asp:CustomValidator>
                            </TD>
					    </TR>
                        <TR>
						    <TD colSpan="5">
							    <span id="lblStep1" Class="clsLabelHeader">Step I. Selection of Dates</span>
                            </TD>
					    </TR>
					    <TR>
                            <td width="12px"></td>
						    <TD>
							    <span id="lblFromDate" Class="clsLabelAuto">From Date</span></TD>
						    <TD>
							    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                    runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                            </TD>
						    <TD>
							    <span id="lblToDate" Class="clsLabelAuto">To Date</span>
                            </TD>
						    <TD>
                                <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;" 
                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                    runat="server" CausesValidation="true"></asp:TextBox>
                                <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
							</TD>
					    </TR>
					    <TR>
						    <TD colSpan="5" align="left">
							    <span id="lblStep2" Class="clsLabelHeader">Step II. Selection of Aircraft</span>
                            </TD>
					    </TR>
					    <TR>
                            <td width="12px"></td>
						    <TD align="left">
							    <span id="lblAircraft" Class="clsLabelAuto">Aircraft </span>
                            </TD>
						    <TD colSpan="3" align="left">
							    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" id="cmbAircraft" runat="server" DataValueField="ID" DataTextField="RegNo"></asp:DropDownList></TD>
					    </TR>
					    <TR>
						    <TD colSpan="5" align="left">
							    <span id="Label1" Class="clsLabelHeader">Step III. Selection of Crew</span>
                            </TD>
					    </TR>
					    <TR>
                            <td width="12px"></td>
						    <TD align="left">
							    <span id="lblCrew" Class="clsLabelAuto">Crew</span>
                            </TD>
						    <TD colSpan="3" align="left">
							    <asp:TextBox CssClass="clsTextBoxTagSearch"  id="txtSearch" runat="server" ClientIDMode="Static" onChange="setCrewID(this,'txtSearch_AutocompleteExtender')"></asp:TextBox>
                            </TD>
                            <cc2:AutoCompleteExtender ID="txtSearch_AutocompleteExtender" runat="server" ClientIDMode="Static"  DelimiterCharacters=""
                            EnableCaching="false" CompletionInterval="1" CompletionListCssClass="ac_results_Main" CompletionListHighlightedItemCssClass="ac_over_Main"
                            CompletionListItemCssClass="ac_results_li" CompletionSetCount="20" UseContextKey="false"  ContextKey="" 
                            Enabled="true" MinimumPrefixLength="0" ServicePath="" ServiceMethod="GetCrewListAutoComplete" TargetControlID="txtSearch" OnClientItemSelected="setID">
                            </cc2:AutoCompleteExtender>
					    </TR>
						<TR>
						    <TD colSpan="5" align="left">
							    <span id="lblStep4" Class="clsLabelHeader">Step VII. Display Report</span>
                            </TD>
					    </TR>
					    <TR>
                            <td width="12px"></td>
						    <TD colSpan="4" align="left">
							    <span id="lblSummary" Class="clsLabelAuto">Your selection is as follows </span>
                            </TD>
					    </TR>
                        <tr>
                            <td colSpan="5">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <TR>
						                        <TD align="left" width="12px"></TD>
						                        <TD align="left">
							                        <asp:label id="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label>
							                        <asp:label id="lblDateRangeTo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
					                        </TR>
					                        <TR>
						                        <TD align="left" width="12px"></TD>
						                        <TD align="left">
							                        <asp:label id="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
					                        </TR>
					                        <TR>
						                        <TD align="left" width="12px"></TD>
						                        <TD align="left">
							                        <asp:label id="lblPilot1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:label></TD>
					                        </TR>    
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
					    <TR>
						    <TD colSpan="5" align="right">
							    <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <TABLE cellSpacing="0">
									        <TR>
										        <TD>
											        <asp:button CssClass="clsbtnH clsinfoH1" id="btnCurrentSearchCriteria" runat="server" 
												        Text="Current Criteria" CausesValidation="False" ToolTip="Click to Display Current Searching criterias"></asp:button></TD>
										        <TD>
											        <asp:button CssClass="clsbtnH clsinfoH1" id="btnDisplay" runat="server"  Text="Display"
												        ToolTip="Click to Display Report"></asp:button></TD>
										        <TD>
											        <asp:button CssClass="clsbtnH clsinfoH1" id="btnClose" runat="server"  Text="Close" CausesValidation="False"
												        ToolTip="Click to Close"></asp:button></TD>
									        </TR>
								        </TABLE>    
                                    </ContentTemplate>
                                </asp:UpdatePanel>
								
                            </TD>
					    </TR>
				    </TABLE>
			    </asp:panel></td>
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
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="SelectedCrewID" />
   
    <%--<script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoEmpNoName.aspx?', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>--%>
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
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
    <%-- Autocomplete functions to set id--%>
    <script type="text/javascript">
        function setID(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }
            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtSearch_AutocompleteExtender") {
                textbox = document.getElementById('SelectedCrewID');
            }
            

            textbox.value = value;
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
        function setCrewID(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "txtSearch_AutocompleteExtender") {
                        textbox = document.getElementById('SelectedCrewID');
                    }
                   
                    textbox.value = val;
                    return;
                }

            }

            if (extenderid == "txtSearch_AutocompleteExtender") {
                document.getElementById('SelectedCrewID').value = '';
            }
            


        }
        
    </script>
    </form>
</body>
</html>
