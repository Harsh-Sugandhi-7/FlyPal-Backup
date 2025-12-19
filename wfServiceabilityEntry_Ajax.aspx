<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfServiceabilityEntry_Ajax.aspx.vb"
    Inherits="Flypal.wfServiceabilityEntry_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html>
<head id="Head1" runat="server">
    <title>Task Card List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table1" class="clstablelistout" border="0" cellspacing="1" cellpadding="1"
        width="100%">
        <tr>
            <td>
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                    <ContentTemplate>
                        <table id="Table2" class="clstablelistin" border="0" cellspacing="1" cellpadding="1">
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblTaskCardList" runat="server" CssClass="clstitle1"> Serviceable Entry</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="Validationsummary1" Width="100%" HeaderText="Fill Up The Following Fields"
                                        runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
                                    <asp:CustomValidator ID="cvBrokenRules" runat="server" Display="None" ControlToValidate="cmbMonthList"></asp:CustomValidator>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto">Year</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="cmbYearList" runat="server" CssClass="clsComboBoxSmall" AutoPostBack="True">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Month</asp:Label>
                                    <asp:DropDownList ID="cmbMonthList" runat="server" CssClass="clsComboBoxSmall" AutoPostBack="True"
                                        Width="88px">
                                        <asp:ListItem Value="1">January</asp:ListItem>
                                        <asp:ListItem Value="2">February</asp:ListItem>
                                        <asp:ListItem Value="3">March</asp:ListItem>
                                        <asp:ListItem Value="4">April</asp:ListItem>
                                        <asp:ListItem Value="5">May</asp:ListItem>
                                        <asp:ListItem Value="6">June</asp:ListItem>
                                        <asp:ListItem Value="7">July</asp:ListItem>
                                        <asp:ListItem Value="8">August</asp:ListItem>
                                        <asp:ListItem Value="9">September</asp:ListItem>
                                        <asp:ListItem Value="10">October</asp:ListItem>
                                        <asp:ListItem Value="11">November</asp:ListItem>
                                        <asp:ListItem Value="12">December</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- 'Added by Shital on 1-Sep-2016--%>
                                    &nbsp;<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Day</asp:Label>
                                    <asp:DropDownList ID="cmbDateList" runat="server" Width="50px" CssClass="clsComboBoxsmall">
                                    </asp:DropDownList>
                                    <%-- -----%>
                                </td>
                                <td align="right">
                                </td>
                                <td align="left">
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton" Text="Find now"
                                        ToolTip="Click to Find as per search criteria"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnPrevious" TabIndex="0" runat="server" CssClass="clsButton" Text="<<<<"
                                        ToolTip="Click to shift previous Date" ForeColor="Purple" Font-Bold="True"></asp:Button>
                                </td>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mServiciability.CurrentDateWithDay %>"
                                                    Style="text-align: center;" ForeColor="#0000C0" Font-Bold="True" ReadOnly="True"
                                                    BackColor="#E0E0E0" MaxLength="12">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsHoliday" runat="server" Checked='<%# mServiciability.IsHoliday  %>'
                                                    onclick="Enable();" ClientIDMode="Static" CssClass="clsCheckBox" Text="Holiday">
                                                </asp:CheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:Button ID="btnNext" TabIndex="0" runat="server" CssClass="clsButton" Text=">>>>"
                                        ToolTip="Click to shift next Date" ForeColor="Purple"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:DataGrid ID="dgServiciabilityDetailList" runat="server" CssClass="clsGrid" AutoGenerateColumns="False">
                                        <AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
                                        <ItemStyle CssClass="clsdgItem"></ItemStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="Id"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Model" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblModel" runat="server" class="clsLabel" Style="display: none;" Font-Italic="true"
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"Model") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Select">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelect") %>'>
                                                    </asp:CheckBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="DateNo" HeaderText="Sr. No."></asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtErrorMark" runat="server" Font-Bold="True" ForeColor="Red" MaxLength="8"
                                                        ReadOnly="True" Width="11px" BorderStyle="None"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="RegNo" HeaderText="Reg No.">
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="FlyingHours" HeaderText="Hrs."></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Serviceable">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chkServiceability" onclick="CheckDayPercent(this);" runat="server"
                                                        GroupName="A" name="SerGrp" Checked='<%# DataBinder.Eval(Container.DataItem,"S_Status") %>'>
                                                    </asp:RadioButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Schedule">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chkSchedule" runat="server" GroupName="A" name="SerGrp" Checked='<%# DataBinder.Eval(Container.DataItem,"SM_Status") %>'
                                                        onclick="CheckDayPercent(this);"></asp:RadioButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Un Schedule">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="chkUnSchedule" onclick="CheckDayPercent(this);" runat="server"
                                                        GroupName="A" name="SerGrp" Checked='<%# DataBinder.Eval(Container.DataItem,"USM_Status") %>'>
                                                    </asp:RadioButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                              <%--   Added by Shital on 16-Dec-2021 for TSL15122021--%>
                                            <asp:TemplateColumn HeaderText="UnSchedule Catagory">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="cmbUnscheduleCatagory" runat="server" CssClass="clsComboBoxSmall" ClientIDMode="Static"
                                                        DataValueField="ID" DataTextField="Name" DataSource="<%# munscheduleCatagoryList %>" Enabled="false"
                                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem,"UnscheduleCatagoryID") %>'>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <%-- ---- --%>
                                            <asp:TemplateColumn HeaderText="Day %">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDayPercent" runat="server" Height="23px" Width="100px" CssClass="clsTextBox_Ajax"
                                                        ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem,"DayPercent") %>'
                                                        MaxLength="10">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Priority">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuplicatePriority" runat="server" ForeColor="Red" class="clsLabel"
                                                        Style="display: none;" Font-Italic="true" Text="* Duplicate"></asp:Label>
                                                    <asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsComboBoxSmall" DataValueField="ID"
                                                        onchange="CheckDuplicatePriority();" DataTextField="Name" DataSource="<%# mServiceabilityPriorityList %>"
                                                        SelectedValue='<%# DataBinder.Eval(Container.DataItem,"PriorityID") %>'>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Remark">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBox1" Text='<%# DataBinder.Eval(Container.DataItem,"Remark") %>'
                                                        MaxLength="500">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="left">
                                    <table id="Table4" class="clsTablelistin" border="0" cellspacing="1" cellpadding="1"
                                        width="100%">
                                        <tr>
                                            <td align="left">
                                                <table id="Table3" class="clstableButton" border="0" cellspacing="1" cellpadding="1"
                                                    width="300">
                                                    <tr>
                                                        <td colspan="8" align="left">
                                                            <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeaderItem" Width="304px">Print Aircrafts:</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn1" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 01-10</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn2" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 11-20</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn3" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 21-30</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn4" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 31-40</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn5" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 41-50</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn6" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 51-60</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn7" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 61-70</asp:LinkButton>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkbtn8" runat="server" CssClass="clsLabelAuto" Width="40px"
                                                                Visible="False"> 71-80</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" align="right">
                                                <table id="Table5" class="clstableButton" border="0" cellspacing="1" cellpadding="1">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btnSendMail" TabIndex="0" runat="server" CssClass="clsButton" Text="Send mail"
                                                                Visible='<%# AppSettings("ClientCode") = "APFT" Or
                                                                            AppSettings("ClientCode") = "AAP" %>' ToolTip="Click to Send Serviceability report by mail">
                                                            </asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnPrint" TabIndex="0" runat="server" CssClass="clsButton" Text="Print"
                                                                ToolTip="Click to print Serviceability report for the selected month"></asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSave" TabIndex="0" runat="server" CssClass="clsButton" Text="Save"
                                                                ToolTip="Click to save Serviceability Entry"></asp:Button>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnBack" TabIndex="0" runat="server" CssClass="clsButton" Text="Close"
                                                                ToolTip="Click to go back to the previous page"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr style="height: 0px;">
            <td style="height: 0px;">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                            CausesValidation="False" Style="display: none;"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
    <script type="text/javascript">
        function CheckDuplicatePriority(sender, args) {
            var grid = document.getElementById("<%=dgServiciabilityDetailList.ClientID %>");
            var inputs = $('#<%=dgServiciabilityDetailList.ClientID %>').find('select[id$="cmbPriority"]');
            var span = $('#<%=dgServiciabilityDetailList.ClientID %>').find('span[id$="lblDuplicatePriority"]');
            var Model = $('#<%=dgServiciabilityDetailList.ClientID %>').find('span[id$="lblModel"]');
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].style.backgroundColor = "";
                span[i].style.display = 'none';
            }
            for (var i = 0; i < inputs.length; i++) {
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[i] != inputs[j] && (inputs[i].value != "10" && inputs[j].value != "10") && inputs[i].value == inputs[j].value && (Model[i].innerText == Model[j].innerText)) {
                        inputs[i].style.backgroundColor = "Orchid";
                        inputs[j].style.backgroundColor = "Orchid";
                        span[i].style.display = 'block';
                        span[j].style.display = 'block';

                    }
                }
            }
        }
        function SetPercentage() {

        }
        function Enable() {
            var IsHoliday = $get("chkIsHoliday").checked;
            if (IsHoliday) {
                $("[id$='cmbPriority']").attr('disabled', true);
                $("[id$='txtDayPercent']").attr('disabled', true);
                $("[id$='txtDayPercent']").val('0');
                $("[id$='cmbPriority']").val('10');

                var grid = document.getElementById("dgServiciabilityDetailList");
                var inputs = $('#dgServiciabilityDetailList').find('select[id$="cmbPriority"]');
                var span = $('#dgServiciabilityDetailList').find('span[id$="lblDuplicatePriority"]');
                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].style.backgroundColor = "";
                    span[i].style.display = 'none';
                }
            }
            else {
                $("[id$='cmbPriority']").attr('disabled', false);
                $("[id$='txtDayPercent']").attr('disabled', false);
                $("[id$='txtDayPercent']").val('100');
                $("[id$='cmbPriority']").val('10');
            }
        }
    </script>
    <script type="text/javascript">
        function CheckDayPercent(myradio) {
            var td = $("td", $(myradio).closest("tr"));
            var inputs = document.getElementById('dgServiciabilityDetailList$' + myradio.id.split("_")[1] + '$txtDayPercent');
            if (myradio.value == "chkServiceability") {
                $("#txtDayPercent", td).val('100.00');
                $("#cmbUnscheduleCatagory", td).attr('disabled', true);
                $("#cmbUnscheduleCatagory", td).val(0);
            }
            else if (myradio.value == "chkSchedule") {
                $("#txtDayPercent", td).val('0');
                $("#cmbUnscheduleCatagory", td).attr('disabled', true);
                $("#cmbUnscheduleCatagory", td).val(0);
                }
            else if (myradio.value == "chkUnSchedule") {
                $("#txtDayPercent", td).val('0');
                $("#cmbUnscheduleCatagory", td).attr('disabled', false); 
            }
            // alert(myradio.value);

        }
    </script>
    <!-- Send Mail Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySendMail" Text="Dummy Send Mail" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlPopupSendMail" HorizontalAlign="Center" Style="height: 100%;
        width: 100%; vertical-align: Center;">
        <iframe id="iPopupSendMail" frameborder="0" allowtransparency="true" height="100%"
            width="100%" src="JavaScript:''" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSendMail" runat="server" TargetControlID="btnDummySendMail"
        PopupControlID="pnlPopupSendMail" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSendMailStateComplete() {
            $("#btnDummySendMail").click();
            $get("AjaxLoader").style.visibility = "hidden";
        }
        function OpenByMaiWindow() {

            try {
                $get("AjaxLoader").style.visibility = "visible";
                $("#iPopupSendMail").attr("src", "wfServiceabilityMailSend_Ajax.aspx?Type=pup");
                if (!$.browser.msie) {
                    $("#btnDummySendMail").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
            
       
    </script>
    <script type="text/javascript">
        function ParentCallBackFunctionForSendMail() {
            var SendMailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
            //close Send Mail popup window
            SendMailWindow.hide();
            $("#iPopupSendMail").attr("src", "JavaScript:''");
            //call Send Mail image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!-- End-->
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var IsHoliday = $get("chkIsHoliday").checked;
            if (IsHoliday) {
                $("[id$='cmbPriority']").attr('disabled', true);
                $("[id$='txtDayPercent']").attr('disabled', true);

            }
            else {
                $("[id$='cmbPriority']").attr('disabled', false);
                $("[id$='txtDayPercent']").attr('disabled', false);

            }
        });

        
    </script>
</body>
</html>
