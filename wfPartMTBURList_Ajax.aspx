<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPartMTBURList_Ajax.aspx.vb"
    Inherits="Flypal.wfPartMTBURList_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Update Part MTBUR Values</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmChangeLocation" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2">
                                    <span class="clstitle1">Update Part MTBUR Values</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlValidationsummary" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary1" HeaderText="Fill Up The Following Fields"
                                                CssClass="clsValidationSummary" runat="server"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table1">
                                                <tr>
                                                    <td>
                                                        <span id="lblPartNo" class="clsLabelAuto">Part Name</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBox_Ajax" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <span id="lblCategory" class="clsLabel">Model</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbModelList" runat="server" CssClass="clsComboBox_Ajax" DataValueField="ID"
                                                            ClientIDMode="Static" DataTextField="ModelName">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="Span1" class="clsLabel">ATA</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsComboBox2_Ajax"
                                                            DataValueField="ID" DataTextField="ATAChapter">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right">
                                    <div>
                                        <asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
                                            Text="Find Now" ToolTip="Click to find as per criteria"></asp:Button>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAddClose">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnUpdateTop" runat="server" CssClass="clsButton_Ajax" Text="Update"
                                                            CausesValidation="true" ToolTip="Click to Update Part MTBUR Details"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExportToExcelTop" runat="server" CssClass="clsButtonLong_Ajax"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            Text="Export To Excel" CausesValidation="false" ToolTip="Click to Export Part Details alongwith their MTBUR values">
                                                        </asp:Button>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close" CausesValidation="false" ToolTip="Click to close Part List screen"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnExportToExcel" EventName="click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 100%">
                                                <div>
                                                    <asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
                                                </div>
                                            </div>
                                            <div style="width: 100%">
                                                <asp:GridView ID="dgPartList" runat="server" PageSize="100" AutoGenerateColumns="False"
                                                    DataKeyNames="ID" CssClass="clsGrid" ShowHeaderWhenEmpty="True" AllowPaging="True"
                                                    AllowSorting="True">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                    <RowStyle CssClass="clsdgItem" />
                                                    <HeaderStyle CssClass="clsdgHeader" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'>
                                                                </asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PartName" HeaderText="Part">
                                                            <HeaderStyle Wrap="False" HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PartDescription" HeaderText="Description">
                                                            <HeaderStyle ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Self MTBUR">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSelfMTBUR" CssClass="clsTextBoxRightAlignSmall_Ajax" runat="server"
                                                                    ClientIDMode="Static" MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem,"SelfMTBUR") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="90px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="World MTBUR">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWorldMTBUR" CssClass="clsTextBoxRightAlignSmall_Ajax" runat="server"
                                                                    ClientIDMode="Static" MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem,"WorldMTBUR") %>'
                                                                    onkeypress="return onlyNumbers(this);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="90px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Update Date">
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel ID="upnlUpdateDateValidate" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:CustomValidator ID="cvUpdateDate" runat="server" ControlToValidate="txtLastUpdateDate"
                                                                            CssClass="clsLabel" Display="dynamic" Font-Italic="true" ForeColor="Red" InitialValue="-1"
                                                                            SetFocusOnError="true" Text="* Last Update" ValidationGroup='<%# string.Format("Group_{0}", Eval("ID")) %>'></asp:CustomValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <asp:TextBox ID="txtLastUpdateDate" runat="server" CssClass="clsTextBox_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"UpdateDateFormatted") %>'
                                                                    onchange="ValidateDateText(this,'LastUpdateDate_watermarkextender');" Width="90px"></asp:TextBox>
                                                                <cc2:CalendarExtender ID="txtLastUpdateDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                    Enabled="True" Format="<%$ AppSettings:DateFormat %>" TargetControlID="txtLastUpdateDate">
                                                                </cc2:CalendarExtender>
                                                                <cc2:TextBoxWatermarkExtender ID="LastUpdateDate_watermarkextender" runat="server"
                                                                    Enabled="True" TargetControlID="txtLastUpdateDate" WatermarkCssClass="clsDateTextBox"
                                                                    WatermarkText="<%$ AppSettings:DateFormat %>">
                                                                </cc2:TextBoxWatermarkExtender>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="130px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="130px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <asp:Panel ID="PnlPaging" runat="server">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <table border="0" cellpadding="2" cellspacing="1" align="right">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label Text="" EnableViewState="false" runat="server" ClientIDMode="Static" ID="valuetodisplay"
                                                                                class="letterbox" />
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnfirstpage" class="first" onclick="setValue(0);" title="Move First">
                                                                            </span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnprevpage" onclick="setValue(1);" class="prev" title="Move Previous">
                                                                            </span>
                                                                        </td>
                                                                        <td align="center">
                                                                            <div align="center">
                                                                                <asp:TextBox runat="server" Text="" ID="Slidercontrol">
                                                                                </asp:TextBox>
                                                                                <cc2:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="Slidercontrol"
                                                                                    Minimum="-100" Maximum="100" BoundControlID="txtPageDisplay" EnableHandleAnimation="true"
                                                                                    Length="300" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnnextvpage" onclick="setValue(2);" class="next" title="Move Next"></span>
                                                                        </td>
                                                                        <td>
                                                                            <span id="btnlastpage" onclick="setValue(3);" class="last" title="Move Last"></span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtPageDisplay" ToolTip="Enter page no." CssClass="clsTextBoxMegaSmall_Ajax" />
                                                                        </td>
                                                                        <td>
                                                                            <span>of </span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label Text="" ID="lblpagecount" CssClass="clsLabelHeader" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <div>
                                                                                <asp:Button ID="btnGridPaging" CssClass="clsButtonPlus_Ajax" runat="server" Text="Go" />
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <table class="clstableButton" align="right">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnUpdate" runat="server" CssClass="clsButton_Ajax" Text="Update"
                                                    ToolTip="Click to Update Part MTBUR Details"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnExportToExcel" runat="server" CssClass="clsButtonLong_Ajax" Text="Export To Excel"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                    CausesValidation="false" ToolTip="Click to Export Part Details alongwith their MTBUR values">
                                                </asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="false" CssClass="clsButton_Ajax"
                                                    Text="Close" ToolTip="Click to close Part List screen"></asp:Button>
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
    </div>
    </form>
    <!-- Slider control events  -->
    <script type="text/javascript">
        //initialize slider control and attach events
        function pageLoad(sender, e) {
            var slider = $find('<%=SliderExtender1.ClientID %>');
            if (slider) {
                slider.add_slideStart(sliderStart);
                slider.add_slideEnd(sliderEnd);
                slider.add_valueChanged(valChanged);
            }
        }

            
    </script>
    <script type="text/javascript">
        function valChanged() {
            var showval = $('#valuetodisplay');
            var curval = $('#<%=Slidercontrol.ClientID %>');
            showval.html(curval.val());
        }
       
        
    </script>
    <script type="text/javascript">

        function sliderStart() {
            $('#valuetodisplay').css('display', 'inline-block');
        }
    </script>
    <script type="text/javascript">
        function sliderEnd() {
            $('#valuetodisplay').css('display', 'none');

        }
    </script>
    <script type="text/javascript">
        function setValue(val) {
            if (val === 0) {//first
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var minval = slider.get_Minimum();
                $('#<%=txtPageDisplay.ClientID %>').val(minval);
                $('#<%=Slidercontrol.ClientID %>').val(minval);
                slider.set_Value(minval);


            }
            else if (val === 1) {//prev
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval - 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);


            }
            else if (val === 2) {//next
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                curval = curval + 1;
                $('#<%=txtPageDisplay.ClientID %>').val(curval);
                $('#<%=Slidercontrol.ClientID %>').val(curval);
                var slider = $find('<%=SliderExtender1.ClientID %>');
                slider.set_Value(curval);
                //                            sliderStart();
                //                            valChanged();
                //                            sliderEnd();

            }
            else if (val === 3) {//last
                var curval = parseInt($('#<%=txtPageDisplay.ClientID %>').val());
                var slider = $find('<%=SliderExtender1.ClientID %>');
                var maxval = slider.get_Maximum();
                $('#<%=txtPageDisplay.ClientID %>').val(maxval);
                $('#<%=Slidercontrol.ClientID %>').val(maxval);
                slider.set_Value(maxval);
            }
        }
    </script>
    <!-- End  -->
    <script type="text/javascript">
        function onlyNumbers(evt) {
            var e = event || evt; // for trans-browser compatibility
            var charCode = e.which || e.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#chkSelectAll").live("click", function () {
                var status = $("#chkSelectAll").attr("checked");
                $("#dgPartList tr:gt(0)").find(":checkbox").each(function () {
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
                $(elem).closest("tr").addClass('HighLightRow');
            }
            else {
                $(elem).closest("tr").removeClass('HighLightRow');
            }
        }

        function pageLoad() {
            var status;
            $("#dgPartList tr:gt(0)").find(":checkbox").each(function () {
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
