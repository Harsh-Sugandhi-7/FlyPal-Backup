<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUpdateWOJobTaskDateTime_Ajax.aspx.vb"
    Inherits="Flypal.wfUpdateWOJobTaskDateTime_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Finding Rectification Details</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
      
    </script>
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
    <table class="clstablelistout" id="tblmain">
        <tr>
            <td>
                <table class="clsTablelistin" id="tblinner">
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">WO Job and Task Plan Date Time Update</asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlValidation" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="Valsummary3" CssClass="clsValidationSummary" runat="server"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlAuditDet" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset id="fdswodetail" class="clsFieldSet" style="border-width: 1px">
                                        <legend id="ldwodetail" runat="server"><b>WO Detail</b></legend>
                                        <table class="clsTablelistin" id="Table2" cellspacing="1" cellpadding="1" border="0">
                                            <tr>
                                                <td>
                                                    <span class="clsLabelHeader">WO No.: </span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWONo" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WONumber %>"> </asp:Label>
                                                </td>
                                                <td style="width: 10px;">
                                                </td>
                                                <td>
                                                    <span class="clsLabelHeader">WO Date: </span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWODate" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WODateFormatted %>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="clsLabelHeader">WO Description: </span>
                                                </td>
                                                <td colspan="4">
                                                    <asp:Label ID="lblWODesc" runat="server" CssClass="clsLabelAuto" Text="<%# mnWO.WORemark %>"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="dgWOJobsWithTaskCard" runat="server" AutoGenerateColumns="False"
                                        GridLines="None" CssClass="clsGrid" PageSize="3" Width="100%" ShowHeaderWhenEmpty="true">
                                        <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                        <RowStyle CssClass="clsdgItem"></RowStyle>
                                        <HeaderStyle CssClass="clsdgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <div>
                                                        <a href="javascript:showNestedGridView('ID-<%# Eval("ID") %>');">
                                                            <img id="imageID-<%# Eval("ID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif" />
                                                        </a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
                                                <HeaderStyle Width="10px" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WOJobDescription" HeaderText="Job Description">
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Plan Start Date & Time" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%--<asp:UpdatePanel ID="upnlJobPlanStartDateValidate" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CustomValidator ID="cvJobPlanStartDate" runat="server" ControlToValidate="txtJobPlanStartDate"
                                                                CssClass="clsLabelAuto" Display="dynamic" Font-Italic="true" ForeColor="Red"
                                                                InitialValue="-1" SetFocusOnError="true" Text="* Plan Start Date" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                    <asp:UpdatePanel ID="upnlValidationSummary3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ValidationSummary ID="Validationsummary3" runat="server" CssClass="clsValidationSummary"
                                                                DisplayMode="List" ValidationGroup='<%# string.Format("Group2_{0}", Eval("ID")) %>'
                                                                HeaderText=""></asp:ValidationSummary>
                                                            <asp:CustomValidator ID="cvJobStartDate" runat="server" ControlToValidate="txtJobPlanStartDate"
                                                                Display="None" ValidationGroup='<%# string.Format("Group2_{0}", Eval("ID")) %>'
                                                                ErrorMessage=""></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:TextBox ID="txtJobPlanStartDate" CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this,'txtJobPlanStartDate_CalendarExtender')"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtJobPlanStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$ AppSettings:DateTimeFormatLOG %>" TargetControlID="txtJobPlanStartDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtJobPlanStartDate" ID="txtJobPlanStartDate_watermarkextender"
                                                        WatermarkCssClass="clsTextBoxDate_Ajax" runat="server" WatermarkText="<%$ AppSettings:DateTimeFormatLOG %>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Plan End Date & Time" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%--<asp:UpdatePanel ID="upnlJobPlanEndDateValidate" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:CustomValidator ID="cvJobPlanEndDate" runat="server" ControlToValidate="txtJobPlanEndDate"
                                                                CssClass="clsLabel" Display="dynamic" Font-Italic="true" ForeColor="Red" InitialValue="-1"
                                                                SetFocusOnError="true" Text="* Plan End Date" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                                    <asp:UpdatePanel ID="upnlValidationSummary4" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ValidationSummary ID="Validationsummary4" runat="server" CssClass="clsValidationSummary"
                                                                DisplayMode="List" ValidationGroup='<%# string.Format("Group3_{0}", Eval("ID")) %>'
                                                                HeaderText=""></asp:ValidationSummary>
                                                            <asp:CustomValidator ID="cvJobEndDate" runat="server" ControlToValidate="txtJobPlanEndDate"
                                                                Display="None" ValidationGroup='<%# string.Format("Group3_{0}", Eval("ID")) %>'
                                                                ErrorMessage=""></asp:CustomValidator>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:TextBox ID="txtJobPlanEndDate" CssClass="clsTextBoxDate_Ajax" onchange="ValidateDateText(this,'txtJobPlanEndDate_CalendarExtender')"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="txtJobPlanEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$ AppSettings:DateTimeFormatLOG %>" TargetControlID="txtJobPlanEndDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtJobPlanEndDate" ID="txtJobPlanEndDate_watermarkextender"
                                                        WatermarkCssClass="clsTextBoxDate_Ajax" runat="server" WatermarkText="<%$ AppSettings:DateTimeFormatLOG %>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Persons" HeaderStyle-HorizontalAlign="right"
                                                ItemStyle-HorizontalAlign="right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtJobNoOfPersons" MaxLength="4" CssClass="clsTextBoxRightAlignQty_Ajax"
                                                        OnTextChanged="AddAttributesForGridControls" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td colspan="95%" bgcolor="White" width="0px">
                                                            <div id="ID-<%# Eval("ID") %>" style="display: none; position: absolute; left: 17px">
                                                                <panel>
                                                            
                                                            
                                                                <table>
                                                                    <tr>
                                                                        <asp:Label ID="lblTaskCards" runat="server" CssClass="clsLabelHeaderItem">Task Card(s) </asp:Label>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="100%" bgcolor="White" width="0px">
                                                                            <asp:GridView ID="grdTaskCards" runat="server" AutoGenerateColumns="False" Width="95%"   DataKeyNames="ID,WOJobID"
                                                                                GridLines="None" BorderStyle="Groove" CellPadding="0" ForeColor="#333333" CssClass="clsGrid"
                                                                                AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="true" HeaderStyle-Wrap="true"
                                                                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <Columns>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <asp:BoundField Visible="False" DataField="WOJobID" HeaderText="WOJobID"></asp:BoundField>
                                                                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Left">
                                                                                        <HeaderStyle Width="10px" Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="TaskCardNo" HeaderText="Task Card No." HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>
                                                                                     <asp:BoundField DataField="TaskDescription" HeaderText="Task Description" HeaderStyle-HorizontalAlign="Left">
                                                                                    </asp:BoundField>
                                                                                    <asp:TemplateField HeaderText="Task Start Date & Time" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                          
                                                                                            <asp:UpdatePanel ID="upnlValidationSummary1" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary" DisplayMode="List"
                                                                                                        ValidationGroup='<%# string.Format("Group1_{0}", Eval("ID")) %>' HeaderText=""></asp:ValidationSummary>
                                                                                                    <asp:CustomValidator ID="cvTaskStartDate" runat="server" ControlToValidate="txtTaskPlanStartDate" Display="None"
                                                                                                        ValidationGroup='<%# string.Format("Group1_{0}", Eval("ID")) %>' ErrorMessage=""></asp:CustomValidator>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:TextBox ID="txtTaskPlanStartDate" CssClass="clsTextBoxDate_Ajax"  onchange="ValidateDateText(this,'txtTaskPlanStartDate_CalendarExtender')"
                                                                                                runat="server"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtTaskPlanStartDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="True" Format="<%$AppSettings:DateTimeFormatLOG %>" TargetControlID="txtTaskPlanStartDate">
                                                                                            </cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtTaskPlanStartDate" ID="txtTaskPlanStartDate_watermarkextender" WatermarkCssClass="clsTextBoxDate_Ajax"
                                                                                                runat="server" WatermarkText="<%$AppSettings:DateTimeFormatLOG %>">
                                                                                            </cc2:TextBoxWatermarkExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Task End Date & Time" HeaderStyle-HorizontalAlign="Left"
                                                                                        ItemStyle-HorizontalAlign="Left">
                                                                                        <ItemTemplate>
                                                                                            <asp:UpdatePanel ID="upnlValidationSummary2" runat="server" UpdateMode="Conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary" DisplayMode="List"
                                                                                                        ValidationGroup='<%# string.Format("Group2_{0}", Eval("ID")) %>' HeaderText=""></asp:ValidationSummary>
                                                                                                    <asp:CustomValidator ID="cvTaskEndDate" runat="server" ControlToValidate="txtTaskPlanEndDate" Display="None"
                                                                                                        ValidationGroup='<%# string.Format("Group2_{0}", Eval("ID")) %>' ErrorMessage=""></asp:CustomValidator>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:TextBox ID="txtTaskPlanEndDate" CssClass="clsTextBoxDate_Ajax"  onchange="ValidateDateText(this,'txtTaskPlanEndDate_CalendarExtender')"
                                                                                                runat="server"></asp:TextBox>
                                                                                            <cc2:CalendarExtender ID="txtTaskPlanEndDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                Enabled="True" Format="<%$AppSettings:DateTimeFormatLOG %>" TargetControlID="txtTaskPlanEndDate">
                                                                                            </cc2:CalendarExtender>
                                                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtTaskPlanEndDate" ID="txtTaskPlanEndDate_watermarkextender" WatermarkCssClass="clsTextBoxDate_Ajax"
                                                                                                runat="server" WatermarkText="<%$AppSettings:DateTimeFormatLOG %>">
                                                                                            </cc2:TextBoxWatermarkExtender>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="No of Persons" HeaderStyle-HorizontalAlign="right"
                                                                                        ItemStyle-HorizontalAlign="right">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtTaskNoOfPersons" MaxLength="4" CssClass="clsTextBoxRightAlignQty_Ajax" OnTextChanged="AddAttributesForGridControls"
                                                                                                runat="server"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle BackColor="ControlDark" />
                                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" cellspacing="1" cellpadding="1" border="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save"
                                                    Text="Save"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Close Screen"
                                                    Text="Close"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div clawftaskss="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <%--call parent function after completing subroutine..(when page open as popup)--%>
    <script type="text/javascript">
        function CallParentCallback() {
            parent.ParentCallBackFunctionForUpdate();
            return false;
        }
    </script>
    <%--End--%>
    <%--Set page layout when open as popup aspx page--%>
    <script type="text/javascript">
        <% Dim mopen As String = Request.QueryString("Type") %>
        <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

        $(document).ready(function () {
        SetPageLayout();
            if ($.browser.msie) {
                parent.IFrameUpdateStateComplete();
            }
       
      
    });
        <% End if %>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
        function endRequestHandler() {
            SetPageLayout();
        }

        function SetPageLayout()
        {
        <% Dim mopenas As String = Request.QueryString("Type") %>
            <% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
            ReSetPageLayout();
            onResize();//for Top bottom link
            <% End if %>
        }
        function ReSetPageLayout()
        {
        $("body,html").css({ 'background-color': 'transparent' });
            var tempMargtop=$("body #tblmain:eq(0)").outerHeight();
            var windowheight=$(window).height();
            if (tempMargtop>=windowheight)
            {
            $("body #tblmain:eq(0)").css({ 'margin': 'auto'});
            }
            else
            {
            var margintop=(windowheight/2)-(tempMargtop/2);
            $("body #tblmain:eq(0)").css({ 'margin': 'auto' ,'margin-top':margintop +'px'});
            }
       
        }
    </script>
    <%--End--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': 'true' };
            $.ajax({
                type: "POST",
                url: "DateTimeValidationHandler.ashx",
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
