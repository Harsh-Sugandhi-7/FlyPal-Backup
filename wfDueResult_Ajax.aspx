<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDueResult_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfDueResult_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Due Jobs</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style>
        .clsCollapsePnl {
            background: url("css/img/BGLink.png") repeat-x #ccc;
            font-family: Verdana; /*font-size: 14pt; */
            font-size: 12pt;
            color: White;
            font-weight: 500;
            width: 100%;
            display: inline-block;
            border: 1px solid gray;
        }

        .clsExpandiblePnl {
            overflow: hidden;
            height: 0px;
            border: 1px solid #ccc;
        }
    </style>
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
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <style type="text/css">
            .GbiHighlight {
                background-color: Aqua;
            }
        </style>
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
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                        <table class="clsTablelistin" id="tblinner">
                            <tr>
                                <td class="clsFormHeader1Newstyle">


                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <span id="lblTitle1" class="clsFormHeader">Due Job(s)</span>
                                            </td>

                                            <td align="right">
                                                <asp:UpdatePanel ID="upnlCloseBottom" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server" Text="Print" Visible='<%# iif(AppSettings("ClientCode") = "Heligo" ,True,False) %>' />
                                                        &nbsp;&nbsp;
                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader" style="background-color: yellow" visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Panel ID="pnlTargetDepartment" runat="server">
                                                            <div style="vertical-align: middle;" class="clsCollapsePnl">
                                                                <div style="float: left;">
                                                                    <span id="lblDepartmentRecCount" class="clsLabelHeader">Create Work Order</span>
                                                                </div>
                                                                <div style="float: right;">
                                                                    <span id="lblMessageDepartment" class="clsLabelHeader"></span>
                                                                    <asp:Image ID="imgArrowsDepartment" Style="vertical-align: middle;" runat="server" />
                                                                </div>
                                                                <div style="clear: both">
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Panel ID="pnlExpandDepartment" runat="server" CssClass="clsExpandiblePnl">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">Work Order Date</asp:Label>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" runat="server" ClientIDMode="Static"
                                                                            onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                                        <cc2:TextBoxWatermarkExtender ClientIDMode="Static" TargetControlID="txtFromDate"
                                                                            ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWOTime" runat="server" AutoPostBack="True"
                                                                            Visible='<%#IIf(AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND", True, False) %>'
                                                                            MaxLength="10" ToolTip="Enter Time" Width="65px"></asp:TextBox>
                                                                        <asp:Label ID="lblUTC" runat="server" Visible='<%# iif(AppSettings("ClientCode") = "STR" or AppSettings("ClientCode") = "IND",True,False) %>'
                                                                            CssClass="clsLabelHeader">(UTC
                                                                    Time)</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">For Aircraft</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" DataTextField="RegNo"
                                                                            AutoPostBack="true" DataValueField="ID">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="upnlCreateWO" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:LinkButton ID="lnkbtnCreateWorkOrder" CssClass="clsLinkButton" runat="server"
                                                                                    Text="Create Work Order"></asp:LinkButton>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <cc2:CollapsiblePanelExtender ID="cpeDepartment" runat="Server" TargetControlID="pnlExpandDepartment"
                                                                            Collapsed="false" ExpandControlID="pnlTargetDepartment" CollapseControlID="pnlTargetDepartment"
                                                                            AutoCollapse="False" AutoExpand="False" ScrollContents="false" TextLabelID="lblMessageDepartment"
                                                                            CollapsedText="Show Details..." ExpandedText="Hide Details" ImageControlID="imgArrowsDepartment"
                                                                            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
                                                                            ExpandDirection="Vertical" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDuePeriodList" runat="server" CssClass="clsLabelHeader">Due Job(s) List</asp:Label>
                                                    </td>
                                                    <td>
                                                        <img alt="" src="icons/asterisk.ico" style="height: 15px; width: 15px" />
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader"> Linked Maintenance Activity</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:UpdatePanel ID="upnlCloseTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnPrintTop" runat="server" CssClass="clsButton_Ajax" Text="Print"
                                                                    Visible='<%# iif(AppSettings("ClientCode") = "Heligo" ,True,False) %>' />
                                                                &nbsp;&nbsp;
                                                            <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClseTop" runat="server" Text="Close" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="dgDueJob" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
                                                            DataKeyNames="StatusID" ShowHeaderWhenEmpty="true" ToolTip="Due Job.">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" />
                                                            <RowStyle CssClass="clsdgItem" />
                                                            <HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <input type="checkbox" id="chkSelectAll" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <input type="checkbox" name="chkSelect" class="cbSelectRow" value="<%# Eval("StatusID") %>"
                                                                            <%# NumeroChequeInclus(Eval("StatusID").ToString()) %> onclick="EnableDisable(this);"></input>
                                                                        <input type="checkbox" id="chkMaintenanceTypeID" name="chkMaintenanceTypeList" class="cbSelectRow" value="<%# Eval("MaintenanceTypeID") %>"
                                                                            style="display: none;"></input>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                                <asp:BoundField DataField="MaintenanceOn" HeaderText="Maintenance On" HtmlEncode="false"
                                                                    SortExpression="MaintenanceOn">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaintenanceInformation" HeaderText="Maintenance Information"
                                                                    HtmlEncode="false" SortExpression="MaintenanceInformation">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Things to do" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Frequency" HeaderText="Frequency" HtmlEncode="false" SortExpression="Frequency">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SinceNewAll" HeaderText="Since New" HtmlEncode="false"
                                                                    SortExpression="SinceNewAll">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ElapsedAll" HeaderText="Elapsed" HtmlEncode="false" SortExpression="ElapsedAll">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoneAtAll" HeaderText="Done At" HtmlEncode="false" SortExpression="DoneAtAll">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExtensionAll" HeaderText="Extension" HtmlEncode="false"
                                                                    SortExpression="ExtensionAll">
                                                                    <HeaderStyle ForeColor="White" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DueAsofAll" HeaderText="Due At" HtmlEncode="false" SortExpression="DueAsofAll">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AssDueAsofAll" HeaderText="Due At Assembly" HtmlEncode="false"
                                                                    SortExpression="AssDueAsofAll">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RemainingTimeAll" HeaderText="Remaining" HtmlEncode="false"
                                                                    SortExpression="MinimumRemainingValue">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="EstimatedDate" HeaderText="Estimated Date" HtmlEncode="false"
                                                                    SortExpression="EstDate">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                                <%--  <asp:BoundField DataField="WONumber" HeaderText="WO Number" HtmlEncode="false">
                                                                <HeaderStyle  HorizontalAlign="Left"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                            </asp:BoundField>--%>
                                                                <asp:TemplateField HeaderText="WO Number">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkbtnWONumber" runat="server" CommandName="WONumberRec" CommandArgument='<%# Eval("StatusID") %>' Text='<%# Eval("WONumber") %>'
                                                                            CausesValidation="false"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                                                </asp:TemplateField>

                                                                <asp:ButtonField CommandName="Comply" HeaderText="Comply" Text="Comply">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--16--%>
                                                                <asp:ButtonField CommandName="History" HeaderText="History" Text="History">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:ButtonField>
                                                                <%--17--%>
                                                                <asp:TemplateField HeaderText="View Required Spare/Tool/Task Card List" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnImageButton" runat="server" CommandArgument='<%# Eval("StatusMasterID") %>'
                                                                            ToolTip="Click to view spare list" CommandName="ViewSpareList" Style="height: 30px; width: 30px"
                                                                            ImageUrl="~/icons/iconfinder_-_Eye-Show-View-Watch-See_3844411.ico" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--18--%>
                                                                <asp:BoundField DataField="IsMaster" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsMaster"
                                                                    ItemStyle-CssClass="hideGridColumn" />
                                                                <%--19--%>
                                                                <asp:BoundField DataField="StatusMasterID" HeaderText="Status Master ID" Visible="false">
                                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                                                                </asp:BoundField>
                                                                <%--20--%>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btn" runat="server" Style="height: 15px; width: 15px" ImageUrl="~/icons/asterisk.ico"
                                                                            Visible='<%#  Eval("LinkedMaintenanceActivityCount")>0 %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <%--21--%>
                                                                <asp:BoundField DataField="LinkedMaintenanceActivityCount" HeaderText="LinkedMaintenanceActivityCount"
                                                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaintenanceTypeID" HeaderText="MaintenanceTypeID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--22--%>
                                                                <%-- <asp:TemplateField>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <a href="javascript:showNestedGridView('ID-<%# Eval("StatusMasterID") %>');">
                                                                                  <img id="imageID-<%# Eval("StatusMasterID") %>" alt="Click to show/hide Type" border="0" src="images/detail.gif"  />
                                                                            </a>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <%--23--%>
                                                                <%--<asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td colspan="100%" bgcolor="White" width="0px">
                                                                                <div id="ID-<%# Eval("StatusMasterID") %>" style="display: none; position: relative; left: 25px;">
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
                                                                                                <HeaderStyle ForeColor="White" Wrap="true" Width="330px" HorizontalAlign="Left">
                                                                                                </HeaderStyle>
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
                                                                </asp:TemplateField>--%>
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
                                <%--<td align="right">
                                <asp:UpdatePanel ID="upnlCloseBottom" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnPrint" runat="server"  Text="Print" Visible='<%# iif(AppSettings("ClientCode") = "Heligo" ,True,False) %>' />
                                        &nbsp;&nbsp;
                                        <asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose" runat="server" Text="Close">
                                        </asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            </tr>
                            <!--Dummy panel to open modelpopup for city-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnBtnInspectionHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnDirectiveHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnServiceHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnInspHistory" ClientIDMode="Static" runat="server" Text="Add"
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnCompServiceHistory" ClientIDMode="Static" runat="server" Text="..."
                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                            <asp:Button ID="hdnBtnCompDirectiveHistory" ClientIDMode="Static" runat="server"
                                                Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
        <!--Ass Inspection History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyInspectionHistory" Text="Inspection History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlInspectionHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeInspectionHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupInspectionHistory" runat="server" TargetControlID="btnDummyInspectionHistory"
            PopupControlID="pnlInspectionHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameInspectionHistoryStateComplete() {
                $("#btnDummyInspectionHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenInspectionHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeInspectionHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorInspStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyInspectionHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForInspectionHistory() {
                var InspectionHistorywindow = $find("<%=mdlPopupInspectionHistory.ClientID %>");
                //close Inspection History popup window
                InspectionHistorywindow.hide();
                //           release resources
                $("#IframeInspectionHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnInspectionHistory").click();
            }
        </script>
        <!-- End-->
        <!--Ass Directive History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyDirectiveHistory" Text="Directive History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeDirectiveHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupDirectiveHistory" runat="server" TargetControlID="btnDummyDirectiveHistory"
            PopupControlID="pnlDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameDirectiveHistoryStateComplete() {
                $("#btnDummyDirectiveHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenDirectiveHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeDirectiveHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorModStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyDirectiveHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForDirectiveHistory() {
                var DirectiveHistorywindow = $find("<%=mdlPopupDirectiveHistory.ClientID %>");
                //close Directive History popup window
                DirectiveHistorywindow.hide();
                //           release resources
                $("#IframeDirectiveHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnDirectiveHistory").click();
            }
        </script>
        <!-- End-->
        <!--Ass Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyServiceHistory" Text="Service History" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeServiceHistory" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupServiceHistory" runat="server" TargetControlID="btnDummyServiceHistory"
            PopupControlID="pnlServiceHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameServiceHistoryStateComplete() {
                $("#btnDummyServiceHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeServiceHistory").attr("src", "wfUpdateComplyHistoryAssemblyMonitorServiceStatusList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyServiceHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForServiceHistory() {
                var ServiceHistorywindow = $find("<%=mdlPopupServiceHistory.ClientID %>");
                //close Service History popup window
                ServiceHistorywindow.hide();
                //           release resources
                $("#IframeServiceHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnServiceHistory").click();
            }
        </script>
        <!-- End-->
        <!-- Comp Insp History Popup Window -->
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

            function OpenHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeRemHistory").attr("src", "wfUpdateComplyHistoryCompMonitorInspStatusList_AJAX.aspx?Type=pup");

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
                $("#hdnBtnInspHistory").click();
            }
        </script>
        <!-- End-->
        <!--Comp Directive History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCompDirectiveHistory" Text="Comp Directive History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlCompDirectiveHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeCompDirectiveHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCompDirectiveHistory" runat="server" TargetControlID="btnDummyCompDirectiveHistory"
            PopupControlID="pnlCompDirectiveHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCompDirectiveHistoryStateComplete() {
                $("#btnDummyCompDirectiveHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenCompDirectiveHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeCompDirectiveHistory").attr("src", "wfUpdateComplyHistoryCompMonitorModStatusList_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyCompDirectiveHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForCompDirectiveHistory() {
                var CompDirectiveHistorywindow = $find("<%=mdlPopupCompDirectiveHistory.ClientID %>");
                //close Comp Directive History popup window
                CompDirectiveHistorywindow.hide();
                //           release resources
                $("#IframeCompDirectiveHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnCompDirectiveHistory").click();
            }
        </script>
        <!-- End-->
        <!--Comp Service History Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyCompServiceHistory" Text="Comp Service History"
                ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlCompServiceHistory" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeCompServiceHistory" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupCompServiceHistory" runat="server" TargetControlID="btnDummyCompServiceHistory"
            PopupControlID="pnlCompServiceHistory" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameCompServiceHistoryStateComplete() {
                $("#btnDummyCompServiceHistory").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenCompServiceHistoryWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeCompServiceHistory").attr("src", "wfUpdateComplyHistoryCompMonitorServiceStatusList_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyCompServiceHistory").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForCompServiceHistory() {
                var CompServiceHistorywindow = $find("<%=mdlPopupCompServiceHistory.ClientID %>");
                //close Comp Service History popup window
                CompServiceHistorywindow.hide();
                //           release resources
                $("#IframeCompServiceHistory").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnCompServiceHistory").click();
            }
        </script>
        <!-- End-->
        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': true };
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
        <!--End-->
        <!-- SpareList popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySpareList" Text="Maintenance Activity" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlSpareList" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSpareList" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSpareList" runat="server" TargetControlID="btnDummySpareList"
            PopupControlID="pnlSpareList" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSpareListStateComplete() {
                $("#btnDummySpareList").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSpareListWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSpareList").attr("src", "wfSpareList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummySpareList").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSpareList() {
                var SpareListwindow = $find("<%=mdlPopupSpareList.ClientID %>");
                //close Task Card Tool popup window
                SpareListwindow.hide();
                //           release resources
                $("#IframeSpareList").attr("src", "JavaScript:''");
            }
        </script>
        <!-- End-->
        <%--<!-- MaintenanceKitandTaskList popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyMaintenanceKitandTaskList" Text="Maintenance Activity"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlMaintenanceKitandTaskList" ClientIDMode="Static"
        HorizontalAlign="Center" Style="height: 100%; width: 100%;">
        <iframe id="IframeMaintenanceKitandTaskList" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupMaintenanceKitandTaskList" runat="server" TargetControlID="btnDummyMaintenanceKitandTaskList"
        PopupControlID="pnlMaintenanceKitandTaskList" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameMaintenanceKitandTaskListStateComplete() {
            $("#btnDummyMaintenanceKitandTaskList").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }
        function OpenMaintenanceKitandTaskListWindow() {
            try {
                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeMaintenanceKitandTaskList").attr("src", "wfMaintenanceKitandTaskList_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyMaintenanceKitandTaskList").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }
                return false;
            } catch (e) {
                alert(e);
            }
        }
        function ParentCallBackFunctionForMaintenanceKitandTaskList() {
            var MaintenanceKitandTaskListwindow = $find("<%=mdlPopupMaintenanceKitandTaskList.ClientID %>");
            //close Task Card Tool popup window
            MaintenanceKitandTaskListwindow.hide();
            //           release resources
            $("#IframeMaintenanceKitandTaskList").attr("src", "JavaScript:''");
        }
    </script>
    <!-- End-->--%>
    </form>
    <script type="text/javascript">
        function EnableDisable(control) {
            var grid = $(control).closest("table");
            if (!$(control).is(":checked")) {
                var td = $("td", $(control).closest("tr"));
                $(control).closest("td").find("input[type=checkbox][id*=chkMaintenanceTypeID]").attr("checked", false);
            } else {
                var td = $("td", $(control).closest("tr"));
                var s = $("#chkSelectList", td).val();
                $(control).closest("td").find("input[type=checkbox][id*=chkMaintenanceTypeID]").attr("checked", true);
            }
        }
    </script>
</body>
</html>
